using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Web;
using wojilu.Reflection;
using System.Reflection;
using wojilu.Web.Templates;
using wojilu.Test.Common.Jsons;
using wojilu.Web.Templates.Tokens;


namespace wojilu {

    public class TVUser {
        public String Name { get; set; }
        public List<TVPage> Pages { get; set; }
    }


    public class TVPage {
        public String Info { get; set; }
        public TVUser User { get; set; }
        public List<TVPost> Posts { get; set; }
    }

    public class TVPost {
        public String Title { get; set; }
        public TVUser User { get; set; }
        public TVPage Page { get; set; }
    }


}

namespace wojilu.Test.Web.Templates {


    [TestFixture]
    public class TemplateNew {

        [Test]
        public void testString() {


            //-----------------------------------------------------
            String html = @"z";
            wojilu.Web.Template tpl = new Template().InitContent( html ) as Template;

            List<Token> tokens = tpl.getTokens();
            Assert.AreEqual( 1, tokens.Count );

            Token x = tokens[0];
            Assert.AreEqual( TokenType.String, x.getType() );
            Assert.AreEqual( "z", x.getValue() );

            //-----------------------------------------------------
            html = @""; // 空字符串不进行解析
            tpl = new Template().InitContent( html ) as Template;

            tokens = tpl.getTokens();
            Assert.AreEqual( 0, tokens.Count );

            //-----------------------------------------------------
            html = @"   "; // 空字符串不进行解析
            tpl = new Template().InitContent( html ) as Template;

            tokens = tpl.getTokens();
            Assert.AreEqual( 0, tokens.Count );

            //-----------------------------------------------------
            html = @"zz";
            tpl = new Template().InitContent( html ) as Template;

            tokens = tpl.getTokens();
            Assert.AreEqual( 1, tokens.Count );

            x = tokens[0];
            Assert.AreEqual( TokenType.String, x.getType() );
            Assert.AreEqual( "zz", x.getValue() );

            //-----------------------------------------------------
            html = @" z";
            tpl = new Template().InitContent( html ) as Template;

            tokens = tpl.getTokens();
            Assert.AreEqual( 1, tokens.Count );

            x = tokens[0];
            Assert.AreEqual( TokenType.String, x.getType() );
            Assert.AreEqual( " z", x.getValue() );

            //-----------------------------------------------------
            html = @"z ";
            tpl = new Template().InitContent( html ) as Template;

            tokens = tpl.getTokens();
            Assert.AreEqual( 1, tokens.Count );

            x = tokens[0];
            Assert.AreEqual( TokenType.String, x.getType() );
            Assert.AreEqual( "z ", x.getValue() );

            //-----------------------------------------------------
            html = @" z ";
            tpl = new Template().InitContent( html ) as Template;

            tokens = tpl.getTokens();
            Assert.AreEqual( 1, tokens.Count );

            x = tokens[0];
            Assert.AreEqual( TokenType.String, x.getType() );
            Assert.AreEqual( " z ", x.getValue() );
        }

        [Test]
        public void testCodeToken() {

            String html = @"<%%>";

            wojilu.Web.Template tpl = new Template().InitContent( html ) as Template;

            List<Token> tokens = tpl.getTokens();
            Assert.AreEqual( 1, tokens.Count );

            Token x = tokens[0];
            Assert.AreEqual( TokenType.Code, x.getType() );
            Assert.AreEqual( "", x.getValue() );

            //-----------------------------------------------------
            html = @"<% %>";
            tpl = new Template().InitContent( html ) as Template;

            tokens = tpl.getTokens();
            Assert.AreEqual( 1, tokens.Count );

            x = tokens[0];
            Assert.AreEqual( TokenType.Code, x.getType() );
            Assert.AreEqual( " ", x.getValue() );

            //-----------------------------------------------------
            html = @"<%  %>";
            tpl = new Template().InitContent( html ) as Template;

            tokens = tpl.getTokens();
            Assert.AreEqual( 1, tokens.Count );

            x = tokens[0];
            Assert.AreEqual( TokenType.Code, x.getType() );
            Assert.AreEqual( "  ", x.getValue() );

            //-----------------------------------------------------
            html = @" <%%>";
            tpl = new Template().InitContent( html ) as Template;

            tokens = tpl.getTokens();
            Assert.AreEqual( 2, tokens.Count );

            x = tokens[1];
            Assert.AreEqual( TokenType.Code, x.getType() );
            Assert.AreEqual( "", x.getValue() );

            //-----------------------------------------------------
            html = @"<%%> ";
            tpl = new Template().InitContent( html ) as Template;

            tokens = tpl.getTokens();
            Assert.AreEqual( 2, tokens.Count );

            x = tokens[0];
            Assert.AreEqual( TokenType.Code, x.getType() );
            Assert.AreEqual( "", x.getValue() );

            //-----------------------------------------------------
            html = @"x<%%>y";
            tpl = new Template().InitContent( html ) as Template;

            tokens = tpl.getTokens();
            Assert.AreEqual( 3, tokens.Count );

            x = tokens[0];
            Assert.AreEqual( TokenType.String, x.getType() );
            Assert.AreEqual( "x", x.getValue() );

            Token n = tokens[1];
            Assert.AreEqual( TokenType.Code, n.getType() );
            Assert.AreEqual( "", n.getValue() );

            Token y = tokens[2];
            Assert.AreEqual( TokenType.String, y.getType() );
            Assert.AreEqual( "y", y.getValue() );


            //-----------------------------------------------------
            html = @"x<% %>y";
            tpl = new Template().InitContent( html ) as Template;

            tokens = tpl.getTokens();
            Assert.AreEqual( 3, tokens.Count );

            x = tokens[0];
            Assert.AreEqual( TokenType.String, x.getType() );
            Assert.AreEqual( "x", x.getValue() );

            n = tokens[1];
            Assert.AreEqual( TokenType.Code, n.getType() );
            Assert.AreEqual( " ", n.getValue() );

            y = tokens[2];
            Assert.AreEqual( TokenType.String, y.getType() );
            Assert.AreEqual( "y", y.getValue() );
        }



        [Test]
        public void testCode() {
            string html = @"

<%@ Page Language=""C#"" %>

<%@ Import Namespace=""wojilu"" %>
<%@ Import Namespace=""System.Collections.Generic"" %>
<%@   Import    Namespace="" wojilu.Test.Orm.Entities ""   %>

<%
html.show( ""这是第一行"" );
html.showLine( ""这是第2行"" );
html.show( ""这是第3行"" );
html.showLine( html.encode( ""<script>"" ) );
List<TVPost> posts = v.data(""post"") as List<TVPost>;
TVPage p = v.data(""p"") as TVPage;
%>
<div>文章列表：</div>
<div>页面信息：#{p.Info}，审核：#{p.User.Name}</div>
<div>页面信息(此行必须先强类型转换)：<%=p.Info%>，审核：<%=p.User.Name %></div>


<script runat=""server"">
        public String getTitle( String title, int count ) {
            return strUtil.CutString( title, count )+""--ztitle"";
        }
</script>

<% if( posts.Count==0 ) { %>
    <div>没有数据#{msg}</div>
<% } else {%>
    <div>数据总共 <%= posts.Count %> 条。#{tip}</div>

    <% for( int i=0;i<posts.Count;i++ ) {%>
        <div>编号：<%=i%>， 标题：<%= getTitle(posts[i].Title,5) %>，作者：<%= posts[i].User.Name %></div>
    <%}%>

<%}%>
<div>---------传统的循环标记-----------</div>
<!-- BEGIN list -->
    <div>没有数据#{x.Title}</div>
<!-- END list -->

<div>---------调用ORM-----------</div>
<%
List<TArticle> articles = db.find<TArticle>( """" ).list();
%>
<% for( int i=0;i<articles.Count;i++ ) { %>
    <div>编号：<%=i%>， 博客：<%= articles[i].Title %>，作者：<%= articles[i].Member.Name %></div>
<%}%>
";
            TVUser u1 = new TVUser { Name = "孙中山1" };
            TVUser u2 = new TVUser { Name = "袁世凯2" };

            TVPage p = new TVPage { Info = "页面info", User = u2 };

            List<TVPost> list = new List<TVPost> { 
                new TVPost { Title = "标题111", User=u1 }, 
                new TVPost { Title = "标题222", User=u2 } 
            };


            wojilu.Web.Template tpl = new Template().InitContent( html ) as Template;

            //tpl.Set( "post", list ); // 这种是错误的
            tpl.Bind( "post", list ); // 绑定对象，必须使用bind
            tpl.Bind( "p", p );
            tpl.Set( "msg", "--" );
            tpl.Set( "postId", 8 );
            tpl.Set( "tip", "{这是动态提示}" );

            tpl.BindList( "list", "x", list );



            ITemplateResult x = tpl.Compile();

            Console.WriteLine( x.GetResult() );
            Console.ReadLine();

        }

        [Test]
        public void testFunction() {

            // 多个函数测试
            String html = @"
<%@ Page Language=""C#"" %>

<%@ Import Namespace=""wojilu"" %>
<%@ Import Namespace=""System.Collections.Generic"" %>

<% List<TVUser> userList=v.data(""ulist"") as List<TVUser> %>

<script runat=""server"">
public String getTitle( String title, int count ) {
    return strUtil.CutString( title, count )+""--ztitle"";
}
public String getUser( String title, int count ) {
    return strUtil.CutString( title, count )+""--zuser"";
}
</script>

<div>数据循环显示</div>
<%foreach( TVUser user in userList ) {%>

    <div>用户：<%= getUser(user.Name,5) %></div>

    <%foreach( TVPage page in user.Pages ) {%>
        <div>页面：<%= page.Info %></div>
        <%foreach( TVPost post in page.Posts ) {%>
            <div>帖子：<%= getTitle(post.Title,10) %></div>
        <%}%>

    <%}%>

<%}%>
";

            wojilu.Web.Template tpl = new Template().InitContent( html ) as Template;

            // 必须使用 Bind
            tpl.Bind( "ulist", getUserList() );

            // 不能使用BindList，因为这是传统区块模式
            //tpl.BindList( "ulist", "x", getUserList() );


            ITemplateResult x = tpl.Compile();
            Console.WriteLine( x.GetResult() );
        }

        // 多级循环测试
        [Test]
        public void testLoop() {

            String html = @"
<%@ Page Language=""C#"" %>

<%@ Import Namespace=""wojilu"" %>
<%@ Import Namespace=""System.Collections.Generic"" %>

<% List<TVUser> userList=v.data(""ulist"") as List<TVUser> %>

<div>数据循环显示</div>
<%foreach( TVUser user in userList ) {%>

    <div>用户：<%= user.Name %></div>

    <%foreach( TVPage page in user.Pages ) {%>
        <div>页面：<%= page.Info %></div>
        <%foreach( TVPost post in page.Posts ) {%>
            <div>帖子：<%= post.Title %></div>
        <%}%>

    <%}%>

<%}%>
";

            wojilu.Web.Template tpl = new Template().InitContent( html ) as Template;

            // 必须使用 Bind
            tpl.Bind( "ulist", getUserList() );

            // 不能使用BindList，因为这是传统区块模式
            //tpl.BindList( "ulist", "x", getUserList() );


            ITemplateResult x = tpl.Compile();
            Console.WriteLine( x.GetResult() );

        }

        private List<TVUser> getUserList() {


            TVUser user1 = new TVUser { Name = "user1" };
            TVUser user2 = new TVUser { Name = "user2" };

            TVPage page1 = new TVPage { Info = "页面1", User = user1 };
            TVPage page2 = new TVPage { Info = "页面2", User = user1 };
            TVPage page3 = new TVPage { Info = "页面3", User = user2 };
            TVPage page4 = new TVPage { Info = "页面4", User = user2 };

            TVPost post1 = new TVPost { Page = page1, Title = "标题1" };
            TVPost post2 = new TVPost { Page = page1, Title = "标题2" };
            TVPost post3 = new TVPost { Page = page2, Title = "标题3" };
            TVPost post4 = new TVPost { Page = page2, Title = "标题4" };
            TVPost post5 = new TVPost { Page = page3, Title = "标题5" };
            TVPost post6 = new TVPost { Page = page3, Title = "标题6" };
            TVPost post7 = new TVPost { Page = page4, Title = "标题7" };
            TVPost post8 = new TVPost { Page = page4, Title = "标题8" };

            List<TVPost> list = new List<TVPost> { post1, post2, post3, post4, post5, post6, post7, post8 };

            page1.Posts = (from x in list where x.Page.Info == page1.Info select x).ToList();
            page2.Posts = (from x in list where x.Page.Info == page2.Info select x).ToList();
            page3.Posts = (from x in list where x.Page.Info == page3.Info select x).ToList();
            page4.Posts = (from x in list where x.Page.Info == page4.Info select x).ToList();

            List<TVPage> pages = new List<TVPage> { page1, page2, page3, page4 };

            user1.Pages = (from x in pages where x.User.Name == user1.Name select x).ToList();
            user2.Pages = (from x in pages where x.User.Name == user2.Name select x).ToList();

            return new List<TVUser> { user1, user2 };
        }

        [Test]
        public void testMsgHtml() {

            String html = @"<!DOCTYPE html>
<html lang=""zh-CN"">
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
<title>_{sysMsg}</title>
<link href=""~css/wojilu._common.css?v=#{cssVersion}"" rel=""stylesheet"" type=""text/css"" />
<script>var __funcList = []; var _run = function (aFunc) { __funcList.push(aFunc); }; var require = { urlArgs: 'v=#{jsVersion}' };</script>
</head>

<body style=""background:#fff;"">


<div style=""width:500px;margin:50px auto;background:#efefef; border:1px #ccc dotted; line-height:150%"">
	<div style=""padding:15px; "">
	
		<div>#{msg}</div>
        <div style=""margin:20px 0px 10px 0px;font-size:14px;font-weight:bold; text-align:center;"" id=""msgInfoGlobalSite"">
            <a href=""javascript:history.back();"">&lsaquo;&lsaquo; _{return}</a> 
            <span id=""lnkHome"" class=""link"" data-link=""#{siteUrl}"" style=""margin-left:30px""><img src=""~img/home.gif""/>_{siteHome}</span>
        </div>
		
	</div>	
</div>
<script>
_run( function() {
    $('#lnkHome').click( function() {
        wojilu.tool.forwardPage( $(this).attr('data-link') );
    });
});
</script>
<script data-main=""~js/main"" src=""~js/lib/require-jquery-wojilu.js?v=#{jsVersion}""></script>
<script>require(['wojilu._nolayout']);</script>
</body>
</html>";

            Template t = new Template();
            t.InitContent( html );

            String ret = t.ToString();

            Console.WriteLine( ret );


        }


    }




}
