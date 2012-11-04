using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Members.Interface;

using wojilu.Web.Mvc.Routes;

namespace wojilu.Test.Web.Mvc {


    /// <summary>
    /// 测试横杠分隔的url，比如 "forum-post-32.aspx"
    /// </summary>
    [TestFixture]
    public class RouteTestNew {



        // 此处点击右键开始测试













        [Test]
        public void testSimple() {

            string routecfg = @"
~/{controller}/{id};default:{action=Show};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter}
~/{controller}/{id}/{action};
";

            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );


            Route result = RouteTool.RecognizePath( "blog-8-show" );

            Assert.AreEqual( "blog", result.controller );
            Assert.AreEqual( "show", result.action );
            Assert.AreEqual( 8, result.id );
            Assert.AreEqual( 1, result.page );

            Route result2 = RouteTool.RecognizePath( "blog-list" );

            Assert.AreEqual( "blog", result2.controller );
            Assert.AreEqual( "list", result2.action );
            Assert.AreEqual( 0, result2.id );
            Assert.AreEqual( 1, result2.page );

            Route result3 = RouteTool.RecognizePath( "blog" );

            Assert.AreEqual( "blog", result3.controller );
            Assert.AreEqual( "Show", result3.action );
            Assert.AreEqual( 0, result3.id );
            Assert.AreEqual( 1, result3.page );



        }

        [Test]
        public void testSubdomain() {


            string routecfg = @"
~/{controller}/{id};default:{action=Show};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter}
~/{controller}/{id}/{action};
";
            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            Route result = RouteTool.RecognizePath( "http://www.abc.com/blog-8-show" );

            Assert.AreEqual( "blog", result.controller );
            Assert.AreEqual( "show", result.action );
            Assert.AreEqual( 8, result.id );
            Assert.AreEqual( 1, result.page );

            Route result2 = RouteTool.RecognizePath( "http://blog.abc.com/blog-8-show" );

            Assert.AreEqual( "blog", result2.controller );
            Assert.AreEqual( "show", result2.action );
            Assert.AreEqual( 8, result2.id );
            Assert.AreEqual( 1, result2.page );


        }

        [Test]
        public void testQueryItem() {

            string routecfg = @"
article/{typeid}/{id};default:{controller=_.Article,action=List}
search/{query};default:{controller=_.Main,action=Search}
product/{factory}/{category};default:{controller=_.Product,action=List}
book/{author};default:{controller=_.Book,action=List}


~/{controller}/{id};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter,action=letter}
~/{controller}/{id}/{action};requirements:{controller=letter,id=int,action=letter}
~/{controller}/{action}/{page};requirements:{controller=letter,action=letter,page=page}
~/{controller}/{id}/{page};requirements:{controller=letter,id=int,page=page}
~/{controller}/{id}/{action}/{page};requirements:{controller=letter,id=int,action=letter,page=page}
";


            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            Route result = RouteTool.RecognizePath( "blog-8-show" );

            Assert.AreEqual( "blog", result.controller );
            Assert.AreEqual( "show", result.action );
            Assert.AreEqual( 8, result.id );
            Assert.AreEqual( 1, result.page );

            Route result2 = RouteTool.RecognizePath( "blog-list" );

            Assert.AreEqual( "blog", result2.controller );
            Assert.AreEqual( "list", result2.action );
            Assert.AreEqual( 0, result2.id );
            Assert.AreEqual( 1, result2.page );

            result = RouteTool.RecognizePath( "search-新闻" );
            Assert.AreEqual( result.controller, "Main" );
            Assert.AreEqual( result.action, "Search" );
            Assert.AreEqual( result.query, "新闻" );

            result = RouteTool.RecognizePath( "product-苹果-电脑" );
            Assert.AreEqual( result.controller, "Product" );
            Assert.AreEqual( result.action, "List" );
            Assert.AreEqual( result.getItem( "factory" ), "苹果" );
            Assert.AreEqual( result.getItem( "category" ), "电脑" );

            result = RouteTool.RecognizePath( "book-金庸" );
            Assert.AreEqual( result.controller, "Book" );
            Assert.AreEqual( result.action, "List" );
            Assert.AreEqual( result.getItem( "author" ), "金庸" );

            //-------------

            result = RouteTool.RecognizePath( "article-12-1" );
            Assert.AreEqual( result.controller, "Article" );
            Assert.AreEqual( result.action, "List" );
            Assert.AreEqual( result.getItem( "typeid" ), "12" );
            Assert.AreEqual( 1, result.id );
        }

        [Test]
        public void testNs() {

            string routecfg = @"
~/{controller}/{id};default:{action=Show};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter}
~/{controller}/{id}/{action};
";

            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            Route rt = RouteTool.RecognizePath( "myapp-newdata-blog-8-show" );

            Assert.AreEqual( "blog", rt.controller );
            Assert.AreEqual( "show", rt.action );
            Assert.AreEqual( 8, rt.id );
            Assert.AreEqual( "myapp.newdata", rt.ns );

            Route result = RouteTool.RecognizePath( "blog-8-show" );

            Assert.AreEqual( "blog", result.controller );
            Assert.AreEqual( "show", result.action );
            Assert.AreEqual( 8, result.id );

            Assert.AreEqual( string.Empty, result.ns );
        }

        [Test]
        public void testOwner() {

            string routecfg = @"
~/{controller}/{id};default:{action=Show};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter}
~/{controller}/{id}/{action};
";


            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            Route rt = RouteTool.RecognizePath( "blog-8-show" );

            Assert.AreEqual( "blog", rt.controller );
            Assert.AreEqual( "show", rt.action );
            Assert.AreEqual( 8, rt.id );
            Assert.AreEqual( "site", rt.owner );
            Assert.AreEqual( "site", rt.ownerType );

            Route rt2 = RouteTool.RecognizePath( "space-zhangsan-blog-8-show" );

            Assert.AreEqual( "blog", rt2.controller );
            Assert.AreEqual( "show", rt2.action );
            Assert.AreEqual( 8, rt2.id );
            Assert.AreEqual( "zhangsan", rt2.owner );
            Assert.AreEqual( "user", rt2.ownerType );

            Route rt3 = RouteTool.RecognizePath( "space-zhangsan-myapp-newdata-blog-8-show" );
            Assert.AreEqual( "blog", rt3.controller );
            Assert.AreEqual( "show", rt3.action );
            Assert.AreEqual( 8, rt3.id );
            Assert.AreEqual( "zhangsan", rt3.owner );
            Assert.AreEqual( "user", rt3.ownerType );
            Assert.AreEqual( "myapp.newdata", rt3.ns );

            Route rt4 = RouteTool.RecognizePath( "group-zhangsan-myapp-newdata-blog-8-show" );
            Assert.AreEqual( "blog", rt4.controller );
            Assert.AreEqual( "show", rt4.action );
            Assert.AreEqual( 8, rt4.id );
            Assert.AreEqual( "zhangsan", rt4.owner );
            Assert.AreEqual( "group", rt4.ownerType );
            Assert.AreEqual( "myapp.newdata", rt4.ns );
        }

        [Test]
        public void testAppId() {

            string routecfg = @"
~/{controller}/{id};default:{action=Show};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter}
~/{controller}/{id}/{action};
";

            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            Route rt = RouteTool.RecognizePath( "blog256-post-8-show" );

            Assert.AreEqual( "post", rt.controller );
            Assert.AreEqual( "show", rt.action );
            Assert.AreEqual( 8, rt.id );
            Assert.AreEqual( "blog", rt.ns );
            Assert.AreEqual( 256, rt.appId );

            Route rt3 = RouteTool.RecognizePath( "space-zhangsan-myapp38-newdata-blog-8-show" );

            Assert.AreEqual( "blog", rt3.controller );
            Assert.AreEqual( "show", rt3.action );
            Assert.AreEqual( 8, rt3.id );

            Assert.AreEqual( "zhangsan", rt3.owner );
            Assert.AreEqual( "user", rt3.ownerType );

            Assert.AreEqual( "myapp.newdata", rt3.ns );
            Assert.AreEqual( 38, rt3.appId );

            Route x4 = RouteTool.RecognizePath( "Forum11-Board-21" );
            Assert.AreEqual( "Board", x4.controller );
            Assert.AreEqual( "Show", x4.action );
            Assert.AreEqual( 11, x4.appId );
            Assert.AreEqual( 21, x4.id );
            Assert.AreEqual( 1, x4.page );

        }

        [Test]
        public void testHomepage() {

            string routecfg = @"
default;default:{controller=SiteMain, action=Index}
{owner};default:{ownertype=space}
~/{controller}/{id};default:{action=Show};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter}
~/{controller}/{id}/{action};
";


            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            Route rt = RouteTool.RecognizePath( "" );

            Assert.AreEqual( "site", rt.owner );
            Assert.AreEqual( "site", rt.ownerType );

            Assert.AreEqual( "SiteMain", rt.controller );
            Assert.AreEqual( "Index", rt.action );
            Assert.AreEqual( 0, rt.id );
            Assert.AreEqual( "", rt.ns );
            Assert.AreEqual( 0, rt.appId );


            Route rt2 = RouteTool.RecognizePath( "http://www.abc.com" );

            Assert.AreEqual( "site", rt2.owner );
            Assert.AreEqual( "site", rt2.ownerType );

            Route rt3 = RouteTool.RecognizePath( "http://blog.abc.com" );

            Assert.AreEqual( "site", rt3.owner );
            Assert.AreEqual( "site", rt3.ownerType );

        }


        [Test]
        public void testNamedItems() {

            string routecfg = @"
myapp;default:{controller=MyApp,action=List,appId=5}
~/{controller}/{id};default:{action=Show};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter}
~/{controller}/{id}/{action};
";

            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            List<RouteSetting> list = RouteTable.GetRoutes();
            Assert.AreEqual( 4, list.Count );

            Route rr1 = RouteTool.RecognizePath( "myapp" );
            Assert.AreEqual( "", rr1.ns );
            Assert.AreEqual( "site", rr1.ownerType );
            Assert.AreEqual( "site", rr1.owner );
            Assert.AreEqual( "MyApp", rr1.controller );
            Assert.AreEqual( "List", rr1.action );
            Assert.AreEqual( 5, rr1.appId );


        }

        [Test]
        public void testNamedRoute() {

            string routecfg = @"
default;default:{controller=SiteMain, action=Index}
login;default:{controller=SiteMain,action=Login}
logout;default:{controller=SiteMain,action=Logout}
register;default:{controller=SiteMain,action=Register}

user;default:{ownertype=site,owner=site}
bbs;default:{ownertype=site,owner=site}
group;default:{ownertype=site,owner=site}

//~/{controller}/{id}/{action};requirements:{controller=letter,id=int,action=letter}
//~/{controller}/{id}/{action}/{page};requirements:{controller=letter,id=int,action=letter,page=page}

~/{controller}/{id};default:{action=Show};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter}
~/{controller}/{id}/{action};
";




            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            Route rr1 = RouteTool.RecognizePath( "login" );
            Assert.AreEqual( "", rr1.ns );
            Assert.AreEqual( "site", rr1.ownerType );
            Assert.AreEqual( "site", rr1.owner );
            Assert.AreEqual( "SiteMain", rr1.controller );
            Assert.AreEqual( "Login", rr1.action );

            Route rr2 = RouteTool.RecognizePath( "logout" );
            Assert.AreEqual( "site", rr2.ownerType );
            Assert.AreEqual( "site", rr2.owner );
            Assert.AreEqual( "SiteMain", rr2.controller );
            Assert.AreEqual( "Logout", rr2.action );

            Route rr3 = RouteTool.RecognizePath( "register" );
            Assert.AreEqual( "site", rr3.ownerType );
            Assert.AreEqual( "site", rr3.owner );
            Assert.AreEqual( "SiteMain", rr3.controller );
            Assert.AreEqual( "Register", rr3.action );

            Route rt4 = RouteTool.RecognizePath( "default" );
            Assert.AreEqual( "site", rt4.ownerType );
            Assert.AreEqual( "site", rt4.owner );
            Assert.AreEqual( "SiteMain", rt4.controller );
            Assert.AreEqual( "Index", rt4.action );

            Route rt41 = RouteTool.RecognizePath( "" );
            Assert.AreEqual( "site", rt41.ownerType );
            Assert.AreEqual( "site", rt41.owner );
            Assert.AreEqual( "SiteMain", rt41.controller );
            Assert.AreEqual( "Index", rt41.action );

            Route rr5 = RouteTool.RecognizePath( "user" );
            Assert.AreEqual( "site", rr5.ownerType );
            Assert.AreEqual( "site", rr5.owner );
            Assert.AreEqual( "", rr5.controller );
            Assert.AreEqual( "Index", rr5.action );

            Route rr6 = RouteTool.RecognizePath( "bbs" );
            Assert.AreEqual( "site", rr6.ownerType );
            Assert.AreEqual( "site", rr6.owner );
            Assert.AreEqual( "", rr6.controller );
            Assert.AreEqual( "Index", rr6.action );

            Route rr7 = RouteTool.RecognizePath( "group" );
            Assert.AreEqual( "site", rr7.ownerType );
            Assert.AreEqual( "site", rr7.owner );
            Assert.AreEqual( "", rr7.controller );
            Assert.AreEqual( "Index", rr7.action );
        }

        [Test]
        public void testUserUrl() {

            string routecfg = @"

{owner};default:{ownertype=user,controller=Users.Microblog,action=List}
t/{owner};default:{ownertype=user,controller=Users.Microblog,action=List}
t/{owner}/{page};default:{ownertype=user,controller=Users.Microblog,action=List}

//~/{controller}/{id}/{action};requirements:{controller=letter,id=int,action=letter}
//~/{controller}/{id}/{action}/{page};requirements:{controller=letter,id=int,action=letter,page=page}

~/{controller}/{id};default:{action=Show};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter}
~/{controller}/{id}/{action};
";
            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            Route rr3 = RouteTool.RecognizePath( "dddddddddd" );
            Assert.AreEqual( "user", rr3.ownerType );
            Assert.AreEqual( "dddddddddd", rr3.owner );

            Route rt3 = RouteTool.RecognizePath( "space-zhangsan-myapp38-newdata-blog-8-show" );

            Assert.AreEqual( "blog", rt3.controller );
            Assert.AreEqual( "show", rt3.action );
            Assert.AreEqual( 8, rt3.id );

            Assert.AreEqual( "zhangsan", rt3.owner );
            Assert.AreEqual( "user", rt3.ownerType );

            Assert.AreEqual( "myapp.newdata", rt3.ns );
            Assert.AreEqual( 38, rt3.appId );

            Route nrt = RouteTool.RecognizePath( "space-lvxing-Blog1-BlogComment-47-Create" );
            Assert.AreEqual( "BlogComment", nrt.controller );
            Assert.AreEqual( "Create", nrt.action );
            Assert.AreEqual( 47, nrt.id );
            Assert.AreEqual( "Blog.BlogComment", nrt.getControllerNameWithoutRootNamespace() );
            Assert.AreEqual( "lvxing", nrt.owner );
            Assert.AreEqual( "user", nrt.ownerType );

        }

        [Test]
        public void testMicroblogUrl() {

            string routecfg = @"
t-{owner};default:{ownertype=user,controller=Users.Microblog,action=List}

~/{controller}/{id};default:{action=Show};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter}
~/{controller}/{id}/{action};
";
            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            Route result = RouteTool.RecognizePath( "t-zhangsan" );

            Assert.AreEqual( "zhangsan", result.owner );
            Assert.AreEqual( "Users", result.ns );
            Assert.AreEqual( "Microblog", result.controller );
            Assert.AreEqual( "List", result.action );
        }



        [Test]
        public void testNamedItem() {

            // default值中如果明确指定 ns=empty（用 _.表示空的命名空间），表示命名空间的值为空，会覆盖前面的命名空间

            string routecfg = @"
tag/{query};default:{ownertype=site,owner=site,controller=_.Tag,action=Show}
blog/{controller}/{action}/{query}
photo/{controller}/{action}/{query};default:{ns=myphoto};

//~/{controller}/{id}/{action};requirements:{controller=letter,id=int,action=letter}
//~/{controller}/{id}/{action}/{page};requirements:{controller=letter,id=int,action=letter,page=page}

~/{controller}/{id};default:{action=Show};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter}
~/{controller}/{id}/{action};
";


            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            Route result = RouteTool.RecognizePath( "blog-post-show-recent" );

            Assert.AreEqual( "blog", result.ns );
            Assert.AreEqual( "post", result.controller );
            Assert.AreEqual( "show", result.action );
            Assert.AreEqual( "recent", result.getItem( "query" ) );
            Assert.AreEqual( "recent", result.query );

            Assert.AreEqual( 0, result.id );


            Route r2 = RouteTool.RecognizePath( "photo-post-show-recent" );

            Assert.AreEqual( "myphoto", r2.ns );
            Assert.AreEqual( "post", r2.controller );
            Assert.AreEqual( "show", r2.action );
            Assert.AreEqual( "recent", r2.query );

            Route rtTag = RouteTool.RecognizePath( "tag-互联网" );

            Assert.AreEqual( "", rtTag.ns );
            Assert.AreEqual( "Tag", rtTag.controller );
            Assert.AreEqual( "Show", rtTag.action );

            Route rt3 = RouteTool.RecognizePath( "space-zhangsan-myapp38-newdata-blog-8-show" );

            Assert.AreEqual( "blog", rt3.controller );
            Assert.AreEqual( "show", rt3.action );
            Assert.AreEqual( 8, rt3.id );

            Assert.AreEqual( "zhangsan", rt3.owner );
            Assert.AreEqual( "user", rt3.ownerType );

            Assert.AreEqual( "myapp.newdata", rt3.ns );
            Assert.AreEqual( 38, rt3.appId );
        }

        [Test]
        public void testDefaultAction() {

            string routecfg = @"
~/{controller}/{id};default:{action=Show};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter}
~/{controller}/{id}/{action};
";

            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            Route result = RouteTool.RecognizePath( "blog-post-256" );

            Assert.AreEqual( "post", result.controller );
            Assert.AreEqual( "Show", result.action );
            Assert.AreEqual( 256, result.id );

        }

        [Test]
        public void testPage() {

            string routecfg = @"
~/{controller}/{id};default:{action=Show};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter,action=letter}
~/{controller}/{id}/{action};requirements:{controller=letter,id=int,action=letter}
~/{controller}/{action}/{page};requirements:{controller=letter,action=letter,page=page}
~/{controller}/{id}/{page};requirements:{page=page}
";
            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            Route result = RouteTool.RecognizePath( "blog-8-p22" );

            Assert.AreEqual( "blog", result.controller );
            Assert.AreEqual( "Show", result.action );
            Assert.AreEqual( 8, result.id );
            Assert.AreEqual( 22, result.page );

            Route rt = RouteTool.RecognizePath( "blog-Index-p22" );

            Assert.AreEqual( "blog", rt.controller );
            Assert.AreEqual( "Index", rt.action );
            Assert.AreEqual( 0, rt.id );
            Assert.AreEqual( 22, rt.page );

        }

        [Test]
        public void testNewSeparator() {

            string routecfg = @"
//~/{controller}-{id}-{action};requirements:{controller=letter,id=int,action=letter}
~/{controller}/{id};default:{action=Show};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter,action=letter}
~/{controller}/{id}/{action};requirements:{controller=letter,id=int,action=letter}
~/{controller}/{action}/{page};requirements:{controller=letter,action=letter,page=page}
~/{controller}/{id}/{page};requirements:{page=page}
";
            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            Route result = RouteTool.RecognizePath( "blog-8-p22" );

            Assert.AreEqual( "blog", result.controller );
            Assert.AreEqual( "Show", result.action );
            Assert.AreEqual( 8, result.id );
            Assert.AreEqual( 22, result.page );

            Route rt = RouteTool.RecognizePath( "blog-Index-p22" );

            Assert.AreEqual( "blog", rt.controller );
            Assert.AreEqual( "Index", rt.action );
            Assert.AreEqual( 0, rt.id );
            Assert.AreEqual( 22, rt.page );


        }

        public void testOwnerName() {

            string routecfg = @"
default;default:{controller=Main.SiteMain}

home/{userName};default:{controller=Users.MainPage,action=Index}
home/{userName}/{controller}/{action};requirements:{controller=letter,action=letter}

~/{controller}/{id};requirements:{id=int}
~/{controller}/{action};requirements:{controller=letter,action=letter}
~/{controller}/{id}/{action};requirements:{controller=letter,id=int,action=letter}
~/{controller}/{action}/{page};requirements:{controller=letter,action=letter,page=page}
~/{controller}/{id}/{page};requirements:{controller=letter,id=int,page=page}
~/{controller}/{id}/{action}/{page};requirements:{controller=letter,id=int,action=letter,page=page}
";

            RouteTable.GetRoutes().Clear();
            RouteTable.Init( routecfg );

            Route result = RouteTool.RecognizePath( "space-user3" );
            Assert.AreEqual( "user", result.ownerType );
            Assert.AreEqual( "user3", result.owner );

            Route rt = RouteTool.RecognizePath( "group-gao086" );
            Assert.AreEqual( "group", rt.ownerType );
            Assert.AreEqual( "gao086", rt.owner );


        }

    }


}
