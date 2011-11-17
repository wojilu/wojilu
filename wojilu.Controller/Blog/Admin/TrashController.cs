/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;

using wojilu.Common.Feeds.Service;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.AppBase;
using wojilu.Common.Tags;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Service;
using wojilu.Apps.Blog.Interface;

namespace wojilu.Web.Controller.Blog.Admin {

    [App( typeof( BlogApp ) )]
    public class TrashController : ControllerBase {

        public IBlogService blogService { get; set; }
        public IBlogCategoryService categoryService { get; set; }
        public IBlogPostService postService { get; set; }

        public IFeedService feedService { get; set; }
        public IFriendService friendService { get; set; }

        public TrashController() {

            blogService = new BlogService();
            postService = new BlogPostService();
            categoryService = new BlogCategoryService();
            feedService = new FeedService();
            friendService = new FriendService();
        }


        public void Trash() {
            target( Admin );

            DataPage<BlogPost> blogpostList = postService.GetTrash( ctx.app.Id, 25 );
            bindTrashList( blogpostList );
        }



        private void bindTrashList( DataPage<BlogPost> blogpostList ) {
            IBlock block = getBlock( "list" );
            foreach (BlogPost post in blogpostList.Results) {
                block.Set( "post.CategoryName", post.Category.Name );
                block.Set( "post.CategoryUrl", to( new MyListController().ListByCategory, post.Category.Id ) );
                block.Set( "post.Id", post.Id );
                block.Set( "post.Title", post.Title );
                block.Set( "post.Created", post.Created );
                block.Next();
            }

            set( "page", blogpostList.PageBar );
        }


        [HttpPost, DbTransaction]
        public void Admin() {

            if (adminList()) {
                echoAjaxOk();
            }
            else {
                echoText( lang( "exUnknowCmd" ) );
            }
        }


        private Boolean adminList() {

            String ids = ctx.PostIdList( "choice" );
            String cmd = ctx.Post( "action" );

            if (strUtil.IsNullOrEmpty( cmd )) return false;

            int appId = ctx.app.Id;

            if (cmd.Equals( "undelete" )) {
                postService.UnDelete( ids, appId );
                return true;
            }

            if (cmd.Equals( "deletetrue" )) {
                postService.DeleteTrue( ids, appId );
                return true;
            }

            return false;
        }


    }
}
