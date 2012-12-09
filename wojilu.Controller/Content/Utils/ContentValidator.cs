/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Enum;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Content {


    public class ContentValidator {

        public static ContentPost SetValue( MvcContext ctx ) {

            ContentPost post = new ContentPost();
            post.Creator = (User)ctx.viewer.obj;
            post.CreatorUrl = ctx.viewer.obj.Url;

            post.OwnerId = ctx.owner.Id;
            post.OwnerUrl = ctx.owner.obj.Url;
            post.OwnerType = ctx.owner.obj.GetType().FullName;

            post.AppId = ctx.app.Id;
            post.CategoryId = ctx.GetInt( "categoryId" );

            SetPostValue( post, ctx );

            return post;
        }

        public static ContentPost SetValueBySection( ContentSection section, MvcContext ctx ) {
            ContentPost post = new ContentPost();
            post.Creator = (User)ctx.viewer.obj;
            post.CreatorUrl = ctx.viewer.obj.Url;

            post.OwnerId = ctx.owner.Id;
            post.OwnerUrl = ctx.owner.obj.Url;
            post.OwnerType = ctx.owner.obj.GetType().FullName;

            post.AppId = section.AppId;
            post.CategoryId = ctx.GetInt( "categoryId" );

            post.PageSection = section;

            SetPostValue( post, ctx );

            return post;
        }

        public static ContentPost ValidateTitleBody( ContentPost post, MvcContext ctx ) {

            if (strUtil.IsNullOrEmpty( post.Title ))
                ctx.errors.Add( lang.get( "exTitle" ) );

            if (strUtil.IsNullOrEmpty( post.Content ) && strUtil.IsNullOrEmpty( post.SourceLink ))
                ctx.errors.Add( ctx.controller.alang( "exContentLink" ) );

            return post;
        }

        public static ContentPost SetPostValue( ContentPost post, MvcContext ctx ) {

            post.Author = strUtil.CutString( ctx.Post( "Author" ), 100 );
            post.Title = strUtil.CutString( ctx.Post( "Title" ), 100 );
            post.TitleHome = strUtil.CutString( ctx.Post( "TitleHome" ), 100 );
            post.Content = ctx.PostHtml( "Content" );
            post.Summary = ctx.Post( "Summary" );
            post.SourceLink = strUtil.CutString( ctx.Post( "SourceLink" ), 250 );
            post.AccessStatus = cvt.ToInt( ctx.Post( "AccessStatus" ) );
            post.CommentCondition = cvt.ToInt( ctx.Post( "IsCloseComment" ) );

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

        public static ContentPost ValidateImg( ContentPost post, Boolean isUpload, MvcContext ctx ) {

            if (strUtil.IsNullOrEmpty( post.Title ))
                ctx.errors.Add( lang.get( "exName" ) );

            if (!(!strUtil.IsNullOrEmpty( post.ImgLink ) || isUpload))
                ctx.errors.Add( ctx.controller.alang( "exImgUrl" ) );

            post.CategoryId = PostCategory.Img;

            return post;
        }

        public static ContentSection SetSectionValueAndValidate( int layoutId, MvcContext ctx ) {
            ContentSection section = new ContentSection();
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

        public static ContentSection PopulateFeed( int layoutId, MvcContext ctx ) {
            
            ContentSection section = new ContentSection();
            section.AppId = ctx.app.Id;

            String layoutStr = layoutId.ToString();
            int rowId = cvt.ToInt( layoutStr.Substring( 0, layoutStr.Length - 1 ) );
            int columnId = cvt.ToInt( layoutStr.Substring( layoutStr.Length - 1, 1 ) );
            section.RowId = rowId;
            section.ColumnId = columnId;
            return section;
        }

        public static ContentSection ValidateSectionEdit( ContentSection section, MvcContext ctx ) {
            validateSectionPrivate( section, ctx );
            return section;
        }

        private static void validateSectionPrivate( ContentSection section, MvcContext ctx ) {
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

        public static ContentPost ValidateTalk( ContentPost post, MvcContext ctx ) {

            if (strUtil.IsNullOrEmpty( post.Author ))
                ctx.errors.Add( ctx.controller.alang( "exTalker" ) );

            if (strUtil.IsNullOrEmpty( post.Content ))
                ctx.errors.Add( lang.get( "exContent" ) );

            if (strUtil.IsNullOrEmpty( post.SourceLink ))
                ctx.errors.Add( lang.get( "exUrl" ) );

            return post;
        }

        public static ContentPost ValidateVideo( ContentPost post, MvcContext ctx ) {

            if (strUtil.IsNullOrEmpty( post.Title ))
                ctx.errors.Add( lang.get( "exName" ) );

            if (post.HasImg()==false)
                ctx.errors.Add( ctx.controller.alang( "exVideoImgUrl" ) );

            if (strUtil.IsNullOrEmpty( post.SourceLink ))
                ctx.errors.Add( ctx.controller.alang( "exVideoLink" ) );


            post.CategoryId = PostCategory.Video;
            post.TypeName = typeof( ContentVideo ).FullName;


            return post;
        }
    }
}

