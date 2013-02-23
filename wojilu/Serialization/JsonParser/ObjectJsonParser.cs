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


    internal class ObjectJsonParser : JsonParserBase {

        private JsonObject map = new JsonObject();

        public override Object getResult() {
            return this.map;
        }

        public ObjectJsonParser( CharSource charSrc )
            : base( charSrc ) {
        }

        protected override void parse() {

            charSrc.moveToText();

            if (charSrc.getCurrent() != '{') throw ex( "json Object must start with { " );


            charSrc.moveToText();
            if (charSrc.getCurrent() == '}') return;
            charSrc.back();

            parseOne();

        }

        private void parseOne() {


            charSrc.moveToText();

            if (charSrc.getCurrent() == '}') return;

            // 解析key
            charSrc.back();
            String key = moveAndGetParser().getResult().ToString();

            // 解析冒号
            charSrc.moveToText();
            if (charSrc.getCurrent() != ':') throw ex( "json object's property pair must seperated with :" );

            // 解析value
            Object val = moveAndGetParser().getResult();

            // 获取值
            map.Add( key, val );

            // 处理剩下的字符
            charSrc.moveToText();
            char c = charSrc.getCurrent();
            if (c == '}') return;
            if (c != ',') throw ex( "json object's property must seperated with ," );

            charSrc.moveToText();
            if (charSrc.getCurrent() == '}')
                return;
            else {
                charSrc.back();
                parseOne();
            }


        }


    }

}
