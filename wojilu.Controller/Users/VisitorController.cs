/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Users {

    public class VisitorController : ControllerBase {

        public IVisitorService visitorService { get; set; }

        public VisitorController() {
            visitorService = new VisitorService();
        }

        public void Index() {

            if (ctx.viewer.HasPrivacyPermission( ctx.owner.obj, UserPermission.RecentVisitor.ToString() ) == false) {
                echo( lang( "exVisitNoPermission" ) );
                return;
            }

            ctx.Page.Title = lang( "recentVisitors" );

            DataPage<User> list = visitorService.GetPage( ctx.owner.Id, 50 );
            bindUsers( list.Results, "list" );
            set( "page", list.PageBar );

        }


        private void bindUsers( List<User> users, String blockName ) {

            IBlock block = getBlock( blockName );
            foreach (User user in users) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", toUser( user ) );
                block.Next();
            }
        }

    }

}
