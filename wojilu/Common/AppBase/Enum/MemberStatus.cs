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

namespace wojilu.Common.AppBase {

    /// <summary>
    /// 注册用户的状态(待审核、删除、置顶、推荐、普通)
    /// </summary>
    public class MemberStatus {

        public static readonly int Normal = 0;
        public static readonly int Pick = 1;
        public static readonly int Top = 2;
        public static readonly int Approving = -2;
        public static readonly int Deleted = -1;
        public static readonly int UnApproved = -3;

    }

}

