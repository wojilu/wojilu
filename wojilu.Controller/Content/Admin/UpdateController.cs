using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class UpdateController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( UpdateController ) );

        public void Index() {

            target( TransSection );

        }

        [HttpPost, DbTransaction]
        public void TransSection() {

            // 获取所有Post
            List<ContentPost> list = ContentPost.find( "AppId=" + ctx.app.Id ).list();
            foreach (ContentPost x in list) {
                transSectionOne( x );
            }

            echoRedirect( lang( "opok" ) );
        }

        private void transSectionOne( ContentPost x ) {

            if (x.PageSection == null || x.PageSection.Id <= 0) return;

            int sectionId = x.PageSection.Id;

            ContentPostSection ps =hasTrans( x, sectionId );

            if ( ps != null) {

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

            return ContentPostSection.find( "SectionId=" + sectionId + " and PostId=" + x.Id ).first() ;

        }

    }

}
