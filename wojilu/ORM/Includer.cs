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
using System.Collections;

namespace wojilu.ORM {

    /// <summary>
    /// 查询属性等附件信息
    /// </summary>
    [Serializable]
    public class Includer {

        private String _selectedProperty;
        private ArrayList _EntityPropertyList;
        private EntityInfo entityInfo;

        public Includer( EntityInfo _entityInfo ) {
            _selectedProperty = "*";
            entityInfo = _entityInfo;
        }

        public Includer( Type t ) {
            _selectedProperty = "*";
            entityInfo = Entity.GetInfo( t );
        }

        public static Hashtable getSelectColumnFromInclude( IList includeEntityPropertyList ) {

            if (includeEntityPropertyList == null) return null;

            Hashtable hashtable = new Hashtable();
            foreach (EntityPropertyInfo info in includeEntityPropertyList) {
                hashtable[info.Name] = "*";
            }
            return hashtable;
        }

        public void Include( String propertyList ) {
            if (strUtil.HasText( propertyList ) && ((_EntityPropertyList == null) || (_EntityPropertyList.Count <= 0))) {
                _EntityPropertyList = new ArrayList();
                String[] strArray = propertyList.Split( new char[] { ',' } );
                foreach (EntityPropertyInfo info in entityInfo.EntityPropertyList) {
                    foreach (String str in strArray) {
                        if (strUtil.HasText( str ) && (str.Equals( info.Name ) && info.IsEntity)) {
                            _EntityPropertyList.Add( info );
                            break;
                        }
                    }
                }
            }
        }

        public void IncludeAll() {
            if ((_EntityPropertyList == null) || (_EntityPropertyList.Count <= 0)) {
                _EntityPropertyList = new ArrayList();
                foreach (EntityPropertyInfo info in entityInfo.EntityPropertyList) {
                    _EntityPropertyList.Add( info );
                }
            }
        }

        public void Select( String selectProperty ) {
            _selectedProperty = selectProperty;
            if (strUtil.HasText( selectProperty )) {
                Include( SqlBuilder.GetIncludeProperty( _selectedProperty ) );
            }
            else {
                _EntityPropertyList = null;
            }
        }

        public IList EntityPropertyList {
            get { return _EntityPropertyList; }
        }

        public String SelectedProperty {
            get { return _selectedProperty; }
            set { _selectedProperty = value; }
        }


    }
}

