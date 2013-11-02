using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using wojilu.Members.Users.Domain;
using wojilu.Common.MemberApp.Interface;
using wojilu.Web.Url;
using wojilu.Web.Mvc;
using wojilu.Common.Menus.Interface;
using wojilu.Members.Users.Service;
using wojilu.Common.AppBase.Interface;
using wojilu.Web.Context;

namespace wojilu.Open {

    public class AppService {

        /// <summary>
        /// 安装程序和菜单
        /// </summary>
        /// <param name="user"></param>
        /// <param name="UserInitApp">从 home,blog,photo,microblog,friend,visitor,forumpost,about,feedback 中选择若干项</param>
        public void InstallAppAndMenu( User user, String UserInitApp ) {

            if (strUtil.IsNullOrEmpty( UserInitApp )) return;
            List<String> menus = new List<string>();
            String[] arr = UserInitApp.Split( ',' );
            foreach (String app in arr) {
                if (strUtil.IsNullOrEmpty( app )) continue;
                menus.Add( app.Trim() );
            }

            if (menus.Contains( "home" )) {
                addMenu( user, "主页", Link.ToMember( user ), true );
            }

            if (menus.Contains( "blog" )) {
                addApp( user, "博客", 2, "wojilu.Apps.Blog.Domain.BlogApp" );
            }

            if (menus.Contains( "photo" )) {
                addApp( user, "相册", 3, "wojilu.Apps.Photo.Domain.PhotoApp" );
            }

            if (menus.Contains( "microblog" )) {
                addMenu( user, "微博", "t/" + user.Url, false );
            }

            if (menus.Contains( "share" )) {
                addMenu( user, "转帖", "Users/Share/Index", false );
            }

            if (menus.Contains( "friend" )) {
                addMenu( user, "好友", "Users/Friend/FriendList", false );
            }

            if (menus.Contains( "visitor" )) {
                addMenu( user, "访客", "Users/Visitor/Index", false );
            }

            if (menus.Contains( "forumpost" )) {
                addMenu( user, "论坛帖子", "Users/Forum/Topic", false );
            }

            if (menus.Contains( "about" )) {
                addMenu( user, "关于我", "Users/Profile/Main", false );
            }

            if (menus.Contains( "feedback" )) {
                addMenu( user, "留言", "Users/Feedback/List", false );
            }
        }


        public static UserApp addApp( User user, String name, int appInfoId, String appTypeFullName ) {
            return addApp( user, name, appInfoId, appTypeFullName, false );
        }

        public static UserApp addApp( User user, String name, int appInfoId, String appTypeFullName, Boolean isDefault ) {


            // 1) app

            //IApp app = Entity.New( appTypeFullName ) as IApp;
            IApp app = rft.GetInstance( "wojilu.Apps", appTypeFullName ) as IApp;

            app.OwnerId = user.Id;
            app.OwnerUrl = user.Url;
            app.OwnerType = typeof( User ).FullName;
            db.insert( app );

            // 2) userApp
            UserApp ua = new UserApp();
            ua.AppInfoId = appInfoId;
            ua.AppOid = app.Id;
            ua.Name = name;
            ua.Creator = user;
            ua.CreatorUrl = user.Url;
            ua.OwnerId = user.Id;
            ua.OwnerUrl = user.Url;
            ua.insert();

            // 3) menu
            // "wojilu.Apps.Blog.Domain.BlogApp";
            String appName = appTypeFullName.Split( '.' )[2];
            String path = string.Format( "{0}{1}/{0}/Index", appName, app.Id );
            addMenu( user, name, path, isDefault );

            return ua;
        }

        public static void addMenu( User user, String name, String path, Boolean isDefault ) {

            UserMenu m = new UserMenu();
            m.Name = name;
            m.RawUrl = path;

            m.Creator = user;
            m.OwnerId = user.Id;
            m.OwnerUrl = user.Url;

            if (isDefault) {
                m.Url = "default";
            }

            m.insert();
        }


    }
}
