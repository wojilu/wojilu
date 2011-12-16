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


    public class ItemImgBinderController : ControllerBase, ISectionBinder {

        public IShopCustomTemplateService ctService { get; set; }

        public ItemImgBinderController()
        {
            ctService = new ShopCustomTemplateService();
        }

        public void Bind(ShopSection section, IList serviceData)
        {

            TemplateUtil.loadTemplate( this, section, ctService );

            IBlock block = base.getBlock( "list" );

            int i = 1;
            foreach (IBinderValue item in serviceData) {

                wojilu.Web.Controller.Content.Utils.BinderUtils.bindMashupData( block, item, i );

                block.Next();
                i++;
            }
        }

    }
}

