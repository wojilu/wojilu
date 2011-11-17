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

namespace wojilu.Web.Mvc.Routes {

    public class DefaultRouteValue {

        private String _controller;
        private String _action;
        private int _id;
        private int _appId;
        private String _owner;
        private String _ownerType;
        private int _page;
        private String _query;

        private String _ns;

        public String getNs() { return _ns; }
        public void setNs( String ns ) { _ns = ns; }

        public String getController() { return _controller; }
        public void setController( String controller ) {

            if (strUtil.IsNullOrEmpty( controller )) return;

            if (controller.IndexOf( "." ) > 0) {
                setNsAndController( controller );
            }
            else
                _controller = controller;
        }

        private void setNsAndController( String controller ) {
            String[] arr = controller.Split( '.' );
            for (int i = 0; i < arr.Length - 1; i++) {
                if (i > 0) _ns += ".";
                _ns += arr[i];
            }
            _controller = arr[arr.Length - 1];
        }

        public String getAction() { return _action; }
        public void setAction( String action ) { _action = action; }

        public int getId() { return _id; }
        public void setId( int id ) { _id = id; }

        public int getAppId() { return _appId; }
        public void setAppId( int appId ) { _appId = appId; }

        public String getOwner() { return _owner; }
        public void setOwner( String owner ) { _owner = owner; }

        public String getOwnerType() { return _ownerType; }
        public void setOwnerType( String ownerType ) { _ownerType = ownerType; }

        public int getPage() { return _page; }
        public void setPage( int page ) { _page = page; }

        public String getQuery() { return _query; }
        public void setQuery( String query ) { _query = query; }

        public DefaultRouteValue() {
            this.init();
        }

        public DefaultRouteValue( String controller ) {
            this.init();
            _controller = controller;
        }

        public DefaultRouteValue( String controller, String action ) {
            this.init();
            _controller = controller;
            _action = action;
        }

        private void init() {
            _owner = "site";
            _ownerType = "site";
            _controller = "";
            _action = "";
            _query = "";
            _page = 1;
            _appId = 0;
        }




    }
}

