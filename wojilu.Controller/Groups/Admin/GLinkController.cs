/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Groups.Service;

namespace wojilu.Web.Controller.Groups.Admin {

    public class GLinkController : ControllerBase {

        public IMemberAppService userAppService { get; set; }

        public GLinkController() {
            userAppService = new GroupAppService();
        }

        public void Index() {

            set( "addMenu", to( new MenuController().AddMenu ) );

            set( "recentMember", lnkFull( to( new Groups.MemberController().List ) ) );
            set( "friends", lnkFull( to( new Groups.FriendController().Index ) ) );

            IList apps = userAppService.GetByMember( ctx.owner.obj.Id );
            IBlock block = getBlock( "list" );
            foreach (IMemberApp app in apps) {
                block.Set( "app.Name", app.Name );
                String lnk = lnkFull( alink.ToUserAppFull( app ) );
                block.Set( "app.Link", lnk );
                block.Next();
            }

        }

        private String lnkFull( String link ) {
            if (link.StartsWith( "http" )) return link;
            return strUtil.Join( ctx.url.SiteUrl, link );
        }
    }

}
