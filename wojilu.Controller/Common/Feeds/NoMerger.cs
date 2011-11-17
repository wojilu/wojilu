/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Controller.Common.Feeds {

    public class NoMerger : Merger {

        public override Boolean Merge() {
            return false;
        }

    }

}
