/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Feeds.Service;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Common.Feeds.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Users.Admin;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Users {

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


        public void Index() {

            if (ctx.viewer.HasPrivacyPermission( ctx.owner.obj, UserPermission.Share.ToString() ) == false) {
                echo( lang( "exVisitNoPermission" ) );
                return;
            }

            ctx.Page.Title = lang( "share" );

            int userId = ctx.owner.Id;

            DataPage<Share> list;
            if (userId > 0) {
                list = shareService.GetPageByUser( userId, 20 );
            }
            else {
                list = shareService.GetFriendsPage( ctx.owner.Id, 20 );
            }

            bindShareList( list );

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
        }

        private String getComment( String comment ) {
            if (strUtil.IsNullOrEmpty( comment )) return "";
            return string.Format( "<div class=\"quote\"><span class=\"qSpan\">{0}<span></div>", comment );
        }

        private String getCreatorInfos( User user ) {
            return string.Format( "<a href='{0}'>{1}</a>", toUser( user ), user.Name );
        }

    }

}
