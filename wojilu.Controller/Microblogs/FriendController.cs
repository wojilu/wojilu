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

namespace wojilu.Web.Controller.Microblogs {

    public class FriendController : ControllerBase {

        public IFollowerService followService { get; set; }
        public IFriendService friendService { get; set; }
        public IUserService userService { get; set; }
        public IBlacklistService blacklistService { get; set; }

        public FriendController() {
            followService = new FollowerService();
            friendService = new FriendService();
            userService = new UserService();
            blacklistService = new BlacklistService();
            LayoutControllerType = typeof( MicroblogController );
        }

        public void FriendList() {

            if (ctx.viewer.HasPrivacyPermission( ctx.owner.obj, UserPermission.Friends.ToString() ) == false) {
                echo( lang( "exVisitNoPermission" ) );
                return;
            }

            set( "listName", lang( "friendList" ) );
            DataPage<User> list = friendService.GetFriendsPage( ctx.owner.Id );
            bindUsers( list.Results, "list" );
            set( "page", list.PageBar );
        }

        public void FollowingList() {

            if (ctx.viewer.HasPrivacyPermission( ctx.owner.obj, UserPermission.Friends.ToString() ) == false) {
                echo( lang( "exVisitNoPermission" ) );
                return;
            }

            view( "FriendList" );
            set( "listName", lang( "myFollowing" ) );
            DataPage<User> list = followService.GetFollowingPage( ctx.owner.Id );
            bindUsers( list.Results, "list" );
            set( "page", list.PageBar );
        }

        public void FollowerList() {

            if (ctx.viewer.HasPrivacyPermission( ctx.owner.obj, UserPermission.Friends.ToString() ) == false) {
                echo( lang( "exVisitNoPermission" ) );
                return;
            }

            view( "FriendList" );
            set( "listName", lang( "myFollowed" ) );
            DataPage<User> list = followService.GetFollowersPage( ctx.owner.Id );
            bindUsers( list.Results, "list" );
            set( "page", list.PageBar );
        }


        private void bindUsers( List<User> users, String blockName ) {

            IBlock block = getBlock( blockName );
            foreach (User user in users) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", alink.ToUserMicroblog( user ) );
                block.Next();
            }
        }




    }

}
