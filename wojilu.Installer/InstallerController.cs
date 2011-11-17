using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using System.IO;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller {

    public class InstallerController : ControllerBase {

        private static readonly Random rd = new Random();

        public InstallerController() {

            HideLayout( typeof( wojilu.Web.Controller.LayoutController ) );
            HidePermission( typeof( wojilu.Web.Controller.SecurityController ) );
        }


        public void Index() {

            // 检测网站目录权限
            testDiskPermission();

            DateTime n = DateTime.Now;
            String accessDbName = "wojilu_db_" + n.Year + "" + n.Month + "" + n.Day + "" + rd.Next( 10000000, 90000000 );
            set( "accessDbName", accessDbName );

            set( "setConfigLink", to( setDbConfig ) );
            set( "doneLink", to( Done ) );

            if (ctx.HasErrors) {
                set( "writeFileMsg", string.Format( "<div class=\"warning\">{0}<br/>【提醒】请不要把项目放在桌面或者其他有特殊权限的文件夹下，文件路径不能有空格。</div>", errors.ErrorsHtml ) );
            }
            else {
                set( "writeFileMsg", "" );
            }

        }

        [HttpPost]
        public void Done() {

            config.Instance.Site.IsInstall = true;
            config.Instance.Site.Update( "IsInstall", true );

            echoAjaxOk();
        }



        [HttpPost]
        public void setDbConfig() {

            String dbType = ctx.Post( "dbType" );
            String dbAddress = ctx.Post( "dbAddress" );
            String dbName = ctx.Post( "dbName" );
            String user = ctx.Post( "user" );
            String pwd = ctx.Post( "pwd" );

            String connectionString = createConnectionString( dbType, dbAddress, dbName, user, pwd );

            String strConfig = getConfigTemplate();
            Template t = new Template();
            t.InitContent( strConfig );
            t.Set( "connectionString", connectionString );
            t.Set( "dbType", dbType );

            String fileName = "/config/orm_test.config";
            String filePath = strUtil.Join( cfgHelper.FrameworkRoot, fileName );
            String dataPath = PathHelper.Map( strUtil.Join( SystemInfo.ApplicationPath, filePath ) );
            file.Write( dataPath, t.ToString() );

            echoAjaxOk();
        }

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

        private string createConnectionString( string dbType, string dbAddress, string dbName, string user, string pwd ) {

            if ("access".Equals( dbType )) {
                return string.Format( "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", dbName );
            }
            else if ("sqlserver".Equals( dbType ) || "sqlserver2000".Equals( dbType )) {
                return string.Format( "server={0};uid={1};pwd={2};database={3};", dbAddress, user, pwd, dbName );
            }
            else if ("mysql".Equals( dbType )) {
                return string.Format( "server={0};user={1};password={2};database={3};port=3306;Charset=utf8", dbAddress, user, pwd, dbName );
            }
            else {
                errors.Add( "请选择正确的数据库类型" );
                return "";
            }
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
            String dataPath = PathHelper.Map( strUtil.Join( SystemInfo.ApplicationPath, filePath ) );
            writeFilePrivate( dataPath );
        }

        // framework目录检测
        private void testFrameworkPath() {
            String fileName = getRandomFileName();
            String filePath = strUtil.Join( cfgHelper.FrameworkRoot, fileName );
            String dataPath = PathHelper.Map( strUtil.Join( SystemInfo.ApplicationPath, filePath ) );
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
            String fileName = "_test_write_file_" + rd.Next( 1000000, 9000000 ) + ".txt";
            return fileName;
        }

    }

}
