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

    internal class ArrayJsonParser : JsonParserBase {

        private List<object> list = new List<object>();

        public override Object getResult() {
            return list;
        }

        public ArrayJsonParser( CharSource charSrc )
            : base( charSrc ) {
        }

        protected override void parse() {

            charSrc.moveToText();
            if (charSrc.getCurrent() != '[') throw ex( "json array must start with [" );

            charSrc.moveToText();
            if (charSrc.getCurrent() == ']') return;

            // 回到[
            charSrc.back(); 

            parseOne();

        }

        private void parseOne() {

            charSrc.moveToText();

            if (charSrc.getCurrent() == ',') {
                charSrc.back();
                list.Add( null );
            }
            else {
                charSrc.back();

                // 将值加入列表
                Object val = moveAndGetParser().getResult(); 
                list.Add( val );
            }

            // 剩余字符处理
            charSrc.moveToText();

            char c = charSrc.getCurrent();
            if (c == ']') {
                return;
            }

            if (c != ',') throw ex( "json array must seperated with , " );

            charSrc.moveToText();
            if (charSrc.getCurrent() == ']') {
                return;
            }
            else {
                charSrc.back();
                parseOne();
            }

        }


    }

}
