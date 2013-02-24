/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Data;
using wojilu.OAuth;

using wojilu.Common.Onlines;

using wojilu.Web.Context;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Interface;

using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;


namespace wojilu.Web.Controller {

    public class RenderHelper : IMvcFilter {

        private static readonly ILog logger = LogManager.GetLogger( typeof( RenderHelper ) );

        public static Dictionary<string, string> CachedPageList = new Dictionary<string, string>();

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

            if (cPath.ToLower().IndexOf( "/layouts/topnav/nav" ) >= 0) {

                if (e.ctx.web.UserIsLogin == false) {

                    OnlineManager.Refresh( e.ctx );

                    OnlineStats o = OnlineStats.Instance;
                    String onlineInfo = "{\"count\":" + o.Count + ",\"member\":" + o.MemberCount + ",\"guest\":" + o.GuestCount + ",\"max\":" + o.MaxCount + ",\"maxTime\":\"" + o.MaxTime.ToShortDateString() + "\"}";

                    String isSite = cPath.IndexOf( "/Layouts/" ) == 0 ? "true" : "false";
                    String connects = getConnectLinks();

                    //String json = "{\"viewer\":{\"obj\" :{\"Id\":0,\"Name\":\"guest\",\"Url\":\"\"},\"Id\":0,\"IsLogin\":false,\"IsAdministrator\":false,\"HasPic\":false}, \"owner\":{\"IsSite\":" + isSite + ", \"LoginValidImg\":" + config.Instance.Site.LoginNeedImgValidation.ToString().ToLower() + "},\"navInfo\":{\"topNavDisplay\":" + config.Instance.Site.TopNavDisplay + "}, \"online\":" + onlineInfo + "}";
                    String json = "{\"viewer\":{\"obj\" :{\"Id\":0,\"Name\":\"guest\",\"Url\":\"\"},\"Id\":0,\"IsLogin\":false,\"IsAdministrator\":false,\"HasPic\":false}, \"owner\":{\"IsSite\":" + isSite + ", \"LoginValidImg\":" + config.Instance.Site.LoginNeedImgValidation.ToString().ToLower() + "},\"navInfo\":{\"topNavDisplay\":" + config.Instance.Site.TopNavDisplay + "}, \"online\":" + onlineInfo + ", \"connects\":" + connects + "}";



                    e.ctx.RenderJson( json );
                    e.ctx.utils.end();
                    e.ctx.utils.skipRender();

                    return;
                }
            }
        }


        public String getConnectLinks() {

            List<AuthConnectConfig> xlist = AuthConnectConfig.GetEnabledList();
            String lnk = Link.To( Site.Instance, new ConnectController().Login ) + "?connectType=";

            StringBuilder sb = new StringBuilder();
            sb.Append( "[" );

            for (int i = 0; i < xlist.Count; i++) {

                sb.Append( "{" );
                sb.AppendFormat( "\"name\":\"{0}\",", xlist[i].Name );
                sb.AppendFormat( "\"lname\":\"{0}\",", strUtil.HasText( xlist[i].LoginName ) ? xlist[i].LoginName : xlist[i].Name );
                sb.AppendFormat( "\"link\":\"{0}\",", lnk + xlist[i].TypeFullName );
                sb.AppendFormat( "\"logos\":\"{0}\",", xlist[i].LogoS );
                sb.AppendFormat( "\"pick\":\"{0}\"", xlist[i].IsPick );
                sb.Append( "}" );

                if (i < xlist.Count - 1) sb.AppendFormat( "," );

            }

            sb.Append( "]" );
            return sb.ToString();
        }


        void publisher_Begin_Render( object sender, MvcEventArgs e ) {

            String output = e.ctx.utils.getCurrentOutputString();

            if (e.ctx.url.Path.IndexOf( "/Admin/" ) < 0) {

                string[] arrWords = config.Instance.Site.BadWords;
                foreach (String w in arrWords) {
                    output = output.Replace( w, config.Instance.Site.BadWordsReplacement );
                }
            }

            // 加上页面执行时间
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


    }

}
