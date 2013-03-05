/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Service;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Blog.Interface;

namespace wojilu.Web.Controller.Blog {

    public partial class BlogController : ControllerBase {

        private void bindTopPosts( BlogSetting s, IBlock block ) {
            List<BlogPost> top = postService.GetTop( ctx.app.Id, s.StickyCount );
            foreach (BlogPost post in top) {
                bindPostOne( block, post, s );
                block.Next();
            }
        }

        private void bindPosts( DataPage<BlogPost> results, BlogSetting s ) {
            IBlock listBlock = getBlock( "bloglist" );
            foreach (BlogPost post in results.Results) {
                bindPostOne( listBlock, post, s );
                listBlock.Next();
            }
        }

        private void bindPostOne( IBlock listBlock, BlogPost post, BlogSetting s ) {

            String status = string.Empty;
            if (post.IsTop == 1) status = "<span class=\"lblTop\">[" + lang( "sticky" ) + "]</span>";
            if (post.IsPick == 1) status = status + "<span class=\"lblTop\">[" + lang( "picked" ) + "]</span>";
            if (post.AttachmentCount > 0) {
                status = status + string.Format( "<span><img src=\"{0}\"/></span>", strUtil.Join( sys.Path.Img, "attachment.gif" ) );
            }

            String postLink = alink.ToAppData( post );

            listBlock.Set( "blogpost.Status", status );
            listBlock.Set( "blogpost.Title", post.Title );
            listBlock.Set( "blogpost.Url", postLink );

            String body = s.ListMode == BlogListMode.Full ? post.Content : strUtil.ParseHtml( post.Content, s.ListAbstractLength );
            listBlock.Set( "blogpost.Body", body );
            listBlock.Set( "author", ctx.owner.obj.Name );
            listBlock.Set( "authroUrl", Link.ToMember( ctx.owner.obj ) );
            listBlock.Set( "blogpost.CreateTime", post.Created.ToShortTimeString() );
            listBlock.Set( "blogpost.CreateDate", post.Created.ToShortDateString() );
            listBlock.Set( "blogpost.Hits", post.Hits );

            String replies = post.Replies > 0 ?
                string.Format( "<a href=\"{0}\">{1}(<span class=\"blogItemReviews\">{2}</span>)</a>", postLink + "#comments", lang( "comment" ), post.Replies ) :
                string.Format( "<a href=\"{0}\">∑¢±Ì∆¿¬€</a>", postLink + "#comments" );
            listBlock.Set( "blogpost.ReplyCount", replies );


            listBlock.Set( "blogpost.CategoryName", post.Category.Name );
            listBlock.Set( "blogpost.CategoryLink", to( new CategoryController().Show, post.Category.Id ) );

            String tags = post.Tag.List.Count > 0 ? "tag:" + post.Tag.HtmlString : "";
            listBlock.Set( "blogpost.TagList", tags );
        }

    }
}

