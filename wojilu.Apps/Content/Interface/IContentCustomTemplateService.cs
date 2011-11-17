using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;

namespace wojilu.Apps.Content.Interface {

    public interface IContentCustomTemplateService {
        ContentCustomTemplate GetById( int id, int ownerId );
        void Insert( ContentCustomTemplate ct );
        void Update( ContentCustomTemplate ct );
    }


}
