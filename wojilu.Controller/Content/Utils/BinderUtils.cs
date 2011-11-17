/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Apps;
using wojilu.Web.Context;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Common;
using wojilu.DI;
using System.Collections.Generic;
using wojilu.Members.Sites.Domain;
using wojilu.Apps.Content.Enum;

namespace wojilu.Web.Controller.Content.Utils {

    public class BinderUtils {

        private static readonly ILog logger = LogManager.GetLogger( typeof( BinderUtils ) );

        public static readonly String iconPic = string.Format( "<img src=\"{0}\"/> ", strUtil.Join( sys.Path.Img, "img.gif" ) );
        public static readonly String iconVideo = string.Format( "<img src=\"{0}\"/> ", strUtil.Join( sys.Path.Img, "video.gif" ) );
        public static readonly String iconAttachment = string.Format( "<img src=\"{0}\"/>", strUtil.Join( sys.Path.Img, "attachment.gif" ) );

        public static readonly String iconPicked = string.Format( "<img src=\"{0}star.gif\" />", sys.Path.Img );
        public static readonly String iconFocus = string.Format( "<img src=\"{0}sticky2.gif\" />", sys.Path.Img );

        public static String getTypeIcon( ContentPost post ) {

            if (post.TypeName == typeof( ContentVideo ).FullName) return iconVideo;
            if (post.TypeName == typeof( ContentImg ).FullName) return iconPic;
            if (post.HasImg()) return iconPic;

            return "";
        }

        public static String getPickedIcon( ContentPost post ) {

            if (post.PickStatus == PickStatus.Picked) return iconPicked;
            if (post.PickStatus == PickStatus.Focus) return iconFocus;
            return "";

        }

        public static void bindListItem( IBlock block, ContentPost post, MvcContext ctx ) {
            block.Set( "post.SectionName", post.PageSection.Title );
            block.Set( "post.SectionUrl", ctx.to( new SectionController().Show, post.PageSection.Id ) );

            String typeIcon = BinderUtils.getTypeIcon( post );
            block.Set( "post.ImgIcon", typeIcon );

            String att = post.Attachments > 0 ? "<img src=\"" + strUtil.Join( sys.Path.Img, "attachment.gif" ) + "\"/>" : "";
            block.Set( "post.AttachmentIcon", att );

            block.Set( "post.TitleCss", post.Style );

            block.Set( "post.Title", strUtil.SubString( post.GetTitle(), 50 ) );
            block.Set( "post.Created", post.Created );
            block.Set( "post.Hits", post.Hits );
            block.Set( "post.Url", alink.ToAppData( post ) );

            if (post.Creator != null) {
                block.Set( "post.Submitter", string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", Link.ToMember( post.Creator ), post.Creator.Name ) );
            }
            else {
                block.Set( "post.Submitter", "" );
            }

        }

        public static ISectionBinder GetBinder( ContentSectionTemplate template, MvcContext ctx, Template currentView ) {

            String binderName = string.Format( "wojilu.Web.Controller.Content.Binder.{0}BinderController", template.TemplateName );

            ControllerBase controller = ControllerFactory.FindController( binderName, ctx ) as ControllerBase;
            if (controller == null) throw new Exception( "ISectionBinder not found:" + binderName );
            controller.utils.setCurrentView( currentView );
            return controller as ISectionBinder;
        }

        public static IPageSection GetPageSection( ContentSection articleSection, MvcContext ctx, String currentView ) {

            ControllerBase controller = ControllerFactory.FindController( articleSection.SectionType, ctx ) as ControllerBase;
            controller.view( currentView );
            return controller as IPageSection;
        }

        public static IPageSection GetPageSectionAdmin( ContentSection articleSection, MvcContext ctx, String currentView ) {
            String adminSectionControllerName = getAdminControllerName( articleSection );
            ControllerBase controller = ControllerFactory.FindController( adminSectionControllerName, ctx ) as ControllerBase;
            controller.view( currentView );
            return controller as IPageSection;
        }

        private static String getAdminControllerName( ContentSection articleSection ) {
            string[] typeItem = articleSection.SectionType.Split( '.' );
            String controllerName = typeItem[typeItem.Length - 1];
            String namespaceStr = strUtil.TrimEnd( articleSection.SectionType, "." + controllerName );
            String adminNamespace = getAdminNamespace( namespaceStr );
            return adminNamespace + "." + controllerName;
        }

        // wojilu.Web.Controller.Content.Section =>
        // wojilu.Web.Controller.Content.Admin.Section
        private static String getAdminNamespace( String namespaceStr ) {
            string[] item = namespaceStr.Split( '.' );
            String lastItem = item[item.Length - 1];
            String namespaceStart = strUtil.TrimEnd( namespaceStr, "." + lastItem );
            return namespaceStart + ".Admin." + lastItem;
        }

        // 得到视图view的模板文件
        public static String GetBinderTemplatePath( ContentSectionTemplate sectionTemplate ) {
            return "Content/Binder/" + sectionTemplate.TemplateName;
        }

        // 得到缩略图示例
        public static String GetBinderTemplateThumbPath() {
            return strUtil.Join( sys.Path.Img, "app/content/Binder/" );
        }

        // 得到缩略图示例
        public static String GetSectionTemplateThumbPath() {
            return strUtil.Join( sys.Path.Img, "app/content/section/" );
        }

        public static void bindPostSingle( IBlock block, ContentPost post ) {

            block.Set( "post.TitleCss", post.Style );
            block.Set( "post.TitleFull", post.Title );

            if (strUtil.HasText( post.TitleHome ))
                block.Set( "post.Title", post.TitleHome );
            else
                block.Set( "post.Title", post.Title );

            block.Set( "post.Url", alink.ToAppData( post ) );

            block.Set( "post.Description", post.Content );

            block.Set( "post.Created", post.Created );
            block.Set( "post.CreatedDay", post.Created.ToShortDateString() );
            block.Set( "post.CreatedTime", post.Created.ToShortTimeString() );

            block.Set( "post.CreatorName", post.Creator == null ? "" : post.Creator.Name );
            block.Set( "post.CreatorLink", Link.ToMember( post.Creator ) );

            block.Bind( "post", post );

        }

        public static void bindMashupData( IBlock block, IBinderValue item, int itemIndex ) {

            block.Set( "post.ItemIndex", itemIndex );
            block.Set( "post.Title", item.Title );
            block.Set( "post.Url", item.Link );

            block.Set( "post.Created", item.Created.Day );
            block.Set( "post.CreatedDay", item.Created.ToShortDateString() );
            block.Set( "post.CreatedTime", item.Created.ToShortTimeString() );

            block.Set( "post.CreatorName", item.CreatorName );
            block.Set( "post.CreatorLink", item.CreatorLink );
            block.Set( "post.CreatorPic", item.CreatorPic );

            block.Set( "post.Content", item.Content );
            block.Set( "post.Summary", strUtil.CutString( item.Content, 200 ) );
            block.Set( "post.PicUrl", item.PicUrl );
            block.Set( "post.Replies", item.Replies );

            block.Bind( "post", item );

        }



    }

}
