/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Content.Domain;

namespace wojilu.Apps.Content.Interface {

    public interface IContentSectionTemplateService {

        List<ContentSectionTemplate> GetAll();
        List<ContentSectionTemplate> GetBy( String filterString );
        ContentSectionTemplate GetById( int templateId );

    }

}

