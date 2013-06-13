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
using wojilu.Common;
using wojilu.Web.Context;

namespace wojilu.Web.Controller.Users {

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
        }

        public override void CheckPermission() {
            Boolean isFriendClose = Component.IsClose( typeof( FriendApp ) );
            if (isFriendClose) {
                echo( "对不起，本功能已经停用" );
            }
        }

        //---------------------------------------------------------------------------------------------------------

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
                block.Set( "user.Link", toUser( user ) );
                block.Next();
            }
        }

        //------------------------- friend --------------------------------------------

        [Login]
        public void AddFriend( int targetId ) {

            Result result = friendService.CanAddFriend( ctx.viewer.Id, targetId );
            if (result.HasErrors) {
                echo( result.ErrorsText );
                return;
            }

            target( SaveFriend, targetId );
        }

        [HttpPost, DbTransaction]
        public void SaveFriend( int targetId ) {

            Result result = ctx.viewer.AddFriend( targetId, strUtil.CutString( ctx.Post( "Msg" ), 100 ) );
            if (result.HasErrors) {
                echoError( result );
            }
            else {
                echoToParent( lang( "opok" ) );
            }
        }

        [HttpDelete, DbTransaction]
        public void DeleteFriend( int targetId ) {
            friendService.DeleteFriend( ctx.viewer.Id, targetId );
            echoRedirect( lang( "opok" ) );
        }

        [HttpDelete, DbTransaction]
        public void CancelAddFriend( int targetId ) {
            friendService.CancelAddFriend( ctx.viewer.Id, targetId );
            echoRedirect( lang( "opok" ) );
        }

        //----------------------- follow ----------------------------------------------

        [Login]
        public void AddFollow( int targetId ) {

            User f = userService.GetById( targetId );
            if (f == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            checkFollowPermission( targetId );
            if (ctx.HasErrors) {
                echo( errors.ErrorsText );
                return;
            }
            
            target( Follow, targetId );
        }

        [HttpPost, DbTransaction]
        public void Follow( int targetId ) {

            checkFollowPermission( targetId );
            if (ctx.HasErrors) {
                echo( errors.ErrorsText );
                return;
            }

            followService.FollowWithFeedNotification( ctx.viewer.Id, targetId );

            echoToParent( lang( "opok" ) );
        }
        
        [HttpDelete, DbTransaction]
        public void DeleteFollow( int targetId ) {

            if (!ctx.viewer.IsLogin) {
                echo( lang( "exPlsLogin" ) );
                return;
            }

            if (ctx.viewer.Id == targetId) {
                echo( lang( "exop" ) );
                return;
            }

            followService.DeleteFollow( ctx.viewer.Id, targetId );

            echoRedirect( lang( "opok" ) );
        }

        private void checkFollowPermission( int targetId ) {
            if (!ctx.viewer.IsLogin)
                errors.Add( lang( "exPlsLogin" ) );
            else if (ctx.viewer.Id == targetId)
                errors.Add( lang( "exFollowSelf" ) );
            else if (blacklistService.IsBlack( ctx.owner.Id, ctx.viewer.Id ))
                errors.Add( lang( "blackFollow" ) );
            else if (ctx.viewer.IsFriend( targetId ))
                errors.Add( lang( "exFriendBeen" ) );

            else if (followService.IsFollowing( ctx.viewer.Id, targetId ))
                errors.Add( lang( "exFollowed" ) );
        }


    }

}
