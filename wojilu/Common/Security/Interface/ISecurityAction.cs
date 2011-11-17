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

namespace wojilu.Common.Security {

    /// <summary>
    /// 权限系统中的 action 接口
    /// </summary>
    public interface ISecurityAction {

        int Id { get; set; }
        String Name { get; set; }
        String Url { get; set; }

        IList findAll();
        ISecurityAction GetById( int id );
        void insert();
        Result update();
        void delete();

    }

}
