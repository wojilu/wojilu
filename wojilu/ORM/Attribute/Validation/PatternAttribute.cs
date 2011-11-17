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
using System.Text.RegularExpressions;

namespace wojilu.ORM {

    /// <summary>
    /// 正则表达式批注，验证属性的对象是否符合指定的正则表达式
    /// </summary>
    [Serializable]
    public class PatternAttribute : ValidationAttribute {

        private String _regexp;

        public PatternAttribute() {
        }

        public PatternAttribute( String regexp ) {
            _regexp = regexp;
        }

        public PatternAttribute( String regexp, String msg ) {
            _regexp = regexp;
            base.Message = msg;
        }

        public String Regexp {
            get { return _regexp; }
            set { _regexp = value; }
        }

        public override void Validate( String action, IEntity target, EntityPropertyInfo info, Result result ) {

            if (!Regex.IsMatch( cvt.ToNotNull( target.get( info.Name ) ), this.Regexp, RegexOptions.Singleline )) {
                if (strUtil.HasText( this.Message )) {
                    result.Add( this.Message );
                }

                else {
                    EntityInfo ei = Entity.GetInfo( target );
                    String str = "[" + ei.FullName + "] : property \"" + info.Name + "\" ";
                    result.Add( str + " is not match the format pattern : " + this.Regexp );
                }

            }

        }



    }
}
