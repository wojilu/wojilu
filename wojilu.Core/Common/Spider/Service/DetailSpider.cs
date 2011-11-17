using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using wojilu.Drawing;
using wojilu.Net;
using wojilu.Web.Utils;
using wojilu.Common.Spider.Domain;

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
            if (this._template.IsSavePic == 1 ) {
                page = processPic( page, this._template.SiteUrl );
            }

            return page;
        }
        
        protected string getDetailPageBody( string detailUrl, SpiderTemplate template, StringBuilder sb ) {

            try {

                sb.AppendLine( "抓取详细页..."+ detailUrl );

                String page;
                if (strUtil.HasText( template.DetailEncoding ))
                    page = PageLoader.Download( detailUrl, SpiderConfig.UserAgent, template.DetailEncoding );
                else
                    page = PageLoader.Download( detailUrl, SpiderConfig.UserAgent, "" );

                template.SiteUrl = new UrlInfo( detailUrl ).SiteUrl;

                if (strUtil.IsNullOrEmpty( page )) {
                    logInfo( "error=原始页面没有内容:"+detailUrl, detailUrl, template, sb );
                }

                return page;

            }
            catch (Exception ex) {
                logInfo( "error=抓取" + detailUrl + "发生错误：" + ex.Message, detailUrl, template, sb );
                return null;
            }
        }



        protected string getMatchedBody( string page, SpiderTemplate s, StringBuilder sb ) {
            Match match = Regex.Match( page, s.GetDetailPattern(), RegexOptions.Singleline );
            if (match == null || !match.Success || string.IsNullOrEmpty( match.Value )) {
                logInfo( "error=没有匹配的页面内容:"+_url, this._url, s, sb );
                return null;
            }

            page = match.Groups[1].Value;

            String fpage = HtmlFilter.Filter( page ); // 过滤广告

            return fpage;
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
