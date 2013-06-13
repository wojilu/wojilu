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

    public class MyShareBinderController : ControllerBase, ISectionBinder {

        public FeedService feedService { get; set; }

        public MyShareBinderController() {
            feedService = new FeedService();
        }


        public void Bind( ContentSection section, IList serviceData ) {

            IBlock block = getBlock( "list" );
            foreach (Share share in serviceData) {

                String creatorInfo = string.Format( "<a href='{0}'>{1}</a>", toUser( share.Creator ), share.Creator.Name );

                block.Set( "share.Title", feedService.GetHtmlValue( share.TitleTemplate, share.TitleData, creatorInfo ) );
                block.Set( "share.Created", cvt.ToTimeString( share.Created ) );
                block.Set( "share.DataTypeImg", strUtil.Join( sys.Path.Img, "/app/s/" + typeof( Share ).FullName + ".gif" ) );

                block.Bind( "share", share );

                block.Next();
            }

        }

    }

}
