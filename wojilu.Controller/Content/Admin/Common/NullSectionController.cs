using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppBase;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Content.Section {

    public class NullSectionController : ControllerBase, IPageSection {

        public void SectionShow( long sectionId ) {
            content( "(section content not implemented)" );
        }

    }

}
