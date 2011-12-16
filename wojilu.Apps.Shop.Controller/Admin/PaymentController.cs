using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Common.AppBase;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;
using wojilu.DI;
using wojilu.Web.Utils;

namespace wojilu.Web.Controller.Shop.Admin {

    [App( typeof( ShopApp ) )]
    public class PaymentController : ControllerBase
    {
        public IShopPaymentService PaymentSvr { get; set; }

        public PaymentController()
        {
            PaymentSvr = new ShopPaymentService();
        }

        public void List()
        {

            set("addLink", to(Add));
            set("sortAction", to(SaveSort));

            List<ShopPayment> list = ShopPayment.findAll();
            bindList("list", "data", list, BindLink);
        }

        private void BindLink(IBlock block, int id)
        {
            block.Set("data.LinkEdit", to(Edit, id));
            block.Set("data.LinkDelete", to(Delete, id));
        }

        [HttpPost]
        public virtual void SaveSort()
        {

            int id = ctx.PostInt("id");
            String cmd = ctx.Post("cmd");

            ShopPayment data = ShopPayment.findById(id);

            List<ShopPayment> list = ShopPayment.findAll();

            if (cmd == "up")
            {

                new SortUtil<ShopPayment>(data, list).MoveUp();
                echoRedirect("ok");
            }
            else if (cmd == "down")
            {

                new SortUtil<ShopPayment>(data, list).MoveDown();
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
            editor("shopPayment.Description", "", "200px");
            dropList("shopPayGatewayId", PaymentSvr.FindGatewayAll(), "DisplayName=Id", "");
            List<ShopCurrency> list = PaymentSvr.FindCurrencyAll();
            bindList("list", "data", list);
        }
        private static readonly ILog logger = LogManager.GetLogger(typeof(PaymentController));

        [HttpPost]
        public void Create()
        {
            ShopPayment fi = ctx.PostValue<ShopPayment>();
            if (ctx.HasErrors)
            {
                run(Add);
                return;
            }
            fi.GatewayId = cvt.ToInt(ctx.PostHtml("shopPayGatewayId"));
            fi.Gateway = fi.PaymentGateway.Name;
            fi.Logo = fi.PaymentGateway.Logo;
            fi.SupportedCurrency = ctx.PostHtml("shopPayment.SupportedCurrency");

            fi.insert();

            echoRedirect(lang("opok"), List);
        }

        public void Edit(int id)
        {
            target(Update, id);

            ShopPayment lt = ShopPayment.findById(id);
            bind(lt);
            editor("shopPayment.Description", lt.Description, "200px");
            dropList("shopPayGatewayId", PaymentSvr.FindGatewayAll(), "DisplayName=Id", lt.GatewayId);

            List<ShopCurrency> list = PaymentSvr.FindCurrencyAll();
            IBlock block = getBlock("list");
            foreach (ShopCurrency curr in list)
            {
                block.Bind("data", curr);
                String chk = "checked=\"checked\"";
                if (lt.CurrencyList != null)
                {
                    foreach (ShopCurrency c in lt.CurrencyList)
                    {
                        block.Set("shopPayment.SupportedCurrency", curr.Id == c.Id ? chk : "");
                    }
                }
                block.Next();
            }
        }

        [HttpPost]
        public void Update(int id)
        {

            ShopPayment f = ShopPayment.findById(id);
            f = ctx.PostValue(f) as ShopPayment;
            if (f != null)
            {
                f.GatewayId = cvt.ToInt(ctx.PostHtml("shopPayGatewayId"));
                f.Gateway = f.PaymentGateway.Name;
                f.Logo = f.PaymentGateway.Logo;
                f.SupportedCurrency = ctx.PostHtml("shopPayment.SupportedCurrency");
                f.update();
            }
            echoRedirect(lang("opok"), List);
        }

        [HttpDelete]
        public void Delete(int id)
        {

            ShopPayment lt = ShopPayment.findById(id);
            if (lt != null)
            {
                lt.delete();
                redirect(List);
            }

        }


        #region Gateway View
        public void ListGateway()
        {
            set("addLink", to(AddGateway));
            set("sortAction", to(SaveGatewaySort));

            List<ShopPaymentGateway> list = PaymentSvr.FindGatewayAll();
            IBlock block = getBlock("list");
            foreach (ShopPaymentGateway gateway in list)
            {
                block.Bind("data", gateway);
                block.Set("data.Enabled", gateway.Enabled == 1 ? "√" : "×");
                block.Set("data.LinkEdit", to(EditGateway, gateway.Id));
                block.Set("data.LinkDelete", to(DeleteGateway, gateway.Id));
                block.Next();
            }
            //bindList("list", "data", list, BindGatewayLink);
        }
        public void EditGateway(int id)
        {
            target(UpdateGateway, id);
            ShopPaymentGateway lt = PaymentSvr.FindGatewayById(id);
            bind(lt);
            String chk = "checked=\"checked\"";
            set("shopPaymentGateway.SellerAccount", lt.SellerAccount == 1 ? chk : "");
            set("shopPaymentGateway.EmailAddress", lt.EmailAddress == 1 ? chk : "");
            set("shopPaymentGateway.Password", lt.Password == 1 ? chk : "");
            set("shopPaymentGateway.PrimaryKey", lt.PrimaryKey == 1 ? chk : "");
            set("shopPaymentGateway.SecondKey", lt.SecondKey == 1 ? chk : "");
            set("shopPaymentGateway.Partner", lt.Partner == 1 ? chk : "");
            set("shopPaymentGateway.Enabled", lt.Enabled == 1 ? chk : "");
            editor("shopPaymentGateway.Description", lt.Description, "200px");
        }
        public void AddGateway()
        {
            target(CreateGateway);
            editor("shopPaymentGateway.Description", "", "200px");
        }
        #region Gateway httpPost
        [HttpPost]
        public virtual void SaveGatewaySort()
        {

            int id = ctx.PostInt("id");
            String cmd = ctx.Post("cmd");

            ShopPaymentGateway data = PaymentSvr.FindGatewayById(id);

            List<ShopPaymentGateway> list = PaymentSvr.FindGatewayAll();

            if (cmd == "up")
            {

                new SortUtil<ShopPaymentGateway>(data, list).MoveUp();
                echoRedirect("ok");
            }
            else if (cmd == "down")
            {

                new SortUtil<ShopPaymentGateway>(data, list).MoveDown();
                echoRedirect("ok");
            }
            else
            {
                echoError(lang("exUnknowCmd"));
            }

        }
        [HttpPost]
        public void CreateGateway()
        {
            ShopPaymentGateway fi = ctx.PostValue<ShopPaymentGateway>();
            if (ctx.HasErrors)
            {
                run(AddGateway);
                return;
            }
            fi.SellerAccount = ctx.PostIsCheck("shopPaymentGateway.SellerAccount");
            fi.EmailAddress = ctx.PostIsCheck("shopPaymentGateway.EmailAddress");
            fi.Password = ctx.PostIsCheck("shopPaymentGateway.Password");
            fi.PrimaryKey = ctx.PostIsCheck("shopPaymentGateway.PrimaryKey");
            fi.SecondKey = ctx.PostIsCheck("shopPaymentGateway.SecondKey");
            fi.Partner = ctx.PostIsCheck("shopPaymentGateway.Partner");
            fi.Enabled = ctx.PostIsCheck("shopPaymentGateway.Enabled");
            fi.insert();

            echoRedirect(lang("opok"), ListGateway);
        }
        [HttpPost]
        public void UpdateGateway(int id)
        {
            ShopPaymentGateway f = PaymentSvr.FindGatewayById(id);
            f = ctx.PostValue(f) as ShopPaymentGateway;
            if (f != null)
            {
                f.SellerAccount = ctx.PostIsCheck("shopPaymentGateway.SellerAccount");
                f.EmailAddress = ctx.PostIsCheck("shopPaymentGateway.EmailAddress");
                f.Password = ctx.PostIsCheck("shopPaymentGateway.Password");
                f.PrimaryKey = ctx.PostIsCheck("shopPaymentGateway.PrimaryKey");
                f.SecondKey = ctx.PostIsCheck("shopPaymentGateway.SecondKey");
                f.Partner = ctx.PostIsCheck("shopPaymentGateway.Partner");
                f.Enabled = ctx.PostIsCheck("shopPaymentGateway.Enabled");
                f.update();
            }
            echoRedirect(lang("opok"), ListGateway);
        }
        [HttpDelete]
        public void DeleteGateway(int id)
        {
            ShopPaymentGateway lt = PaymentSvr.FindGatewayById(id);
            if (lt != null)
            {
                lt.delete();
                redirect(ListGateway);
            }
        }
        #endregion
        #endregion


        #region Currency View
        public void ListCurrency()
        {
            set("addLink", to(AddCurrency));
            set("sortAction", to(SaveCurrencySort));

            List<ShopCurrency> list = PaymentSvr.FindCurrencyAll();
            IBlock block = getBlock("list");
            foreach (ShopCurrency curr in list)
            {
                block.Bind("data", curr);
                block.Set("data.Enabled", curr.Enabled == 1 ? "√" : "×");
                block.Set("data.IsDefault", curr.IsDefault == 1 ? "√" : "×");
                block.Set("data.LinkEdit", to(EditCurrency, curr.Id));
                block.Set("data.LinkDelete", to(DeleteCurrency, curr.Id));
                block.Next();
            }
        }
        public void EditCurrency(int id)
        {
            target(UpdateCurrency, id);
            ShopCurrency lt = PaymentSvr.FindCurrencyById(id);
            bind(lt);
            set("authInfo", ctx.web.GetAuthJson());
            set("uploadLink", to(SaveUpload));
            String chk = "checked=\"checked\"";
            set("shopCurrency.IsDefault", lt.IsDefault == 1 ? chk : "");
            set("shopCurrency.Enabled", lt.Enabled == 1 ? chk : "");
        }
        public void AddCurrency()
        {
            target(CreateCurrency);
            set("authInfo", ctx.web.GetAuthJson());
            set("uploadLink", to(SaveUpload));
        }
        #region Currency httpPost
        [HttpPost]
        public virtual void SaveCurrencySort()
        {

            int id = ctx.PostInt("id");
            String cmd = ctx.Post("cmd");

            ShopCurrency data = PaymentSvr.FindCurrencyById(id);

            List<ShopCurrency> list = PaymentSvr.FindCurrencyAll();

            if (cmd == "up")
            {

                new SortUtil<ShopCurrency>(data, list).MoveUp();
                echoRedirect("ok");
            }
            else if (cmd == "down")
            {

                new SortUtil<ShopCurrency>(data, list).MoveDown();
                echoRedirect("ok");
            }
            else
            {
                echoError(lang("exUnknowCmd"));
            }

        }
        [HttpPost]
        public void CreateCurrency()
        {
            ShopCurrency fi = ctx.PostValue<ShopCurrency>();
            if (ctx.HasErrors)
            {
                run(AddCurrency);
                return;
            }
            fi.IsDefault = ctx.PostIsCheck("shopCurrency.IsDefault");
            fi.Enabled = ctx.PostIsCheck("shopCurrency.Enabled");
            fi.insert();

            echoRedirect(lang("opok"), ListCurrency);
        }
        [HttpPost]
        public void UpdateCurrency(int id)
        {
            ShopCurrency f = PaymentSvr.FindCurrencyById(id);
            f = ctx.PostValue(f) as ShopCurrency;
            if (f != null)
            {
                f.IsDefault = ctx.PostIsCheck("shopCurrency.IsDefault");
                f.Enabled = ctx.PostIsCheck("shopCurrency.Enabled");
                f.update();
            }
            echoRedirect(lang("opok"), ListCurrency);
        }
        [HttpDelete]
        public void DeleteCurrency(int id)
        {
            ShopCurrency lt = PaymentSvr.FindCurrencyById(id);
            if (lt != null)
            {
                lt.delete();
                redirect(ListCurrency);
            }
        }
        [HttpPost]
        public void SaveUpload()
        {

            logger.Error("begin SaveUpload");

            Result result = Uploader.SaveFile(ctx.GetFileSingle());

            String fileName = result.Info.ToString();
            String fileUrl = strUtil.Join(sys.Path.Photo, fileName); // 获取文件完整路径
            echoText(fileUrl);

            logger.Error("end SaveUpload, fileUrl=" + fileUrl);
        }
        #endregion
        #endregion
    }

}
