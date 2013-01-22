/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Content.Admin {

    public class LayoutController : ControllerBase {

        public override void Layout() {

            ContentApp app = ctx.app.obj as ContentApp;

            set( "allPostsLink", to( new Common.PostController().List, 0 ) );
            set( "trashPostsLink", to( new Common.PostController().Trash ) );
            set( "settingLink", to( new SettingController().Index ) );
            set( "defaultLink", to( new ContentController().Home ) );

            set( "commentLink", to( new CommentController().List ) );

            IBlock htmlBlock = getBlock( "html" );
            if (ctx.owner.obj is Site) {
                htmlBlock.Set( "staticLink", to( new HtmlController().Index ) );
                htmlBlock.Next();
            }

            if (app.GetSettingsObj().EnableSubmit == 1) {
                String slnk = string.Format( "<li style=\"width:100px;\"><a href=\"{0}\" class=\"frmLink\" loadTo=\"adminPortalContainer\" nolayout=3>{1}</a><span></span></li>",
                    to( new SubmitSettingController().List ),
                    "投递员管理" );
                set( "submitterLink", slnk );
            }
            else {
                set( "submitterLink", "" );
            }

            String appStyle = app.Style == null ? "" : app.Style.Replace( "display:none;", "" );
            String skinStyle = app.SkinStyle == null ? "" : app.SkinStyle.Replace( "display:none;", "" );

            set( "app.Style", appStyle );
            set( "app.SkinStyle", skinStyle );

            StringBuilder sb = new StringBuilder();
            if (ctx.owner.obj is Site) {
                //sb.AppendLine( "#adminPortalContainer {width: 1000px;}" );
                //sb.AppendLine( "#portalAdminNav,#portalAdminNavWrap,.tabMain {width:1030px;}" );
                //sb.AppendLine( "#toggleSidebar { display:none;}" );
                sb.AppendLine( "#adminPortalContainer {width: 100%;}" );
                sb.AppendLine( "#portalAdminNav,#portalAdminNavWrap,.tabMain {width:100%;}" );
                sb.AppendLine( "#toggleSidebar { display:none;}" );
            }
            set( "portalWrapCss", sb );

        }
    }

}
