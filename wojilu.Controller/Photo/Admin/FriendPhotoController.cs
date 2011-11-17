/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Interface;
using wojilu.Members.Users.Interface;


using wojilu.Members.Users.Service;
using wojilu.Apps.Photo.Service;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Photo.Admin {

    [App( typeof( PhotoApp ) )]
    public class PhotoController : ControllerBase {

        public IPhotoPostService postService { get; set; }
        public IFriendService friendService { get; set; }

        public PhotoController() {

            base.HideLayout( typeof( Photo.LayoutController ) );
            postService = new PhotoPostService();
            friendService = new FriendService();

        }


        public void Index( int friendId ) {

            int userId = ctx.viewer.Id;
            DataPage<PhotoPost> list = postService.GetFriendsPhoto( userId, friendId );

            IBlock block = getBlock( "list" );
            foreach (PhotoPost post in list.Results) {
                block.Set( "author.Name", post.Creator.Name );
                block.Set( "author.Face", post.Creator.PicSmall );
                block.Set( "author.Link", Link.ToMember( post.Creator ) );
                block.Set( "post.Title", post.Title );
                block.Set( "post.Link", alink.ToAppData( post ) );
                block.Set( "post.Created", post.Created );
                block.Set( "post.ImgUrl", post.ImgThumbUrl );
                block.Next();
            }

            set( "page", list.PageBar );

            bindFriends();
        }

        private void bindFriends() {
            List<User> friends = friendService.GetRecentActive( 30, ctx.viewer.Id );
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
