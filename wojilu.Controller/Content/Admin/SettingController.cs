/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Content.Caching;
using wojilu.Web.Controller.Content.Htmls;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class SettingController : ControllerBase {

        public SettingController() {
            HideLayout( typeof( wojilu.Web.Controller.Content.LayoutController ) );
        }

        public void Index() {

            target( Save );
            ContentApp app = ctx.app.obj as ContentApp;
            bindSettings( app.GetSettingsObj() );

            set( "submitSettingLink", to( new SubmitSettingController().EditRole ) );
            set( "submitLink", to( new Submit.PostController().Index ) );
            set( "submitAdminLink", to( new Submit.AdminController().Index));
        }

        public void Save() {

            ContentApp app = ctx.app.obj as ContentApp;

            ContentSetting s = ctx.PostValue( app.GetSettingsObj() ) as ContentSetting;

            s.AllowComment = ctx.PostIsCheck( "contentSetting.AllowComment" );
            s.AllowAnonymousComment = ctx.PostIsCheck( "contentSetting.AllowAnonymousComment" );
            s.EnableSubmit = ctx.PostIsCheck( "contentSetting.EnableSubmit" );

            s.MetaDescription = strUtil.CutString( s.MetaDescription, 500 );

            app.Settings = Json.ToString( s );
            app.update();

            echoRedirect( lang( "opok" ) );
        }


        public void bindSettings( ContentSetting s ) {

            String chk = "checked=\"checked\"";

            set( "s.AllowComment", s.AllowComment == 1 ? chk : "" );
            set( "s.AllowAnonymousComment", s.AllowAnonymousComment == 1 ? chk : "" );
            set( "s.EnableSubmit", s.EnableSubmit == 1 ? chk : "" );

            set( "s.ListPostPerPage", dropList( "ListPostPerPage", 3, 100, s.ListPostPerPage ) );
            set( "s.ListPicPerPage", dropList( "ListPicPerPage", 3, 100, s.ListPicPerPage ) );
            set( "s.ListVideoPerPage", dropList( "ListVideoPerPage", 3, 100, s.ListVideoPerPage ) );

            set( "s.RankPosts", dropList( "RankPosts", 3, 20, s.RankPosts ) );
            set( "s.RankPics", dropList( "RankPics", 1, 20, s.RankPics ) );
            set( "s.RankVideos", dropList( "RssCount", 1, 20, s.RankVideos ) );

            set( "s.SummaryLength", dropList( "SummaryLength", 50, 600, s.SummaryLength ) );

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add( "简单列表", ArticleListMode.TitleOnly.ToString() );
            dic.Add( "摘要列表", ArticleListMode.Summary.ToString() );

            dropList( "contentSetting.ArticleListMode", dic, s.ArticleListMode.ToString() );

            set( "s.StaticDir", s.StaticPath );
            set( "s.MetaKeywords", s.MetaKeywords );
            set( "s.MetaDescription", s.MetaDescription );

        }

        private String dropList( String name, int istart, int iend, int val ) {
            int count = iend - istart + 1;

            String[] arr = new String[count];
            for (int i = 0; i < count; i++) {
                arr[i] = (i + istart).ToString();
            }

            return Html.DropList( arr, "contentSetting." + name, val );
        }

    }

}
