using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Common.Menus.Interface;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Forum.Utils {

    /// <summary>
    /// 修改(CRUD)数据之后，对缓存的处理
    /// 论坛首页：1）加上新注册会员的依赖；2）在线用户列表ajax化
    /// 版块页：1）修改版主、公告；2）置顶、高亮、删除、精华、锁定、分类、移动主题；3）帖子点击数更新
    /// 只看该作者的页面
    /// 
    /// 【功能说明】根据业务逻辑，让特定缓存失效。提供了两种失效方式
    /// 1）直接从缓存中移除缓存项(比如网站首页失效就是直接移除)
    /// 2）记录最后更新时间，这样读取缓存的时候可以检查是否已经失效
    /// </summary>
    public class ForumCacheRemove : CacheRemoveBase {

        private IForumBoardService boardService = null;
        private IForumTopicService topicService = null;

        public ForumCacheRemove( IForumBoardService bds, ControllerBase controller ) {
            this.controller = controller;
            this.boardService = bds;
            this.owner = controller.ctx.owner.obj;
        }

        public ForumCacheRemove( IForumBoardService bds, IForumTopicService topicService, ControllerBase ctx ) {
            this.controller = ctx;
            this.boardService = bds;
            this.owner = ctx.ctx.owner.obj;
            this.topicService = topicService;
        }

        // 创建主题
        public void CreateTopic( ForumBoard bd ) {

            List<String> urls = new List<String>();

            urls.Add( controller.to( new ForumController().Index ) ); // 论坛首页失效
            addBoardUrl( urls, bd, 0 );// 所在版块+父版块失效

            removeCacheList( urls );

            //-------------------------------------------------
            // 修改 board 和 forum 的 timestamp，让版块分页和论坛最新主题等失效
            String boardKey = "forumboard_" + bd.Id;
            cacheHelper.SetTimestamp( boardKey, DateTime.Now );

            String forumKey = "forum_" + controller.ctx.app.Id;
            cacheHelper.SetTimestamp( forumKey, DateTime.Now );

        }

        // 修改主题
        public void UpdateTopic( ForumTopic topic ) {

            List<String> urls = new List<String>();
            urls.Add( controller.to( new ForumController().Index ) ); // 论坛首页失效

            ForumBoard bd = boardService.GetById( topic.ForumBoard.Id, owner );
            int page = getPage( bd, topic ); // 所在版块的分页列表失效
            addBoardUrl( urls, bd, page );

            removeCacheList( urls );

            //-------------------------------------------------
            // 修改 forum 和 topic 的 timestamp，让论坛最新主题、所有主题的回帖列表失效
            String forumKey = "forum_" + controller.ctx.app.Id;
            cacheHelper.SetTimestamp( forumKey, DateTime.Now );

            // 之所以所有回帖分页要全部失效，在于所有分页顶部都有主题的标题，而标题是可以修改的
            String topicKey = "forumtopic_" + topic.Id;
            cacheHelper.SetTimestamp( topicKey, DateTime.Now );
        }

        // 创建回帖
        public void CreatePost( ForumPost post ) {

            ForumTopic topic = topicService.GetById( post.TopicId, owner );

            List<String> urls = new List<String>();
            urls.Add( controller.to( new ForumController().Index ) ); // 论坛首页失效

            ForumBoard bd = boardService.GetById( topic.ForumBoard.Id, owner );
            addBoardUrl( urls, bd, 0 ); // 所在版块失效(帖子被顶到前面)

            removeCacheList( urls );

            //-------------------------------------------------
            // 修改 forum 和 topic 的 timestamp，让论坛最新主题、所有主题的回帖列表失效
            String forumKey = "forum_" + controller.ctx.app.Id;
            cacheHelper.SetTimestamp( forumKey, DateTime.Now );

            String topicKey = "forumtopic_" + topic.Id;
            cacheHelper.SetTimestamp( topicKey, DateTime.Now );
        }

        // 修改回帖
        public void UpdatePost( ForumPost post ) {

            ForumTopic topic = topicService.GetById( post.TopicId, owner );

            List<String> urls = new List<String>();
            urls.Add( controller.to( new ForumController().Index ) ); // 论坛首页失效

            ForumBoard bd = boardService.GetById( topic.ForumBoard.Id, owner );
            addBoardUrl( urls, bd, 0 ); // 所在版块失效(帖子被顶到前面)

            String turl = controller.to( new TopicController().Show, topic.Id );
            int pageNumber = getPostPage( post.Id, topic.Id, 10 ); // 所属主题详细页的分页页面失效
            if (pageNumber > 1) {
                turl = Link.AppendPage( turl, pageNumber );
            }
            urls.Add( turl );

            urls.Add( controller.to( new PostController().Show, post.Id ) ); // 回帖详细页失效

            removeCacheList( urls );
        }


        //-------------------------------------------------------

        // 全局置顶
        public void GlobalSticky() {
            deleteAllBoardCache(); // 所有版块第一页失效
        }

        // 置顶
        public void Sticky( ForumBoard bd ) {
            deleteSingleBoardCache( bd ); // 特定版块第一页失效
        }

        // 精华
        public void PickTopics( String boardUrl ) {

            // boardUrl 是主题所在版块列表分页页面失效
            removeCacheSingle( boardUrl );
        }

        // 锁定
        public void LockTopics( String boardUrl, String ids ) {

            // 所在列表页的分页失效
            removeCacheSingle( boardUrl );

            // 所有主题页(包括主题的分页列表)失效
            int[] arrIds = cvt.ToIntArray( ids );
            foreach (int topicId in arrIds) {
                String topicKey = "forumtopic_" + topicId;
                cacheHelper.SetTimestamp( topicKey, DateTime.Now );
            }
        }

        // 删除主题
        public void DeleteTopic( ForumBoard bd, String ids ) {

            // 版块页，包括分页全部失效
            String boardKey = "forumboard_" + bd.Id;
            cacheHelper.SetTimestamp( boardKey, DateTime.Now );

            // 论坛更新过
            String forumKey = "forum_" + controller.ctx.app.Id;
            cacheHelper.SetTimestamp( forumKey, DateTime.Now );

            // 所有主题页(包括主题的分页列表)失效
            int[] arrIds = cvt.ToIntArray( ids );
            foreach (int topicId in arrIds) {
                String topicKey = "forumtopic_" + topicId;
                cacheHelper.SetTimestamp( topicKey, DateTime.Now );
            }
        }

        public void SortTopic( ForumBoard bd ) {
            deleteSingleBoardCache( bd ); // 特定版块第一页失效
        }

        // 移动主题
        public void MoveTopic( ForumBoard bd, ForumBoard targetBd, String ids ) {

            String boardKey = "forumboard_" + bd.Id;
            cacheHelper.SetTimestamp( boardKey, DateTime.Now );

            String boardKey2 = "forumboard_" + targetBd.Id;
            cacheHelper.SetTimestamp( boardKey2, DateTime.Now );

            int[] arrIds = cvt.ToIntArray( ids );
            foreach (int topicId in arrIds) {
                String topicKey = "forumtopic_" + topicId;
                cacheHelper.SetTimestamp( topicKey, DateTime.Now );
            }
        }

        public void HighlightTopic( String boardUrl, String ids ) {

            // 所在列表页的分页失效
            removeCacheSingle( boardUrl );


        }

        public void SaveTag( ForumTopic topic ) {

            removeCacheSingle( controller.to( new TopicController().Show, topic.Id ) );
        }

        //-------------------------------------------------------

        public void LockSingle( ForumTopic topic ) {

            // 获取主题所在的版块列表页(分页)
            String boardUrl = getBoardUrl( topic );

            // 所在列表页的分页失效
            removeCacheSingle( boardUrl );

            // 主题页(包括主题的分页列表)失效
            String topicKey = "forumtopic_" + topic.Id;
            cacheHelper.SetTimestamp( topicKey, DateTime.Now );
        }

        private string getBoardUrl( ForumTopic topic ) {
            ForumBoard bd = boardService.GetById( topic.ForumBoard.Id, owner );
            int page = getPage( bd, topic ); // 所在版块的分页列表失效
            String url = controller.to( new BoardController().Show, bd.Id );
            if (page > 1) {
                url = Link.AppendPage( url, page );
            }
            return url;
        }

        public void DeleteTopicSingle( ForumTopic topic ) {

            // 修改 board 的 timestamp，让版块分页失效
            String boardKey = "forumboard_" + topic.ForumBoard.Id;
            cacheHelper.SetTimestamp( boardKey, DateTime.Now );

            // 主题页(包括主题的分页列表)失效
            String topicKey = "forumtopic_" + topic.Id;
            cacheHelper.SetTimestamp( topicKey, DateTime.Now );
        }

        public void DeletePostSingle( ForumPost post ) {
            // 主题页(包括主题的分页列表)失效
            String topicKey = "forumtopic_" + post.TopicId;
            cacheHelper.SetTimestamp( topicKey, DateTime.Now );

            String url = controller.to( new PostController().Show, post.Id );
            removeCacheSingle( url );
        }

        public void BanPost( ForumPost post ) {

            // post 所在分页列表失效
            String turl = controller.to( new TopicController().Show, post.TopicId );
            int pageNumber = getPostPage( post.Id, post.TopicId, 10 ); // 所属主题详细页的分页页面失效
            if (pageNumber > 1) {
                turl = Link.AppendPage( turl, pageNumber );
            }
            removeCacheSingle( turl );


            String url = controller.to( new PostController().Show, post.Id );
            removeCacheSingle( url );
        }

        //----------------------------------------------------------------------------------------------

        // 刷新所有版块
        private void deleteAllBoardCache() {
            List<ForumBoard> boads = boardService.GetBoardAll( controller.ctx.app.Id, true );
            List<String> urls = new List<string>();
            foreach (ForumBoard bd in boads) {
                urls.Add( controller.to( new BoardController().Show, bd.Id ) );
            }

            removeCacheList( urls );

        }

        // 刷新某个版块
        private void deleteSingleBoardCache( ForumBoard bd ) {

            String url = controller.to( new BoardController().Show, bd.Id );
            removeCacheSingle( url );
        }

        // 检查父版块
        private void addBoardUrl( List<String> urls, ForumBoard bd, int pageNumber ) {


            if (bd.ParentId > 0) {
                ForumBoard pbd = boardService.GetById( bd.ParentId, owner );
                if (pbd != null) addBoardUrl( urls, pbd, 0 );
            }

            String url = controller.to( new BoardController().Show, bd.Id );
            if (pageNumber > 1) {
                url = Link.AppendPage( url, pageNumber );
            }

            urls.Add( url );
        }


        private int getPage( ForumBoard bd, ForumTopic topic ) {
            return topicService.GetBoardPage( topic.Id, bd.Id, 25 );
        }

        private int getPostPage( int postId, int topicId, int pageSize ) {
            return topicService.GetPostPage( postId, topicId, pageSize );
        }


    }

}
