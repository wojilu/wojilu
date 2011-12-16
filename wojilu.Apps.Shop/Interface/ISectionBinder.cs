/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web;
using wojilu.Apps.Shop.Domain;

namespace wojilu.Apps.Shop.Interface {


    public interface ISectionBinder {

        void Bind( ShopSection section, IList serviceData );

    }

}

