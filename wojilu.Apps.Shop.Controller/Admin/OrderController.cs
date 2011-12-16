using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Shop.Enum;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;
using wojilu.DI;

namespace wojilu.Web.Controller.Shop.Admin {

    [App(typeof (ShopApp))]
    public class OrderController : ControllerBase
    {
        public IShopOrderService OrderSvr { get; set; }
        public IShopItemService ItemSvr { get; set; }
        public IShopCategoryService ClassSvr { get; set; }
        public IShopPaymentService PaymentSvr { get; set; }
        public IShopDeliverService DeliverSvr { get; set; }

        public OrderController()
        {
            OrderSvr = new ShopOrderService();
            ItemSvr = new ShopItemService();
            ClassSvr = new ShopCategoryService();
            PaymentSvr = new ShopPaymentService();
            DeliverSvr = new ShopDeliverService();
        }

        public void Show(int orderid)
        {
            ShopOrder order = OrderSvr.GetById(orderid);
            if (order == null)
            {
                echo(lang("exDataNotFound"));
                return;
            }
            bind("order", order);
        }

        //public void Show(string orderNum)
        //{
        //    ShopOrder order = OrderSvr.GetByOrder(orderNum);
        //    if (order == null)
        //    {
        //        echo(lang("exDataNotFound"));
        //        return;
        //    }
        //    bind("order", order);
        //}

        public void List()
        {
            set("OperationUrl", to(SaveAdmin));
            ShopApp app = ctx.app.obj as ShopApp;
            set("app.Name", ctx.app.Name);
            set("app.Link", to(new ShopController().Index));
            set("searchKey", "");
            OrderFilterMethod filterm = OrderFilterMethod.All;
            if (ctx.GetHas("Filter"))
            {
                filterm = (OrderFilterMethod)cvt.ToInt(ctx.GetInt("Filter"));
            }
            DataPage<ShopOrder> ords = OrderSvr.FindOrder(ctx.app.Id, filterm, 50);

            bool isTrash = false;
            bindAdminList(ords, isTrash);

            target(Search);
        }

        private void bindAdminList(DataPage<ShopOrder> ords, bool isTrash)
        {
            IBlock block = getBlock("list");
            foreach (ShopOrder order in ords.Results)
            {
                block.Set("order.Title", strUtil.SubString(order.Title, 50));
                block.Set("order.OrderProcess",((OrderStatus)order.Orderstatus));
                if (order.Creator != null)
                {
                    block.Set("order.Buyer", string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", Link.ToMember(order.Creator), order.Creator.Name));
                }
                else
                {
                    block.Set("order.Buyer", "无");
                }
                block.Bind("order", order);
                IBlock blockitem = block.GetBlock("itemlist");
                foreach (ShopOrderItem item in order.ListItem)
                {

                    blockitem.Set("item.Title", item.Item.Title);
                    blockitem.Set("item.ItemSKU", item.Item.ItemSKU);
                    blockitem.Set("item.Url", Link.To(new Shop.ItemController().Show, item.Item.Id));
                    blockitem.Bind("item", item);
                    blockitem.Next();
                }
                String lnkEdit = "";
                if (isTrash)
                {
                    lnkEdit = "#";
                }
                else
                {
                    lnkEdit = to(Edit, order.Id);
                }

                String lnkDelete = to(Delete, order.Id);
                if (isTrash) lnkDelete = to(DeleteTrue, order.Id);
                block.Set("order.EditUrl", lnkEdit);
                block.Set("order.DeleteUrl", lnkDelete);
                block.Set("order.RestoreUrl", to(Restore, order.Id));
                block.Next();
            }
            set("page", ords.PageBar);
        }

        public void Search()
        {
            view("List");
            set("OperationUrl", to(SaveAdmin));
            ShopApp app = ctx.app.obj as ShopApp;
            set("app.Name", ctx.app.Name);
            set("app.Link", to(new ShopController().Index));
            String key = strUtil.SqlClean(ctx.Get("q"), 10);
            set("searchKey", key);
            target(Search);
            OrderFilterMethod filterm = OrderFilterMethod.All;
            if (ctx.GetHas("Filter"))
            {
                filterm = (OrderFilterMethod)cvt.ToInt(ctx.GetInt("Filter"));
            }
            DataPage<ShopOrder> ords = OrderSvr.FindOrder(ctx.app.Id, filterm, 50);
            bool isTrash = false;
            bindAdminList(ords, isTrash);

        }

        public void Trash()
        {
            ShopApp app = ctx.app.obj as ShopApp;
            set("app.Name", ctx.app.Name);
            set("app.Link", to(new ShopController().Index));

            DataPage<ShopOrder> posts = OrderSvr.FindOrder(ctx.app.Id, 50);

            bool isTrash = true;
            bindAdminList(posts, isTrash);

            target(Search);
        }

        public void Edit(int id)
        {
            target(Update, id);
        }

        public void Process(int id, OrderStatus stat)
        {
        }

        [HttpPost, DbTransaction]
        public void OrderProcess(int id, OrderStatus stat)
        {
        }

        [HttpPost, DbTransaction]
        public void SaveAdmin()
        {
        }

        [HttpPost]
        public void Update(int id)
        {
        }

        [HttpDelete, DbTransaction]
        public void Delete(int id)
        {
            ShopOrder post = OrderSvr.GetById(id, ctx.owner.Id);
            if (post == null)
            {
                echo(lang("exDataNotFound"));
                return;
            }
            OrderSvr.Delete(post);
            redirect(List);
        }

        [HttpPut, DbTransaction]
        public void Restore(int id)
        {
            OrderSvr.Restore(id);
            ShopOrder post = OrderSvr.GetById(id, ctx.owner.Id);
            redirect(List);
        }
        [HttpDelete, DbTransaction]
        public void DeleteTrue(int id)
        {
            OrderSvr.DeleteTrue(id);
            redirect(List);
        }
    }

}
