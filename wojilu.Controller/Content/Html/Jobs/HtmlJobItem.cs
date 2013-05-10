/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Data;
using wojilu.Web.Jobs;
using wojilu.Apps.Content.Domain;
using wojilu.ORM;

namespace wojilu.Web.Controller.Content.Htmls {

    [NotSave]
    public class HtmlJobItem : CacheObject {

        public String Method { get; set; }

        public int PostId { get; set; }

        public String Ids { get; set; }
    }


}
