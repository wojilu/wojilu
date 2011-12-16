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
    public class CategoryController : ControllerBase
    {
        public IShopCategoryService classService { get; set; }

        public CategoryController()
        {
            classService = new ShopCategoryService();
        }

        public void List()
        {

            set("addLink", to(Add));
            set("sortAction", to(SaveSort));

            List<ShopCategory> list = classService.GetRootList();
            bindList("list", "data", list, bindLink);


        }

        private void bindLink(IBlock block, int id)
        {
            block.Set("data.LinkEdit", to(Edit, id));
            block.Set("data.LinkDelete", to(Delete, id));
        }

        [HttpPost]
        public virtual void SaveSort()
        {

            int id = ctx.PostInt("id");
            String cmd = ctx.Post("cmd");

            ShopCategory acategory = classService.GetById(id);

            List<ShopCategory> list = classService.GetRootList();

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


        public void Add()
        {
            target(Create);
        }

        [HttpPost]
        public void Create()
        {
            string name = ctx.Post("Name");
            if (strUtil.IsNullOrEmpty(name))
            {
                errors.Add("请填写名称");
                run(Add);
                return;
            }

            ShopCategory cat = new ShopCategory();
            cat.Name = name;
            cat.IsThumbView = ctx.PostIsCheck("IsThumbView");
            cat.insert();

            echoRedirect(lang("opok"), List);
        }

        public void Edit(int id)
        {
            target(Update, id);

            ShopCategory cat = classService.GetById(id);
            set("Name", cat.Name);

            String chkstr = "";
            if (cat.IsThumbView == 1) chkstr = "checked=\"checked\"";
            set("checked", chkstr);

        }

        [HttpPost]
        public void Update(int id)
        {

            string name = ctx.Post("Name");
            if (strUtil.IsNullOrEmpty(name))
            {
                errors.Add("请填写名称");
                run(Edit, id);
                return;
            }

            ShopCategory cat = classService.GetById(id);
            cat.Name = name;
            cat.IsThumbView = ctx.PostIsCheck("IsThumbView");
            cat.update();

            echoRedirect(lang("opok"), List);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            ShopCategory cat = classService.GetById(id);
            if (cat != null)
            {
                cat.delete();
                redirect(List);
            }
        }
    }
}
