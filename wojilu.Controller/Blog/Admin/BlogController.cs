/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Blog.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Members.Users.Interface;
using wojilu.Apps.Blog.Service;
using wojilu.Members.Users.Service;

namespace wojilu.Web.Controller.Blog.Admin {

    [App( typeof( BlogApp ) )]
    public class BlogController : ControllerBase {

        public IBlogPostService postService { get; set; }
        public IFriendService friendService { get; set; }

        public BlogController() {

            postService = new BlogPostService();
            friendService = new FriendService();

        }

        public override void Layout() {
            List<User> friends = friendService.GetRecentActive( 30, ctx.viewer.Id );
            bindFriends( friends );
        }

        public void Index() {
            redirect( new MyListController().My );
        }

        public void Friends( int friendId ) {

            int userId = ctx.viewer.Id;
            DataPage<BlogPost> list = postService.GetFriendsBlog( userId, friendId );

            bindFriendPosts( list );
        }

        private void bindFriendPosts( DataPage<BlogPost> list ) {
            IBlock block = getBlock( "list" );
            foreach (BlogPost post in list.Results) {
                block.Set( "author.Name", post.Creator.Name );
                block.Set( "author.Face", post.Creator.PicSmall );
                block.Set( "author.Link", toUser( post.Creator ) );
                block.Set( "post.Title", post.Title );
                block.Set( "post.Link", alink.ToAppData( post ) );
                block.Set( "post.Body", strUtil.ParseHtml( post.Content, 100 ) );
                block.Set( "post.Created", post.Created );
                block.Next();
            }

            set( "page", list.PageBar );
        }

        private void bindFriends( List<User> friends ) {
            IBlock block = getBlock( "friends" );
            foreach (User user in friends) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.BlogLink", to( Friends, user.Id ) );
                block.Next();
            }
        }

    }

}
