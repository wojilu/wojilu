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
using System.Reflection;

namespace wojilu.Web.Mvc {

    public class ControllerViewsPath : IViewsPath {

        private ControllerBase _controller;
        private String _action;

        private MethodInfo _method;

        public void setAction( String aAction ) {
            this._action = aAction;
        }

        public void setMethod( MethodInfo method ) {
            this._method = method;
            this._action = method.Name;
        }

        public string getPath( String viewsDir ) {
            return getTemplatePathByAction( this._action, viewsDir );
        }

        public ControllerBase getController() {
            return _controller;
        }

        public void setController( ControllerBase controller ) {
            _controller = controller;
        }

        /// <summary>
        /// 获取某 action 的模板文件的绝对路径(包括模板的后缀名)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="viewsDir"></param>
        /// <returns></returns>
        public String getTemplatePathByAction( String action, String viewsDir ) {
            return PathHelper.Map( getControllerDir( viewsDir ) + "/" + action + MvcConfig.Instance.ViewExt );
        }

        public string getPathNoDir() {
            return getControllerDirNoViewsDir() + "/" + this._action + MvcConfig.Instance.ViewExt;
        }

        private String getControllerDir( String viewsDir ) {

            String result = getControllerDirNoViewsDir();

            return strUtil.Join( viewsDir, result );
        }

        private String getControllerDirNoViewsDir() {

            Type declaringController = _method != null ? this._method.DeclaringType : _controller.GetType();

            String pathRaw = strUtil.GetTypeFullName( declaringController );

            // 去掉根目录
            String result = trimRootNamespace( pathRaw ).TrimStart( '.' );

            // 换成路径分隔符
            result = result.Replace( '.', '/' );
            result = strUtil.TrimEnd( result, "Controller" );
            return result;
        }

        private String trimRootNamespace( String pathRaw ) {

            foreach (String ns in MvcConfig.Instance.RootNamespace) {
                if (pathRaw.StartsWith( ns )) return strUtil.TrimStart( pathRaw, ns );
            }

            return pathRaw;
        }


    }



}
