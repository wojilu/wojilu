/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Domain;
using wojilu.Common.Msg.Interface;

namespace wojilu.Web.Controller.Users {

    public class FeedbackController : ControllerBase {

        public virtual IFeedbackService feedbackService { get; set; }
        public virtual IBlacklistService blacklistService { get; set; }

        public FeedbackController() {
            feedbackService = new FeedbackService();
            blacklistService = new BlacklistService();
        }

        public override void Layout() {
            load( "userMenu", new Users.ProfileController().UserMenu );
            set( "user.Name", ctx.owner.obj.Name );
        }

        [HttpPost]
        public virtual void Create() {

            checkFeedbackPermission();
            if (ctx.HasErrors) {
                echoError();
                return;
            }

            Feedback f = validate( 0 );
            if (ctx.HasErrors) {
                echoError();
                return;
            }

            if (f.IsPrivate == 1) {
                sendMsg( f );

                String postContent = "<div style=\"color:red;margin-left:10px;\">说明：私信发送成功！此处不会公开显示。</div>";
                Dictionary<String, Object> dic = new Dictionary<string, object>();
                dic.Add( "IsValid", true );
                dic.Add( "Info", "formResult" );
                dic.Add( "Msg", postContent );

                echoJson( dic );

                return;
            }
            else {

                feedbackService.Insert( f, t2( List ) );

                List<Feedback> flist = new List<Feedback>();
                flist.Add( f );
                ctx.SetItem( "feedbackList", flist );

                String postContent = loadHtml( bindList );

                Dictionary<String, Object> dic = new Dictionary<string, object>();
                dic.Add( "IsValid", true );
                dic.Add( "Info", "formResult" );
                dic.Add( "Msg", postContent );

                echoJson( dic );

            }
        }

        public virtual void Reply( long id ) {

            checkFeedbackPermission();
            if (ctx.HasErrors) {
                echo( errors.ErrorsText );
                return;
            }

            Feedback parent = feedbackService.GetById( id );
            if (parent.Target.Id != ctx.owner.Id) {
                echo( lang( "exCommentTarget" ) );
                return;
            }

            set( "maxCount", Feedback.ContentLength );
            target( SaveReply, id );
        }

        [HttpPost, Login]
        public virtual void SaveReply( long id ) {

            checkFeedbackPermission();
            if (ctx.HasErrors) {
                echoError();
                return;
            }

            Feedback parent = feedbackService.GetById( id );
            if (parent.Target.Id != ctx.owner.Id) {
                echoError( lang( "exCommentTarget" ) );
                return;
            }

            Feedback f = validate( id );
            if (ctx.HasErrors) {
                echoError();
                return;
            }

            feedbackService.Reply( parent, f, t2( List ) );

            echoToParent( lang( "opok" ), t2( List ) );

        }

        private void sendMsg( Feedback f ) {
            String title = lang( "feedbackPrivate" ) + ": " + strUtil.CutString( f.Content, 20 );
            ctx.viewer.SendMsg( ctx.owner.obj.Name, title, f.Content );
        }

        public virtual void List() {

            if (hasAdminPermission()) redirect( AdminList );

            ctx.Page.Title = lang( "feedback" );

            set( "ActionLink", t2( Create ) );
            String pwTip = string.Format( lang( "pwTip" ), Feedback.ContentLength );
            set( "pwTip", pwTip );


            DataPage<Feedback> list = feedbackService.GetPageList( ctx.owner.Id );
            set( "page", list.PageBar );
            ctx.SetItem( "feedbackList", list.Results );
            load( "feedbackList", bindList );
        }

        [Login]
        public virtual void AdminList() {

            if (!hasAdminPermission()) {
                echoRedirect( lang( "exNoPermission" ), List );
                return;
            }

            DataPage<Feedback> list = feedbackService.GetPageList( ctx.owner.Id );
            set( "page", list.PageBar );
            IBlock block = getBlock( "list" );
            foreach (Feedback f in list.Results) {

                bindItem( block, f );
                block.Set( "f.DeleteLink", t2( new FeedbackController().Delete, f.Id ) );
                block.Next();
            }
        }

        [HttpDelete, Login]
        public virtual void Delete( long id ) {

            if (!hasAdminPermission()) {
                echo( lang( "exNoPermission" ) );
                return;
            }


            Feedback f = feedbackService.GetById( id );
            if (f.Target.Id != ctx.owner.Id) { echoRedirect( lang( "exDataNotFound" ) ); return; }

            feedbackService.Delete( f );
            redirect( AdminList );
        }

        //--------------------------------------------------------------------------------------------

        private void checkFeedbackPermission() {

            if (ctx.viewer.IsLogin == false) {
                errors.Add( lang( "exPlsLogin" ) );
                return;
            }

            if (blacklistService.IsBlack( ctx.owner.Id, ctx.viewer.Id )) {
                errors.Add( lang( "exCantFeedback" ) );
                return;
            }

            User owner = ctx.owner.obj as User;
            if (ctx.viewer.HasPrivacyPermission( owner, UserPermission.Feedback.ToString() ) == false) {
                errors.Add( lang( "exCantFeedback" ) );
                return;
            }
        }

        private Boolean hasAdminPermission() {
            return ctx.viewer.Id == ctx.owner.Id && ctx.owner.obj is User;
        }

        private Feedback validate( long parentId ) {

            Feedback f = new Feedback();
            f.Creator = (User)ctx.viewer.obj;
            f.Target = ctx.owner.obj as User;
            f.Content = ctx.Post( "Content" );
            if (strUtil.HasText( f.Content )) f.Content = strUtil.CutString( f.Content, Feedback.ContentLength );

            f.Ip = ctx.Ip;
            f.Created = DateTime.Now;
            f.IsPrivate = ctx.PostIsCheck( "chkPrivate" );
            f.ParentId = parentId;

            if (strUtil.IsNullOrEmpty( f.Content )) errors.Add( lang( "exContent" ) );

            return f;
        }

        [NonVisit]
        public virtual void bindList() {
            List<Feedback> list = ctx.GetItem( "feedbackList" ) as List<Feedback>;
            IBlock block = getBlock( "list" );
            foreach (Feedback f in list) {
                bindItem( block, f );
                block.Next();
            }
        }

        private void bindItem( IBlock block, Feedback f ) {
            block.Set( "f.UserName", f.Creator.Name );
            block.Set( "f.UserFace", f.Creator.PicSmall );
            block.Set( "f.UserLink", toUser( f.Creator ) );
            block.Set( "f.ReplyLink", t2( new FeedbackController().Reply, f.Id ) );
            block.Set( "f.Content", f.GetContent() );
            block.Set( "f.Created", cvt.ToTimeString( f.Created ) );
        }

    }

}
