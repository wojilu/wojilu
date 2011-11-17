/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Content.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Common;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Utils;
using wojilu.Web.Controller.Users;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Content.Binder {

    public class MyFaceBinderController : ControllerBase, ISectionBinder {

        public IContentCustomTemplateService ctService { get; set; }
        public MyFaceBinderController() {
            ctService = new ContentCustomTemplateService();
        }

        public void Bind( ContentSection section, IList serviceData ) {

            TemplateUtil.loadTemplate( this, section, ctService );

            User user = ctx.owner.obj as User;

            if (user == null) {
                actionContent( "" );
                return;
            }

            bindFace( user );


        }

        private void bindFace( User user ) {

            set( "profile", t2( new ProfileController().Main ) );
            set( "user.Face", user.PicMedium );

            set( "user.FriendCount", user.FriendCount );
            set( "user.FollowingCount", user.FollowingCount );
            set( "user.FollowersCount", user.FollowersCount );

            set( "user.FriendLink", t2( new Users.FriendController().FriendList ) );
            set( "user.FollowingLink", t2( new Users.FriendController().FollowingList ) );
            set( "user.FollowerLink", t2( new Users.FriendController().FollowerList ) );

            set( "user.AddFriendLink", t2( new FriendController().AddFriend, ctx.owner.Id ) );
            set( "user.AddFollowLink", t2( new FriendController().AddFollow, ctx.owner.Id ) );

            String lnkMsg = Link.T2( new LinkController().NewMsg, ctx.owner.Id );

            set( "sendMsgLink", lnkMsg );

            String shareLink = Link.T2( ctx.owner.obj, new wojilu.Web.Controller.ShareController().Add );
            shareLink = shareLink + string.Format( "?url={0}&title={1}&pic={2}",
                getFullUrl( Link.ToMember( user ) ), "" + user.Name + " µÄ¿Õ¼ä", user.PicOriginal );

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
