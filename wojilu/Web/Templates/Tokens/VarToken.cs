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

        private Boolean _isobj;
        private String _objname;

        private String[] _objAccessor;


        public void setIsObject( Boolean isobj ) { _isobj = isobj; }
        public Boolean isObject() { return _isobj; }

        public String getObjName() { return _objname; }
        public void setObjName( String objname ) { _objname = objname; }

        public String[] getObjAccessor() { return _objAccessor; }
        public void setObjAccessor( String[] accessor ) { _objAccessor = accessor; }

        private String _rawLabelAndVar;
        public String getRawLabelAndVar() { return _rawLabelAndVar; }
        public void setRawLabelAndVar( String val ) { _rawLabelAndVar = val; }



        public override void appendData( StringBuilder sb, ContentBlock block, BlockData blockdata ) {

            Object dicValue;
            blockdata.getDic().TryGetValue( this.getName(), out dicValue );

            if (dicValue != null) {
                sb.Append( dicValue );
                return;
            }

            if (this.isObject()) {

                Dictionary<String, Object> dicValues = blockdata.getDic();
                Object obj;
                dicValues.TryGetValue( this.getObjName(), out obj );

                if (obj != null) {

                    Object pval = getPropertyValue( obj, _objAccessor, this.getObjName() );
                    sb.Append( pval );
                    return;
                }

            }

            sb.Append( this.getRawLabelAndVar() );
        }



        private static String getPropertyValue( Object obj, String[] _objAccessor, String objName ) {

            Object pval = obj;

            if (_objAccessor.Length == 1) {
                return pval.ToString();
            }

            for (int i = 1; i < _objAccessor.Length; i++) {
                pval = getpvalue( pval, _objAccessor[i], objName );
            }

            return pval == null ? "" : pval.ToString();
        }

        private static Object getpvalue( Object obj, String pname, String objName ) {

            if (obj == null) return null;

            IEntity objbase = obj as IEntity;
            if (objbase != null) return objbase.get( pname );

            Dictionary<String, String> objDic = obj as Dictionary<String, String>;
            if (objDic != null) {

                String result;
                objDic.TryGetValue( pname, out result );
                if (result == null) {
                    if (objName == "lang") {
                        return lang.CoreLangPrefix + pname;
                    }
                    else if (objName == "alang") {
                        return lang.AppLangPrefix + pname;
                    }
                    else {
                        throw new KeyNotFoundException( "no view data item in dictionary: " + objName + "->" + pname );
                    }
                }
                else {
                    return result;
                }

            }

            PropertyInfo p = obj.GetType().GetProperty( pname );
            if (p == null) {
                throw new KeyNotFoundException( "no view data item in object: " + objName + "->" + pname );
            }

            return p.GetValue( obj, null );
        }

    }

}
