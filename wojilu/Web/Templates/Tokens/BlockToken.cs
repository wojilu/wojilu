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

namespace wojilu.Web.Templates.Tokens {


    internal class BlockToken : Token {

        private List<Token> tokens;
        //private Dictionary<String, Token> tokenMap = new Dictionary<String, Token>();

        public List<Token> getTokens() {
            return tokens;
        }

        public void setTokens( List<Token> tokens ) {
            this.tokens = tokens;

            //addMap( tokens );
        }

        //public Token getByName( String tokenName ) {
        //    Token x;
        //    this.tokenMap.TryGetValue( tokenName, out x );
        //    return x;
        //}

        //private void addMap( List<Token> tokens ) {
        //    if (tokens == null) return;
        //    foreach (Token x in tokens) {

        //        if (x.getName() == null) continue;

        //        // 允许变量存在多次
        //        if (tokenMap.ContainsKey( x.getName() )) continue;

        //        tokenMap.Add( x.getName(), x );
        //    }
        //}

        public override void appendData( StringBuilder sb, ContentBlock block, BlockData blockdata ) {

            ContentBlock subBlock = blockdata.getBlock( this.getName() );
            if (subBlock == null) return;

            List<BlockData> datalist = subBlock.getDataList();
            foreach (BlockData oneData in datalist) {
                subBlock.addResultOne( sb, oneData );
            }

        }

    }

}
