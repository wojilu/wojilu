using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Groups;

namespace wojilu.Web.Controller.Admin.Groups {

    public class SettingController : ControllerBase {

        public void Index() {
            target( Save );
            bind( "s", GroupSetting.Instance );
        }

        public void Save() {
            GroupSetting.Save( ctx.PostValue<GroupSetting>() );
            echoRedirectPart( lang( "opok" ) );
        }

    }

}
