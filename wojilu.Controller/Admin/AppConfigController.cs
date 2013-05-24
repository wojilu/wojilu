/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common;
using wojilu.Common.AppInstall;
using wojilu.Common.Themes;

namespace wojilu.Web.Controller.Admin {

    public class AppConfigController : ControllerBase {

        public IThemeService themeService { get; set; }
        public IAppInstallerService appService { get; set; }

        public AppConfigController() {
            themeService = new ThemeService();
            appService = new AppInstallerService();
        }

        public void App() {

            target( AppSave );

            Dictionary<string, string> dicApp = new Dictionary<string, string>();
            dicApp.Add( "主页", "home" );
            dicApp.Add( "博客", "blog" );
            dicApp.Add( "相册", "photo" );
            dicApp.Add( "微博", "microblog" );
            dicApp.Add( "分享", "share" );
            dicApp.Add( "好友", "friend" );
            dicApp.Add( "访客", "visitor" );
            dicApp.Add( "论坛帖子", "forumpost" );
            dicApp.Add( "关于我", "about" );
            dicApp.Add( "留言", "feedback" );
            checkboxList( "initApp", dicApp, config.Instance.Site.UserInitApp );

            // app部署管理：按照分类归类，分类进行排序
            List<AppInstaller> list = cdb.findAll<AppInstaller>();
            bindList( "list", "app", list, bindAppAdminLink );

            // 基础组件
            List<Component> clist = cdb.findAll<Component>();
            IBlock cblock = getBlock( "clist" );
            foreach (Component c in clist) {
                cblock.Set( "c.Name", c.Name );
                cblock.Set( "c.StatusName", c.StatusName );
                cblock.Set( "c.AdminLink", to( EditComponent, c.Id ) );
                cblock.Next();
            }

        }


        public void AppSave() {
            String initApp = ctx.Post( "initApp" );
            config.Instance.Site.UserInitApp = initApp; config.Instance.Site.Update( "UserInitApp", initApp );
            echoRedirect( lang( "opok" ) );
        }

        private void bindAppAdminLink( IBlock block, String lbl, object obj ) {
            AppInstaller installer = obj as AppInstaller;
            block.Set( "app.StatusAdminName", "修改" );
            block.Set( "app.StatusAdminLink", to( EditStatus, installer.Id ) );

            // 绑定安装主题
            List<ITheme> themeList = themeService.GetThemeList( installer );
            if (themeList.Count > 0) {
                block.Set( "app.ThemeInfo", string.Format( "{0}个主题", themeList.Count ) );
                block.Set( "app.ThemeAdminLink", to( new AppThemeController().Index, installer.Id ) );
                block.Set( "app.ThemeAdmin", "<span class=\"cmd\">管理主题</span>" );
            } else {
                block.Set( "app.ThemeInfo", "" );
                block.Set( "app.ThemeAdminLink", "#" );
                block.Set( "app.ThemeAdmin", "" );
            }

        }


        //---------------------------------------------------------------------------------------------


        public void EditStatus( int id ) {

            target( UpdateStatus, id );

            AppInstaller installer = cdb.findById<AppInstaller>( id );
            if (installer == null) throw new NullReferenceException();

            set( "installer.Name", installer.Name );
            set( "installer.Description", installer.Description );


            List<AppCategory> cats = new List<AppCategory>();
            if (installer.CatId == AppCategory.General) {
                cats = AppCategory.GetAllWithoutGeneral();
            } else {
                cats.Add( AppCategory.GetByCatId( installer.CatId ) );
            }
            checkboxList( "appCheckboxList", cats, "Name=TypeFullName", installer.StatusValue );

            radioList( "closeMode", AppCloseMode.GetAllMode(), "Name=Id", installer.CloseMode );
        }

        [HttpPost]
        public void UpdateStatus( int id ) {

            AppInstaller installer = cdb.findById<AppInstaller>( id );
            if (installer == null) throw new NullReferenceException();

            String name = ctx.Post( "Name" );
            String description = strUtil.CutString( ctx.Post( "Description" ), 150 );

            if (strUtil.IsNullOrEmpty( name )) {
                errors.Add( "请填写名称" );
                run( EditStatus, id );
                return;
            }

            if (strUtil.IsNullOrEmpty( description )) {
                errors.Add( "请填写简介" );
                run( EditStatus, id );
                return;
            }

            int closeMode = ctx.PostInt( "closeMode" );

            installer.Name = name;
            installer.Description = description;
            installer.CloseMode = closeMode;

            installer.update();


            String val = ctx.Post( "appCheckboxList" );

            appService.UpdateStatus( installer, val );

            echoToParentPart( lang( "opok" ), to( App ), 0 );
        }


        public void EditComponent( int id ) {
            target( SaveComponent, id );
            Component c = cdb.findById<Component>( id );
            set( "c.Name", c.Name );
            radioList( "status", ComponentStatus.GetStatusList( c.TypeFullName ), "Name=Id", c.Status );
        }

        public void SaveComponent( int id ) {
            Component c = cdb.findById<Component>( id );
            c.Status = ctx.PostInt( "status" );
            c.update();
            echoToParentPart( lang( "opok" ), to( App ), 0 );
        }


    }

}
