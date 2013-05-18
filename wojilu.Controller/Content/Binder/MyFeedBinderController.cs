/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Feeds.Domain;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Utils;

namespace wojilu.Web.Controller.Content.Binder {

    public class MyFeedBinderController : ControllerBase, ISectionBinder {

        public FeedService feedService { get; set; }

        public MyFeedBinderController() {
            feedService = new FeedService();
        }

        public void Bind( ContentSection section, IList serviceData ) {

            IBlock block = getBlock( "list" );
            foreach (Feed feed in serviceData) {

                block.Set( "feed.UserFace", feed.Creator.PicSmall );
                block.Set( "feed.UserLink", toUser(feed.Creator) );


                String creatorInfo = string.Format( "<a href='{0}'>{1}</a>", toUser( feed.Creator ), feed.Creator.Name );
                String feedTitle = feedService.GetHtmlValue( feed.TitleTemplate, feed.TitleData, creatorInfo );
                block.Set( "feed.Title", feedTitle );
                block.Set( "feed.DataType", feed.DataType );

                String feedBody = feedService.GetHtmlValue( feed.BodyTemplate, feed.BodyData, creatorInfo );
                block.Set( "feed.Body", feedBody );
                block.Set( "feed.Created", cvt.ToTimeString(feed.Created) );

                block.Bind( "feed", feed );


                block.Next();
            }
        }


    }

}
