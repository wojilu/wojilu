/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Content.Domain;

namespace wojilu.Apps.Content.Interface {

    public interface IContentSectionService {

        ContentSection GetById(long sectionId);
        ContentSection GetById(long id, long appId);

        List<ContentSection> GetByApp(long appId);
        List<ContentSection> GetInputSectionsByApp(long appId);
        List<ContentSection> GetByRowColumn( List<ContentSection> list, int rowId, int columnId );

        void Insert( ContentSection section );
        void Update( ContentSection section );
        void Delete( ContentSection section );

        int Count(long appId, int rowId);

        List<ContentSection> GetForCombine( ContentSection section );
        void CombineSections(long sectionId, long targetSectionId);
        void RemoveSection(long sectionId, long fromSectionId);


        string GetSectionIdsByPost(long postId);

    }

}

