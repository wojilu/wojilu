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

    internal class VarBlockParser : BlockParser {

        private VarLabelParsed objVar;

        public VarBlockParser( VarLabelParsed objVar, CharSource charSrc )
            : base( charSrc ) {

            this.objVar = objVar;

            parse(  );

        }

        private void parse(  ) {

            charSrc.move( this.objVar.GetFullLength() );
        }

        public override Token getToken() {

            VarToken token = new VarToken();

            token.setName( this.objVar.GetFullVarName() );

            int dotIndex = token.getName().IndexOf( '.' );
            if (dotIndex > 0 && dotIndex < token.getName().Length) {

                token.setIsObject( true );

                String[] arrp = token.getName().Split( '.' );

                token.setObjName( arrp[0] );
                token.setObjAccessor( arrp );

            }

            token.setRawLabelAndVar( this.objVar.GetFullLabelAndVar() );


            return token;

        }

    }

}
