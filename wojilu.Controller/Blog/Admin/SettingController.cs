/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Blog.Domain;

namespace wojilu.Web.Controller.Blog.Admin {

    [App( typeof( BlogApp ) )]
    public class SettingController : ControllerBase {

        public void Index() {

            target( Save );
            BlogApp app = ctx.app.obj as BlogApp;
            bindSettings( app.GetSettingsObj() );
        }

        public void Save() {

            BlogSetting s = ctx.PostValue<BlogSetting>();

            s.AllowComment = ctx.PostIsCheck( "blogSetting.AllowComment" );
            s.AllowAnonymousComment = ctx.PostIsCheck( "blogSetting.AllowAnonymousComment" );
            s.IsShowStats = ctx.PostIsCheck( "blogSetting.IsShowStats" );

            BlogApp app = ctx.app.obj as BlogApp;
            app.Settings = Json.ToString( s );
            app.update();

            echoRedirect( lang( "opok" ) );
        }

        public void bindSettings( BlogSetting s ) {

            Dictionary<String, String> dic = new Dictionary<string, string>();

            String chk = "checked=\"checked\"";

            set( "s.AllowComment", s.AllowComment == 1 ? chk : "" );
            set( "s.AllowAnonymousComment", s.AllowAnonymousComment == 1 ? chk : "" );
            set( "s.IsShowStats", s.IsShowStats == 1 ? chk : "" );

            set( "s.PerPageBlogs", dropList( "PerPageBlogs", 3, 20, s.PerPageBlogs ) );
            set( "s.StickyCount", dropList( "StickyCount", 1, 20, s.StickyCount ) );

            set( "s.NewBlogCount", dropList( "NewBlogCount", 3, 20, s.NewBlogCount ) );
            set( "s.NewCommentCount", dropList( "NewCommentCount", 3, 20, s.NewCommentCount ) );
            set( "s.RssCount", dropList( "RssCount", 5, 20, s.RssCount ) );

            set( "s.ListModeFull", s.ListMode == BlogListMode.Full ? chk : "" );
            set( "s.ListModeAbstract", s.ListMode == BlogListMode.Abstract ? chk : "" );

            String[] options = new String[] {
                "100", "200", "300", "500", "600", "800", "1000", "1200", "1500", "2000", "3000"
            };
            radioList( "blogSetting.ListAbstractLength", options, s.ListAbstractLength );

        }

        private String dropList( String name, int istart, int iend, int val ) {
            int count = iend - istart + 1;

            String[] arr = new String[count];
            for (int i = 0; i < count; i++) {
                arr[i] = (i + istart).ToString();
            }

            return Html.DropList( arr, "blogSetting." + name, val );
        }

    }

}
