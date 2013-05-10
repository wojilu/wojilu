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
using wojilu.Web.Controller.Content.Htmls;

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
                block.Set( "post.UserLink", toUser( p.Creator ) );
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

            content( "ok" );

        }


        private void passPosts( string ids ) {

            int[] arrId = cvt.ToIntArray( ids );
            if (arrId.Length == 0) return;

            List<ContentPost> xList = new List<ContentPost>();
            foreach (int id in arrId) {

                ContentTempPost p = tempPostService.GetById( id );
                if (p == null) continue;

                ContentPost x = passSinglePost( p );
                xList.Add( x );
            }

            HtmlHelper.SetPostListToContext( ctx, xList );
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

            HtmlHelper.SetPostToContext( ctx, post );

            echoToParentPart( lang( "opok" ) );
        }


        public void SaveNoPass( int id ) {
            String desc = ctx.Post( "msg" );
            ContentTempPost p = tempPostService.GetById( id );
            if (p == null) {
                echoError( lang( "exDataNotFound" ) );
                return;
            }
            tempPostService.NoPass( p );

            sendNoPassMsg( desc, p );

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

            sendPassNotification( post );
            return post;
        }

        private void sendPassNotification( ContentPost post ) {
            String msg = string.Format( "您的投递 <a href=\"{0}\">{1}</a> 已经通过审核", alink.ToAppData( post ), post.Title );
            ntService.send( post.Creator.Id, msg );
        }

        private void sendNoPassMsg( String desc, ContentTempPost p ) {
            string title = string.Format( "您投递的 “{0}” 没有通过审核", p.Title );
            String msg = string.Format( "对不起，您投递的 “{0}” 没有通过审核", p.Title ) + "<br/>审核说明：" + desc;
            msgService.SiteSend( title, msg, p.Creator );
        }

    }

}
