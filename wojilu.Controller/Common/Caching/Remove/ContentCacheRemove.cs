//using System;
//using System.Collections.Generic;
//using System.Text;
//using wojilu.Apps.Content.Domain;
//using wojilu.Web.Controller.Common;
//using wojilu.Web.Mvc;

//namespace wojilu.Web.Controller.Content {

//    /// <summary>
//    /// 修改(CRUD)数据之后，对缓存的处理
//    /// 移除缓存：1）删除缓存项 2）标记缓存最后更新时间
//    /// </summary>
//    public class ContentCacheRemove : CacheRemoveBase {

//        public ContentCacheRemove( ControllerBase controller ) {
//            base.controller = controller;
//            base.owner = controller.ctx.owner.obj;
//        }

//        // TODO 侧边栏使用ajax方式？如果相关数据更新，则只是侧边栏失效
//        // 评论使用iframe方式

//        public void AddPost( ContentPost post ) {

//            // 首页缓存移除
//            removeCacheSingle( controller.to( new ContentController().Index ) );

//            // 最新列表页标记过期
//            String recentKey = "content_recent_" + post.AppId;
//            cacheHelper.SetTimestamp( recentKey, DateTime.Now );

//            // 列表页标记过期
//            String listKey = "content_list_" + post.PageSection.Id;
//            cacheHelper.SetTimestamp( listKey, DateTime.Now );
//        }

//        public void UpdateAttachment( ContentPost post ) {
//            // 详细页标记过期(包括分页)
//            String detailKey = "content_post_" + post.Id;
//            cacheHelper.SetTimestamp( detailKey, DateTime.Now );
//        }

//        public void UpdatePost( ContentPost post ) {

//            // 首页缓存移除
//            removeCacheSingle( controller.to( new ContentController().Index ) );
            
//            // 所在列表页缓存移除
//            int page = getPage( post ); // 所在版块的分页列表失效
//            String listUrl = Link.AppendPage( controller.to( new SectionController().Show, post.PageSection.Id ), page );
//            removeCacheSingle( listUrl );

//            // 所在recent列表页缓存移除
//            int rPage = getRecentPageIndex( post ); // 所在版块的分页列表失效
//            String rListUrl = Link.AppendPage( controller.to( new PostController().Recent ), rPage );
//            removeCacheSingle( rListUrl );

//            // 详细页标记过期(包括分页)
//            String detailKey = "content_post_" + post.Id;
//            cacheHelper.SetTimestamp( detailKey, DateTime.Now );
//        }

//        public void MovePost( ContentPost post, int targeSectionId ) {

//            // 首页缓存移除
//            removeCacheSingle( controller.to( new ContentController().Index ) );

//            // 列表页标记过期
//            String listKey = "content_list_" + post.PageSection.Id;
//            cacheHelper.SetTimestamp( listKey, DateTime.Now );

//            // 目标列表页标记过期
//            String tagetListKey = "content_list_" + targeSectionId;
//            cacheHelper.SetTimestamp( tagetListKey, DateTime.Now );

//            // 详细页标记过期(包括分页)
//            String detailKey = "content_post_" + post.Id;
//            cacheHelper.SetTimestamp( detailKey, DateTime.Now );

//        }

//        public void MovePostList( String ids, int targeSectionId ) {

//            // 首页缓存移除
//            removeCacheSingle( controller.to( new ContentController().Index ) );

//            List<ContentPost> posts = ContentPost.find( "Id in (" + ids + ")" ).list();

//            foreach (ContentPost post in posts) {
//                // 列表页标记过期
//                String listKey = "content_list_" + post.PageSection.Id;
//                cacheHelper.SetTimestamp( listKey, DateTime.Now );

//                // 详细页标记过期(包括分页)
//                String detailKey = "content_post_" + post.Id;
//                cacheHelper.SetTimestamp( detailKey, DateTime.Now );

//            }

//            // 目标列表页标记过期
//            String tagetListKey = "content_list_" + targeSectionId;
//            cacheHelper.SetTimestamp( tagetListKey, DateTime.Now );


//        }

//        public void UpdateTitleStyle( ContentPost post ) {
//            UpdatePost( post );
//        }


//        private int getRecentPageIndex( ContentPost post ) {
//            int count = ContentPost.count( "Id>=" + post.Id + " and AppId=" + post.AppId + " " ); // TODO 移到service
//            int pageSize = 25; // TODO 配置
//            return Link.GetPageIndex( count, pageSize );
//        }


//        private int getPage( ContentPost post ) {
//            int count = ContentPost.count( "Id>=" + post.Id + " and SectionId=" + post.PageSection.Id + " " );// TODO 移到service
//            int pageSize = 25; // TODO 配置
//            return Link.GetPageIndex( count, pageSize );
//        }


//        public void DeletePost( ContentPost post ) {

//            // 首页缓存移除
//            removeCacheSingle( controller.to( new ContentController().Index ) );

//            // recent列表页标记过期
//            String recentKey = "content_recent_" + post.AppId;
//            cacheHelper.SetTimestamp( recentKey, DateTime.Now );
            
//            // 列表页标记过期
//            String listKey = "content_list_" + post.PageSection.Id;
//            cacheHelper.SetTimestamp( listKey, DateTime.Now );

//            // 详细页标记过期(包括分页)
//            String detailKey = "content_post_" + post.Id;
//            cacheHelper.SetTimestamp( detailKey, DateTime.Now );
//        }

//        public void UpdateTag( ContentPost post ) {

//            // TODO 更新tag首页

//            // 详细页标记过期(包括分页)
//            String detailKey = "content_post_" + post.Id;
//            cacheHelper.SetTimestamp( detailKey, DateTime.Now );
//        }

//        public void AddRow() {

//            // 移除首页缓存
//            removeCacheSingle( controller.to( new ContentController().Index ) );
//        }

//        public void DeleteRow() {
//            // 移除首页缓存
//            removeCacheSingle( controller.to( new ContentController().Index ) );
//        }

//        public void DragSection() {

//            // 移除首页缓存
//            removeCacheSingle( controller.to( new ContentController().Index ) );

//        }
//        public void AddSection() {

//            // 移除首页缓存
//            removeCacheSingle( controller.to( new ContentController().Index ) );

//        }
//        public void CombineSection() {

//            // 移除首页缓存
//            removeCacheSingle( controller.to( new ContentController().Index ) );

//        }
//        public void DeleteSection( ContentSection section ) {

//            // 移除首页缓存
//            removeCacheSingle( controller.to( new ContentController().Index ) );


//            // 列表页标记过期
//            String listKey = "content_list_" + section.Id;
//            cacheHelper.SetTimestamp( listKey, DateTime.Now );

//        }

//        public void UpdateUI() { // 三种样式……

//            // 移除首页缓存
//            removeCacheSingle( controller.to( new ContentController().Index ) );
//        }

//        public void UpdateTemplate() {

//            // 移除首页缓存
//            removeCacheSingle( controller.to( new ContentController().Index ) );

//        }
//        public void UpdateSkin() {

//            // 移除首页缓存
//            removeCacheSingle( controller.to( new ContentController().Index ) );

//        }

//        // 更新区块配置
//        public void UpdateSettings() {
//            // 移除首页缓存
//            removeCacheSingle( controller.to( new ContentController().Index ) );
//        }

//        // TODO 针对每个选项进行有针对性的移除缓存
//        public void UpdateAppSetting() {
//            // 移除首页缓存
//            removeCacheSingle( controller.to( new ContentController().Index ) );

//            // 所有列表页过期

//            // 所有详细页布局栏(排行数据)过期

//            // 所有都过期
//        }

//    }

//}
