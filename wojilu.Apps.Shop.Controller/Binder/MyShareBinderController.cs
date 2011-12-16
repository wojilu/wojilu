/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;

using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Feeds.Domain;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Shop.Utils;

namespace wojilu.Web.Controller.Shop.Binder {

    public class MyShareBinderController : ControllerBase, ISectionBinder {

        public FeedService feedService { get; set; }
        public IShopCustomTemplateService ctService { get; set; }

        public MyShareBinderController() {
            feedService = new FeedService();
            ctService = new ShopCustomTemplateService();
        }


        public void Bind( ShopSection section, IList serviceData ) {

            TemplateUtil.loadTemplate( this, section, ctService );

            IBlock block = getBlock( "list" );
            foreach (Share share in serviceData) {

                String creatorInfo = string.Format( "<a href='{0}'>{1}</a>", Link.ToMember( share.Creator ), share.Creator.Name );

                block.Set( "share.Title", feedService.GetHtmlValue( share.TitleTemplate, share.TitleData, creatorInfo ) );
                block.Set( "share.Created", cvt.ToTimeString( share.Created ) );
                block.Set( "share.DataTypeImg", strUtil.Join( sys.Path.Img, "/app/s/" + typeof( Share ).FullName + ".gif" ) );

                block.Bind( "share", share );

                block.Next();
            }

        }

    }

}
