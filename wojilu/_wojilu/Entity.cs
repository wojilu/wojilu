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
using wojilu.Reflection;
using wojilu.ORM;

namespace wojilu {

    /// <summary>
    /// 实体类常用方法
    /// </summary>
    public class Entity {

        /// <summary>
        /// 根据类型全名，创建持久化对象
        /// </summary>
        /// <param name="typeFullName">类型的全名</param>
        /// <returns>返回一个 IEntity 持久化对象</returns>
        public static IEntity New( String typeFullName ) {
            IConcreteFactory factory = (IConcreteFactory)MappingClass.Instance.FactoryList[typeFullName];
            return factory == null ? null : factory.New();
        }

        /// <summary>
        /// 获取类型 t 的元数据信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static EntityInfo GetInfo( Type t ) {
            return MappingClass.Instance.ClassList[t.FullName] as EntityInfo;
        }

        /// <summary>
        /// 获取类型 typeFullName 的元数据信息
        /// </summary>
        /// <param name="typeFullName">类型全名，包括namespace，但不包括程序集</param>
        /// <returns></returns>
        public static EntityInfo GetInfo( String typeFullName ) {
            return MappingClass.Instance.ClassList[typeFullName] as EntityInfo;
        }

        /// <summary>
        /// 获取对象 obj 的元数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static EntityInfo GetInfo( Object obj ) {
            return GetInfo( obj.GetType() );
        }

        /// <summary>
        /// 根据全名获取类型
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static Type GetType( String typeFullName ) {
            return MappingClass.Instance.TypeList[typeFullName] as Type;
        }


    }

}
