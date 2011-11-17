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
    /// 拦截器接口，用户可以通过自定义拦截器，在插入前或插入后等动作中注入自己的处理逻辑
    /// </summary>
    public interface IInterceptor {

        void BeforInsert( IEntity entity );
        void AfterInsert( IEntity entity );

        void BeforUpdate( IEntity entity );
        void AfterUpdate( IEntity entity );

        void BeforUpdateBatch( Type t, String action, String condition );
        void AfterUpdateBatch( Type t, String action, String condition );

        void BeforDelete( IEntity entity );
        void AfterDelete( IEntity entity );

        void BeforDeleteBatch( Type t, String condition );
        void AfterDeleteBatch( Type t, String condition );

    }


}

