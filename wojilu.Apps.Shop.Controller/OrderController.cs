using System;
using System.Collections.Generic;
using System.Text;
using wojilu.DI;
using wojilu.Serialization;
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
using wojilu.Apps.Shop.Enum;

namespace wojilu.Web.Controller.Shop
{
    [App(typeof(ShopApp))]
    public class OrderController : ControllerBase
    {
        public IShopItemService itemService;
        public IShopDeliverService deliverService;
        public IShopPaymentService paymentService;
        public IShopCategoryService classService;
        public IShopBrandService brandService;
        public IShopCartService cartService;
        public IShopOrderService oService;
        
        public OrderController()
        {
            itemService = new ShopItemService();
            classService = new ShopCategoryService();
            brandService = new ShopBrandService();
            deliverService = new ShopDeliverService();
            paymentService = new ShopPaymentService();
            cartService = new ShopCartService();
            oService = new ShopOrderService();
        }
        private void ReloadUserCart()
        {
            if (!ctx.viewer.IsLogin)
                cartService = new ShopCartCookies();
            else
                cartService = new ShopCartService();
        }
        [Login]
        public void Show(int id)
        {
        }

        [Login]
        public void List()
        {
        }
        [Login]
        public void Finished() {
            if (ctx.GetHas("orderNum")) {
                ShopOrder ord = oService.GetByOrder(ctx.Get("orderNum"));
                if (ord != null)
                {
                    set("order.Url", to(Show, ord.Id));
                    set("order.OrderNumb", ord.OrderNum);
                    set("order.TotalAmount", ord.TotalSaleAmount);
                    set("order.Payment.Title", ord.Payment.Title);
                    set("order.Deliver.Title", ord.Deliver.Title);
                    bind("order", ord);
                }
                else {
                    echoRedirect("订单号不存在，请重新下单！", Step);
                }
            }
        }
        [Login]
        public void Step() {
            target(Finished);

            WebUtils.pageTitle(this, "购物流程");
            String location = string.Format("<a href='{0}'>{1}</a> &gt; {2}",
                  Link.To(new ShopController().Index),
                  ((AppContext)ctx.app).Menu.Name,
                  "购物流程"
              );

            set("location", location);

            int TotalQty = 0;
            decimal TotalAmount = 0;
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
                block.Set("cart.Url", to(new ItemController().Show, cart.Item.Id));
                block.Set("cart.TotalSaleAmount", cart.TotalSaleAmount);
                block.Set("cart.TotalWeight", cart.TotalWeight);
                block.Set("cart.ImgSrc", cart.Item.GetImgThumb());
                block.Set("cart.ItemSKU", cart.Item.ItemSKU);
                block.Set("cart.Title", (cart.Item.IsGift == 1 ? "礼品" : ""));
                block.Next();
                TotalAmount += cart.TotalSaleAmount;
                TotalQty += cart.BuyQty;
            }
            set("cartLink", to(new CartController().List));
            set("cart.TotalAmount", TotalAmount);
            set("cart.TotalQty", TotalQty);
            set("cart.FreightAmount","10");//运费
     
            load("addressbox", AddressBox);
            load("shippingbox", ShippingBox);
        }
        [NonVisit]
        public void AddressBox() {
            List<ShopDeliverContacts> attlist = deliverService.FindContact(ctx.viewer.Id);
            IBlock block = getBlock("contactlist");
            foreach (ShopDeliverContacts att in attlist)
            {
                block.Set("contact.Id", att.Id);
                block.Set("contact.Title", att.Title);
                block.Set("contact.Contact", att.Contact);
                block.Set("contact.Address", att.AddressFirst + ", " + att.AddressSecond);
                block.Set("contact.ShipTime", att.AgreeShipTime);
                block.Set("contact.Faxphone", att.Faxphone);
                block.Set("contact.LocalBuild", att.LocalBuilding);
                block.Set("contact.Mobilephone", att.Mobilephone);
                block.Set("contact.PostCode", att.PostCode);
                block.Set("contact.Telphone", att.Telphone);
                block.Next();
            }
            ShopDeliverContacts defatt = deliverService.LoadContactDef(ctx.viewer.Id);
            if (defatt != null)
            {
                set("contactDef.Title", defatt.Title);
                set("contactDef.Contact", defatt.Contact);
                set("contactDef.AddressState", defatt.AddressFirst);
                set("contactDef.Address", defatt.AddressSecond);
                set("contactDef.Faxphone", defatt.Faxphone);
                set("contactDef.LocalBuild", defatt.LocalBuilding);
                set("contactDef.Mobile", defatt.Mobilephone);
                set("contactDef.PostCode", defatt.PostCode);
                set("contactDef.TelPhone", defatt.Telphone);
            }
            else {
                set("contactDef.Title", "");
                set("contactDef.Contact", "");
                set("contactDef.AddressState", "");
                set("contactDef.Address", "");
                set("contactDef.Faxphone", "");
                set("contactDef.LocalBuild", "");
                set("contactDef.Mobile", "");
                set("contactDef.PostCode", "");
                set("contactDef.TelPhone", "");
            }
        }
        [Login,HttpPost]
        public void SaveAddress() { 
            //添加地址保存操作
            if (ctx.HasErrors)
            {
                echoError();
                return;
            }
            int attid = ctx.PostInt("AddressId");
            string contact = ctx.PostHtml("Consignee");
            int provinceId = ctx.PostInt("Province");
            int cityId = ctx.PostInt("City");
            int districtId = ctx.PostInt("District");
            string address = ctx.PostHtml("Address");
            string telPhone = ctx.PostHtml("Tel");
            string mobilePhone = ctx.PostHtml("Mobile");
            string provinceName = ctx.PostHtml("ProvinceName");
            string cityName = ctx.PostHtml("CityName");
            string districtName = ctx.PostHtml("DistrictName");
            ShopDeliverContacts shopAtts = new ShopDeliverContacts();
            if (ctx.PostHas("AddressId") && attid > 0)
            {
                shopAtts = ShopDeliverContacts.findById(attid);
            }
            else
            {
                shopAtts.OrderId = 0;
                shopAtts.IsDefault = 0;
                shopAtts.Enabled = 1;
                shopAtts.Creator = (User)ctx.viewer.obj;
                shopAtts.Created = DateTime.Now;
            }
            shopAtts.Title = contact;
            shopAtts.Contact = contact;
            shopAtts.AddressFirst = provinceName + "-" + cityName + "-" + districtName;
            shopAtts.CountryId = 1;
            shopAtts.StateId = provinceId;
            shopAtts.CityId = cityId;
            shopAtts.PlaceId = districtId;
            shopAtts.AddressSecond = address;
            shopAtts.Telphone = telPhone;
            shopAtts.Mobilephone = mobilePhone;
            shopAtts.Updated = DateTime.Now;
            if (ctx.PostHas("AddressId") && attid > 0)
            {
                deliverService.UpdateContact(shopAtts);
                echoJson(JsonString.ConvertObject(shopAtts));
            }
            else {
                //shopAtts.IsDefault = 1;
                deliverService.InsertContact(shopAtts);
                echoJson(JsonString.ConvertObject(deliverService.LoadContact(ctx.viewer.Id,shopAtts.Contact)));
            }
        }
        [Login, HttpPost,DbTransaction]
        public void SaveOrder()
        {
            //订单保存操作
            //添加地址保存操作
            if (ctx.HasErrors)
            {
                echoError();
                return;
            }
            //int Ordid = ctx.PostInt("OrderId");
            int seladdrid = ctx.PostInt("seladdrid");
            int selshipid = ctx.PostInt("selshipid");
            int selpayid = ctx.PostInt("selpayid");
            //ShopOrderItem ordItems = new ShopOrderItem();
            List<ShoppingCart> clist = cartService.List(ctx.viewer.Id);
            ShopOrder ord = new ShopOrder();
                ord.OrderNum = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + ctx.viewer.Id.ToString();
                ord.Title = "";
                ord.OwnerId = ctx.owner.Id;
                ord.OwnerUrl = ctx.owner.obj.Url;
                ord.OwnerType = ctx.owner.obj.GetType().FullName;
                ord.Creator = (User)ctx.viewer.obj;
                ord.CreatorUrl = ctx.viewer.obj.Url;
                ord.AppId = ctx.app.Id;
                ord.Ip = ctx.Ip;
                ord.Deliver = ShopDeliver.findById(selshipid);
                ord.DeliverContact = ShopDeliverContacts.findById(seladdrid);
                ord.ShipTo = ord.DeliverContact.Contact;
                ord.ShipAddress = ord.DeliverContact.AddressFirst + " " + ord.DeliverContact.AddressSecond;
                ord.ShipZip = ord.DeliverContact.PostCode;
                ord.ShipTelPhone = ord.DeliverContact.Telphone;
                ord.ShipMobile = ord.DeliverContact.Mobilephone;
                ord.ShipArea = ord.DeliverContact.AddressFirst;
                ord.ShipBuilding = ord.DeliverContact.LocalBuilding;
                ord.ShipTaxName = "";
                ord.Payment = ShopPayment.findById(selpayid);
                ord.Created = DateTime.Now;
                ord.Updated = DateTime.Now;
                ord.Activitystatus = OrderActStatus.InProgress;
                ord.Orderstatus = OrderStatus.UnStart;
                ord.Paymentstatus = PaymentStatus.All;
                ord.CurrencyId = 1;
                ord.OrderRemark = "";
                ord.Description = "";
                decimal totalAmount = 0M;
                foreach (ShoppingCart cart in clist) {
                    totalAmount += cart.TotalSaleAmount;
                }
                ord.FreightAmount = 0;
                ord.TotalAmount = totalAmount;
                ord.TotalPoint = cvt.ToInt(totalAmount);
                ord.SaveStatus = 0;
                ord.AccessStatus = 0;
            Result r = ord.insert();
            if (r.HasErrors)
            {
                echoError(r);
                return;
            }
            //TODO:批量添加购物车产品至订单产品表，同时清空购物车。
            if (!strUtil.IsNullOrEmpty(ord.OrderNum))
            {
                ord = oService.GetByOrder(ord.OrderNum);
                if (ord != null && ord.Id > 0)
                {
                    oService.InsertOrderItemBatch(ord.Id, clist);
                    if (r.HasErrors)
                    {
                        echoJson(r.ErrorsJson);
                    }
                    else
                    {
                        cartService.Clear(ctx.viewer.Id);//清空购物车
                        echoJsonMsg("订单创建成功", true, ord.OrderNum);
                    }
                }
                else {
                    echoJsonMsg("订单创建失败", false,"");
                }
            }
        }

        [NonVisit]
        public void ShippingBox()
        {
            List<ShopDeliver> devlist = deliverService.ListDeliver();
            IBlock block = getBlock("devliverlist");
            foreach (ShopDeliver devl in devlist)
            {
                block.Set("devliver.Id", devl.Id);
                block.Set("devliver.Title", devl.Title);
                block.Set("devliver.Weight", devl.Weight);
                block.Set("devliver.Price", devl.Price);
                block.Set("devliver.AddWeight", devl.AddWeight);
                block.Set("devliver.AddPrice", devl.AddPrice);
                block.Set("devliver.Description", devl.Description);
                block.Next();
            }
            List<ShopPayment> plist = paymentService.List();
            IBlock block2 = getBlock("paylist");
            foreach (ShopPayment gpay in plist)
            {
                block2.Set("pay.Id", gpay.Id);
                block2.Set("pay.Title", gpay.Title);
                block2.Set("pay.Logo", gpay.Logo);
                block2.Set("pay.Description", gpay.Description);
                block2.Next();
            }
        }

        public void QueryAddressDef() {
            ShopDeliverContacts dc = deliverService.LoadContactDef(ctx.viewer.Id);
            echoJson(dc != null ? JsonString.ConvertObject(dc) : "");
        }
        public void QueryAddress(int attid)
        {
            ShopDeliverContacts dc = deliverService.LoadContact(attid);
            echoJson(dc != null ? JsonString.ConvertObject(dc) : "");
        }
        public void AddressList() {
            List<ShopDeliverContacts> dcs = deliverService.FindContact(ctx.viewer.Id);
            echoJson(dcs.Count > 0 ? JsonString.ConvertList(dcs) : "");
        }
        public void QueryPayment(int shipid) {
            ShopDeliver dc = ShopDeliver.findById(shipid);
            echoJson(dc.PaymentList != null ? JsonString.ConvertList(dc.PaymentList) : "");
        }
        public void PaymentList() {
            List<ShopPayment> dcs = paymentService.List();
            echoJson(dcs.Count > 0 ? JsonString.ConvertList(dcs) : "");
        }
    }
}
