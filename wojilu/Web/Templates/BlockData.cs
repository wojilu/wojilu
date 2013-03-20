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

namespace wojilu.Web.Templates {

    // 包括1行数据，即一个Dictionary + 多个Block引用
    internal class BlockData {


        public BlockData( Dictionary<String, Object> vars ) {
            copyValues( vars, this.vars );
            copyValues( vars, this.parentVars );
        }

        private void copyValues( Dictionary<String, Object> values, Dictionary<String, Object> target ) {
            foreach (String key in values.Keys) {
                target.Add( key, values[key] );
            }
        }

        private Dictionary<String, Object> vars = new Dictionary<String, Object>();
        private Dictionary<String, Object> parentVars = new Dictionary<String, Object>();
        private Dictionary<String, ContentBlock> _blocks = new Dictionary<String, ContentBlock>();

        public Dictionary<String, Object> getDic() {
            return vars;
        }

        public Dictionary<String, String> getLang( String langKey ) {
            return this.getDic()[langKey] as Dictionary<String, String>;
        }

        public Dictionary<String, Object> getParentDic() {
            return parentVars;
        }


        public void addBlock( ContentBlock block ) {
            if (_blocks.ContainsKey( block.Name )) throw new TemplateException( "请勿重复绑定(getBlock)区块。区块名=" + block.Name + "，模板文件=" + block.getTemplatePath() );
            _blocks.Add( block.Name, block );
        }

        public ContentBlock getBlock( String key ) {

            ContentBlock val;
            _blocks.TryGetValue( key, out val );
            return val;
        }

    }

}
