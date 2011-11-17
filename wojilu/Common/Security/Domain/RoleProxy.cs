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
using System.Collections;
using System.Text;

using wojilu.ORM;

namespace wojilu.Common.Security {

    /// <summary>
    /// 角色代理对象，对象临时转换使用
    /// </summary>
    public class RoleProxy : IRole {

        public int Id { get; set; }
        public String Name { get; set; }
        public IRole Role { get; set; }

    }
}
