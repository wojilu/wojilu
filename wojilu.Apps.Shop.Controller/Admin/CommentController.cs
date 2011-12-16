using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Controller.Common;
using wojilu.Apps.Shop.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Blog.Domain;

namespace wojilu.Web.Controller.Shop.Admin {

    [App( typeof( ShopApp ) )]
    public class CommentController : CommentController<ShopItemComment> {
    }

}
