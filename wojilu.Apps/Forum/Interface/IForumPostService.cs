/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Forum.Domain;
using wojilu.Common.Money.Service;
using wojilu.Common.Msg.Interface;
using wojilu.Members.Users.Interface;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Msg.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Money.Interface;
using wojilu.Common;

namespace wojilu.Apps.Forum.Interface {

    public interface IForumPostService {

        IAttachmentService attachmentService { get; set; }
        IForumBoardService boardService { get; set; }
        IForumCategoryService categoryService { get; set; }
        IForumLogService forumLogService { get; set; }
        IForumService forumService { get; set; }
        IMessageService msgService { get; set; }
        IForumRateService rateService { get; set; }
        IForumTopicService topicService { get; set; }
        IUserService userService { get; set; }

        INotificationService notificationService { get; set; }
        IFeedService feedService { get; set; }
        IUserIncomeService incomeService { get; set; }

        void AddHits( ForumPost post );
        void AddReward( ForumPost post, int rewardValue );

        ForumPost GetById( int id, IMember owner );
        ForumPost GetById_ForAdmin( int id );
        DataPage<ForumPost> GetDeletedPage( int appId );
        List<IBinderValue> GetNewSitePost( int count );
        DataPage<ForumPost> GetPageByApp( int appId, int pageSize );
        DataPage<ForumPost> GetPageList( int topicId, int pageSize, int memberId );
        DataPage<ForumPost> GetPageList_ForAdmin( int topicId, int pageSize );
        ForumPost GetPostByTopic( int topicId );
        List<ForumPost> GetPostsByIds( String ids );
        List<ForumPost> GetRecentByApp( int appId, int count );

        Result Insert( ForumPost post, User creator, IMember owner, IApp app );
        void Restore( String choice );
        void SetPostCredit( ForumPost post, int currencyId, int currencyValue, String reason, User viewer );
        void UnBanPost( ForumPost post, User user, int appId, String ip );
        void BanPost( ForumPost post, String reason, int isSendMsg, User user, int appId, String ip );

        Result Update( ForumPost post, User editor );
        void DeleteListTrue( String choice, User user, String ip );
        void DeleteToTrash( ForumPost post, User creator, String ip );
        void DeleteTrue( ForumPost post, IMember owner, User user, String ip );

        DataPage<ForumPost> GetByAppAndUser( int appId, int userId, int pageSize );
        DataPage<ForumPost> GetByUser( int userId, int pageSize );

        int GetPageCount( int topicId, int pageSize );

        ForumPost GetLastPostByTopic( int topicId );




    }

}
