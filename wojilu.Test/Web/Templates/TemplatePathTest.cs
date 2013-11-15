using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Web;

namespace wojilu.Test.Web.Templates {

    [TestFixture]
    public class TemplatePathTest {











        private static readonly ILog logger = LogManager.GetLogger( typeof( TemplatePathTest ) );


        // 如要扩展路径，请打开源码 \wojilu\Web\Context\MvcContextUtils.cs 
        // 修改其中的方法 setGlobalVariable(ITemplate tpl)

        [Test]
        public void testImgPath() {

            string html = "<div><img src=\"~img/doc.gif\" /></div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            string result = tpl.ToString();
            Console.WriteLine( result );
            //~img/
            //=>
            //#{path.img}
            Assert.AreEqual( "<div><img src=\"#{path.img}doc.gif\" /></div>", result );

        }

        [Test]
        public void testImgPathSlash() {

            string html = "<div><img src=\"~/img/doc.gif\" /></div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            string result = tpl.ToString();
            Console.WriteLine( result );
            //~/img/
            //=>
            //#{patha.img}
            Assert.AreEqual( "<div><img src=\"#{patha.img}doc.gif\" /></div>", result );

        }

        [Test]
        public void testJsPath() {

            string html = "<div><img src=\"~js/doc.gif\" /></div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            string result = tpl.ToString();
            Console.WriteLine( result );
            //~js/
            //=>
            //#{path.js}
            Assert.AreEqual( "<div><img src=\"#{path.js}doc.gif\" /></div>", result );
        }

        [Test]
        public void testJsPathSlash() {

            string html = "<div><img src=\"~/js/doc.gif\" /></div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            string result = tpl.ToString();
            Console.WriteLine( result );
            //~/js/
            //=>
            //#{patha.js}
            Assert.AreEqual( "<div><img src=\"#{patha.js}doc.gif\" /></div>", result );
        }


        [Test]
        public void testCssPath() {

            string html = "<div><img src=\"~css/doc.gif\" /></div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            string result = tpl.ToString();
            Console.WriteLine( result );
            //~css/
            //=>
            //#{path.css}
            Assert.AreEqual( "<div><img src=\"#{path.css}doc.gif\" /></div>", result );
        }

        [Test]
        public void testCssPathSlash() {

            string html = "<div><img src=\"~/css/doc.gif\" /></div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            string result = tpl.ToString();
            Console.WriteLine( result );
            //~/css/
            //=>
            //#{patha.css}
            Assert.AreEqual( "<div><img src=\"#{patha.css}doc.gif\" /></div>", result );
        }

        [Test]
        public void testStaticPath() {

            string html = "<div><img src=\"~static/doc.gif\" /></div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            string result = tpl.ToString();
            Console.WriteLine( result );
            //~static/
            //=>
            //#{path.static}
            Assert.AreEqual( "<div><img src=\"#{path.static}doc.gif\" /></div>", result );
        }

        [Test]
        public void testStaticPathSlash() {

            string html = "<div><img src=\"~/static/doc.gif\" /></div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            string result = tpl.ToString();
            Console.WriteLine( result );
            //~/static/
            //=>
            //#{patha.static}
            Assert.AreEqual( "<div><img src=\"#{patha.static}doc.gif\" /></div>", result );
        }


        [Test]
        public void testSkinPath() {

            string html = "<div><img src=\"~skin/doc.gif\" /></div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            string result = tpl.ToString();
            Console.WriteLine( result );
            //~skin/
            //=>
            //#{path.skin}
            Assert.AreEqual( "<div><img src=\"#{path.skin}doc.gif\" /></div>", result );
        }

        [Test]
        public void testSkinPathSlash() {

            string html = "<div><img src=\"~/skin/doc.gif\" /></div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            string result = tpl.ToString();
            Console.WriteLine( result );
            //~/skin/
            //=>
            //#{patha.skin}
            Assert.AreEqual( "<div><img src=\"#{patha.skin}doc.gif\" /></div>", result );
        }


        // 如要扩展路径，请打开源码 \wojilu\Web\Context\MvcContextUtils.cs 
        // 修改其中的方法 setGlobalVariable(ITemplate tpl)
        [Test]
        public void testOtherPath() {

            string html = "<div><img src=\"~xpath/doc.gif\" /></div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            string result = tpl.ToString();
            Console.WriteLine( result );
            //~xpath/
            //=>
            //#{path.xpath}
            Assert.AreEqual( "<div><img src=\"#{path.xpath}doc.gif\" /></div>", result );
        }


        [Test]
        public void testErrorPath() {

            // 只支持26个英文字母(包括大写)

            // 不支持数字
            string html = "<div><img src=\"~xpath1/doc.gif\" /></div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            string result = tpl.ToString();
            Console.WriteLine( result );

            Assert.AreEqual( html, result );

            // 不支持特殊符号，比如<
            html = "<div>~</div>";
            tpl = new Template().InitContent( html );
            result = tpl.ToString();
            Console.WriteLine( result );
            Assert.AreEqual( html, result );
        }



        [Test]
        public void testRoot() {

            //~/
            //=>
            //#{path}
            //=>
            //sys.Path.Root

            // 根目录
            string html = "<div><img src=\"~/doc.gif\" /></div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            string result = tpl.ToString();
            Console.WriteLine( result );

            Assert.AreEqual( "<div><img src=\"#{path}doc.gif\" /></div>", result );

            // 纯粹的根目录
            html = "<div><a href=\"~/\">site</a></div>";
            tpl = new Template().InitContent( html );
            result = tpl.ToString();
            Console.WriteLine( result );
            Assert.AreEqual( "<div><a href=\"#{path}\">site</a></div>", result );
        }


    }
}
