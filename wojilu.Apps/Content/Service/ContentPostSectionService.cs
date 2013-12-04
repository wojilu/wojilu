using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;

namespace wojilu.Apps.Content.Service {

    public class ContentPostSectionService {

        public virtual List<ContentPostSection> GetByPost(long postId) {
            return ContentPostSection.find( "PostId=" + postId ).list();
        }

        public virtual ContentSection GetFirstSectionByPost(long postId) {

            List<ContentPostSection> setions = GetByPost( postId );
            if (setions.Count == 0) return null;

            return setions[0].Section;
        }

    }

}
