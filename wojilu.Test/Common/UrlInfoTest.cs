using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace wojilu.Test.Common {

    [TestFixture]
    public class UrlInfoTest {





        [Test]
        public void shortUrl() {

            Int64 num = 0;
            Assert.AreEqual( num, cvt.DeBase62( cvt.ToBase62( num ) ) );
            Console.WriteLine( num + "-->" + cvt.ToBase62( num ) );

            num = 1;
            Assert.AreEqual( num, cvt.DeBase62( cvt.ToBase62( num ) ) );
            Console.WriteLine( num + "-->" + cvt.ToBase62( num ) );

            num = 51;
            Assert.AreEqual( num, cvt.DeBase62( cvt.ToBase62( num ) ) );
            Console.WriteLine( num + "-->" + cvt.ToBase62( num ) );

            num = 63;
            Assert.AreEqual( num, cvt.DeBase62( cvt.ToBase62( num ) ) );
            Console.WriteLine( num + "-->" + cvt.ToBase62( num ) );

            num = 64;
            Assert.AreEqual( num, cvt.DeBase62( cvt.ToBase62( num ) ) );
            Console.WriteLine( num + "-->" + cvt.ToBase62( num ) );

            num = 120;
            Assert.AreEqual( num, cvt.DeBase62( cvt.ToBase62( num ) ) );
            Console.WriteLine( num + "-->" + cvt.ToBase62( num ) );

            num = 20308;
            Assert.AreEqual( num, cvt.DeBase62( cvt.ToBase62( num ) ) );
            Console.WriteLine( num + "-->" + cvt.ToBase62( num ) );

            num = 5000001;
            Assert.AreEqual( num, cvt.DeBase62( cvt.ToBase62( num ) ) );
            Console.WriteLine( num + "-->" + cvt.ToBase62( num ) );

            num = 1000555001;
            Assert.AreEqual( num, cvt.DeBase62( cvt.ToBase62( num ) ) );
            Console.WriteLine( num + "-->" + cvt.ToBase62( num ) );

            num = 1030535001;
            Assert.AreEqual( num, cvt.DeBase62( cvt.ToBase62( num ) ) );
            Console.WriteLine( num + "-->" + cvt.ToBase62( num ) );

            num = 1030339888434435001;
            Assert.AreEqual( num, cvt.DeBase62( cvt.ToBase62( num ) ) );
            Console.WriteLine( num + "-->" + cvt.ToBase62( num ) );

        }

        public void test() {

            Uri uri = new Uri( "http://zhangsan:123@www.wojilu.net/myapp/Photo/1984?title=eee#top" );

            UrlInfo u = new UrlInfo( uri, "/myapp/", "myPathInfo" );

            Console.WriteLine( "Scheme=>" + u.Scheme );
            Console.WriteLine( "UserName=>" + u.UserName );
            Console.WriteLine( "Password=>" + u.Password );
            Console.WriteLine( "Host=>" + u.Host );
            Console.WriteLine( "Port=>" + u.Port );
            Console.WriteLine( "Path=>" + u.Path );
            Console.WriteLine( "PathAndQuery=>" + u.PathAndQuery );
            Console.WriteLine( "PathInfo=>" + u.PathInfo );
            Console.WriteLine( "AppPath=>" + u.AppPath );

            Console.WriteLine( "PathAndQueryWithouApp=>" + u.PathAndQueryWithouApp );

            Console.WriteLine( "Query=>" + u.Query );
            Console.WriteLine( "Fragment=>" + u.Fragment );

            Console.WriteLine( "SiteUrl=>" + u.SiteUrl );
            Console.WriteLine( "SiteAndAppPath=>" + u.SiteAndAppPath );
            Console.WriteLine( "ToString=>" + u.ToString() );

            /*
            Scheme=>http
            UserName=>zhangsan
            Password=>123
            Host=>www.wojilu.net
            Port=>80
            Path=>/myapp/Photo/1984
            PathAndQuery=>/myapp/Photo/1984?title=eee
            PathInfo=>myPathInfo
            AppPath=>/myapp/
            PathAndQueryWithouApp=>/Photo/1984?title=eee
            Query=>?title=eee
            Fragment=>#top
            SiteUrl=>http://zhangsan:123@www.wojilu.net
            SiteAndAppPath=>http://zhangsan:123@www.wojilu.net/myapp/
            ToString=>http://zhangsan:123@www.wojilu.net/myapp/Photo/1984?title=eee#top
            
             */


        }

    }

}
