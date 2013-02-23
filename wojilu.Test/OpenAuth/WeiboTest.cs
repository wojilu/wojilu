using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using wojilu.OAuth;
using wojilu.OAuth.Connects;
using wojilu.DI;

namespace wojilu.Test.OpenAuth {


    [TestFixture]
    public class WeiboTest {







        // 入口
        public void test() {
            testAuth();
        }

        private String accessToken {
            get { return "2.00HjPWVDN2uziD598cbae2dfI9CHMD"; }
        }

        [TestFixtureSetUp]
        public void initAccessToken() {

            if (strUtil.IsNullOrEmpty( accessToken ))
                throw new ArgumentNullException( "请先获取并 accessToken 的值" );
        }


        // 测试之前，请先配置 WeiboConnect
        public void testAuth() {

            Console.WriteLine( "请按任意键开始用户授权……" );
            Console.ReadLine();

            WeiboConnect connect = AuthConnectFactory.GetConnect( typeof( WeiboConnect ).FullName ) as WeiboConnect;
            if (connect == null) throw new Exception( "尚未配置" );

            Console.WriteLine( "启动浏览器..." );
            connect.CallbackUrl = "http://127.0.0.1/oauth/callback";
            Process.Start( connect.GetAuthorizationFullUrl() );

            Console.Write( "请拷贝浏览器地址栏code的值，粘贴到此处:" );
            string code = Console.ReadLine();
            Console.WriteLine( "授权的code=" + code );


            Console.WriteLine( "开始获取access token……" );

            AccessToken result = OAuthClient.New().GetAccessToken( connect, code );
            Console.WriteLine( "access token=" + result.Token );
            Console.WriteLine( "uid=" + result.Uid );


            Console.WriteLine( "请按 Enter 键结束……" );
            Console.ReadLine();
        }

        [Test]
        public void testProfile() {

            // 如果报错 source paramter(appkey) is missing，很有可能是 access token 过期

            Console.WriteLine( "请按任意键，开始发布……" );
            Console.ReadLine();

            WeiboConnect connect = AuthConnectFactory.GetConnect( typeof( WeiboConnect ).FullName ) as WeiboConnect;
            if (connect == null) throw new Exception( "尚未配置" );

            AccessToken x = new AccessToken();
            x.Token = accessToken;
            x.Uid = "3214151865";

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

            WeiboConnect connect = AuthConnectFactory.GetConnect( typeof( WeiboConnect ).FullName ) as WeiboConnect;
            String content = "这是测试的微博内容" + DateTime.Now;
            JsonObject jsonValue = connect.PublishPost( accessToken, content );
            Assert.AreEqual( content, jsonValue.Get( "text" ) );

        }

        // 测试之前，请先
        // 1) 获取 access token，并替换本页源码中的 accessToken 属性
        // 2) 将某图片保存到 c:\testpic.jpg，以供测试
        [Test]
        public void testUploadPic() {

            WeiboConnect connect = AuthConnectFactory.GetConnect( typeof( WeiboConnect ).FullName ) as WeiboConnect;
            String content = "这是测试的微博内容" + DateTime.Now;
            JsonObject jsonValue = connect.PublishPic( accessToken, content, "c:\\testpic.jpg" );
            Assert.AreEqual( content, jsonValue.Get( "text" ) );

            //-------------------------------
            //Console.WriteLine( "获取最新的微博..." );
            //items = new Dictionary<String, String>();
            //items.Add( "id", getFirstId() );

            //result = OAuth.GetApi( accessToken, "https://api.weibo.com/2/statuses/show.json", items );
            //jsonValue = Json.DeserializeJson( result );
            //Assert.AreEqual( content, jsonValue.Get( "text" ) );
        }


        //------------------------------ 其他测试 ----------------------------------------------------------

        // 测试之前，请先获取 access token，并替换本页源码中的 accessToken 属性
        [Test]
        public void testGetPublicData() {

            JsonObject jsonValue = OAuthClient
                .Init( "https://api.weibo.com/2/statuses/public_timeline.json", accessToken, "GET" )
                .RunJson();

            List<Object> list = jsonValue.GetList( "statuses" );
            Assert.IsTrue( list.Count > 10 ); // 有时候居然返回19条

            foreach (Object item in list) {

                JsonObject x = item as JsonObject;

                JsonObject user = x.GetJson( "user" );
                String userName = user.Get( "name" );
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

            String firstId = getFirstId();
            Assert.IsNotNull( firstId );

            Console.WriteLine( "开始删除……" + firstId );

            String response = OAuthClient
                .Init( "https://api.weibo.com/2/statuses/destroy.json", accessToken, "POST" )
                .AddParam( "id", firstId )
                .Run();

            Console.WriteLine( "删除成功:" + response );
        }

        private String getFirstId() {

            JsonObject json = OAuthClient
                .Init( "https://api.weibo.com/2/statuses/user_timeline/ids.json", accessToken, "GET" )
                .RunJson();

            List<Object> ids = json.GetList( "statuses" );
            foreach (Object x in ids) {
                Console.WriteLine( x );
            }

            if (ids.Count == 0) {
                Console.WriteLine( "没有数据可以删除" );
                return null;
            }

            return ids[0].ToString();
        }

    }


}
