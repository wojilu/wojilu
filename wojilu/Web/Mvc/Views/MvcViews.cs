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

    public class MvcViews {

        private static readonly ILog logger = LogManager.GetLogger( typeof( MvcViews ) );

        private ControllerBase _controller;

        public ControllerBase getController() {
            return _controller;
        }

        public void setController( ControllerBase controller ) {
            _controller = controller;
        }

        /// <summary>
        /// 根据 action 名称获取模板对象
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Template getTemplateByAction( String action ) {
            ControllerViewsPath x = new ControllerViewsPath();

            // TODO 检查action是否属于controller，如果是继承来的，则获取基类
            x.setController( this._controller );
            x.setAction( action );

            return getTemplateByPath( x );
        }

        public Template getTemplateByAction( MethodInfo actionMethod ) {
            ControllerViewsPath x = new ControllerViewsPath();

            // TODO 检查action是否属于controller，如果是继承来的，则获取基类
            x.setController( this._controller );
            x.setMethod( actionMethod );

            return getTemplateByPath( x );
        }

        /// <summary>
        /// 根据文件名称获取模板对象，文件名必须从视图 view 的根目录算起
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Template getTemplateByFileName( String fileName ) {
            FileViewsPath x = new FileViewsPath();
            x.setController( this._controller );
            x.setFileName( fileName );
            return getTemplateByPath( x );
        }

        public Template getTemplateByPath( IViewsPath viewsPath ) {

            List<String> dirList = MvcConfig.Instance.ViewDirList;
            if (dirList.Count == 0) {
                return new Template( viewsPath.getPath( MvcConfig.Instance.ViewDir ) );
            }

            StringBuilder notFoundPath = new StringBuilder();
            foreach (String dir in dirList) {

                MvcContext ctx = _controller == null ? null : _controller.ctx;

                if (isSkipDir( ctx, dir )) continue;

                String xPath = viewsPath.getPath( dir );
                if (Template.ContainsCache( xPath )) return new Template( xPath );

                Template x = new Template( xPath );
                if (x.IsTemplateExist()) return x;

                notFoundPath.Append( xPath );
                notFoundPath.Append( "<br/>" );
            }

            Template ret = new Template();
            ret.NoTemplates( notFoundPath.ToString() );
            return ret;
        }

        private Boolean isSkipDir( MvcContext ctx, String dir ) {

            foreach (IViewsFilter x in ViewsFilterLoader.GetFilterList()) {
                if (x.IsApplyViews( ctx, dir ) == false) {
                    logger.Info( "skip viewsDir '" + dir + "' by filter '" + x.GetType().FullName + "'" );
                    return true;
                }
            }

            return false;
        }

    }


}
