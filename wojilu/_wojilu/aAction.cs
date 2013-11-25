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

namespace wojilu {

    /// <summary>
    /// 不带参数的委托，主要用于mvc中的链接。
    /// </summary>
    public delegate void aAction();

    /// <summary>
    /// 带整型参数的委托，主要用于mvc中的链接。
    /// </summary>
    /// <param name="id"></param>
    public delegate void aActionWithId( long id );

    /// <summary>
    /// 带字符串参数的委托，主要用于mvc中的链接，本委托不常用。
    /// </summary>
    /// <param name="param"></param>
    public delegate void aActionWithQuery( String param );

}
