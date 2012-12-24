using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Content.Caching {

    public class ListMaker : HtmlMakerBase {

        public ListMaker( MvcContext ctx )
            : base( ctx ) {
        }

        protected override string GetDir() {
            return PathHelper.Map( "/html/list/" );
        }

        public int Process( ContentPost post ) {

            if (post == null) return 0;

            int totalCount = 0;

            List<ContentPostSection> psList = ContentPostSection.find( "PostId=" + post.Id ).list();
            foreach (ContentPostSection x in psList) {

                // TODO 只生成前两页
                int recordCount = 2;

                totalCount += this.Process( x.Section.Id, recordCount );
            }

            return totalCount;
        }

        public int Process( int sectionId, int recordCount ) {

            CheckDir();

            String cpLink = _ctx.link.To( new SectionController().Show, sectionId );
            String caLink = _ctx.link.To( new SectionController().Archive, sectionId );


            int pageSize = 20;

            return makeHtmlLoop( recordCount, sectionId, cpLink, caLink, pageSize );
        }



    }

}
