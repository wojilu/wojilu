/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Data;
using wojilu.ORM;

namespace wojilu.Web.GlobalApp {



    [NotSave]
    public class AppErrorItem : CacheObject {

        public String Title { get; set; }
        public String ErrorHtml { get; set; }
        public DateTime Occured { get; set; }

    }
}
