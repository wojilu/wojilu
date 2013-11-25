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

        void AdminSectionShow(long sectionId);
        List<IPageSettingLink> GetSettingLink(long sectionId);
        string GetEditLink(long postId);
        string GetSectionIcon(long sectionId);
        List<ContentPost> GetSectionPosts(long sectionId);

    }

    /// <summary>
    /// 门户app中的页面区块接口
    /// </summary>
    public interface IPageSection {

        void SectionShow(long sectionId);

    }

}

