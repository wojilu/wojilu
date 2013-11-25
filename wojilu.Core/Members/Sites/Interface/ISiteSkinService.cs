/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Interface;

namespace wojilu.Members.Sites.Interface {

    public interface ISiteSkinService {

        SiteSkin GetById(long id);

        List<SiteSkin> GetSysAll();

        String GetSkin();
        string GetSkin(long querySkinId, string cssVersion);



        void CustomBg( IMember iMember, String ele, String val );

        Boolean IsUserCustom();
    }

}
