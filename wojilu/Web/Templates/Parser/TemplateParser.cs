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

namespace wojilu.Web.Templates.Parser {

    internal class TemplateParser : BlockParser {

        private static readonly ILog logger = LogManager.GetLogger( typeof( TemplateParser ) );
        public static Dictionary<String, BlockParser> parserCache = new Dictionary<String, BlockParser>();
        private static object objLock = new object();

        public static TemplateParser GetParser( String templateContent ) {

            if (strUtil.IsNullOrEmpty( templateContent )) return new TemplateParser( new CharSource( null ) );

            return new TemplateParser( new CharSource( templateContent ) );
        }

        public static TemplateParser GetParser( String absPath, String templateContent ) {

            if (strUtil.IsNullOrEmpty( templateContent )) return new TemplateParser( new CharSource( null ) );

            String _cacheKey = absPath;

            if (parserCache.ContainsKey( _cacheKey )==false) {

                lock (objLock) {

                    if (parserCache.ContainsKey( _cacheKey ) == false) {

                        TemplateParser tp = new TemplateParser( new CharSource( templateContent ) );
                        parserCache.Add( _cacheKey, tp );

                    }
                }

            }

            return (TemplateParser)parserCache[_cacheKey];
        }
        
        public TemplateParser( CharSource charSrc )
            : base( charSrc ) {

            if (charSrc.charList == null || charSrc.charList.Length == 0) return;

            base.beginParse();
        }


        internal static void Reset() {
            parserCache.Clear();
        }

    }

}
