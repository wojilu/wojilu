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
    /// 通用绑定对象接口
    /// </summary>
    public interface IBinderValue {

        String CreatorName { get; set; }
        String CreatorLink { get; set; }
        String CreatorPic { get; set; }

        String Category { get; set; }

        String Title { get; set; }
        String Link { get; set; }
        String Content { get; set; }
        String Summary { get; set; }

        String PicUrl { get; set; }

        DateTime Created { get; set; }
        int Replies { get; set; }

        Object obj { get; set; }

    }



}
