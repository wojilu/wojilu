using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Enum;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Interface;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Content.Submit {

    [App( typeof( ContentApp ) )]
    public class AdminController : ControllerBase {

        public IContentSectionService sectionService { get; set; }
        public IContentPostService postService { get; set; }
        public ContentTempPostService tempPostService { get; set; }
        public INotificationService ntService { get; set; }
        public IMessageService msgService { get; set; }

        public AdminController() {
            sectionService = new ContentSectionService();
            postService = new ContentPostService();
            tempPostService = new ContentTempPostService();
            ntService = new NotificationService();
            msgService = new MessageService();
        }

        public override void CheckPermission() {
            if (hasAdminPermission() == false) echo( "对不起，你没有管理权限" );
        }

        public void Index() {

            set( "OperationUrl", to( SaveAdmin ) );
            set( "submintLink", to( new PostController().Index ) );
            set( "portalHome", to( new ContentController().Index ) );

            DataPage<ContentTempPost> list = tempPostService.GetPage( ctx.owner.obj, ctx.app.Id );

            IBlock block = getBlock( "list" );
            foreach (ContentTempPost p in list.Results) {
                block.Bind( "post", p );
                block.Set( "post.UserLink", Link.ToMember( p.Creator ) );
                block.Set( "post.PassLink", to( Pass, p.Id ) );
                block.Set( "post.ShowLink", to( new PostController().Show, p.Id ) );

                block.Set( "post.NoPassLink", to( NoPass, p.Id ) );
                block.Next();
            }

            set( "page", list.PageBar );
        }

        [HttpPost, DbTransaction]
        public void SaveAdmin() {

            String ids = ctx.PostIdList( "choice" );

            if (strUtil.IsNullOrEmpty( ids )) {
                redirect( Index );
                return;
            }

            String cmd = ctx.Post( "action" );

            if ("pass" == cmd) {
                passPosts( ids );
            }
            else if ("nopass" == cmd) {
                nopassPosts( ids );
            }

            actionContent( "ok" );

        }


        private void passPosts( string ids ) {

            int[] arrId = cvt.ToIntArray( ids );
            if (arrId.Length == 0) return;
            foreach (int id in arrId) {

                ContentTempPost p = tempPostService.GetById( id );
                if (p == null) continue;

                passSinglePost( p );

            }

        }

        private void nopassPosts( string ids ) {
            tempPostService.NoPass( ids );
        }

        public void Pass( int id ) {
            target( SavePass, id );
            ContentTempPost p = tempPostService.GetById( id );
            bind( "post", p );
        }

        public void NoPass( int id ) {
            target( SaveNoPass, id );
            ContentTempPost p = tempPostService.GetById( id );
            bind( "post", p );
        }

        public void SavePass( int id ) {

            String msg = ctx.Post( "msg" );
            ContentTempPost p = tempPostService.GetById( id );
            if (p == null) {
                echoError( lang( "exDataNotFound" ) );
                return;
            }

            ContentPost post = passSinglePost( p );
            String lnk = alink.ToAppData( post );

            string title = string.Format( "你投递的 “{0}” 通过审核", p.Title );
            msg = string.Format( "谢谢您的投递，你的 “<a href=\"{0}\">{1}</a>” 已通过审核。", lnk, p.Title ) + "<br/>审核说明：" + msg;
            sendMsg( title, msg, p.Creator );

            echoToParentPart( lang( "opok" ) );
        }

        private void sendNotification( ContentPost post ) {
            String msg = string.Format( "您的投递 <a href=\"{0}\">{1}</a> 已经通过审核", alink.ToAppData( post ), post.Title );
            ntService.send( post.Creator.Id, msg );
        }

        private void sendMsg( string title, string msg, User user ) {
            msgService.SiteSend( title, msg, user );
        }

        public void SaveNoPass( int id ) {
            String msg = ctx.Post( "msg" );
            ContentTempPost p = tempPostService.GetById( id );
            if (p == null) {
                echoError( lang( "exDataNotFound" ) );
                return;
            }
            tempPostService.NoPass( p );

            string title = string.Format( "您投递的 “{0}” 没有通过审核", p.Title );
            msg = string.Format( "对不起，您投递的 “{0}” 没有通过审核", p.Title ) + "<br/>审核说明：" + msg;
            sendMsg( title, msg, p.Creator );

            echoToParentPart( lang( "opok" ) );
        }


        private bool hasAdminPermission() {
            if (ctx.viewer.IsAdministrator()) return true;
            return false;
        }


        private ContentPost passSinglePost( ContentTempPost p ) {
            ContentPost post = tempPostService.GetBySubmitPost( p, ctx.owner.obj );
            postService.Insert( post, p.TagList );

            tempPostService.Delete( p );

            sendNotification( post );
            return post;
        }

    }

}
