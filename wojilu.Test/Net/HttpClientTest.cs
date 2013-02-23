using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Net;
using NUnit.Framework;
using wojilu.Aop;

namespace wojilu.Test.Net {



    [TestFixture]
    public class HttpClientTest {






        [Test]
        public void testGetUrl() {

            HttpClient client = HttpClient.Init( "http://www.abc.com/xy.aspx", "GET" );
            Assert.AreEqual( "http://www.abc.com/xy.aspx", client.GetRequestUrl() );

            // 在 GET 情况下，param 和 query string 都会拼接

            client = HttpClient.Init( "http://www.abc.com/xy.aspx", "GET" );
            client.AddParam( "name", "zhangsan" );
            client.AddParam( "gender", 1 );
            client.AddParam( "day", "2012-12-12" );
            Assert.AreEqual( "http://www.abc.com/xy.aspx?name=zhangsan&gender=1&day=2012-12-12", client.GetRequestUrl() );

            client = HttpClient.Init( "http://www.abc.com/xy.aspx", "GET" );
            client.AddQuery( "name", "zhangsan" );
            client.AddQuery( "gender", 1 );
            client.AddQuery( "day", "2012-12-12" );
            Assert.AreEqual( "http://www.abc.com/xy.aspx?name=zhangsan&gender=1&day=2012-12-12", client.GetRequestUrl() );

            client = HttpClient.Init( "http://www.abc.com/xy.aspx", "GET" );
            client.AddParam( "name", "zhangsan" );
            client.AddQuery( "gender", 1 );
            client.AddParam( "day", "2012-12-12" );
            client.AddQuery( "location", "beijing" );
            // 先拼接 query，然后拼接 params
            Assert.AreEqual( "http://www.abc.com/xy.aspx?gender=1&location=beijing&name=zhangsan&day=2012-12-12", client.GetRequestUrl() );

            client = HttpClient.Init( "http://www.abc.com/xy.aspx?z1=v1&z2=v2", "GET" );
            client.AddQuery( "name", "zhangsan" );
            client.AddQuery( "gender", 1 );
            client.AddQuery( "day", "2012-12-12" );
            Assert.AreEqual( "http://www.abc.com/xy.aspx?z1=v1&z2=v2&name=zhangsan&gender=1&day=2012-12-12", client.GetRequestUrl() );


        }

        [Test]
        public void testPostUrl() {

            HttpClient client = HttpClient.Init( "http://www.abc.com/xy.aspx", "POST" );
            Assert.AreEqual( "http://www.abc.com/xy.aspx", client.GetRequestUrl() );


            // 在 POST 情况下，仅拼接 query string

            client = HttpClient.Init( "http://www.abc.com/xy.aspx", "POST" );
            client.AddParam( "name", "zhangsan" );
            client.AddParam( "gender", 1 );
            client.AddParam( "day", "2012-12-12" );
            Assert.AreEqual( "http://www.abc.com/xy.aspx", client.GetRequestUrl() );

            client = HttpClient.Init( "http://www.abc.com/xy.aspx", "POST" );
            client.AddQuery( "name", "zhangsan" );
            client.AddQuery( "gender", 1 );
            client.AddQuery( "day", "2012-12-12" );
            Assert.AreEqual( "http://www.abc.com/xy.aspx?name=zhangsan&gender=1&day=2012-12-12", client.GetRequestUrl() );

            client = HttpClient.Init( "http://www.abc.com/xy.aspx", "POST" );
            client.AddParam( "name", "zhangsan" );
            client.AddQuery( "gender", 1 );
            client.AddParam( "day", "2012-12-12" );
            client.AddQuery( "location", "beijing" );
            Assert.AreEqual( "http://www.abc.com/xy.aspx?gender=1&location=beijing", client.GetRequestUrl() );

            // 如果不指定 HttpMethod，则使用 POST
            client = HttpClient.Init( "http://www.abc.com/xy.aspx", null );
            client.AddParam( "name", "zhangsan" );
            client.AddQuery( "gender", 1 );
            client.AddParam( "day", "2012-12-12" );
            client.AddQuery( "location", "beijing" );
            Assert.AreEqual( "http://www.abc.com/xy.aspx?gender=1&location=beijing", client.GetRequestUrl() );

            client = HttpClient.Init( "http://www.abc.com/xy.aspx?z1=v1&z2=v2", "POST" );
            client.AddParam( "name", "zhangsan" );
            client.AddQuery( "gender", 1 );
            client.AddParam( "day", "2012-12-12" );
            client.AddQuery( "location", "beijing" );
            Assert.AreEqual( "http://www.abc.com/xy.aspx?z1=v1&z2=v2&gender=1&location=beijing", client.GetRequestUrl() );
        }


        [Test]
        public void testOtherUrl() {

            // 会去掉空格
            HttpClient client = HttpClient.Init( " abc ", "POST" );
            Assert.AreEqual( "abc", client.GetRequestUrl() );

            client = HttpClient.Init( " http://www.abc.com/xy.aspx             ", "POST" );
            Assert.AreEqual( "http://www.abc.com/xy.aspx", client.GetRequestUrl() );

        }


        [Test]
        public void testDownload() {

            // 抓取百度页面内容
            String page = HttpClient.Init( "http://www.baidu.com " ).Run();

            Console.WriteLine( page );
            Assert.IsTrue( page.Length > 200 );


        }

    }

}
