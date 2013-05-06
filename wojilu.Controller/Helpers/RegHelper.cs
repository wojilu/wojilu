/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common;
using wojilu.Members.Users.Domain;
using wojilu.Web.Context;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Members.Users.Service;
using wojilu.Web.Controller.Common.Installers;
using wojilu.Web.Url;
using wojilu.Web.Mvc;

using wojilu.Members.Users.Interface;

namespace wojilu.Web.Controller.Helpers {

    public class RegHelper {

        // 根据邀请码注册，需要加为好友
        public static void ProcessFriend( User newRegUser, MvcContext ctx ) {


            IInviteService inviteService = new InviteService();
            IFriendService friendService = new FriendService();

            int friendId = ctx.PostInt( "friendId" );
            if (friendId <= 0) return;

            String friendCode = ctx.Post( "friendCode" );

            Result result = inviteService.Validate( friendId, friendCode );
            if (result.HasErrors) return;

            friendService.AddInviteFriend( newRegUser, friendId );
        }

        //----------------------------------------------------------------------------------------------------

        public static void CheckUserSpace( User user, MvcContext ctx ) {

            // 开启空间
            if (Component.IsEnableUserSpace()) {
                addUserAppAndMenus( user, ctx );
            }
        }

        private static void addUserAppAndMenus( User user, MvcContext ctx ) {

            if (strUtil.IsNullOrEmpty( config.Instance.Site.UserInitApp )) return;

            IMemberAppService appService = new UserAppService();
            IMenuService menuService = new UserMenuService();

            List<String> menus = new List<string>();

            String[] arr = config.Instance.Site.UserInitApp.Split( ',' );
            foreach (String app in arr) {
                if (strUtil.IsNullOrEmpty( app )) continue;
                menus.Add( app.Trim() );
            }

            if (menus.Contains( "home" )) {
                new UserHomeInstaller().Install( ctx, user, wojilu.lang.get( "homepage" ), wojilu.Common.AppBase.AccessStatus.Public );
            }

            if (menus.Contains( "blog" )) {
                IMemberApp blogApp = appService.Add( user, "博客", 2 );
                // 添加菜单：此处需要明确传入MemberType，否则将会使用ctx.Owner，也就是Site的值，导致bug
                String blogUrl = UrlConverter.clearUrl( alink.ToUserAppFull( blogApp ), ctx, typeof( User ).FullName, user.Url );
                menuService.AddMenuByApp( blogApp, blogApp.Name, "", blogUrl );
            }

            if (menus.Contains( "photo" )) {
                IMemberApp photoApp = appService.Add( user, "相册", 3 );
                String photoUrl = UrlConverter.clearUrl( alink.ToUserAppFull( photoApp ), ctx, typeof( User ).FullName, user.Url );
                menuService.AddMenuByApp( photoApp, photoApp.Name, "", photoUrl );
            }

            if (menus.Contains( "microblog" )) {
                IMenu menu = getMenu( user, "微博", alink.ToUserMicroblog( user ), ctx );
                menuService.Insert( menu, user, user );
            }

            //if (menus.Contains( "share" )) {
            //    IMenu menu = getMenu( user, "转帖", lnkToUser( new Users.ShareController().Index ), ctx );
            //    menuService.Insert( menu, user, user );
            //}

            if (menus.Contains( "friend" )) {
                IMenu menu = getMenu( user, "好友", lnkToUser( user, new Users.FriendController().FriendList ), ctx );
                menuService.Insert( menu, user, user );
            }

            if (menus.Contains( "visitor" )) {
                IMenu menu = getMenu( user, "访客", lnkToUser( user, new Users.VisitorController().Index ), ctx );
                menuService.Insert( menu, user, user );
            }

            if (menus.Contains( "forumpost" )) {
                IMenu menu = getMenu( user, "论坛帖子", lnkToUser( user, new Users.ForumController().Topic ), ctx );
                menuService.Insert( menu, user, user );
            }

            if (menus.Contains( "about" )) {
                IMenu menu = getMenu( user, "关于我", lnkToUser( user, new Users.ProfileController().Main ), ctx );
                menuService.Insert( menu, user, user );
            }

            if (menus.Contains( "feedback" )) {
                IMenu menu = getMenu( user, "留言", lnkToUser( user, new Users.FeedbackController().List ), ctx );
                menuService.Insert( menu, user, user );
            }

        }

        private static String lnkToUser( User user, aAction action ) {
            return Link.To( user, action, 0 );
        }

        private static IMenu getMenu( User user, string name, string url, MvcContext ctx ) {
            IMenu menu = new UserMenu();
            menu.Name = name;
            menu.RawUrl = UrlConverter.clearUrl( url, ctx, typeof( User ).FullName, user.Url );

            return menu;
        }

    }
}
