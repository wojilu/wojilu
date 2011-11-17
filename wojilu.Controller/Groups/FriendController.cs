/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Members.Groups.Interface;
using wojilu.Members.Groups.Service;
using wojilu.Members.Groups.Domain;

namespace wojilu.Web.Controller.Groups {

    public class FriendController : ControllerBase {

        public IGroupFriendService friendService { get; set; }

        public FriendController() {
            friendService = new GroupFriendService();
        }

        public void Index() {

            DataPage<Group> list = friendService.GetPage( ctx.owner.Id, 30 );


            IBlock block = getBlock( "list" );
            foreach (Group g in list.Results) {
                block.Set( "g.Name", g.Name );
                block.Set( "g.Logo", g.LogoSmall );
                block.Set( "g.Link", Link.ToMember( g ) );
                block.Next();
            }

            set( "page", list.PageBar );

        }


    }

}
