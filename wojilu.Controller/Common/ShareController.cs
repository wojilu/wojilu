/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.Feeds.Service;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Msg.Service;

using wojilu.Members.Users.Domain;
using wojilu.ORM;
using wojilu.Common;

namespace wojilu.Web.Controller.Common {

    public class ShareController : ControllerBase {

        public IShareService shareService { get; set; }
        public IFeedService feedService { get; set; }

        public ShareController() {
            shareService = new ShareService();
            feedService = new FeedService();
        }

        [Login]
        public void Add( int id ) {

            target( Create, id );

            String dataType = ctx.Get( "dataType" );
            String name = ctx.web.UrlDecode( ctx.Get( "name" ) );
            String dataLink = ctx.Get( "dataLink" );

            if (Entity.GetType( dataType ) == null) {
                echo( lang( "exTypeError" ) );
                return;
            }

            set( "name", name );
            set( "dataType", dataType );
            set( "dataLink", dataLink );

            Type et = Entity.GetType( dataType );

            IShareData targetData = ndb.findById( et, id ) as IShareData;
            IShareInfo shareInfo = targetData.GetShareInfo();

            String shareHtml = feedService.GetHtmlValue( shareInfo.GetShareBodyTemplate(), shareInfo.GetShareBodyData( "#" ), null );
            set( "shareContent", shareHtml );
        }

        [Login, HttpPost, DbTransaction]
        public void Create( int id ) {

            User creator = (User)ctx.viewer.obj;

            String dataType = ctx.Post( "dataType" );
            String name = ctx.Post( "name" );
            String dataLink = ctx.Post( "dataLink" );

            Type et = Entity.GetType( dataType );
            IShareData targetData = ndb.findById( et, id ) as IShareData;
            IShareInfo shareInfo = targetData.GetShareInfo();

            Share share = new Share();

            share.Creator = creator;
            share.DataType = typeof( Share ).FullName;

            share.TitleTemplate = shareInfo.GetShareTitleTemplate();
            share.TitleData = shareInfo.GetShareTitleData();

            share.BodyTemplate = shareInfo.GetShareBodyTemplate();
            share.BodyData = shareInfo.GetShareBodyData( dataLink );

            share.BodyGeneral = ctx.Post( "Comment" );

            Result result = shareService.Create( share );
            if (result.HasErrors) {
                echoToParent( result.ErrorsHtml );
            }
            else {
                feedService.publishUserAction( share );
                shareInfo.addNotification( creator.Name, toUser( creator ) );
                echoToParent( lang( "opok" ) );
            }
        }

    }

}
