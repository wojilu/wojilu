/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Data;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Interface;
using wojilu.Web.Mvc.Processors;
using wojilu.Members.Sites.Domain;
using wojilu.Apps.Forum.Domain;
using wojilu.Caching;
using wojilu.DI;
using System.Data;
using wojilu.ORM;
using System.IO;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Controller.Common;
using wojilu.Common.Onlines;
using wojilu.Web.Mvc.Routes;
using System.Web;
using System.Web.Security;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller {

    public class RenderHelper : IMvcFilter {

        private static readonly ILog logger = LogManager.GetLogger( typeof( RenderHelper ) );

        public static Dictionary<string, string> CachedPageList = new Dictionary<string, string>();

        private MyCacher ch = new MyCacher();

        public void Process( MvcEventPublisher publisher ) {

            publisher.Begin_ProcessMvc += new EventHandler<MvcEventArgs>( publisher_Begin_ProcessMvc );
            publisher.Begin_ParseRoute += new EventHandler<MvcEventArgs>( publisher_Begin_ParseRoute );
            publisher.Begin_Render += new EventHandler<MvcEventArgs>( publisher_Begin_Render );

        }

        void publisher_Begin_ParseRoute( object sender, MvcEventArgs e ) {


            if (MvcConfig.Instance.IsDomainMap == false) return;
            if (SystemInfo.HostIsIp) return;
            if (SystemInfo.HostIsLocalhost) return;

            logger.Info( "===============subdomain publisher_Begin_ParseRoute==============" );

            MvcContext ctx = e.ctx;

            String host = ctx.url.Host;
            if (host == null) return;

            String[] arrItem = host.Split( '.' );
            if (arrItem.Length != 3) return;

            if (strUtil.EqualsIgnoreCase( arrItem[0], "www" )) return;

            String newHostAndPath = getNewHostPath( ctx, arrItem );
            logger.Info( "newHost_final=" + newHostAndPath );

            // 重设当前 url
            e.ctx.setUrl( newHostAndPath );
        }

        //zhangsan.mytest.com/Default.aspx?skinId=2 转换为 www.mytest.com/space/zhangsan.aspx?skinId=2
        //zhangsan.mytest.com/blog/post.aspx 转换为 www.mytest.com/space/zhangsan/blog/post.aspx
        private static String getNewHostPath( MvcContext ctx, String[] arrItem ) {

            String memberPath = MemberPath.GetPath( typeof( User ).Name );

            String newHostAndPath = "http://www." + arrItem[1] + "." + arrItem[2] + "/" + memberPath + "/" + arrItem[0];

            logger.Info( "newHost1=" + newHostAndPath );

            String oUrl = ctx.url.ToString();
            String oPath = ctx.url.SiteAndAppPath;
            String oUrlRest = strUtil.TrimStart( oUrl, oPath );
            //zhangsan.mytest.com/Default.aspx?skinId=2=>/Default.aspx?skinId=2
            //zhangsan.mytest.com/blog/post.aspx=>/blog/post.aspx
            logger.Info( "oUrlRest1=" + oUrlRest );

            String path = strUtil.TrimEnd( ctx.url.Path, MvcConfig.Instance.UrlExt ).TrimStart( '/' );
            logger.Info( "path=" + path );

            if (path == "t") return "http://www." + arrItem[1] + "." + arrItem[2] + "/t/" + arrItem[0] + MvcConfig.Instance.UrlExt;

            if (strUtil.EqualsIgnoreCase( path, defaultPageName ) || strUtil.EqualsIgnoreCase( path, defaultPageName2 )) {

                oUrlRest = strUtil.TrimStart( oUrlRest, "/" );
                int defaultIndex = defaultPageName.Length;
                oUrlRest = oUrlRest.Substring( defaultIndex, oUrlRest.Length - defaultIndex );
                logger.Info( "oUrlRest2=" + oUrlRest );

                newHostAndPath = newHostAndPath + oUrlRest;
                logger.Info( "(default)newHost2=" + newHostAndPath );
            }
            else {
                newHostAndPath = strUtil.Join( newHostAndPath, oUrlRest );
                logger.Info( "(no default)newHost2=" + newHostAndPath );
            }

            return newHostAndPath;
        }

        private static readonly String defaultPageName = "default";
        private static readonly String defaultPageName2 = "Default";


        // 如果缓存中存在，则直接返回
        void publisher_Begin_ProcessMvc( object sender, MvcEventArgs e ) {

            String cPath = e.ctx.url.Path;

            if (cPath.IndexOf( "/Layouts/TopNav/Nav" ) >= 0) {

                if (e.ctx.web.UserIsLogin == false) {

                    OnlineManager.Refresh( e.ctx );

                    OnlineStats o = OnlineStats.Instance;
                    String onlineInfo = "{\"count\":" + o.Count + ",\"member\":" + o.MemberCount + ",\"guest\":" + o.GuestCount + ",\"max\":" + o.MaxCount + ",\"maxTime\":\"" + o.MaxTime.ToShortDateString() + "\"}";

                    String isSite = cPath.IndexOf( "/Layouts/" ) == 0 ? "true" : "false";

                    String json = "{\"viewer\":{\"obj\" :{\"Id\":0,\"Name\":\"guest\",\"Url\":\"\"},\"Id\":0,\"IsLogin\":false,\"IsAdministrator\":false,\"HasPic\":false}, \"owner\":{\"IsSite\":" + isSite + ", \"LoginValidImg\":" + config.Instance.Site.LoginNeedImgValidation.ToString().ToLower() + "},\"navInfo\":{\"topNavDisplay\":" + config.Instance.Site.TopNavDisplay + "}, \"online\":" + onlineInfo + "}";


                    e.ctx.RenderJson( json );
                    e.ctx.utils.end();
                    e.ctx.utils.skipRender();

                    return;

                }


            }


            //------------------------------------------------------

            if (ch.shouldCache( e.ctx ) == false) return;
            String pageContent = ch.ReadCache( e.ctx.url.PathAndQuery, e.ctx );
            if (pageContent == null) return;
            //pageContent = pageContent.Replace( "#{elapseTime}", getTimeStr() );
            pageContent += getTimeJs();

            e.ctx.web.ResponseWrite( pageContent.ToString() );
            e.ctx.utils.end();
            e.ctx.utils.skipRender();

            logger.Info( "----------------------------------cacheEnd------------------------------------" );
            LogManager.Flush();

        }


        void publisher_Begin_Render( object sender, MvcEventArgs e ) {

            String output = e.ctx.utils.getCurrentOutputString();


            if (e.ctx.url.Path.IndexOf( "/Admin/" ) < 0) {

                string[] arrWords = config.Instance.Site.BadWords;
                foreach (String w in arrWords) {
                    output = output.Replace( w, config.Instance.Site.BadWordsReplacement );
                }
            }



            //output = strUtil.ResetScript( output );
            if (ch.shouldCache( e.ctx )) ch.AddCache( e.ctx.url.PathAndQuery, output );
            //output = output.Replace( "#{elapseTime}", getTimeStr() );

            output = output.Replace( "</body>", getTimeJs() + "</body>" );


            e.ctx.utils.setCurrentOutputString( output );

        }

        private static String getTimeJs() {

            StringBuilder sb = new StringBuilder();
            sb.Append( "<script>var eleTime = document.getElementById('elapseTime');if( eleTime ) {" );
            sb.AppendFormat( "eleTime.innerHTML='{0}';", (WebStopwatch.Stop().ElapsedMilliseconds / 1000.0).ToString( "0.0000" ) );
            sb.Append( "};" );
            sb.Append( "var eleSql = document.getElementById('sqlQueries');if( eleSql ) {" );
            sb.AppendFormat( "eleSql.innerHTML='{0}';", DbContext.getSqlCount() );
            sb.Append( "}</script>" );

            return sb.ToString();
        }



        //private static String getTimeStr() {
        //    String elapsed = string.Format( "<div>Processed in {0} seconds, {1} queries</div>",
        //        (WebStopwatch.Stop().ElapsedMilliseconds / 1000.0).ToString( "0.0000" ),
        //        DbContext.getSqlCount()
        //    );

        //    String copyright = "<div>Powered by <a href=\"http://www.wojilu.com\" target=\"_blank\">我记录1.6</a></div>";

        //    String timeStr = copyright + elapsed;
        //    return timeStr;
        //}

        //----------------------------------------------- 有针对性的缓存TODO ------------------------------------------------------

        //private void addPageToCache( MvcEventArgs e, String output ) {
        //    if (shouldCachePage.Contains( e.ctx.url.PathAndQuery )) {
        //        CachedPageList[e.ctx.url.PathAndQuery] = output;
        //    }
        //}

        //private static List<String> shouldCachePage = getShouldCachePages();

        // TODO 1)统计访问量前100位主要页面 2)自定义添加的页面
        //private static List<String> getShouldCachePages() {

        //    List<String> list = new List<String>();

        //    List<String> menuPage = getMenuPages( list );
        //    List<String> mainPages = getMainPages( menuPage );
        //    List<String> forumPages = getForumPages( mainPages );

        //    return menuPage;
        //}

        // 【portal页面通知】
        //  1）每个聚合区块都和service相关，即和domain model相关
        //  2）给ORM增加 interceptor ，一旦插入或者更新或者删除，即更新相关页面的缓存
        //    a）根据domain获取service，以及serviceId
        //    b）查找所有相关portal的appId，得到页面url，然后移除此页面的缓存
        // ——仍然是粗粒度的

        // 【论坛页面通知】
        // 每次发帖、删帖、修改，都获取board，然后更新论坛首页，board页面，topicShow和postShow页面
        // 修改积分单位、用户信息
        // 【论坛登录的区别】针对登录用户和游客进行各自缓存
        private static List<string> getForumPages( List<string> list ) {

            // 获取网站的所有论坛
            List<ForumApp> forums = ForumApp.find( "OwnerType=:otype" ).set( "otype", typeof( Site ).FullName ).list();

            // 获取所有版块页面
            foreach (ForumApp app in forums) {
                List<ForumBoard> boards = ForumBoard.find( "AppId=" + app.Id ).list();

                foreach (ForumBoard bd in boards) {
                    String url = alink.ToAppData( bd );
                    if (list.Contains( url ) == false) list.Add( url );

                    // 获取前5页的列表页
                    for (int i = 2; i < 6; i++) {
                        String pagedUrl = getListPage( url, i );
                        if (list.Contains( pagedUrl ) == false) list.Add( pagedUrl );
                    }
                }
            }

            return list;
        }

        private static string getListPage( string url, int p ) {
            String turl = strUtil.TrimEnd( url, MvcConfig.Instance.UrlExt );
            return turl + "/p" + p + MvcConfig.Instance.UrlExt;
        }

        private static List<string> getMainPages( List<string> list ) {

            List<String> pages = new List<string>();
            pages.Add( "/Users/Main/Index" );
            pages.Add( "/Users/Main/Rank" );
            pages.Add( "/Users/Main/ListAll" );
            pages.Add( "/Users/Main/ListAll/p2" );
            pages.Add( "/Users/Main/ListAll/p3" );

            pages.Add( "/Groups/Main/Index" );
            pages.Add( "/Groups/Main/List" );
            pages.Add( "/Groups/Main/List/p2" );
            pages.Add( "/Groups/Main/List/p3" );

            pages.Add( "/Blog/Main/Index" );
            pages.Add( "/Blog/Main/Recent" );
            pages.Add( "/Blog/Main/Recent/p2" );
            pages.Add( "/Blog/Main/Recent/p3" );

            pages.Add( "/Photo/Main/Index" );
            pages.Add( "/Tag/Index" );

            foreach (String u in pages) {
                String url = u + MvcConfig.Instance.UrlExt;
                if (list.Contains( url ) == false) list.Add( url );
            }
            return list;
        }

        private static List<String> getMenuPages( List<String> list ) {

            List<SiteMenu> menus = cdb.findAll<SiteMenu>();
            foreach (SiteMenu m in menus) {

                if (strUtil.HasText( m.Url )) {
                    String urlWithExt = "/" + m.Url + MvcConfig.Instance.UrlExt;
                    if (list.Contains( urlWithExt ) == false) list.Add( urlWithExt );
                    processDefaultPage( list, m.Url );
                }

                if (strUtil.HasText( m.RawUrl )) {
                    String url = strUtil.Join( "/", m.RawUrl ) + MvcConfig.Instance.UrlExt;
                    if (list.Contains( url ) == false) list.Add( url );
                }

            }

            return list;
        }

        private static void processDefaultPage( List<String> list, String url ) {
            if (strUtil.EqualsIgnoreCase( url, "default" )) {
                String defaultUrl = "/Default" + MvcConfig.Instance.UrlExt;
                String defaultAspx = "/Default.aspx";
                if (list.Contains( defaultUrl ) == false) list.Add( defaultUrl );
                if (list.Contains( defaultAspx ) == false) list.Add( defaultAspx );
            }
        }

    }

}
