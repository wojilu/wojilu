/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Forum.Admin {

    public class LayoutController :ControllerBase{

        // 不适用前台的数据（引入了<div>&nbsp;</div>和不必要的js）
        public LayoutController() {
            HideLayout( typeof( Forum.LayoutController ) );
        }

        public override void Layout() {

            set( "noticeLink", to( new ForumController().Notice ) );
            set( "forumBoardList", to( new ForumController().Index ) );
            set( "dataCombine", to( new ForumController().DataCombine ) );
            set( "security", to( new SecurityController().Setting ) );
            set( "forumLog", to( new SecurityController().Log ) );
            set( "addFriend", to( new ForumLinkController().New ) );
            set( "friendList", to( new ForumLinkController().List ) );
            set( "recyclebin", to( new ForumController().TopicTrash ) );

            set( "settings", to( new SettingController().Index ) );

            set( "pickedImg", to( new PickedImgController().Index ) );


        }
    }

}
