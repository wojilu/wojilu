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
using wojilu.ORM;
using wojilu.Reflection;

namespace wojilu {

    /// <summary>
    /// 可以被 ORM 持久化的对象，都自动实现了本接口
    /// </summary>
    public interface IEntity {

        /// <summary>
        /// 每一个持久化对象，都具有一个 Id 属性
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// 获取属性的值(并非通过反射，速度较快)
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        Object get( String propertyName );

        /// <summary>
        /// 设置属性的值(并非通过反射，速度较快)
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性的值</param>
        void set( String propertyName, Object propertyValue );

        /// <summary>
        /// 包括对象的元数据，以及在对象查询的时候需要的额外信息，不常用
        /// </summary>
        //ObjectInfo state { get; set; }
    }



}
