/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;

using wojilu.Common;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Shop.Utils;

namespace wojilu.Web.Controller.Shop.Binder
{


    public class FocusItemBinderController : ControllerBase, ISectionBinder
    {

        public void Bind(ShopSection section, IList serviceData)
        {

            IBlock fblock = getBlock("focus");
            IBlock block = getBlock("list");
            if (serviceData.Count == 0) return;

            bindFocus(fblock, (ShopItem)serviceData[0]); // 第一个是焦点产品
            bindPickedList(serviceData, block);
        }

        private void bindFocus(IBlock fblock, ShopItem article)
        {
            fblock.Set("binder.Title", strUtil.SubString(article.Title, 19));
            fblock.Set("binder.SummaryInfo", strUtil.CutString(article.Summary, 100));
            fblock.Set("binder.Url", alink.ToAppData(article));
            fblock.Next();
        }

        private void bindPickedList(IList serviceData, IBlock block)
        {
            for (int i = 1; i < serviceData.Count; i++)
            {
                ShopItem a = serviceData[i] as ShopItem;
                block.Set("binder.Title", a.Title);
                block.Set("binder.Url", alink.ToAppData(a));
                block.Set("binder.Created", a.Created.ToShortDateString());
                block.Next();
            }
        }

    }
}

