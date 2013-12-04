using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppBase;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Admin.Section {

    public class NullSectionAdmin : IPageAdminSection {

        public virtual void AdminSectionShow( long sectionId ) {
        }

        public virtual List<IPageSettingLink> GetSettingLink( long sectionId ) {
            return new List<IPageSettingLink>();
        }

        public virtual string GetEditLink( long postId ) {
            return "#";
        }

        public virtual string GetSectionIcon( long sectionId ) {
            return "";
        }

        public virtual List<ContentPost> GetSectionPosts( long sectionId ) {
            return new List<ContentPost>();
        }

    }
}
