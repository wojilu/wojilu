using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Caching {

    public class HtmlListMaker : HtmlMakerBase {

        public int MakeHtml( MvcContext ctx ) {

            ContentPost post = HtmlHelper.GetCurrentPost( ctx );
            if (post == null) return 0;

            int recordCount = 0; // TODO


            List<ContentPostSection> psList = ContentPostSection.find( "PostId="+post.Id ).list();
            foreach (ContentPostSection x in psList) {
                recordCount += this.MakeHtml( ctx, x.Section.Id, recordCount );
            }

            return recordCount;
        }

        public int MakeHtml( MvcContext ctx, int sectionId, int recordCount ) {

            CheckDir( sectionId );

            String cpLink = ctx.link.To( new SectionController().Show, sectionId );
            String caLink = ctx.link.To( new SectionController().Archive, sectionId );

            int pageSize = 20;

            return makeHtmlLoop( ctx, recordCount, sectionId, cpLink, caLink, pageSize );
        }


        protected override string GetDir() {
            return PathHelper.Map( "/html/list/" );
        }

    }

}
