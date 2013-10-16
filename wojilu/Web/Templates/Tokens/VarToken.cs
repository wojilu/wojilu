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
using System.Reflection;

namespace wojilu.Web.Templates.Tokens {

    internal class VarToken : Token {

        private VarInfo _varInfo;
        public void setVarInfo( VarInfo varInfo ) { _varInfo = varInfo; }
        public VarInfo getVarInfo() { return _varInfo; }

        public override void appendData( StringBuilder sb, ContentBlock block, BlockData blockdata ) {

            sb.Append( _varInfo.getValue( blockdata, this.getName() ) );
        }

    }

}
