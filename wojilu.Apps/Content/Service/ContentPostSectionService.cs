using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;

namespace wojilu.Apps.Content.Service {

    public class ContentPostSectionService {

        public List<ContentPostSection> GetByPost( int postId ) {
            return ContentPostSection.find( "PostId=" + postId ).list();
        }

        public ContentSection GetFirstSectionByPost( int postId ) {

            List<ContentPostSection> setions = GetByPost( postId );
            if (setions.Count == 0) return null;

            return setions[0].Section;
        }

    }

}
