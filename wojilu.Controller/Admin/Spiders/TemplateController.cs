/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.AppBase;
using wojilu.Common.Spider.Domain;
using wojilu.Common.Spider.Interface;
using wojilu.Common.Spider.Service;

namespace wojilu.Web.Controller.Admin.Spiders {

    public class TemplateController : ControllerBase {

        public ISpiderTemplateService templateService { get; set; }
        public ISpiderTool spiderTool { get; set; }

        public TemplateController() {
            templateService = new SpiderTemplateService();
            spiderTool = new SpiderTool();
        }

        public void List() {


            set( "addUrl", to( SetTemplate, 0 ) );
            set( "sortAction", to( SaveSort ) );

            List<SpiderTemplate> list = templateService.GetAll();

            IBlock block = getBlock( "list" );

            foreach (SpiderTemplate s in list) {

                block.Set( "s.Id", s.Id );
                block.Set( "s.SiteName", s.SiteName );
                block.Set( "s.ListUrl", s.ListUrl );
                block.Set( "s.EditUrl", to( SetTemplate, s.Id ) );
                block.Set( "s.DeleteUrl", to( Delete, s.Id ) );
                block.Set( "s.RefreshUrl", to( DoRefresh, s.Id ) );

                String cmd;
                if (s.IsDelete == 1) {
                    cmd = string.Format( "<span class=\"cmdStart\" href=\"{0}\">启动</span>", to( Start, s.Id ) );
                    block.Set( "rowStatus", "stopped" );
                }
                else {
                    cmd = string.Format( "<span class=\"cmdStop\" href=\"{0}\">暂停</span>", to( Stop, s.Id ) );
                }
                block.Set( "s.Cmd", cmd );

                block.Next();
            }
            set( "page", "" );
        }


        [HttpPost, DbTransaction]
        public virtual void SaveSort() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            SpiderTemplate s = templateService.GetById( id );
            List<SpiderTemplate> list = templateService.GetAll();

            if (cmd == "up") {

                new SortUtil<SpiderTemplate>( s, list ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {

                new SortUtil<SpiderTemplate>( s, list ).MoveDown();
                echoJsonOk();
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }
        }

        public void DoRefresh( int id ) {

            if (id <= 0) {
                echoRedirect( "请先选择模板" );
                return;
            }

            set( "processLink", to( Process, id ) );

            SpiderTemplate s = templateService.GetById( id );
            TemplateAndLog tl = new TemplateAndLog();
            tl.Template = s;

            StringBuilder sb = LogCacher.GetNewSpiderLog( "log" + ctx.viewer.Id );
            tl.log = sb;

            new Thread( beginRefresh ).Start( tl );
        }

        private void beginRefresh( object obj ) {
            TemplateAndLog tl = obj as TemplateAndLog;
            spiderTool.DownloadPage( tl.Template, tl.log, new int[] { 100, 100 } );
        }

        [HttpPost]
        public void Process( int id ) {
            StringBuilder sb = LogCacher.GetSpiderLog( "log" + ctx.viewer.Id );
            echoText( reverseText( sb.ToString() ) );
        }

        private string reverseText( string log ) {
            StringBuilder sb = new StringBuilder();
            String[] arr = log.Split( '\n' );

            for (int i = arr.Length - 1; i >= 0; i--) {
                sb.AppendLine( arr[i].Trim() );
            }
            return sb.ToString();
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {
            templateService.Delete( id );
            redirect( List );
        }

        [HttpPost, DbTransaction]
        public void Start( int id ) {
            templateService.Start( id );
            echoAjaxOk();
        }

        [HttpPost, DbTransaction]
        public void Stop( int id ) {
            templateService.Stop( id );
            echoAjaxOk();
        }

        public void SetTemplate( int id ) {

            target( GetList );

            if (id > 0) {
                SpiderTemplate s = templateService.GetById( id );
                // 感谢 sgzwiz (http://www.wojilu.com/sgzwiz) 贡献此处代码
                set( "objTemplate", Json.ToString( s ).Replace( "<", "&lt;" ).Replace( ">", "&gt;" ) );
            }
            else {
                set( "objTemplate", "{Id:0}" );
            }

            set( "detailAction", to( GetDetail ) );
            set( "saveLink", to( Save ) );

            set( "listPattern", SpiderConfig.ListLinkPattern );
            set( "returnUrl", to( List ) );
        }


        [HttpPost]
        public void GetList() {

            SpiderTemplate s = ctx.PostValue<SpiderTemplate>();

            //String beginCode = ctx.PostHtmlAll( "listBeginCode" );
            //String endCode = ctx.PostHtmlAll( "listEndCode" );
            //s.ListBodyPattern = beginCode + ".+?" + endCode;

            String listBodyPattern = ctx.PostHtmlAll( "ListBodyPattern" );
            String ListPattern = ctx.PostHtmlAll( "ListPattern" );

            s.ListBodyPattern = listBodyPattern;

            if (strUtil.IsNullOrEmpty( ListPattern )) {
                ListPattern = SpiderConfig.ListLinkPattern;
            }
            s.ListPattern = ListPattern;

            String listEncoding = ctx.Post( "listEncoding" );
            s.ListEncoding = listEncoding;

            StringBuilder log = new StringBuilder();
            List<DetailLink> list = SpiderTool.GetDataList( s, log );

            if (list.Count == 0) {

                Dictionary<String, Object> dic = new Dictionary<String, Object>();
                dic.Add( "IsValid", false );
                dic.Add( "listUrl", s.ListUrl );
                dic.Add( "patternBody", s.ListBodyPattern );
                dic.Add( "patternLinks", s.ListPattern );

                echoJson( dic );
            }
            else {
                renderJson( list );
            }
        }

        private void renderJson( List<DetailLink> list ) {

            List<Dictionary<String, String>> ls = new List<Dictionary<String, String>>();
            foreach (DetailLink item in list) {
                Dictionary<String, String> dic = new Dictionary<String, String>();
                dic["Title"] = item.Title;
                dic["Url"] = item.Url;
                ls.Add( dic );
            }

            Dictionary<String, Object> result = new Dictionary<String, Object>();
            result.Add( "IsValid", true );
            result.Add( "List", ls );

            echoJson( result );
        }

        private static readonly ILog logger = LogManager.GetLogger( typeof( TemplateController ) );

        [HttpPost]
        public void GetDetail() {

            String newsUrl = ctx.Post( "detailUrl" );

            SpiderTemplate s = new SpiderTemplate();

            //String detailBeginCode = ctx.PostHtmlAll( "detailBeginCode" );
            //String detailEndCode = ctx.PostHtmlAll( "detailEndCode" );
            //String DetailPattern = detailBeginCode + "(.+?)" + detailEndCode;

            String DetailPattern = ctx.PostHtmlAll( "DetailPattern" );
            s.DetailPattern = DetailPattern;

            logger.Info( "DetailPattern=" + s.DetailPattern );

            String detailEncoding = ctx.Post( "detailEncoding" );
            s.DetailEncoding = detailEncoding;

            s.IsSavePic = 0;

            StringBuilder log = new StringBuilder();

            string newsBody = new PagedDetailSpider().GetContent( newsUrl, s, log );

            String strLog = log.ToString();
            if (strLog.IndexOf( "error=" ) >= 0) {
                StringBuilder sblog = new StringBuilder();
                sblog.AppendLine( "detailUrl=" + newsUrl );
                sblog.AppendLine( "detailPattern=" + s.DetailPattern );
                sblog.Append( log );
                echoText( sblog.ToString() );
            }
            else {
                echoText( newsBody );
            }

        }

        [HttpPost, DbTransaction]
        public void Save() {

            int templateId = ctx.PostInt( "tid" );

            String listUrl = ctx.Post( "listUrl" );

            //String beginCode = ctx.PostHtmlAll( "listBeginCode" );
            //String endCode = ctx.PostHtmlAll( "listEndCode" );
            String listBodyPattern = ctx.PostHtmlAll( "ListBodyPattern" );
            String ListPattern = ctx.PostHtmlAll( "ListPattern" );

            //String detailBeginCode = ctx.PostHtmlAll( "detailBeginCode" );
            //String detailEndCode = ctx.PostHtmlAll( "detailEndCode" );
            String DetailPattern = ctx.PostHtmlAll( "DetailPattern" );

            if (strUtil.IsNullOrEmpty( listUrl )) errors.Add( "请填写列表页的网址" );
            //if (strUtil.IsNullOrEmpty( beginCode )) errors.Add( "请填写列表页开始代码" );
            //if (strUtil.IsNullOrEmpty( endCode )) errors.Add( "请填写列表页结束代码" );
            //if (strUtil.IsNullOrEmpty( detailBeginCode )) errors.Add( "请填写详细页开始代码" );
            //if (strUtil.IsNullOrEmpty( detailEndCode )) errors.Add( "请填写详细页结束代码" );

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            SpiderTemplate s = new SpiderTemplate();
            if (templateId > 0) s = templateService.GetById( templateId );
            if (s == null) {
                echoError( "采集模板不存在" );
                return;
            }

            s.ListUrl = listUrl;
            //s.ListBodyBegin = beginCode;
            //s.ListBodyEnd = endCode;

            s.ListPattern = ListPattern;
            s.ListBodyPattern = listBodyPattern;

            //s.DetailBegin = detailBeginCode;
            //s.DetailEnd = detailEndCode;
            s.DetailPattern = DetailPattern;
            s.SiteName = ctx.Post( "siteName" );


            if (strUtil.IsNullOrEmpty( s.SiteName )) {
                echoError( "请填写采集名称" );
                return;
            }

            s.ListEncoding = ctx.Post( "listEncoding" );
            s.DetailEncoding = ctx.Post( "detailEncoding" );

            Boolean chkPic = cvt.ToBool( ctx.Post( "checkPic" ) );
            s.IsSavePic = chkPic ? 1 : 0;

            s.DetailClearTag = ctx.Post( "clearTag" );

            if (templateId > 0) {
                templateService.Update( s );
            }
            else {
                templateService.Insert( s );
            }

            echoAjaxOk();

        }



    }

}
