/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Domain;
using System.Collections;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Shop.Utils;

namespace wojilu.Web.Controller.Shop.Binder {

    public class UserFacesBinderController : ControllerBase, ISectionBinder {

        public IShopCustomTemplateService ctService { get; set; }

        public UserFacesBinderController() {
            ctService = new ShopCustomTemplateService();
        }

        public void Bind( ShopSection section, IList serviceData ) {

            TemplateUtil.loadTemplate( this, section, ctService );

            IBlock block = base.getBlock( "list" );

            foreach (User user in serviceData) {

                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", Link.ToMember( user ) );

                block.Bind( "user", user );

                block.Next();
            }

        }

    }

}
