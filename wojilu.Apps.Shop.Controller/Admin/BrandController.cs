using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;
using wojilu.DI;
using wojilu.Common.AppBase;
using wojilu.Web.Utils;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Shop.Admin {

    [App( typeof( ShopApp ) )]
    public class BrandController : ControllerBase {
        public void List()
        {

            set("addLink", to(Add));
            set("sortAction", to(SaveSort));

            List<ShopBrand> list = ShopBrand.findAll();
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

            ShopBrand data = ShopBrand.findById(id);

            List<ShopBrand> list = ShopBrand.findAll();

            if (cmd == "up")
            {

                new SortUtil<ShopBrand>(data, list).MoveUp();
                echoRedirect("ok");
            }
            else if (cmd == "down")
            {

                new SortUtil<ShopBrand>(data, list).MoveDown();
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

            editor("shopBrand.Description", "", "200px");
            set("authInfo", ctx.web.GetAuthJson());
            set("uploadLink", to(SaveUpload));
        }
        private static readonly ILog logger = LogManager.GetLogger(typeof(BrandController));

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

        [HttpPost]
        public void Create()
        {
            ShopBrand fi = ctx.PostValue<ShopBrand>();
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

            ShopBrand lt = ShopBrand.findById(id);
            bind(lt);
            editor("shopBrand.Description", lt.Description, "200px");
        }

        [HttpPost]
        public void Update(int id)
        {

            ShopBrand f = ShopBrand.findById(id);
            f = ctx.PostValue(f) as ShopBrand;
            if (f != null) f.update();
            echoRedirect(lang("opok"), List);
        }

        [HttpDelete]
        public void Delete(int id)
        {

            ShopBrand lt = ShopBrand.findById(id);
            if (lt != null)
            {
                lt.delete();
                redirect(List);
            }

        }
    }

}
