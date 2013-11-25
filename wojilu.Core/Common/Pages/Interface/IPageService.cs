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

        PageCategory GetCategoryById(long categoryId, IMember owner);
        List<PageCategory> GetCategories( IMember owner );

        Page GetPostById(long id, IMember owner);
        List<Page> GetPosts(IMember owner, long categoryId);

        int GetPagesCount( IMember owner );

        Result Insert( Page p );

        Result Update( Page p );

        void Delete( Page p );

        DataPage<PageHistory> GetHistoryPage(long pageId, IMember owner, int pageSize);


        PageHistory GetHistory(long id);
        List<long> GetEditorIds(long pageId);

        void UpdateCategory(string ids, long categoryId);
    }

}
