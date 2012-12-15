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
using wojilu.Web.Templates.Tokens;

namespace wojilu.Web.Templates.Parser {

    internal class StringBlockParser : BlockParser {

        public StringBlockParser( CharSource charSrc )
            : base( charSrc ) {

            parseString();
        }

        public override Token getToken() {
            StringToken token = new StringToken();
            token.setValue( this.sb.ToString() );
            return token;
        }

        private StringBuilder sb = new StringBuilder();

        private void parseString() {

            while (true) {

                if (charSrc.isEnd()) {
                    sb.Append( this.charSrc.current() );
                    return;
                }

                else if (shouldBack()) {
                    this.charSrc.back();
                    return;
                }


                else {
                    sb.Append( this.charSrc.current() );
                    this.charSrc.move();
                }
            }
        }

        private Boolean shouldBack() {
            if (charSrc.isBlock()) return true;
            if (this.charSrc.isBlockEnd()) return true;

            VarLabelParsed objVar = VarLabel.GetVarLabelValue( charSrc );
            if (objVar != null) return true;

            return false;
        }

    }

}
