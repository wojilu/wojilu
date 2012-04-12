using System;
using wojilu.weibo.Data.Sina;

namespace wojilu.weibo.web
{
    public partial class _Default : System.Web.UI.Page
    {
        public static string AppKey = "1424217769";

        public static string AppSecret = "4d0ac2511e53743db50224957d698072";

        private SinaWeibo weibo;

        protected void Page_Load(object sender, EventArgs e)
        {
             weibo = new SinaWeibo(AppKey, AppSecret);
             //string url = weibo.GetAuthorizationUri("http://localhost:23173/authraize.aspx");
             //this.Response.Redirect(url);

             var token = "2.00mIyVoB2Gs4YB1f435e23798bUHBD";
             weibo.SetToken(token);

             var list = weibo.GetPublicStatuses(20, false);

             var test1 = "string.e;";


        }
    }
}
