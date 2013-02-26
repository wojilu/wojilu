/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Content.Domain;

using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;

using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class SubmitSettingController : ControllerBase {

        public IUserService userService { get; set; }
        public IMessageService msgService { get; set; }

        public SubmitSettingController() {
            userService = new UserService();
            msgService = new MessageService();
        }

        public override void Layout() {
            set( "userListLink", to( List ) );
            set( "editRoleLink", to( EditRole ) );
        }

        // 记者和高级记者一览
        public void List() {

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSubmitterRole roles = app.GetSubmitterRoleObj();

            set( "addUrl", to( Add ) );

            DataPage<ContentSubmitter> list = ContentSubmitter.findPage( "AppId=" + ctx.app.Id );
            IBlock block = getBlock( "list" );
            foreach (ContentSubmitter s in list.Results) {

                block.Bind( "s", s );
                block.Set( "s.RoleName", roles.getName( s.RoleId ) );
                block.Set( "s.UserLink", toUser( s.User ) );
                block.Set( "s.DeleteLink", to( DeleteUser, s.Id ) );


                block.Next();
            }

            set( "page", list.PageBar );
        }

        // 添加正式记者
        public void Add() {
            target( CreateSubmitter );
        }

        [HttpPost]
        public void CreateSubmitter() {

            String name = strUtil.SqlClean( ctx.Post( "Name" ), 20 );
            if (strUtil.IsNullOrEmpty( name )) {
                echoError( "请填写用户名" );
                return;
            }

            User user = userService.GetByName( name );
            if (user == null) {
                echoError( "用户不存在" );
                return;
            }

            ContentSubmitter s = new ContentSubmitter();
            s.AppId = ctx.app.Id;
            s.User = user;
            s.RoleId = ContentSubmitterRole.SubmitterValue;

            s.insert();

            msgService.SiteSend( "恭喜您，您已成为本站记者", user.Name + "：<br/><br/>您好！您已被加为本站记者，可以直接投递，不用审核。<br/><br/>感谢您的参与。<br/><br/>" + config.Instance.Site.SiteName+ " " + DateTime.Now.ToShortDateString(), user );

            echoToParentPart( lang( "opok" ) );

        }

        [HttpDelete]
        public void DeleteUser( int id ) {

            ContentSubmitter s = ContentSubmitter.findById( id );
            if (s == null) {
                echoError( lang( "exDataNotFound" ) );
                return;
            }

            s.delete();

            redirect( List );

        }

        // 见习记者一览
        public void ListGuest() {
        }


        public void EditRole() {

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSubmitterRole roles = app.GetSubmitterRoleObj();

            //set( "r.NeedApproval", roles.NeedApproval );
            //set( "r.Submitter", roles.Submitter );
            //set( "r.AdvancedSubmitter", roles.AdvancedSubmitter );
            //set( "r.Editor", roles.Editor );

            bind( "r", roles );

            target( SaveRole );

        }

        public void SaveRole() {

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSubmitterRole roles = ctx.PostValue( app.GetSubmitterRoleObj() ) as ContentSubmitterRole;

            app.SubmitterRole = Json.ToString( roles );

            app.update( "SubmitterRole" );

            echoRedirect( lang( "opok" ) );

        }

    }

}
