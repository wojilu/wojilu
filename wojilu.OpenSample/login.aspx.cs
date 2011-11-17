using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace wojilu.OpenSample {

    public partial class login : System.Web.UI.Page {

        protected void Page_Load( object sender, EventArgs e ) {

            String msg = Request.QueryString["msg"];

            if (string.IsNullOrEmpty( msg )) {
            }
            else {
                lblError.Text = msg;
            }
        }

        protected void btnSubmit_Click( object sender, EventArgs e ) {

            String name = txtName.Text;
            String pwd = txtPwd.Text;

            if (SecurityHelper.Login( name, pwd )) {
                Response.Redirect( "default.aspx" );
            }
            else {
                lblError.Text = "用户名或密码错误";
            }

        }
    }
}
