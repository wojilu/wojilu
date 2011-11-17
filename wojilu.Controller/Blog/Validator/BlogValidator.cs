/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Apps.Blog.Domain;
using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Blog {

    public class BlogValidator {

        public static BlogCategory ValidateCategory( BlogCategory category, MvcContext ctx ) {

            String name = ctx.Post( "Name" );
            int orderId = ctx.PostInt( "OrderId" );
            String description = ctx.Post( "Description" );

            if (strUtil.IsNullOrEmpty( name )) ctx.errors.Add( lang.get( "exName" ) );

            if (category == null) category = new BlogCategory();

            category.Name = name;
            category.OrderId = orderId;
            category.Description = description;
            category.OwnerId = ctx.owner.obj.Id;
            category.OwnerUrl = ctx.owner.obj.Url;
            category.AppId = ctx.app.Id;

            return category;
        }



    }
}

