/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Common.Microblogs.Domain;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Microblogs.My {

    public partial class MicroblogController : ControllerBase {


        private void bindUserInfo( User user ) {
            set( "siteName", config.Instance.Site.SiteName );
            set( "microblogHomeLink", getFullUrl( alink.ToMicroblog() ) );
            set( "homeLink", to( Home ) );
            set( "favoriteLink", to( new MicroblogFavoriteController().List ) );
            set( "atmeLink", to( Atme ) );
            set( "myCommentLink", to( new MicroblogCommentsController().My ) );

            set( "user.Name", user.Name );
            set( "user.Pic", user.PicSX );
            set( "user.PicSmall", user.PicSmall );

            set( "user.PicBig", user.PicM );
            set( "user.Link", getFullUrl( toUser( user ) ) );
            set( "user.MLink", getFullUrl( alink.ToUserMicroblog( user ) ) );
            set( "user.Signature", user.Signature );
            set( "user.Description", user.Profile.Description );

            int microblogCount = microblogService.CountByUser( user.Id );
            set( "user.MicroblogCount", microblogCount );

            bindStats( user );
        }

        private String getFullUrl( String url ) {
            if (url == null) return "";
            if (url.StartsWith( "http" )) return url;
            return strUtil.Join( ctx.url.SiteAndAppPath, url );
        }

        private String bindCmd( User user ) {

            set( "followUrl", to( new wojilu.Web.Controller.Microblogs.MicroblogController().Follow ) );
            set( "cancelUrl", to( new wojilu.Web.Controller.Microblogs.MicroblogController().CancelFollow ) );

            if (ctx.viewer.IsLogin == false) return "<div id=\"lblFollow\"><span>加关注</span></div>";
            if (ctx.viewer.Id == user.Id) return "";
            if (ctx.viewer.IsFriend( user.Id ))
                return "<div id=\"cmdCancelFollow\"><span id=\"followed\">已是好友</span></div>";

            if (ctx.viewer.IsFollowing( user.Id ))
                return "<div id=\"cmdCancelFollow\"><span id=\"followed\">已关注</span><span id=\"cancelFollow\">取消关注</span></div>";

            return "<div id=\"cmdFollow\"><span>加关注</span></div>";
        }

        private void bindStats( User user ) {
            set( "user.FollowingCount", (user.FriendCount + user.FollowingCount) );
            set( "user.FollowersCount", (user.FollowersCount + user.FriendCount) );

            set( "user.FollowingLink", t2( new Users.FriendController().FollowingList ) );
            set( "user.FollowerLink", t2( new Users.FriendController().FollowerList ) );
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
