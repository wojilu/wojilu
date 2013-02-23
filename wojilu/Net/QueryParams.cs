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
using System.Collections.Generic;
using System.Text;

namespace wojilu.Net {

    public class QueryParams : ICollection<QueryParam> {

        public static QueryParams Init( string queryString ) {

            QueryParams items = new QueryParams();
            if (queryString.Contains( "?" )) {
                queryString = queryString.Substring( queryString.IndexOf( "?" ) + 1 );
            }

            String[] arrQuery = queryString.Split( '&' );
            foreach (String x in arrQuery) {
                string[] arrPair = x.Split( '=' );
                if (arrPair.Length != 2) continue;
                items.Add( new QueryParam( arrPair[0], arrPair[1] ) );
            }

            return items;
        }

        private List<QueryParam> _items = new List<QueryParam>();

        public void Add( QueryParam item ) {
            _items.Add( item );
        }

        public void Add( string name, string value ) {
            if (value == null) return;
            _items.Add( new QueryParam( name, value ) );
        }

        public string this[string key] {
            get { return getByKey( key ); }
            set {
                foreach (QueryParam x in _items) {
                    if (x.Key == key) _items.Remove( x );
                }
                _items.Add( new QueryParam( key, value ) );
            }
        }

        public bool HasName( string name ) {
            foreach (QueryParam x in _items) {
                if (x.Key == name) return true;
            }
            return false;
        }

        public string Get( string key ) {
            return getByKey( key );
        }

        private string getByKey( string key ) {
            foreach (QueryParam x in _items) {
                if (x.Key == key) return x.Value;
            }
            return null;
        }

        public void Clear() {
            _items.Clear();
        }

        public bool Contains( QueryParam item ) {
            return _items.Contains( item );
        }

        public void CopyTo( QueryParam[] array, int arrayIndex ) {
            _items.CopyTo( array, arrayIndex );
        }

        public int Count {
            get { return _items.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove( QueryParam item ) {
            return _items.Remove( item );
        }

        public IEnumerator<QueryParam> GetEnumerator() {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _items.GetEnumerator();
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            foreach (QueryParam x in _items) {
                sb.Append( "&" );
                sb.AppendFormat( "{0}={1}", x.Key, x.Value );
            }
            return sb.ToString().Substring( 1 );
        }

        public string ToEncodedString() {
            StringBuilder sb = new StringBuilder();
            foreach (QueryParam x in _items) {
                if (x.Value == null) continue;
                sb.Append( "&" );
                sb.Append( System.Web.HttpUtility.UrlEncode( x.Key ) );
                sb.Append( "=" );
                sb.Append( System.Web.HttpUtility.UrlEncode( x.Value ) );
            }
            return sb.ToString().Substring( 1 );
        }

    }

}
