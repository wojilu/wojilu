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
using System.Text;

using wojilu.Web.Mvc.Routes;
using wojilu.Web.Context;

using wojilu.Members.Interface;
using wojilu.Common;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Mvc {


    /// <summary>
    /// 带上下文(context=ctx)的链接生成工具
    /// </summary>
    [MvcLink]
    public class CtxLink {

        private int _appId;
        private IMember _owner;

        public CtxLink( MvcContext ctx ) {
            _appId = ctx.route.appId;
            if (ctx.owner != null) _owner = ctx.owner.obj;
        }

        public String To( aAction action ) {
            return To( action, _appId );
        }

        public String To( aAction action, int appId ) {
            return Link.To( _owner, getController( action.Target.GetType() ), action.Method.Name, -1, appId );
        }

        public String To( aActionWithId action, int id ) {
            return To( action, id, _appId );
        }

        public String To( aActionWithId action, int id, int appId ) {
            return Link.To( _owner, getController( action.Target.GetType() ), action.Method.Name, id, appId );
        }

        //---------------------------------------------------------------------------

        public String T2( aAction action ) {
            return Link.To( _owner, getController( action.Target.GetType() ), action.Method.Name, 0, 0 );
        }

        public String T2( aActionWithId action, int id ) {
            return Link.To( _owner, getController( action.Target.GetType() ), action.Method.Name, id, 0 );
        }

        private static String getController( Type controllerType ) {
            return trimRootNamespace( strUtil.GetTypeFullName( controllerType ) )
                .TrimStart( '.' )
                .Replace( ".", MvcConfig.Instance.UrlSeparator );
        }

        private static String trimRootNamespace( String typeFullName ) {

            foreach (String ns in MvcConfig.Instance.RootNamespace) {
                if (typeFullName.StartsWith( ns )) return strUtil.TrimStart( typeFullName, ns );
            }

            return typeFullName;
        }

    }
}

