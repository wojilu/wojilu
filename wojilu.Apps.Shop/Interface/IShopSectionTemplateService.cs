/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Shop.Domain;

namespace wojilu.Apps.Shop.Interface {

    public interface IShopSectionTemplateService {

        List<ShopSectionTemplate> GetAll();
        List<ShopSectionTemplate> GetBy( String filterString );
        ShopSectionTemplate GetById( int templateId );

    }

}

