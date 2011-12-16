/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.SOA;
using wojilu.Web.Mvc;

using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Serialization;
using wojilu.Web.Controller.Shop.Utils;

namespace wojilu.Web.Controller.Shop.Admin {

    public partial class ShopController : ControllerBase {

        private void bindCmd() {
            
            ShopApp app = ctx.app.obj as ShopApp;
            ShopSkin s = ShopSkin.findById(app.SkinId);
            set( "skinPath", strUtil.Join( sys.Path.Skin, s.StylePath ) );

            set( "skinLink", to( new SkinController().Index ) );
            set( "addRow1Url", to( new ShopController().ConfirmAddRow, 1 ) );
            set( "addRow2Url", to( new ShopController().ConfirmAddRow, 2 ) );
            set( "addRow3Url", to( new ShopController().ConfirmAddRow, 3 ) );
            set( "addRow4Url", to( new ShopController().ConfirmAddRow, 4 ) );
            set( "addRow5Url", to( new ShopController().ConfirmAddRow, 5 ) );
            set( "addRow6Url", to( new ShopController().ConfirmAddRow, 6 ) );

            set( "setStyleUrl", to( new ShopController().SetStyle ) );
            set( "layoutSaveUrl", to( new ShopController().SaveLayout ) );

            set( "resizeLink", to( new ShopController().SaveResize ) );
        }

        private void bindRowList( ShopApp article, List<ShopSection> sections ) {
            IBlock block = getBlock( "rowList" );
            string[] rowList = article.RowList;
            for (int i = 1; i < (rowList.Length + 1); i++) {

                int columnCount = cvt.ToInt( rowList[i - 1] );
                if (columnCount <= 0) continue;

                block.Set( "row.No", string.Format( alang( "rowNo" ), i ) );

                block.Set( "row.DeleteUrl", to( DeleteRow, i ) );
                block.Set( "row.ColumnCount", columnCount );
                block.Set( "row.Index", i );
                block.Set( "row.EditUILink", to( new ShopSectionController().EditRowUI, i ) );


                IBlock columnBlock = block.GetBlock( "columnList" );
                bindColumnList( sections, i, columnCount, columnBlock );
                block.Next();

            }
        }

        private void bindColumnList( List<ShopSection> sectionList, int iRow, int columnCount, IBlock columnBlock ) {
            for (int i = 1; i < (columnCount + 1); i++) {
                columnBlock.Set( "App.ImgPath", sys.Path.Img );
                columnBlock.Set( "column.Id", string.Concat( new object[] { "row", iRow, "_column", i } ) );

                String ctitle = alang( "columnNo" );
                columnBlock.Set( "column.Name", string.Format( ctitle, i ) );
                columnBlock.Set( "column.Index", i );

                int rowColumnId = cvt.ToInt( iRow + "" + i );

                String addUrl = Link.To( new ShopSectionController().Add, rowColumnId );
                String addAutoUrl = Link.To( new ShopSectionController().AddAuto, rowColumnId );
                String addFeed = Link.To( new ShopSectionController().AddFeed, rowColumnId );
                String editUILink = to( new ShopSectionController().EditUI, rowColumnId );

                columnBlock.Set( "column.AddModuleUrl", addUrl );
                columnBlock.Set( "column.AddAutoSection", addAutoUrl );
                columnBlock.Set( "column.AddFeed", addFeed );

                columnBlock.Set( "column.EditUILink", editUILink );

                List<ShopSection> sections = sectionService.GetByRowColumn( sectionList, iRow, i );
                IBlock sectionBlock = columnBlock.GetBlock( "sectionList" );
                bindSectionList( sections, sectionBlock );
                columnBlock.Next();
            }
        }

        private void bindSectionList( IList sections, IBlock sectionBlock ) {
            foreach (ShopSection section in sections) {
                sectionBlock.Set( "section.Id", section.Id );
                sectionBlock.Set( "section.Title", section.Title );
                sectionBlock.Set( "section.CombineIds", section.CombineIds );

                sectionBlock.Set( "section.DeleteUrl", Link.To( new ShopSectionController().Delete, section.Id ) );
                sectionBlock.Set( "section.EditUILink", Link.To( new ShopSectionController().EditSectionUI, section.Id ) );
                sectionBlock.Set( "section.EditTitleUILink", Link.To( new ShopSectionController().EditSectionTitleUI, section.Id ) );
                sectionBlock.Set("section.EditContentUILink", Link.To(new ShopSectionController().EditSectionContentUI, section.Id));
                sectionBlock.Set("section.EditEffectLink", Link.To(new ShopSectionController().EditEffect, section.Id));

                //

                bindCombineLink( section, sectionBlock );
                bindRemoveLinks( section, sectionBlock );

                String sectionContent = getSectionContent( sectionBlock, section );
                sectionBlock.Set("section.Content", sectionContent);
                sectionBlock.Next();
            }
        }

        private void bindCombineLink( ShopSection section, IBlock sectionBlock ) {
            IBlock block = sectionBlock.GetBlock( "combineSection" );
            if (cvt.ToIntArray( section.CombineIds ).Length == 0) {
                block.Set( "section.CombineUrl", Link.To( new ShopSectionController().Combine, section.Id ) );
                block.Next();
            }
        }

        private void bindRemoveLinks( ShopSection section, IBlock sectionBlock ) {
            IBlock block = sectionBlock.GetBlock( "removeSections" );
            int[] arrIds = cvt.ToIntArray( section.CombineIds );
            if (arrIds.Length <= 0) return;

            for (int i = 0; i < arrIds.Length; i++) {
                ShopSection target = sectionService.GetById( arrIds[i], ctx.app.Id );
                if (target == null) continue;
                block.Set( "r.Name", target.Title );
                block.Set( "r.RemoveLink", Link.To( new ShopSectionController().RemoveSection, section.Id ) + "?targetSection=" + target.Id );
                block.Next();
            }
        }

        private String getSectionContent( IBlock sectionBlock, ShopSection section ) {
            String sectionContent;
            if (section.ServiceId <= 0)
                sectionContent = bindData( sectionBlock, section );
            else
                sectionContent = "<div class=\"binderSectionBody\">" + bindAutoData(sectionBlock, section) + "</div>";
            return sectionContent;
        }

        private String bindAutoData( IBlock sectionBlock, ShopSection section ) {

            IList setttingLinks = getAutoBinderSettingLinks( section );
            bindSettingLink( sectionBlock, setttingLinks );

            Dictionary<string, string> presult = getDefaultValue( section );

            IList data = ServiceContext.GetData( section.ServiceId, section.GetServiceParamValues(), presult );
            if (section.TemplateId <= 0) return getJsonResult( section, data );


            ShopSectionTemplate tpl = templatelService.GetById( section.TemplateId );
            Template currentView = utils.getTemplateByFileName( BinderUtils.GetBinderTemplatePath( tpl ) );
            ISectionBinder binder = BinderUtils.GetBinder( tpl, ctx, currentView );
            binder.Bind( section, data );

            ControllerBase cb2 = binder as ControllerBase;
            return cb2.utils.getActionResult();
        }

        private String getJsonResult( ShopSection section, IList data ) {

            String jsonStr = JsonString.ConvertList( data );
            String scriptData = string.Format( "	<script>var sectionData{0} = {1};</script>", section.Id, jsonStr );
            if (section.CustomTemplateId <= 0)
                return scriptData;

            ShopCustomTemplate ct = ctService.GetById(section.CustomTemplateId, ctx.owner.Id);
            if (ct == null) return scriptData;

            return scriptData + ct.Content;
        }

        private IList getAutoBinderSettingLinks( ShopSection section ) {

            IList setttingLinks = new ArrayList();

            PageSettingLink slink = new PageSettingLink();
            slink.Name = lang( "editSetting" );
            slink.Url = Link.To( new SectionSettingController().EditBinder, section.Id );
            setttingLinks.Add( slink );

            PageSettingLink lnktmp = new PageSettingLink();
            lnktmp.Name = alang( "editTemplate" );
            lnktmp.Url = to( new TemplateCustomController().EditBinder, section.Id );
            setttingLinks.Add( lnktmp );

            return setttingLinks;
        }

        private Dictionary<string, string> getDefaultValue( ShopSection section ) {
            Service service = ServiceContext.Get( section.ServiceId );

            Dictionary<string, string> pd = service.GetParamDefault();
            Dictionary<string, string> presult = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in pd) {
                if (pair.Key.Equals( "ownerId" ))
                    presult.Add( pair.Key, ctx.owner.obj.Id.ToString() );
                else if (pair.Key.Equals( "viewerId" ))
                    presult.Add( pair.Key, ctx.viewer.Id.ToString() );
                else
                    presult.Add( pair.Key, pair.Key );
            }
            return presult;
        }

        private String bindData( IBlock sectionBlock, ShopSection articleSection ) {
            IPageSection section = BinderUtils.GetPageSectionAdmin( articleSection, ctx, "AdminSectionShow" );
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

