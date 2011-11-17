/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Service;
using wojilu.Apps.Blog.Interface;

namespace wojilu.Web.Controller.Blog {

    [App( typeof( BlogApp ) )]
    public partial class PostController : ControllerBase {

        public IBlogPostService postService { get; set; }

        public PostController() {
            postService = new BlogPostService();
        }

        public void Show( int id ) {

            BlogPost post = postService.GetById( id, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            String saveStatusInfo = string.Empty;
            if (post.SaveStatus == 1) {
                saveStatusInfo = "<span class='warning border'>" + alang( "blog_draft_info" ) + "</span>";
                if (ctx.viewer.Id != ctx.owner.obj.Id) {
                    echoRedirect( alang( "blog_error_status" ) );
                    return;
                }
            }

            postService.AddHits( post );

            bindBlogPost( post, saveStatusInfo );
            bindComment( post );
            bindVisitor( post );
        }


    }
}

