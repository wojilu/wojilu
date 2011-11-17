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

namespace wojilu.ORM {

    /// <summary>
    /// 对象查询的附件信息
    /// </summary>
    [Serializable]
    public class ObjectInfo {


        private int _cacheMinutes;
        private String _order;
        private ObjectPage pager;
        private Boolean _findChild;

        private EntityInfo _entityInfo;
        private Includer _includer;

        public ObjectInfo() {
            init();
        }

        public ObjectInfo( EntityInfo entity ) {
            init();
            _entityInfo = entity;
        }

        public ObjectInfo( Type t ) {
            init();
            _entityInfo = Entity.GetInfo( t );
        }

        public ObjectInfo( String typeFullName ) {
            init();
            _entityInfo = Entity.GetInfo( typeFullName );
        }

        private void init() {
            _order = "desc";
            _findChild = true;
            pager = new ObjectPage();
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

                if (val.Equals( "asc")) {
                    _order = "asc";
                }
                else if (val.Equals( "desc" )) {
                    _order = "desc";
                }

            }
        }

        public ObjectPage Pager {
            get { return pager; }
            set { pager = value; }
        }


    }
}

