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


    internal class CodeParser : BlockParser {

        public CodeParser( CharSource charSrc )
            : base( charSrc ) {

            this.charSrc = charSrc;
        }

        StringBuilder sbCode = new StringBuilder();

        private static readonly String beginTag = "<%";
        private static readonly String endTag = "%>";

        internal void parse() {

            // begin 开始
            charSrc.move( beginTag.Length );


            while (true) {

                if (charSrc.isCodeEnd() || charSrc.isEnd()) {
                    break;
                }

                sbCode.Append( charSrc.current() );
                charSrc.move();
            }

            if (charSrc.isEnd() == false) {
                charSrc.move( endTag.Length );
            }

        }

        public override Token getToken() {

            CodeToken token = new CodeToken();
            token.setValue( sbCode.ToString() );
            token.setType( TokenType.Code );

            return token;
        }

    }
}
