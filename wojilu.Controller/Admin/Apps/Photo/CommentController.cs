/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */


using wojilu.Web.Mvc.Attr;
using wojilu.Web.Controller.Common;
using wojilu.Apps.Photo.Domain;
using wojilu.Web.Controller.Open.Admin;

namespace wojilu.Web.Controller.Admin.Apps.Photo {

    [App( typeof( PhotoApp ) )]
    public class CommentController : CommentBaseController<PhotoPost> {


    }

}
