/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;

using wojilu.Common;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Shop.Utils;

namespace wojilu.Web.Controller.Shop.Binder
{


    public class ImgItemBinderController : ControllerBase, ISectionBinder {

        public IShopCustomTemplateService ctService { get; set; }
        public ImgItemBinderController()
        {
            ctService = new ShopCustomTemplateService();
        }

        public void Bind(ShopSection section, IList serviceData)
        {

            IBlock block = base.getBlock( "list" );

            foreach (IBinderValue item in serviceData) {

                block.Set("binder.Title", strUtil.SubString(item.Title, 10));
                block.Set("binder.Url", item.Link);
                block.Set("binder.PicUrl", item.PicUrl);

                block.Bind("binder", item);

                block.Next();
            }

        }
    }
}

