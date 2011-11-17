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

namespace wojilu.ORM {

    /// <summary>
    /// 默认值批注，当属性没有被赋值的时候，系统使用此默认值存入数据库
    /// </summary>
    [Serializable, AttributeUsage( AttributeTargets.Property )]
    public class DefaultAttribute : Attribute {

        private Object _value;

        public DefaultAttribute( Object val ) {
            _value = val;
        }

        public Object Value {
            get { return _value; }
            set { _value = value; }
        }

    }

}

