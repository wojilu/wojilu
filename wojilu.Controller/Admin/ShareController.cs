using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Feeds.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin {


    public class ShareController : ControllerBase {

        public IShareService shareService { get; set; }
        public IFeedService feedService { get; set; }

        public ShareController() {
                shareService = new ShareService();
                feedService = new FeedService();
        }


        public void Index() {

            DataPage<Share> list = shareService.GetPageAll();

            bindShares( list.Results );

            set( "page", list.PageBar );

        }

        private void bindShares( List<Share> list ) {
            IBlock block = getBlock( "list" );
            foreach (Share share in list) {


                block.Set( "share.Id", share.Id );
                block.Set( "share.DataType", share.DataType );
                block.Set( "share.UserFace", share.Creator.PicSmall );
                block.Set( "share.UserLink", toUser( share.Creator ) );

                String creatorInfo = getCreatorInfos( share.Creator );
                String feedTitle = feedService.GetHtmlValue( share.TitleTemplate, share.TitleData, creatorInfo );
                block.Set( "share.Title", feedTitle );


                String feedBody = feedService.GetHtmlValue( share.BodyTemplate, share.BodyData, creatorInfo );
                block.Set( "share.Body", feedBody );
                block.Set( "share.BodyGeneral", getComment( share.BodyGeneral ) );

                block.Set( "share.Created", share.Created );

                block.Set( "share.DeleteLink", to( Delete, share.Id ) );

                block.Next();
            }

        }

        private String getComment( String comment ) {
            if (strUtil.IsNullOrEmpty( comment )) return "";
            return string.Format( "<div class=\"quote\"><span class=\"qSpan\">{0}<span></div>", comment );
        }

        private String getCreatorInfos( User user ) {
            return string.Format( "<a href='{0}'>{1}</a>", toUser( user ), user.Name );
        }

        [HttpDelete]
        public void Delete( int id ) {

            Result result = shareService.Delete( id );
            if (result.IsValid) {
                echoRedirect( lang( "opok" ) );
            }
            else {
                echoError( result );
            }

        }


    }

}
