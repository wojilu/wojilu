using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Serialization;
using wojilu.Apps.Shop.Domain;
using wojilu.DI;

namespace wojilu.Web.Controller.Shop.Admin {

    [App( typeof( ShopApp ) )]
    public class SettingController : ControllerBase {

        public SettingController() {
            base.LayoutControllerType = typeof( ItemController );
        }

        public void Index() {

            target( Save );
            ShopApp app = ctx.app.obj as ShopApp;
            bindSettings( app.GetSettingsObj() );

            //set( "submitSettingLink", to( new SubmitSettingController().EditRole ) );
        }

        public void Save() {

            ShopSetting s = ctx.PostValue<ShopSetting>();

            s.AllowComment = ctx.PostIsCheck( "ShopSetting.AllowComment" );
            s.AllowAnonymousComment = ctx.PostIsCheck( "ShopSetting.AllowAnonymousComment" );
            //s.EnableSubmit = ctx.PostIsCheck( "ShopSetting.EnableSubmit" );

            ShopApp app = ctx.app.obj as ShopApp;
            app.Settings = JsonString.ConvertObject( s );
            app.update();

            echoRedirect( lang( "opok" ) );
        }

        public void bindSettings( ShopSetting s ) {

            String chk = "checked=\"checked\"";

            set( "s.AllowComment", s.AllowComment == 1 ? chk : "" );
            set( "s.AllowAnonymousComment", s.AllowAnonymousComment == 1 ? chk : "" );
            //set( "s.EnableSubmit", s.EnableSubmit == 1 ? chk : "" );

            set( "s.ListPostPerPage", dropList( "ListPostPerPage", 3, 100, s.ListPostPerPage ) );
            set( "s.ListPicPerPage", dropList( "ListPicPerPage", 3, 100, s.ListPicPerPage ) );
            set( "s.ListVideoPerPage", dropList( "ListVideoPerPage", 3, 100, s.ListVideoPerPage ) );

            set( "s.RankPosts", dropList( "RankPosts", 3, 20, s.RankPosts ) );
            set( "s.RankPics", dropList( "RankPics", 1, 20, s.RankPics ) );
            set( "s.RankVideos", dropList( "RssCount", 1, 20, s.RankVideos ) );

            set( "s.CacheSeconds", dropList( "CacheSeconds", 0, 600, s.CacheSeconds ) );
            set( "s.SummaryLength", dropList( "SummaryLength", 50, 600, s.SummaryLength ) );

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("简单标题列表", ItemListMode.TitleOnly.ToString());
            dic.Add("摘要列表", ItemListMode.TitleSummary.ToString());
            dic.Add("简单图文列表", ItemListMode.PhotoOnly.ToString());
            dic.Add("摘要图文列表", ItemListMode.PhotoSummary.ToString());

            dropList("ShopSetting.ItemListMode", dic, s.ItemListMode.ToString());

        }

        private String dropList( String name, int istart, int iend, int val ) {
            int count = iend - istart + 1;

            String[] arr = new String[count];
            for (int i = 0; i < count; i++) {
                arr[i] = (i + istart).ToString();
            }

            return Html.DropList( arr, "ShopSetting." + name, val );
        }

    }

}
