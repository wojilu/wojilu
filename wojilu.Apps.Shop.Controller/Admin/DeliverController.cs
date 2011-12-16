using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppBase;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;
using wojilu.DI;

namespace wojilu.Web.Controller.Shop.Admin {

    [App( typeof( ShopApp ) )]
    public class DeliverController : ControllerBase {
        public void List()
        {

            set("addLink", to(Add));
            set("sortAction", to(SaveSort));

            List<ShopDeliver> list = ShopDeliver.findAll();
            bindList("list", "data", list, BindLink);
        }

        private void BindLink(IBlock block, int id)
        {
            block.Set("data.LinkEdit", to(Edit, id));
            block.Set("data.LinkDelete", to(Delete, id));
        }
        //private void BindContactLink(IBlock block, int id)
        //{
        //    block.Set("data.LinkEdit", to(EditContact, id));
        //    block.Set("data.LinkDelete", to(DeleteContact, id));
        //}
        private void BindProviderLink(IBlock block, int id)
        {
            block.Set("data.LinkEdit", to(EditProvider, id));
            block.Set("data.LinkDelete", to(DeleteProvider, id));
        }


        [HttpPost]
        public virtual void SaveSort()
        {

            int id = ctx.PostInt("id");
            String cmd = ctx.Post("cmd");

            ShopDeliver data = ShopDeliver.findById(id);

            List<ShopDeliver> list = ShopDeliver.findAll();

            if (cmd == "up")
            {

                new SortUtil<ShopDeliver>(data, list).MoveUp();
                echoRedirect("ok");
            }
            else if (cmd == "down")
            {

                new SortUtil<ShopDeliver>(data, list).MoveDown();
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
            dropList("shopProvider", ShopDeliverProvider.findAll(), "Title=Id", "");
            editor("shopDeliver.Description", "", "200px");
            List<ShopPayment> list = ShopPayment.findAll();
            bindList("list", "payment", list);
        }
        private static readonly ILog logger = LogManager.GetLogger(typeof(DeliverController));

        [HttpPost]
        public void Create()
        {
            ShopDeliver fi = ctx.PostValue<ShopDeliver>();
            if (ctx.HasErrors)
            {
                run(Add);
                return;
            }
            if (!string.IsNullOrEmpty(ctx.PostHtml("shopProvider"))) fi.Provider = ShopDeliverProvider.findById(cvt.ToInt(ctx.PostHtml("shopProvider")));
            fi.SupportedPayment = ctx.PostHtml("shopDeliver.SupportedPayment");

            fi.insert();

            echoRedirect(lang("opok"), List);
        }

        public void Edit(int id)
        {
            target(Update, id);

            ShopDeliver lt = ShopDeliver.findById(id);
            bind(lt);
            dropList("shopProvider", ShopDeliverProvider.findAll(), "Title=Id", lt.Provider.Id);
            editor("shopDeliver.Description", lt.Description, "200px");
            List<ShopPayment> list = ShopPayment.findAll();
            IBlock block = getBlock("list");
            foreach (ShopPayment curr in list)
            {
                block.Bind("payment", curr);
                String chk = "checked=\"checked\"";
                if (lt.PaymentList != null)
                {
                    foreach (ShopPayment c in lt.PaymentList)
                    {
                        block.Set("shopDeliver.SupportedPayment", curr.Id == c.Id ? chk : "");
                    }
                }
                block.Next();
            }
        }

        [HttpPost]
        public void Update(int id)
        {

            ShopDeliver f = ShopDeliver.findById(id);
            f = ctx.PostValue(f) as ShopDeliver;
            if (f != null)
            {
                if (!string.IsNullOrEmpty(ctx.PostHtml("shopProvider"))) f.Provider = ShopDeliverProvider.findById(cvt.ToInt(ctx.PostHtml("shopProvider")));
                f.SupportedPayment = ctx.PostHtml("shopDeliver.SupportedPayment");
                f.update();
            }
            echoRedirect(lang("opok"), List);
        }

        [HttpDelete]
        public void Delete(int id)
        {

            ShopDeliver lt = ShopDeliver.findById(id);
            if (lt != null)
            {
                lt.delete();
                redirect(List);
            }

        }

        #region Provider View
        public void ListProvider()
        {
            set("addLink", to(AddProvider));
            set("sortAction", to(SaveProviderSort));
            List<ShopDeliverProvider> list = ShopDeliverProvider.findAll();
            bindList("list", "data", list, BindProviderLink);
        }
        public void EditProvider(int id)
        {
            target(UpdateProvider, id);
            ShopDeliverProvider lt = ShopDeliverProvider.findById(id);
            bind(lt);
            editor("shopDeliverProvider.Description", lt.Description, "200px");
        }
        public void AddProvider()
        {
            target(CreateProvider);
            editor("shopDeliverProvider.Description", "", "200px");
        }
        #region Provider httpPost

        [HttpPost]
        public virtual void SaveProviderSort()
        {

            int id = ctx.PostInt("id");
            String cmd = ctx.Post("cmd");

            ShopDeliverProvider data = ShopDeliverProvider.findById(id);

            List<ShopDeliverProvider> list = ShopDeliverProvider.findAll();

            if (cmd == "up")
            {

                new SortUtil<ShopDeliverProvider>(data, list).MoveUp();
                echoRedirect("ok");
            }
            else if (cmd == "down")
            {

                new SortUtil<ShopDeliverProvider>(data, list).MoveDown();
                echoRedirect("ok");
            }
            else
            {
                echoError(lang("exUnknowCmd"));
            }

        }
        [HttpPost]
        public void CreateProvider()
        {
            ShopDeliverProvider fi = ctx.PostValue<ShopDeliverProvider>();
            if (ctx.HasErrors)
            {
                run(AddProvider);
                return;
            }
            fi.insert();

            echoRedirect(lang("opok"), ListProvider);
        }
        [HttpPost]
        public void UpdateProvider(int id)
        {
            ShopDeliverProvider f = ShopDeliverProvider.findById(id);
            f = ctx.PostValue(f) as ShopDeliverProvider;
            if (f != null) f.update();
            echoRedirect(lang("opok"), ListProvider);
        }
        [HttpDelete]
        public void DeleteProvider(int id)
        {
            ShopDeliverProvider lt = ShopDeliverProvider.findById(id);
            if (lt != null)
            {
                lt.delete();
                redirect(ListProvider);
            }
        }
        #endregion
        #endregion

        #region Contacts View
        public void ListContact()
        {
            set("addLink", to(AddContact));
            set("sortAction", to(SaveContactSort));
            List<ShopDeliverContacts> list = ShopDeliverContacts.findAll();
            IBlock block = getBlock("list");
            foreach (ShopDeliverContacts contact in list)
            {
                block.Bind("data",contact);
                block.Set("data.IsDefault", contact.IsDefault==1 ? "√" : "×");
                block.Set("data.Enabled", contact.Enabled == 1 ? "√" : "×");
                block.Set("data.LinkEdit", to(EditContact, contact.Id));
                block.Set("data.LinkDelete", to(DeleteContact, contact.Id));
                block.Next();
            }
            //bindList("list", "data", list, BindContactLink);
        }
        public void EditContact(int id)
        {
            target(UpdateContact, id);
            ShopDeliverContacts lt = ShopDeliverContacts.findById(id);
            bind(lt);
            String chk = "checked=\"checked\"";
            set("shopDeliverContacts.IsDefault", lt.IsDefault == 1 ? chk : "");
            set("shopDeliverContacts.Enabled", lt.Enabled == 1 ? chk : "");
        }
        public void AddContact()
        {
            target(CreateContact);
        }
        #region Contact httpPost
        [HttpPost]
        public virtual void SaveContactSort()
        {

            int id = ctx.PostInt("id");
            String cmd = ctx.Post("cmd");

            ShopDeliverContacts data = ShopDeliverContacts.findById(id);

            List<ShopDeliverContacts> list = ShopDeliverContacts.findAll();

            if (cmd == "up")
            {

                new SortUtil<ShopDeliverContacts>(data, list).MoveUp();
                echoRedirect("ok");
            }
            else if (cmd == "down")
            {

                new SortUtil<ShopDeliverContacts>(data, list).MoveDown();
                echoRedirect("ok");
            }
            else
            {
                echoError(lang("exUnknowCmd"));
            }

        }
        [HttpPost]
        public void CreateContact()
        {
            ShopDeliverContacts fi = ctx.PostValue<ShopDeliverContacts>();
            if (ctx.HasErrors)
            {
                run(AddContact);
                return;
            }

            fi.Creator = (User)ctx.viewer.obj;
            fi.IsDefault = ctx.PostIsCheck("shopDeliverContacts.IsDefault");
            fi.Enabled = ctx.PostIsCheck("shopDeliverContacts.Enabled");

            fi.insert();

            echoRedirect(lang("opok"), ListContact);
        }
        [HttpPost]
        public void UpdateContact(int id)
        {
            ShopDeliverContacts f = ShopDeliverContacts.findById(id);
            f = ctx.PostValue(f) as ShopDeliverContacts;
            if (f != null)
            {
                f.IsDefault = ctx.PostIsCheck("shopDeliverContacts.IsDefault");
                f.Enabled = ctx.PostIsCheck("shopDeliverContacts.Enabled");
                f.update();
            }
            echoRedirect(lang("opok"), ListContact);
        }
        [HttpDelete]
        public void DeleteContact(int id)
        {
            ShopDeliverContacts lt = ShopDeliverContacts.findById(id);
            if (lt != null)
            {
                lt.delete();
                redirect(ListContact);
            }
        }
        #endregion
        #endregion
    }

}
