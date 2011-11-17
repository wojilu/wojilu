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

using wojilu.Data;
using wojilu.OpenSample.Domain;
using wojilu.OpenSample;

namespace wojilu.OpenSample {

    public partial class register : System.Web.UI.Page {
        protected void Page_Load( object sender, EventArgs e ) {

        }

        protected void btnSubmit_Click( object sender, EventArgs e ) {

            SampleUser user = new SampleUser();
            user.Name = txtName.Text;
            user.Pwd = txtPwd.Text;
            user.Email = txtEmail.Text;

            if (hasError( user )) {
                lblError.Text = "请完整输入用户名、密码和 email 信息";
            }
            else {
                SecurityHelper.Register( user ); // 注册：1)本站注册 2)调用wojilu的OpenService注册
                Response.Redirect( "login.aspx?msg=注册成功，请登录" );
            }

        }

        private Boolean hasError( SampleUser user ) {

            return string.IsNullOrEmpty( user.Name ) ||
                string.IsNullOrEmpty( user.Pwd ) ||
                string.IsNullOrEmpty( user.Email );
        }




    }
}
