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

namespace wojilu.Serialization {

    internal class StringJsonParser : ValueJsonParser {

        private String _result;

        public override Object getResult() {
            return _result;
        }


        public StringJsonParser( CharSource charSrc, char c ) {
            this.quote = c;
            this.charSrc = charSrc;
            parse();
        }

        private char quote = '"';


        protected override void parse() {

            charSrc.move();

            char c = charSrc.getCurrent();

            if (c == '\n' || c == '\r') throw ex( "String ends at a new line while not end" );

            if (c == quote) {
                _result = sb.ToString();
                return;
            }

            if (c == '\\')
                processEscape();
            else
                sb.Append( c );

            parse();

        }

    }

}
