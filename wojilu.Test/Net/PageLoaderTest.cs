using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Net;
using System.Net;
using System.IO;
using wojilu.Common.Spider.Domain;
using wojilu.Common.Spider.Service;

namespace wojilu.Test.Net {

    [TestFixture]
    [Ignore("本方法是网络测试，需要先连接到互联网才能进行。联网之后，请删除本批注")]
    public class PageLoaderTest {











        [Test]
        public void testPage() {

            String page = PageLoader.Download( "http://news.ifeng.com/toprank/day/", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)", "utf-8" );
            Assert.IsNotEmpty( page );
            Assert.Greater( page.IndexOf( "点击排行榜" ), 0 );

        }

        [Test]
        public void testEncoding() {

            // 可以提供encoding，如果不提供，根据 WebResponse 的 ContentType 进行设置。

            String page = PageLoader.Download( "http://www.baidu.com/" );
            Assert.IsNotEmpty( page );
            Assert.Greater( page.IndexOf( "百度" ), 0 );

            page = PageLoader.Download( "http://news.163.com/", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)", "" );
            Assert.IsNotEmpty( page );
            Assert.Greater( page.IndexOf( "网易新闻" ), 0 );

        }

        [Test]
        public void testSpider() {

            StringBuilder log = new StringBuilder();

            SpiderTemplate s = getTemplate( "http://news.163.com", "<div class=\"content\" style=\"zoom:1;\">", "<h2>图片新闻</h2>" );
            List<DetailLink> list = SpiderTool.GetDataList( s, log );
            Assert.Greater( list.Count, 1 );

            s = getTemplate( "http://women.sohu.com/love-story/", "<div class=\"f14list\">", "<div class=\"pages\">" );
            list = SpiderTool.GetDataList( s, log );
            Assert.Greater( list.Count, 1 );

        }

        private static SpiderTemplate getTemplate( String listUrl, String beginCode, String endCode ) {

            SpiderTemplate s = new SpiderTemplate();
            s.ListUrl = listUrl;
            s.ListBodyPattern = beginCode + ".+?" + endCode;
            s.ListPattern = SpiderConfig.ListLinkPattern;

            return s;

        }


    }

}
