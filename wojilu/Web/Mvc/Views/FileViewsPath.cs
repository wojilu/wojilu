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
using wojilu.Web.Context;

namespace wojilu.Web.Mvc {

    public class FileViewsPath : IViewsPath {

        private String _fileName;
        private ControllerBase _controller;

        public void setFileName( String afileName ) {
            this._fileName = afileName;
        }

        public string getPath( string viewsDir ) {
            return getTemplatePathByFile( this._fileName, viewsDir );
        }

        public String getTemplatePathByFile( String fileName, String viewsDir ) {
            return PathHelper.Map( strUtil.Join( viewsDir, fileName ) + MvcConfig.Instance.ViewExt );
        }

        public ControllerBase getController() {
            return _controller;
        }

        public void setController( ControllerBase controller ) {
            _controller = controller;
        }

        public string getPathNoDir() {
            return strUtil.Join( _fileName, MvcConfig.Instance.ViewExt );
        }
    }


}
