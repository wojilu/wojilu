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

    internal class VarLabelParsed {

        public String TypeName { get; set; }
        public String VarName { get; set; }

        public String GetFullVarName() {

            if (this.TypeName.Equals( VarLabel.CommonVar )) return VarName;
            if (strUtil.IsNullOrEmpty( this.VarName )) return this.TypeName;
            return this.TypeName + "." + VarName;
        }

        public VarLabel VarLabel { get; set; }

        public int GetFullLength() {
            return GetFullLabelAndVar().Length;
        }

        public String GetFullLabelAndVar() {
            return this.VarLabel.Prefix + this.VarName + this.VarLabel.Postfix;
        }

    }
}
