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
    /// rss КэѕЭПо
    /// </summary>
    public class RssItem {

        public int ItemId { get; set; }
        public String Author { get; set; }
        public String Category { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Link { get; set; }
        public DateTime PubDate { get; set; }

        public String DescriptionCDATA {
            get { return cdata( this.Description ); }
        }

        private static String cdata( String str ) {
            return "<![CDATA[" + str + "]]>";
        }


        private Hashtable _container;

        public Object this[Object key] {
            get {
                if (_container == null) {
                    return null;
                }
                return _container[key];
            }
            set {
                if (_container == null) {
                    _container = new Hashtable();
                }
                _container[key] = value;
            }
        }

    }
}

