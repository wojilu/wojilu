using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Data;
using NUnit.Framework;

namespace wojilu.Test.Orm {

    /// <summary>
    /// 连接字符串参考 http://connectionstrings.com/
    /// </summary>
    [TestFixture]
    public class ConnectionStringTest {



        // 此处右键测试




        // 数据库连接格式
        // server=127.0.0.1;database=数据库名称; uid=用户名;pwd=密码;

        // 或者(带端口)
        // server=127.0.0.1,1458;database=数据库名称; uid=用户名;pwd=密码;

        // 或者(同义词)
        // data source=127.0.0.1,1458;Initial Catalog=数据库名称; User ID=用户名;Password=密码;

        // 或者(信任模式)
        // Data Source=.;Initial Catalog=mytest;Trusted_Connection=True
        // Data Source=.;Initial Catalog=mytest;Integrated Security=True

        // 或者(SQLExpress，反斜杠需要转义)
        // Server=.\\SQLExpress;AttachDbFilename=C:\\MyFolder\\MyDataFile.mdf;Database=dbname;Trusted_Connection=Yes;";

        /* 同义词表
        +----------------------+-------------------------+
        | Value                | Synonym                 |
        +----------------------+-------------------------+
        | app                  | application name        |
        | async                | asynchronous processing |
        | extended properties  | attachdbfilename        |
        | initial file name    | attachdbfilename        |
        | connection timeout   | connect timeout         |
        | timeout              | connect timeout         |
        | language             | current language        |
        | addr                 | data source             |
        | address              | data source             |
        | network address      | data source             |
        | server               | data source             |
        | database             | initial catalog         |
        | trusted_connection   | integrated security     |
        | connection lifetime  | load balance timeout    |
        | net                  | network library         |
        | network              | network library         |
        | pwd                  | password                |
        | persistsecurityinfo  | persist security info   |
        | uid                  | user id                 |
        | user                 | user id                 |
        | wsid                 | workstation id          |
        +----------------------+-------------------------+
        */

        [Test]
        public void testDbConfig() {

            checkConnectionString( "Server=localhost;uid=用户名;pwd=你的密码;Database=数据库名称;" );
            checkConnectionString( "server=192.168.1.122,1230;uid=myname;pwd=abc111;database=mydb;" );

            checkConnectionString( "Data Source=190.190.200.100,1433;Network Library=DBMSSOCN;Initial Catalog=myDataBase;User ID=myUsername;Password=myPassword;" );

            // 反斜杠需要转义
            checkConnectionString( @"server=localhost\\SQL2005;uid=abctest;pwd=***;database=dbnametest;", @"server=localhost\SQL2005;uid=abctest;pwd=***;database=dbnametest;" );

            checkConnectionString( "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;" );
            checkConnectionString( "Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;" );
        }

        private void checkConnectionString( String connString ) {
            checkConnectionString( connString, null );
        }

        private void checkConnectionString( String connString, String experted ) {
            String[] arrStr = getConfigTemplate();
            String str = arrStr[0] + connString + arrStr[1];
            DbConfig x = DbConfig.loadConfigByString( str );

            if (experted == null) {
                Assert.AreEqual( connString, x.ConnectionStringTable["default"] );
            }
            else {
                Assert.AreEqual( experted, x.ConnectionStringTable["default"] );
            }
        }


        [Test]
        public void getDatabase() {

            String str = "server=localhost\\SQL2005;uid=abctest;pwd=***;database=dbnametest;";

            IDatabaseDialect x = new SQLServerDialect();

            String dbname = x.GetConnectionItem( str, ConnectionItemType.Database );
            Assert.AreEqual( dbname, "dbnametest" );

            String dbserver = x.GetConnectionItem( str, ConnectionItemType.Server );
            Assert.AreEqual( dbserver, "localhost\\SQL2005" );

            String userId = x.GetConnectionItem( str, ConnectionItemType.UserId );
            Assert.AreEqual( userId, "abctest" );

            String pwd = x.GetConnectionItem( str, ConnectionItemType.Password );
            Assert.AreEqual( pwd, "***" );


        }

        [Test]
        public void testPort() {

            String str = "server=192.168.1.122,1230;uid=myname;pwd=abc111;database=mydb;";

            IDatabaseDialect x = new SQLServerDialect();

            String dbname = x.GetConnectionItem( str, ConnectionItemType.Database );
            Assert.AreEqual( dbname, "mydb" );

            String dbserver = x.GetConnectionItem( str, ConnectionItemType.Server );
            Assert.AreEqual( dbserver, "192.168.1.122,1230" );

            String userId = x.GetConnectionItem( str, ConnectionItemType.UserId );
            Assert.AreEqual( userId, "myname" );

            String pwd = x.GetConnectionItem( str, ConnectionItemType.Password );
            Assert.AreEqual( pwd, "abc111" );

        }

        [Test]
        public void testPort2() {

            String str = "Data Source=190.190.200.100,1433;Network Library=DBMSSOCN;Initial Catalog=myDataBase;User ID=myUsername;Password=myPassword;";

            IDatabaseDialect x = new SQLServerDialect();

            String dbname = x.GetConnectionItem( str, ConnectionItemType.Database );
            Assert.AreEqual( dbname, "myDataBase" );

            String dbserver = x.GetConnectionItem( str, ConnectionItemType.Server );
            Assert.AreEqual( dbserver, "190.190.200.100,1433" );

            String userId = x.GetConnectionItem( str, ConnectionItemType.UserId );
            Assert.AreEqual( userId, "myUsername" );

            String pwd = x.GetConnectionItem( str, ConnectionItemType.Password );
            Assert.AreEqual( pwd, "myPassword" );

        }


        [Test]
        public void testTrust() {

            // 下面也可以
            // Data Source=.;Initial Catalog=mytest;Integrated Security=True
            // Data Source=.;Initial Catalog=mytest;Trusted_Connection=True

            String str = "Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;";

            IDatabaseDialect x = new SQLServerDialect();

            String dbname = x.GetConnectionItem( str, ConnectionItemType.Database );
            Assert.AreEqual( dbname, "myDataBase" );

            String dbserver = x.GetConnectionItem( str, ConnectionItemType.Server );
            Assert.AreEqual( dbserver, "myServerAddress" );

            String IsTrusted = x.GetConnectionItem( str, ConnectionItemType.IsTrusted );
            Assert.AreEqual( IsTrusted, "True" );


            String IsTrusted2 = x.GetConnectionItem( "Data Source=myServerAddress;Initial Catalog=myDataBase;Integrated Security=True", ConnectionItemType.IsTrusted );
            Assert.AreEqual( IsTrusted2, "True" );
        }

        [Test]
        public void testInstance() {

            String str = "Server=myServerName\\myInstanceName;Database=myDataBase;User Id=myUsername; Password=myPassword;";

            IDatabaseDialect x = new SQLServerDialect();

            String dbname = x.GetConnectionItem( str, ConnectionItemType.Database );
            Assert.AreEqual( dbname, "myDataBase" );

            String dbserver = x.GetConnectionItem( str, ConnectionItemType.Server );
            Assert.AreEqual( dbserver, "myServerName\\myInstanceName" );

            String userId = x.GetConnectionItem( str, ConnectionItemType.UserId );
            Assert.AreEqual( userId, "myUsername" );

            String pwd = x.GetConnectionItem( str, ConnectionItemType.Password );
            Assert.AreEqual( pwd, "myPassword" );
        }

        [Test]
        public void testExpress() {

            String str = "Server=.\\SQLExpress;AttachDbFilename=C:\\MyFolder\\MyDataFile.mdf;Database=dbname;Trusted_Connection=Yes;";

            IDatabaseDialect x = new SQLServerDialect();

            String dbname = x.GetConnectionItem( str, ConnectionItemType.Database );
            Assert.AreEqual( dbname, "dbname" );

            String dbserver = x.GetConnectionItem( str, ConnectionItemType.Server );
            Assert.AreEqual( dbserver, ".\\SQLExpress" );

            String IsTrusted = x.GetConnectionItem( str, ConnectionItemType.IsTrusted );
            Assert.AreEqual( IsTrusted, "Yes" );
        }


        [Test]
        public void testOther() {

            String str = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";

            IDatabaseDialect x = new SQLServerDialect();

            String dbname = x.GetConnectionItem( str, ConnectionItemType.Database );
            Assert.AreEqual( dbname, "myDataBase" );

            String dbserver = x.GetConnectionItem( str, ConnectionItemType.Server );
            Assert.AreEqual( dbserver, "myServerAddress" );

            String userId = x.GetConnectionItem( str, ConnectionItemType.UserId );
            Assert.AreEqual( userId, "myUsername" );

            String pwd = x.GetConnectionItem( str, ConnectionItemType.Password );
            Assert.AreEqual( pwd, "myPassword" );

            //String str2 = "Server=.;Database=myDataBase;User Id=myUsername;Password=myPassword;";
        }

        [Test]
        public void testOther2() {

            String str = "Server=.;Database=myDataBase;User Id=myUsername;Password=myPassword;";

            IDatabaseDialect x = new SQLServerDialect();

            String dbname = x.GetConnectionItem( str, ConnectionItemType.Database );
            Assert.AreEqual( dbname, "myDataBase" );

            String dbserver = x.GetConnectionItem( str, ConnectionItemType.Server );
            Assert.AreEqual( dbserver, "." );

            String userId = x.GetConnectionItem( str, ConnectionItemType.UserId );
            Assert.AreEqual( userId, "myUsername" );

            String pwd = x.GetConnectionItem( str, ConnectionItemType.Password );
            Assert.AreEqual( pwd, "myPassword" );
        }

        private String[] getConfigTemplate() {
            String[] arr = new String[2];
            arr[0] = @"{
    ConnectionStringTable : {
        default:""";

            arr[1] = @"""
    },
    
    DbType : {
        // 根据不同的数据库，你可以选填：sqlserver/sqlserver2000/access/mysql 四种之一
        default:""sqlserver""
    },
    
    Mapping:[
    ],
    
    // 是否启用二级二级缓存
    ApplicationCache:true,
    
    // 二级缓存的过期时间，如果想永久性缓存，请填写-999
    ApplicationCacheMinutes:-999,
    ApplicationCacheManager:"""",
    
    AssemblyList : [""wojilu.Core"",""wojilu.Apps"",""wojilu.Apps.Download""],

    Interceptor:[ 
    ]


}
";

            return arr;
        }

    }

}
