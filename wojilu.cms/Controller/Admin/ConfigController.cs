using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.cms.Controller.Admin {
    
    public class ConfigController : ControllerBase {

        public void Index() {
            target( Save );
            bind( SystemConfig.Instance );
        }

        public void Save() {

            int pageSize = ctx.PostInt( "PageSize" );
            string email = ctx.Post( "Email" );
            string address = ctx.Post( "Address" );
            string tel = ctx.Post( "Tel" );
            string contact = ctx.Post( "Contact" );

            if (pageSize <= 0) errors.Add( "请填写每页文章数" );

            if (ctx.HasErrors) {
                run( Index );
                return;
            }

            SystemConfig.Instance.PageSize = pageSize;
            SystemConfig.Instance.Email = email;
            SystemConfig.Instance.Address = address;
            SystemConfig.Instance.Tel = tel;
            SystemConfig.Instance.Contact = contact;

            SystemConfig.Save();

            echoRedirect( "保存成功", Index );
        }

    }

}
