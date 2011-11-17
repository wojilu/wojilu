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


namespace wojilu {

    /// <summary>
    /// DTO(Data Transfer Object) 的接口
    /// </summary>
    public interface IDto {

        /// <summary>
        /// 将实体类赋值给DTO对象
        /// </summary>
        /// <param name="obj"></param>
        void Init( IEntity obj );

        /// <summary>
        /// 从DTO中获取实体类
        /// </summary>
        /// <returns></returns>
        IEntity GetEntity();

        /// <summary>
        /// 创建一个新对象
        /// </summary>
        /// <returns></returns>
        IDto New();

    }

    public interface IDtoFactory {
        IDto CreateDto( String entityTypeName );
        Dictionary<String, IDto> GetDtoMap();
    }

}
