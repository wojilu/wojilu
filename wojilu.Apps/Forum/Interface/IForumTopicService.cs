/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Money.Interface;
using wojilu.Common;
using wojilu.Common.Microblogs.Interface;

namespace wojilu.Apps.Forum.Interface {

    public interface IForumTopicService {

        IAttachmentService AttachmentService { get; set; }
        IForumBoardService boardService { get; set; }
        IForumCategoryService categoryService { get; set; }
        IForumService forumService { get; set; }
        IForumLogService logService { get; set; }
        IUserService userService { get; set; }
        IUserIncomeService incomeService { get; set; }
        IMicroblogService microblogService { get; set; }

        ForumTopic GetByPost(long postId);
        ForumTopic GetById(long id, IMember owner);
        ForumTopic GetById_ForAdmin(long id);
        List<ForumTopic> GetByIds( String idList );

        DataPage<ForumTopic> FindPickedPage(long boardId, int pageSize);
        DataPage<ForumTopic> FindPollPage(long boardId, int pageSize);
        DataPage<ForumTopic> FindTopicPage(long boardId, int pageSize, long categoryId, string sort, string time);

        List<ForumTopic> GetByApp(long appId, int count);
        DataPage<ForumTopic> GetDeletedPage(long appId);
        List<ForumTopic> getMergedStickyList(List<ForumTopic> globalStickyList, long boardId, int page);
        List<IBinderValue> GetNewGroupTopic( int count );
        List<IBinderValue> GetNewSiteTopic( int count );
        ForumTopic GetNext( ForumTopic topic );
        DataPage<ForumTopic> GetPageByApp(long appId, int pageSize);
        ForumPost GetPostByTopic(long topicId);
        ForumTopic GetPre( ForumTopic topic );
        List<ForumTopic> GetStickyList(long boardId);
        List<ForumTopic> getSubstractStickyList(List<ForumTopic> globalStickyList, long boardId);

        int GetBoardPage(long topicId, long boardId, int pageSize);
        int GetPostPage(long postId, long topicId, int pageSize);


        void AddAuthorIncome(string condition, long actionId, string actionName);
        void SubstractAuthorIncome(string condition, long actionId, string actionName);
        void SubstractTopicReward( ForumTopic topic, int postValue );

        void AddHits( ForumTopic topic );
        int CountReply(long topicId);

        void AddFeedInfo( ForumTopic data );

        Result CreateTopic( ForumTopic topic, User user, IMember owner, IApp app );
        Result CreateTopicOther(long forumId, string title, string content, Type dataType, User user, IMember owner, IApp app);

        Result Update( ForumTopic topic, User user, IMember owner );
        void UpdateReply( ForumTopic topic );
        void UpdateAttachments( ForumTopic topic, int attachmentCount );
        void UpdateAttachmentPermission( ForumTopic topic, int ischeck );

        void DeleteListToTrash( String choice );
        void DeleteListTrue( String choice, User user, String ip );
        void DeleteToTrash( ForumTopic topic, User creator, String ip );
        void DeleteTrue( ForumTopic topic, User user, String ip );
        void DeletePostCount(long topicId, IMember owner);

        DataPage<ForumTopic> GetByUserAndApp(long appId, long userId, int pageSize);
        DataPage<ForumTopic> GetByUser(long userId, int pageSize);
        DataPage<ForumTopic> GetPickedByApp(long appId, int count);

        List<ForumTopic> GetByAppAndReplies(long appId, int count);
        List<ForumTopic> GetByAppAndReplies(long appId, int count, int days);
        List<ForumTopic> GetByAppAndViews(long appId, int count);

        DataPage<ForumTopic> Search(long appId, string key, int pageSize);


        void Lock( ForumTopic topic, User user, String ip );
        void UnLock( ForumTopic topic, User user, String ip );
        void Restore( String choice );
        void StickyMoveUp(long topicId);
        void StickyMoveDown(long topicId);

        void AdminUpdate( String action, String condition );

        void MakeSticky( AdminValue av );
        void MakeStickyUndo( AdminValue av );

        void MakePick( AdminValue av );
        void MakePickUndo( AdminValue av );

        void MakeHighlight( string style, AdminValue av );
        void MakeHighlightUndo( AdminValue av );

        void MakeLock( AdminValue av );
        void MakeLockUndo( AdminValue av );

        void DeleteList( AdminValue av );

        void MakeCategory(long categoryId, AdminValue av);

        void MakeGlobalSticky( AdminValue adminValue );
        void MakeGloablStickyUndo( AdminValue adminValue );

        void MakeMove(long p, AdminValue adminValue);
    }

}
