using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using wojilu.Web;


namespace wojilu.Test.Web.Templates {

    [TestFixture]
    public class TemplateTest1 {










        private static readonly ILog logger = LogManager.GetLogger( typeof( TemplateTest1 ) );

        [Test]
        public void testSetVarSimple() {
            string html = "<div>#{title}</div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            tpl.Set( "title", "我是标题" );
            string result = tpl.ToString();
            Assert.AreEqual( "<div>我是标题</div>", result );
        }

        [Test]
        public void testSetVar() {
            string html = "<div>#{title}<br/>#{body}#{created}</div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            tpl.Set( "title", "我是标题" );
            tpl.Set( "body", "我是内容" );
            tpl.Set( "created", "2008-6-4" );
            string result = tpl.ToString();
            Assert.AreEqual( "<div>我是标题<br/>我是内容2008-6-4</div>", result );
        }



        [Test]
        public void testSetNoVar() {
            string html = "<div>我是标题<br/>我是内容2008-6-4</div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            tpl.Set( "title", "我是标题" );
            tpl.Set( "body", "我是内容" );
            tpl.Set( "created", "2008-6-4" );
            string result = tpl.ToString();
            Assert.AreEqual( html, result );
        }

        [Test]
        public void testNoSetVar() {
            string html = "<div>#{title}<br/>#{body}(时间：#{created})</div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            tpl.Set( "title", "我是标题" );
            tpl.Set( "body", "我是内容" );
            string result = tpl.ToString();
            Assert.AreEqual( "<div>我是标题<br/>我是内容(时间：#{created})</div>", result );
        }

        [Test]
        public void testSetVarEmpty() {
            string html = "<div>#{title}<br/>#{body}(时间：#{created})</div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            tpl.Set( "title", "我是标题" );
            tpl.Set( "body", null );
            tpl.Set( "created", string.Empty );
            string result = tpl.ToString();
            Assert.AreEqual( "<div>我是标题<br/>(时间：)</div>", result );
        }

        [Test]
        public void testSetVarRepeat() {
            string html = "<div>#{title}<br/>#{body}<br/>#{title}</div>";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            tpl.Set( "title", "我是标题" );
            tpl.Set( "body", "我是内容" );
            string result = tpl.ToString();
            Assert.AreEqual( "<div>我是标题<br/>我是内容<br/>我是标题</div>", result );
        }

        [Test]
        public void testTwo() {

            testSetVar();
            testSetVar();
            testSetVarRepeat();

        }


        [Test]
        public void testBlockExist() {

            string html = "<div>#{title}<br/>#{body}<!-- BEGIN mylist --><p>kkkk</p><!-- END mylist --></div>";

            wojilu.Web.ITemplate tpl = new Template().InitContent( html );

            Assert.IsFalse( tpl.HasBlock( "list" ) );
            Assert.IsTrue( tpl.HasBlock( "mylist" ) );

        }


        [Test]
        public void testLoop() {
            string html = "<!-- BEGIN list --><div>#{title}<br/>#{body}</div><!-- END list -->";
            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            wojilu.Web.IBlock block = tpl.GetBlock( "list" );
            for (int i = 1; i < 3; i++) {
                block.Set( "title", "我是标题" + i );
                block.Set( "body", "我是内容" + i );
                block.Next();
            }
            string result = tpl.ToString();
            Assert.AreEqual( "<div>我是标题1<br/>我是内容1</div><div>我是标题2<br/>我是内容2</div>", result );
        }

        [Test]
        public void testLoopTwo() {

            testLoop();
            testLoop();
            testLoop();

        }

        public void testBlockShow() {

            string html = "这是相关内容<!-- BEGIN list --><div>需要显示的内容</div><!-- END list -->";


            wojilu.Web.ITemplate tpl2 = new Template().InitContent( html );
            wojilu.Web.IBlock block2 = tpl2.GetBlock( "list" );
            Assert.AreEqual( "这是相关内容", tpl2.ToString() );

            wojilu.Web.ITemplate tpl3 = new Template().InitContent( html );
            wojilu.Web.IBlock block3 = tpl3.GetBlock( "list" );
            block3.Next();
            Assert.AreEqual( "这是相关内容<div>需要显示的内容</div>", tpl3.ToString() );


        }


        [Test]
        public void testLoopTwice() {

            string html = @"<!-- BEGIN category -->#{category.Name}<!-- BEGIN list --><div>#{title}<br/>#{body}</div><!-- END list --><!-- END category -->";

            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            wojilu.Web.IBlock categoryBlock = tpl.GetBlock( "category" );
            for (int k = 1; k < 3; k++) {

                categoryBlock.Set( "category.Name", "分类" + k );

                wojilu.Web.IBlock block = categoryBlock.GetBlock( "list" );
                for (int i = 1; i < 3; i++) {
                    block.Set( "title", "我是标题" + k + "_" + i );
                    block.Set( "body", "我是内容" + k + "_" + i );
                    block.Next();
                }

                categoryBlock.Next();
            }

            string result = tpl.ToString();
            string expected = @"分类1<div>我是标题1_1<br/>我是内容1_1</div><div>我是标题1_2<br/>我是内容1_2</div>分类2<div>我是标题2_1<br/>我是内容2_1</div><div>我是标题2_2<br/>我是内容2_2</div>";

            Assert.AreEqual( expected, result );
        }

        [Test]
        public void testLoop22() {
            testLoopTwice();
            testLoopTwice();
            testLoopTwice();
        }

        // 下级有两个并列的子区块
        [Test]
        public void testGetSubBlock() {

            string html = @"cc
<!-- BEGIN page -->【#{page.Name}】
    <!-- BEGIN category -->#{category.Name}<!-- END category -->
    <!-- BEGIN list --><div>#{title}<br/>#{body}</div><!-- END list -->
<!-- END page -->
aa
";

            wojilu.Web.ITemplate tpl = new Template().InitContent( html );

            wojilu.Web.IBlock pageBlock = tpl.GetBlock( "page" );

            for (int j = 1; j < 4; j++) {

                pageBlock.Set( "page.Name", "页面" + j );

                wojilu.Web.IBlock categoryBlock = pageBlock.GetBlock( "category" );
                wojilu.Web.IBlock listBlock = pageBlock.GetBlock( "list" );

                for (int k = 1; k < 3; k++) {

                    categoryBlock.Set( "category.Name", "分类" + k );
                    categoryBlock.Next();

                }

                for (int x = 1; x < 3; x++) {
                    listBlock.Set( "title", "title" + x );
                    listBlock.Set( "body", "body" + x );
                    listBlock.Next();
                }

                pageBlock.Next();

            }

            string result = tpl.ToString();
            Console.WriteLine( "" );
            Console.WriteLine( "-----------------------------------------------------------------" );
            Console.WriteLine( result );


        }


        [Test]
        public void testLoopThird() {

            string html = @"
<!-- BEGIN page -->【#{page.Name}】
<!-- BEGIN category -->#{category.Name}
<!-- BEGIN list --><div>#{title}<br/>#{body}</div>
<!-- END list -->
<!-- END category -->
<!-- END page -->
";

            wojilu.Web.ITemplate tpl = new Template().InitContent( html );

            wojilu.Web.IBlock pageBlock = tpl.GetBlock( "page" );
            for (int j = 1; j < 4; j++) {

                pageBlock.Set( "page.Name", "页面" + j );

                wojilu.Web.IBlock categoryBlock = pageBlock.GetBlock( "category" );
                for (int k = 1; k < 3; k++) {

                    categoryBlock.Set( "category.Name", "分类" + k );

                    wojilu.Web.IBlock block = categoryBlock.GetBlock( "list" );
                    for (int i = 1; i < 3; i++) {
                        block.Set( "title", "我是标题" + k + "_" + i );
                        block.Set( "body", "我是内容" + k + "_" + i );
                        block.Next();
                    }

                    categoryBlock.Next();
                }

                pageBlock.Next();

            }

            string result = tpl.ToString();
            Console.WriteLine( "" );
            Console.WriteLine( "-----------------------------------------------------------------" );
            Console.WriteLine( result );
        }

        [Test]
        public void testGlobalVar() {

            string html = @"页面开头
<!-- BEGIN category -->#{category}<!-- BEGIN list --><div>#{category}:#{title}<br/>#{body}</div><!-- END list -->
<!-- END category -->";

            wojilu.Web.ITemplate tpl = new Template().InitContent( html );
            IBlock block = tpl.GetBlock( "category" );
            for (int i = 0; i < 2; i++) {
                block.Set( "category", "大门"+i );

                IBlock list = block.GetBlock( "list" );
                for (int x = 20; x < 22; x++) {
                    list.Set( "title", "标题"+x );
                    list.Set( "body", "内容" + x );
                    list.Next();
                }

                block.Next();
            }

            string result = tpl.ToString();

            string target = @"页面开头
大门0<div>大门0:标题20<br/>内容20</div><div>大门0:标题21<br/>内容21</div>
大门1<div>大门1:标题20<br/>内容20</div><div>大门1:标题21<br/>内容21</div>
";

            Console.WriteLine( "" );
            Console.WriteLine( "-----------------------------------------------------------------" );
            Console.WriteLine( result );

            Assert.AreEqual( target, result );


        }


        [Test]
        public void testSameVar() {

            string html = @"第一个listing<!-- BEGIN list --><div>#{title}<br/>#{body}</div><!-- END list -->
第二个listing2<!-- BEGIN list2 --><div>#{title}<br/>#{body}</div><!-- END list2 -->
第三个listing3<!-- BEGIN list3 --><div>#{title}<br/>#{body}</div><!-- END list3 -->";

            wojilu.Web.ITemplate tpl = new Template().InitContent( html );

            wojilu.Web.IBlock block = tpl.GetBlock( "list" );
            wojilu.Web.IBlock block2 = tpl.GetBlock( "list2" );
            wojilu.Web.IBlock block3 = tpl.GetBlock( "list3" );

            for (int i = 1; i < 3; i++) {
                block.Set( "title", "我是标题" + i );
                block.Set( "body", "我是内容" + i );
                block.Next();
            }

            for (int i = 1; i < 3; i++) {
                block2.Set( "title", "我是2标题" + i );
                block2.Set( "body", "我是2内容" + i );
                block2.Next();
            }

            for (int i = 1; i < 3; i++) {
                block3.Set( "title", "我是3标题" + i );
                block3.Set( "body", "我是3内容" + i );
                block3.Next();
            }

            string result = tpl.ToString();
            Console.WriteLine( "" );
            Console.WriteLine( "-----------------------------------------------------------------" );
            Console.WriteLine( result );

            string target = @"第一个listing<div>我是标题1<br/>我是内容1</div><div>我是标题2<br/>我是内容2</div>
第二个listing2<div>我是2标题1<br/>我是2内容1</div><div>我是2标题2<br/>我是2内容2</div>
第三个listing3<div>我是3标题1<br/>我是3内容1</div><div>我是3标题2<br/>我是3内容2</div>";

            Assert.AreEqual( target, result );

        }

    }

}
