/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System.Collections.Generic;
using wojilu.Apps.Content.Domain;

namespace wojilu.Apps.Content.Interface {

    public interface IContentSectionTypeService {

        DataPage<ContentSectionType> GetPage();
        List<ContentSectionType> GetAll();
    }

}

