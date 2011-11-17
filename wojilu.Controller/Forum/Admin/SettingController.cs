using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Domain;
using wojilu.Serialization;

namespace wojilu.Web.Controller.Forum.Admin {

    [App( typeof( ForumApp ) )]
    public class SettingController : ControllerBase{

        public void Index() {

            target( Save );
            ForumApp app = ctx.app.obj as ForumApp;
            bindSettings( app.GetSettingsObj() );
        }

        public void Save() {

            ForumSetting s = ctx.PostValue<ForumSetting>();
            s.IsHideStats = ctx.PostIsCheck( "forumSetting.HideShowStats" );
            s.IsHideTop = ctx.PostIsCheck( "forumSetting.IsHideTop" );
            s.IsHideOnline = ctx.PostIsCheck( "forumSetting.IsHideOnline" );
            s.IsHideLink = ctx.PostIsCheck( "forumSetting.IsHideLink" );

            ForumApp app = ctx.app.obj as ForumApp;
            app.Settings = JsonString.ConvertObject( s );
            app.update();

            echoRedirect( lang( "opok" ) );
        }

        public void bindSettings( ForumSetting s ) {

            String chk = "checked=\"checked\"";
            set( "s.HideShowStats", s.IsHideStats == 1 ? chk : "" );
            set( "s.IsHideTop", s.IsHideTop == 1 ? chk : "" );
            set( "s.IsHideOnline", s.IsHideOnline == 1 ? chk : "" );
            set( "s.IsHideLink", s.IsHideLink == 1 ? chk : "" );

            set( "s.PageSize", dropList( "PageSize", 1, 200, s.PageSize ) );
            set( "s.TopicPageSize", dropList( "TopicPageSize", 1, 200, s.ReplySize ) );
            set( "s.NewDays", dropList( "NewDays", 1, 60, s.NewDays ) );
            set( "s.HomeHotDays", dropList( "HomeHotDays", 1, 365, s.HomeHotDays ) );
            set( "s.HomeImgCount", dropList( "HomeImgCount", 1, 10, s.HomeImgCount ) );
            set( "s.HomeListCount", dropList( "HomeListCount", 1, 20, s.HomeListCount ) );

        }

        private String dropList( String name, int istart, int iend, int val ) {
            int count = iend - istart + 1;

            String[] arr = new String[count];
            for (int i = 0; i < count; i++) {
                arr[i] = (i + istart).ToString();
            }

            return Html.DropList( arr, "forumSetting." + name, val );
        }

    }

}
