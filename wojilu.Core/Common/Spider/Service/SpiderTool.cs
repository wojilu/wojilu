using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Spider.Domain;
using System.Text.RegularExpressions;
using wojilu.Data;
using wojilu.Net;
using System.Threading;
using wojilu.Common.Spider.Interface;
using HtmlAgilityPack;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using System.Linq;
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

            if (strUtil.HasText( s.ListUrl )) {
                s.SiteUrl = new UrlInfo( s.ListUrl ).SiteUrl;
            }

            // 一、先抓取列表页面内容
            string page = downloadListPage( s, sb );
            if (strUtil.IsNullOrEmpty( page )) {
                logger.Error( "list page is empty, url=" + s.SiteUrl );
            }

            // 二、得到所有文章的title和url
            List<DetailLink> list = getListItem( s, page, sb );
            return list;
        }

        public static List<DetailLink> getListItem( SpiderTemplate s, string page, StringBuilder sb ) {

            List<DetailLink> list = new List<DetailLink>();
            if (strUtil.IsNullOrEmpty( page )) return list;

            //获取全部url
            MatchCollection matchs = Regex.Matches( page, SpiderConfig.ListLinkPattern, RegexOptions.Singleline );
            if (matchs.Count == 0) {
                logger.Error( "list link match count=0" );
                logInfo( "list link match count=0", s, sb );
            }

            for (int i = matchs.Count - 1; i >= 0; i--) {
                DetailLink dlink = getDetailLink( matchs[i], s );

                if (dlink == null) continue;

                if (dlink.Url.Length > 100) continue;
                list.Add( dlink );
            }
            logInfo( "共抓取到链接：" + list.Count, s, sb );

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
        //解析用户输入的通配符方式的目标网页模式
        //将/www.cnblogs.com/*/archive/*/*/*/*.html
        //转换为www\.cnblogs\.com/(.*?)/archive/(.*?)/(.*?)/(.*?)/(.*?).html
        private static string ParseUrl( string strUrlSrc ) {
            string strRet = strUrlSrc.Replace( ".", @"\." );
            strRet = strRet.Replace( "?", @"\?" );
            strRet = strRet.Replace( "*", "(.*?)" );
            return strRet;
        }

        private static DetailLink getDetailLink( Match match, SpiderTemplate s ) {

            string url = match.Groups[1].Value;
            string title = match.Groups[2].Value;
            //判断输入的url是否满足用户定义的通配符方式的模式
            MatchCollection matchs = Regex.Matches( url, ParseUrl( s.ListPattern ), RegexOptions.Singleline );
            if (matchs.Count == 0) {
                return null;
            }
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

            if (strUtil.HasText( s.ListEncoding )) {
                target = PageLoader.Download( s.ListUrl, SpiderConfig.UserAgent, s.ListEncoding );
            }
            else {
                target = PageLoader.Download( s.ListUrl, SpiderConfig.UserAgent, "" );
            }

            if (strUtil.IsNullOrEmpty( target )) {
                logInfo( "error=原始页面没有内容: " + s.ListUrl, s, sb );
                return target;
            }
            else {
                logInfo( "抓取列表内容成功", s, sb );
            }

            if (strUtil.HasText( s.GetListBodyPattern() )) {
                HtmlDocument htmlDoc = new HtmlDocument {
                    OptionAddDebuggingAttributes = false,
                    OptionAutoCloseOnEnd = true,
                    OptionFixNestedTags = true,
                    OptionReadEncoding = true
                };

                htmlDoc.LoadHtml( target );
                try {
                    IEnumerable<HtmlNode> Nodes = htmlDoc.DocumentNode.QuerySelectorAll( s.GetListBodyPattern() );

                    if (Nodes.Count() > 0) {
                        logInfo( "匹配列表内容成功", s, sb );
                        target = Nodes.ToArray()[0].OuterHtml;
                        target = target.Trim();
                        return target;
                    }
                    else {
                        logInfo( "error=没有匹配的页面内容:" + s.ListUrl, s, sb );
                        return null;
                    }
                }
                catch (Exception ex) {
                    logInfo( "htmlDoc QuerySelectorAll解析出错=" + ex.Message, s, sb );
                    return null;
                }
            }

            //这里未来也可以改成css选择器的方式，来细化目标url集合的范围
            //Match match = Regex.Match(target, s.GetListBodyPattern(), RegexOptions.Singleline);
            //if (match.Success)
            //{
            //    target = match.Value;
            //}
            //else
            //{
            //    target = "";
            //    logInfo("error=没有匹配的页面内容:" + s.ListUrl, s, sb);
            //}

            return target.Trim();
        }

        private static void logInfo( String msg, SpiderTemplate s, StringBuilder sb ) {
            logger.Info( msg );
            sb.AppendLine( msg );
        }

    }


}
