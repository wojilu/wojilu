/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web;
using wojilu.Apps.Content.Domain;

namespace wojilu.Apps.Content.Interface {


    public interface ISectionBinder {

        void Bind( ContentSection section, IList serviceData );

    }

}

