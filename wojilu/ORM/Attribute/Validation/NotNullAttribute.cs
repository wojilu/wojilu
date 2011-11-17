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
using System.Reflection;

namespace wojilu.ORM {

    /// <summary>
    /// 检查对象属性是否已经赋值，如果是字符串类型，empty的字符串也是不合法的
    /// </summary>
    [Serializable]
    public class NotNullAttribute : ValidationAttribute {

        public NotNullAttribute() {
        }

        public NotNullAttribute( String msg ) {
            base.Message = msg;
        }

        public override void Validate( String action, IEntity target, EntityPropertyInfo info, Result result ) {
            Object obj = target.get( info.Name );

            Boolean isNull = false;
            if (info.Type == typeof( String )) {
                if (obj == null) {
                    isNull = true;
                }
                else if (strUtil.IsNullOrEmpty( obj.ToString() )) {
                    isNull = true;
                }
            }
            else if (obj == null) {
                isNull = true;
            }

            if (isNull) {
                if (strUtil.HasText( this.Message )) {
                    result.Add( this.Message );
                }
                else {
                    EntityInfo ei = Entity.GetInfo( target );
                    String str = "[" + ei.FullName + "] : property \"" + info.Name + "\" ";
                    result.Add( str + "can not be null" );
                }
            }
        }

    }
}

