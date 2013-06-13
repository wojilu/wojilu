/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Apps;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Web.Controller.Common;
using wojilu.Members.Users.Domain;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Microblogs.Service;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Users {


    public class ProfileController : ControllerBase {

        public void Main() {

            ctx.Page.Title = "about";

            User user = ctx.owner.obj as User;
            load( "userMenu", UserMenu );

            Microblog blog = new MicroblogService().GetFirst( user.Id );

            if (blog != null) {
                String lnkMore = alink.ToUserMicroblog( user );
                String more = "<a href='" + lnkMore + "'>" + lang( "more" ) + "...</a>";
                String logcontent = blog.Content + " <span class='left10'>" + more + "</span>";

                set( "microblog", logcontent );
            }
            else
                set( "microblog", "" );

            UserVo uservo = new UserVo( user );

            bind( "user", uservo );

            IBlock pblock = getBlock( "profile" );
            IBlock cblock = getBlock( "contact" );
            IBlock iblock = getBlock( "interest" );

            if (ctx.viewer.HasPrivacyPermission( user, UserPermission.Profile.ToString() )) {
                pblock.Bind( "user", uservo );
                pblock.Next();
            }

            if (ctx.viewer.HasPrivacyPermission( user, UserPermission.Contact.ToString() )) {
                cblock.Bind( "user", uservo );
                cblock.Next();
            }

            if (ctx.viewer.HasPrivacyPermission( user, UserPermission.Hobby.ToString() )) {
                iblock.Bind( "user", uservo );
                iblock.Next();
            }

        }


        public void UserMenu() {

            User user = ctx.GetItem( "__owner" ) as User;
            if (user == null) {
                user = ctx.owner.obj as User;
            }

            if (user == null) {
                content( "" );
                return;
            }

            set( "profile", t2( Main ) );
            set( "user.Face", user.PicM );

            set( "user.FriendCount", user.FriendCount );
            set( "user.FollowingCount", user.FollowingCount );
            set( "user.FollowersCount", user.FollowersCount );

            set( "user.FriendLink", t2( new Users.FriendController().FriendList ) );
            set( "user.FollowingLink", t2( new Users.FriendController().FollowingList ) );
            set( "user.FollowerLink", t2( new Users.FriendController().FollowerList ) );

            String friendCmd = WebUtils.getFriendCmd( ctx );
            set( "friendCmd", friendCmd );

            String lnkMsg;
            if (ctx.viewer.IsLogin) {
                //lnkMsg = Link.T2( ctx.viewer.obj, new Users.Admin.MsgController().New ) + "?ToName=" + ctx.web.UrlDecode( user.Name );
                lnkMsg = Link.To( ctx.viewer.obj, new Users.Admin.MsgController().New, user.Id );
            }
            else {
                lnkMsg = "#";
            }

            set( "sendMsgLink", lnkMsg );

            String shareLink = Link.To( ctx.owner.obj, new wojilu.Web.Controller.ShareController().Add );
            shareLink = shareLink + string.Format( "?url={0}&title={1}&pic={2}",
                getFullUrl( toUser( user ) ), user.Name + " 的空间", user.PicO );

            set( "shareLink", shareLink );
        }

        private String getFullUrl( String url ) {
            if (url == null) return "";
            if (url.StartsWith( "http" )) return url;
            return strUtil.Join( ctx.url.SiteAndAppPath, url );
        }

    }
}

