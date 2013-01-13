/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Common {

    /// <summary>
    /// 通用绑定对象
    /// </summary>
    public class ItemValue : IBinderValue {

        public String CreatorName { get; set; }
        public String CreatorLink { get; set; }
        public String CreatorPic { get; set; }

        public String Category { get; set; }

        public String Title { get; set; }
        public String Link { get; set; }
        public String Content { get; set; }
        public String Summary { get; set; }
        public String PicUrl { get; set; }

        public DateTime Created { get; set; }
        public int Replies { get; set; }

        public Object obj { get; set; }

    }


}
