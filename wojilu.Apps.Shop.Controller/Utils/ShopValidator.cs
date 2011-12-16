/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Enum;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Shop {


    public class ShopValidator
    {

        public static ShopItem Validate( MvcContext ctx ) {

            ShopItem post = new ShopItem();
            post.Creator = (User)ctx.viewer.obj;
            post.CreatorUrl = ctx.viewer.obj.Url;
            post.OwnerId = ctx.owner.Id;
            post.OwnerUrl = ctx.owner.obj.Url;
            post.OwnerType = ctx.owner.obj.GetType().FullName;
            post.AppId = ctx.app.Id;

            ValidateEdit( post, ctx );

            return post;
        }

        public static ShopItem Validate( ShopSection section, MvcContext ctx ) {
            ShopItem post = new ShopItem();
            post.Creator = (User)ctx.viewer.obj;
            post.CreatorUrl = ctx.viewer.obj.Url;
            post.OwnerId = ctx.owner.Id;
            post.OwnerUrl = ctx.owner.obj.Url;
            post.OwnerType = ctx.owner.obj.GetType().FullName;
            post.AppId = section.AppId;
            post.PageSection = section;
            //post.CategoryId = ctx.GetInt( "categoryId" );

            post.Width = ctx.PostInt( "Width" );
            post.Height = ctx.PostInt( "Height" );


            ValidateEdit( post, ctx );

            if (strUtil.IsNullOrEmpty( post.Title )) post.Title = section.Title + " " + DateTime.Now.ToShortDateString();

            return post;
        }

        public static ShopItem ValidateArticle( ShopItem post, MvcContext ctx ) {

            if (strUtil.IsNullOrEmpty( post.Title ))
                ctx.errors.Add( lang.get( "exTitle" ) );

            if (strUtil.IsNullOrEmpty( post.Content) && strUtil.IsNullOrEmpty( post.SourceLink ))
                ctx.errors.Add( ctx.controller.alang( "exContentLink" ) );

            return post;
        }

        public static ShopItem ValidateEdit( ShopItem post, MvcContext ctx ) {

            post.Author = strUtil.CutString( ctx.Post( "Author" ), 100 );
            post.Title = strUtil.CutString( ctx.Post( "Title" ), 100 );
            post.TitleHome = strUtil.CutString( ctx.Post( "TitleHome" ), 100 );
            post.Content = ctx.PostHtml("Content");
            post.Summary = ctx.Post( "Summary" );
            post.SourceLink = strUtil.CutString( ctx.Post( "SourceLink" ), 250 );
            post.AccessStatus = cvt.ToInt( ctx.Post( "AccessStatus" ) );
            post.CommentCondition = cvt.ToInt( ctx.Post( "IsCloseComment" ) );
            post.UnitString = ctx.PostHtml("UnitString");
            post.Weight = cvt.ToInt(ctx.Post("Weight"));
            post.SalePrice = cvt.ToDecimal(ctx.Post("SalePrice"));
            post.RetaPrice = cvt.ToDecimal(ctx.Post("RetaPrice"));
            post.CostPrice = cvt.ToDecimal(ctx.Post("CostPrice"));
            post.CategoryId = cvt.ToInt(ctx.PostHtml("CategoryId"));
            post.IsGift = cvt.ToInt(ctx.Post("IsGift"));
            post.IsSale = cvt.ToInt(ctx.Post("isSale"));
            post.ItemSKU = ctx.PostHtml("ItemSKU");
            post.MethodId = ctx.GetInt("typeId"); ;
            post.MaxOrderQty = cvt.ToInt(ctx.Post("MaxOrderQty"));
            post.MinOrderQty = cvt.ToInt(ctx.Post("MinOrderQty"));
            post.OrderTimes = 0;
            post.Provider = ShopSupplier.findById(cvt.ToInt(ctx.PostHtml("goodProviderId")));
            post.Brand = ShopBrand.findById(cvt.ToInt(ctx.PostHtml("goodBrandId")));
            post.ShortDescription = ctx.PostHtml("ShortDescription");

            post.Hits = ctx.PostInt( "Hits" );
            post.Created = ctx.PostTime( "Created" );

            post.MetaKeywords = strUtil.CutString( ctx.Post( "MetaKeywords" ), 250 );
            post.MetaDescription = strUtil.CutString( ctx.Post( "MetaDescription" ), 250 );
            post.RedirectUrl = strUtil.CutString( ctx.Post( "RedirectUrl" ), 250 );
            post.PickStatus = ctx.PostInt( "PickStatus" );

            post.SaveStatus = 0;
            post.Ip = ctx.Ip;
            post.Style = strUtil.CutString( ctx.Post( "Style" ), 250 );
            post.OrderId = ctx.PostInt( "OrderId" );

            post.ImgLink = sys.Path.GetPhotoRelative( ctx.Post( "ImgLink" ) );

            post.Width = ctx.PostInt( "Width" );
            post.Height = ctx.PostInt( "Height" );

            return post;
        }

        public static ShopItem ValidateImg( ShopItem post, Boolean isUpload, MvcContext ctx ) {

            if (strUtil.IsNullOrEmpty( post.Title ))
                ctx.errors.Add( lang.get( "exName" ) );

            if (!(!strUtil.IsNullOrEmpty( post.ImgLink ) || isUpload))
                ctx.errors.Add( ctx.controller.alang( "exImgUrl" ) );

            return post;
        }

        public static ShopSection ValidateSection( int layoutId, MvcContext ctx ) {
            ShopSection section = new ShopSection();
            section.AppId = ctx.app.Id;

            String layoutStr = layoutId.ToString();

            int rowId = cvt.ToInt( layoutStr.Substring( 0, layoutStr.Length - 1 ) );
            int columnId = cvt.ToInt( layoutStr.Substring( layoutStr.Length - 1, 1 ) );

            section.RowId = rowId;
            section.ColumnId = columnId;
            if (ctx.PostInt( "serviceType" ) == 0) {
                int serviceId = ctx.PostInt( "serviceId" );
                section.ServiceId = serviceId;
                section.TemplateId = ctx.PostInt( "templateId" );
            }
            section.SectionType = ctx.Post( "SectionType" );
            validateSectionPrivate( section, ctx );
            return section;
        }

        public static ShopSection PopulateFeed( int layoutId, MvcContext ctx ) {
            
            ShopSection section = new ShopSection();
            section.AppId = ctx.app.Id;

            String layoutStr = layoutId.ToString();
            int rowId = cvt.ToInt( layoutStr.Substring( 0, layoutStr.Length - 1 ) );
            int columnId = cvt.ToInt( layoutStr.Substring( layoutStr.Length - 1, 1 ) );
            section.RowId = rowId;
            section.ColumnId = columnId;
            return section;
        }

        public static ShopSection ValidateSectionEdit( ShopSection section, MvcContext ctx ) {
            validateSectionPrivate( section, ctx );
            return section;
        }

        private static void validateSectionPrivate( ShopSection section, MvcContext ctx ) {
            section.Title = ctx.Post( "Title" );
            section.MoreLink = strUtil.CutString( ctx.PostHtml( "MoreLink" ), 250 );
            section.TemplateId = ctx.PostInt( "templateId" );

            if (strUtil.IsNullOrEmpty( section.Title )) {
                ctx.errors.Add( lang.get( "exName" ) );
            }
            else {
                section.Title = strUtil.SubString( section.Title, 50 );
            }

        }
    }
}

