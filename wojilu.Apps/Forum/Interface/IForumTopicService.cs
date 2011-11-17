/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Common.Money.Service;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Money.Interface;
using wojilu.Common;

namespace wojilu.Apps.Forum.Interface {

    public interface IForumTopicService {

        IAttachmentService AttachmentService { get; set; }
        IForumBoardService boardService { get; set; }
        IForumCategoryService categoryService { get; set; }
        IForumService forumService { get; set; }
        IForumLogService logService { get; set; }
        IUserService userService { get; set; }
        IUserIncomeService incomeService { get; set; }

        ForumTopic GetByPost( int postId );
        ForumTopic GetById( int id, IMember owner );
        ForumTopic GetById_ForAdmin( int id );
        List<ForumTopic> GetByIds( String idList );

        DataPage<ForumTopic> FindPickedPage( int boardId, int pageSize );
        DataPage<ForumTopic> FindPollPage( int boardId, int pageSize );
        DataPage<ForumTopic> FindTopicPage( int boardId, int pageSize, int categoryId, String sort, String time );

        List<ForumTopic> GetByApp( int appId, int count );
        DataPage<ForumTopic> GetDeletedPage( int appId );
        List<ForumTopic> getMergedStickyList( List<ForumTopic> globalStickyList, int boardId, int page );
        List<IBinderValue> GetNewGroupTopic( int count );
        List<IBinderValue> GetNewSiteTopic( int count );
        ForumTopic GetNext( ForumTopic topic );
        DataPage<ForumTopic> GetPageByApp( int appId, int pageSize );
        ForumPost GetPostByTopic( int topicId );
        ForumTopic GetPre( ForumTopic topic );
        List<ForumTopic> GetStickyList( int boardId );
        List<ForumTopic> getSubstractStickyList( List<ForumTopic> globalStickyList, int boardId );

        int GetBoardPage( int topicId, int boardId, int pageSize );
        int GetPostPage( int postId, int topicId, int pageSize );

        void Lock( ForumTopic topic, User user, String ip );
        void UnLock( ForumTopic topic, User user, String ip );
        void Move( int targetForumId, String idList );
        void Restore( String choice );
        void SetGlobalSticky( int appId, String ids );
        void SetGloablStickyUndo( int appId, String ids );
        void StickyMoveUp( int topicId );
        void StickyMoveDown( int topicId );
        void SubstractAuthorIncome( String condition, int actionId );
        void SubstractTopicReward( ForumTopic topic, int postValue );

        void AddAuthorIncome( String condition, int actionId );
        void AddHits( ForumTopic topic );
        void AdminUpdate( String action, String condition );
        int CountReply( int topicId );

        Result CreateTopic( ForumTopic topic, User user, IMember owner, IApp app );
        Result CreateTopicOther( int forumId, String title, String content, Type dataType, User user, IMember owner, IApp app );

        Result Update( ForumTopic topic, User user, IMember owner );
        void UpdateReply( ForumTopic topic );
        void UpdateAttachments( ForumTopic topic, int attachmentCount );
        void UpdateAttachmentPermission( ForumTopic topic, int ischeck );

        void DeleteListToTrash( String choice );
        void DeleteListTrue( String choice, User user, String ip );
        void DeleteToTrash( ForumTopic topic, User creator, String ip );
        void DeleteTrue( ForumTopic topic, User user, String ip );
        void DeletePostCount( int topicId, IMember owner );

        DataPage<ForumTopic> GetByUserAndApp( int appId, int userId, int pageSize );
        DataPage<ForumTopic> GetByUser( int userId, int pageSize );
        DataPage<ForumTopic> GetPickedByApp( int appId, int count );

        List<ForumTopic> GetByAppAndReplies( int appId, int count );
        List<ForumTopic> GetByAppAndReplies( int appId, int count, int days );
        List<ForumTopic> GetByAppAndViews( int appId, int count );

        DataPage<ForumTopic> Search( int appId, string key, int pageSize );



    }

}
