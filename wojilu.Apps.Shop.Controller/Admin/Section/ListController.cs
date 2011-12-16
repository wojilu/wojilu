/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;

using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;

using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Shop.Admin.Section {


    [App( typeof( ShopApp ) )]
    public partial class ListController : ControllerBase, IPageSection {

        public IShopItemService postService { get; set; }
        public IShopCategoryService classService { get; set; }
        public IShopSectionService sectionService { get; set; }
        public IAttachmentService attachService { get; set; }

        public ListController() {
            postService = new ShopItemService();
            classService = new ShopCategoryService();
            sectionService = new ShopSectionService();
            attachService = new AttachmentService();
        }

        public List<IPageSettingLink> GetSettingLink( int sectionId ) {
            List<IPageSettingLink> links = new List<IPageSettingLink>();

            PageSettingLink lnk = new PageSettingLink();
            lnk.Name = lang( "editSetting" );
            lnk.Url = to( new SectionSettingController().EditCount, sectionId );
            links.Add( lnk );

            PageSettingLink lnktmp = new PageSettingLink();
            lnktmp.Name = alang( "editTemplate" );
            lnktmp.Url = to( new TemplateCustomController().Edit, sectionId );
            links.Add( lnktmp );


            return links;
        }

        public void SectionShow( int sectionId ) {
        }

        public void AdminSectionShow( int sectionId ) {
            IList posts = postService.GetBySection( ctx.app.Id, sectionId );
            bindSectionShow( sectionId, posts );
        }


        public void AdminList( int sectionId ) {
            ShopSection section = sectionService.GetById( sectionId, ctx.app.Id );
            DataPage<ShopItem> posts = postService.GetBySectionAndType( section.Id, ctx.GetInt( "typeId" ) );

            bindAdminList( section, posts );
        }

        [HttpDelete, DbTransaction]
        public void Delete( int ItemId ) {
            ShopItem post = postService.GetById( ItemId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            postService.Delete( post );
            echoRedirectPart(lang("opok"));
        }

        ////--------------------------------------- 图片上传处理 -------------------------------------------------------------

        public void Upload( int sectionId ) {
            target( SaveUpload, sectionId );

            set( "width", ctx.GetInt( "width" ) );
            set( "height", ctx.GetInt( "height" ) );

        }

        public void SaveUpload( int sectionId ) {

            if (ctx.HasUploadFiles == false) {
                echoRedirect( alang( "exUploadEmpty" ), to( Upload, sectionId ) );
                return;
            }

            HttpFile file = ctx.GetFileSingle();
            Result result = Uploader.SaveImg( file );
            if (result.HasErrors)
                errors.Join( result );
            else {
                String imgUrl = sys.Path.GetPhotoOriginal( result.Info.ToString() );
                set( "imgUrl", imgUrl );
                set( "width", ctx.PostInt( "width" ) );
                set( "height", ctx.PostInt( "height" ) );
            }

        }

    }
}

