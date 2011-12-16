/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Shop.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Shop.Domain;
using wojilu.Web.Controller.Common;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Shop.Utils;
using wojilu.Web.Controller.Users;

namespace wojilu.Web.Controller.Shop.Binder {

    public class MyFaceBinderController : ControllerBase, ISectionBinder {

        public IShopCustomTemplateService ctService { get; set; }
        public MyFaceBinderController() {
            ctService = new ShopCustomTemplateService();
        }

        public void Bind( ShopSection section, IList serviceData ) {

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

            //String friendCmd = WebUtils.getFriendCmd( ctx );
            //set( "friendCmd", friendCmd );

            //String lnkMsg;
            //if (ctx.viewer.IsLogin)
            //    //lnkMsg = Link.T2( ctx.viewer.obj, new Users.Admin.MsgController().New ) + "?ToName=" + ctx.web.UrlDecode( user.Name );
            //    lnkMsg = Link.T2( ctx.viewer.obj, new Users.Admin.MsgController().New, user.Id );
            //else
            //    lnkMsg = "#";

            String lnkMsg = Link.T2( new LinkController().NewMsg, ctx.owner.Id );


            set( "sendMsgLink", lnkMsg );

            //set( "shareLink", WebUtils.getShareLink( ctx, user, lang( "user" ) ) );

            bind( "user", user );

        }

    }

}
