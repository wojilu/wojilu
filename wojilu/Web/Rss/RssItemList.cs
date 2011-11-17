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

namespace wojilu.Web {

    /// <summary>
    /// rss 数据列表
    /// </summary>
    public class RssItemList : CollectionBase {

        public int Add( RssItem rssItem ) {
            return base.List.Add( rssItem );
        }

        public Boolean Contains( RssItem value ) {
            return base.List.Contains( value );
        }

        public int IndexOf( RssItem value ) {
            return base.List.IndexOf( value );
        }

        public void Insert( int index, RssItem value ) {
            base.List.Insert( index, value );
        }

        public void Remove( RssItem value ) {
            base.List.Remove( value );
        }

        public RssItem this[int index] {
            get {
                return (RssItem)base.List[index];
            }
            set {
                base.List[index] = value;
            }
        }

    }
}

