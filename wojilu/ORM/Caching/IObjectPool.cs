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
using System.Collections;
using wojilu.ORM.Operation;

namespace wojilu.ORM.Caching {

    internal interface IObjectPool {

         void Add( IEntity obj );
         void AddAll( Type t, IList objList );
         void AddQueryList( Type t, String sql, Dictionary<String, Object> parameters, IList objList );
         void AddPage( Type t, String condition, PageHelper pager, IList list );
         void AddCount( Type t, int count );
         void AddCount( Type t, String condition, int count );
         void AddSqlList( String sql, IList objList );

         IEntity FindOne( Type t, int id );
         IList FindAll( Type t );
         IList FindByQuery( ConditionInfo condition );
         IList FindByQuery( Type type, String _queryString, Dictionary<String, Object> _namedParameters );
         IPageList FindPage( Type t, String condition, PageHelper pager );
         IList FindBySql( String sql, Type t );
         int FindCount( Type t );
         int FindCount( Type t, String condition );

         void Delete( Type t, int id );

   }



}
