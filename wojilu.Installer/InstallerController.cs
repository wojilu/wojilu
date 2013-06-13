/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Data;
using System.IO;

using wojilu.Common;
using wojilu.Data;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Web.Controller.Helpers;
using wojilu.Web.Context.Initor;
using wojilu.Web.Context;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Menus.Interface;
using System.Collections.Generic;

namespace wojilu.Web.Controller {

    public class InstallerController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( InstallerController ) );

        private static readonly Random rd = new Random();

        public IUserService userService { get; set; }
        public ILoginService loginService { get; set; }

        public InstallerController() {

            HideLayout( typeof( wojilu.Web.Controller.LayoutController ) );
            HidePermission( typeof( wojilu.Web.Controller.SecurityController ) );

            userService = new UserService();
            loginService = new LoginService();
        }

        public void Index() {

            // 检测网站目录权限
            testDiskPermission();

            DateTime n = DateTime.Now;
            String accessDbName = "wojilu_db_" + n.Year + "" + n.Month + "" + n.Day + "" + rd.Next( 10000000, 90000000 );
            set( "accessDbName", accessDbName );

            set( "setConfigLink", to( setDbConfig ) );
            set( "doneLink", to( Done ) );
            set( "setUserAndAppLink", to( setUserAndApp ) );

            if (ctx.HasErrors) {
                set( "writeFileMsg", string.Format( "<div class=\"warning\">【错误提醒】在安装之前，您必须让网站根目录允许写入权限，<strong>否则无法正常安装</strong>。正确设置方法，请 <a href=\"http://www.wojilu.com/tag/%E5%86%99%E6%9D%83%E9%99%90\" target=\"_blank\">参考此处</a><br/>" +
                    "{0}</div>", errors.ErrorsHtml ) );
            }
            else {
                set( "writeFileMsg", "" );
            }

            set( "userUrlPrefix", strUtil.TrimStart( strUtil.Append( ctx.url.SiteAndAppPath, "/" ), "http://" ) );
            set( "urlExt", MvcConfig.Instance.UrlExt );
        }

        [HttpPost]
        public void setDbConfig() {

            String dbType = ctx.Post( "dbType" );
            String dbName = ctx.Post( "dbName" );
            String connectionStr = ctx.Post( "connectionStr" );

            String connectionString = createConnectionString( dbType, dbName, connectionStr );

            if (ctx.HasErrors) {
                echoText( errors.ErrorsText );
                return;
            }

            String strConfig = getConfigTemplate();
            Template t = new Template();
            t.InitContent( strConfig );
            t.Set( "connectionString", connectionString );
            t.Set( "dbType", dbType );

            String fileName = "/config/orm.config";
            String filePath = strUtil.Join( cfgHelper.FrameworkRoot, fileName );
            String dataPath = PathHelper.Map( filePath );
            file.Write( dataPath, t.ToString() );

            sys.Clear.ClearAll();

            echoAjaxOk();
        }

        [HttpPost]
        public void setUserAndApp() {

            // 1）初始化网站基本信息
            SiteInitHelper initHelper = ObjectContext.Create<SiteInitHelper>();
            if (initHelper.HasInit() == false) {
                initHelper.InitSite();
            }

            // 2）创建用户
            String name = ctx.Post( "name" );
            String email = ctx.Post( "email" );
            String pwd = ctx.Post( "pwd" );
            String pageUrl = ctx.Post( "url" );

            User user = new User();
            user.Name = name;
            user.Pwd = pwd;
            user.Email = email;
            user.Url = strUtil.IsNullOrEmpty( pageUrl ) ? "admin" : pageUrl;

            user = userService.Register( user, ctx );
            if (ctx.HasErrors) {
                echoText( errors.ErrorsText );
                return;
            }

            RegHelper.CheckUserSpace( user, ctx );
            loginService.Login( user, LoginTime.Forever, ctx.Ip, ctx );

            // 3）初始化owner/viewer
            initOwner( ctx );
            initViewer( ctx, user );

            // 4) 网站名称
            String siteName = ctx.Post( "siteName" );
            updateSiteName( siteName );

            // 5）安装app
            int siteType = ctx.PostInt( "siteType" );
            initSiteApp( siteType, user );

            echoAjaxOk();
        }

        private void updateSiteName( string siteName ) {
            if (strUtil.IsNullOrEmpty( siteName )) return;
            config.Instance.Site.SiteName = siteName;
            config.Instance.Site.Update( "SiteName", siteName );
        }

        private void initOwner( Context.MvcContext ctx ) {
            IMember owner = Site.Instance;

            OwnerContext context = new OwnerContext();
            context.Id = owner.Id;
            context.obj = owner;
            ctx.utils.setOwnerContext( context );
        }

        private void initViewer( MvcContext ctx, User user ) {
            ViewerContext context = new ViewerContext( ctx );
            context.Id = user.Id;
            context.obj = user;
            context.IsLogin = true;
            ctx.utils.setViewerContext( context );
        }

        private void initSiteApp( int siteType, User user ) {

            if (siteType <= 0) return;

            if (siteType == 1) {
                createFullSite();
            }
            else if (siteType == 2) {
                createCmsSite();
            }
            else if (siteType == 3) {
                createForumSite();
            }
            else if (siteType == 4) {
                createMicroblogSite();
            }
            else if (siteType == 5) {
                createWaterfallSite();
            }
            else if (siteType == 6) {
                createPersonalSite( user );
            }
        }


        private void createCmsSite() {
            CmsInstaller x1 = ObjectContext.Create<CmsInstaller>();
            LinkInstaller x2 = ObjectContext.Create<LinkInstaller>();

            // 门户首页
            IMemberApp mappPortal = x1.CreatePortal( ctx, "首页", "default" );

            // 新闻
            IMemberApp mappNews = x1.CreateNews( ctx, "新闻资讯", "news" );

            // 设置安装完毕
            updateSiteDone();

            // 生成静态页面
            HtmlInstallerHelper htmlHelper = ObjectContext.Create<HtmlInstallerHelper>();
            htmlHelper.MakeHtml( ctx, mappPortal, mappNews );
        }

        private static void updateSiteDone() {
            config.Instance.Site.IsInstall = true;
            config.Instance.Site.Update( "IsInstall", true );
        }

        // 论坛
        private void createForumSite() {
            ObjectContext.Create<ForumInstaller>().Init( ctx, "首页", "default" );
            updateSiteDone();
        }

        // 微博
        private void createMicroblogSite() {
            ObjectContext.Create<MicroblogInstaller>().Init( ctx, "首页", "default" );
            updateSiteDone();
        }

        // 瀑布流
        private void createWaterfallSite() {
            ObjectContext.Create<WaterfallInstaller>().Init( ctx, "首页", "default" );
            updateSiteDone();
        }

        private void createPersonalSite( User user ) {

            // 将博客=>设为个人空间首页
            UserMenu blogMenu = getBlogMenu( user );
            blogMenu.Url = "default";
            ServiceMap.GetMenuService( typeof( User ) ).Update( blogMenu );

            // 修改路由default;default:{owner=admin,ownertype=user}
            updateRoute( user );

            // 修改主题
            user.TemplateId = 31;
            user.update();

            // done
            updateSiteDone();

            // 重启网站
            sys.Clear.ClearAll();
        }

        private static void updateRoute( User user ) {
            String routePath = strUtil.Join( cfgHelper.ConfigRoot, "route.config" );
            routePath = PathHelper.Map( routePath );
            String routeContent = file.Read( routePath );
            routeContent = routeContent.Replace( "default;default:{controller=SiteInit}", "//default;default:{controller=SiteInit}" );
            routeContent = routeContent.Replace( "//default;default:{owner=userUrl,ownertype=user}", "default;default:{owner=" + user.Url + ",ownertype=user}" );
            file.Write( routePath, routeContent );
        }

        private UserMenu getBlogMenu( User user ) {

            List<IMenu> menus = ServiceMap.GetMenuService( typeof( User ) ).GetList( user );
            foreach (IMenu x in menus) {
                if (x.RawUrl == null) continue;
                if (x.RawUrl.ToLower().IndexOf( "blog/index" ) > 0) return x as UserMenu;
            }
            return null;
        }

        private void createFullSite() {

            // 完整安装
            CmsInstaller x1 = ObjectContext.Create<CmsInstaller>();
            LinkInstaller x2 = ObjectContext.Create<LinkInstaller>();

            // 门户首页
            IMemberApp mappPortal = x1.CreatePortal( ctx, "首页", "default" );

            // 新闻
            IMemberApp mappNews = x1.CreateNews( ctx, "新闻资讯", "news" );

            // 论坛
            ObjectContext.Create<ForumInstaller>().Init( ctx, "讨论区", "bbs" );

            // 微博
            ObjectContext.Create<MicroblogInstaller>().Init( ctx, "微博", "t" );

            // 瀑布流
            ObjectContext.Create<WaterfallInstaller>().Init( ctx, "图片", "pic" );

            // 下载
            ObjectContext.Create<DownloadInstaller>().Init( ctx, "资源下载", "download" );

            // 博客
            x2.AddBlog( ctx, "博客", "blog" );

            // 群组
            x2.AddGroup( ctx, "群组", "group" );

            // 用户
            x2.AddUser( ctx, "用户列表", "user" );

            // tag
            x2.AddTag( ctx, "Tag", "tags" );


            // 设置安装完毕
            config.Instance.Site.IsInstall = true;
            config.Instance.Site.Update( "IsInstall", true );

            // 生成静态页面
            HtmlInstallerHelper htmlHelper = ObjectContext.Create<HtmlInstallerHelper>();
            htmlHelper.MakeHtml( ctx, mappPortal, mappNews );
        }


        [HttpPost]
        public void Done() {

            echoAjaxOk();
        }

        //----------------------------------------------------------------------------------

        private string getConfigTemplate() {

            return @"{ 
    ConnectionStringTable:{
        // 数据库连接字符串 
        // default:""server=localhost;uid=用户名;pwd=你的密码;database=数据库名称;""
        // default:""Provider=Microsoft.Jet.OLEDB.4.0;Data Source=数据库名称.mdb""
        default:""#{connectionString}""
     }, 
     
    DbType:{
        // 根据不同的数据库，你可以选填：sqlserver/sqlserver2000/access/mysql 四种之一
        default:""#{dbType}""
     }, 
     
    AssemblyList : [""wojilu.Core"",""wojilu.Apps"",""wojilu.Apps.Download""],

    // 是否启用二级二级缓存
    ApplicationCache:true,
    
    // 二级缓存的过期时间，如果想永久性缓存，请填写-999
    ApplicationCacheMinutes:-999

 }
";

        }

        private string createConnectionString( string dbType, String dbName, string connectionStr ) {

            if ("access".Equals( dbType )) {

                if (strUtil.IsNullOrEmpty( dbName )) {
                    errors.Add( "请填写access数据库的名称" );
                    return null;
                }
                else {
                    return string.Format( "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", dbName );
                }
            }


            if ("sqlserver".Equals( dbType ) || "sqlserver2000".Equals( dbType )) {

                String str = clearStrLine( connectionStr );
                if (ctx.HasErrors) return null;

                try {
                    IDbConnection cn = DataFactory.GetConnection( str, DatabaseType.SqlServer );
                    cn.Open();
                    cn.Close();
                    return str;
                }
                catch (Exception ex) {
                    errors.Add( ex.Message );
                    return null;
                }
            }

            if ("mysql".Equals( dbType )) {

                String str = clearStrLine( connectionStr );
                if (ctx.HasErrors) return null;

                try {
                    IDbConnection cn = DataFactory.GetConnection( str, DatabaseType.MySql );
                    cn.Open();
                    cn.Close();
                    return str;
                }
                catch (Exception ex) {
                    errors.Add( ex.Message );
                    return "";
                }

            }

            errors.Add( "请选择正确的数据库类型" );
            return null;

        }

        private string clearStrLine( string connectionStr ) {
            if (strUtil.IsNullOrEmpty( connectionStr )) {
                errors.Add( "请填写数据库连接字符串" );
                return "";
            }
            return connectionStr.Replace( "\n", "" ).Replace( "\r", "" ).Replace( "\\", "\\\\" );
        }

        // 在检测之前，必须关闭log，将其设为error，否则日志系统会提前写磁盘，然后提前报错
        private void testDiskPermission() {
            testRootPath();
            testFrameworkPath();
            testUploadPath();
        }

        //-----------------------------------------------------------------------------------------

        // upload目录检测
        private void testUploadPath() {
            String fileName = getRandomFileName();
            String filePath = strUtil.Join( sys.Path.DiskUpload, fileName );
            String dataPath = PathHelper.Map( filePath );
            writeFilePrivate( dataPath );

            try {
                String testDir = PathHelper.Map( strUtil.Join( sys.Path.DiskUpload, "imageTest" ) );
                Directory.CreateDirectory( testDir );
                Directory.Delete( testDir );
            }
            catch (Exception ex) {
                errors.Add( "不能在上传目录创建子目录:" + sys.Path.DiskUpload );
            }
        }

        // framework目录检测
        private void testFrameworkPath() {
            String fileName = getRandomFileName();
            String filePath = strUtil.Join( cfgHelper.FrameworkRoot, fileName );
            String dataPath = PathHelper.Map( filePath );
            writeFilePrivate( dataPath );

            testConfigFile( "orm.config" );
            testConfigFile( "route.config" );
            testConfigFile( "site.config" );
        }

        private void testConfigFile( String configFile ) {
            String ormPath = strUtil.Join( cfgHelper.ConfigRoot, configFile );
            ormPath = PathHelper.Map( ormPath );
            String ormContent = file.Read( ormPath );
            try {
                file.Write( ormPath, ormContent );
                logger.Info( "write " + ormPath + " ok" );
            }
            catch (Exception ex) {
                logger.Error( "write error:" + configFile );
                errors.Add( "出错了！这个文件不能修改: " + ormPath );
            }
        }

        // 根目录检测
        private void testRootPath() {
            String fileName = getRandomFileName();
            String rootPathFile = PathHelper.Map( strUtil.Join( SystemInfo.ApplicationPath, fileName ) );
            writeFilePrivate( rootPathFile );

            String rootDir = PathHelper.Map( strUtil.Join( SystemInfo.ApplicationPath, "_dirtest" ) );
            try {
                Directory.CreateDirectory( rootDir );
                Directory.Delete( rootDir );
            }
            catch (Exception ex) {
                errors.Add( "不能在根目录创建子目录" );
            }
        }

        //-----------------------------------------------------------------------------------------

        // 创建一个文件，然后删除
        private void writeFilePrivate( String filePath ) {
            try {
                file.Write( filePath, "_test_write_file_content ..." );
                file.Delete( filePath );
                logger.Info( "write test ok:" + filePath );
            }
            catch (IOException ex) {
                errors.Add( ex.Message );
                logger.Error( "write error:" + filePath );
            }
        }

        private String getRandomFileName() {
            String fileName = "_test_write_file_" + Guid.NewGuid() + ".txt";
            return fileName;
        }

    }

}
