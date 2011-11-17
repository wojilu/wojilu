/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Data;
using wojilu.ORM;
using wojilu.Common.Security;

namespace wojilu.Members.Sites.Domain {

    [Serializable]
    public class SiteActionConfig : CacheObject, ISecurity {

        public String Security { get; set; }

        //-------------------------------------------

        public static SiteActionConfig Instance {
            get {
                IList list = new SiteActionConfig().findAll();
                if (list.Count == 0) {
                    SiteActionConfig config = new SiteActionConfig();
                    config.insert();
                    return config;
                }
                return (list[0] as SiteActionConfig);
            }
        }

    }

}
