/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps;
using wojilu.ORM;

using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using System.Text;
using wojilu.Web.Controller.Shop.Utils;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Utils;

namespace wojilu.Web.Controller.Shop.Section {


    public partial class ListController : ControllerBase, IPageSection {


        private void bindSectionPosts( IList posts ) {
            IBlock block = getBlock( "list" );
            foreach (ShopItem post in posts) {

                BinderUtils.bindPostSingle( block, post );

                block.Next();
            }
        }


        private void bindPostList( ShopSection section, DataPage<ShopItem> posts, ShopSetting setting ) {
            set( "section.Name", section.Title );
            IBlock block = getBlock( "list" );
            foreach (ShopItem post in posts.Results) {

                BinderUtils.bindListItem( block, post, ctx );

                if (setting.ItemListMode == ItemListMode.TitleSummary)
                {
                    block.Set("sku.Summary", post.GetSummary(setting.SummaryLength));
                }

                block.Next();
            }
            set( "page", posts.PageBar );
        }


        private void bindDetail(int id, ShopItem sku)
        {

            set("sku.Title", sku.GetTitle());
            set("sku.Author", sku.Author);
            set("sku.Created", sku.Created);

            String tag = sku.Tag.List.Count > 0 ? tag = "tag: " + sku.Tag.HtmlString : "";
            String src = getSrc(sku);
            String replies = getReplies(sku);

            set("sku.Tag", tag);
            set("sku.Replies", replies);
            set("sku.Hits", sku.Hits);
            set("sku.Source", src);

            set("sku.ItemSKU", sku.ItemSKU);
            set("sku.ShortDescription", sku.ShortDescription);
            set("sku.SalePrice", sku.SalePrice);
            set("sku.CostPrice", sku.CostPrice);
            set("sku.RetaPrice", sku.RetaPrice);
            set("sku.MinOrderQty", sku.MinOrderQty);
            set("sku.MaxOrderQty", sku.MaxOrderQty);
            set("sku.UnitString", sku.UnitString);
            set("sku.Weight", sku.Weight);
            set("sku.Brand", "<a href=\"" + Link.To(new BrandController().Show, sku.Brand.Id) + "\">" + sku.Brand.Title + "</a>");

            set("addcartaction", to(new CartController().Add));//加入购物车
            set("sku.buySKU", sku.ItemSKU);
            set("sku.buyQty", sku.MinOrderQty > 0 ? sku.MinOrderQty : 1);
            set("sku.buyAttr", "");//TODO:产品属性
            set("sku.buyId", sku.Id);

            bindSummary(sku);
            if (sku.HasImg())
            {
                set("sku.ImgSrc", sku.GetImgMedium());
                set("sku.ThumImg", sku.GetImgThumb());
                set("sku.OrgImg", sku.GetImgUrl());
            }
            if (sku.HasImgList==0)
            {
                IBlock block = getBlock("albumlist");
                foreach (ShopItemImg skuimg in imgService.GetImgList(sku.Id))
                {
                    block.Set("album.ThumImg", skuimg.GetThumb());
                    block.Set("album.ImgSrc", skuimg.GetImgUrl());
                    block.Set("album.Description", skuimg.Description);
                    block.Next();
                }
            }
            String content;

            String pageSeparator = getPageSeparator(sku.Content);
            if (pageSeparator == null) {
                content = sku.Content;
            }
            else {
                content = getPagedContent(sku, pageSeparator);
            }

            if (sku.HasImg())
            {
                content = string.Format("<div style=\"text-align:center;\"><img src=\"{0}\" />", sku.GetImgMedium()) + "</div>" + content;
            }

            if (sku.IsAttachmentLogin == 1 && ctx.viewer.IsLogin == false)
            {
                content += "<div class=\"downloadWarning\"><div>" + alang( "downloadNeedLogin" ) + "</div></div>";
            }
            else {
                content = addAttachment(sku, content);
            }

            set("sku.Content", content);
        }

        private static readonly String staticPageSeparator = "<hr>"; //---page---

        private static String getPageSeparator( String content ) {
            if (content.IndexOf( staticPageSeparator ) > 20) return staticPageSeparator;
            if (content.IndexOf( staticPageSeparator.ToUpper() ) > 20) return staticPageSeparator.ToUpper(); //纠正IE的bug
            if (content.IndexOf( "<hr />" ) > 20) return "<hr />";
            return null;
        }

        private String getPagedContent( ShopItem post, String pageSeparator ) {
            String content;
            string[] ss = { pageSeparator };
            string[] arrContent = post.Content.Split( ss, StringSplitOptions.None );

            int currentPage = ctx.GetInt( "cp" );
            int pidx = currentPage - 1;
            if (pidx < 0) pidx = 0;

            content = arrContent[pidx];
            content = strUtil.CloseHtml( content );
            content += ObjectPage.GetPageBarByLink( alink.ToAppData( post ), arrContent.Length, pidx + 1 );
            return content;
        }

        private void bindSummary(ShopItem sku)
        {
            IBlock summaryBlock = getBlock( "summary" );
            if (strUtil.HasText(sku.Summary))
            {
                summaryBlock.Set("sku.Summary", sku.Summary);
                summaryBlock.Next();
            }
        }

        private String getReplies(ShopItem sku)
        {

            String replies = lang( "commentClosed" );

            if (sku.CommentCondition != CommentCondition.AllowAll) return replies;

            if (sku.Replies == 0)
            {
                replies = string.Format( "{0}:0", lang( "comment" ) );
            }
            else {
                replies = string.Format("{0}:{1} <a href=\"#comments\">{2}</a>", lang("comment"), sku.Replies, lang("viewByHit"));
            }
            return replies;
        }

        private String getSrc(ShopItem sku)
        {
            String src = null;
            if (strUtil.HasText(sku.SourceLink))
            {
                if (sku.SourceLink.ToLower().StartsWith("http:"))
                {
                    src = lang("src") + string.Format(": <a href=\"{0}\" target=\"_blank\">{0}</a>", sku.SourceLink);
                }
                else {
                    src = lang("src") + ": " + sku.SourceLink;
                }
            }
            return src;
        }




        private String addAttachment( ShopItem data, String content ) {

            if (data.Attachments <= 0) return content;

            List<ShopItemAttachment> attachList = attachmentService.GetAttachmentsByPost( data.Id );

            StringBuilder sb = new StringBuilder();
            String created = attachList[0].Created.ToString();
            sb.Append( "<div class=\"hr\"></div><div class=\"attachmentTitleWrap\"><div class=\"attachmentTitle\">" + alang( "attachment" ) + " <span class=\"note\">(" + created + ")</span> " );
            sb.Append( "</div></div><ul class=\"attachmentList\">" );

            foreach (ShopItemAttachment attachment in attachList) {

                string fileName = attachment.GetFileShowName();

                if (isImage( attachment )) {

                    sb.AppendFormat( "<li><div>{0} <span class=\"note\">({1}KB)</span></div>", fileName, attachment.FileSizeKB );
                    sb.AppendFormat( "<div><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" /></a></div>",
                        attachment.FileUrl, attachment.FileMediuUrl );
                    sb.Append( "</li>" );

                }
                else {


                    sb.AppendFormat( "<li><div>{0} <span class=\"note right10\">({1}KB)</span>", fileName, attachment.FileSizeKB );
                    sb.AppendFormat( "<img src=\"{1}\" /><a href=\"{0}\" target=\"_blank\">" + alang( "hitDownload" ) + "</a></div>", to( new ShopAttachmentController().Show, attachment.Id ) + "?id=" + attachment.Guid, strUtil.Join( sys.Path.Img, "/s/download.png" ) );
                    sb.Append( "</li>" );
                }
            }
            sb.Append( "</ul>" );

            content = string.Format( "<div>{0}</div><div id=\"attachmentPanel\">{1}</div>", content, sb.ToString() );

            return content;
        }

        private Boolean isImage( ShopItemAttachment attachment ) {
            return Uploader.IsImage( attachment.Type );
        }


    }
}

