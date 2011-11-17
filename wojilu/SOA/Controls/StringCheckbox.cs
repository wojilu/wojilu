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
using System.Text;

namespace wojilu.SOA.Controls {


    internal class StringCheckbox : ParamControl, IListControl {

        public override String Html {
            get {
                StringBuilder builder = new StringBuilder();
                builder.Append( "<span class=\"paramLabel\">" );
                builder.Append( base.Label );
                builder.Append( "</span> <span class=\"paramControl\">" );
                foreach (String str in this.Options) {
                    builder.AppendFormat( "<input name=\"{0}\" value=\"{1}\" type=\"checkbox\" class=\"StringCheckbox\"", base.Name, str );
                    if (str.Equals( base.Value )) {
                        builder.AppendFormat( "checked=\"checked\" />{0} ", str );
                    }
                    else {
                        builder.AppendFormat( "/>{0} ", str );
                    }
                }
                builder.Append( "</span>" );
                return builder.ToString();
            }
        }

        public String[] Options { get; set; }

        public override Type Type {
            get { return typeof( String ); }
        }

    }
}

