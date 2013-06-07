/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.SOA;
using wojilu.Web.Mvc;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Common.AppBase;
using wojilu.Web.Controller.Content.Utils;

namespace wojilu.Web.Controller.Content.Admin {

    public partial class ContentController : ControllerBase {

        private void bindCmd() {

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSkin s = ContentSkin.findById( app.SkinId );
            set( "skinPath", strUtil.Join( sys.Path.Skin, s.StylePath ) );

            set( "skinLink", to( new SkinController().Index ) );
            set( "addRow1Url", to( new RowController().ConfirmAddRow, 1 ) );
            set( "addRow2Url", to( new RowController().ConfirmAddRow, 2 ) );
            set( "addRow3Url", to( new RowController().ConfirmAddRow, 3 ) );
            set( "addRow4Url", to( new RowController().ConfirmAddRow, 4 ) );
            set( "addRow5Url", to( new RowController().ConfirmAddRow, 5 ) );
            set( "addRow6Url", to( new RowController().ConfirmAddRow, 6 ) );

            set( "setStyleUrl", to( new ContentController().SetStyle ) );
            set( "layoutSaveUrl", to( new ContentController().SaveLayout ) );

            set( "resizeLink", to( new ContentController().SaveResize ) );

            IBlock xblock = getBlock( "export" );
            if (ctx.viewer.IsAdministrator()) {
                xblock.Set( "lnkExport", to( new ExportController().Index ) );
                xblock.Next();
            }
        }

        private void bindRowList( ContentApp app, List<ContentSection> sections ) {
            IBlock block = getBlock( "rowList" );
            string[] rowList = app.RowList;
            for (int i = 1; i < (rowList.Length + 1); i++) {

                int columnCount = cvt.ToInt( rowList[i - 1] );
                if (columnCount <= 0) continue;

                block.Set( "row.No", string.Format( alang( "rowNo" ), i ) );

                block.Set( "row.DeleteUrl", to( new RowController().DeleteRow, i ) );
                block.Set( "row.ColumnCount", columnCount );
                block.Set( "row.Index", i );
                block.Set( "row.EditUILink", to( new ContentSectionController().EditRowUI, i ) );

                if (rowList.Length == 1) {
                    block.Set( "row.RowUpUrl", "#" );
                    block.Set( "row.RowDownUrl", "#" );
                }
                else if (i == 1) {
                    block.Set( "row.RowUpUrl", "#" );
                    block.Set( "row.RowDownUrl", to( new RowController().Move ) + "?action=down&rowId=" + i );
                }
                else if (i == rowList.Length) {
                    block.Set( "row.RowUpUrl", to( new RowController().Move ) + "?action=up&rowId=" + i );
                    block.Set( "row.RowDownUrl", "#" );
                }
                else {
                    block.Set( "row.RowUpUrl", to( new RowController().Move ) + "?action=up&rowId=" + i );
                    block.Set( "row.RowDownUrl", to( new RowController().Move ) + "?action=down&rowId=" + i );
                }


                IBlock columnBlock = block.GetBlock( "columnList" );
                bindColumnList( sections, i, columnCount, columnBlock );
                block.Next();

            }
        }

        private void bindColumnList( List<ContentSection> sectionList, int iRow, int columnCount, IBlock columnBlock ) {
            for (int i = 1; i < (columnCount + 1); i++) {
                columnBlock.Set( "App.ImgPath", sys.Path.Img );
                columnBlock.Set( "column.Id", string.Concat( new object[] { "row", iRow, "_column", i } ) );

                String ctitle = alang( "columnNo" );
                columnBlock.Set( "column.Name", string.Format( ctitle, i ) );
                columnBlock.Set( "column.Index", i );

                int rowColumnId = cvt.ToInt( iRow + "" + i );

                String addUrl = to( new ContentSectionController().Add, rowColumnId );
                String addAutoUrl = to( new ContentSectionController().AddAuto, rowColumnId );
                String addFeed = to( new ContentSectionController().AddFeed, rowColumnId );
                String editUILink = to( new ContentSectionController().EditUI, rowColumnId );

                columnBlock.Set( "column.AddModuleUrl", addUrl );
                columnBlock.Set( "column.AddAutoSection", addAutoUrl );
                columnBlock.Set( "column.AddFeed", addFeed );

                columnBlock.Set( "column.EditUILink", editUILink );

                List<ContentSection> sections = sectionService.GetByRowColumn( sectionList, iRow, i );
                IBlock sectionBlock = columnBlock.GetBlock( "sectionList" );
                bindSectionList( sections, sectionBlock );
                columnBlock.Next();
            }
        }

        private void bindSectionList( IList sections, IBlock sectionBlock ) {
            foreach (ContentSection section in sections) {
                sectionBlock.Set( "section.Id", section.Id );
                sectionBlock.Set( "section.Title", section.Title );
                sectionBlock.Set( "section.CombineIds", section.CombineIds );

                sectionBlock.Set( "section.DeleteUrl", to( new ContentSectionController().Delete, section.Id ) );
                sectionBlock.Set( "section.EditUILink", to( new ContentSectionController().EditSectionUI, section.Id ) );
                sectionBlock.Set( "section.EditTitleUILink", to( new ContentSectionController().EditSectionTitleUI, section.Id ) );
                sectionBlock.Set( "section.EditContentUILink", to( new ContentSectionController().EditSectionContentUI, section.Id ) );
                sectionBlock.Set( "section.EditEffectLink", to( new ContentSectionController().EditEffect, section.Id ) );

                //

                bindCombineLink( section, sectionBlock );
                bindRemoveLinks( section, sectionBlock );

                String sectionContent = getSectionContent( sectionBlock, section );
                sectionBlock.Set( "section.Content", sectionContent );
                sectionBlock.Next();
            }
        }

        private void bindCombineLink( ContentSection section, IBlock sectionBlock ) {
            IBlock block = sectionBlock.GetBlock( "combineSection" );
            if (cvt.ToIntArray( section.CombineIds ).Length == 0) {
                block.Set( "section.CombineUrl", to( new ContentSectionController().Combine, section.Id ) );
                block.Next();
            }
        }

        private void bindRemoveLinks( ContentSection section, IBlock sectionBlock ) {
            IBlock block = sectionBlock.GetBlock( "removeSections" );
            int[] arrIds = cvt.ToIntArray( section.CombineIds );
            if (arrIds.Length <= 0) return;

            for (int i = 0; i < arrIds.Length; i++) {
                ContentSection target = sectionService.GetById( arrIds[i], ctx.app.Id );
                if (target == null) continue;
                block.Set( "r.Name", target.Title );
                block.Set( "r.RemoveLink", to( new ContentSectionController().RemoveSection, section.Id ) + "?targetSection=" + target.Id );
                block.Next();
            }
        }

        private String getSectionContent( IBlock sectionBlock, ContentSection section ) {
            String sectionContent;
            if (section.ServiceId <= 0)
                sectionContent = bindData( sectionBlock, section );
            else
                sectionContent = "<div class=\"binderSectionBody\">" + bindAutoData( sectionBlock, section ) + "</div>";
            return sectionContent;
        }

        private String bindAutoData( IBlock sectionBlock, ContentSection section ) {

            IList setttingLinks = getAutoBinderSettingLinks( section );
            bindSettingLink( sectionBlock, setttingLinks );

            Dictionary<string, string> presult = getDefaultValue( section );

            IList data = ServiceContext.GetData( section.ServiceId, section.GetServiceParamValues(), presult );
            if (section.TemplateId <= 0) return getJsonResult( section, data );


            ISectionBinder binder = BinderUtils.GetBinder( section, ctx );
            binder.Bind( section, data );
            ControllerBase cb2 = binder as ControllerBase;
            return cb2.utils.getActionResult();
        }

        private String getJsonResult( ContentSection section, IList data ) {

            String jsonStr = Json.ToString( data );
            String scriptData = string.Format( "	<script>var sectionData{0} = {1};</script>", section.Id, jsonStr );
            if (section.CustomTemplateId <= 0)
                return scriptData;

            ContentCustomTemplate ct = ctService.GetById( section.CustomTemplateId, ctx.owner.Id );
            if (ct == null) return scriptData;

            return scriptData + ct.Content;
        }

        private IList getAutoBinderSettingLinks( ContentSection section ) {

            IList setttingLinks = new ArrayList();

            PageSettingLink slink = new PageSettingLink();
            slink.Name = lang( "editSetting" );
            slink.Url = to( new SectionSettingController().EditBinder, section.Id );
            setttingLinks.Add( slink );

            PageSettingLink lnktmp = new PageSettingLink();
            lnktmp.Name = alang( "editTemplate" );
            lnktmp.Url = to( new TemplateCustomController().EditBinder, section.Id );
            setttingLinks.Add( lnktmp );

            return setttingLinks;
        }

        private Dictionary<string, string> getDefaultValue( ContentSection section ) {
            Service service = ServiceContext.Get( section.ServiceId );

            Dictionary<string, string> pd = service.GetParamDefault();
            Dictionary<string, string> presult = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in pd) {
                if (pair.Key.Equals( "ownerId" )) {
                    presult.Add( pair.Key, ctx.owner.Id.ToString() );
                }
                else if (pair.Key.Equals( "viewerId" )) {
                    presult.Add( pair.Key, ctx.viewer.Id.ToString() );
                }
                else if (pair.Key.Equals( "appId" )) {
                    presult.Add( pair.Key, ctx.app.Id.ToString() );
                }
                else {
                    presult.Add( pair.Key, pair.Key );
                }
            }
            return presult;
        }

        private String bindData( IBlock sectionBlock, ContentSection articleSection ) {
            IPageAdminSection section = BinderUtils.GetPageSectionAdmin( articleSection, ctx, "AdminSectionShow" );
            ControllerBase controller = section as ControllerBase;
            bindSettingLink( sectionBlock, section.GetSettingLink( articleSection.Id ) );
            section.AdminSectionShow( articleSection.Id );
            return controller.utils.getActionResult();
        }

        private static void bindSettingLink( IBlock sectionBlock, IList setttingLinks ) {
            IBlock block = sectionBlock.GetBlock( "links" );
            foreach (IPageSettingLink link in setttingLinks) {
                block.Set( "settting.Name", link.Name );
                block.Set( "setting.Url", link.Url );
                block.Next();
            }
        }


    }
}

