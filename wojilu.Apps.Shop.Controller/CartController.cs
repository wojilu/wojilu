using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Serialization;
using wojilu.DI;
using wojilu.Web.Controller.Shop.Utils;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Shop.Caching;
using wojilu.Web.Context;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Shop
{
    [App(typeof(ShopApp))]
    public class CartController : ControllerBase
    {
        public IShopItemService itemService;
        public IShopCategoryService classService;
        public IShopBrandService brandService;
        public IShopCartService cartService;

        public CartController()
        {
            itemService = new ShopItemService();
            classService = new ShopCategoryService();
            brandService = new ShopBrandService();
            cartService = new ShopCartService();
        }
        private void ReloadUserCart() {
            if (!ctx.viewer.IsLogin)
                cartService = new ShopCartCookies();
            else
                cartService = new ShopCartService();
        }

        [NonVisit]
        public void CartBar()
        {
            set("cartlistaction", to(List));
            set("cartcount", "(" + ctx.app.Id + ")" + cartService.Count(ctx.viewer.Id).ToString());
            set("jsoncartcount", to(QueryCount));
            set("jsoncartlist", to(QueryList));
        }
        public void QueryList() {
            echoJson(JsonString.ConvertList(cartService.List(ctx.viewer.Id)));
        }
        public void QueryCount() {
            echoJson(JsonString.Convert(cartService.Count(ctx.viewer.Id)));
        }

        public void List()
        {
            this.ReloadUserCart();

            WebUtils.pageTitle(this, "购物车");
            String location = string.Format("<a href='{0}'>{1}</a> &gt; {2}",
                  Link.To(new ShopController().Index),
                  ((AppContext)ctx.app).Menu.Name,
                  "查看购物车"
              );

            set("location", location);
            int TotalQty = 0;
            decimal  TotalAmount = 0;
            List<ShoppingCart> list = cartService.List(ctx.viewer.Id);
            IBlock block = getBlock("list");
            foreach (ShoppingCart cart in list)
            {
                block.Set("cart.SKUId", cart.Item.Id);
                block.Set("cart.Title", cart.Title);
                block.Set("cart.Price", cart.SalePrice);
                block.Set("cart.Points", cart.Points);
                block.Set("cart.Weight", cart.ItemWeight);
                block.Set("cart.Attribute", cart.ItemAttribute);
                block.Set("cart.Qty", cart.BuyQty);
                block.Set("cart.Url", to(new ItemController().Show,cart.Item.Id));
                block.Set("cart.TotalSaleAmount", cart.TotalSaleAmount);
                block.Set("cart.TotalWeight", cart.TotalWeight);
                block.Set("cart.ImgSrc", cart.Item.GetImgThumb());
                block.Set("cart.ItemSKU", cart.Item.ItemSKU);
                block.Set("cart.Title", (cart.Item.IsGift == 1 ? "礼品" : ""));
                block.Next();
                TotalAmount += cart.TotalSaleAmount;
                TotalQty += cart.BuyQty;
            }
            set("cart.PrvtBtn", to(new ShopController().Index));
            set("cart.NextBtn", to(new OrderController().Step));
            set("cart.TotalAmount", TotalAmount);
            set("cart.TotalQty", TotalQty);
        }
        //public void Add(int id,int qty,string attr)
        [HttpPost, DbTransaction]
        public void Add()
        {
            this.ReloadUserCart();
            if (ctx.HasErrors)
            {
                echoError();
                return;
            }

            int itemid = ctx.PostInt("buyId");
            int itemQty = ctx.PostInt("buyQty");
            itemQty = itemQty > 0 ? itemQty : 1;
            string itemSKU = ctx.Post("buySKU");
            string itemAttr = ctx.Post("buyAttr");
            if (ctx.PostHas("buyId") && itemid > 0)
            {
                ShopItem sku = ShopItem.findById(itemid);
                if (sku != null && sku.IsSale==1) {
                    ShoppingCart cart = new ShoppingCart();
                    cart.Creator = (User)ctx.viewer.obj;
                    cart.Item = sku;
                    cart.BuyQty = (itemQty > 0 ? itemQty : 1);
                    cart.ItemAttribute = (!strUtil.IsNullOrEmpty(itemAttr) ? itemAttr : "");
                    cart.Title = sku.Title + "(" + (!strUtil.IsNullOrEmpty(itemSKU) ? itemSKU : sku.ItemSKU) + ")";
                    cart.SalePrice = sku.SalePrice;
                    cart.Created = DateTime.Now;
                    cart.Ip = ctx.Ip;
                    cart.Points = cvt.ToInt(sku.SalePrice);
                    //cart.TotalSaleAmount = sku.SalePrice * qty;
                    //cart.TotalWeight = sku.Weight * qty;
                    cartService.Add(cart);
                    echoJsonMsg("已成功加入购物车！", false, "cartResult");
                    //echoAjaxOk();
                }
                else
                {
                    echoError("下架产品不能加入购物车！");
                    //echoText("下架产品不能加入购物车！");
                }
            }
            else
            {
                echoError("加入购物车失败！");
                //echoText("加入购物车失败！");
            }
        }

        [HttpPost, DbTransaction]
        public void AddItem(int id)
        {
            this.ReloadUserCart();
            if (ctx.HasErrors)
            {
                echoError();
                return;
            }

            //if (qty <= 0) echoRedirect("加入购物车商品数量必须大于0！");
            if (id > 0)
            {
                ShopItem sku = ShopItem.findById(id);
                if (sku != null && sku.IsSale==1)
                {
                    ShoppingCart cart = new ShoppingCart();
                    cart.Creator = (User)ctx.viewer.obj;
                    string itemSKU = ctx.Post("buySKU");
                    int itemQty = ctx.PostInt("buyQty");
                    string itemAttr = ctx.Post("buyAttr");
                    cart.Item = sku;
                    cart.BuyQty = (itemQty > 0 ? itemQty : 1);
                    cart.ItemAttribute = (!strUtil.IsNullOrEmpty(itemAttr)?itemAttr:"");
                    cart.Title = sku.Title + "(" + (!strUtil.IsNullOrEmpty(itemSKU) ? itemSKU : sku.ItemSKU) + ")";
                    cart.SalePrice = sku.SalePrice;
                    cart.Created = DateTime.Now;
                    cart.Ip = ctx.Ip;
                    cart.Points = cvt.ToInt(sku.SalePrice);
                    //cart.TotalSaleAmount = sku.SalePrice * qty;
                    //cart.TotalWeight = sku.Weight * qty;
                    cartService.Add(cart);
                    echoJsonMsg("已成功加入购物车！", true, "formResult");
                }
                else
                {
                    echoError("下架产品不能加入购物车！");
                    //echoText("下架产品不能加入购物车！");
                }
            }
            else
            {
                echoError("加入购物车失败！");
                //echoText("加入购物车失败！");
            }
        }

        public void Edit(int id) {
            this.ReloadUserCart();


        }

        [HttpPost, DbTransaction]
        public void Update(int id, int qty)
        {
            this.ReloadUserCart();

            if (id > 0 && qty > 0)
            {
                cartService.Update(id, qty, ctx.viewer.Id);
                echoRedirect("产品更新成功！", to(List));

            }
            else {
                echoRedirect("产品更新失败！", to(List));
            }
        }

        [HttpPost, DbTransaction]
        public void Delete(int id)
        {
            this.ReloadUserCart();

            if (id > 0)
            {
                cartService.Remove(id, ctx.viewer.Id);
                echoRedirect("成功删除", to(List));
            }
        }
    }
}
