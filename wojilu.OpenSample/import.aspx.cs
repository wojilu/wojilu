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
using wojilu.OpenSample.Domain;
using wojilu.Open;

namespace wojilu.OpenSample {

    public partial class import : System.Web.UI.Page {

        private static readonly ILog logger = LogManager.GetLogger( typeof( import ) );

        protected void Page_Load( object sender, EventArgs e ) {

        }

        protected void btnSubmit_Click( object sender, EventArgs e ) {

            List<SampleUser> users = getUserInfoList();
            if (users.Count == 0) {
                showMsg( "请填写用户信息" );
                return;
            }

            logger.Info( "user count=" + users.Count );

            DbContext.beginTransactionAll();

            try {
                foreach (SampleUser u in users) {
                    registerUser( u ); // 逐个导入用户(导入的过程就是注册的过程)
                    logger.Info( "register user=" + u.Name );
                }
                DbContext.commitAll();
                logger.Info( "register done" );
            }
            catch ( Exception ex) {
                logger.Info( "" + ex.Message );
                logger.Info( "" + ex.StackTrace );
                DbContext.rollbackAll();
            }



            showOk( "导入成功！" );
        }


        private void registerUser( SampleUser user ) {

            // 1) 你自己的网站用户注册
            user.insert();

            // 2) 调用 OpenService 进行 wojilu 注册
            new OpenService().UserRegister( user.Name, user.Pwd, user.Email );
        }

        private List<SampleUser> getUserInfoList() {

            if (strUtil.IsNullOrEmpty( txtBody.Text )) return new List<SampleUser>();

            List<SampleUser> users = new List<SampleUser>();

            String[] arrLines = txtBody.Text.Trim().Split( new char[] { '\n', '\r' } );

            foreach (String line in arrLines) {

                if (strUtil.IsNullOrEmpty( line )) continue;

                String[] arrItems = line.Split( '/' );
                if (arrItems.Length != 3) continue;

                SampleUser user = new SampleUser();
                user.Name = arrItems[0];
                user.Pwd = arrItems[1];
                user.Email = arrItems[2];

                if (hasError( user )) continue;

                users.Add( user );
            }

            return users;
        }

        private bool hasError( SampleUser user ) {
            return string.IsNullOrEmpty( user.Name ) ||
                string.IsNullOrEmpty( user.Pwd ) ||
                string.IsNullOrEmpty( user.Email );
        }



        private void showMsg( string msg ) {
            this.ClientScript.RegisterStartupScript( this.GetType(), "RegisterStartupScript", "<script>alert( '" + msg + "' );</script>" );
        }

        private void showOk( string msg ) {
            this.ClientScript.RegisterStartupScript( this.GetType(), "RegisterStartupScript", "<script>alert( '" + msg + "' );window.location.href='import.aspx'</script>" );
        }

    }
}
