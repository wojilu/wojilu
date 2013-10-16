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
using wojilu.Web.Templates;
using System.Reflection;
using wojilu.Web.Templates.Tokens;
using wojilu.Web;

namespace wojilu {


    /// <summary>
    /// 视图对象集合
    /// </summary>
    public class ViewData {

        private BlockData _blockData;
        private Template _template;

        public ViewData() {
        }

        public ViewData( Template template ) {
            _blockData = template.getData();
            _template = template;
        }

        /// <summary>
        /// 根据变量名称，获取值
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public Object get( String varName ) {

            VarInfo x = VarInfo.Init( varName );
            return x.getValue( _blockData, varName );
        }

        public Object getBlockResultByIndex( int tokenIndex ) {
            List<Token> tokenList = _template.getTokens();
            Token tk = tokenList[tokenIndex];
            StringBuilder sb = new StringBuilder();
            tk.appendData( sb, null, _blockData );
            return sb.ToString();
        }


        public new Object this[String key] {
            get {
                return get( key );
            }
        }

        public void Add( String key, Object val ) {
            _blockData.getDic().Add( key, val );
            ;
        }



    }

}
