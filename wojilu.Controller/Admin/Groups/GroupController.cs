/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Members.Groups.Service;
using wojilu.Members.Groups.Domain;
using wojilu.Common.Msg.Service;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Groups.Interface;
using wojilu.Common.Msg.Interface;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Sites.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Web.Controller.Security;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Admin.Groups {

    public partial class GroupController : ControllerBase {

        public virtual IGroupPostService postService { get; set; }
        public virtual IGroupService groupService { get; set; }
        public virtual IMessageService msgService { get; set; }
        public virtual IMemberGroupService mgrService { get; set; }
        public virtual IUserService userService { get; set; }
        public virtual IAdminLogService<SiteLog> logService { get; set; }
        public virtual IForumTopicService topicService { get; set; }
        public virtual IForumPostService fpostService { get; set; }

        public GroupController() {
            postService = new GroupPostService();
            groupService = new GroupService();
            topicService = new ForumTopicService();
            msgService = new MessageService();
            mgrService = new MemberGroupService();
            userService = new UserService();
            logService = new SiteLogService();
            fpostService = new ForumPostService();
        }

        public virtual void Index() {

            // 左侧：最新讨论帖子
            List<ForumPost> recentPots = postService.GetRecent( 50 );
            IBlock pblock = getBlock( "posts" );
            bindPosts( recentPots, pblock );

            set( "postLink", to( PostAdmin ) );

            // 右侧上：新创建的群组
            List<Group> groups = groupService.AdminGetRecent( 10 );
            bindGroups( groups );

            set( "groupLink", to( GroupAdmin, -1 ) );
        }

        //----------------------------------------------------------------------------------

        public virtual void PostAdmin() {
            DataPage<ForumPost> posts = getPosts();
            IBlock pblock = getBlock( "posts" );
            bindPosts( posts.Results, pblock );
            set( "page", posts.PageBar );
            set( "SearchAction", to( PostAdmin ) );
        }

        public virtual void GroupAdmin( long id ) {

            set( "SearchAction", to( GroupAdmin, -1 ) );
            set( "categoryLink", to( GroupAdmin, 999 ) );

            int typeId = getTypeId();
            DataPage<Group> groups = getGroups( id, typeId );
            bindGroups( groups.Results );
            set( "page", groups.PageBar );

            GroupCategory c = db.findById<GroupCategory>( id );
            String categoryName = (c == null ? "" : c.Name);
            set( "categoryName", categoryName );

            List<GroupCategory> categories = new List<GroupCategory>();
            categories.Add( new GroupCategory( 0, 0, lang( "all" ) ) );
            foreach (GroupCategory gc in GroupCategory.GetAll()) categories.Add( gc );

            bindGroupFilter( id, categories );
        }


        //----------------------------------------------------------------------------------

        public virtual void SendMsg( long id ) {
            target( SaveMsg, id );
            Group g = groupService.GetById( id );
        }

        [HttpPost, DbTransaction]
        public virtual void SaveMsg( long id ) {

            String msg = ctx.Post( "Content" );

            if (strUtil.IsNullOrEmpty( msg )) {
                errors.Add( lang( "exRequireContent" ) );
                run( SendMsg, id );
                return;
            }

            sendMsg( id, msg, lang( "groupNotice" ) );
            log( SiteLogString.SendMsgToGroupAdministrator(), "{Id:"+id+", Msg:'"+msg+"'}" );

            echoToParentPart( lang( "sentok" ) );
        }

        private void sendMsg( long id, String msg, String type ) {
            String title = type + ":" + strUtil.ParseHtml( msg, 20 );
            List<User> officers = mgrService.GetOfficer( id );
            foreach (User user in officers) {
                msgService.SiteSend( title, msg, user );
            }
        }

        public virtual void Lock( long id ) {
            target( SaveLock, id );
        }

        [HttpPost, DbTransaction]
        public virtual void SaveLock( long id ) {

            String msg = ctx.Post( "Content" );

            if (strUtil.IsNullOrEmpty( msg )) {
                errors.Add( lang( "exRequireContent" ) );
                run( Lock, id );
                return;
            }

            Group g = groupService.GetById( id );
            String cmdName = (g.IsLock == 1 ? lang( "unlock" ) : lang( "lock" ));

            groupService.Lock( g );
            log( SiteLogString.LockGroup(), g );


            String msgf = lang( "groupSysAdminMsg" );
            sendMsg( id, msg, string.Format( msgf, g.Name, cmdName ) );

            echoToParentPart( cmdName + lang( "ok" ) );
        }

        public virtual void Hide( long id ) {
            target( SaveHide, id );
        }

        [HttpPost, DbTransaction]
        public virtual void SaveHide( long id ) {

            String msg = ctx.Post( "Content" );

            if (strUtil.IsNullOrEmpty( msg )) {
                errors.Add( lang( "exRequireContent" ) );
                run( Hide, id );
                return;
            }

            Group g = groupService.GetById( id );
            String cmdName = (g.IsSystemHide == 1 ? lang( "unhide" ) : lang( "hide" ));
            groupService.SystemHide( g );
            log( SiteLogString.HideGroup(), g );


            String msgf = lang( "groupSysAdminMsg" );
            sendMsg( id, msg, string.Format( msgf, g.Name, cmdName ) );

            echoToParentPart( cmdName + lang( "ok" ) );
        }

        // TODO 连带删除下属成员关系、app等
        [HttpDelete, DbTransaction]
        public virtual void Delete( long id ) {

            Group g = groupService.GetById( id );
            groupService.Delete( id );
            log( SiteLogString.DeleteGroup(), g );

            echoRedirect( lang( "deleted" ) );
        }

    }

}
