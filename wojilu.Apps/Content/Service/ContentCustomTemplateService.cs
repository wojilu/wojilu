using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;

namespace wojilu.Apps.Content.Service {


    public class ContentCustomTemplateService : IContentCustomTemplateService {



        public virtual ContentCustomTemplate GetById(long id, long ownerId) {

            ContentCustomTemplate ct = ContentCustomTemplate.findById( id );
            if (ct == null) return null;
            if (ct.OwnerId != ownerId) return null;
            return ct;
        }


        public virtual void Insert( ContentCustomTemplate ct ) {
            ct.insert();
        }

        public virtual void Update( ContentCustomTemplate ct ) {
            ct.update();
        }
    }

}
