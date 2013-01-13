/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Common.Picks {

    public class MergedData {

        public IAppData Topic { get; set; }

        public String Title { get; set; } // 支持 html

        public String Link { get; set; } // 自定义 link

        public String Summary { get; set; } // 支持 html

        // 是否固定
        public Boolean IsPin {
            get { return this.Topic == null; }
        }

        public int IsEdit { get; set; } // 是否编辑过

    }

}
