/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */


using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Blog.Domain;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Open.Admin;

namespace wojilu.Web.Controller.Admin.Apps.Blog {

    [App( typeof( BlogApp ) )]
    public class CommentController : CommentBaseController<BlogPost> {


    }

}
