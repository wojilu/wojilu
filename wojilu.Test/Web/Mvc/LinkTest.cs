using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Routes;
using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;

namespace wojilu.Test.Web.Mvc {



    [TestFixture]
    public class LinkTest {




        /*
        LinkMap.Add( "u", "wojilu.Web.Controller.Users.MainController" );
        LinkMap.Add( "us", "wojilu.Web.Controller.Users.MainController.Search" );
        
        // 同名映射会产生歧义，尽量不要使用，比如：
        LinkMap.Add( "blog", "wojilu.Web.Controller.Blog.BlogController" );        
        // blog/category/3.aspx 本来指 Blog.CategoryController 的 Show 方法
        // 现在被指向 Blog.BlogController 的 Category 方法

        // 解决办法：将产生歧义的url，新增另外的映射
        LinkMap.Add( "blogcat", "wojilu.Web.Controller.Blog.CategoryController.Show" );


        LinkMap.Add( "log", "wojilu.Web.Controller.Blog.PostController.Show" );
         
        // 此处会导致论坛回帖的时候，无法局部刷新
        LinkMap.Add( "thread", "wojilu.Web.Controller.Forum.TopicController.Show" );  

        
        LinkMap.ShortUrl( "xz", "/common/page/3.aspx" );
        LinkMap.ShortUrl( "ceo", "us.aspx?name=a&g=0&r=0&a1=0&a2=0&d=0&b=0&p1=0&city1=&p2=0&city2=&z=0" );
         */


        [TestFixtureSetUp]
        public void Init() {
            LinkMap.Add( "post", "wojilu.Test.Web.Mvc.TestPostController" ); // 映射controller
            LinkMap.Add( "category", "wojilu.Test.Web.Mvc.TestPostController.List" ); // 映射action
            LinkMap.Add( "product", "wojilu.Test.Web.Mvc.TestPostController.Product" ); // 映射action_id
            LinkMap.Add( "cm", "wojilu.Test.Web.Mvc.Admin.TestCommentController" );
            LinkMap.Add( "u", "wojilu.Test.Web.Mvc.Admin.Users.TestUserController" );
            LinkMap.Add( "addr", "wojilu.Test.Web.Mvc.Admin.Users.TestUserController.Address" );

            LinkMap.Add( "blog", "wojilu.Test.Web.Mvc.Blog.BlogController" );
        }

        [TestFixtureTearDown]
        public void Clear() {
            LinkMap.Clear();
        }


        [Test]
        public void testLink() {

            LinkMap.SetLinkToLow( false );

            Assert.AreEqual( "/post/Index.aspx", LinkMap.To( new TestPostController().Index ) );
            Assert.AreEqual( "/category.aspx", LinkMap.To( new TestPostController().List ) );
            Assert.AreEqual( "/post/Show/88.aspx", LinkMap.To( new TestPostController().Show, 88 ) );
            Assert.AreEqual( "/product/99.aspx", LinkMap.To( new TestPostController().Product, 99 ) );

            LinkMap.SetLinkToLow( true );

            Assert.AreEqual( "/post/index.aspx", LinkMap.To( new TestPostController().Index ) );
            Assert.AreEqual( "/category.aspx", LinkMap.To( new TestPostController().List ) );
            Assert.AreEqual( "/post/show/88.aspx", LinkMap.To( new TestPostController().Show, 88 ) );
            Assert.AreEqual( "/product/99.aspx", LinkMap.To( new TestPostController().Product, 99 ) );

            // 未映射的Link
            Assert.AreEqual( null, LinkMap.To( new TestArticleController().Index ) );
            Assert.AreEqual( null, LinkMap.To( new TestArticleController().List ) );
            Assert.AreEqual( null, LinkMap.To( new TestArticleController().Show, 88 ) );
            Assert.AreEqual( null, LinkMap.To( new TestArticleController().Product, 99 ) );

        }


        [Test]
        public void testLinkStr() {

            LinkMap.SetLinkToLow( false );

            Assert.AreEqual( "/post/Index.aspx", LinkMap.To( "TestPost", "Index" ) );

            Assert.AreEqual( "/category.aspx", LinkMap.To( "TestPost", "List" ) );
            Assert.AreEqual( "/post/Show/88.aspx", LinkMap.To( "TestPost", "Show", 88 ) );
            Assert.AreEqual( "/product/99.aspx", LinkMap.To( "TestPost", "Product", 99 ) );

            // 未映射的Link
            Assert.AreEqual( null, LinkMap.To( "TestArticle", "Index" ) );
            Assert.AreEqual( null, LinkMap.To( "TestArticle", "List" ) );
            Assert.AreEqual( null, LinkMap.To( "TestArticle", "Show", 88 ) );
            Assert.AreEqual( null, LinkMap.To( "TestArticle", "Product", 99 ) );


        }

        [Test]
        public void testLinkFull() {

            LinkMap.SetLinkToLow( false );

            User u = new User { Id = 3, Url = "zhang" };
            int appId = 5;
            Assert.AreEqual( "/space/zhang/post5/Index.aspx", LinkMap.To( u, new TestPostController().Index, appId ) );
            Assert.AreEqual( "/space/zhang/category5.aspx", LinkMap.To( u, new TestPostController().List, appId ) );
            Assert.AreEqual( "/space/zhang/post5/Show/88.aspx", LinkMap.To( u, new TestPostController().Show, 88, appId ) );
            Assert.AreEqual( "/space/zhang/product5/99.aspx", LinkMap.To( u, new TestPostController().Product, 99, appId ) );

            appId = 0;
            Assert.AreEqual( "/space/zhang/post/Index.aspx", LinkMap.To( u, new TestPostController().Index, appId ) );
            Assert.AreEqual( "/space/zhang/category.aspx", LinkMap.To( u, new TestPostController().List, appId ) );
            Assert.AreEqual( "/space/zhang/post/Show/88.aspx", LinkMap.To( u, new TestPostController().Show, 88, appId ) );
            Assert.AreEqual( "/space/zhang/product/99.aspx", LinkMap.To( u, new TestPostController().Product, 99, appId ) );

            LinkMap.SetLinkToLow( true );

            appId = 5;
            Assert.AreEqual( "/space/zhang/post5/index.aspx", LinkMap.To( u, new TestPostController().Index, appId ) );
            Assert.AreEqual( "/space/zhang/category5.aspx", LinkMap.To( u, new TestPostController().List, appId ) );
            Assert.AreEqual( "/space/zhang/post5/show/88.aspx", LinkMap.To( u, new TestPostController().Show, 88, appId ) );
            Assert.AreEqual( "/space/zhang/product5/99.aspx", LinkMap.To( u, new TestPostController().Product, 99, appId ) );


            Assert.AreEqual( "/post5/index.aspx", LinkMap.To( null, new TestPostController().Index, appId ) );
            Assert.AreEqual( "/category5.aspx", LinkMap.To( null, new TestPostController().List, appId ) );
            Assert.AreEqual( "/post5/show/88.aspx", LinkMap.To( null, new TestPostController().Show, 88, appId ) );
            Assert.AreEqual( "/product5/99.aspx", LinkMap.To( null, new TestPostController().Product, 99, appId ) );

            // 未映射的Link
            Assert.AreEqual( null, LinkMap.To( u, new TestArticleController().Index, appId ) );
            Assert.AreEqual( null, LinkMap.To( u, new TestArticleController().List, appId ) );
            Assert.AreEqual( null, LinkMap.To( u, new TestArticleController().Show, 88, appId ) );
            Assert.AreEqual( null, LinkMap.To( u, new TestArticleController().Product, 99, appId ) );


        }

        [Test]
        public void testLinkFullStr() {
            LinkMap.SetLinkToLow( false );

            User u = new User { Id = 3, Url = "zhang" };
            int appId = 5;
            Assert.AreEqual( "/space/zhang/post5/Index.aspx", LinkMap.To( u, "TestPost", "Index", appId ) );
            Assert.AreEqual( "/space/zhang/category5.aspx", LinkMap.To( u, "TestPost", "List", appId ) );
            Assert.AreEqual( "/space/zhang/post5/Show/88.aspx", LinkMap.To( u, "TestPost", "Show", 88, appId ) );
            Assert.AreEqual( "/space/zhang/product5/99.aspx", LinkMap.To( u, "TestPost", "Product", 99, appId ) );
        }


        [Test]
        public void testParse() {

            LinkMap.SetLinkToLow( false );

            Route x = LinkMap.Parse( "/post8/Index" );
            Assert.AreEqual( "TestPost", x.controller );
            Assert.AreEqual( "Index", x.action );
            Assert.AreEqual( 8, x.appId );

            x = LinkMap.Parse( "/category8" );
            Assert.AreEqual( "TestPost", x.controller );
            Assert.AreEqual( "List", x.action );
            Assert.AreEqual( 8, x.appId );

            x = LinkMap.Parse( "/post8/Show/88" );
            Assert.AreEqual( "TestPost", x.controller );
            Assert.AreEqual( "Show", x.action );
            Assert.AreEqual( 88, x.id );
            Assert.AreEqual( 8, x.appId );

            x = LinkMap.Parse( "/product8/99" );
            Assert.AreEqual( "TestPost", x.controller );
            Assert.AreEqual( "Product", x.action );
            Assert.AreEqual( 99, x.id );
            Assert.AreEqual( 8, x.appId );

            // 未映射
            x = LinkMap.Parse( "/computer8/99" );
            Assert.IsNull( x );

            x = LinkMap.Parse( "/categoryList" );
            Assert.IsNull( x );

            x = LinkMap.Parse( "/cat" );
            Assert.IsNull( x );


            LinkMap.SetLinkToLow( true );

            x = LinkMap.Parse( "/post/index" );
            Assert.AreEqual( "TestPost", x.controller );
            Assert.AreEqual( "index", x.action ); // action由path指定
            x = LinkMap.Parse( "/post/Index" );
            Assert.AreEqual( "TestPost", x.controller );
            Assert.AreEqual( "Index", x.action ); // action由path指定

            x = LinkMap.Parse( "/category" );
            Assert.AreEqual( "TestPost", x.controller );
            Assert.AreEqual( "List", x.action );
            x = LinkMap.Parse( "/Category" );
            Assert.AreEqual( "TestPost", x.controller );
            Assert.AreEqual( "List", x.action );

            x = LinkMap.Parse( "/post/show/88" );
            Assert.AreEqual( "TestPost", x.controller );
            Assert.AreEqual( "show", x.action ); // action由path指定
            Assert.AreEqual( 88, x.id );
            x = LinkMap.Parse( "/post/Show/88" );
            Assert.AreEqual( "TestPost", x.controller );
            Assert.AreEqual( "Show", x.action ); // action由path指定
            Assert.AreEqual( 88, x.id );

            x = LinkMap.Parse( "/product/99" );
            Assert.AreEqual( "TestPost", x.controller );
            Assert.AreEqual( "Product", x.action );
            Assert.AreEqual( 99, x.id );
            x = LinkMap.Parse( "/Product/99" );
            Assert.AreEqual( "TestPost", x.controller );
            Assert.AreEqual( "Product", x.action );
            Assert.AreEqual( 99, x.id );
        }


        [Test]
        public void testParseFull() {

            int appId = 5;

            Route x = LinkMap.Parse( "/space/zhang/post5/Index" );
            Assert.AreEqual( "TestPost", x.controller );
            Assert.AreEqual( "Index", x.action );
            Assert.AreEqual( "zhang", x.owner );
            Assert.AreEqual( "user", x.ownerType );
            Assert.AreEqual( appId, x.appId );


            x = LinkMap.Parse( "/space/zhang/category8" );
            Assert.AreEqual( "TestPost", x.controller );
            Assert.AreEqual( "List", x.action );
            Assert.AreEqual( "zhang", x.owner );
            Assert.AreEqual( "user", x.ownerType );
            Assert.AreEqual( 8, x.appId );


            // 未映射
            x = LinkMap.Parse( "/space/zhang/computer8/99" );
            Assert.IsNull( x );
        }

        [Test]
        public void testNsLink() {

            LinkMap.SetLinkToLow( false );

            Assert.AreEqual( "/cm/Index.aspx", LinkMap.To( new Admin.TestCommentController().Index ) );
            Assert.AreEqual( "/cm/List.aspx", LinkMap.To( new Admin.TestCommentController().List ) );
            Assert.AreEqual( "/cm/Show/88.aspx", LinkMap.To( new Admin.TestCommentController().Show, 88 ) );
            Assert.AreEqual( "/cm/Product/99.aspx", LinkMap.To( new Admin.TestCommentController().Product, 99 ) );

            LinkMap.SetLinkToLow( true );

            Assert.AreEqual( "/cm/index.aspx", LinkMap.To( new Admin.TestCommentController().Index ) );
            Assert.AreEqual( "/cm/list.aspx", LinkMap.To( new Admin.TestCommentController().List ) );
            Assert.AreEqual( "/cm/show/88.aspx", LinkMap.To( new Admin.TestCommentController().Show, 88 ) );
            Assert.AreEqual( "/cm/product/99.aspx", LinkMap.To( new Admin.TestCommentController().Product, 99 ) );

        }


        [Test]
        public void testParseNs() {

            Route x = LinkMap.Parse( "/cm/Index" );
            Assert.AreEqual( "Admin", x.ns );
            Assert.AreEqual( "TestComment", x.controller );
            Assert.AreEqual( "Index", x.action );

            x = LinkMap.Parse( "/cm/List" );
            Assert.AreEqual( "Admin", x.ns );
            Assert.AreEqual( "TestComment", x.controller );
            Assert.AreEqual( "List", x.action );

            x = LinkMap.Parse( "/cm/Show/88" );
            Assert.AreEqual( "Admin", x.ns );
            Assert.AreEqual( "TestComment", x.controller );
            Assert.AreEqual( "Show", x.action );
            Assert.AreEqual( 88, x.id );

            // 未映射
            x = LinkMap.Parse( "/cms/List" );
            Assert.IsNull( x );

            x = LinkMap.Parse( "/c/List" );
            Assert.IsNull( x );
        }


        [Test]
        public void testNsLink2() {

            LinkMap.SetLinkToLow( false );

            Assert.AreEqual( "/u/Index.aspx", LinkMap.To( new Admin.Users.TestUserController().Index ) );
            Assert.AreEqual( "/u/List.aspx", LinkMap.To( new Admin.Users.TestUserController().List ) );
            Assert.AreEqual( "/u/Show/88.aspx", LinkMap.To( new Admin.Users.TestUserController().Show, 88 ) );
            Assert.AreEqual( "/addr/99.aspx", LinkMap.To( new Admin.Users.TestUserController().Address, 99 ) );

            LinkMap.SetLinkToLow( true );

            Assert.AreEqual( "/u/index.aspx", LinkMap.To( new Admin.Users.TestUserController().Index ) );
            Assert.AreEqual( "/u/list.aspx", LinkMap.To( new Admin.Users.TestUserController().List ) );
            Assert.AreEqual( "/u/show/88.aspx", LinkMap.To( new Admin.Users.TestUserController().Show, 88 ) );
            Assert.AreEqual( "/addr/99.aspx", LinkMap.To( new Admin.Users.TestUserController().Address, 99 ) );

        }

        [Test]
        public void testSameMap() {

            LinkMap.SetLinkToLow( false );

            Assert.AreEqual( "/blog/Index.aspx", LinkMap.To( new Blog.BlogController().Index ) );
            Assert.AreEqual( "/blog/List.aspx", LinkMap.To( new Blog.BlogController().List ) );
            Assert.AreEqual( "/blog/Show/88.aspx", LinkMap.To( new Blog.BlogController().Show, 88 ) );

            Route x = LinkMap.Parse( "/blog/Index" );
            Assert.AreEqual( "Blog", x.ns );
            Assert.AreEqual( "Blog", x.controller );
            Assert.AreEqual( "Index", x.action );

            // 正常url
            x = LinkMap.Parse( "/Blog/Blog/Index" );
            Assert.IsNull( x );

            x = LinkMap.Parse( "/space/zhang/Blog1/Blog/Index" );
            Assert.IsNull( x );

            LinkMap.SetLinkToLow( true );

            x = LinkMap.Parse( "/space/zhang/blog1/blog/index" );
            Assert.IsNull( x );

        }


        [Test]
        public void testParseNs2() {

            Route x = LinkMap.Parse( "/u/Index" );
            Assert.AreEqual( "Admin.Users", x.ns );
            Assert.AreEqual( "TestUser", x.controller );
            Assert.AreEqual( "Index", x.action );

            x = LinkMap.Parse( "/u/List" );
            Assert.AreEqual( "Admin.Users", x.ns );
            Assert.AreEqual( "TestUser", x.controller );
            Assert.AreEqual( "List", x.action );

            x = LinkMap.Parse( "/u/Show/88" );
            Assert.AreEqual( "Admin.Users", x.ns );
            Assert.AreEqual( "TestUser", x.controller );
            Assert.AreEqual( "Show", x.action );
            Assert.AreEqual( 88, x.id );

            x = LinkMap.Parse( "/addr/99" );
            Assert.AreEqual( "Admin.Users", x.ns );
            Assert.AreEqual( "TestUser", x.controller );
            Assert.AreEqual( "Address", x.action );
            Assert.AreEqual( 99, x.id );


        }

    }


    public class TestPostController : ControllerBase {

        public void Index() { }
        public void List() { }
        public void Show( long id ) { }
        public void Product( long id ) { }

        public void Add() { }
    }

    public class TestArticleController : ControllerBase {

        public void Index() { }
        public void List() { }
        public void Show( long id ) { }
        public void Product( long id ) { }
    }


    // done 支持 alink.ToApp, alink.ToAppData 
    // done 支持：link.to( String 字符类型的controller ) 以及 rootNamespace
    // done 支持：大小写
    // done 支持：分隔符 /, -, _ 支持
    // done 支持：owner, appId
    // done 支持：翻页 page
    // done u/us/user等映射和 user 个性网址冲突


}

namespace wojilu.Test.Web.Mvc.Blog {

    public class BlogController : ControllerBase {

        public void Index() { }
        public void List() { }
        public void Show( long id ) { }
        public void Product( long id ) { }

        public void Add() { }
    }

}


namespace wojilu.Test.Web.Mvc.Admin {

    public class TestCommentController : ControllerBase {

        public void Index() { }
        public void List() { }
        public void Show( long id ) { }
        public void Product( long id ) { }

        public void Add() { }
    }

}

namespace wojilu.Test.Web.Mvc.Admin.Users {

    public class TestUserController : ControllerBase {

        public void Index() { }
        public void List() { }
        public void Show( long id ) { }
        public void Address( long id ) { }

        public void Add() { }
    }

}