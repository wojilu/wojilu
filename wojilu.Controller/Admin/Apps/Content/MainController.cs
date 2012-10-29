/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Common.AppBase;
using wojilu.Web.Controller.Security;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Domain;


namespace wojilu.Web.Controller.Admin.Apps.Content {

    [App( typeof( ContentApp ) )]
    public partial class MainController : ControllerBase {

        public ISysPostService postService { get; set; }
        public IAdminLogService<SiteLog> logService { get; set; }

        public MainController() {
            postService = new SysPostService();
            logService = new SiteLogService();
        }

        public void Index() {

            target( Admin );
            DataPage<ContentPost> list = postService.GetPage();
            bindPosts( list );
        }

        public void Trash() {
            target( Admin );

            DataPage<ContentPost> list = postService.GetPageTrash();
            bindPosts( list );
        }


        private void bindPosts( DataPage<ContentPost> list ) {
            IList posts = list.Results;

            set( "page", list.PageBar );

            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {

                block.Set( "post.Id", post.Id );
                block.Set( "post.Title", post.Title );
                block.Set( "post.Url", alink.ToAppData( post ) );

                block.Set( "post.Hits", post.Hits );
                block.Set( "post.ReplyCount", post.Replies );
                block.Set( "post.CreateTime", post.Created.GetDateTimeFormats( 'g' )[0] );

                String author = post.Creator == null ? "" : post.Creator.Name;

                block.Set( "post.UserName", author );
                block.Set( "post.UserLink", toUser( post.CreatorUrl ) );

                String status = getStatus( post );
                block.Set( "post.Status", status );
                block.Set( "post.UnDeleteLink", to( UnDelete, post.Id ) );

                block.Next();
            }
        }

        private String getStatus( ContentPost post ) {

            if (post.HasImg()==false) return "";

            return "<img src=\""+strUtil.Join( sys.Path.Img, "img.gif" )+"\" />";

        }


        [HttpPost, DbTransaction]
        public void Admin() {

            String ids = ctx.Post( "choice" );
            String cmd = ctx.Post( "action" );

            if (strUtil.IsNullOrEmpty( cmd ) || cvt.IsIdListValid( ids ) == false) {
                echoText( lang( "exCmd" ) );
                return;
            }


            if ("delete".Equals( cmd )) {

                postService.Delete( ids );
                log( SiteLogString.SystemDeleteContentPost(), ids );
                echoAjaxOk();


            }
            else if ("deletetrue".Equals( cmd )) {

                postService.DeleteTrue( ids );
                log( SiteLogString.SystemDeleteContentPost(), ids );
                echoAjaxOk();


            }
            else
                echoText( lang( "exUnknowCmd" ) );

        }

        [HttpPut, DbTransaction]
        public void UnDelete( int id ) {

            ContentPost post = postService.GetById_ForAdmin( id );
            if (post == null) { echoRedirect( lang( "exDataNotFound" ) ); return; }

            postService.UnDelete( post );
            log( SiteLogString.SystemUnDeleteContentPost(), post );
            redirect( Trash );
        }

        private void log( String msg, String ids ) {
            String dataInfo = "{Ids:[" + ids + "']";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( ContentPost ).FullName, ctx.Ip );
        }

        private void log( String msg, ContentPost post ) {
            String dataInfo = "{Id:" + post.Id + ", Title:'" + post.Title + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( ContentPost ).FullName, ctx.Ip );
        }


    }

}
