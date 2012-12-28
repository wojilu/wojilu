/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Content.Domain;

namespace wojilu.Apps.Content.Interface {

    public interface IContentSectionService {

        ContentSection GetById( int sectionId );
        ContentSection GetById( int id, int appId );

        List<ContentSection> GetByApp( int appId );
        List<ContentSection> GetInputSectionsByApp( int appId );
        List<ContentSection> GetByRowColumn( List<ContentSection> list, int rowId, int columnId );

        void Insert( ContentSection section );
        void Update( ContentSection section );
        void Delete( ContentSection section );

        int Count( int appId, int rowId );

        List<ContentSection> GetForCombine( ContentSection section );
        void CombineSections( int sectionId, int targetSectionId );
        void RemoveSection( int sectionId, int fromSectionId );


        String GetSectionIdsByPost( int postId );

    }

}

