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
    /// 数据列批注，用于标识属性在数据库中对应的列名称和长度
    /// </summary>
    [Serializable, AttributeUsage( AttributeTargets.Property )]
    public class ColumnAttribute : Attribute {
        private String _columnName;
        private String _label;
        private int _length;

        public ColumnAttribute() {
            _length = 250;
        }

        public String Name {
            get {
                return _columnName;
            }
            set {
                _columnName = value;
            }
        }

        public String Label {
            get {
                return _label;
            }
            set {
                _label = value;
            }
        }

        public int Length {
            get {
                return _length;
            }
            set {
                _length = value;
            }
        }

        public Boolean LengthSetted() {
            return _length > 0 && _length != 250;
        }


    }
}

