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

namespace wojilu.Web.Controller {

    public class InstallerController : ControllerBase {

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
                set( "writeFileMsg", string.Format( "<div class=\"warning\">{0}<br/>【提醒】请不要把项目放在桌面或者其他有特殊权限的文件夹下，文件路径不能有空格。</div>", errors.ErrorsHtml ) );
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

            // 4）安装app
            int siteType = ctx.PostInt( "siteType" );
            initSiteApp( siteType );

            echoAjaxOk();
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

        private void initSiteApp( int siteType ) {

            if (siteType <= 0) return;

            // 完整安装
            CmsInstaller x1 = ObjectContext.Create<CmsInstaller>();
            LinkInstaller x2 = ObjectContext.Create<LinkInstaller>();

            // 门户首页
            x1.CreatePortal( ctx, "首页", "default" );

            // 新闻
            x1.CreateNews( ctx, "新闻资讯", "news" );

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
        }


        [HttpPost]
        public void Done() {

            config.Instance.Site.IsInstall = true;
            config.Instance.Site.Update( "IsInstall", true );

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
        }

        // framework目录检测
        private void testFrameworkPath() {
            String fileName = getRandomFileName();
            String filePath = strUtil.Join( cfgHelper.FrameworkRoot, fileName );
            String dataPath = PathHelper.Map( filePath );
            writeFilePrivate( dataPath );
        }

        // 根目录检测
        private void testRootPath() {
            String fileName = getRandomFileName();
            String rootPathFile = PathHelper.Map( strUtil.Join( SystemInfo.ApplicationPath, fileName ) );
            writeFilePrivate( rootPathFile );
        }

        //-----------------------------------------------------------------------------------------

        // 创建一个文件，然后删除
        private void writeFilePrivate( String filePath ) {
            try {
                file.Write( filePath, "_test_write_file_content ..." );
                file.Delete( filePath );
            }
            catch (IOException ex) {
                errors.Add( ex.Message );
            }
        }

        private String getRandomFileName() {
            String fileName = "_test_write_file_" + Guid.NewGuid() + ".txt";
            return fileName;
        }

    }

}
