using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using wojilu.Web.Mvc;
using wojilu.Common.Tags;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using wojilu.ORM;
using wojilu.Web.Utils;

namespace wojilu.Test.Common {

    [TestFixture]
    public class strUtilTest {







        static int i = 0;
        private static void ploop() {
            i++;
            Console.WriteLine( "i=" + i );
            if (i < 3) {
                ploop();
            }
        }

        private static void beginLoop() {
            ploop();
            Console.WriteLine( "--------------out-------------" );
        }

        private String getHostNoSubdomain( String Host ) {
            int firstDotIndex = Host.IndexOf( '.' );
            return Host.Substring( firstDotIndex + 1, Host.Length - firstDotIndex - 1 );
        }


        [Test]
        public void testSubdomain() {

            Assert.AreEqual( "abc.com", getHostNoSubdomain( "www.abc.com" ) );
            Assert.AreEqual( "abc.com.cn", getHostNoSubdomain( "www.abc.com.cn" ) );

        }


        [Test]
        public void testSplitStr() {

            String[] arr = strUtil.Split( "select top 10 * from posts order by id desc", " top " );
            Assert.AreEqual( 2, arr.Length );
            Assert.AreEqual( "select", arr[0] );
            Assert.AreEqual( "10 * from posts order by id desc", arr[1] );

            arr = strUtil.Split( "这是abc地球上abc的一段abc历史abc", "abc" );
            Assert.AreEqual( 5, arr.Length );
            Assert.AreEqual( "这是", arr[0] );
            Assert.AreEqual( "地球上", arr[1] );
            Assert.AreEqual( "的一段", arr[2] );
            Assert.AreEqual( "历史", arr[3] );
            Assert.AreEqual( "", arr[4] );


            List<String> list = strUtil.SplitByNum( "abcdefghijklmn", 6 );
            Assert.AreEqual( 3, list.Count );
            Assert.AreEqual( "abcdef", list[0] );
            Assert.AreEqual( "ghijkl", list[1] );
            Assert.AreEqual( "mn", list[2] );

            list = strUtil.SplitByNum( "abcd", 0 );
            Assert.AreEqual( 1, list.Count );
            Assert.AreEqual( "abcd", list[0] );

            list = strUtil.SplitByNum( "", 140 );
            Assert.AreEqual( 1, list.Count );
            Assert.AreEqual( "", list[0] );

            list = strUtil.SplitByNum( " ", 140 );
            Assert.AreEqual( 1, list.Count );
            Assert.AreEqual( " ", list[0] );

            list = strUtil.SplitByNum( null, 140 );
            Assert.AreEqual( 0, list.Count );



            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 3000; i++) {
                sb.Append( i );
            }

            outputStr( sb.ToString(), 140 );
            outputStr( "", 140 );
            outputStr( " ", 140 );
            outputStr( "abc", 140 );
            outputStr( null, 140 );




        }

        private static void outputStr( String sb, int clength ) {


            List<String> list = strUtil.SplitByNum( sb, clength );
            foreach (String str in list) {
                Console.WriteLine( str );
            }

            if (sb == null) return;

            if (sb.Length < 100) {
                Console.WriteLine( "--------[" + sb + "]----------" );
            }
            else {
                Console.WriteLine( "============" );
            }
        }


        [Test]
        public void testHtml22() {

            String str = HttpUtility.HtmlEncode( "这是双\\引号的\\内容" );
            Console.WriteLine( str );

        }

        public void testTagEqual() {

            Assert.IsTrue( TagService.tagEqual( "", "" ) );
            Assert.IsTrue( TagService.tagEqual( null, "" ) );
            Assert.IsTrue( TagService.tagEqual( null, null ) );

            Assert.IsFalse( TagService.tagEqual( "abc", "" ) );
            Assert.IsTrue( TagService.tagEqual( "abc,中国", "abc,中国" ) );
            Assert.IsTrue( TagService.tagEqual( "abc,中国", "中国，abc" ) );
            Assert.IsTrue( TagService.tagEqual( "abc,中国", "  abc , 中国 " ) );
            Assert.IsFalse( TagService.tagEqual( "abc,中国", "这是什么" ) );

        }

        [Test]
        public void testEncoding() {

            Encoding e = Encoding.GetEncoding( "UTF-8" );
            Console.WriteLine( e );
            Console.WriteLine( Encoding.GetEncoding( "gb2312" ) );
            Console.WriteLine( Encoding.GetEncoding( "utf-8" ) );
            Console.WriteLine( Encoding.GetEncoding( "Unicode" ) );
            Console.WriteLine( Encoding.GetEncoding( "gbk" ) );
            Console.WriteLine( Encoding.GetEncoding( "Big5" ) );
            Console.WriteLine( Encoding.GetEncoding( "GBK" ) );

        }

        public void testPagedLink() {

            String page = @"		<table align=""center""> 
		<tr> 
		<td> 
		 <span class=""s1 s3"">上一页</span> 
		 <a href=""http://tech.163.com/10/0929/15/6HOS2LHK00093879.html"" target=""_self"" class=""s2"">1</a> 
		 <a href=""http://tech.163.com/10/0929/15/6HOS2LHK00093879_2.html"" target=""_self"">2</a> 
		 <a href=""http://tech.163.com/10/0929/15/6HOS2LHK00093879_3.html"" target=""_self"">3</a> 
		 <a href=""http://tech.163.com/10/0929/15/6HOS2LHK00093879_4.html"" target=""_self"">4</a> 
		 <a href=""http://tech.163.com/10/0929/15/6HOS2LHK00093879_5.html"" target=""_self"">5</a> 
		 <a href=""http://tech.163.com/10/0929/15/6HOS2LHK00093879_2.html"" target=""_self"" class=""s1"">下一页</a> 
		<div class=""clear""></div> 
		</td> 
		</tr> 
		</table> ";

            String detailUrl = "http://tech.163.com/10/0929/15/6HOS2LHK00093879.html";

            List<String> list = getPagedUrl( page, detailUrl );
            Assert.AreEqual( 5, list.Count );

            foreach (String url in list) {
                Console.WriteLine( url );
            }

        }

        private static List<String> getPagedUrl( String page, String url ) {

            String urlWithouExt = getUrlWithouExt( url );

            List<String> list = new List<string>();

            MatchCollection matchs = Regex.Matches( page, "<a href=\"(" + urlWithouExt + "[^\"]*?)\".+?\">", RegexOptions.Singleline );
            foreach (Match m in matchs) {
                if (list.Contains( m.Groups[1].Value )) continue;
                list.Add( m.Groups[1].Value );
            }

            return list;
        }

        private static String getUrlWithouExt( String url ) {
            int lastDot = url.LastIndexOf( '.' );
            int lastSlash = url.LastIndexOf( '/' );
            if (lastDot > lastSlash)
                return url.Substring( 0, lastDot );
            else
                return url;
        }



        [Test]
        public void testColor() {

            Assert.IsTrue( strUtil.IsColorValue( "#8c8ca3" ) );
            Assert.IsTrue( strUtil.IsColorValue( "#fff" ) );
            Assert.IsTrue( strUtil.IsColorValue( "#222" ) );
            Assert.IsTrue( strUtil.IsColorValue( "1c1c30" ) );
            Assert.IsTrue( strUtil.IsColorValue( "333" ) );
            Assert.IsTrue( strUtil.IsColorValue( "af6" ) );


            Assert.IsFalse( strUtil.IsColorValue( "#" ) );
            Assert.IsFalse( strUtil.IsColorValue( "" ) );
            Assert.IsFalse( strUtil.IsColorValue( null ) );
            Assert.IsFalse( strUtil.IsColorValue( "   " ) );
            Assert.IsFalse( strUtil.IsColorValue( "#22" ) );
            Assert.IsFalse( strUtil.IsColorValue( "#$33" ) );
            Assert.IsFalse( strUtil.IsColorValue( "#8c8ca33" ) );

            Assert.IsFalse( strUtil.IsColorValue( "#8c_a33" ) );
            Assert.IsFalse( strUtil.IsColorValue( "_333" ) );
            Assert.IsFalse( strUtil.IsColorValue( "1c.c30" ) );
        }

        [Test]
        public void testUrlValid() {
            Assert.IsTrue( strUtil.IsUrlItem( "abc" ) );
            Assert.IsTrue( strUtil.IsUrlItem( "abc123" ) );
            Assert.IsTrue( strUtil.IsUrlItem( "abc_123" ) );
            Assert.IsTrue( strUtil.IsUrlItem( "abc123_" ) );


            Assert.IsFalse( strUtil.IsUrlItem( null ) );
            Assert.IsFalse( strUtil.IsUrlItem( "" ) );
            Assert.IsFalse( strUtil.IsUrlItem( "  " ) );
            Assert.IsFalse( strUtil.IsUrlItem( "_abc" ) );
            Assert.IsFalse( strUtil.IsUrlItem( "武功" ) );
            Assert.IsFalse( strUtil.IsUrlItem( "abc-123" ) );
        }


        [Test]
        public void testHtml() {

            String str = HttpUtility.HtmlEncode( "这是双\"引号的\"内容" );
            Console.WriteLine( str );

            String expected = "这是双&quot;引号的&quot;内容";
            Assert.AreEqual( expected, str );
        }


        [Test]
        public void testGetScripts() {
            //string html = file.Read( "zScript2.html" );
            //String result = strUtil.ResetScript( html );
            //Console.WriteLine( result );
        }

        [Test]
        public void testTrimHtml() {

            string str = "&nbsp;";
            str = strUtil.TrimHtml( str );
            Assert.AreEqual( "", str );

            str = "&nbsp;&nbsp;";
            str = strUtil.TrimHtml( str );
            Assert.AreEqual( "", str );

            str = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            str = strUtil.TrimHtml( str );
            Assert.AreEqual( "", str );

            str = null;
            str = strUtil.TrimHtml( str );
            Assert.AreEqual( null, str );

            str = "   ";
            str = strUtil.TrimHtml( str );
            Assert.AreEqual( "", str );


            str = "&nbsp;&nbsp;&nbsp;abc&nbsp;&nbsp;&nbsp;&nbsp;";
            str = strUtil.TrimHtml( str );
            Assert.AreEqual( "abc", str );

            str = "<P>&nbsp;</P>";
            str = strUtil.TrimHtml( str );
            Assert.AreEqual( "", str );

            str = "  <P>      </P>  ";
            str = strUtil.TrimHtml( str );
            Assert.AreEqual( "", str );

            str = " &nbsp; <P>  &nbsp;    </P> &nbsp; ";
            str = strUtil.TrimHtml( str );
            Assert.AreEqual( "", str );


            str = "<br/>";
            str = strUtil.TrimHtml( str );
            Assert.AreEqual( "", str );

            str = "<img src=\"abc.jpg\" />";
            str = strUtil.TrimHtml( str );
            Assert.AreEqual( "<img src=\"abc.jpg\" />", str );

            str = "&nbsp;&nbsp;&nbsp;abc&nbsp;<img src=\"abc.jpg\" />&nbsp;&nbsp;";
            str = strUtil.TrimHtml( str );
            Assert.AreEqual( "abc&nbsp;<img src=\"abc.jpg\" />", str );

            String flash = "<object data=\"http://player.youku.com/player.php/sid/XMTg0MTI0NjU2/v.swf\" type=\"application/x-shockwave-flash\" width=\"300\" height=\"255\"><param name=\"movie\" value =\"http://player.youku.com/player.php/sid/XMTg0MTI0NjU2/v.swf\" /></object>";
            str = strUtil.TrimHtml( flash );
            Assert.AreEqual( flash, str );

            String flash2 = WebHelper.GetFlash( "www.abc.com/x.swf", 480, 360 );
            str = strUtil.TrimHtml( flash2 );
            Assert.AreEqual( flash2, str );


        }

        private static int getPage( string url ) {
            return PageHelper.GetPageNoByUrl( url );
        }

        [Test]
        public void testGetPage() {

            Assert.AreEqual( 1, getPage( null ) );
            Assert.AreEqual( 1, getPage( "" ) );
            Assert.AreEqual( 1, getPage( "   " ) );

            Assert.AreEqual( 1, getPage( "/Admin/Apps/Photo/Main/Index.aspx" ) );
            Assert.AreEqual( 1, getPage( "/Admin/Apps/Photo/Main/Index" ) );

            Assert.AreEqual( 1, getPage( "Index.aspx" ) );
            Assert.AreEqual( 1, getPage( "Index" ) );

            Assert.AreEqual( 1, getPage( "/Admin/Apps/Photo/Main/Index2.aspx" ) );
            Assert.AreEqual( 1, getPage( "/Admin/Apps/Photo/Main/Index2" ) );

            Assert.AreEqual( 2, getPage( "/Admin/Apps/Photo/Main/Index/p2.aspx" ) );
            Assert.AreEqual( 2, getPage( "/Admin/Apps/Photo/Main/Index/p2" ) );

            Assert.AreEqual( 2, getPage( "Index/p2.aspx" ) );
            Assert.AreEqual( 2, getPage( "Index/p2" ) );

            Assert.AreEqual( 2, getPage( "/Admin/Apps/Photo/Main/Index3/p2.aspx" ) );
            Assert.AreEqual( 2, getPage( "/Admin/Apps/Photo/Main/Index3/p2" ) );

            Assert.AreEqual( 59377329, getPage( "/Admin/Apps/Photo/Main/Index/p59377329.aspx" ) );
            Assert.AreEqual( 59377329, getPage( "/Admin/Apps/Photo/Main/Index/p59377329" ) );


        }

        public static string AppendPage( string srcUrl, int pageNumber ) {
            return PageHelper.AppendNo( srcUrl, pageNumber );
        }

        [Test]
        public void testAppendPage() {

            string url = "/Admin/Apps/Photo/Main/Index.aspx";
            Assert.AreEqual( "/Admin/Apps/Photo/Main/Index/p2.aspx", AppendPage( url, 2 ) );
            Assert.AreEqual( "/Admin/Apps/Photo/Main/Index.aspx", AppendPage( url, 1 ) );

            url = "/Admin/Apps/Photo/Main/Index/p3.aspx";
            Assert.AreEqual( "/Admin/Apps/Photo/Main/Index/p2.aspx", AppendPage( url, 2 ) );
            Assert.AreEqual( "/Admin/Apps/Photo/Main/Index.aspx", AppendPage( url, 1 ) );


            url = "/Admin/Apps/Photo/Main/Index.aspx?name=lisi";
            Assert.AreEqual( "/Admin/Apps/Photo/Main/Index/p2.aspx?name=lisi", AppendPage( url, 2 ) );

            url = "/Admin/Apps/Photo/Main/Index";
            Assert.AreEqual( "/Admin/Apps/Photo/Main/Index/p2", AppendPage( url, 2 ) );

            url = "/Admin/Apps/Photo/Main/Index/p3";
            Assert.AreEqual( "/Admin/Apps/Photo/Main/Index/p2", AppendPage( url, 2 ) );


            url = "/Admin/Apps/Photo/Main/Index?name=lisi";
            Assert.AreEqual( "/Admin/Apps/Photo/Main/Index/p2?name=lisi", AppendPage( url, 2 ) );

            url = "/Admin/Apps/Photo/Main/Index?name=lisi&gender=2";
            Assert.AreEqual( "/Admin/Apps/Photo/Main/Index/p2?name=lisi&gender=2", AppendPage( url, 2 ) );

            url = "/Admin/Apps/Photo.List/Main/Index";
            Assert.AreEqual( "/Admin/Apps/Photo.List/Main/Index/p2", AppendPage( url, 2 ) );

            url = "user.aspx";
            Assert.AreEqual( "user/p2.aspx", AppendPage( url, 2 ) );

            url = "user";
            Assert.AreEqual( "user/p2", AppendPage( url, 2 ) );

            url = "blog.aspx?categoryId=3";
            Assert.AreEqual( "blog/p2.aspx?categoryId=3", AppendPage( url, 2 ) );

            url = "blog?categoryId=3";
            Assert.AreEqual( "blog/p2?categoryId=3", AppendPage( url, 2 ) );


        }

        [Test]
        public void testAppendHtmlPage() {

            String url = null;
            Assert.AreEqual( null, PageHelper.AppendHtmlNo( url, 2 ) );

            url = "";
            Assert.AreEqual( "", PageHelper.AppendHtmlNo( url, 2 ) );

            url = "/html/2010/11/22/195.html";
            Assert.AreEqual( "/html/2010/11/22/195_2.html", PageHelper.AppendHtmlNo( url, 2 ) );

            url = "/html/2010/11/22/195.html";
            Assert.AreEqual( "/html/2010/11/22/195.html", PageHelper.AppendHtmlNo( url, 1 ) );

            url = "/html/2010/11/22/195.html";
            Assert.AreEqual( "/html/2010/11/22/195_4383843.html", PageHelper.AppendHtmlNo( url, 4383843 ) );

            url = "/html/2010/11/22/195.html";
            Assert.AreEqual( "/html/2010/11/22/195.html", PageHelper.AppendHtmlNo( url, 0 ) );

            url = "/html/2010/11/22/195.html";
            Assert.AreEqual( "/html/2010/11/22/195.html", PageHelper.AppendHtmlNo( url, -2334 ) );

        }

        [Test]
        public void testIsNullOrEmpty() {
            Assert.IsTrue( strUtil.IsNullOrEmpty( "" ) );
            Assert.IsTrue( strUtil.IsNullOrEmpty( " " ) );
            Assert.IsTrue( strUtil.IsNullOrEmpty( null ) );
            Assert.IsFalse( strUtil.IsNullOrEmpty( "abc" ) );
            Assert.IsTrue( strUtil.IsNullOrEmpty( "\r" ) );

            Assert.IsFalse( string.IsNullOrEmpty( "  " ) );
        }

        [Test]
        public void testHasText() {
            Assert.IsFalse( strUtil.HasText( "" ) );
            Assert.IsFalse( strUtil.HasText( " " ) );
            Assert.IsFalse( strUtil.HasText( null ) );
            Assert.IsTrue( strUtil.HasText( "abc" ) );
        }

        [Test]
        public void testCutStringStringInt() {
            Assert.AreEqual( strUtil.CutString( "abcd1234", 5 ), "abcd1..." );
            Assert.AreEqual( strUtil.CutString( "", 5 ), "" );
            Assert.AreEqual( strUtil.CutString( null, 5 ), null );
            Assert.AreEqual( strUtil.CutString( "abcd1234", 8 ), "abcd1234" );
            Assert.AreEqual( strUtil.CutString( "abcd1234", 28 ), "abcd1234" );
        }

        [Test]
        public void testSubString() {
            Assert.AreEqual( strUtil.SubString( "abcd1234", 5 ), "abcd1" );
            Assert.AreEqual( strUtil.SubString( "", 5 ), "" );
            Assert.AreEqual( strUtil.SubString( null, 5 ), null );
            Assert.AreEqual( strUtil.SubString( "abcd1234", 8 ), "abcd1234" );
            Assert.AreEqual( strUtil.SubString( "abcd1234", 28 ), "abcd1234" );
        }

        [Test]
        public void testConverToNotNull() {
            Assert.AreEqual( strUtil.ConverToNotNull( "abcd1234" ), "abcd1234" );
            Assert.AreEqual( strUtil.ConverToNotNull( null ), "" );
        }

        [Test]
        public void testAppend() {
            Assert.AreEqual( strUtil.Append( "abcd1234", "xyz" ), "abcd1234xyz" );
            Assert.AreEqual( strUtil.Append( "abcd1234", "234" ), "abcd1234" );
            Assert.AreEqual( strUtil.Append( "abcd1234", null ), "abcd1234" );
            Assert.AreEqual( strUtil.Append( "abcd1234", "" ), "abcd1234" );
            Assert.AreEqual( strUtil.Append( "abcd1234", "   " ), "abcd1234" );
        }

        [Test]
        public void testJoin() {
            Assert.AreEqual( strUtil.Join( "abcd1234", "xyz" ), "abcd1234/xyz" );
            Assert.AreEqual( strUtil.Join( "abcd1234/", "/xyz" ), "abcd1234/xyz" );
            Assert.AreEqual( strUtil.Join( "abcd1234/", "xyz" ), "abcd1234/xyz" );
            Assert.AreEqual( strUtil.Join( "abcd1234", "/xyz" ), "abcd1234/xyz" );
            Assert.AreEqual( strUtil.Join( "abcd1234", null ), "abcd1234/" );
            Assert.AreEqual( strUtil.Join( "abcd1234", "" ), "abcd1234/" );
            Assert.AreEqual( strUtil.Join( "abcd1234", "   " ), "abcd1234/" );
        }

        [Test]
        public void testTrimEnd() {
            Assert.AreEqual( strUtil.TrimEnd( "frienedend", "end" ), "friened" );
            Assert.AreEqual( strUtil.TrimEnd( "abcd123334", "34" ), "abcd1233" );
            Assert.AreEqual( strUtil.TrimEnd( "abcd123334", null ), "abcd123334" );
            Assert.AreEqual( strUtil.TrimEnd( "abcd123334", "" ), "abcd123334" );
            Assert.AreEqual( strUtil.TrimEnd( "abcd123334", "    " ), "abcd123334" );
            Assert.AreEqual( strUtil.TrimEnd( "abcd123334", "abcc" ), "abcd123334" );
        }

        [Test]
        public void testTrimStart() {
            Assert.AreEqual( strUtil.TrimStart( "abaabcd123334", "aba" ), "abcd123334" );
            Assert.AreEqual( strUtil.TrimStart( "abaabcd123334", null ), "abaabcd123334" );
            Assert.AreEqual( strUtil.TrimStart( "abaabcd123334", "" ), "abaabcd123334" );
            Assert.AreEqual( strUtil.TrimStart( "abaabcd123334", "  " ), "abaabcd123334" );
            Assert.AreEqual( strUtil.TrimStart( "abaabcd123334", "abcd" ),
                    "abaabcd123334" );
        }

        [Test]
        public void testSqlClean() {
            Assert.AreEqual( strUtil.SqlClean( "abbc' d4", 20 ), "abbc'' d4" );
            Assert.AreEqual( strUtil.SqlClean( "abbcd4", 20 ), "abbcd4" );
        }

        [Test]
        public void testCamelCase() {
            Assert.AreEqual( strUtil.GetCamelCase( "" ), "" );
            Assert.AreEqual( strUtil.GetCamelCase( " " ), " " );
            Assert.AreEqual( strUtil.GetCamelCase( "MyNameIs" ), "myNameIs" );
            Assert.AreEqual( strUtil.GetCamelCase( "myNameIs" ), "myNameIs" );
            Assert.AreEqual( strUtil.GetCamelCase( "name" ), "name" );
            Assert.AreEqual( strUtil.GetCamelCase( "Name" ), "name" );
            Assert.AreEqual( strUtil.GetCamelCase( "12abc" ), "12abc" );
        }

        [Test]
        public void testGetType() {

            // System.Collections.Generic.List`1[System.String]
            Type t = typeof( List<String> );
            Console.WriteLine( t.ToString() );

            String tFullName = strUtil.GetTypeFullName( t );
            String tname = strUtil.GetTypeName( t );
            Console.WriteLine( tFullName );
            Console.WriteLine( tname );

            Assert.AreEqual( "System.Collections.Generic.List", tFullName );
            Assert.AreEqual( "List", tname );

            // 获取内部元素信息
            Type[] arrArgs = t.GetGenericArguments();
            Assert.AreEqual( 1, arrArgs.Length );
            Assert.AreEqual( "System.String", arrArgs[0].ToString() );

            String glist = strUtil.GetGenericTypeWithArgs( t );
            Assert.AreEqual( "System.Collections.Generic.List<System.String>", glist );


            //---------------------------------------------------

            Type tclass = typeof( List<strUtil> );
            Console.WriteLine( tclass.ToString() );

            String tclassFullName = strUtil.GetTypeFullName( tclass );
            String tclassname = strUtil.GetTypeName( tclass );

            Console.WriteLine( tclassFullName );
            Console.WriteLine( tclassname );

            Assert.AreEqual( "System.Collections.Generic.List", tclassFullName );
            Assert.AreEqual( "List", tclassname );

            String gclslist = strUtil.GetGenericTypeWithArgs( tclass );
            Assert.AreEqual( "System.Collections.Generic.List<wojilu.strUtil>", gclslist );


            //---------------------------------------------------
            Dictionary<int, string> dic = new Dictionary<int, string>();

            Type tmap = typeof( Dictionary<int, string> );
            Console.WriteLine( tmap.ToString() );
            String tmapFullName = strUtil.GetTypeFullName( tmap );
            String tmapname = strUtil.GetTypeName( tmap );

            Console.WriteLine( tmapFullName );
            Console.WriteLine( tmapname );

            Assert.AreEqual( "System.Collections.Generic.Dictionary", tmapFullName );
            Assert.AreEqual( "Dictionary", tmapname );


            // 获取内部元素信息
            Type[] arrMapArgs = tmap.GetGenericArguments();
            Assert.AreEqual( 2, arrMapArgs.Length );
            Assert.AreEqual( "System.Int32", arrMapArgs[0].ToString() );
            Assert.AreEqual( "System.String", arrMapArgs[1].ToString() );

            String gmapList = strUtil.GetGenericTypeWithArgs( tmap );
            Assert.AreEqual( "System.Collections.Generic.Dictionary<System.Int32,System.String>", gmapList );


        }


        [Test]
        public void testCount() {

            int count = strUtil.CountString( "dfakf33dlkafkef33eiefla33kdkd", "33" );
            Assert.AreEqual( 3, count );

            count = strUtil.CountString( "", "33" );
            Assert.AreEqual( 0, count );

            count = strUtil.CountString( null, "33" );
            Assert.AreEqual( 0, count );

            count = strUtil.CountString( "aaaaaaaaaaaaaaaaaaa<br><br><br><br><br><br><br>", "<br" );
            Assert.AreEqual( 7, count );

            count = strUtil.CountString( "aaaaaaaaaaaaaaaaaaa<br><br><br>", "<br" );
            Assert.AreEqual( 3, count );

            count = strUtil.CountString( "aaaaaaaa<br>xx<br>aaaaaaaaaaa", "<br" );
            Assert.AreEqual( 2, count );

            count = strUtil.CountString( "<br", "<br" );
            Assert.AreEqual( 1, count );

            count = strUtil.CountString( "br", "<br" );
            Assert.AreEqual( 0, count );

        }

    }
}
