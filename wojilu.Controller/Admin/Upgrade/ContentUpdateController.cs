using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Mvc;
using System.Data;

namespace wojilu.Web.Controller.Admin.Upgrade {

    public class ContentUpdateController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ContentUpdateController ) );

        public void Index() {

            target( TransSection );

        }

        [HttpPost, DbTransaction]
        public void TransSection() {


            runsql( "update ContentPostSection set SaveStatus=0" );
            runsql( "update microblog set SaveStatus=0" );

            List<ContentApp> apps = ContentApp.find( "" ).list();

            foreach (ContentApp app in apps) {

                // 获取所有Post
                List<ContentPost> list = ContentPost.find( "AppId=" + app.Id ).list();
                foreach (ContentPost x in list) {
                    transSectionOne( x );
                }

            }

            echoRedirect( lang( "opok" ) );
        }

        private void runsql( string sql ) {
            IDbCommand cmd = db.getCommand( sql );
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        private void transSectionOne( ContentPost x ) {

            if (x.PageSection == null || x.PageSection.Id <= 0) return;

            int sectionId = x.PageSection.Id;

            ContentPostSection ps = hasTrans( x, sectionId );

            if (ps != null) {

                ps.SaveStatus = x.SaveStatus;
                ps.update();

            }
            else {

                ContentPostSection newPs = new ContentPostSection();
                newPs.Post = x;
                newPs.Section = x.PageSection;
                newPs.SaveStatus = x.SaveStatus;

                newPs.insert();

                logger.Info( "transSectionOne=> postId=" + x.Id + ", sectionId=" + x.PageSection.Id );
            }

        }

        private ContentPostSection hasTrans( ContentPost x, int sectionId ) {

            return ContentPostSection.find( "SectionId=" + sectionId + " and PostId=" + x.Id ).first();

        }
    }
}
