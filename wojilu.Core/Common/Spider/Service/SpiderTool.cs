using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Spider.Domain;
using System.Text.RegularExpressions;
using wojilu.Data;
using wojilu.Net;
using System.Threading;
using wojilu.Common.Spider.Interface;

namespace wojilu.Common.Spider.Service {

    public class SpiderTool : ISpiderTool {

        private static readonly ILog logger = LogManager.GetLogger( typeof( SpiderTool ) );
        private static Random rd = new Random();

        public void DownloadPage( SpiderTemplate s, StringBuilder log, int[] arrSleep ) {

            logger.Info( "抓取列表页..." + s.SiteName + "_" + s.ListUrl );
            log.AppendLine( "抓取列表页..." + s.SiteName + "_" + s.ListUrl );

            List<DetailLink> list = GetDataList( s, log );

            foreach (DetailLink link in list) {
                savePageDetail( link, log );

                // 暂停几秒，TODO 可配置
                int sleepms = rd.Next( arrSleep[0], arrSleep[1] );
                Thread.Sleep( sleepms );
            }

            log.AppendLine( "抓取完毕。" );

        }

        public static List<DetailLink> GetDataList( SpiderTemplate s, StringBuilder sb ) {

            if (strUtil.HasText( s.ListUrl ))
                s.SiteUrl = new UrlInfo( s.ListUrl ).SiteUrl;

            // 一、先抓取列表页面内容
            string page = downloadListPage( s, sb );

            // 二、得到所有文章的title和url
            List<DetailLink> list = getListItem( s, page, sb );
            return list;
        }

        public static List<DetailLink> getListItem( SpiderTemplate s, string page, StringBuilder sb ) {

            List<DetailLink> list = new List<DetailLink>();
            if (strUtil.IsNullOrEmpty( page )) return list;

            MatchCollection matchs = Regex.Matches( page, s.ListPattern, RegexOptions.Singleline );
            sb.AppendLine( "共抓取到链接：" + matchs.Count );
            for (int i = matchs.Count - 1; i >= 0; i--) {
                DetailLink dlink = getDetailLink( matchs[i], s );

                if (dlink == null) continue;

                if (dlink.Url.Length > 100) continue;
                list.Add( dlink );
            }
            return list;
        }

        private static void savePageDetail( DetailLink lnk, StringBuilder sb ) {

            SpiderTemplate template = lnk.Template;
            string url = lnk.Url;
            string title = lnk.Title;
            string summary = lnk.Abstract;

            if (isPageExist( url, sb )) return;

            String pageBody = new PagedDetailSpider().GetContent( url, template, sb );
            if (pageBody == null) return;

            SpiderArticle pd = new SpiderArticle();
            pd.Title = title;
            pd.Url = strUtil.SubString( url, 200 );
            pd.Abstract = summary;
            pd.Body = pageBody;
            pd.SpiderTemplate = template;

            MatchCollection matchs = Regex.Matches( pageBody, RegPattern.Img, RegexOptions.Singleline );
            if (matchs.Count > 0) {
                pd.IsPic = 1;
                pd.PicUrl = matchs[0].Groups[1].Value;
            }

            pd.insert();

            sb.AppendLine( "保存成功..." + lnk.Title + "_" + lnk.Url );
        }

        //检查数据库中是否已经存在此数据？
        private static bool isPageExist( string pageUrl, StringBuilder sb ) {
            bool isExist = false;
            List<SpiderArticle> list = SpiderArticle.find( "Url=:url and IsDelete=0" ).set( "url", pageUrl ).list();
            if (list.Count > 0) {
                logger.Info( "pass..." + pageUrl );
                sb.AppendLine( "pass..." + pageUrl );
                isExist = true;
            }
            return isExist;
        }

        private static DetailLink getDetailLink( Match match, SpiderTemplate s ) {

            string url = match.Groups[1].Value;
            string title = match.Groups[2].Value;

            if (url.IndexOf( "javascript:" ) >= 0) return null;
            if (url.StartsWith( "#" )) return null;

            title = Regex.Replace( title, "<.+?>", "" );
            if (strUtil.IsNullOrEmpty( title )) return null;
            if (title == "更多") return null;
            if (title == "more") return null;
            if (title == "更多&gt;&gt;") return null;

            string summary = "";
            if (match.Groups.Count > 2) summary = match.Groups[3].Value;

            if (url.StartsWith( "http" ) == false) url = strUtil.Join( s.SiteUrl, url );


            DetailLink lnk = new DetailLink();
            lnk.Template = s;
            lnk.Url = url;
            lnk.Title = title;
            lnk.Abstract = summary;

            return lnk;
        }

        private static string downloadListPage( SpiderTemplate s, StringBuilder sb ) {
            string page = null;

            try {
                page = downloadListPageBody( s, sb );

            }
            catch (Exception ex) {
                logInfo( "error=抓取" + s.ListUrl + "发生错误：" + ex.Message, s, sb );

                return page;
            }

            return page;
        }


        private static string downloadListPageBody( SpiderTemplate s, StringBuilder sb ) {

            String target;

            if (strUtil.HasText( s.ListEncoding ))
                target = PageLoader.Download( s.ListUrl, SpiderConfig.UserAgent, s.ListEncoding );
            else
                target = PageLoader.Download( s.ListUrl, SpiderConfig.UserAgent, "" );

            if (strUtil.IsNullOrEmpty( target )) {

                logInfo( "error=原始页面没有内容: " + s.ListUrl, s, sb );

                return target;
            }

            Match match = Regex.Match( target, s.GetListBodyPattern(), RegexOptions.Singleline );
            if (match.Success) {
                target = match.Value;
            }
            else {
                target = "";
                logInfo( "error=没有匹配的页面内容:" + s.ListUrl, s, sb );
            }

            return target.Trim();
        }

        private static void logInfo( String msg, SpiderTemplate s, StringBuilder sb ) {
            logger.Info( msg );
            sb.AppendLine( msg );
        }

    }


}
