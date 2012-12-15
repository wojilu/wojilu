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

    internal class BlockParser {

        protected CharSource charSrc;
        private List<Token> _tokens = new List<Token>();


        public BlockParser( CharSource charSrc ) {

            this.charSrc = charSrc;

        }

        public virtual Token getToken() {

            BlockToken token = new BlockToken();
            token.setName( sbname.ToString() );
            token.setTokens( _tokens );

            return token;

        }

        StringBuilder sbname = new StringBuilder();

        private void parse() {

            // begin 开始
            charSrc.moveBegin();

            // 解析 block name
            while (true) {

                if (isBlockBeginTail()) { // lable end
                    charSrc.move( TemplateUtil.loopPostfix.Length );
                    break;
                }

                sbname.Append( charSrc.current() );
                charSrc.move();
            }
           

            // 内部解析
            beginParse();

        }

        private Boolean isBlockBeginTail() {

            for (int i = 0; i < TemplateUtil.loopPostfix.Length; i++) {
                if (charSrc.charList[charSrc.getIndex() + i] != TemplateUtil.loopPostfix[i]) return false;
            }
            return true;
        }

        protected void beginParse() {

            while (true) {

                if (charSrc.isEnd()) return;
                if (charSrc.isBlockEnd()) {
                    int step = 13 + this.sbname.Length;
                    charSrc.move( step );
                    return;
                }

                VarLabelParsed objVar = VarLabel.GetVarLabelValue( charSrc );
                if (objVar != null) {

                    Token token = new VarBlockParser( objVar, this.charSrc ).getToken();
                    _tokens.Add( token );
                    continue;
                }
                // block
                else if (charSrc.isBlock()) {

                    BlockParser p = new BlockParser( this.charSrc );
                    p.parse();

                    Token token = p.getToken();
                    _tokens.Add( token );
                    continue;

                }

                // string
                Token stoken = new StringBlockParser( this.charSrc ).getToken();
                _tokens.Add( stoken );

                // 再次检查
                if (charSrc.isEnd()) return;


                charSrc.move();

                // 如果是区块结束，也 return
                if (charSrc.isBlockEnd()) {
                    int step = 13 + this.sbname.Length;
                    charSrc.move( step );
                    return;
                }

            }

        }



    }

}
