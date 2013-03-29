using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Net;
using NUnit.Framework;
using wojilu.Aop;
using System.Web;

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

            // 如果不指定 HttpMethod，则使用 GET
            client = HttpClient.Init( "http://www.abc.com/xy.aspx", null );
            client.AddParam( "name", "zhangsan" );
            client.AddQuery( "gender", 1 );
            client.AddParam( "day", "2012-12-12" );
            client.AddQuery( "location", "beijing" );
            Assert.AreEqual( "http://www.abc.com/xy.aspx?gender=1&location=beijing&name=zhangsan&day=2012-12-12", client.GetRequestUrl() );

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

        [Test]
        public void testAddObject() {

            TAddObject x = new TAddObject();
            x.TFId = 2;
            x.TFMoney = 2.88M;
            x.TFName = "fname";
            x.TFState = true;
            x.TFTime = DateTime.Now.AddDays( -1 );

            x.TPId = 5;
            x.TPMoney = 5.88M;
            x.TPName = "pname";
            x.TPState = false;
            x.TPTime = DateTime.Now.AddDays( -5 );

            HttpClient client = HttpClient.Init( "http://www.abc.com", "GET" );
            client.AddObject( x );

            //String expected = "http://www.abc.com?TPId=5&TPName=pname&TPTime=2013%2f2%2f24+19%3a56%3a00&TPMoney=5.88&TPState=False&TFId=2&TFName=fname&TFTime=2013%2f2%2f28+19%3a56%3a00&TFMoney=2.88&TFState=True";
            String requestUrl = client.GetRequestUrl();
            Console.WriteLine( requestUrl );

            String[] arr = requestUrl.Split( '?' );
            Assert.AreEqual( 2, arr.Length );

            var qlist = HttpUtility.ParseQueryString( arr[1] );

            Assert.AreEqual( x.TFId, cvt.ToInt( qlist["TFId"] ) );
            Assert.AreEqual( x.TFMoney, cvt.ToDecimal( qlist["TFMoney"] ) );
            Assert.AreEqual( x.TFName, qlist["TFName"] );
            Assert.AreEqual( x.TFState, cvt.ToBool( qlist["TFState"] ) );
            Assert.IsTrue( isTimeEqual( x.TFTime, cvt.ToTime( qlist["TFTime"] ) ) );

            Assert.AreEqual( x.TPId, cvt.ToInt( qlist["TPId"] ) );
            Assert.AreEqual( x.TPMoney, cvt.ToDecimal( qlist["TPMoney"] ) );
            Assert.AreEqual( x.TPName, qlist["TPName"] );
            Assert.AreEqual( x.TPState, cvt.ToBool( qlist["TPState"] ) );
            Assert.IsTrue( isTimeEqual( x.TPTime, cvt.ToTime( qlist["TPTime"] ) ) );

        }

        private bool isTimeEqual( DateTime x, DateTime y ) {

            return x.Year == y.Year &&
                x.Month == y.Month &&
                x.Day == y.Day &&
                x.Hour == y.Hour &&
                x.Minute == y.Minute &&
                x.Second == y.Second;

        }

    }


    public class TAddObject {

        public int TFId;
        public String TFName;
        public DateTime TFTime;
        public decimal TFMoney;
        public Boolean TFState;

        public int TPId { get; set; }
        public String TPName { get; set; }
        public DateTime TPTime { get; set; }
        public decimal TPMoney { get; set; }
        public Boolean TPState { get; set; }

    }

}
