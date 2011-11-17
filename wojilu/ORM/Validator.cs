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
using System.Data;
using System.Text.RegularExpressions;

using wojilu.Data;
using wojilu.Log;

namespace wojilu.ORM {

    internal class Validator {

        public static Result Validate( IEntity target ) {
            return Validate( target, "" );
        }

        public static Result Validate( IEntity target, String action ) {

            EntityInfo entityInfo = Entity.GetInfo( target );

            Result result = new Result();

            foreach (EntityPropertyInfo info in entityInfo.SavedPropertyList) {

                if (!info.SaveToDB) continue;


                if (checkLength(info)) {
                    Object val = target.get( info.Name );
                    if( val != null )
                        target.set( info.Name, strUtil.SubString( val.ToString(), info.SaveAttribute.Length ) );
                }

                foreach (ValidationAttribute vattr in info.ValidationAttributes) {
                    vattr.Validate( action, target, info.Name, result );
                }

            }

            return result;
        }

        private static Boolean checkLength( EntityPropertyInfo info ) {
            if (info.SaveAttribute == null) return false;
            if (info.SaveAttribute.LengthSetted() == false) return false;
            if (info.Type != typeof( String )) return false;
            return true;
        }

    }
}

