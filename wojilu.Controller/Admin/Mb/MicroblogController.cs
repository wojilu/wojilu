using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.Microblogs.Interface;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Microblogs;
using wojilu.ORM;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin.Mb {

    public class MicroblogController : ControllerBase {

        public IMicroblogService microblogService { get; set; }
        public SysMicroblogService sysMicroblogService { get; set; }

        public MicroblogController() {
            microblogService = new MicroblogService();
            sysMicroblogService = new SysMicroblogService();
        }

        public override void Layout() {

            set( "mbAdminLink", to( List ) );
            set( "mbSettingsLink", to( Settings ) );

        }

        public void Settings() {

            target( SaveSettings );
            bind( "x", MicroblogAppSetting.Instance );
        }

        public void SaveSettings() {

            MicroblogAppSetting.Save( ctx.PostValue<MicroblogAppSetting>( "x" ) );
            echoRedirectPart( lang( "opok" ) );
        }


        public void List() {
            DataPage<Microblog> list = sysMicroblogService.GetPageAll( 30 );
            bindMbList( list );
        }

        public void PicList() {
            view( "List" );
            DataPage<Microblog> list = sysMicroblogService.GetPicPageAll( 30 );
            bindMbList( list );
        }

        public void ListFilter() {
            view( "List" );
            String condition = getCondition();
            DataPage<Microblog> list = sysMicroblogService.GetPageByCondition( condition );
            bindMbList( list );
        }

        public void Search() {
            view( "List" );
            String searchType = ctx.Get( "t" );
            String key = getSearchKey();
            String condition = getSearchCondition( searchType, key );
            DataPage<Microblog> list = sysMicroblogService.GetPageByCondition( condition );
            bindMbList( list );
        }

        private String getSearchKey() {
            return strUtil.SqlClean( ctx.Get( "q" ), 15 );
        }

        private String getSearchCondition( String searchType, String key ) {

            String condition = "";
            if (strUtil.HasText( key )) {

                if (searchType == "author") {

                    String userIds = getUserIds( key );
                    if (strUtil.HasText( userIds )) {
                        condition = "UserId in (" + userIds + ")";
                    }

                }
                else if (searchType == "content") {
                    condition = "Content like '%" + key + "%'";
                }
            }

            return condition;
        }

        private string getUserIds( string key ) {
            List<User> users = db.find<User>( "Name like '%" + key + "%'" ).list();
            return strUtil.GetIds( users );
        }

        private void bindMbList( DataPage<Microblog> list ) {

            set( "OperationUrl", to( Admin ) );
            set( "searchAction", to( Search ) );
            set( "searchKey", getSearchKey() );

            String searchType = ctx.Get( "t" );
            String sel = "selected=\"selected\"";
            if (searchType == "author") {
                set( "authorSelectStatus", sel );
                set( "contentSelectStatus", "" );
            }
            else if (searchType == "content") {
                set( "authorSelectStatus", "" );
                set( "contentSelectStatus", sel );
            }

            set( "allLink", to( List ) );
            set( "picLink", to( PicList ) );
            set( "filterLink", to( ListFilter ) );

            list.Results.ForEach( x => {
                x.data["CreatorLink"] = alink.ToUserMicroblog( x.User );
                x.data.show = alink.ToAppData( x );
                x.data["PicIcon"] = x.IsPic ? string.Format( "<a href=\"{0}\" target=\"_blank\" title=\"点击查看原始图片\"><img src=\"{1}img.gif\" /></a>", x.PicOriginal, sys.Path.Img ) : "";
            } );

            bindList( "list", "x", list.Results );

            set( "page", list.PageBar );
        }

        private String getCondition() {

            String condition = "";

            String filter = ctx.Get( "filter" );
            if (strUtil.HasText( filter )) {

                EntityInfo ei = Entity.GetInfo( typeof( Microblog ) );

                String t = ei.Dialect.GetTimeQuote();
                String fs = "and Created between " + t + "{0}" + t + " and " + t + "{1}" + t + " order by ";
                String fs1 = fs + " Reposts";
                String fs2 = fs + " Replies";

                DateTime now = DateTime.Now;

                if (filter == "repost_day1")
                    condition += string.Format( fs1, now.ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
                else if (filter == "repost_day2")
                    condition += string.Format( fs1, now.ToShortDateString(), now.AddDays( 2 ).ToShortDateString() );
                else if (filter == "repost_day3")
                    condition += string.Format( fs1, now.ToShortDateString(), now.AddDays( 3 ).ToShortDateString() );
                else if (filter == "repost_week")
                    condition += string.Format( fs1, now.AddDays( -7 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
                else if (filter == "repost_month")
                    condition += string.Format( fs1, now.AddMonths( -1 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
                else if (filter == "repost_month3")
                    condition += string.Format( fs1, now.AddMonths( -3 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );

                else if (filter == "reply_day1")
                    condition += string.Format( fs2, now.ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
                else if (filter == "reply_day2")
                    condition += string.Format( fs2, now.ToShortDateString(), now.AddDays( 2 ).ToShortDateString() );
                else if (filter == "reply_day3")
                    condition += string.Format( fs2, now.ToShortDateString(), now.AddDays( 3 ).ToShortDateString() );
                else if (filter == "reply_week")
                    condition += string.Format( fs2, now.AddDays( -7 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
                else if (filter == "reply_month")
                    condition += string.Format( fs2, now.AddMonths( -1 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
                else if (filter == "reply_month3")
                    condition += string.Format( fs2, now.AddMonths( -3 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );

            }

            return condition;
        }


        [HttpPost, DbTransaction]
        public void Admin() {

            String ids = ctx.PostIdList( "choice" );
            String cmd = ctx.Post( "action" );

            if ("delete".Equals( cmd )) {
                sysMicroblogService.DeleteSysBatch( ids );
                echoAjaxOk();
            }
            else {
                echoError( "errorCmd" );
            }
        }

    }

}
