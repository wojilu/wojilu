/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Content.Caching;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Controller.Content.Admin;
using System.Collections.Generic;

namespace wojilu.Web.Controller.Content {

    public class LayoutController : ControllerBase {

        public override void Layout() {

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSkin s = ContentSkin.findById( app.SkinId );
            set( "skinPath", strUtil.Join( sys.Path.Skin, s.StylePath ) );

            set( "adminUrl", to( new Admin.ContentController().Index ) );
            set( "appUrl", to( new ContentController().Index ) );

            set( "adminCheckUrl", t2( new SecurityController().CanAppAdmin, app.Id ) + "?appType=" + typeof( ContentApp ).FullName );

            // 当前app/module所有页面，所属的首页
            List<String> moduleUrlList = new List<string>();
            moduleUrlList.Add( to( new ContentController().Index ) );
            moduleUrlList.Add( HtmlLink.ToApp( app ) );

            ContentSetting setting = app.GetSettingsObj();
            if (strUtil.HasText( setting.StaticPath )) {
                // 把所有可能的路径都加到 _moduleUrl 中
                moduleUrlList.Add( setting.StaticPath ); //news/default.html
                moduleUrlList.Add( getStaticDir( setting.StaticPath ) ); //news/
            }

            ctx.SetItem( "_moduleUrl", moduleUrlList.ToArray() );

            // admin link
            set( "allPostsLink", to( new Admin.Common.PostController().List, 0 ) );
            set( "trashPostsLink", to( new Admin.Common.PostController().Trash ) );
            set( "settingLink", to( new Admin.SettingController().Index ) );
            set( "defaultLink", to( new Admin.ContentController().Home ) );
            set( "commentLink", to( new CommentController().List ) );

            IBlock htmlBlock = getBlock( "html" );
            if (ctx.owner.obj is Site) {
                htmlBlock.Set( "staticLink", to( new Admin.HtmlController().Index ) );
                htmlBlock.Next();
            }

            if (app.GetSettingsObj().EnableSubmit == 1) {
                String slnk = string.Format( "<li><a href=\"{0}\" class=\"frmLink\" loadTo=\"contentPage\" nolayout=3>{1}</a></li>",
                    to( new Admin.SubmitSettingController().List ),
                    "投递员管理" );
                set( "submitterLink", slnk );
            }
            else {
                set( "submitterLink", "" );
            }

        }

        //news/default.html=>news/
        private string getStaticDir( string spath ) {
            String[] arrItem = spath.Split( '/' );
            if (arrItem.Length <= 1) return spath;
            return strUtil.TrimEnd( spath, arrItem[arrItem.Length - 1] );
        }


    }

}
