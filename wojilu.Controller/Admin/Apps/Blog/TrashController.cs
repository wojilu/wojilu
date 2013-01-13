/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Sites.Service;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Apps.Blog.Service;
using wojilu.Web.Controller.Security;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin.Apps.Blog {

    [App( typeof( BlogApp ) )]
    public class TrashController : ControllerBase {

        public IBlogPostService postService { get; set; }
        public IPickedService pickedService { get; set; }
        public ISysBlogService sysblogService { get; set; }
        public IAdminLogService<SiteLog> logService { get; set; }
        public IBlogSysCategoryService categoryService { get; set; }

        public TrashController() {
            postService = new BlogPostService();
            pickedService = new PickedService();
            sysblogService = new SysBlogService();
            logService = new SiteLogService();
            categoryService = new BlogSysCategoryService();
        }


        public void Trash() {
            target( Admin );

            DataPage<BlogPost> list = sysblogService.GetSysPageTrash();
            bindPosts( list );
        }


        private void bindPosts( DataPage<BlogPost> list ) {

            IList posts = list.Results;

            set( "page", list.PageBar );

            IBlock block = getBlock( "list" );
            foreach (BlogPost post in posts) {

                block.Set( "post.Id", post.Id );
                block.Set( "post.Title", post.Title );
                block.Set( "post.Url", alink.ToAppData( post ) );

                block.Set( "post.Hits", post.Hits );
                block.Set( "post.ReplyCount", post.Replies );
                block.Set( "post.CreateTime", post.Created.GetDateTimeFormats( 'g' )[0] );

                String author = post.Creator == null ? "" : post.Creator.Name;

                block.Set( "post.UserName", author );
                block.Set( "post.UserLink", toUser( post.CreatorUrl ) );
                block.Set( "post.UnDeleteLink", to( UnDelete, post.Id ) );

                block.Next();
            }
        }



        [HttpPost, DbTransaction]
        public void Admin() {

            String ids = ctx.PostIdList( "choice" );
            String cmd = ctx.Post( "action" );
            int categoryId = ctx.PostInt( "categoryId" );

            String condition = string.Format( "Id in ({0}) ", ids );

            if (strUtil.IsNullOrEmpty( cmd )) {
                echoText( lang( "exCmd" ) );
                return;
            }

            if (strUtil.IsNullOrEmpty( ids )) {
                echoText( lang( "plsSelect" ) );
                return;
            }


            if ("undelete".Equals( cmd )) {
                sysblogService.UnDelete( ids );
                log( SiteLogString.UnDeleteBlogPost(), ids );
                echoAjaxOk();
            }
            else if ("deletetrue".Equals( cmd )) {
                sysblogService.DeleteTrue( ids );
                log( SiteLogString.DeleteBlogPostTrue(), ids );
                echoAjaxOk();
            }

            else
                echoText( lang( "exUnknowCmd" ) );

        }

        private void log( String msg, String ids ) {
            String dataInfo = "{Ids:[" + ids + "']";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( BlogPost ).FullName, ctx.Ip );
        }

        private void log( String msg, BlogPost post ) {
            String dataInfo = "{Id:" + post.Id + ", Title:'" + post.Title + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( BlogPost ).FullName, ctx.Ip );
        }


        [HttpPut, DbTransaction]
        public void UnDelete( int id ) {

            BlogPost post = postService.GetById_ForAdmin( id );
            if (post == null) { echoRedirect( lang( "exDataNotFound" ) ); return; }

            sysblogService.SystemUnDelete( post );
            log( SiteLogString.SystemUnDeleteBlogPost(), post );
            redirect( Trash );
        }

    }
}
