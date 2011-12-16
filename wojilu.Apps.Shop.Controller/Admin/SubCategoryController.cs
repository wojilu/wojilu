using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;
using wojilu.DI;
using wojilu.Common.AppBase;
using wojilu.Web.Utils;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Shop.Admin
{

    [App(typeof (ShopApp))]
    public class SubCategoryController : ControllerBase
    {
        public IShopCategoryService classService { get; set; }

        public SubCategoryController()
        {
            classService = new ShopCategoryService();
        }

        public void List()
        {
            set("addLink", to(Add, 0));

            List<ShopCategory> cats = classService.GetRootList();
            IBlock block = getBlock("cat");
            foreach (ShopCategory cat in cats)
            {

                block.Set("cat.ThumbIcon", cat.ThumbIcon);

                block.Set("cat.Name", cat.Name);
                block.Set("cat.Link", to(new CategoryController().Edit, cat.Id));
                block.Set("cat.AddSubLink", to(Add, cat.Id));
                block.Set("cat.SortLink", to(ListSub, cat.Id));

                bindSubCats(block, cat);

                block.Next();

            }
        }

        private void bindSubCats(IBlock block, ShopCategory cat)
        {
            IBlock subBlock = block.GetBlock("subcat");
            List<ShopCategory> subcats = classService.GetByParentId(cat.Id);
            foreach (ShopCategory subcat in subcats)
            {

                subBlock.Set("subcat.ThumbIcon", subcat.ThumbIcon);
                subBlock.Set("subcat.Name", subcat.Name);
                subBlock.Set("subcat.Link", to(Edit, subcat.Id));
                subBlock.Next();
            }
        }


        public void ListSub(int id)
        {
            ShopCategory cat = classService.GetById(id);
            set("cat.Name", cat.Name);
            set("addLink", to(Add, id));
            set("sortAction", to(SaveSort, id));

            IBlock block = getBlock("list");
            List<ShopCategory> subcats = classService.GetByParentId(cat.Id);
            foreach (ShopCategory subcat in subcats)
            {

                block.Set("data.ThumbIcon", subcat.ThumbIcon);
                block.Set("data.Id", subcat.Id);
                block.Set("data.Name", subcat.Name);
                block.Set("data.LinkEdit", to(Edit, subcat.Id));
                block.Set("data.LinkDelete", to(Delete, subcat.Id));
                block.Next();

            }
        }

        //------------------------------------------------------------------------------------------------------

        public void Items()
        {

            List<ShopCategory> cats = classService.GetRootList();
            IBlock block = getBlock("cat");
            foreach (ShopCategory cat in cats)
            {

                block.Set("cat.ThumbIcon", cat.ThumbIcon);
                block.Set("cat.Name", cat.Name);
                block.Set("cat.Link", to(new ItemController().Category, cat.Id));

                bindSubCatsGoods(block, cat);

                block.Next();
            }
        }

        private void bindSubCatsGoods(IBlock block, ShopCategory cat)
        {
            IBlock subBlock = block.GetBlock("subcat");
            List<ShopCategory> subcats = classService.GetByParentId(cat.Id);
            foreach (ShopCategory subcat in subcats)
            {

                subBlock.Set("subcat.ThumbIcon", subcat.ThumbIcon);
                subBlock.Set("subcat.Name", subcat.Name);
                subBlock.Set("subcat.DataCount", subcat.DataCount);
                subBlock.Set("subcat.Link", to(new ItemController().Category, subcat.Id));
                subBlock.Next();
            }
        }


        [HttpPost]
        public virtual void SaveSort(int parentId)
        {

            int id = ctx.PostInt("id");
            String cmd = ctx.Post("cmd");

            ShopCategory acategory = classService.GetById(id);

            List<ShopCategory> list = classService.GetByParentId(parentId);

            if (cmd == "up")
            {

                new SortUtil<ShopCategory>(acategory, list).MoveUp();
                echoRedirect("ok");
            }
            else if (cmd == "down")
            {

                new SortUtil<ShopCategory>(acategory, list).MoveDown();
                echoRedirect("ok");
            }
            else
            {
                echoError(lang("exUnknowCmd"));
            }

        }

        //------------------------------------------------------------------------------------------------------


        public void Add(int id)
        {
            target(Create);

            List<ShopCategory> cats = classService.GetRootList();
            dropList("shopCategory.ParentId", cats, "Name=Id", id);
        }

        [HttpPost]
        public void Create()
        {

            ShopCategory cat = ctx.PostValue<ShopCategory>();
            if (ctx.HasErrors)
            {
                run(Add, ctx.PostInt("shopCategory.ParentId"));
                return;
            }

            cat.IsThumbView = ctx.PostIsCheck("shopCategory.IsThumbView");

            cat.insert();
            echoRedirect(lang("opok"), List);
        }


        public void Edit(int id)
        {

            ShopCategory cat = classService.GetById(id);
            bind(cat);

            List<ShopCategory> cats = classService.GetRootList();
            dropList("shopCategory.ParentId", cats, "Name=Id", cat.ParentId);
            target(Update, id);

            String chkstr = "";
            if (cat.IsThumbView == 1) chkstr = "checked=\"checked\"";
            set("checked", chkstr);
        }

        [HttpPost]
        public void Update(int id)
        {
            ShopCategory c = classService.GetById(id);

            ShopCategory cat = ctx.PostValue(c) as ShopCategory;
            if (ctx.HasErrors)
            {
                run(Edit, id);
                return;
            }
            cat.IsThumbView = ctx.PostIsCheck("shopCategory.IsThumbView");


            cat.update();
            echoRedirect(lang("opok"), List);

        }

        [HttpDelete]
        public void Delete(int id)
        {
            ShopCategory f = classService.GetById(id);
            if (f != null)
            {
                f.delete();
                redirect(List);
            }
        }
    }
}
