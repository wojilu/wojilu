using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Groups;
using wojilu.Apps.Photo;

namespace wojilu.Web.Controller.Admin.Apps.Photo {

    public class SettingController : ControllerBase {

        public virtual void Index() {
            target( Save );
            bind( "x", PhotoAppSetting.Instance );
        }

        public virtual void Save() {
            PhotoAppSetting.Save( ctx.PostValue<PhotoAppSetting>( "x" ) );
            echoRedirectPart( lang( "opok" ) );
        }

    }

}
