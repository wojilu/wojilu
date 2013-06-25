/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.ORM;

using wojilu.Common;
using wojilu.Common.Tags;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Jobs;
using wojilu.Common.AppBase;

using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Interface;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;

using wojilu.Members.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Groups.Domain;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;

namespace wojilu.Apps.Forum.Service {

    public class ForumTopicService : IForumTopicService {

        public virtual IAttachmentService AttachmentService { get; set; }
        public virtual IForumBoardService boardService { get; set; }
        public virtual IForumCategoryService categoryService { get; set; }
        public virtual IForumService forumService { get; set; }
        public virtual IForumLogService logService { get; set; }
        public virtual IUserService userService { get; set; }
        public virtual IUserIncomeService incomeService { get; set; }
        public virtual IMessageService msgService { get; set; }

        public ForumTopicService() {
            forumService = new ForumService();
            categoryService = new ForumCategoryService();
            boardService = new ForumBoardService();
            userService = new UserService();
            AttachmentService = new AttachmentService();
            logService = new ForumLogService();
            incomeService = new UserIncomeService();
            msgService = new MessageService();
        }

        public virtual ForumTopic GetById_ForAdmin( int id ) {
            return GetById( id );
        }

        private ForumTopic GetById( int id ) {
            return db.findById<ForumTopic>( id );
        }

        public virtual ForumTopic GetByPost( int postId ) {
            ForumPost post = ForumPost.findById( postId );
            if (post == null) return null;
            ForumTopic topic = GetById( post.TopicId );
            if (topic == null) return null;
            if (topic.Status == (int)TopicStatus.Delete) return null;
            return topic;
        }

        public virtual ForumTopic GetById( int id, IMember owner ) {

            ForumTopic topic = GetById( id );
            if (topic == null) return null;
            if (topic.OwnerId != owner.Id) return null;
            if (topic.OwnerType.Equals( owner.GetType().FullName ) == false) return null;

            if (topic.Status == (int)TopicStatus.Delete) return null;
            return topic;
        }

        public virtual int GetBoardPage( int topicId, int boardId, int pageSize ) {
            int count = ForumTopic.count( "Id>=" + topicId + " and ForumBoardId=" + boardId + " and " + TopicStatus.GetShowCondition() );
            return getPage( count, pageSize );
        }

        public virtual int GetPostPage( int postId, int topicId, int pageSize ) {

            int count = ForumPost.count( "Id<=" + postId + " and TopicId=" + topicId + " and " + TopicStatus.GetShowCondition() );
            return getPage( count, pageSize );
        }

        private int getPage( int count, int pageSize ) {

            if (count == 0) return 1;

            int mod = count % pageSize;
            if (mod == 0) return count / pageSize;

            return count / pageSize + 1;

        }

        public virtual List<ForumTopic> GetByApp( int appId, int count ) {
            return ForumTopic.find( "AppId=" + appId + " and " + TopicStatus.GetShowCondition() ).list( count );
        }

        public virtual DataPage<ForumTopic> GetPageByApp( int appId, int pageSize ) {
            return ForumTopic.findPage( "AppId=" + appId + " and " + TopicStatus.GetShowCondition(), pageSize );
        }

        public virtual DataPage<ForumTopic> GetByUserAndApp( int appId, int userId, int pageSize ) {
            if (userId <= 0 || appId <= 0) return DataPage<ForumTopic>.GetEmpty();
            return ForumTopic.findPage( "AppId=" + appId + " and CreatorId=" + userId + " and " + TopicStatus.GetShowCondition(), pageSize );
        }

        public virtual DataPage<ForumTopic> GetByUser( int userId, int pageSize ) {
            if (userId <= 0) return DataPage<ForumTopic>.GetEmpty();
            return ForumTopic.findPage( "CreatorId=" + userId + " and OwnerType='" + typeof( Site ).FullName + "' and " + TopicStatus.GetShowCondition(), pageSize );
        }

        public virtual DataPage<ForumTopic> GetPickedByApp( int appId, int pageSize ) {
            return ForumTopic.findPage( "AppId=" + appId + " and IsPicked=1 and " + TopicStatus.GetShowCondition(), pageSize );
        }

        public virtual List<ForumTopic> GetByAppAndReplies( int appId, int count ) {
            return ForumTopic.find( "AppId=" + appId + " and " + TopicStatus.GetShowCondition() + " order by Replies desc, Id desc" ).list( count );
        }

        public virtual List<ForumTopic> GetByAppAndReplies( int appId, int count, int days ) {

            EntityInfo ei = Entity.GetInfo( typeof( User ) );

            String t = ei.Dialect.GetTimeQuote();
            String fs = " and Created between " + t + "{0}" + t + " and " + t + "{1}" + t + " ";
            DateTime now = DateTime.Now;
            String dc = string.Format( fs, now.AddDays( -days + 1 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() ); // 加1表示包含今天

            return ForumTopic.find( "AppId=" + appId + " and " + TopicStatus.GetShowCondition() + dc + " order by Replies desc, Id desc" ).list( count );
        }

        public virtual List<ForumTopic> GetByAppAndViews( int appId, int count ) {
            return ForumTopic.find( "AppId=" + appId + " and " + TopicStatus.GetShowCondition() + " order by Hits desc, Id desc" ).list( count );
        }


        public virtual DataPage<ForumTopic> GetDeletedPage( int appId ) {
            return db.findPage<ForumTopic>( "AppId=" + appId + " and Status=" + TopicStatus.Delete );
        }

        //-----------------------------------------------------------------------

        public virtual List<IBinderValue> GetNewSiteTopic( int count ) {
            return getNewTopic( count, typeof( Site ) );
        }

        public virtual List<IBinderValue> GetNewGroupTopic( int count ) {
            return getNewTopic( count, typeof( Group ) );
        }

        public virtual List<IBinderValue> GetNewBoardTopic( String ids, int count ) {

            if (count <= 0) count = 10;

            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) return new List<IBinderValue>();

            String bd = " and ForumBoardId in ( " + sids + " )";

            List<ForumTopic> list = db.find<ForumTopic>( TopicStatus.GetShowCondition() + bd + " and OwnerType=:otype order by Id desc" )
                .set( "otype", typeof( Site ).FullName )
                .list( count );

            return SysForumTopicService.populateBinderValue( list );
        }

        private String checkIds( String ids ) {

            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return null;

            String sids = "";
            for (int i = 0; i < arrIds.Length; i++) {
                if (arrIds[i] == 0) continue;
                sids += arrIds[i];
                if (i < arrIds.Length - 1) sids += ",";
            }

            return sids;
        }

        private List<IBinderValue> getNewTopic( int count, Type ownerType ) {
            if (count <= 0) count = 10;

            List<ForumTopic> list = db.find<ForumTopic>( TopicStatus.GetShowCondition() + " and OwnerType=:otype order by Id desc" )
                .set( "otype", ownerType.FullName )
                .list( count );

            return SysForumTopicService.populateBinderValue( list );
        }

        //private static List<IBinderValue> populateBinderValue( List<ForumTopic> list ) {
        //    List<IBinderValue> results = new List<IBinderValue>();
        //    foreach (ForumTopic topic in list) {
        //        IBinderValue vo = new ItemValue();

        //        vo.Title = topic.Creator.Name + ":" + topic.Title;
        //        vo.Created = topic.Created;
        //        vo.CreatorName = topic.Creator.Name;
        //        vo.Link = alink.ToAppData( topic );
        //        vo.Replies = topic.Replies;

        //        results.Add( vo );
        //    }

        //    return results;
        //}

        //------------------------------------------------------------------------------------------------------

        public virtual ForumPost GetPostByTopic( int topicId ) {
            return db.find<ForumPost>( "TopicId=" + topicId + " and ParentId=0" ).first();
        }

        public virtual ForumTopic GetNext( ForumTopic topic ) {
            return db.find<ForumTopic>( "Replied<:replied and " + getCondition( topic.ForumBoard.Id ) + " order by Replied desc, Id desc" )
                .set( "replied", topic.Created )
                .first();
        }

        public virtual ForumTopic GetPre( ForumTopic topic ) {
            return db.find<ForumTopic>( "Replied>:replied and " + getCondition( topic.ForumBoard.Id ) + " order by Replied asc, Id asc" )
                .set( "replied", topic.Created )
                .first();
        }

        //------------------------------------------------------------------------------------------------------

        public virtual List<ForumTopic> GetStickyList( int boardId ) {
            return db.find<ForumTopic>( "ForumBoardId=" + boardId + " and Status=" + TopicStatus.Sticky + " order by OrderId desc, Id desc" ).list();
        }

        public virtual List<ForumTopic> getSubstractStickyList( List<ForumTopic> globalStickyList, int boardId ) {
            List<ForumTopic> boardStickyList = GetStickyList( boardId );
            if (globalStickyList == null || globalStickyList.Count == 0) return boardStickyList;

            List<ForumTopic> results = new List<ForumTopic>();
            foreach (ForumTopic t in boardStickyList) {
                if (isTopicInGlobal( t, globalStickyList )) continue;
                results.Add( t );
            }
            return results;
        }

        public virtual List<ForumTopic> getMergedStickyList( List<ForumTopic> globalStickyList, int boardId, int page ) {
            List<ForumTopic> boardStickyList = page <= 1 ? GetStickyList( boardId ) : null;
            return mergeStickyList( globalStickyList, boardStickyList );
        }

        private List<ForumTopic> mergeStickyList( List<ForumTopic> globalStickyList, List<ForumTopic> boardStickyList ) {
            if (globalStickyList == null || globalStickyList.Count == 0) return boardStickyList;
            if (boardStickyList == null || boardStickyList.Count == 0) return globalStickyList;
            List<ForumTopic> results = new List<ForumTopic>();
            foreach (ForumTopic gt in globalStickyList) results.Add( gt );
            foreach (ForumTopic t in boardStickyList) {
                if (isTopicInGlobal( t, results )) continue;
                results.Add( t );
            }
            return results;
        }

        private Boolean isTopicInGlobal( ForumTopic t, List<ForumTopic> globalStickyList ) {
            foreach (ForumTopic gt in globalStickyList) {
                if (t.Id == gt.Id) return true;
            }
            return false;
        }

        public virtual void StickyMoveUp( int topicId ) {

            ForumTopic topic = db.findById<ForumTopic>( topicId );
            List<ForumTopic> stickyList = GetStickyList( topic.ForumBoard.Id );
            new SortUtil<ForumTopic>( topic, stickyList ).MoveUp();
        }

        public virtual void StickyMoveDown( int topicId ) {

            ForumTopic topic = db.findById<ForumTopic>( topicId );
            List<ForumTopic> stickyList = GetStickyList( topic.ForumBoard.Id );
            new SortUtil<ForumTopic>( topic, stickyList ).MoveDown();
        }

        //--------------------------------------------------------------------------------

        public virtual DataPage<ForumTopic> FindPickedPage( int boardId, int pageSize ) {
            return db.findPage<ForumTopic>( getCondition( boardId ) + " and IsPicked=1 order by Replied desc, Id desc", pageSize );
        }

        public virtual DataPage<ForumTopic> FindPollPage( int boardId, int pageSize ) {
            String pollTypeName = typeof( ForumPoll ).FullName;
            return db.findPage<ForumTopic>( getCondition( boardId ) + " and TypeName='" + pollTypeName + "' order by Replied desc, Id desc", pageSize );
        }


        public virtual DataPage<ForumTopic> FindTopicPage( int boardId, int pageSize, int categoryId, String sort, String time ) {

            String condition = getCondition( boardId );

            if (categoryId > 0) condition += " and CategoryId=" + categoryId;

            if (strUtil.IsNullOrEmpty( time ) || time.Equals( "all" )) {
            }
            else {
                DateTime t = getValidTime( time );

                EntityInfo ei = Entity.GetInfo( typeof( ForumTopic ) );

                String tquote = ei.Dialect.GetTimeQuote();

                condition += " and Replied>" + tquote + "" + t.ToShortDateString() + tquote;
            }

            if (strUtil.IsNullOrEmpty( sort ))
                condition += " order by Replied desc, Id desc";
            else if (sort.Equals( "created" ))
                condition += " order by Created desc, Id desc";
            else if (sort.Equals( "replies" ))
                condition += " order by Replies desc, Id desc";
            else if (sort.Equals( "views" ))
                condition += " order by Hits desc, Id desc";

            return db.findPage<ForumTopic>( condition, pageSize );
        }

        private static DateTime getValidTime( String time ) {
            DateTime t = DateTime.Now.AddYears( 99 );
            if (time.Equals( "day" ))
                t = DateTime.Now.AddDays( -1 );
            else if (time.Equals( "day2" ))
                t = DateTime.Now.AddDays( -2 );
            else if (time.Equals( "week" ))
                t = DateTime.Now.AddDays( -7 );
            else if (time.Equals( "month" ))
                t = DateTime.Now.AddMonths( -1 );
            else if (time.Equals( "month3" ))
                t = DateTime.Now.AddMonths( -3 );
            else if (time.Equals( "month6" ))
                t = DateTime.Now.AddMonths( -6 );
            return t;
        }

        private String getCondition( int boardId ) {
            return string.Format( "ForumBoardId={0} and Status={1}", boardId, TopicStatus.Normal );
        }

        //--------------------------------------------------------------------------------


        public virtual int CountReply( int topicId ) {
            return ForumPost.find( "TopicId=" + topicId + " and Status<>" + TopicStatus.Delete ).count() - 1;
        }

        public virtual Result CreateTopic( ForumTopic topic, User user, IMember owner, IApp app ) {

            populateData( topic, user, owner, app.Id );
            return saveToDb( topic, user, app );
        }

        public virtual Result CreateTopicOther( int forumId, String title, String content, Type dataType, User user, IMember owner, IApp app ) {
            ForumTopic topic = new ForumTopic();
            topic.ForumBoard = new ForumBoard( forumId );
            topic.Title = title;
            topic.Content = content;
            if (dataType != typeof( ForumTopic )) {
                topic.TypeName = dataType.FullName;
            }
            populateData( topic, user, owner, app.Id );
            return saveToDb( topic, user, app );
        }

        public virtual ForumPost InsertTopicPost( ForumTopic topic ) {
            ForumPost post = new ForumPost();
            post.AppId = topic.AppId;
            post.ForumBoardId = topic.ForumBoard.Id;
            post.Creator = topic.Creator;
            post.CreatorUrl = topic.CreatorUrl;
            post.OwnerId = topic.OwnerId;
            post.OwnerType = topic.OwnerType;
            post.OwnerUrl = topic.OwnerUrl;
            post.TopicId = topic.Id;
            post.ParentId = 0;
            post.Title = topic.Title;
            post.Content = topic.Content;
            post.Created = topic.Created;
            post.Ip = topic.Ip;
            Result result = db.insert( post );
            if (result.HasErrors) throw new Exception( "insert forum post error." );
            return post;
        }

        private static void populateData( ForumTopic topic, User user, IMember owner, int appId ) {
            topic.AppId = appId;
            topic.Creator = user;
            topic.CreatorUrl = user.Url;
            topic.OwnerId = owner.Id;
            topic.OwnerUrl = owner.Url;
            topic.OwnerType = owner.GetType().FullName;
            topic.Replied = DateTime.Now;
            topic.RepliedUserName = user.Name;
            topic.RepliedUserFriendUrl = user.Url;
        }

        private Result saveToDb( ForumTopic topic, User user, IApp app ) {

            if (topic.Reward > 0) {
                topic.TypeName = PostTypeString.Question;
            }

            if (topic.Reward > 0 && topic.Reward > user.Credit) {
                return new Result( string.Format( "对不起，您现有积分{0}，不够悬赏", user.Credit ) );
            }

            Result result = db.insert( topic );
            if (result.HasErrors) throw new Exception( "insert forumtopic error" );

            InsertTopicPost( topic );
            TagService.SaveDataTag( topic, topic.TagRawString );
            AppTagService.Insert( app, topic.TagRawString, user.Id );
            updateCount( topic, user, app );

            // 发布悬赏问题，不会产生任何收益
            if (topic.Reward > 0) {
                String msg = string.Format( "发布悬赏帖子 <a href=\"{0}\">{1}</a>，扣除积分", alink.ToAppData( topic ), topic.Title );
                incomeService.AddIncome( user, KeyCurrency.Instance.Id, -topic.Reward, msg );
            }
            else {
                String msg = string.Format( "发布主题 <a href=\"{0}\">{1}</a>，得到奖励", alink.ToAppData( topic ), topic.Title );
                incomeService.AddIncome( user, UserAction.Forum_CreateTopic.Id, msg );
            }

            addFeedInfo( topic );

            return result;
        }

        private void addFeedInfo( ForumTopic data ) {
            String lnkPost = alink.ToAppData( data );

            String post = string.Format( "<a href=\"{0}\">{1}</a>", lnkPost, data.Title );

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add( "topic", post );
            String templateData = Json.ToString( dic );

            TemplateBundle tplBundle = TemplateBundle.GetForumTopicTemplateBundle();
            new FeedService().publishUserAction( data.Creator, typeof( ForumTopic ).FullName, tplBundle.Id, templateData, "", data.Ip );
        }

        public virtual Result Update( ForumTopic topic, User user, IMember owner ) {
            Result result = db.update( topic );
            if (result.IsValid) {
                UpdatePostByTopic( topic, user );
                updateForumLastupdate( topic, user );
                TagService.SaveDataTag( topic, topic.TagRawString );
            }
            return result;
        }

        private void updateForumLastupdate( ForumTopic topic, User user ) {

            ForumBoard fb = boardService.GetById( topic.ForumBoard.Id, topic.OwnerId, topic.OwnerType );

            LastUpdateInfo info = new LastUpdateInfo();
            info.PostId = topic.Id;
            info.PostType = typeof( ForumTopic ).Name;
            info.PostTitle = topic.Title;

            info.CreatorName = user.Name;
            info.CreatorUrl = user.Url;
            info.UpdateTime = DateTime.Now;

            fb.LastUpdateInfo = info;

            fb.Updated = info.UpdateTime;

            boardService.Update( fb );

        }

        public virtual void UpdateAttachments( ForumTopic topic, int attachmentCount ) {
            topic.Attachments = attachmentCount;
            db.update( topic, "Attachments" );
            ForumPost postByTopic = GetPostByTopic( topic.Id );
            postByTopic.Attachments = attachmentCount;
            db.update( postByTopic, "Attachments" );
        }

        private void updateCount( ForumTopic topic, User user, IApp app ) {


            ForumBoard fb = boardService.GetById( topic.ForumBoard.Id, topic.OwnerId, topic.OwnerType );

            fb.TodayPosts++;
            fb.Topics = boardService.CountTopic( fb.Id );

            LastUpdateInfo info = new LastUpdateInfo();
            info.PostId = topic.Id;
            info.PostType = typeof( ForumTopic ).Name;
            info.PostTitle = topic.Title;

            info.CreatorName = user.Name;
            info.CreatorUrl = user.Url;
            info.UpdateTime = DateTime.Now;

            fb.LastUpdateInfo = info;

            fb.Updated = info.UpdateTime;

            boardService.Update( fb );
            ForumApp forum = app as ForumApp;
            forum.TopicCount = forum.CountTopic();
            forum.TodayTopicCount++;
            forum.LastUpdateMemberName = user.Name;
            forum.LastUpdateMemberUrl = user.Url;
            forum.LastUpdatePostTitle = topic.Title;
            forum.LastUpdateTime = topic.Created;
            forumService.Update( forum );

            userService.AddPostCount( user );
        }

        internal void UpdatePostByTopic( ForumTopic topic, User user ) {


            ForumPost post = GetPostByTopic( topic.Id );
            post.Title = topic.Title;
            post.Content = topic.Content;

            UpdateLastEditInfo( user, post );

        }

        public void UpdateLastEditInfo( User user, ForumPost post ) {
            post.EditTime = DateTime.Now;
            post.EditCount++;
            post.EditMemberId = user.Id;
            db.update( post );
        }

        public virtual void UpdateReply( ForumTopic topic ) {
            db.update( topic, new string[] { "Replies", "RepliedUserName", "RepliedUserFriendUrl", "Replied" } );
        }

        //--------------------------------------- delete -----------------------------------------

        public virtual void DeleteToTrash( ForumTopic topic, User creator, String ip ) {

            topic.Status = TopicStatus.Delete;
            db.update( topic, "Status" );

            List<ForumPost> posts = db.find<ForumPost>( "TopicId=" + topic.Id ).list();
            foreach (ForumPost p in posts) {
                p.Status = TopicStatus.Delete;
                db.update( p, "Status" );
            }

            // 更新作者的收入
            if (topic.Creator != null && topic.Creator.Id > 0) { // 规避已注销用户
                String msg = string.Format( "主题被删除<a href=\"{0}\">{1}</a>", alink.ToAppData( topic ), topic.Title );
                incomeService.AddIncome( topic.Creator, UserAction.Forum_TopicDeleted.Id, msg );
            }

            // 检查总置顶帖子缓存
            StickyTopic.DeleteTopic( topic );

            logService.AddTopic( creator, topic.AppId, topic.Id, ForumLogAction.Delete, ip );
        }

        public virtual void DeleteTrue( ForumTopic topic, User viewer, String ip ) {


            // 作者的帖子数
            int creatorId = topic.Creator.Id;
            userService.DeletePostCount( creatorId );

            // 帖子本身和tag
            topic.Tag.DeleteTags();
            db.delete( topic );

            // 主题相关的post
            ForumPost postByTopic = GetPostByTopic( topic.Id );
            //postByTopic.delete();
            List<ForumPost> posts = db.find<ForumPost>( "TopicId=" + topic.Id ).list();
            foreach (ForumPost p in posts) {
                db.delete( p );
            }

            // 相关的附件
            int postId = postByTopic.Id;
            AttachmentService.DeleteByPost( postId );

            // 更新板块的统计
            //int replies = topic.Replies;
            //int forumBoardId = topic.ForumBoard.Id;
            //boardService.DeleteTopicCount( forumBoardId, replies, topic.OwnerId );

            logService.AddTopic( viewer, topic.AppId, topic.Id, ForumLogAction.DeleteTrue, ip );
        }

        // 只是放到回收站
        public virtual void DeleteListToTrash( String choice ) {

            int[] arrId = cvt.ToIntArray( choice );
            foreach (int id in arrId) {

                ForumTopic topic = GetById( id );

                if (topic == null) continue;

                topic.Status = TopicStatus.Delete;
                db.update( topic, "Status" );

                List<ForumPost> posts = db.find<ForumPost>( "TopicId=" + id ).list();
                foreach (ForumPost p in posts) {
                    p.Status = TopicStatus.Delete;
                    db.update( p, "Status" );
                }

            }
        }

        public virtual void Restore( String choice ) {
            int[] arrId = cvt.ToIntArray( choice );
            foreach (int id in arrId) {

                ForumTopic topic = GetById( id );

                if (topic == null) continue;

                topic.Status = TopicStatus.Normal;
                db.update( topic, "Status" );

                List<ForumPost> posts = db.find<ForumPost>( "TopicId=" + id ).list();
                foreach (ForumPost p in posts) {
                    p.Status = TopicStatus.Normal;
                    db.update( p, "Status" );
                }

                String msg = string.Format( "撤销删除(主题): <a href=\"{0}\">{1}</a>", alink.ToAppData( topic ), topic.Title );
                incomeService.AddIncomeReverse( topic.Creator, UserAction.Forum_TopicDeleted.Id, msg );

            }
        }

        public virtual void DeleteListTrue( String choice, User viewer, String ip ) {
            int[] arrId = cvt.ToIntArray( choice );
            foreach (int id in arrId) {
                ForumTopic topic = GetById( id );
                if (topic == null) continue;
                DeleteTrue( topic, viewer, ip );
            }
        }

        public virtual void DeletePostCount( int topicId, IMember owner ) {
            ForumTopic topic = GetById( topicId, owner );
            topic.Replies--;
            db.update( topic );
        }

        public void AddHits( ForumTopic topic ) {
            //topic.Hits++;
            //db.update( topic, "Hits" );
            HitsJob.Add( topic );
        }

        public virtual void SubstractTopicReward( ForumTopic topic, int postValue ) {
            topic.RewardAvailable -= postValue;
            db.update( topic, "RewardAvailable" );
        }

        //--------------------------------------- admin -----------------------------------------
        
        public virtual void AdminUpdate( String action, String condition ) {
            db.updateBatch<ForumTopic>( action, condition );
        }

        public virtual void MakeSticky( AdminValue av ) {
            av.ActionId = ForumLogAction.Sticky;
            AdminUpdate( "set Status=" + TopicStatus.Sticky, av.Condition );
            AddAuthorIncome( av.Condition, UserAction.Forum_TopicSticky.Id, "置顶" );
            makeLog( av );
        }

        public virtual void MakeStickyUndo( AdminValue av ) {
            av.ActionId = ForumLogAction.UnSticky;
            AdminUpdate( "set Status=" + TopicStatus.Normal, av.Condition );
            //取消置顶不减少积分
            makeLog( av );
        }

        public virtual void MakePick( AdminValue av ) {
            av.ActionId = ForumLogAction.Pick;
            AdminUpdate( "set IsPicked=1", av.Condition );
            AddAuthorIncome( av.Condition, UserAction.Forum_TopicPicked.Id, "加为精华" );
            makeLog( av );
        }

        public virtual void MakePickUndo( AdminValue av ) {
            av.ActionId = ForumLogAction.UnPick;
            AdminUpdate( "set IsPicked=0", av.Condition );
            //取消精华不减少积分
            makeLog( av );
        }

        public virtual void MakeHighlight( String style, AdminValue av ) {
            av.ActionId = ForumLogAction.Highlight;
            String action = string.Format( "set TitleStyle='{0}'", style );
            ForumTopic.updateBatch( action, av.Condition );
            makeLog( av );
        }

        public virtual void MakeHighlightUndo( AdminValue av ) {
            av.ActionId = ForumLogAction.UnHighlight;
            String action = "set TitleStyle=''";
            ForumTopic.updateBatch( action, av.Condition );
            makeLog( av );
        }

        public virtual void MakeLock( AdminValue av ) {
            av.ActionId = ForumLogAction.Lock;
            AdminUpdate( "set IsLocked=1", av.Condition );
            // 积分规则中本身定义的是负值，所以此处用AddIncome
            AddAuthorIncome( av.Condition, UserAction.Forum_TopicLocked.Id, "锁定" );
            makeLog( av );
        }

        public virtual void MakeLockUndo( AdminValue av ) {
            av.ActionId = ForumLogAction.UnLock;
            AdminUpdate( "set IsLocked=0", av.Condition );
            // 积分规则中本身定义的是负值，所以反操作用SubstractIncome
            SubstractAuthorIncome( av.Condition, UserAction.Forum_TopicLocked.Id, "取消锁定" );
            makeLog( av );
        }

        public virtual void DeleteList( AdminValue av ) {
            av.ActionId = ForumLogAction.Delete;

            DeleteListToTrash( av.Ids );
            // 积分规则中本身定义的是负值，所以此处用AddIncome
            AddAuthorIncome( av.Condition, UserAction.Forum_TopicDeleted.Id, "删除" );

            // 检查总置顶
            StickyTopic.DeleteTopic( av.AppId, av.Ids );

            makeLog( av );
        }

        public virtual void MakeCategory( int categoryId, AdminValue av ) {
            av.ActionId = ForumLogAction.SetCategory;
            String action = string.Format( "set CategoryId=" + categoryId );
            ForumTopic.updateBatch( action, av.Condition );
            makeLog( av );
        }

        public virtual void MakeMove( int targetForumId, AdminValue av) {

            av.ActionId = ForumLogAction.MoveTopic;

            String condition = "Id in (" + av.Ids + ")";
            String conditionPost = "TopicId in (" + av.Ids + ")";
            String action = "set ForumBoardId=" + targetForumId;

            db.updateBatch<ForumTopic>( action, condition );
            db.updateBatch<ForumPost>( action, conditionPost );

            makeLog( av );
        }

        public virtual void MakeGlobalSticky( AdminValue av ) {

            av.ActionId = ForumLogAction.GlobalSticky;

            ForumApp app = db.findById<ForumApp>( av.AppId );
            if (app == null) return;

            List<ForumTopic> newStickTopics = db.find<ForumTopic>( av.Condition ).list();

            app.StickyTopic = StickyTopic.MergeData( app.StickyTopic, newStickTopics );
            db.update( app );

            makeLog( av );
        }

        public virtual void MakeGloablStickyUndo( AdminValue av ) {

            av.ActionId = ForumLogAction.GlobalUnSticky;

            ForumApp app = db.findById<ForumApp>( av.AppId );
            if (app == null) return;

            List<ForumTopic> newStickTopics = db.find<ForumTopic>( av.Condition ).list();

            app.StickyTopic = StickyTopic.SubtractData( app.StickyTopic, newStickTopics );
            db.update( app );

            makeLog( av );
        }

        private void makeLog( AdminValue av ) {

            List<ForumTopic> topics = GetByIds( av.Ids );

            foreach (ForumTopic topic in topics) {

                logService.AddTopic( av.User, av.AppId, topic.Id, av.ActionId, av.Reason, av.Ip );

                // 发送短信通知
                if (!av.IsSendMsg) continue;

                String msg = ForumLogAction.GetLable( av.ActionId );
                String title = string.Format( alang.get( typeof( ForumApp ), "adminPostMsgTitle" ), topic.Title, msg );

                String topicInfo = "<a href='" + alink.ToAppData( topic ) + "'>" + topic.Title + "</a>";
                if (av.ActionId == ForumLogAction.Delete) topicInfo = topic.Title;

                String body = string.Format( alang.get( typeof( ForumApp ), "adminPostMsgBody" ), topicInfo, msg, DateTime.Now, av.Reason );

                msgService.SiteSend( title, body, topic.Creator );
            }

        }

        //-----------------------------------------------------------------------------------------


        public virtual void Lock( ForumTopic topic, User user, String ip ) {
            topic.IsLocked = 1;
            db.update( topic, "IsLocked" );

            String msg = string.Format( "帖子被锁定 <a href=\"{0}\">{1}</a>", alink.ToAppData( topic ), topic.Title );
            incomeService.AddIncome( topic.Creator, UserAction.Forum_TopicLocked.Id, msg );

            logService.AddTopic( user, topic.AppId, topic.Id, ForumLogAction.Lock, ip );
        }

        public virtual void UnLock( ForumTopic topic, User user, String ip ) {
            topic.IsLocked = 0;
            db.update( topic, "IsLocked" );

            String msg = string.Format( "帖子取消锁定 <a href=\"{0}\">{1}</a>", alink.ToAppData( topic ), topic.Title );
            incomeService.AddIncomeReverse( topic.Creator, UserAction.Forum_TopicLocked.Id, msg );
            logService.AddTopic( user, topic.AppId, topic.Id, ForumLogAction.UnLock, ip );
        }

        //--------------------------------------- income -----------------------------------------

        public virtual void AddAuthorIncome( String condition, int actionId, String actionName ) {
            List<ForumTopic> topics = db.find<ForumTopic>( condition ).list();
            foreach (ForumTopic topic in topics) {
                String msg = string.Format( "帖子被{0} <a href=\"{1}\">{2}</a>", actionName, alink.ToAppData( topic ), topic.Title ); incomeService.AddIncome( topic.Creator, actionId, msg );
            }
        }

        public virtual void SubstractAuthorIncome( String condition, int actionId, String actionName ) {
            List<ForumTopic> topics = db.find<ForumTopic>( condition ).list();
            foreach (ForumTopic topic in topics) {
                String msg = string.Format( "帖子被{0} <a href=\"{1}\">{2}</a>", actionName, alink.ToAppData( topic ), topic.Title );
                incomeService.AddIncomeReverse( topic.Creator, actionId, msg );
            }
        }

        public virtual List<ForumTopic> GetByIds( String idList ) {
            if (cvt.IsIdListValid( idList ) == false) return new List<ForumTopic>();
            return db.find<ForumTopic>( "Id in (" + idList + ")" ).list();
        }

        //--------------------------------------------------------------------------------



        public virtual void UpdateAttachmentPermission( ForumTopic topic, int ischeck ) {
            topic.IsAttachmentLogin = ischeck;
            topic.update( "IsAttachmentLogin" );
        }

        //--------------------------------------------------------------------------------

        public virtual DataPage<ForumTopic> Search( int appId, string key, int pageSize ) {
            if (strUtil.IsNullOrEmpty( key )) return DataPage<ForumTopic>.GetEmpty();
            String q = strUtil.SqlClean( key, 10 );
            return ForumTopic.findPage( "AppId=" + appId + " and Title like '%" + q + "%' and " + TopicStatus.GetShowCondition(), pageSize );
        }

    }
}

