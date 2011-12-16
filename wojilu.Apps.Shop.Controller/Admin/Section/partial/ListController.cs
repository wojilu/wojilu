/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Apps.Shop.Domain;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Apps.Shop.Enum;

namespace wojilu.Web.Controller.Shop.Admin.Section {

    public partial class ListController : ControllerBase, IPageSection {


        private void bindSectionShow( int sectionId, IList posts ) {
            set("addUrl", to(new ItemController().Add, sectionId));
            set( "listUrl", to( AdminList, sectionId ) );
            IBlock block = getBlock( "list" );
            foreach (ShopItem post in posts) {

                block.Set("sku.TitleCss", post.Style);
                block.Set("sku.TitleFull", post.Title);

                if (strUtil.HasText( post.TitleHome ))
                    block.Set("sku.Title", post.TitleHome);
                else
                    block.Set("sku.Title", post.Title);

                block.Set("sku.Url", to(new ItemController().Edit, post.Id));
                block.Next();
            }
        }


        private void bindAdminList(ShopSection section, DataPage<ShopItem> skus)
        {

            set( "section.Title", section.Title );
            IBlock block = getBlock( "list" );

            //String icon = string.Format( "<img src=\"{0}\"/> ", strUtil.Join( sys.Path.Img, "img.gif" ) );

            foreach (ShopItem sku in skus.Results)
            {

                //String imgIcon = sku.HasImg() ? icon : "";
                //block.Set("sku.ImgIcon", imgIcon);

                block.Set("sku.Title", strUtil.SubString(sku.Title, 50));
                block.Set("sku.TitleCss", sku.Style);
                block.Set("sku.TitleFull", sku.Title);

                block.Set("sku.OrderId", sku.OrderId);
                block.Set("sku.Url", sku.SourceLink);
                block.Set("sku.Link", strUtil.CutString(sku.SourceLink, 100));
                block.Set("sku.PubDate", sku.Created);

                //String attachments = sku.Attachments > 0 ? "<img src='" + strUtil.Join(sys.Path.Img, "attachment.gif") + "'/>" : "";
                //block.Set("sku.Attachments", attachments);

                if (sku.HasImg())
                    block.Set("sku.EditUrl", to(new ItemController().EditImg, sku.Id));
                else
                    block.Set("sku.EditUrl", to(new ItemController().Edit, sku.Id));

                block.Set("sku.DeleteUrl", to(Delete, sku.Id));
                block.Next();
            }
            set("page", skus.PageBar);
        }

    }
}

