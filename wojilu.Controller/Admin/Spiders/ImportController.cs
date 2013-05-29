/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Spider.Domain;
using wojilu.Common.Spider.Interface;
using wojilu.Common.Spider.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

namespace wojilu.Web.Controller.Admin.Spiders {

    public class ImportController : ControllerBase {

        public ISpiderTemplateService templateService { get; set; }
        public ISportImportService importService { get; set; }
        public ISpiderTool spiderTool { get; set; }
        public IMemberAppService appService { get; set; }
        public IUserService userService { get; set; }

        public ImportController() {
            templateService = new SpiderTemplateService();
            importService = new SportImportService();
            spiderTool = new SpiderTool();
            appService = new SiteAppService();
            userService = new UserService();
        }

        public void List() {

            set( "addUrl", to( Add, 0 ) );
            set( "sortAction", to( SaveSort ) );

            List<SpiderTemplate> dataSrc = templateService.GetAll();
            List<SpiderImport> list = importService.GetAll();

            IBlock block = getBlock( "list" );
            foreach (SpiderImport it in list) {
                block.Set( "s.Id", it.Id );
                block.Set( "s.Name", it.Name );
                block.Set( "s.DeleteUrl", to( Delete, it.Id ) );
                block.Set( "s.ShowUrl", to( Show, it.Id ) );
                block.Set( "s.EditUrl", to( Add, it.Id ) );

                block.Set( "s.RefreshUrl", to( DoRefresh, it.Id ) );

                String approveInfo = it.IsApprove == 1 ? "<span class=\"approveInfo\">需审核</span>" : "";
                block.Set( "s.ApproveInfo", approveInfo );

                String desc = getImportDescription( it, dataSrc );
                block.Set( "s.Description", desc );

                String cmd;
                if (it.IsDelete == 1) {
                    cmd = string.Format( "<span class=\"cmdStart\" href=\"{0}\">启动</span>", to( Start, it.Id ) );
                    block.Set( "rowStatus", "stopped" );
                }
                else {
                    cmd = string.Format( "<span class=\"cmdStop\" href=\"{0}\">暂停</span>", to( Stop, it.Id ) );
                }
                block.Set( "s.Cmd", cmd );


                block.Next();
            }

            set( "page", "" );
        }

        private string getImportDescription( SpiderImport it, List<SpiderTemplate> dataSrc ) {

            StringBuilder sb = getDataSrcInfo( it, dataSrc );
            sb.Append( " --> " );
            sb.Append( getTargetInfo( it ) );

            return sb.ToString();
        }

        private StringBuilder getDataSrcInfo( SpiderImport it, List<SpiderTemplate> dataSrc ) {
            StringBuilder sb = new StringBuilder();
            int[] arrSrc = cvt.ToIntArray( it.DataSourceIds );
            for (int i = 0; i < arrSrc.Length; i++) {
                sb.Append( getTemplateName( arrSrc[i], dataSrc ) );
                if (i < arrSrc.Length - 1) sb.Append( "," );
            }

            return sb;
        }

        private StringBuilder getTargetInfo( SpiderImport it ) {
            StringBuilder sb = new StringBuilder();
            List<ContentSection> sections = ContentSection.find( "Id in (" + it.SectionIds + ")" ).list();
            for (int i = 0; i < sections.Count; i++) {
                sb.Append( sections[i].Title );
                if (i < sections.Count - 1) sb.Append( "," );
            }
            return sb;
        }

        private String getTemplateName( int id, List<SpiderTemplate> dataSrc ) {
            foreach (SpiderTemplate t in dataSrc) {
                if (t.Id == id) {
                    return t.SiteName;
                }
            }
            return "";
        }


        [HttpPost, DbTransaction]
        public virtual void SaveSort() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            SpiderImport s = importService.GetById( id );
            List<SpiderImport> list = importService.GetAll();

            if (cmd == "up") {

                new SortUtil<SpiderImport>( s, list ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {

                new SortUtil<SpiderImport>( s, list ).MoveDown();
                echoJsonOk();
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }
        }

        public void Show( int id ) {
            SpiderImport item = importService.GetById( id );

            List<SpiderTemplate> dataSrc = templateService.GetAll();

            set( "item.ApproveInfo", item.IsApprove == 1 ? "需要审核" : "不需要" );
            bind( "item", item );

            set( "item.DataSourceInfo", getDataSrcInfo( item, dataSrc ) );
            set( "item.TargetInfo", getTargetInfo( item ) );

            set( "editUrl", to( Add, id ) );

        }

        public void DoRefresh( int id ) {

            set( "processLink", to( Process, id ) );

            StringBuilder sb = LogCacher.GetNewImportLog( "log" + ctx.viewer.Id );
            ImportState ts = new ImportState();
            ts.TemplateId = id;
            ts.Log = sb;

            new Thread( ImportUtil.BeginImport ).Start( ts );
        }

        [HttpPost]
        public void Process( int id ) {
            StringBuilder sb = LogCacher.GetImportLog( "log" + ctx.viewer.Id );
            echoText( sb.ToString() );
        }

        [HttpPost, DbTransaction]
        public void Start( int id ) {
            SpiderImport item = importService.GetById( id );
            item.IsDelete = 0;
            item.update( "IsDelete" );
            echoAjaxOk();
        }

        [HttpPost, DbTransaction]
        public void Stop( int id ) {
            SpiderImport item = importService.GetById( id );
            item.IsDelete = 1;
            item.update( "IsDelete" );
            echoAjaxOk();
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {
            SpiderImport item = importService.GetById( id );
            item.delete();
            redirect( List );
        }

        public void Add( int id ) {

            if (id > 0) {
                SpiderImport item = importService.GetById( id );
                if (item == null) {
                    echoRedirect( lang( "exDataNotFound" ) );
                    return;
                }
                set( "itemJson", Json.ToString( new ImportJson( item ) ) );
            }
            else {
                set( "itemJson", "{Id:0}" );
            }

            set( "step3Action", to( Save ) );
            set( "returnUrl", to( List ) );

            List<SpiderTemplate> list = templateService.GetAll();
            checkboxList( "dataSrc", list, "SiteName=Id", null );

            //dataTarget
            List<ContentApp> apps = ContentApp.find( "OwnerType=:otype" ).set( "otype", typeof( Site ).FullName ).list();
            IBlock block = getBlock( "apps" );
            foreach (ContentApp app in apps) {
                block.Set( "appId", app.Id );

                IMemberApp ma = appService.GetByApp( app );
                if (ma == null) continue;

                block.Set( "appName", ma.Name );
                List<ContentSection> sections = ContentSection.find( "AppId=" + app.Id ).list();
                block.Set( "dataTarget", Html.CheckBoxList( sections, "dataTarget", "Title", "Id", null ) );
                block.Next();
            }
        }

        [HttpPost, DbTransaction]
        public void Save() {

            int id = ctx.PostInt( "Id" );
            int isApprove = ctx.PostInt( "isApprove" );
            String name = ctx.Post( "name" );
            String srcIds = ctx.PostIdList( "srcIds" );
            String targetIds = ctx.PostIdList( "targetIds" );
            String userName = ctx.Post( "userName" );

            if (strUtil.IsNullOrEmpty( name )) errors.Add( "请填写名称" );
            if (strUtil.IsNullOrEmpty( srcIds )) errors.Add( "请选择数据源" );
            if (strUtil.IsNullOrEmpty( targetIds )) errors.Add( "请选择目标区块" );

            User user = userService.GetByName( userName );
            if (user == null) errors.Add( "用户不存在" );

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            SpiderImport it = id > 0 ? importService.GetById( id ) : new SpiderImport();

            it.Name = name;
            it.DataSourceIds = srcIds;
            it.SectionIds = targetIds;
            it.IsApprove = isApprove;
            it.Creator = user;

            if (id > 0) {
                it.update();
            }
            else {
                it.insert();
            }

            echoJsonMsg( "操作成功", true, null );
        }

    }
}
