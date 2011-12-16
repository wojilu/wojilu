/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Serialization;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.UI;

namespace wojilu.Web.Controller.Shop.Admin {


    [App( typeof( ShopApp ) )]
    public partial class ShopController : ControllerBase {

        public IShopSectionService sectionService { get; set; }
        public IShopSectionTemplateService templatelService { get; set; }
        public IShopItemService postService { get; set; }
        public IShopCustomTemplateService ctService { get; set; }

        public ShopController() {
            sectionService = new ShopSectionService();
            templatelService = new ShopSectionTemplateService();
            postService = new ShopItemService();
            ctService = new ShopCustomTemplateService();

            HideLayout( typeof( wojilu.Web.Controller.Shop.LayoutController ) );
        }

        //public void Menu() {

        //    ShopApp app = ctx.app.obj as ShopApp;

        //    set( "allPostsLink", to( new ItemController().List ) );
        //    set( "trashPostsLink", to( new ItemController().Trash ) );
        //    set( "settingLink", to( new SettingController().Index ) );
        //    set( "defaultLink", to( new ShopController().Index ) );
        //    set( "commentLink", to( new CommentController().AdminList ) );

        //    if (app.GetSettingsObj().EnableSubmit == 1) {
        //        String slnk = string.Format( "<td class=\"otherTab\" style=\"width:15%\"><a href=\"{0}\"><img src=\"{1}\" /> {2}</a></td>",
        //            to( new SubmitSettingController().List ),
        //            strUtil.Join( sys.Path.Img, "user.gif" ),
        //            "投递员管理" );
        //        set( "submitterLink", slnk );
        //    }
        //    else {
        //        set( "submitterLink", "" );
        //    }

        //    String appStyle = app.Style == null ? "" : app.Style.Replace( "display:none;", "" );
        //    String skinStyle = app.SkinStyle == null ? "" : app.SkinStyle.Replace( "display:none;", "" );

        //    set( "app.Style", appStyle );
        //    set( "app.SkinStyle", skinStyle );

        //    StringBuilder sb = new StringBuilder();
        //    if (ctx.owner.obj is Site) {
        //        //sb.AppendLine( "#adminPortalContainer {width: 1000px;}" );
        //        //sb.AppendLine( "#portalAdminNav,#portalAdminNavWrap,.tabMain {width:1030px;}" );
        //        sb.AppendLine( "#toggleSidebar { display:none;}" );
        //    }
        //    set( "portalWrapCss", sb );

        //}

        public void Index() {
            set( "indexLink", to( new ItemController().List ) );
        }

        public void Home() {

            //load( "menu", Menu );

            bindCmd();

            bindCss();

            ShopApp article = ctx.app.obj as ShopApp;
            List<ShopSection> sections = sectionService.GetByApp( ctx.app.Id );

            bindRowList( article, sections );
        }

        private void bindCss() {

            ShopApp app = ctx.app.obj as ShopApp;

            String appStyle = app.Style == null ? "" : app.Style.Replace( "display:none;", "" );
            String skinStyle = app.SkinStyle == null ? "" : app.SkinStyle.Replace( "display:none;", "" );

            set( "app.Style", appStyle );
            set( "app.SkinStyle", skinStyle );

            //StringBuilder sb = new StringBuilder();
            //if (ctx.owner.obj is Site) {
            //    //sb.AppendLine( "#adminPortalContainer {width: 1000px;}" );
            //    //sb.AppendLine( "#portalAdminNav,#portalAdminNavWrap,.tabMain {width:1030px;}" );
            //    //sb.AppendLine( "#toggleSidebar { display:none;}" );

            //    sb.AppendLine( "#adminPortalContainer {width: 100%;}" );
            //    sb.AppendLine( "#portalAdminNav,#portalAdminNavWrap,.tabMain {width:100%;}" );
            //    sb.AppendLine( "#toggleSidebar { display:none;}" );

            //}
            //set( "portalWrapCss", sb );

        }

        public void ConfirmAddRow( int columnCount ) {
            target( AddRow, columnCount );
            if (columnCount <= 0 || columnCount > 6) {
                echo( "column count error" );
                return;
            }
            String row = alang( "addRow" + columnCount );
            set( "rowText", row );
        }

        [HttpPost, DbTransaction]
        public void AddRow( int columnCount ) {

            ShopApp app = ctx.app.obj as ShopApp;

            app.Layout = app.Layout + "/" + columnCount;

            int row = app.RowList.Length;
            String newStyle = getStyle( row, columnCount );
            String mergedStyle = CssFormUtil.mergeStyle( app.Style, newStyle );

            app.Style = mergedStyle;

            db.update( app, new string[] { "Layout", "Style" } );

            echoToParent( lang( "opok" ) );
        }

        private String getStyle( int row, int columnCount ) {

            String style = Environment.NewLine;
            int width = 100 / columnCount - 2;
            for (int i = 1; i < columnCount + 1; i++) {

                String margin = "margin-top:5px; margin-right:5px; margin-bottom:5px; margin-left:5px;";
                if (i == 1) margin = "margin-top:5px; margin-right:5px; margin-bottom:5px; margin-left:10px;";

                style += "#row" + row + "_column" + i + " { width:" + width + "%; " + margin + " }" + Environment.NewLine;
            }
            return style;
        }


        [HttpDelete, DbTransaction]
        public void DeleteRow( int rowId ) {
            ShopApp article = ctx.app.obj as ShopApp;
            if (sectionService.Count( ctx.app.Id, rowId ) > 0) {
                echoRedirect( alang( "exRemoveSectionFirst" ), to( Index ) );
                return;
            }
            article.DeleteRow( rowId, ctx.app.Id );
            echoRedirect( lang( "opok" ), to( Index ) );
        }


        [HttpPost, DbTransaction]
        public void SaveLayout() {
            IList sections = sectionService.GetByApp( ctx.app.Id );
            string[] strArray = ctx.Get( "layout" ).Split( new char[] { '/' } );
            int orderId = 0;
            foreach (String strOne in strArray) {
                string[] arrItem = strOne.Split( new char[] { '_' } );
                int rowId = cvt.ToInt( strUtil.TrimStart( arrItem[0], "row" ) );
                int columnId = cvt.ToInt( strUtil.TrimStart( arrItem[1], "column" ) );
                int sectionId = cvt.ToInt( strUtil.TrimStart( arrItem[2], "section" ) );
                updateRowColumn( sections, rowId, columnId, sectionId, orderId );
                orderId++;
            }

            echoAjaxOk();
        }


        [HttpPost, DbTransaction]
        public void SaveResize() {

            String colIds = ctx.Post( "colIds" );
            String widths = ctx.Post( "widths" );
            if (strUtil.IsNullOrEmpty( colIds ) || strUtil.IsNullOrEmpty( widths )) return;

            string[] arrIds = colIds.Split( ',' );
            string[] arrWidth = widths.Split( ',' );
            if (isColumnValid( arrIds ) == false) return;
            if (isWidthValid( arrWidth ) == false) return;

            ShopApp app = ctx.app.obj as ShopApp;
            Dictionary<string, Dictionary<string, string>> dic = Css.FromAndFill( app.Style );

            saveColumnOne( arrIds, arrWidth, dic, 0 );
            saveColumnOne( arrIds, arrWidth, dic, 1 );

            app.Style = Css.To( dic );
            db.update( app, "Style" );

            echoAjaxOk();
        }

        private Boolean isColumnValid( string[] arrIds ) {
            if (arrIds.Length != 2) return false;
            foreach (String id in arrIds) {
                string[] arrPair = id.Split( '_' );
                if (arrPair.Length != 2) return false;

                if (arrPair[0].StartsWith( "row" ) == false) return false;
                if (cvt.IsInt( strUtil.TrimStart( arrPair[0], "row" ) ) == false) return false;

                if (arrPair[1].StartsWith( "column" ) == false) return false;
                if (cvt.IsInt( strUtil.TrimStart( arrPair[1], "column" ) ) == false) return false;

            }
            return true;
        }

        private Boolean isWidthValid( string[] arrWidth ) {
            if (arrWidth.Length != 2) return false;
            foreach (String w in arrWidth) {
                if (w.EndsWith( "%" ) == false) return false;
                String tw = w.TrimEnd( '%' );
                if (cvt.IsInt( tw ) == false) return false;
                if (cvt.ToInt( tw ) > 100) return false;
            }
            return true;
        }

        private static void saveColumnOne( string[] arrIds, string[] arrWidth, Dictionary<string, Dictionary<string, string>> dic, int i ) {
            String key = "#" + arrIds[i];
            if (dic.ContainsKey( key )) {
                dic[key]["margin-left"] = "7px";
                dic[key]["margin-right"] = "1px";
                dic[key]["width"] = arrWidth[i];
            }
            else {
                Dictionary<string, string> cc = new Dictionary<string, string>();
                cc["margin-left"] = "7px";
                cc["margin-right"] = "1px";
                cc["width"] = arrWidth[i];
                dic[key] = cc;
            }
        }

        public void SetStyle() {
            ShopApp app = ctx.app.obj as ShopApp;
            set( "styleContent", app.SkinStyle );
            target( SaveStyle );
        }

        [HttpPost, DbTransaction]
        public void SaveStyle() {
            ShopApp app = ctx.app.obj as ShopApp;
            app.SkinStyle = ctx.Post( "Style" );
            app.update( "SkinStyle" );
            echoToParent( lang( "opok" ) );
        }

        private static void updateRowColumn( IList sectionList, int rowId, int columnId, int sectionId, int orderId ) {
            foreach (ShopSection section in sectionList) {
                if ((section.Id == sectionId) && (((section.RowId != rowId) || (section.ColumnId != columnId)) || (section.OrderId != orderId))) {
                    section.RowId = rowId;
                    section.ColumnId = columnId;
                    section.OrderId = orderId;
                    db.update( section );
                }
            }
        }

        // 获取当前app首页的数据、排版布局、样式，存储为皮肤
        public void Snapshot() {

            ShopApp app = ctx.app.obj as ShopApp;

            PortalMockSkin s = new PortalMockSkin();
            s.Style = app.Style; // 样式
            s.Layout = app.Layout; // 排版布局
            s.Sections = new List<PortalMockSection>();

            List<ShopSection> sections = sectionService.GetByApp( app.Id );
            foreach (ShopSection section in sections) {
                PortalMockSection ps = new PortalMockSection {
                    RowId = section.RowId,
                    ColumnId = section.ColumnId,
                    SectionType = section.SectionType,
                    Title = section.Title,
                    CustomTemplateId = section.CustomTemplateId,
                    ServiceId = section.ServiceId,
                    ServiceParams = section.ServiceParams,
                    TemplateId = section.TemplateId
                };

                ps.Posts = populatePost( postService.GetBySection( ctx.app.Id, section.Id, 20 ) );
                s.Sections.Add( ps );
            }

            String json = JsonString.ConvertObject( s, true );
            file.Write( PathHelper.Map( "/content.json" ), json );


        }

        public void ShowSnapshot() {
            Dictionary<string, object> dic = JsonParser.Parse( file.Read( PathHelper.Map( "/content.json" ) ) ) as Dictionary<string, object>;
            String str = dic["Style"].ToString();
            str += dic["Layout"] + "<br/>";
        }

        private List<PortalMockPost> populatePost( List<ShopItem> list ) {
            List<PortalMockPost> ps = new List<PortalMockPost>();

            foreach (ShopItem p in list) {
                ps.Add( new PortalMockPost {
                    CategoryId = p.CategoryId,
                    OrderId = p.OrderId,
                    SectionId = p.PageSection.Id,
                    TypeName = p.TypeName,
                    Title = p.Title,
                    TitleHome = p.TitleHome,
                    Style = p.Style,
                    Author = p.Author,
                    Width = p.Width,
                    Height = p.Height,
                    Content = p.Content,
                    Summary = p.Summary,
                    ImgLink = p.ImgLink,
                    Hits = p.Hits,
                    Replies = p.Replies,
                    HasImgList = p.HasImgList
                } );
            }

            return ps;
        }
    }
    public class PortalMockSection {
        public int Id { get; set; }
        public int RowId { get; set; }
        public int ColumnId { get; set; }
        public String SectionType { get; set; }
        public String Title { get; set; }

        public int CustomTemplateId { get; set; }
        public int ServiceId { get; set; }
        public int TemplateId { get; set; }
        public String ServiceParams { get; set; }


        public List<PortalMockPost> Posts { get; set; }
    }
    public class PortalMockPost {

        public int CategoryId { get; set; }
        public int OrderId { get; set; }
        public int SectionId { get; set; }
        public String TypeName { get; set; }

        public String Title { get; set; }
        public String TitleHome { get; set; }

        public String Style { get; set; }
        public String Author { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public String Content { get; set; }
        public String Summary { get; set; }
        public String ImgLink { get; set; }

        public int Hits { get; set; }
        public int Replies { get; set; }

        public int HasImgList { get; set; }
    }


    public class PortalMockSkin {
        public String Style { get; set; }
        public String Layout { get; set; }
        public List<PortalMockSection> Sections { get; set; }

    }





}

