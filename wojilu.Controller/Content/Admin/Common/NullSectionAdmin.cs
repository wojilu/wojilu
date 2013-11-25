using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppBase;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Admin.Section {

    public class NullSectionAdmin : IPageAdminSection {

        public void AdminSectionShow( long sectionId ) {
        }

        public List<IPageSettingLink> GetSettingLink( long sectionId ) {
            return new List<IPageSettingLink>();
        }

        public string GetEditLink( long postId ) {
            return "#";
        }

        public string GetSectionIcon( long sectionId ) {
            return "";
        }

        public List<ContentPost> GetSectionPosts( long sectionId ) {
            return new List<ContentPost>();
        }

    }
}
