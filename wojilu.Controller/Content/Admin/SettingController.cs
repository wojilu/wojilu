using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Serialization;
using wojilu.Apps.Content.Domain;
using wojilu.DI;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class SettingController : ControllerBase {

        public SettingController() {
            base.LayoutControllerType = typeof( PostController );
            HideLayout( typeof( wojilu.Web.Controller.Content.LayoutController ) );
        }

        public void Index() {

            target( Save );
            ContentApp app = ctx.app.obj as ContentApp;
            bindSettings( app.GetSettingsObj() );

            set( "submitSettingLink", to( new SubmitSettingController().EditRole ) );
        }

        public void Save() {

            ContentSetting s = ctx.PostValue<ContentSetting>();

            s.AllowComment = ctx.PostIsCheck( "contentSetting.AllowComment" );
            s.AllowAnonymousComment = ctx.PostIsCheck( "contentSetting.AllowAnonymousComment" );
            s.EnableSubmit = ctx.PostIsCheck( "contentSetting.EnableSubmit" );

            if (isStaticDirError( s )) {
                echoError();
                return;
            }

            ContentApp app = ctx.app.obj as ContentApp;
            app.Settings = JsonString.ConvertObject( s );
            app.update();

            echoRedirect( lang( "opok" ) );
        }

        private bool isStaticDirError( ContentSetting s ) {

            if (strUtil.HasText( s.StaticDir )) {


                if (s.StaticDir.Length > 50) {
                    errors.Add( "目录名称不能超过50个字符" );
                    return true;
                }

                if (isReservedKeyContains( s.StaticDir )) {
                    errors.Add( "目录名称是保留词，请换一个" );
                    return true;
                }

                if (isDirUsed( s.StaticDir )) {
                    errors.Add( "目录名称已被使用，请换一个" );
                    return true;
                }

            }

            return false;
        }

        private bool isDirUsed( string dirName ) {

            List<ContentApp> appList = ContentApp.find( "OwnerType=:otype" )
                .set( "otype", typeof( Site ).FullName )
                .list();

            foreach (ContentApp app in appList) {

                if (dirName.Equals( app.GetSettingsObj().StaticDir )) return true;
            }

            return false;
        }

        private bool isReservedKeyContains( string dirName ) {

            String[] arrKeys = new String[] { "framework", "bin", "html", "static" };

            return new List<String>( arrKeys ).Contains( dirName );
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

            set( "s.CacheSeconds", dropList( "CacheSeconds", 0, 600, s.CacheSeconds ) );
            set( "s.SummaryLength", dropList( "SummaryLength", 50, 600, s.SummaryLength ) );

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add( "简单列表", ArticleListMode.TitleOnly.ToString() );
            dic.Add( "摘要列表", ArticleListMode.Summary.ToString() );

            dropList( "contentSetting.ArticleListMode", dic, s.ArticleListMode.ToString() );

            set( "s.StaticDir", s.StaticDir );

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
