using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

using wojilu.Net;
using wojilu.Web.Utils;
using wojilu.Common.Spider.Domain;

using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;

namespace wojilu.Common.Spider.Service {

    // 其抓取内容，不负责分页部分
    public class DetailSpider {

        private static readonly ILog logger = LogManager.GetLogger( typeof( DetailSpider ) );

        protected SpiderTemplate _template; // 包括配置：pattern/encoding/savePic
        protected String _url;
        protected StringBuilder _log;

        public virtual String GetContent( String url, SpiderTemplate s, StringBuilder sb ) {
            this._template = s;
            this._url = url;
            this._log = sb;
            return this.getPageContent();
        }

        private string getPageContent() {

            // 1) 抓取网页内容
            string page = getDetailPageBody( this._url, this._template, this._log );
            if (string.IsNullOrEmpty( page )) return null;

            // 2) 获取匹配的部分
            page = getMatchedBody( page, this._template, this._log );
            if (string.IsNullOrEmpty( page )) return null;

            // 3) 图片处理
            if (this._template.IsSavePic == 1) {
                page = processPic( page, this._template.SiteUrl );
            }

            return page;
        }

        protected string getDetailPageBody( string detailUrl, SpiderTemplate template, StringBuilder sb ) {

            try {

                sb.AppendLine( "抓取详细页..." + detailUrl );

                String page;
                if (strUtil.HasText( template.DetailEncoding ))
                    page = PageLoader.Download( detailUrl, SpiderConfig.UserAgent, template.DetailEncoding );
                else
                    page = PageLoader.Download( detailUrl, SpiderConfig.UserAgent, "" );

                template.SiteUrl = new UrlInfo( detailUrl ).SiteUrl;

                if (strUtil.IsNullOrEmpty( page )) {
                    logInfo( "error=原始页面没有内容:" + detailUrl, detailUrl, template, sb );
                }

                return page;

            }
            catch (Exception ex) {
                logInfo( "error=抓取" + detailUrl + "发生错误：" + ex.Message, detailUrl, template, sb );
                return null;
            }
        }

        //css选择器方式提取详细页内容
        protected string getMatchedBody( HtmlDocument htmlDoc, SpiderTemplate s, StringBuilder sb ) {
            IEnumerable<HtmlNode> Nodes = htmlDoc.DocumentNode.QuerySelectorAll( s.GetDetailPattern() );
            if (Nodes.Count() > 0) {
                String fpage = Nodes.ToArray()[0].OuterHtml;
                return fpage;
            }
            else {
                logInfo( "error=没有匹配的页面内容:" + _url, this._url, s, sb );
                return null;
            }
        }
        protected string getMatchedBody( string page, SpiderTemplate s, StringBuilder sb ) {
            Match match = Regex.Match( page, s.GetDetailPattern(), RegexOptions.Singleline );
            if (match == null || !match.Success || string.IsNullOrEmpty( match.Value )) {
                logInfo( "error=没有匹配的页面内容:" + _url, this._url, s, sb );
                return null;
            }

            return match.Groups[1].Value;
        }

        //利用HtmlAgilityPack生成HtmlDocument
        protected HtmlDocument getDetailPageBodyHtmlDocument( string detailUrl, SpiderTemplate template, StringBuilder sb ) {
            try {
                sb.AppendLine( "抓取详细页..." + detailUrl );
                HtmlDocument htmlDoc = new HtmlDocument {
                    OptionAddDebuggingAttributes = false,
                    OptionAutoCloseOnEnd = true,
                    OptionFixNestedTags = true,
                    OptionReadEncoding = true
                };

                String page;
                if (strUtil.HasText( template.DetailEncoding ))
                    page = PageLoader.Download( detailUrl, SpiderConfig.UserAgent, template.DetailEncoding );
                else
                    page = PageLoader.Download( detailUrl, SpiderConfig.UserAgent, "" );

                htmlDoc.LoadHtml( page );

                return htmlDoc;

            }
            catch (Exception ex) {
                logInfo( "error=抓取" + detailUrl + "发生错误：" + ex.Message, detailUrl, template, sb );
                return null;
            }
        }





        // 下载所有相关图片到本地
        protected string processPic( string pageBody, string siteUrl ) {

            return PageLoader.ProcessPic( pageBody, siteUrl );
        }



        private static void logInfo( String msg, String detailUrl, SpiderTemplate s, StringBuilder sb ) {
            logger.Info( msg );
            sb.AppendLine( msg );
        }

    }


}
