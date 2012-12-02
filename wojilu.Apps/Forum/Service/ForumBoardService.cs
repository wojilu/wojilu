/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Web.Utils;
using wojilu.Apps.Forum.Interface;
using wojilu.ORM;

namespace wojilu.Apps.Forum.Service {


    public class ForumBoardService : IForumBoardService {

        public virtual IForumService forumService { get; set; }

        public ForumBoardService() {
            this.forumService = new ForumService();
        }

        //--------------------------------------------------------------------------------------------------


        public virtual List<ForumBoard> GetBoardAll( int forumId, Boolean isLogin ) {
            List<ForumBoard> results = this.getBoardsByApp( forumId, isLogin );
            return results;
        }

        public virtual ForumBoard GetById( int id, IMember owner ) {
            return this.GetById( id, owner.Id, owner.GetType().FullName );
        }

        public virtual ForumBoard GetById( int id, int ownerId, String typeFullName ) {
            ForumBoard board = db.findById<ForumBoard>( id );
            if (board == null) return null;

            if (!((board.OwnerId == ownerId) && board.OwnerType.Equals( typeFullName ))) {
                return null;
            }
            return board;
        }
        
        private List<ForumBoard> getBoardsByApp( int appId ) {
            return db.find<ForumBoard>( "AppId=" + appId ).list();
        }

        private List<ForumBoard> getBoardsByApp( int appId, Boolean isLogin ) {
            if (isLogin) return db.find<ForumBoard>( "AppId=" + appId + " order by OrderId desc, Id asc" ).list();
            return db.find<ForumBoard>( "AppId=" + appId + " order by OrderId desc, Id asc" ).list();
        }

        public virtual Boolean HasChildren( int boardId ) {
            return ForumBoard.count( "ParentId=" + boardId ) > 0;
        }

        //-------------------------------------------------------------------------------------------------------


        public virtual void ClearSecurity( ForumBoard fb ) {
            fb.Security = string.Empty;
            db.update( fb, "Security" );
        }

        public virtual void Combine( ForumBoard fbSrc, ForumBoard fbTarget ) {
            String action = "set ForumBoardId=" + fbTarget.Id;
            String condition = "ForumBoardId=" + fbSrc.Id;
            db.updateBatch<ForumTopic>( action, condition );
            db.updateBatch<ForumPost>( action, condition );
            this.UpdateStats( fbSrc );
            this.UpdateStats( fbTarget );
        }

        public virtual int CountPost( int forumBoardId ) {
            return db.find<ForumPost>( "ForumBoardId=" + forumBoardId + " and " + TopicStatus.GetShowCondition() ).count();
        }

        public virtual int CountTopic( int forumBoardId ) {
            return db.find<ForumTopic>( "ForumBoardId=" + forumBoardId + " and " + TopicStatus.GetShowCondition() ).count();
        }

        //------------------------------------------------------------------------------------

        public virtual Result Insert( ForumBoard fb ) {
            return db.insert( fb );
        }

        public virtual Result Update( ForumBoard fb ) {
            return db.update( fb );
        }

        public virtual void UpdateSecurity( ForumBoard fb, String str ) {
            fb.Security = str;
            db.update( fb, "Security" );
        }

        public virtual void UpdateStats( ForumBoard forumBoard ) {
            int posts = this.CountPost( forumBoard.Id );
            int topics = this.CountTopic( forumBoard.Id );
            forumBoard.Posts = posts;
            forumBoard.Topics = topics;
            db.update( forumBoard, new string[] { "Posts", "Topics" } );
        }

        public virtual void UpdateSecurityAll( ForumApp forum ) {
            String str = forum.Security;
            List<ForumBoard> boards = getBoardsByApp( forum.Id );
            foreach (ForumBoard bd in boards) {
                UpdateSecurity( bd, str );
            }
        }

        public virtual void UpdateLastInfo( ForumBoard fb ) {

            ForumTopic topic = ForumTopic
                .find( "ForumBoardId=" + fb.Id + " and " + TopicStatus.GetShowCondition() + " order by Id desc" )
                .first();

            LastUpdateInfo info = new LastUpdateInfo();
            info.PostId = topic.Id;
            info.PostType = typeof( ForumTopic ).Name;
            info.PostTitle = topic.Title;

            User user = topic.Creator;

            info.CreatorName = user.Name;
            info.CreatorUrl = user.Url;
            info.UpdateTime = topic.Created;

            fb.LastUpdateInfo = info;
            fb.Updated = info.UpdateTime;

            db.update( fb );

        }

        //------------------------------------------------------------------------------------

        public virtual void Delete( ForumBoard fb ) {
            db.delete( fb );
            String action = "set Status=" + 3;
            String condition = "ForumBoardId=" + fb.Id;
            db.updateBatch<ForumTopic>( action, condition );
            db.updateBatch<ForumPost>( action, condition );
            action = "set ForumBoardId=0";
            condition = "ForumBoardId=" + fb.Id;
            db.updateBatch<ForumTopic>( action, condition );
            db.updateBatch<ForumPost>( action, condition );
        }

        public virtual void DeleteCategoryOnly( ForumBoard category ) {
            db.delete( category );
        }

        public virtual void DeletePostCount( int boardId, IMember owner ) {
            ForumBoard board = this.GetById( boardId, owner );
            board.Posts--;
            board.update();
        }

        public virtual void DeleteTopicCount( int boardId, int replyPosts, IMember owner ) {
            ForumBoard board = this.GetById( boardId, owner );
            board.Topics--;
            board.Posts -= replyPosts;
            board.update();
        }


        public virtual void UpdateLogo( ForumBoard board, String newLogo ) {
            board.Logo = newLogo;
            db.update( board, "Logo" );
        }


        public virtual void DeleteLogo( ForumBoard board ) {

            // 删除物理文件            
            wojilu.Drawing.Img.DeleteImgAndThumb( strUtil.Join( sys.Path.DiskPhoto, board.Logo ) );

            this.UpdateLogo( board, string.Empty );

        }

    }
}