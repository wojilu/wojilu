/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;

namespace wojilu.Apps.Content.Service {


    public class ContentSectionTypeService : IContentSectionTypeService {

        public virtual DataPage<ContentSectionType> GetPage() {
            DataPage<ContentSectionType> list = cdb.findPage<ContentSectionType>( 12 );
            list.Results.Sort( comp );
            return list;
        }

        public virtual List<ContentSectionType> GetAll() {
            List<ContentSectionType> results = cdb.findAll<ContentSectionType>();
            results.Sort( comp );
            return results;
        }

        private static int comp( ContentSectionType x, ContentSectionType y ) {
            if (x.OrderId > y.OrderId) return 1;
            if (x.OrderId < y.OrderId) return -1;
            return 0;
        }

    }
}

