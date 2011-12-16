/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps;
using wojilu.ORM;

using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Web.Controller.Shop.Utils;

namespace wojilu.Web.Controller.Shop.Section {


    [App( typeof( ShopApp ) )]
    public partial class ListController : ControllerBase, IPageSection {

        public IShopItemService postService { get; set; }
        public IShopItemImgService imgService { get; set; }
        public IShopSectionService sectionService { get; set; }
        public IAttachmentService attachmentService { get; set; }
        public IShopCustomTemplateService ctService { get; set; }

        public ListController() {
            postService = new ShopItemService();
            imgService = new ShopItemImgService();
            sectionService = new ShopSectionService();
            attachmentService = new AttachmentService();
            ctService = new ShopCustomTemplateService();
        }

        public void AdminSectionShow( int sectionId ) {
        }

        public List<IPageSettingLink> GetSettingLink( int sectionId ) {
            return new List<IPageSettingLink>();
        }

        public void List( int sectionId ) {
            ShopSection section = sectionService.GetById( sectionId, ctx.app.Id );
            if (section == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }
            Page.Title = section.Title;

            ShopApp app = ctx.app.obj as ShopApp;
            ShopSetting s = app.GetSettingsObj();

            if (s.ItemListMode == ItemListMode.TitleSummary) view("ListSummary");

            DataPage<ShopItem> posts = postService.GetBySectionAndType( section.Id, ctx.GetInt( "typeId" ), s.ListPostPerPage );

            bindPostList( section, posts, s );
        }

        public void SectionShow( int sectionId ) {

            ShopSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( lang( "exDataNotFound" ) + "=>page section:" + sectionId );
            }

            TemplateUtil.loadTemplate( this, s, ctService );

            IList posts = postService.GetBySection( ctx.app.Id, sectionId );
            bindSectionPosts( posts );
        }

        public void Show( int id ) {

            ShopItem post = this.postService.GetById( id, ctx.owner.Id );
            if (post == null || post.PageSection == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            postService.AddHits( post );
            
            ctx.SetItem( "ShopItem", post );
            //target(new CartController().AddItem, post.Id);
            bindDetail( id, post );
        }



    }
}

