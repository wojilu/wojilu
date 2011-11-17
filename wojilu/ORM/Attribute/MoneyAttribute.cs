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
    /// 货币批注，可以保存货币类型数据。
    /// 此批注只能用在dotnet的decimal数据类型上。数据库存储的时候，使用的精度为：总位数19, 小数点后4位
    /// </summary>
    [Serializable, AttributeUsage( AttributeTargets.Property )]
    public class MoneyAttribute : Attribute {

    }






}

