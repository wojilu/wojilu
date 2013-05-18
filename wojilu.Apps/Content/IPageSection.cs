/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Content.Domain;

namespace wojilu.Common.AppBase {

    /// <summary>
    /// 门户app中，后台管理界面，页面区块接口
    /// </summary>
    public interface IPageAdminSection {

        void AdminSectionShow( int sectionId );
        List<IPageSettingLink> GetSettingLink( int sectionId );
        String GetEditLink( int postId );
        String GetSectionIcon( int sectionId );
        List<ContentPost> GetSectionPosts( int sectionId );

    }

    /// <summary>
    /// 门户app中的页面区块接口
    /// </summary>
    public interface IPageSection {

        void SectionShow( int sectionId );

    }

}

