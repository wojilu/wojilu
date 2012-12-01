using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Forum.Admin {

    [App( typeof( ForumApp ) )]
    public class PickedImgController : PickImgBaseController<ForumPickedImg> {

        public override IPageList GetPage() {

            ForumApp app = ctx.app.obj as ForumApp;
            ForumSetting s = app.GetSettingsObj();

            return ndb.findPage( typeof( ForumPickedImg ), "AppId=" + ctx.app.Id, s.HomeImgCount );
        }

    }

}
