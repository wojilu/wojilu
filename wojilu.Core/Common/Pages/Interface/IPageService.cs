/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Members.Interface;
using wojilu.Common.Pages.Domain;

namespace wojilu.Common.Pages.Interface {

    public interface IPageService {

        void AddHits( Page data );

        PageCategory GetCategoryById( int categoryId, IMember owner );
        List<PageCategory> GetCategories( IMember owner );

        Page GetPostById( int id, IMember owner );
        List<Page> GetPosts( IMember owner, int categoryId );

        int GetPagesCount( IMember owner );

        Result Insert( Page p );

        Result Update( Page p );

        void Delete( Page p );

        DataPage<PageHistory> GetHistoryPage( int pageId, IMember owner, int pageSize );


        PageHistory GetHistory( int id );
        List<int> GetEditorIds( int pageId );

        void UpdateCategory( string ids, int categoryId );
    }

}
