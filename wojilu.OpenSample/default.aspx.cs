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
using System.Collections.Generic;

using wojilu.Data;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Domain;

namespace wojilu.OpenSample {

    public partial class _default : System.Web.UI.Page {

        protected void Page_Load( object sender, EventArgs e ) {

            if (SecurityHelper.IsLogin()) {
                lnkLogin.Visible = false;
                lnkLogout.Visible = true;
                lnkRegister.Visible = false;
                lblWelcome.Text = SecurityHelper.GetWelcomeInfo();
            }
            else {
                lnkLogin.Visible = true;
                lnkLogout.Visible = false;
                lnkRegister.Visible = true;
            }

            bindForumTopics();
        }

        private void bindForumTopics() {

            SysForumTopicService topicService = new SysForumTopicService();
            List<ForumTopic> list = topicService.GetRankByHits( 1, 10 );

            rptForumTopic.DataSource = list;
            rptForumTopic.DataBind();

        }

        protected void lnkLogout_Click( object sender, EventArgs e ) {
            SecurityHelper.Logout();
            Response.Redirect( "Default.aspx" );
        }

    }
}
