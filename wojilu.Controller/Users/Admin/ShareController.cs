/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Serialization;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Feeds.Service;

namespace wojilu.Web.Controller.Users.Admin {

    public class ShareController : ControllerBase {

        public IFeedService feedService { get; set; }
        public IFriendService friendService { get; set; }
        public IFollowerService followService { get; set; }

        public IShareService shareService { get; set; }

        public ShareController() {
            feedService = new FeedService();
            friendService = new FriendService();
            followService = new FollowerService();

            shareService = new ShareService();
        }

        public void Index( int userId ) {

            set( "shareLink", to(Index, -1) );
            set( "myShareLink", to(Index, ctx.viewer.Id) );


            DataPage<Share> list;
            if (userId > 0) {
                list = shareService.GetPageByUser( userId, 20 );
            }
            else {
                list = shareService.GetFriendsPage( ctx.owner.Id, 20 );
            }

            bindShareList( list );

            bindShareFriends();
            bindShareFollowing();

            target( SaveShare );
        }


        public void Show( int id ) {

            Share share = shareService.GetByIdWithComments( id );
            set( "shareListLink", to(Index, -1) );
            bindOne( this.utils.getCurrentView(), share );

            set( "commentForm", loadHtml( new ShareCommentsController().commentForm, 0 ) );

        }

        private void bindShareList( DataPage<Share> list ) {

            set( "commentForm", loadHtml( new ShareCommentsController().commentForm, 0 ) );

            IBlock block = getBlock( "list" );
            foreach (Share share in list.Results) {

                bindOne( block, share );

                block.Next();
            }

            set( "page", list.PageBar );
        }

        private void bindOne( IBlock block, Share share ) {

            block.Set( "share.Id", share.Id );
            block.Set( "share.DataType", share.DataType );
            block.Set( "share.UserFace", share.Creator.PicSmall );
            block.Set( "share.UserLink", toUser( share.Creator ) );

            String creatorInfo = getCreatorInfos( share.Creator );
            String feedTitle = feedService.GetHtmlValue( share.TitleTemplate, share.TitleData, creatorInfo );
            block.Set( "share.Title", feedTitle );

            block.Set( "commentList", loadHtml( new ShareCommentsController().commentList, share.Id ) );


            String feedBody = feedService.GetHtmlValue( share.BodyTemplate, share.BodyData, creatorInfo );
            block.Set( "share.Body", feedBody );
            block.Set( "share.BodyGeneral", getComment( share.BodyGeneral ) );

            block.Set( "share.Created", share.Created );
            block.Set( "share.DeleteLink", to( Delete, share.Id ) );
        }

        private String getComment( String comment ) {
            if (strUtil.IsNullOrEmpty( comment )) return "";
            return string.Format( "<div class=\"quote\"><span class=\"qSpan\">{0}<span></div>", comment );
        }

        private String getCreatorInfos( User user ) {
            return string.Format( "<a href='{0}'>{1}</a>", toUser( user ), user.Name );
        }

        // -----------------------------------------------------------------------------------------

        private void bindShareFriends() {
            List<User> friends = friendService.GetRecentActive( 60, ctx.owner.obj.Id );
            IBlock block = getBlock( "shareFriends" );
            foreach (User user in friends) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.ShareLink", to( Index, user.Id ) );
                block.Next();
            }
        }

        private void bindShareFollowing() {
            List<User> friends = followService.GetRecentFollowing( ctx.owner.obj.Id, 60 );
            IBlock block = getBlock( "shareFollowing" );
            foreach (User user in friends) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.ShareLink", to( Index, user.Id ) );
                block.Next();
            }
        }

        // -----------------------------------------------------------------------------------------

        [HttpPost, DbTransaction]
        public void SaveShare() {

            String shareLink = ctx.Post( "shareLink" );
            String shareDescription = ctx.Post( "shareDescription" );

            if (strUtil.IsNullOrEmpty( shareLink ))
                errors.Add( lang( "exUrl" ) );
            else if ( RegPattern.IsMatch( shareLink, RegPattern.Url )==false )
                errors.Add( lang( "exUrlFormat" ) );

            if (errors.HasErrors) {
                echoError();
                return;
            }


            Result result = shareService.CreateUrl( (User)ctx.viewer.obj, shareLink, shareDescription );
            if (result.HasErrors) {
                echoError( result );
            }
            else {
                Share share = result.Info as Share;
                feedService.publishUserAction( share );

                String url = to( Index, ctx.viewer.Id );
                echoRedirect( lang( "opok" ), url );
            }

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
