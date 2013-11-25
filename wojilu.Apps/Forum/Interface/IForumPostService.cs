/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Forum.Domain;
using wojilu.Common.Msg.Interface;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Money.Interface;
using wojilu.Common;
using wojilu.Common.Microblogs.Interface;

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
        IMicroblogService microblogService { get; set; }
        IUserIncomeService incomeService { get; set; }

        void AddHits( ForumPost post );
        void AddReward( ForumPost post, int rewardValue );

        ForumPost GetById(long id, IMember owner);
        ForumPost GetById_ForAdmin(long id);
        DataPage<ForumPost> GetDeletedPage(long appId);
        List<IBinderValue> GetNewSitePost( int count );
        DataPage<ForumPost> GetPageByApp(long appId, int pageSize);
        DataPage<ForumPost> GetPageList(long topicId, int pageSize, long memberId);
        DataPage<ForumPost> GetPageList_ForAdmin(long topicId, int pageSize);
        ForumPost GetPostByTopic(long topicId);
        List<ForumPost> GetPostsByIds( String ids );
        List<ForumPost> GetRecentByApp(long appId, int count);

        Result Insert( ForumPost post, User creator, IMember owner, IApp app );
        Result InsertNoNotification( ForumPost post, User creator, IMember owner, IApp app );


        void Restore( String choice );
        void SetPostCredit(ForumPost post, long currencyId, int currencyValue, string reason, User viewer);
        void UnBanPost(ForumPost post, User user, long appId, string ip);
        void BanPost(ForumPost post, string reason, int isSendMsg, User user, long appId, string ip);

        Result Update( ForumPost post, User editor );
        void DeleteListTrue( String choice, User user, String ip );
        void DeleteToTrash( ForumPost post, User creator, String ip );
        void DeleteTrue( ForumPost post, IMember owner, User user, String ip );

        DataPage<ForumPost> GetByAppAndUser(long appId, long userId, int pageSize);
        DataPage<ForumPost> GetByUser(long userId, int pageSize);

        int GetPageCount(long topicId, int pageSize);

        ForumPost GetLastPostByTopic(long topicId);




    }

}
