/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;

namespace wojilu.Web.Controller.Forum {

    public class LayoutController : ControllerBase {

        public override void Layout() {

            ForumApp app = ctx.app.obj as ForumApp;

            set( "adminUrl", to( new Admin.ForumController().Index ) );
            set( "appUrl", to( new ForumController().Index ) );

            set( "adminCheckUrl", t2( new wojilu.Web.Controller.SecurityController().CanAppAdmin, app.Id ) + "?appType=" + typeof( ForumApp ).FullName );

            // 当前app/module所有页面，所属的首页
            ctx.SetItem( "_moduleUrl", to( new ForumController().Index ) );

            set( "noticeLink", to( new Admin.ForumController().Notice ) );
            set( "headlineLink", to( new Admin.ForumController().Headline ) );
            set( "forumBoardList", to( new Admin.ForumController().Index ) );
            set( "dataCombine", to( new Admin.ForumController().DataCombine ) );
            set( "security", to( new Admin.SecurityController().Setting ) );
            set( "forumLog", to( new Admin.SecurityController().Log ) );
            set( "addFriend", to( new Admin.ForumLinkController().New ) );
            set( "friendList", to( new Admin.ForumLinkController().List ) );
            set( "recyclebin", to( new Admin.ForumController().TopicTrash ) );

            set( "settings", to( new Admin.SettingController().Index ) );

            set( "pickedImg", to( new Admin.PickedImgController().Index ) );


        }
    }

}
