using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Shop.Admin.Section;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Shop.Utils;
using wojilu.DI;
using wojilu.Apps.Shop.Enum;
using wojilu.Web.Controller.Users.Admin;
using wojilu.Web.Utils;
using wojilu.Serialization;
using wojilu.Drawing;
using wojilu.Common.AppBase;
using wojilu.Common.Upload;
using wojilu.Web.Controller.Admin;

namespace wojilu.Web.Controller.Shop.Admin {

    [App( typeof( ShopApp ) )]
    public class ItemController : ControllerBase {

        public IShopItemService postService { get; set; }
        public IShopCategoryService classService { get; set; }
        public IShopSectionService sectionService { get; set; }
        public IAttachmentService attachService { get; set; }

        public ItemController() {
            classService = new ShopCategoryService();
            postService = new ShopItemService();
            sectionService = new ShopSectionService();
            attachService = new AttachmentService();
        }

        public void Category(int id)
        {
            ShopCategory cat = classService.GetById(id);
            if (cat.IsThumbView == 1)
            {
                view("ThumbList");
            }
            else
            {
                view("List");
            }

            set("addUrl", to(Add, id));

            set("OperationUrl", to(SaveAdmin));
            ShopApp app = ctx.app.obj as ShopApp;
            set("app.Name", ctx.app.Name);
            set("app.Link", to(new ShopController().Index));
            set("searchKey", "");

            set("tagAction", to(SaveTag));

            DataPage<ShopItem> pages = postService.GetByCategory(ctx.app.Id, id);
            bool isTrash = false;
            bindAdminList(pages, isTrash);
        }

        public void Add(int id)
        {
            target(to(Create) + "?typeId=" + ctx.GetInt("typeId") + "&fromList=" + ctx.GetInt("fromList"));
            editor("Content", "", "300px");

            List<ShopSection> sections = sectionService.GetInputSectionsByApp( ctx.app.Id );
            checkboxList( "skuSection", sections, "Title=Id", 0 );
            dropList("goodCategoryId", getRootList(), "Name=Id", "");
            dropList("goodProviderId", ShopSupplier.findAll(), "Title=Id", "");
            dropList("goodBrandId", ShopBrand.findAll(), "Title=Id", "");
            set("subCategoriesJson", classService.GetSubCatsJson());

            set( "created", DateTime.Now );

            set("sectionId", id);

            set("width", ctx.GetInt("width"));
            set("height", ctx.GetInt("height"));

            set( "uploadLink", to( new AttachmentController().SaveFlashFile ) ); // 接受上传的网址
            set( "imgUploadLink", to( SavePic ) ); //图片上传

            set("authJson", AdminSecurityUtils.GetAuthCookieJson(ctx));

            radioList( "PickStatus", PickStatus.GetPickStatus(), "0" );

        }

        public void Edit(int ItemId)
        {

            ShopItem post = postService.GetById(ItemId, ctx.owner.Id);
            if (post == null)
            {
                echo(lang("exDataNotFound"));
                return;
            }

            target(to(Update, ItemId) + "?typeId=" + ctx.GetInt("typeId"));

            bindEditInfo(post);

            List<ShopSection> sectionList = sectionService.GetInputSectionsByApp(ctx.app.Id);
            String sectionIds = sectionService.GetSectionIdsByPost(ItemId);

            checkboxList("skuSection", sectionList, "Title=Id", 0);
            set("sectionIds", sectionIds);

            //--------------------------上传信息----------------------------------

            //set( "uploadLink", to( new AttachmentController().SaveFlashFile ) ); // 接受上传的网址
            set("attachmentLink", to(new AttachmentController().AdminList, ItemId));

            set("imgUploadLink", to(SavePic)); //图片上传
            set("authJson", AdminSecurityUtils.GetAuthCookieJson(ctx));

        }

        public void EditImg(int ItemId)
        {
            view("Edit");
            this.Edit(ItemId);
        }

        private void bindEditInfo(ShopItem post)
        {

            if (post.PageSection == null) return;

            set("sku.DeleteUrl", to(Delete, post.Id));

            set("sku.Author", post.Author);
            set("sku.Title", post.Title);
            set("sku.TitleHome", strUtil.EncodeTextarea(post.TitleHome));

            set("sku.ItemSKU", post.ItemSKU);
            set("sku.ShortDescription", post.ShortDescription);
            set("sku.SalePrice", post.SalePrice);
            set("sku.CostPrice", post.CostPrice);
            set("sku.RetaPrice", post.RetaPrice);
            set("sku.MinOrderQty", post.MinOrderQty);
            set("sku.MaxOrderQty", post.MaxOrderQty);
            set("sku.UnitString", post.UnitString);
            set("sku.Weight", post.Weight);
            set("sku.CategoryId", post.CategoryId);

            dropList("goodCategoryId", getRootList(), "Name=Id", post.CategoryId > 0 ? classService.GetParentId(post.CategoryId) : 0);
            dropList("goodProviderId", ShopSupplier.findAll(), "Title=Id", post.Provider != null ? post.Provider.Id : 0);
            dropList("goodBrandId", ShopBrand.findAll(), "Title=Id", post.Brand != null ? post.Brand.Id : 0);

            set("subCategoriesJson", classService.GetSubCatsJson());

            set("sku.Width", post.Width);
            set("sku.Height", post.Height);

            editor("Content", strUtil.Edit(post.Content), "250px");

            set("sku.Created", post.Created);
            set("sku.Hits", post.Hits);
            set("sku.OrderId", post.OrderId);

            set("sku.RedirectUrl", post.RedirectUrl);
            set("sku.MetaKeywords", post.MetaKeywords);
            set("sku.MetaDescription", post.MetaDescription);


            set("sku.Summary", post.Summary);
            set("sku.SourceLink", post.SourceLink);
            set("sku.Style", post.Style);

            set("sku.ImgLink", post.GetImgUrl());
            set("sku.ImgThumbLink", post.GetImgThumb());
            set("sku.ImgDeleteLink", to(DeleteItemPic, post.Id));
            
            set("sku.TagList", post.Tag.TextString);
            String val = AccessStatusUtil.GetRadioList(post.AccessStatus);
            set("sku.AccessStatus", val);
            set("sku.IsCloseComment", Html.CheckBox("IsCloseComment", lang("closeComment"), "1", cvt.ToBool(post.CommentCondition)));

            radioList("PickStatus", PickStatus.GetPickStatus(), post.PickStatus.ToString());


            //bindUploadLink(post.PageSection.Id);

            set("attachmentAdminLink", to(new AttachmentController().AdminList, post.Id));
        }

        //private void bindUploadLink(int sectionId)
        //{
        //    set("uploadLink", to(Upload, sectionId));
        //    set("deleteUploadLink", to(DeleteUpload, sectionId));
        //}


        private List<ShopCategory> getRootList()
        {
            List<ShopCategory> list = classService.GetRootList();
            ShopCategory f = new ShopCategory
            {
                Id = 0,
                Name = lang("plsSelect")
            };
            list.Insert(0, f);
            return list;
        }

        //--------------------------------------------------------------------------------------------------------

        public void SavePic() {

            Dictionary<String, String> dic = new Dictionary<String, String>();

            HttpFile postedFile = ctx.GetFileSingle();

            TempUploadFileService uploadService = new TempUploadFileService();
            Result result = uploadService.Upload(postedFile);

            if (result.HasErrors)
            {

                dic.Add("PicUrl", "");
                dic.Add("Msg", result.ErrorsText);
                echoText(JsonString.ConvertDictionary(dic));
            }
            else
            {

                TempUploadFile tfile = result.Info as TempUploadFile;

                dic.Add("PicUrl", tfile.FileUrl);
                dic.Add("PicThumbUrl", tfile.FileThumbUrl);
                dic.Add("DeleteLink", to(DeleteTempPic, tfile.Id));

                echoText(JsonString.ConvertDictionary(dic));
            }

        }

        //public void DeletePic() {

        //    Object picInfo = ctx.web.SessionGet( "portal_temp_pic" );
        //    if (picInfo == null) {
        //        echoText( "图片失效，不存在" );
        //    }
        //    else {

        //        deleteTempPic( picInfo );
        //        echoAjaxOk();
        //    }
        //}

        public void DeleteItemPic(int id)
        {

            ShopItem post = postService.GetById(id, ctx.owner.Id);
            if (post == null)
            {
                echoText("data not found");
                return;
            }


            wojilu.Drawing.Img.DeleteImgAndThumb(post.GetImgUrl());
            echoAjaxOk();
        }

        public void DeleteTempPic(int id)
        {

            TempUploadFileService uploadService = new TempUploadFileService();
            Result result = uploadService.DeleteTempFile(id);

            if (result.HasErrors)
            {
                echoText(result.ErrorsText);
            }
            else
            {
                echoAjaxOk();
            }

        }

        //--------------------------------------------------------------------------------------------------------


        public void Create() {


            ShopItem post = ShopValidator.Validate( ctx );
            ShopValidator.ValidateArticle( post, ctx );

            String sectionIds = ctx.PostIdList("skuSection");
            if (strUtil.IsNullOrEmpty( sectionIds )) errors.Add( "请选择区块" );

            int[] arrAttachmentIds = cvt.ToIntArray( ctx.PostIdList( "attachmentIds" ) );

            // 图片默认值处理
            if (strUtil.HasText(post.ImgLink))
            {

                if (post.Width <= 0)
                {
                    post.Width = 100;
                    post.Height = 85;
                }
            }

            if (ctx.HasErrors)
            {
                echoError();
                return;
            }


            // 处理远程图片
            if (ctx.PostIsCheck( "isDowloadPic" ) == 1) {
                post.Content = wojilu.Net.PageLoader.ProcessPic( post.Content, "" );
            }

            postService.Insert( post, sectionIds, ctx.Post( "TagList" ) );
            attachService.UpdateAtachments( arrAttachmentIds, post );

            if (ctx.GetInt( "fromList" ) > 0) {
                echoRedirectPart( lang( "opok" ), to( List ), 1 );
            }
            else {
                echoToParentPart( lang( "opok" ) );
            }

        }

        [HttpPost, DbTransaction]
        public void Update(int ItemId)
        {
            ShopItem post = postService.GetById(ItemId, ctx.owner.Id);
            if (post == null)
            {
                echo(lang("exDataNotFound"));
                return;
            }

            String sectionIds = ctx.PostIdList("skuSection");

            ShopValidator.ValidateEdit(post, ctx);
            ShopValidator.ValidateArticle(post, ctx);
            if (errors.HasErrors)
            {
                run(Edit, ItemId);
            }
            else
            {
                if (ctx.PostIsCheck("saveContentPic") == 1)
                {
                    post.Content = wojilu.Net.PageLoader.ProcessPic(post.Content, null);
                }

                postService.Update(post, sectionIds, ctx.Post("TagList"));

                echoToParentPart(lang("opok"));
            }
        }

        //--------------------------------------------------------------------------------------------------------

        public void List() {

            set( "addUrl", to( Add,0 ) );

            set( "OperationUrl", to( SaveAdmin ) );
            ShopApp app = ctx.app.obj as ShopApp;
            set( "app.Name", ctx.app.Name );
            set( "app.Link", to( new ShopController().Index ) );
            set( "searchKey", "" );

            set( "tagAction", to( SaveTag ) );

            DataPage<ShopItem> posts = postService.GetByApp( ctx.app.Id, 50 );

            bool isTrash = false;
            bindAdminList( posts, isTrash );

            //bindCategories(app);

            target( Search );
        }

        //private void bindCategories( ShopApp app ) {
        //    List<ShopSection> sections = sectionService.GetInputSectionsByApp( app.Id );
        //    bindList( "cats", "category", sections );
        //}

        public void SaveTag() {

            int ItemId = ctx.PostInt( "ItemId" );
            String tagValue = ctx.Post( "tagValue" );

            if (strUtil.IsNullOrEmpty( tagValue )) {
                echoText( "请输入内容" );
                return;
            }

            ShopItem post = postService.GetById( ItemId, ctx.owner.Id );
            if (post == null) { echo( lang( "exDataNotFound" ) ); return; }

            post.Tag.Save( tagValue );

            echoAjaxOk();
        }

        public void EditSection( int id ) {
            target( UpdateSection, id );

            ShopItem post = postService.GetById( id, ctx.owner.Id );
            if (post == null) { echo( lang( "exDataNotFound" ) ); return; }
            if (post.PageSection == null) { echo( lang( "exDataNotFound" ) + ":post property PageSection" ); return; }
            List<ShopSection> sections = sectionService.GetInputSectionsByApp( ctx.app.Id );

            dropList( "SectionId", sections, "Title=Id", post.PageSection.Id );
        }

        public void UpdateSection( int id ) {

            int sectionId = ctx.PostInt( "SectionId" );
            ShopItem post = postService.GetById( id, ctx.owner.Id );
            if (post == null) { echo( lang( "exDataNotFound" ) ); return; }
            postService.UpdateSection( post, sectionId );

            echoToParentPart( lang( "opok" ) );
        }

        public void EditTitleStyle( int id ) {
            target( UpdateTitleStyle, id );
            load( "TextStyle", new FormController().TextStyle );
        }

        public void UpdateTitleStyle( int id ) {

            String titleStyle = strUtil.SqlClean( FormController.GetTitleStyle( ctx ), 100 );

            ShopItem post = postService.GetById( id, ctx.owner.Id );
            if (post == null) { echo( lang( "exDataNotFound" ) ); return; }
            postService.UpdateTitleStyle( post, titleStyle );
            echoToParentPart(lang("opok"));
        }

        public void Trash() {

            ShopApp app = ctx.app.obj as ShopApp;
            set( "app.Name", ctx.app.Name );
            set( "app.Link", to( new ShopController().Index ) );

            DataPage<ShopItem> posts = postService.GetTrashByApp( ctx.app.Id, 50 );

            bool isTrash = true;
            bindAdminList( posts, isTrash );

            target( Search );
        }

        public void Search() {

            view( "List" );

            ShopApp app = ctx.app.obj as ShopApp;
            set("app.Name", ctx.app.Name);
            set("app.Link", to(new ShopController().Index));


            String key = strUtil.SqlClean(ctx.Get("q"), 10);
            set("searchKey", key);

            target(Search);

            DataPage<ShopItem> posts = postService.GetBySearch(ctx.app.Id, key, 50);
            bool isTrash = false;
            bindAdminList(posts, isTrash);

        }

        [HttpPost, DbTransaction]
        public void SaveAdmin() {

            String ids = ctx.PostIdList( "choice" );

            if (strUtil.IsNullOrEmpty( ids )) {
                redirect( List );
                return;
            }

            String cmd = ctx.Post( "action" );


            if ("category" == cmd) {
                int sectionId = ctx.PostInt( "categoryId" );
                postService.UpdateSection( ids, sectionId );
            }
            else if ("deletetrue" == cmd) {
                postService.DeleteBatch( ids );
            }
            else if ("status_pick" == cmd) {
                postService.SetStatus_Pick( ids );
            }
            else if ("status_normal" == cmd) {
                postService.SetStatus_Normal( ids );
            }
            else if ("status_focus" == cmd) {
                postService.SetStatus_Focus( ids );
            }
            else if ("sale_pick" == cmd) {
                postService.SetOnSale(ids);
            }
            else if ("unsale_pick" == cmd) {
                postService.SetUnSale(ids);
            }

            echoAjaxOk();
        }



        private void bindAdminList(DataPage<ShopItem> skus, bool isTrash)
        {

            IBlock block = getBlock( "list" );


            foreach (ShopItem sku in skus.Results)
            {


                //String typeIcon = BinderUtils.getTypeIcon(sku);
                //String pickIcon = BinderUtils.getPickedIcon(sku);
                //String attIcon = sku.Attachments > 0 ? BinderUtils.iconAttachment : "";

                //block.Set("sku.ImgIcon", typeIcon);
                //block.Set("sku.PickIcon", pickIcon);
                //block.Set("sku.AttachmentIcon", attIcon);

                block.Set("sku.Title", strUtil.SubString(sku.GetTitle(), 50));
                block.Set("sku.TitleCss", sku.Style);
                block.Set("sku.TitleFull", sku.Title);

                //String sectionName = sku.PageSection == null ? "" : sku.PageSection.Title;
                //block.Set( "sku.SectionName", sectionName );

                block.Set("sku.ItemSKU", sku.ItemSKU);
                block.Set("sku.SalePrice", sku.SalePrice);
                block.Set("sku.RetaPrice", sku.RetaPrice);
                block.Set("sku.Url", sku.SourceLink);
                block.Set("sku.Link", alink.ToAppData(sku));
                block.Set("sku.PubDate", sku.Created);
                block.Set("sku.IsSale", sku.IsSale == 1 ? "<font color=\"red\">销售中</font>" : "未销售");
                if (sku.Creator != null)
                {
                    block.Set("sku.Submitter", string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", Link.ToMember(sku.Creator), sku.Creator.Name));
                }
                else {
                    block.Set("sku.Submitter", "无");
                }



                block.Bind("sku", sku);

                String lnkEdit = "";
                if (isTrash) {
                    lnkEdit = "#";
                }
                //else if (sku.HasImg()) {
                //    lnkEdit = to( new ListController().EditImg, sku.Id );
                //}
                else {
                    lnkEdit = to(Edit, sku.Id);
                }

                String lnkDelete = to(Delete, sku.Id);
                if (isTrash) lnkDelete = to(DeleteTrue, sku.Id);

                block.Set("sku.EditUrl", lnkEdit);
                block.Set("sku.DeleteUrl", lnkDelete);

                block.Set("sku.RestoreUrl", to(Restore, sku.Id));
                block.Set("sku.EditSectionUrl", to(EditSection, sku.Id));
                block.Set("sku.EditTitleStyleUrl", to(EditTitleStyle, sku.Id));

                block.Set("sku.AttachmentLink", to(new AttachmentController().AdminList, sku.Id));


                block.Next();
            }
            set("page", skus.PageBar);
        }



        [HttpDelete, DbTransaction]
        public void Delete( int ItemId ) {
            ShopItem post = postService.GetById( ItemId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            postService.Delete( post );

            echoRedirectPart(lang("opok"));
        }

        [HttpPut, DbTransaction]
        public void Restore( int id ) {

            postService.Restore( id );
            ShopItem post = postService.GetById( id, ctx.owner.Id );

            echoRedirectPart(lang("opok"));
        }

        [HttpDelete, DbTransaction]
        public void DeleteTrue( int ItemId ) {
            postService.DeleteTrue( ItemId );

            echoRedirectPart(lang("opok"));
        }

    }

}
