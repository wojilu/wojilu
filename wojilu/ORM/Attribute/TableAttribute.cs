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
    /// 表名称批注，用于标识对象在数据库中对应的表名称
    /// </summary>
    [Serializable, AttributeUsage( AttributeTargets.Class )]
    public class TableAttribute : Attribute {

        private String _tableName;

        public TableAttribute( String tableName ) {
            _tableName = tableName;
        }

        public String TableName {
            get {
                return _tableName;
            }
            set {
                _tableName = value;
            }
        }

    }
}

