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

namespace wojilu.Web {

    /// <summary>
    /// 额外自定义的绑定方法。在默认的绑定执行完之后，本方法会被附加上去继续执行
    /// </summary>
    /// <param name="tpl"></param>
    /// <param name="lbl"></param>
    /// <param name="obj"></param>
    public delegate void otherBindFunction( IBlock tpl, String lbl, Object obj );

    /// <summary>
    /// 额外自定义的绑定方法。在默认的绑定执行完之后，本方法会被附加上去继续执行
    /// </summary>
    /// <param name="tpl"></param>
    /// <param name="id"></param>
    public delegate void bindFunction( IBlock tpl, int id );

}
