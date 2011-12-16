using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
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

namespace wojilu.Web.Controller.Shop
{
    public class ShopCartCookies : IShopCartService
    {
        private WebContext wc = new WebContext(System.Web.HttpContext.Current);
        private const string CartDataCookieName = "wojilu_cart";
        public virtual void Add(ShoppingCart cart)
        {
            if (cart.BuyQty <= 0)
            {
                cart.BuyQty = 1;
            }
            XmlDocument doc = this.GetShoppingCartData();
            XmlNode node = doc.SelectSingleNode("//lis");
            XmlNode newChild = doc.SelectSingleNode("//l[@skuid='" + cart.Item.Id.ToString() + "']");
            if (newChild != null)
            {
                //update
                newChild.Attributes["qty"].Value = cart.BuyQty.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            else { 
                //add
                newChild = doc.CreateElement("l");
                XmlAttribute attribute = doc.CreateAttribute("skuid");
                attribute.Value = cart.Item.Id.ToString(System.Globalization.CultureInfo.InvariantCulture);
                XmlAttribute attribute2 = doc.CreateAttribute("w");
                attribute2.Value = cart.ItemWeight.ToString(System.Globalization.CultureInfo.InvariantCulture);
                XmlAttribute attribute3 = doc.CreateAttribute("p");
                attribute3.Value = cart.SalePrice.ToString(System.Globalization.CultureInfo.InvariantCulture);
                XmlAttribute attribute4 = doc.CreateAttribute("name");
                attribute4.Value = cart.Title.ToString(System.Globalization.CultureInfo.InvariantCulture);
                XmlAttribute attribute5 = doc.CreateAttribute("qty");
                attribute5.Value = cart.BuyQty.ToString(System.Globalization.CultureInfo.InvariantCulture);
                XmlAttribute attribute6 = doc.CreateAttribute("a");
                attribute6.Value = cart.ItemAttribute.ToString(System.Globalization.CultureInfo.InvariantCulture);
                newChild.Attributes.Append(attribute);
                newChild.Attributes.Append(attribute2);
                newChild.Attributes.Append(attribute3);
                newChild.Attributes.Append(attribute4);
                newChild.Attributes.Append(attribute5);
                newChild.Attributes.Append(attribute6);
                node.InsertBefore(newChild, node.FirstChild);
            }
            this.SaveShoppingCartData(doc);
        }
        public virtual void Clear(int userid)
        {
            wc.CookieRemove(CartDataCookieName);
        }
        public virtual ShoppingCart Get(int skuid, int userid)
        {
            XmlDocument shoppingCartData = this.GetShoppingCartData();
            List<ShoppingCart> carts = new List<ShoppingCart>();
            XmlNode node = shoppingCartData.SelectSingleNode("//lis/l[@skuid='" + skuid.ToString() + "']");
            if (node != null) {
                ShoppingCart shoppingCart = new ShoppingCart();
                shoppingCart.Item.Id = cvt.ToInt(node.Attributes["skuid"].Value);
                shoppingCart.ItemWeight = cvt.ToInt(node.Attributes["w"].Value);
                shoppingCart.SalePrice = cvt.ToDecimal(node.Attributes["p"].Value);
                shoppingCart.Title = node.Attributes["name"].Value;
                shoppingCart.BuyQty = cvt.ToInt(node.Attributes["qty"].Value);
                shoppingCart.ItemAttribute = node.Attributes["a"].Value;
                return shoppingCart;
            }
            return null;
        }
        public virtual List<ShoppingCart> List(int userid)
        {
            XmlDocument shoppingCartData = this.GetShoppingCartData();
            List<ShoppingCart> carts = new List<ShoppingCart>();
            XmlNodeList list = shoppingCartData.SelectSingleNode("//lis").ChildNodes;
            if ((list != null) && (list.Count > 0))
            {
                foreach (XmlNode node in list)
                {
                    if (node.Attributes["skuid"].Value != null)
                    {
                        ShoppingCart shoppingCart = new ShoppingCart();
                        shoppingCart.Item = ShopItem.findById(cvt.ToInt(node.Attributes["skuid"].Value));
                        shoppingCart.ItemWeight = cvt.ToInt(node.Attributes["w"].Value);
                        shoppingCart.SalePrice = cvt.ToDecimal(node.Attributes["p"].Value);
                        shoppingCart.Title = node.Attributes["name"].Value;
                        shoppingCart.BuyQty = cvt.ToInt(node.Attributes["qty"].Value);
                        shoppingCart.ItemAttribute = node.Attributes["a"].Value;
                        carts.Add(shoppingCart);
                    }
                }
            }
            return carts;
        }
        public virtual int Count(int userid) {
            XmlDocument doc = this.GetShoppingCartData();
            XmlNode rnode = doc.SelectSingleNode("//lis");
            return rnode.ChildNodes.Count;
        }
        public virtual void Remove(int skuid, int userid)
        {
            XmlDocument cartdb = this.GetShoppingCartData();
            XmlNode node = cartdb.SelectSingleNode("//lis");
            XmlNode oldChild = node.SelectSingleNode("l[@skuid='" + skuid.ToString() + "']");
            if (oldChild != null)
            {
                cartdb.RemoveChild(oldChild);
                this.SaveShoppingCartData(cartdb);
            }
        }
        public virtual void Update(int skuid, int qty, int userid)
        {
            if (qty <= 0)
            {
                this.Remove(skuid, 0);
            }
            else
            {
                XmlDocument doc = this.GetShoppingCartData();
                XmlNode rnode = doc.SelectSingleNode("//lis");
                XmlNode node = rnode.SelectSingleNode("l[@skuid='" + skuid.ToString() + "']");
                if (node != null && !strUtil.IsNullOrEmpty(node.Value))
                {
                    //update
                    node.Attributes["qty"].Value = (cvt.ToInt(node.Attributes["qty"].Value) + qty).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                this.SaveShoppingCartData(doc);
            }
        }
        public virtual void Update(ShoppingCart cart)
        {
            if (cart.BuyQty <= 0)
            {
                this.Remove(cart.Item.Id, 0);
            }
            else
            {
                XmlDocument doc = this.GetShoppingCartData();
                XmlNode rnode = doc.SelectSingleNode("//lis");
                XmlNode node = rnode.SelectSingleNode("l[@skuid='" + cart.Item.Id.ToString() + "']");
                if (node != null && !strUtil.IsNullOrEmpty(node.Value))
                {
                    //update
                    node.Attributes["qty"].Value = (cvt.ToInt(node.Attributes["qty"].Value) + cart.BuyQty).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                this.SaveShoppingCartData(doc);
            }
        }
        private void SaveShoppingCartData(XmlDocument doc)
        {
            if (doc == null)
            {
                this.Clear(0);
            }
            else
            {
                wc.CookieSet(CartDataCookieName, wc.UrlEncode(doc.OuterXml));
            }
        }
        private XmlDocument GetShoppingCartData()
        {
            XmlDocument document = new XmlDocument();
            string cartValue = wc.CookieGet(CartDataCookieName);
            if ((cartValue == null) || strUtil.IsNullOrEmpty(cartValue))
            {
                return CreateEmptySchema();
            }
            try
            {
                document.LoadXml(wc.UrlDecode(cartValue));
            }
            catch
            {
                this.Clear(0);
                document = CreateEmptySchema();
            }
            return document;
        }
        private static XmlDocument CreateEmptySchema()
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml("<sc><lis></lis></sc>");
            return document;
        }
    }
}
