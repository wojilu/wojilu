/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Common;
using wojilu.Web.Mvc;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Common.AppBase.Interface;

using wojilu.Common.Jobs;

using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Feeds.Service;

using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;

using wojilu.Common.Msg.Enum;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;

using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;


namespace wojilu.Apps.Forum.Service {

    public class ForumPostService : IForumPostService {

        public virtual IForumBoardService boardService { get; set; }
        public virtual IForumCategoryService categoryService { get; set; }
        public virtual IForumService forumService { get; set; }
        public virtual IForumLogService forumLogService { get; set; }
        public virtual IMessageService msgService { get; set; }
        public virtual IForumRateService rateService { get; set; }
        public virtual IForumTopicService topicService { get; set; }
        public virtual IUserService userService { get; set; }
        public virtual IAttachmentService attachmentService { get; set; }

        public virtual IUserIncomeService incomeService { get; set; }
        public virtual INotificationService notificationService { get; set; }
        public virtual IFeedService feedService { get; set; }

        public ForumPostService() {

            forumService = new ForumService();
            categoryService = new ForumCategoryService();
            boardService = new ForumBoardService();
            topicService = new ForumTopicService();
            userService = new UserService();
            forumLogService = new ForumLogService();
            rateService = new ForumRateService();
            attachmentService = new AttachmentService();
            msgService = new MessageService();

            incomeService = new UserIncomeService();

            notificationService = new NotificationService();
            feedService = new FeedService();
        }


        //--------------------------------------- income -----------------------------------------

        public virtual void AddAuthorIncome( ForumPost post, int actionId, String actionName ) {

            String msg = string.Format( "帖子被{0} <a href=\"{1}\">{2}</a>", actionName, alink.ToAppData( post ), post.Title );
            incomeService.AddIncome( post.Creator, actionId, msg );

        }

        public virtual void SubstractAuthorIncome( ForumPost post, int actionId, String actionName ) {

            String msg = string.Format( "帖子被{0} <a href=\"{1}\">{2}</a>", actionName, alink.ToAppData( post ), post.Title );
            incomeService.AddIncomeReverse( post.Creator, actionId, msg );

        }

        public virtual ForumPost GetById_ForAdmin( int id ) {
            return db.findById<ForumPost>( id );
        }

        public virtual ForumPost GetById( int id, IMember owner ) {
            ForumPost post = GetById_ForAdmin( id );
            if (post == null) return null;
            if (!((post.OwnerId == owner.Id) && post.OwnerType.Equals( owner.GetType().FullName ))) {
                return null;
            }

            if (post.Status == TopicStatus.Delete) return null;

            return post;
        }

        public virtual DataPage<ForumPost> GetPageList( int topicId, int pageSize, int memberId ) {
            if (memberId > 0) {
                return db.findPage<ForumPost>( "TopicId=" + topicId + " and Creator.Id=" + memberId + " and " + TopicStatus.GetShowCondition() + " order by Id asc", pageSize );
            }
            return db.findPage<ForumPost>( "TopicId=" + topicId + " and " + TopicStatus.GetShowCondition() + " order by Id asc", pageSize );
        }

        public virtual int GetPageCount( int topicId, int pageSize ) {
            String strCondition = "TopicId=" + topicId + " and " + TopicStatus.GetShowCondition();
            int count = ForumPost.count( strCondition );
            int page = count / pageSize;
            int imod = count % pageSize;
            return imod > 0 ? page + 1 : page;
        }

        public virtual DataPage<ForumPost> GetPageList_ForAdmin( int topicId, int pageSize ) {
            return db.findPage<ForumPost>( "TopicId=" + topicId + " order by Id asc", pageSize );
        }

        public virtual List<ForumPost> GetRecentByApp( int appId, int count ) {
            return ForumPost.find( "AppId=" + appId + " and " + TopicStatus.GetShowCondition() ).list( count );
        }

        public virtual ForumPost GetPostByTopic( int topicId ) {
            return db.find<ForumPost>( "TopicId=" + topicId + " and ParentId=0" ).first();
        }


        public virtual ForumPost GetLastPostByTopic( int topicId ) {
            return db.find<ForumPost>( "TopicId=" + topicId + " order by Id desc" ).first();
        }

        public virtual List<ForumPost> GetPostsByIds( String ids ) {
            return db.find<ForumPost>( "Id in (" + ids + ")" ).list();
        }

        public virtual DataPage<ForumPost> GetByAppAndUser( int appId, int userId, int pageSize ) {
            if (userId <= 0 || appId <= 0) return DataPage<ForumPost>.GetEmpty();
            return ForumPost.findPage( "AppId=" + appId + " and CreatorId=" + userId + " and " + TopicStatus.GetShowCondition(), pageSize );
        }

        public virtual DataPage<ForumPost> GetByUser( int userId, int pageSize ) {
            if (userId <= 0) return DataPage<ForumPost>.GetEmpty();
            return ForumPost.findPage( "CreatorId=" + userId + " and OwnerType='" + typeof( Site ).FullName + "' and " + TopicStatus.GetShowCondition(), pageSize );
        }

        public virtual List<IBinderValue> GetNewSitePost( int count ) {
            return getNewPost( -1, count, typeof( Site ) );
        }

        public virtual List<IBinderValue> GetNewBoardPost( String ids, int count ) {

            if (count <= 0) count = 10;

            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) return new List<IBinderValue>();

            String bd = " and ForumBoardId in ( " + sids + " )";

            List<ForumPost> list = db.find<ForumPost>( TopicStatus.GetShowCondition() + bd + " and OwnerType=:otype order by Id desc" )
                .set( "otype", typeof( Site ).FullName )
                .list( count );

            return populateBinderValue( list );

        }

        private List<IBinderValue> getNewPost( int boardId, int count, Type ownerType ) {

            if (count <= 0) count = 10;

            String bd = boardId > 0 ? " and ForumBoardId=" + boardId : "";

            List<ForumPost> list = db.find<ForumPost>( TopicStatus.GetShowCondition() + bd + " and OwnerType=:otype order by Id desc" )
                .set( "otype", ownerType.FullName )
                .list( count );

            return populateBinderValue( list );
        }

        private static List<IBinderValue> populateBinderValue( List<ForumPost> list ) {
            List<IBinderValue> results = new List<IBinderValue>();
            foreach (ForumPost post in list) {
                if (post.Creator == null) continue;

                IBinderValue vo = new ItemValue();
                vo.Title = post.Title;

                vo.CreatorName = post.Creator.Name;
                vo.CreatorLink = Link.ToMember( post.Creator );
                vo.CreatorPic = post.Creator.PicSmall;

                vo.Content = post.Content;
                vo.Link = alink.ToAppData( post );
                vo.Created = post.Created;

                results.Add( vo );
            }

            return results;
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

        public virtual Result Insert( ForumPost post, User creator, IMember owner, IApp app ) {

            post.AppId = app.Id;
            post.Creator = creator;
            post.CreatorUrl = creator.Url;
            post.OwnerId = owner.Id;
            post.OwnerUrl = owner.Url;
            post.OwnerType = owner.GetType().FullName;
            post.EditTime = DateTime.Now;

            Result result = db.insert( post );

            if (result.IsValid) {

                updateCount( post, creator, owner, app );

                String msg = string.Format( "回复帖子 <a href=\"{0}\">{1}</a>，得到奖励", alink.ToAppData( post ), post.Title );
                incomeService.AddIncome( creator, UserAction.Forum_ReplyTopic.Id, msg );
                addFeedInfo( post );
                addNotification( post );

            }
            return result;
        }

        private void addFeedInfo( ForumPost data ) {
            String lnkPost = alink.ToAppData( data );
            String post = string.Format( "<a href=\"{0}\">{1}</a>", lnkPost, data.Title );

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add( "post", post );
            String templateData = Json.ToString( dic );

            TemplateBundle tplBundle = TemplateBundle.GetForumPostTemplateBundle();
            feedService.publishUserAction( data.Creator, typeof( ForumPost ).FullName, tplBundle.Id, templateData, "", data.Ip );
        }

        private void addNotification( ForumPost post ) {

            ForumTopic topic = ForumTopic.findById( post.TopicId );
            if (topic == null) return;

            // 给主题的作者发送通知
            addNotificationToTopicCreator( topic, post );

            // 给父帖的作者发送通知
            addNotificationToParentCreator( topic, post );
        }


        private void addNotificationToTopicCreator( ForumTopic topic, ForumPost post ) {

            User creator = post.Creator;
            int topicReceiverId = topic.Creator.Id;

            if (topicReceiverId == creator.Id) return;

            String msg = "<a href=\"" + Link.ToMember( creator ) + "\">" + creator.Name + "</a> " + alang.get( typeof( ForumApp ), "replyYourPost" ) + " <a href=\"" + alink.ToAppData( post ) + "\">" + topic.Title + "</a>";
            notificationService.send( topicReceiverId, post.Creator.GetType().FullName, msg, NotificationType.Comment );
        }

        private void addNotificationToParentCreator( ForumTopic topic, ForumPost post ) {

            User creator = post.Creator;
            if (post.ParentId <= 0) return;

            // 如果不清理缓存，则parent.Creator就是null（受制于ORM查询的关联深度只有2级）
            ForumPost parent = db.nocache.findById<ForumPost>( post.ParentId );
            if (parent == null) return;

            User parentUser = userService.GetById( parent.Creator.Id );
            if (parentUser == null) return;

            int parentReceiverId = parent.Creator.Id;
            parentReceiverId = parent.Creator.Id;
            int topicReceiverId = topic.Creator.Id;
            if (parentReceiverId == creator.Id || parentReceiverId == topicReceiverId) return;

            String msgToParent = "<a href=\"" + Link.ToMember( creator ) + "\">" + creator.Name + "</a> " + alang.get( typeof( ForumApp ), "replyYourPost" ) + " <a href=\"" + alink.ToAppData( post ) + "\">" + topic.Title + "</a>";
            notificationService.send( parentReceiverId, typeof( User ).FullName, msgToParent, NotificationType.Comment );
        }


        public virtual Result Update( ForumPost post, User editor ) {
            post.EditTime = DateTime.Now;
            post.EditCount++;
            post.EditMemberId = editor.Id;
            return db.update( post );
        }

        private void updateCount( ForumPost post, User user, IMember owner, IApp app ) {

            ForumTopic topic = post.Topic;
            if (topic == null) {
                topic = topicService.GetById( post.TopicId, owner );
            }
            topic.Replies = topicService.CountReply( post.TopicId );
            topic.RepliedUserName = user.Name;
            topic.RepliedUserFriendUrl = user.Url;
            topic.Replied = DateTime.Now;
            topicService.UpdateReply( topic );

            ForumBoard fb = ForumBoard.findById( post.ForumBoardId );
            fb.TodayPosts++;
            fb.Posts = boardService.CountPost( fb.Id );

            LastUpdateInfo info = new LastUpdateInfo();
            info.PostId = post.Id;
            info.PostType = typeof( ForumPost ).Name;
            info.PostTitle = post.Title;
            info.CreatorName = user.Name;
            info.CreatorUrl = user.Url;
            info.UpdateTime = DateTime.Now;

            fb.LastUpdateInfo = info;
            fb.Updated = info.UpdateTime;

            boardService.Update( fb );

            ForumApp forum = app as ForumApp;
            forum.PostCount++;
            forum.TodayPostCount++;

            forum.LastUpdateMemberName = user.Name;
            forum.LastUpdateMemberUrl = user.Url;
            forum.LastUpdatePostTitle = post.Title;

            forum.LastUpdateTime = post.Created;
            forumService.Update( forum );

            userService.AddPostCount( user );
        }

        public virtual void DeleteToTrash( ForumPost post, User creator, String ip ) {
            post.Status = TopicStatus.Delete;
            post.update( "Status" );

            ForumTopic topic = topicService.GetByPost( post.Id );
            topic.Replies = topicService.CountReply( topic.Id );
            topic.update( "Replies" );

            // 积分规则中本身定义的是负值，所以此处用AddIncome
            AddAuthorIncome( post, UserAction.Forum_PostDeleted.Id, "删除" );

            forumLogService.AddPost( creator, post.AppId, post.Id, ForumLogAction.Delete, ip );
        }

        public virtual void DeleteTrue( ForumPost post, IMember owner, User user, String ip ) {
            int id = post.Id;
            int creatorId = post.Creator.Id;
            int topicId = post.TopicId;
            int forumBoardId = post.ForumBoardId;

            db.delete( post );

            attachmentService.DeleteByPost( id );
            topicService.DeletePostCount( topicId, owner );
            boardService.DeletePostCount( forumBoardId, owner );
            if (creatorId > 0) { //规避已注销用户
                userService.DeletePostCount( creatorId );
            }
            forumLogService.AddPost( user, post.AppId, id, ForumLogAction.DeleteTrue, ip );
        }



        //--------------------------------------- admin -----------------------------------------

        public virtual void AddHits( ForumPost post ) {
            //post.Hits++;
            //db.update( post, "Hits" );
            HitsJob.Add( post );
        }

        public virtual void AddReward( ForumPost post, int rewardValue ) {

            ForumTopic topic = topicService.GetByPost( post.Id );

            post.Reward = rewardValue;
            db.update( post, "Reward" );

            String msg = string.Format( "回复悬赏贴 \"<a href=\"{0}\">{1}</a>\"，作者答谢：{2}{3}", alink.ToAppData( topic ), topic.Title, rewardValue, KeyCurrency.Instance.Unit );

            incomeService.AddKeyIncome( post.Creator, rewardValue, msg );

            // 以下步骤不需要，因为发帖的时候已经被扣除了
            //incomeService.AddKeyIncome( topic.Creator, -rewardValue );

            topicService.SubstractTopicReward( topic, rewardValue );

            notificationService.send( post.Creator.Id, msg );
        }

        public virtual void SetPostCredit( ForumPost post, int currencyId, int credit, String reason, User viewer ) {
            post.Rate += credit;
            db.update( post, "Rate" );

            rateService.Insert( post.Id, viewer, currencyId, credit, reason );

            String msg = string.Format( "帖子被评分 <a href=\"{0}\">{1}</a>", alink.ToAppData( post ), post.Title );

            incomeService.AddIncome( post.Creator, currencyId, credit, msg );

            notificationService.send( post.Creator.Id, msg );
        }

        public virtual void BanPost( ForumPost post, String reason, int isSendMsg, User user, int appId, String ip ) {

            post.Status = 1;
            db.update( post, "Status" );
            if (isSendMsg == 1) {
                String msgTitle = string.Format( ForumConfig.Instance.BanMsgTitle, post.Title );
                String msgBody = string.Format( ForumConfig.Instance.BanMsgTitle, post.Title );

                msgService.SendMsg( user, post.Creator.Name, msgTitle, msgBody );
            }

            String msg = string.Format( "帖子被屏蔽 <a href=\"{0}\">{1}</a>", alink.ToAppData( post ), post.Title );
            incomeService.AddIncome( post.Creator, UserAction.Forum_PostBanned.Id, msg );

            forumLogService.AddPost( user, appId, post.Id, ForumLogAction.Ban, ip );
        }

        public virtual void UnBanPost( ForumPost post, User user, int appId, String ip ) {
            post.Status = 0;
            db.update( post, "Status" );

            String msg = string.Format( "帖子取消屏蔽 <a href=\"{0}\">{1}</a>", alink.ToAppData( post ), post.Title );
            incomeService.AddIncomeReverse( post.Creator, UserAction.Forum_PostBanned.Id, msg );
            forumLogService.AddPost( user, appId, post.Id, ForumLogAction.UnBan, ip );
        }

        public virtual DataPage<ForumPost> GetPageByApp( int appId, int pageSize ) {
            return db.findPage<ForumPost>( "AppId=" + appId + " and " + TopicStatus.GetShowCondition(), pageSize );
        }

        public virtual DataPage<ForumPost> GetDeletedPage( int appId ) {
            return db.findPage<ForumPost>( "AppId=" + appId + " and Status=" + TopicStatus.Delete );
        }

        public virtual void Restore( String choice ) {

            int[] arrId = cvt.ToIntArray( choice );
            foreach (int id in arrId) {

                ForumPost post = GetById_ForAdmin( id );

                if (post == null) continue;

                post.Status = TopicStatus.Normal;
                db.update( post, "Status" );

                if (post.ParentId == 0) {
                    ForumTopic topic = topicService.GetById_ForAdmin( post.TopicId );
                    topic.Status = TopicStatus.Normal;
                    db.update( topic, "Status" );
                }

                String msg = string.Format( "撤销删除(帖子): <a href=\"{0}\">{1}</a>", alink.ToAppData( post ), post.Title );
                incomeService.AddIncomeReverse( post.Creator, UserAction.Forum_PostDeleted.Id, msg );

            }
        }

        public virtual void DeleteListTrue( String choice, User user, String ip ) {

            int[] arrId = cvt.ToIntArray( choice );
            foreach (int id in arrId) {
                ForumPost post = GetById_ForAdmin( id );
                if (post == null) continue;

                if (post.ParentId == 0) {
                    ForumTopic topic = topicService.GetById_ForAdmin( post.TopicId );
                    topicService.DeleteTrue( topic, user, ip );
                }
                else {
                    EntityInfo ei = Entity.GetInfo( post.OwnerType );
                    IMember owner = null;
                    if (ei == null) {
                        owner = Site.Instance;
                    }
                    else {
                        owner = ndb.findById( ei.Type, post.OwnerId ) as IMember;
                    }
                    DeleteTrue( post, owner, user, ip );
                }

            }
        }


    }
}

