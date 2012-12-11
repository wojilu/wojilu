/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;

namespace wojilu.Apps.Content.Interface {

    public interface IContentImgService {

        ContentImg GetImgById( int imgId );
        ContentPost GetTopImg( int sectionId, int categoryId, int appId );
        List<ContentImg> GetImgList( int postId );
        DataPage<ContentImg> GetImgPage( int postId );
        DataPage<ContentImg> GetImgPage( int postId, int currentPage );

        List<ContentPost> GetByCategory( int sectionId, int categoryId, int appId );
        List<ContentPost> GetByCategory( int sectionId, int categoryId, int appId, int count );

        int GetImgCount( int postId );

        void CreateImg( ContentImg img );
        void DeleteImgOne( ContentImg articleImg );
        void UpdateImgLogo( ContentPost post );


    }

}
