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
using System.Reflection;

namespace wojilu.Common.Resource {

    /// <summary>
    /// 属性数据列表，常用于下拉列表中
    /// </summary>
    public class PropertyCollection : CollectionBase {
        public int Add( PropertyItem item ) {
            return List.Add( item );
        }

        public Boolean Contains( PropertyItem item ) {
            return List.Contains( item );
        }

        public PropertyItem FindByValue( int val ) {
            foreach (PropertyItem item in List) {
                if (item.Value == val) {
                    return item;
                }
            }
            return null;
        }

        public String GetName( int val ) {
            PropertyItem item = FindByValue( val );
            if (item == null) return "";
            if (AppResource.IsSelectTip( item.Name )) return "";
            return item.Name;
        }

        public int IndexOf( PropertyItem item ) {
            return List.IndexOf( item );
        }

        public void Insert( int index, PropertyItem item ) {
            List.Insert( index, item );
        }

        public void Remove( PropertyItem item ) {
            List.Remove( item );
        }

        public PropertyItem this[int index] {
            get { return (PropertyItem)List[index]; }
            set { List[index] = value; }
        }
    }
}

