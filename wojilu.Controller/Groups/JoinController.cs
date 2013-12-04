/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Groups.Interface;
using wojilu.Members.Groups.Service;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Groups.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Groups {

    public class JoinController : ControllerBase {

        public virtual IGroupService groupService { get; set; }
        public virtual IMemberGroupService mgrService { get; set; }

        public JoinController() {
            groupService = new GroupService();
            mgrService = new MemberGroupService();

            HidePermission( typeof( wojilu.Web.Controller.SecurityController ) );
        }

        [Login]
        public virtual void Index() {

            target( Save );

            Group group = ctx.owner.obj as Group;

            if (group.IsCloseJoinCmd == 1) {
                showError( "对不起，成员已满，不再接受申请" );
            }
            else if (mgrService.IsGroupApproving( ctx.viewer.Id, @group.Id )) {
                showError( "正在审核，请耐心等待" );
            }
            else if (mgrService.IsGroupMember( ctx.viewer.Id, @group.Id )) {
                showError( "已经是成员，请勿重复操作" );
            }
        }

        [Login, HttpPost, DbTransaction]
        public virtual void Save() {

            String joinReason = strUtil.CutString( ctx.Post( "joinReason" ), 250 );

            Group group = ctx.owner.obj as Group;

            if (group.IsCloseJoinCmd == 1) {
                showError( "对不起，成员已满，不再接受申请" );
                return;
            }
            else if (mgrService.IsGroupApproving( ctx.viewer.Id, @group.Id )) {
                showError( "正在审核，请耐心等待" );
                return;
            }
            else if (mgrService.IsGroupMember( ctx.viewer.Id, @group.Id )) {
                showError( "已经是成员，请勿重复操作" );
                return;
            }

            if (strUtil.IsNullOrEmpty( joinReason )) {
                showError( "请填写申请理由" );
                return;
            }

            Result result = mgrService.JoinGroup( (User)ctx.viewer.obj, group, joinReason, ctx.Ip );

            if (result.IsValid) {
                echoToParent( lang( "opok" ) );
            }
            else {
                echoError( result );
            }
        }

        public virtual void Quit() {
            target( SaveQuit );
        }

        [Login, HttpPost, DbTransaction]
        public virtual void SaveQuit() {

            String quitReason = strUtil.CutString( ctx.Post( "quitReason" ), 250 );

            Group group = ctx.owner.obj as Group;

            if (mgrService.IsGroupMember( ctx.viewer.Id, @group.Id )==false) {
                showError( "不是成员，无法退出" );
                return;
            }

            Result result = mgrService.QuitGroup( (User)ctx.viewer.obj, group, quitReason );
            errors.Join( result );
            if (errors.HasErrors) {
                echoError();
            }
            else
                echoToParent( lang( "opok" ) );
        }

        [Login]
        public virtual void Invite( long id ) {

            String code = ctx.Get( "code" );

            GroupInvite gi = GroupInvite.findById( id );

            String errorInfo = checkValid( gi, code );
            if (strUtil.HasText( errorInfo )) {
                echoError( errorInfo );
                return;
            }

            // 创建用户成员
            Group group = Group.findById( gi.OwnerId );
            Result result = mgrService.JoinGroupDone( (User)ctx.viewer.obj, group, "接受" + gi.Inviter.Name + "邀请加入" , ctx.Ip);
            if (result.IsValid) {
                // 修改邀请码状态
                gi.Status = 1;
                gi.update();

                echoRedirect( lang( "opok" ), Link.ToMember( group ) );
            }
            else {
                echoError( result );
            }
        }

        private String checkValid( GroupInvite gi, String code ) {

            if (gi == null) return "邀请不存在";

            if (gi.Code.Equals( code ) == false) return "邀请不存在";

            if (ctx.viewer.Id != gi.Receiver.Id) return "邀请不存在";

            Group group = Group.findById( gi.OwnerId );
            if (group == null) return "邀请不存在";

            if (gi.Status == 1) return "邀请不存在"; // 邀请只能用一次。比如：受邀用户捣乱，管理员将其删除。此用户不能再利用旧的邀请码直接进入。

            if (mgrService.IsGroupMember( ctx.viewer.Id, @group.Id )) return "您已经是成员";

            return null;
        }


        private void showError( String msg ) {

            content( string.Format( "<div style=\"text-align:center;margin:30px 10px;color:red;font-size:16px;font-weight:bold;\">{0}</div>", msg ) );
        }

    }

}
