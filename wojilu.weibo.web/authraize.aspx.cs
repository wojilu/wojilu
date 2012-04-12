using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wojilu.weibo.Data.Sina;

namespace wojilu.weibo.web
{
    public partial class authraize : System.Web.UI.Page
    {
        public static string AppKey = "1424217769";

        public static string AppSecret = "4d0ac2511e53743db50224957d698072";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["code"]))
            {
                var  weibo = new SinaWeibo(AppKey, AppSecret);

                var token = weibo.GetAccessTokenByAuthorizationCode(Request.QueryString["code"], "http://localhost:23173/authraize.aspx");
                TextBox1.Text = token.Token;
            }
        }
    }
}