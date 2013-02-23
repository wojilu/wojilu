using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using wojilu.OAuth;
using wojilu.OAuth.Connects;
using wojilu.DI;

namespace wojilu.Test.OpenAuth {


    [TestFixture]
    public class QQWeiboTest {

        private static readonly ILog logger = LogManager.GetLogger( typeof( QQWeiboTest ) );

        // 入口
        public void test() {
            testAuth();
        }

        // access_token=xx&expires_in=888&refresh_token=xxx&openid=xxx&name=xx&nick=xx&state= 
        // 每次验证都返回不同的 access token
        private String accessToken {
            get { return "2521429152c7d1130965f95e68c626a7"; }
        }

        private String openId {
            get { return "3845c20a79ae511ee143cfcd390c6315"; }
        }

        [TestFixtureSetUp]
        public void initAccessToken() {

            if (strUtil.IsNullOrEmpty( accessToken ))
                throw new ArgumentNullException( "请先获取并 accessToken 的值" );
        }


        // 测试之前，请先配置 QQWeiboConnect
        public void testAuth() {

            Console.WriteLine( "请按任意键开始用户授权……" );
            Console.ReadLine();

            QQWeiboConnect connect = AuthConnectFactory.GetConnect( typeof( QQWeiboConnect ).FullName ) as QQWeiboConnect;
            if (connect == null) throw new Exception( "尚未配置" );

            Console.WriteLine( "启动浏览器..." );
            connect.CallbackUrl = "http://127.0.0.1/oauth/callback";
            Process.Start( connect.GetAuthorizationFullUrl() );

            Console.Write( "请拷贝浏览器地址栏code的值，粘贴到此处:" );
            string code = Console.ReadLine();
            Console.WriteLine( "授权的code=" + code );


            Console.WriteLine( "开始获取access token……" );

            // 必须同时保存 openId ，调用 API 时可以用到
            AccessToken result = OAuthClient.New().GetAccessToken( connect, code );
            Console.WriteLine( "access token=" + result.Token );
            logger.Info( "access token=" + result.Token );

            Console.WriteLine( "请按 Enter 键结束……" );
            Console.ReadLine();
        }


        [Test]
        public void testProfile() {
            Console.WriteLine( "请按任意键，开始发布……" );
            Console.ReadLine();

            QQWeiboConnect connect = AuthConnectFactory.GetConnect( typeof( QQWeiboConnect ).FullName ) as QQWeiboConnect;
            if (connect == null) throw new Exception( "尚未配置" );

            AccessToken x = new AccessToken();
            x.Token = accessToken;
            x.Uid = openId;

            OAuthUserProfile obj = connect.GetUserProfile( x );

            Console.WriteLine( "id=" + obj.Uid );
            Console.WriteLine( "name=" + obj.Name );

            // 用户头像(小50×50像素)
            Console.WriteLine( "profile_image_url=" + obj.PicUrlSmall );
            // 用户头像(大)
            Console.WriteLine( "avatar_large=" + obj.PicUrlBig );

            Console.WriteLine( "domain=" + obj.FriendlyUrl );

            Console.WriteLine( "请按 Enter 键结束……" );
            Console.ReadLine();



        }

        // 测试之前，请先获取 access token，并替换本页源码中的 accessToken 属性
        [Test]
        public void testPublish() {

            Console.WriteLine( "请按任意键，开始发布……" );
            Console.ReadLine();

            QQWeiboConnect connect = AuthConnectFactory.GetConnect( typeof( QQWeiboConnect ).FullName ) as QQWeiboConnect;
            if (connect == null) throw new Exception( "尚未配置" );

            AccessToken x = new AccessToken { Token = accessToken, Uid = openId };
            JsonObject jsonValue = connect.PublishPost( x, "这是测试的微博内容" + DateTime.Now );

            Assert.AreEqual( "ok", jsonValue["msg"] );
        }

        // 测试之前，请先
        // 1) 获取 access token，并替换本页源码中的 accessToken 属性
        // 2) 将某图片保存到 c:\testpic.jpg，以供测试
        [Test]
        public void testUploadPic() {

            Console.WriteLine( "请按任意键开始……" );
            Console.ReadLine();

            QQWeiboConnect connect = AuthConnectFactory.GetConnect( typeof( QQWeiboConnect ).FullName ) as QQWeiboConnect;
            if (connect == null) throw new Exception( "尚未配置" );

            AccessToken x = new AccessToken { Token = accessToken, Uid = openId };
            JsonObject jsonValue = connect.PublishPic( x, "这是测试的微博内容" + DateTime.Now, "c:\\testpic.jpg" );

            Assert.AreEqual( "ok", jsonValue["msg"].ToString() );
        }

        //-----------------------------------------------------------------------------------------


        // 测试之前，请先获取 access token，并替换本页源码中的 accessToken 属性
        [Test]
        public void testGetPublicData() {

            Console.WriteLine( "请按任意键，开始获取最新公共微博……" );
            Console.ReadLine();

            QQWeiboConnect connect = AuthConnectFactory.GetConnect( typeof( QQWeiboConnect ).FullName ) as QQWeiboConnect;
            if (connect == null) throw new Exception( "尚未配置" );

            List<JsonObject> list = OAuthClient.Init( "https://open.t.qq.com/api/statuses/home_timeline", accessToken, "GET" )
                .AddParam( defaultParams( connect ) )
                .AddParam( "pageflag", "0" )
                .AddParam( "pagetime", "0" )
                .AddParam( "reqnum", "20" )
                .AddParam( "type", "3" )
                .AddParam( "contenttype", "0x80" )
                .RunJson()
                .GetJson( "data" )
                .GetList<JsonObject>( "info" );

            foreach (JsonObject x in list) {

                String userName = x.Get( "name" );
                String blogBody = x.Get( "text" );

                Assert.IsNotNull( blogBody );
                Assert.IsNotNull( userName );

                Console.WriteLine( "---------------------------------------------------------" );
                Console.WriteLine( "[[" + userName + "]]: " + blogBody );
            }
        }


        // 测试之前，请先获取 access token，并替换本页源码中的 accessToken 属性
        [Test]
        public void testDelete() {

            Console.WriteLine( "请按任意键开始……" );
            Console.ReadLine();

            // 1）现发布一条微博
            QQWeiboConnect connect = AuthConnectFactory.GetConnect( typeof( QQWeiboConnect ).FullName ) as QQWeiboConnect;
            if (connect == null) throw new Exception( "尚未配置" );

            JsonObject jsonValue = OAuthClient.Init( "https://open.t.qq.com/api/t/add", accessToken, "POST" )
                .AddParam( defaultParams( connect ) )
                .AddParam( "content", "这是测试的微博内容" + DateTime.Now )
                .RunJson();

            Assert.AreEqual( "ok", jsonValue["msg"] );

            // 2）获取此微博的id
            JsonObject data = jsonValue.GetJson( "data" );
            long currentId = data.Get<long>( "id" );
            Assert.IsTrue( currentId > 0 );

            // 3）删除刚刚发布的微博
            Console.WriteLine( "开始删除……" + currentId );

            JsonObject obj = OAuthClient.Init( "https://open.t.qq.com/api/t/del", accessToken, "POST" )
                .AddParam( defaultParams( connect ) )
                .AddParam( "id", currentId )
                .RunJson();

            Assert.AreEqual( "ok", obj.Get( "msg" ) );
        }

        [Test]
        public void testGetMyStatus() {

            QQWeiboConnect connect = AuthConnectFactory.GetConnect( typeof( QQWeiboConnect ).FullName ) as QQWeiboConnect;
            if (connect == null) throw new Exception( "尚未配置" );

            List<JsonObject> list = OAuthClient.Init( "http://open.t.qq.com/api/statuses/broadcast_timeline", accessToken, "GET" )
                .AddParam( defaultParams( connect ) )
                .AddParam( "pageflag", 0 )
                .AddParam( "pagetime", 0 )
                .AddParam( "reqnum", 20 )
                .AddParam( "lastid", 0 )
                .AddParam( "type", 3 )
                .AddParam( "contenttype", "0x80" )
                .RunJson()
                .GetJson( "data" )
                .GetList<JsonObject>( "info" );

            foreach (JsonObject x in list) {

                String userName = x.Get( "name" );
                String blogBody = x.Get( "text" );

                Assert.IsNotNull( blogBody );
                Assert.IsNotNull( userName );

                Console.WriteLine( "---------------------------------------------------------" );
                Console.WriteLine( "[[" + userName + "]]: " + blogBody );
            }

        }


        // 必须的默认参数
        private Dictionary<String, String> defaultParams( QQWeiboConnect connect ) {
            Dictionary<String, String> items = new Dictionary<String, String>();
            items.Add( "oauth_consumer_key", connect.ConsumerKey );
            //items.Add( "access_token", accessToken );
            items.Add( "oauth_version", "2.a" );
            items.Add( "scope", "all" );
            items.Add( "openid", openId );
            items.Add( "format", "json" );
            items.Add( "clientip", "8.8.8.8" );
            return items;
        }

    }


}
