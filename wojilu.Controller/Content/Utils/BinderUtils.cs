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
using wojilu.Web.Controller.Content.Admin.Section;
using wojilu.Web.Controller.Content.Section;
using wojilu.Apps.Content.Service;

namespace wojilu.Web.Controller.Content.Utils {

    public class BinderUtils {

        private static readonly ILog logger = LogManager.GetLogger( typeof( BinderUtils ) );

        public static readonly String iconPic = string.Format( "<img src=\"{0}\"/> ", strUtil.Join( sys.Path.Img, "img.gif" ) );
        public static readonly String iconVideo = string.Format( "<img src=\"{0}\"/> ", strUtil.Join( sys.Path.Img, "video.gif" ) );
        public static readonly String iconPoll = string.Format( "<img src=\"{0}\"/> ", strUtil.Join( sys.Path.Img, "poll.gif" ) );
        public static readonly String iconTalk = string.Format( "<img src=\"{0}\"/> ", strUtil.Join( sys.Path.Img, "s/comment.png" ) );
        public static readonly String iconText = string.Format( "<img src=\"{0}\"/> ", strUtil.Join( sys.Path.Img, "doc.gif" ) );

        public static readonly String iconAttachment = string.Format( "<img src=\"{0}\"/>", strUtil.Join( sys.Path.Img, "attachment.gif" ) );

        public static readonly String iconPicked = string.Format( "<img src=\"{0}star.gif\" />", sys.Path.Img );
        public static readonly String iconFocus = string.Format( "<img src=\"{0}sticky2.gif\" />", sys.Path.Img );

        public static String getTypeIcon( IPageAdminSection sectionController, ContentPost post ) {

            String typeIcon = sectionController.GetSectionIcon( post.SectionId );
            if (strUtil.HasText( typeIcon )) return typeIcon;

            if (post.TypeName == typeof( ContentVideo ).FullName) return iconVideo;
            if (post.TypeName == typeof( ContentImg ).FullName) return iconPic;
            if (post.TypeName == typeof( ContentPoll ).FullName) return iconPoll;
            if (post.HasImg()) return iconPic;

            return "";
        }

        public static String getPickedIcon( ContentPost post ) {

            if (post.PickStatus == PickStatus.Picked) return iconPicked;
            if (post.PickStatus == PickStatus.Focus) return iconFocus;
            return "";

        }

        public static void bindListItem( IBlock block, ContentPost post, MvcContext ctx ) {

            if (post.PageSection != null) {

                block.Set( "post.SectionName", post.PageSection.Title );
                block.Set( "post.SectionUrl", clink.toSection( post.PageSection.Id, ctx ) );

            }

            IPageAdminSection sectionController = BinderUtils.GetPageSectionAdmin( post, ctx, "AdminSectionShow" );

            String typeIcon = sectionController.GetSectionIcon( post.SectionId );

            block.Set( "post.ImgIcon", typeIcon );

            String att = post.Attachments > 0 ? "<img src=\"" + strUtil.Join( sys.Path.Img, "attachment.gif" ) + "\"/>" : "";
            block.Set( "post.AttachmentIcon", att );

            block.Set( "post.TitleCss", post.Style );

            block.Set( "post.Title", strUtil.SubString( post.GetTitle(), 50 ) );
            block.Set( "post.Created", post.Created.ToString( "yyyy-MM-dd HH:mm:ss" ) );
            block.Set( "post.Hits", post.Hits );
            block.Set( "post.Url", alink.ToAppData( post, ctx ) );

            if (post.Creator != null) {
                block.Set( "post.Submitter", string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", Link.ToMember( post.Creator ), post.Creator.Name ) );
            } else {
                block.Set( "post.Submitter", "" );
            }

        }

        /// <summary>
        /// 获取聚合区块对应的 BinderController。加载模板的时候，检查是否已经自定义了模板。
        /// </summary>
        /// <param name="section"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static ISectionBinder GetBinder( ContentSection section, MvcContext ctx ) {
            IContentSectionTemplateService TplService = ObjectContext.Create<IContentSectionTemplateService>( typeof( ContentSectionTemplateService ) );
            ContentSectionTemplate template = TplService.GetById( section.TemplateId );
            String binderName = string.Format( "wojilu.Web.Controller.Content.Binder.{0}BinderController", template.TemplateName );
            ControllerBase controller = ControllerFactory.FindController( binderName, ctx ) as ControllerBase;
            if (controller == null) throw new Exception( "ISectionBinder not found:" + binderName );

            String customTemplateStr = getCustomTemplateBody( section, ctx );

            // 自定义模板
            if (strUtil.HasText( customTemplateStr )) {
                controller.viewContent( customTemplateStr );
                return controller as ISectionBinder;
            } else {
                Template currentView = controller.utils.getTemplateByFileName( BinderUtils.GetBinderTemplatePath( template ) );
                controller.utils.setCurrentView( currentView );
                return controller as ISectionBinder;
            }
        }

        private static string getCustomTemplateBody( ContentSection section, MvcContext ctx ) {

            if (section.CustomTemplateId <= 0) return null;

            IContentCustomTemplateService ctService = ObjectContext.Create<IContentCustomTemplateService>( typeof( ContentCustomTemplateService ) );
            ContentCustomTemplate ct = ctService.GetById( section.CustomTemplateId, ctx.owner.Id );

            if (ct == null) return null;
            return ct.Content;
        }

        /// <summary>
        /// 获取当前区块对应的 SectionController。加载模板的时候，检查是否已经自定义了模板。
        /// </summary>
        /// <param name="section"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static IPageSection GetPageSection( ContentSection section, MvcContext ctx ) {
            return GetPageSection( section, ctx, null );
        }

        /// <summary>
        /// 获取当前区块对应的 SectionController。加载模板的时候，检查是否已经自定义了模板。
        /// </summary>
        /// <param name="section"></param>
        /// <param name="ctx"></param>
        /// <param name="currentView">需要加载的模板，默认是 SectionShow</param>
        /// <returns></returns>
        public static IPageSection GetPageSection( ContentSection section, MvcContext ctx, String currentView ) {

            if (section == null) return new NullSectionController();
            if (strUtil.IsNullOrEmpty( currentView )) currentView = "SectionShow";

            ControllerBase controller = ControllerFactory.FindController( section.SectionType, ctx ) as ControllerBase;
            if (controller == null) return new NullSectionController();

            String customTemplateStr = getCustomTemplateBody( section, ctx );
            if (strUtil.HasText( customTemplateStr )) {
                controller.viewContent( customTemplateStr );
            } else {
                controller.view( currentView );
            }
            return controller as IPageSection;
        }

        public static IPageAdminSection GetPageSectionAdmin( ContentPost post, MvcContext ctx, String currentView ) {

            if (post == null) return new NullSectionAdmin();
            if (post.PageSection == null) return new NullSectionAdmin();

            return GetPageSectionAdmin( post.PageSection, ctx, currentView );
        }

        public static IPageAdminSection GetPageSectionAdmin( ContentSection articleSection, MvcContext ctx, String currentView ) {
            if (articleSection == null) return new NullSectionAdmin();
            String adminSectionControllerName = getAdminControllerName( articleSection );
            ControllerBase controller = ControllerFactory.FindController( adminSectionControllerName, ctx ) as ControllerBase;
            if (controller == null) return new NullSectionAdmin();
            controller.view( currentView );
            return controller as IPageAdminSection;
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

        public static void bindPostSingle( IBlock block, ContentPost post, MvcContext ctx ) {

            block.Set( "post.TitleCss", post.Style );
            block.Set( "post.TitleFull", post.Title );

            if (strUtil.HasText( post.TitleHome )) {
                block.Set( "post.Title", post.TitleHome );
            }
            else {
                block.Set( "post.Title", post.Title );
            }

            if (post.PageSection != null) {
                block.Set( "post.SectionName", post.PageSection.Title );
                block.Set( "post.SectionUrl", clink.toSection( post.PageSection.Id, ctx ) );
            }

            block.Set( "post.Url", alink.ToAppData( post, ctx ) );

            block.Set( "post.Description", post.Content );

            block.Set( "post.Created", post.Created );
            block.Set( "post.CreatedDay", post.Created.ToShortDateString() );
            block.Set( "post.CreatedTime", post.Created.ToShortTimeString() );

            block.Set( "post.CreatorName", post.Creator == null ? "" : post.Creator.Name );
            block.Set( "post.CreatorLink", Link.ToMember( post.Creator ) );

            block.Bind( "post", post );

        }

        public static void bindMashupData( IBlock block, IBinderValue item, int itemIndex ) {
            bindMashupData( block, item, itemIndex, null );
        }

        public static void bindMashupData( IBlock block, IBinderValue item, int itemIndex, MvcContext ctx ) {

            block.Set( "post.ItemIndex", itemIndex );
            block.Set( "post.Title", item.Title );

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

            String lnk = getDataLink( item, ctx );
            block.Set( "post.Url", lnk );

            block.Bind( "post", item );
        }


        private static string getDataLink( IBinderValue item, MvcContext ctx ) {
            if (item == null) return "#";
            if (ctx == null) return item.Link;

            if (item.obj == null) return item.Link;

            ContentPost post = item.obj as ContentPost;
            if (post == null) return item.Link;

            return alink.ToAppData( post, ctx );
        }



    }

}
