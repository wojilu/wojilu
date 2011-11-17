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


    internal class IntDroplist : ParamControl, IListControl {

        public override String Html {
            get {
                StringBuilder builder = new StringBuilder();
                foreach (String str in this.Options) {
                    if (str.Equals( base.Value )) {
                        builder.AppendFormat( "<option value=\"{0}\" selected=\"selected\">{0}</option>", str );
                    }
                    else {
                        builder.AppendFormat( "<option value=\"{0}\">{0}</option>", str );
                    }
                }
                return String.Format( "<span class=\"paramLabel\">{0}</span> <span class=\"paramControl\"><select name=\"{1}\"  class=\"IntDroplist\">{2}</select></span>", base.Label, base.Name, builder.ToString() );
            }
        }

        public String[] Options { get; set; }

        public override System.Type Type {
            get { return typeof( int ); }
        }

    }
}

