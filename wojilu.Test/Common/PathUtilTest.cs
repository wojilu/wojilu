using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

using NUnit.Framework;
using wojilu;
using wojilu.Web.Mvc;
using System.Web;

namespace wojilu.Test.Common {

    [TestFixture]
    public class PathUtilTest {





        public void Isabs() {

            //Assert.IsTrue( VirtualPathUtility.IsAbsolute( "/abcd/ef" ) );
            //Assert.IsFalse( VirtualPathUtility.IsAbsolute( "abcd/ef" ) );
            //Assert.IsFalse( VirtualPathUtility.IsAbsolute( "\abcd\\ef" ) );
            //Assert.IsTrue( VirtualPathUtility.IsAbsolute( "c:\\abcd\\ifdag" ) );
            //Assert.IsTrue( VirtualPathUtility.IsAbsolute( "C:\\Program Files" ) );

            //Assert.IsTrue( new Uri( "/abcd/ef" ).IsAbsoluteUri );
            //Assert.IsFalse( Uri.IsAbsolute( "abcd/ef" ) );
            //Assert.IsFalse( Uri.IsAbsolute( "\abcd\\ef" ) );
            //Assert.IsTrue( Uri.IsAbsolute( "c:\\abcd\\ifdag" ) );
            //Assert.IsTrue( Uri.IsAbsolute( "C:\\Program Files" ) );

        }

        private static bool hasUrlExt( String str ) {
            return PathHelper.UrlHasExt( str );
        }

        [Test]
        public void testHasExt() {
            // 是否包含后缀名

            Assert.IsTrue( hasUrlExt( "ab.htm" ) );
            Assert.IsTrue( hasUrlExt( "xyzz/ab.htm" ) );
            Assert.IsTrue( hasUrlExt( "ab/daffd/33414.htm" ) );

            Assert.IsFalse( hasUrlExt( "ab" ) );
            Assert.IsFalse( hasUrlExt( " " ) );
            Assert.IsFalse( hasUrlExt( "/my/xyz" ) );
            Assert.IsFalse( hasUrlExt( "my/xyz" ) );
            Assert.IsFalse( hasUrlExt( "my/xyz/dfae3" ) );

        }


        [Test]
        public void testIsFullUrl() {

            Assert.IsFalse( PathHelper.IsFullUrl( "" ) );
            Assert.IsFalse( PathHelper.IsFullUrl( "  " ) );

            Assert.IsTrue( PathHelper.IsFullUrl( "http://abc.com" ) );
            Assert.IsTrue( PathHelper.IsFullUrl( "http://www.abc.com" ) );
            Assert.IsTrue( PathHelper.IsFullUrl( "www.abc.com" ) );
            Assert.IsTrue( PathHelper.IsFullUrl( "abc.com" ) );

            Assert.IsTrue( PathHelper.IsFullUrl( "  http://abc.com" ) );
            Assert.IsTrue( PathHelper.IsFullUrl( "  http://www.abc.com" ) );
            Assert.IsTrue( PathHelper.IsFullUrl( "  www.abc.com" ) );
            Assert.IsTrue( PathHelper.IsFullUrl( "  abc.com" ) );

            //Assert.IsFalse( PathUtil.IsFullUrl( "......." ) );
            //Assert.IsFalse( PathUtil.IsFullUrl( "..../...." ) );


            Assert.IsFalse( PathHelper.IsFullUrl( "abc" ) );
            Assert.IsFalse( PathHelper.IsFullUrl( "abc/abb" ) );
            Assert.IsFalse( PathHelper.IsFullUrl( "/xyz/abb" ) );

            Assert.IsFalse( PathHelper.IsFullUrl( "  abc" ) );
            Assert.IsFalse( PathHelper.IsFullUrl( "  abc/abb" ) );
            Assert.IsFalse( PathHelper.IsFullUrl( "  /xyz/abb" ) );

            //Match match = Regex.Match( "/xyz/abb.aspx", "^\\s*(http://)?[a-zA-z0-9_-\\.]+\\.[a-zA-z0-9]$", RegexOptions.IgnoreCase );
            //if (match.Success) {
            //    Console.WriteLine( match.Value );
            //}


            Assert.IsFalse( PathHelper.IsFullUrl( "/xyz/abb.aspx" ) );

            Assert.IsFalse( PathHelper.IsFullUrl( "abcd.html" ) );
            Assert.IsFalse( PathHelper.IsFullUrl( "xyz.abcd.html" ) );
            Assert.IsFalse( PathHelper.IsFullUrl( "xyz.abcd.3443143.aspx" ) );


        }

        [Test]
        public void testTrimUrlExt() {

            string url = "http://www.abc.com/forum/blog32/8988.aspx";
            Assert.AreEqual( "http://www.abc.com/forum/blog32/8988", PathHelper.TrimUrlExt( url ) );

            url = "/forum/blog32/8988.aspx";
            Assert.AreEqual( "/forum/blog32/8988", PathHelper.TrimUrlExt( url ) );

            url = "/forum/blog32/8988.html";
            Assert.AreEqual( "/forum/blog32/8988", PathHelper.TrimUrlExt( url ) );

            url = "/forum/blog32/8988";
            Assert.AreEqual( "/forum/blog32/8988", PathHelper.TrimUrlExt( url ) );

            url = "8988.aspx";
            Assert.AreEqual( "8988", PathHelper.TrimUrlExt( url ) );

            url = "8988";
            Assert.AreEqual( "8988", PathHelper.TrimUrlExt( url ) );

            url = "http://www.abc.com/forum/blog32/8988";
            Assert.AreEqual( "http://www.abc.com/forum/blog32/8988", PathHelper.TrimUrlExt( url ) );

        }

        [Test]
        public void testMvcLayoutPath() {

            string rootPath = "wojilu.Web.Controller";

            string pathFull1 = "wojilu.Web.Controller.Blog.Admin.MySet";
            IList path1 = getPathList( rootPath, pathFull1 );
            Assert.AreEqual( 4, path1.Count );
            Assert.AreEqual( "Blog/Admin/MySet", path1[0] );
            Assert.AreEqual( "Blog/Admin", path1[1] );
            Assert.AreEqual( "Blog", path1[2] );
            Assert.AreEqual( "", path1[3] );

            string pathFull2 = "wojilu.Web.Controller.Blog.Admin";
            IList path2 = getPathList( rootPath, pathFull2 );
            Assert.AreEqual( 3, path2.Count );
            Assert.AreEqual( "Blog/Admin", path2[0] );
            Assert.AreEqual( "Blog", path2[1] );
            Assert.AreEqual( "", path2[2] );


            string pathFull3 = "wojilu.Web.Controller.Blog";
            IList path3 = getPathList( rootPath, pathFull3 );
            Assert.AreEqual( 2, path3.Count );
            Assert.AreEqual( "Blog", path3[0] );
            Assert.AreEqual( "", path3[1] );


            string pathFull4 = "wojilu.Web.Controller";
            IList path4 = getPathList( rootPath, pathFull4 );
            Assert.AreEqual( 1, path4.Count );
            Assert.AreEqual( "", path4[0] );

        }

        private IList getPathList( string rootPath, string pathFull ) {
            return PathHelper.GetPathList( rootPath, pathFull );
        }

        //[Test]
        //public void testAppDataController() {

        //    string c1 = getAppDataController( new wojilu.Apps.BlogApp.Domain.BlogPost() );
        //    Assert.AreEqual( c1, "BlogApp/PostController" );


        //}




    }

}
