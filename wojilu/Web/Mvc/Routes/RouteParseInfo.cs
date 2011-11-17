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

namespace wojilu.Web.Mvc.Routes {

    internal class RouteParseInfo {

        public RouteParseInfo( OwnerInfo ownerInfo, String[] arrPathRow ) {
            this._ownerInfo = ownerInfo;
            this._arrPathRow = arrPathRow;
        }

        private StringBuilder _ns = new StringBuilder();

        private OwnerInfo _ownerInfo;

        private String[] _arrPath;
        private String[] _arrPathRow;

        private int _appId;

        public StringBuilder getNamespace() { return _ns; }
        public void setNamespace( StringBuilder sb ) { _ns = sb; }

        public int getAppId() { return _appId; }
        public void setAppId( int id ) { _appId = id; }

        public String[] getPathArray() { return _arrPath; }
        public void setPathArray( String[] arrPath ) { _arrPath = arrPath; }

        public String[] getRowPathArray() { return _arrPathRow; }

        public OwnerInfo getOwnerInfo() { return _ownerInfo; }
        public void setOwnerInfo( OwnerInfo ownerInfo ) { _ownerInfo = ownerInfo; }
        

    }

}
