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
using wojilu.ORM.Caching;
using wojilu.DI;

namespace wojilu.ORM {

    /// <summary>
    /// 对象查询的附件信息
    /// </summary>
    [Serializable]
    public class ObjectInfo {

        private int _cacheMinutes;
        private String _order;
        private PageHelper pager;
        private Boolean _findChild;

        private EntityInfo _entityInfo;
        private Includer _includer;

        public ObjectInfo( EntityInfo entity ) {
            init( entity );
        }

        public ObjectInfo( Type t ) {

            if (t == null) throw new ArgumentNullException( "Type" );

            EntityInfo entityInfo = Entity.GetInfo( t );
            if (entityInfo == null) throw new TypeNotFoundException( string.Format( "ORM程序集中没有找到类型:{0}, 请检查 orm.config 中是否正确配置 AssemblyList", t.FullName ) );

            init( entityInfo );
        }

        public ObjectInfo( String typeFullName ) {

            if (typeFullName == null) throw new ArgumentNullException( "typeFullName" );

            EntityInfo entityInfo = Entity.GetInfo( typeFullName );
            if (entityInfo == null) throw new TypeNotFoundException( string.Format( "ORM程序集中没有找到类型:{0}, 请检查 orm.config 中是否正确配置 AssemblyList", typeFullName ) );

            init( entityInfo );
        }

        private void init( EntityInfo entityInfo ) {

            if (entityInfo == null) throw new ArgumentNullException( "EntityInfo" );
            _entityInfo = entityInfo;

            _order = "desc";
            _findChild = true;
            pager = new PageHelper();
        }

        public ObjectInfo Copy( ObjectInfo state ) {
            CacheMinutes = state.CacheMinutes;
            Order = state.Order;
            IsFindChild = state.IsFindChild;
            return this;
        }

        public int CacheMinutes {
            get { return _cacheMinutes; }
            set { _cacheMinutes = value; }
        }

        public void cache() {
            _cacheMinutes = CacheTime.CacheForever;
        }

        public void cache( int minutes ) {
            _cacheMinutes = minutes;
        }

        public EntityInfo EntityInfo {
            get { return _entityInfo; }
            set { _entityInfo = value; }
        }

        public Includer Includer {
            get {
                if (_includer == null) {
                    _includer = new Includer( EntityInfo.Type );
                }
                return _includer;
            }
            set { _includer = value; }
        }

        public void include( String propertyList ) {
            this.Includer.Include( propertyList );
        }

        public void includeAll() {
            this.Includer.IncludeAll();
        }

        public void select( String selectProperty ) {
            this.Includer.Select( selectProperty );
        }

        public Boolean IsFindChild {
            get { return _findChild; }
            set { _findChild = value; }
        }

        /// <summary>
        /// 只接受两种赋值：asc或者desc
        /// </summary>
        public String Order {
            get { return _order; }

            set {

                if (strUtil.IsNullOrEmpty( value )) return;

                String val = value.Trim().ToLower();

                if (val.Equals( "asc" )) {
                    _order = "asc";
                }
                else if (val.Equals( "desc" )) {
                    _order = "desc";
                }

            }
        }

        public PageHelper Pager {
            get { return pager; }
            set { pager = value; }
        }


    }
}

