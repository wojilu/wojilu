/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using wojilu.Web.Mvc;
using wojilu.Apps.Content.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Users;

namespace wojilu.Web.Controller.Content.Binder {

    public class MyFaceBinderController : ControllerBase, ISectionBinder {

        public void Bind( ContentSection section, IList serviceData ) {

            User user = ctx.owner.obj as User;

            if (user == null) {
                content( "" );
                return;
            }

            bindFace( user );
        }

        private void bindFace( User user ) {

            set( "profile", t2( new ProfileController().Main ) );
            set( "user.Face", user.PicM );

            set( "user.FriendCount", user.FriendCount );
            set( "user.FollowingCount", user.FollowingCount );
            set( "user.FollowersCount", user.FollowersCount );

            set( "user.FriendLink", t2( new Users.FriendController().FriendList ) );
            set( "user.FollowingLink", t2( new Users.FriendController().FollowingList ) );
            set( "user.FollowerLink", t2( new Users.FriendController().FollowerList ) );

            set( "user.AddFriendLink", t2( new FriendController().AddFriend, ctx.owner.Id ) );
            set( "user.AddFollowLink", t2( new FriendController().AddFollow, ctx.owner.Id ) );

            String lnkMsg = Link.To( new LinkController().NewMsg, ctx.owner.Id );

            set( "sendMsgLink", lnkMsg );

            String shareLink = Link.To( ctx.owner.obj, new wojilu.Web.Controller.ShareController().Add );
            shareLink = shareLink + string.Format( "?url={0}&title={1}&pic={2}",
                getFullUrl( toUser( user ) ), "" + user.Name + " 的空间", user.PicO );

            set( "shareLink", shareLink );

            bind( "user", user );

        }

        private String getFullUrl( String url ) {
            if (url == null) return "";
            if (url.StartsWith( "http" )) return url;
            return strUtil.Join( ctx.url.SiteAndAppPath, url );
        }

    }

}
