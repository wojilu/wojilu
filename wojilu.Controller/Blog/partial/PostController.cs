/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Controller.Common;
using wojilu.Apps.Blog.Domain;

namespace wojilu.Web.Controller.Blog {

    public partial class PostController : ControllerBase {

        private void bindBlogPost( BlogPost post, String saveStatusInfo ) {

            ctx.Page.Title = post.Title;
            ctx.Page.Keywords = post.Tag.TextString;
            ctx.Page.Description = strUtil.ParseHtml( post.Content, 100 );

            set( "blog.SaveStatus", saveStatusInfo );
            set( "blog.Title", post.Title );
            set( "blog.Body", post.Content );
            set( "blog.Author", post.Creator.Name );
            set( "blog.CreateTime", post.Created );
            set( "blog.Hits", post.Hits );

            String comments = post.Replies > 0 ? lang( "comment" ) + ":" + post.Replies : "";
            set( "blog.Replies", comments );

            set( "blog.CategoryName", post.Category.Name );
            set( "blog.CategoryLink", to( new CategoryController().Show, post.Category.Id ) );


            String tags = post.Tag.List.Count > 0 ? "tag:" + post.Tag.HtmlString : "";
            set( "blog.TagList", tags );

        }

        private void bindVisitor( BlogPost post ) {
            ctx.SetItem( "visitor", new BlogPostVisitor() );
            ctx.SetItem( "visitTarget", post );
            load( "visitorList", new VisitorController().List );
        }


        private string getCommentUrl( BlogPost post ) {

            BlogApp app = ctx.app.obj as BlogApp;

            if (app.GetSettingsObj().AllowComment == 0) {

                return "#";
            }

            return t2( new wojilu.Web.Controller.Open.CommentController().List )
                + "?url=" + alink.ToAppData( post, ctx )
                + "&dataType=" + typeof( BlogPost ).FullName
                + "&dataTitle=" + post.Title
                + "&dataUserId=" + post.Creator.Id
                + "&dataId=" + post.Id
                + "&appId=" + post.AppId;
        }


    }
}

