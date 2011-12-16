using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;
using wojilu.DI;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Shop.Admin {

    [App( typeof( ShopApp ) )]
    public class SupplierController : ControllerBase
    {
        public void List()
        {

            set("addLink", to(Add));
            set("sortAction", to(SaveSort));

            List<ShopSupplier> list = ShopSupplier.findAll();
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

            ShopSupplier data = ShopSupplier.findById(id);

            List<ShopSupplier> list = ShopSupplier.findAll();

            if (cmd == "up")
            {

                new SortUtil<ShopSupplier>(data, list).MoveUp();
                echoRedirect("ok");
            }
            else if (cmd == "down")
            {

                new SortUtil<ShopSupplier>(data, list).MoveDown();
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
            ShopSupplier fi = ctx.PostValue<ShopSupplier>();
            if (ctx.HasErrors)
            {
                run(Add);
                return;
            }

            fi.OwnerId = ctx.owner.Id;
            fi.OwnerType = ctx.owner.obj.GetType().FullName;
            fi.OwnerUrl = ctx.owner.obj.Url;
            fi.GoodCounts = 0;
            fi.AppId = ctx.app.Id;
            fi.Creator = (User)ctx.viewer.obj;
            fi.CreatorUrl = ctx.viewer.obj.Url;

            fi.Ip = ctx.Ip;

            fi.insert();

            echoRedirect(lang("opok"), List);
        }

        public void Edit(int id)
        {
            target(Update, id);
            ShopSupplier lt = ShopSupplier.findById(id);
            bind(lt);
        }

        [HttpPost]
        public void Update(int id)
        {
            ShopSupplier f = ShopSupplier.findById(id);
            f = ctx.PostValue(f) as ShopSupplier;
            if (f != null) f.update();
            echoRedirect(lang("opok"), List);
        }

        [HttpDelete]
        public void Delete(int id)
        {

            ShopSupplier lt = ShopSupplier.findById(id);
            if (lt != null)
            {
                lt.delete();
                redirect(List);
            }

        }
    }

}
