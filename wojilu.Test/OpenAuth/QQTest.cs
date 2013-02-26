using System;
using System.Diagnostics;
using NUnit.Framework;
using wojilu.OAuth;
using wojilu.OAuth.Connects;
using wojilu.DI;

namespace wojilu.Test.OpenAuth {




    // 官网： http://connect.qq.com/ 
    // 用户管理授权 http://rc.qzone.qq.com/profile/link

    [TestFixture]
    public class QQTest {

        private static readonly ILog logger = LogManager.GetLogger( typeof( QQTest ) );

        // 入口
        public void test() {
            testAuth();
        }

        private String accessToken {
            get { return "E7857D2F0535176150AC6F5B6EE992AA"; }
        }

        [TestFixtureSetUp]
        public void initAccessToken() {

            if (strUtil.IsNullOrEmpty( accessToken ))
                throw new ArgumentNullException( "请先获取并 accessToken 的值" );
        }

        // 测试之前，请先配置 QQConnect
        public void testAuth() {

            Console.WriteLine( "请按任意键开始用户授权……" );
            Console.ReadLine();

            QQConnect connect = AuthConnectFactory.GetConnect( typeof( QQConnect ).FullName ) as QQConnect;
            if (connect == null) throw new Exception( "尚未配置" );

            Console.WriteLine( "启动浏览器..." );
            connect.CallbackUrl = "http://www.wojilu.com";
            Process.Start( connect.GetAuthorizationFullUrl() );

            Console.Write( "请拷贝浏览器地址栏code的值，粘贴到此处:" );
            string code = Console.ReadLine();
            Console.WriteLine( "授权的code=" + code );


            Console.WriteLine( "开始获取access token……" );

            AccessToken result = OAuthClient.New().GetAccessToken( connect, code, connect.HttpMethod_AccessToken );
            Console.WriteLine( "access token=" + result.Token );
            Console.WriteLine( "uid=" + result.Uid );


            // 获取openId
            String openIdUrl = "https://graph.qq.com/oauth2.0/me?access_token=" + result.Token;
            String response = wojilu.Net.PageLoader.Download( openIdUrl );

            Console.WriteLine( "获取openid返回结果=" + response );

            String strJson = strUtil.TrimStart( response.Trim().TrimEnd( ';' ), "callback(" ).TrimEnd( ')' );
            JsonObject obj = Json.ParseJson( strJson );
            String openIdMsg = "openid=" + obj.Get( "openid" );
            Console.WriteLine( openIdMsg );
            logger.Info( openIdMsg );

            //---------------------------------------------------------------------

            Console.WriteLine( "请按 Enter 键结束……" );
            Console.ReadLine();
        }

        [Test]
        public void testProfile() {

            Console.WriteLine( "请按任意键，开始发布……" );
            Console.ReadLine();

            QQConnect connect = AuthConnectFactory.GetConnect( typeof( QQConnect ).FullName ) as QQConnect;
            if (connect == null) throw new Exception( "尚未配置" );

            AccessToken x = new AccessToken();
            x.Token = accessToken;

            OAuthUserProfile obj = connect.GetUserProfile( x );
            Assert.IsNotNull( obj );

            Console.WriteLine( "id=" + obj.Uid );
            Console.WriteLine( "name=" + obj.Name );

            Assert.IsNotNull( obj.Uid );
            Assert.IsNotNull( obj.Name );
            Assert.IsTrue( obj.Name.Length > 0 );

            // 用户头像(小50×50像素)
            Console.WriteLine( "profile_image_url=" + obj.PicUrlSmall );
            Assert.IsNotNull( obj.PicUrlSmall );
            Assert.IsTrue( obj.PicUrlSmall.Length > 0 );

            // 用户头像(大)
            Console.WriteLine( "avatar_large=" + obj.PicUrlBig );
            Assert.IsNotNull( obj.PicUrlBig );
            Assert.IsTrue( obj.PicUrlBig.Length > 0 );

            Console.WriteLine( "domain=" + obj.FriendlyUrl );

            Console.WriteLine( "请按 Enter 键结束……" );
            Console.ReadLine();

        }

        // 测试之前，请先获取 access token，并替换本页源码中的 accessToken 属性
        [Test]
        public void testPublish() {

            QQConnect connect = AuthConnectFactory.GetConnect( typeof( QQConnect ).FullName ) as QQConnect;
            if (connect == null) throw new Exception( "尚未配置" );

            AccessToken x = new AccessToken { Token = accessToken };
            String content = "这是来自QQ互联api的微博内容" + DateTime.Now;

            JsonObject jsonValue = connect.PublishPost( x, content );
            Assert.AreEqual( "ok", jsonValue.Get( "msg" ) );
        }


        // 测试之前，请先
        // 1) 获取 access token，并替换本页源码中的 accessToken 属性
        // 2) 将某图片保存到 c:\testpic.jpg，以供测试
        [Test]
        public void testUploadPic() {

            QQConnect connect = AuthConnectFactory.GetConnect( typeof( QQConnect ).FullName ) as QQConnect;
            if (connect == null) throw new Exception( "尚未配置" );

            AccessToken x = new AccessToken { Token = accessToken };
            String content = "这是来自QQ互联api的微博内容" + DateTime.Now;

            JsonObject jsonValue = connect.PublishPic( x, content, "c:\\testpic.jpg" );
            Assert.AreEqual( "ok", jsonValue.Get( "msg" ) );
        }


    }


}
