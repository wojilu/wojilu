using System;
using System.Text;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Mvc;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps;
using wojilu.Web.Context;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Common;
using wojilu.DI;
using System.Collections.Generic;
using wojilu.Members.Sites.Domain;
using wojilu.Apps.Shop.Enum;

namespace wojilu.Web.Controller.Shop.Utils
{
    public class Location
    {
        public static String GetItem(MvcContext ctx, ShopItem f)
        {
            String catLocation = GetCategory(ctx, f.CategoryId);
            String fileLink = string.Format("<a href=\"{0}\">{1}</a>", ctx.to(new ItemController().Show, f.Id), f.Title);
            return catLocation + " " + separator + " " + fileLink;
        }

        public static String GetCategory(MvcContext ctx, int categoryId)
        {

            ShopCategory cat = new ShopCategoryService().GetById(categoryId);
            if (cat.ParentId == 0)
            {
                return getCatLink(ctx, cat);
            }
            else
            {
                ShopCategory parent = new ShopCategoryService().GetById(cat.ParentId);
                return getCatLink(ctx, parent) + " " + separator + " " + getCatLink(ctx, cat);
            }
        }

        private static String getCatLink(MvcContext ctx, ShopCategory cat)
        {
            String catLink = string.Format("<a href=\"{0}\">{1}</a>", ctx.to(new CategoryController().Show, cat.Id), cat.Name);
            return catLink;
        }

        public static String GetSubCategories(MvcContext ctx, ShopCategory c)
        {

            int rootId = c.Id;
            if (c.ParentId > 0) rootId = c.ParentId;

            StringBuilder sb = new StringBuilder();

            List<ShopCategory> subs = new ShopCategoryService().GetByParentId(rootId);
            foreach (ShopCategory sub in subs)
            {

                sb.AppendFormat("<a href=\"{0}\">{1}</a> ", ctx.to(new CategoryController().Show, sub.Id), sub.Name);

            }

            return sb.ToString();
        }

        public static readonly String separator = "&rsaquo;&rsaquo;";
    }
}
