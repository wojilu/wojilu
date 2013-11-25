/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;

namespace wojilu.Apps.Content.Interface {

    public interface IContentImgService {

        ContentImg GetImgById(long imgId);
        ContentPost GetTopImg(long sectionId, long categoryId, long appId);
        List<ContentImg> GetImgList(long postId);
        DataPage<ContentImg> GetImgPage(long postId);
        DataPage<ContentImg> GetImgPage(long postId, int currentPage);

        List<ContentPost> GetByCategory(long sectionId, long categoryId, long appId);
        List<ContentPost> GetByCategory(long sectionId, long categoryId, long appId, int count);

        int GetImgCount(long postId);

        void CreateImg( ContentImg img );
        void DeleteImgOne( ContentImg articleImg );
        void UpdateImgLogo( ContentPost post );


    }

}
