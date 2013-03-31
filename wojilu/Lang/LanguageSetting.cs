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
    /// 某个语言包配置文件的内容，包括一个名称和一个语言包的 Dictionary
    /// </summary>
    public class LanguageSetting {

        private static readonly ILog logger = LogManager.GetLogger( typeof( LanguageSetting ) );

        private String name;
        private Dictionary<String, String> langMap = new Dictionary<String, String>();

        public static LanguageSetting NewNull() {
            return new LanguageSetting();
        }

        private LanguageSetting() {
        }

        public LanguageSetting( String name, Dictionary<String, String> lang ) {
            this.name = name;
            this.langMap = lang;
        }

        /// <summary>
        /// 根据 key 获取语言值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String get( String key ) {

            String ret;
            langMap.TryGetValue( key, out ret );
            if (ret == null) {
                ret = lang.CoreLangPrefix + key;
                logger.Error( "no language key: core.config =>" + key );
            }

            return ret;
        }

        /// <summary>
        /// 获取语言的键值对 Dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, String> getLangMap() {
            return this.langMap;
        }


    }
}
