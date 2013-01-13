using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin.Mb {

    public class MbCommentController : ControllerBase {

        public SysMicroblogCommentService commentService { get; set; }

        public MbCommentController() {
            commentService = new SysMicroblogCommentService();
        }

        public void Index() {
            DataPage<MicroblogComment> list = commentService.GetSysPage( 30 );
            bindMbList( list );
        }

        private void bindMbList( DataPage<MicroblogComment> list ) {
            set( "OperationUrl", to( Admin ) );
            set( "homeLink", to( Index ) );

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

            list.Results.ForEach( x => {
                x.data["CreatorLink"] = alink.ToUserMicroblog( x.User );
                x.data.show = alink.ToAppData( x.Root );
            } );

            bindList( "list", "x", list.Results );

            set( "page", list.PageBar );
        }

        public void Search() {
            view( "Index" );
            String searchType = ctx.Get( "t" );
            String key = getSearchKey();
            String condition = getSearchCondition( searchType, key );
            DataPage<MicroblogComment> list = commentService.GetPageByCondition( condition );
            bindMbList( list );
        }

        private string getUserIds( string key ) {
            List<User> users = db.find<User>( "Name like '%" + key + "%'" ).list();
            return strUtil.GetIds( users );
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


        [HttpPost, DbTransaction]
        public void Admin() {

            String ids = ctx.PostIdList( "choice" );
            String cmd = ctx.Post( "action" );

            if ("deleteTrue".Equals( cmd )) {
                commentService.DeleteTrueBatch( ids );
                echoAjaxOk();
            }
            else {
                echoError( "errorCmd" );
            }
        }

    }

}
