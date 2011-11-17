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

    internal abstract class JsonParserBase {

        protected CharSource charSrc;

        protected abstract void parse();
        public abstract Object getResult();

        public JsonParserBase() {
        }

        public JsonParserBase( CharSource charSrc ) {
            this.charSrc = charSrc;
            parse();
        }

        protected JsonParserBase moveAndGetParser() {

            charSrc.moveToText();

            char c = charSrc.getCurrent();

            if (c == '"' || c == '\'') {
                return new StringJsonParser( this.charSrc, c );
            }

            if (c == '{') {
                charSrc.back();
                return new ObjectJsonParser( this.charSrc );
            }

            if (c == '[') {
                charSrc.back();
                return new ArrayJsonParser( this.charSrc );
            }

            return new ValueJsonParser( this.charSrc );

        }

        protected JsonParserException ex( String msg ) {
            return new JsonParserException( msg + "(index:"+this.charSrc.getIndex().ToString()+")\n" + this.charSrc.strSrc  );
        }



    }
}
