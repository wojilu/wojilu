using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;

namespace wojilu.Apps.Content.Service {


    public class ContentCustomTemplateService : IContentCustomTemplateService {



        public ContentCustomTemplate GetById( int id, int ownerId ) {

            ContentCustomTemplate ct = ContentCustomTemplate.findById( id );
            if (ct == null) return null;
            if (ct.OwnerId != ownerId) return null;
            return ct;
        }


        public void Insert( ContentCustomTemplate ct ) {
            ct.insert();
        }

        public void Update( ContentCustomTemplate ct ) {
            ct.update();
        }
    }

}
