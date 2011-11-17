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
using System.IO;

using System.Web;

namespace wojilu {

    /// <summary>
    /// 获取某个 app 的语言包
    /// </summary>
    public class alang {

        /// <summary>
        /// 根据 app 的类型 t 获取某 key 的语言值
        /// </summary>
        /// <param name="t">app 的类型</param>
        /// <param name="key">语言 key</param>
        /// <returns></returns>
        public static String get( Type t, String key ) {
            LanguageSetting ls = lang.getByApp( t );
            return ls == null ? null : ls.get( key );
        }

    }
}
