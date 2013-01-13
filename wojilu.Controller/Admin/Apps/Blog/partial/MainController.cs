/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Blog.Service;
using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin.Apps.Blog {

    public partial class MainController : ControllerBase {

        private void bindPosts( DataPage<BlogPost> list ) {

            IList posts = list.Results;

            set( "page", list.PageBar );

            IBlock block = getBlock( "list" );
            foreach (BlogPost post in posts) {

                String sysCategoryName = getSysCategoryName( post );
                block.Set( "post.SysCategoryName", sysCategoryName );

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

                block.Set( "post.DeleteLink", to( Delete, post.Id ) );
                //block.Set( "post.UnDeleteLink", to( UnDelete, post.Id ) );

                block.Next();
            }
        }

        private string getSysCategoryName( BlogPost post ) {
            if (post.SysCategoryId == 0) return "";
            BlogSysCategory c = categoryService.GetById( post.SysCategoryId );
            if (c == null) return "";
            return c.Name;
        }

        private String getStatus( BlogPost post ) {

            if (pickedService.IsPicked( post )) return "<span class='red'>[" + lang( "recommend" ) + "]</span>&nbsp;";
            return "";
        }


        private void log( String msg, String ids ) {
            String dataInfo = "{Ids:[" + ids + "']";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( BlogPost ).FullName, ctx.Ip );
        }

        private void log( String msg, BlogPost post ) {
            String dataInfo = "{Id:" + post.Id + ", Title:'" + post.Title + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( BlogPost ).FullName, ctx.Ip );
        }

    }

}
