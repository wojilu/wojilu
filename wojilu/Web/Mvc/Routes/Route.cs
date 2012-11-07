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

using wojilu.Web.Context;
using wojilu.Common;

namespace wojilu.Web.Mvc.Routes {

    /// <summary>
    /// 解析后的路由数据的封装
    /// </summary>
    public class Route {

        private int _id;
        private String _owner;
        private String _ownerType;
        private String _controller;
        private String _action;
        private int _appId;
        private int _page;
        private String _query;
        private String _clearUrl;

        private Dictionary _items = new Dictionary();

        public int id { get { return _id; } }
        public String owner { get { return _owner; } }
        public String ownerType { get { return _ownerType; } }
        public String controller { get { return _controller; } }
        public String action { get { return _action; } }
        public int appId { get { return _appId; } }
        public int page { get { return _page; } }
        public String query { get { return _query; } }

        private int getId() { return _id; }
        public void setId( int id ) { _id = id; }

        private String getOwner() { return _owner; }
        public void setOwner( String owner ) { _owner = owner; }

        private String getOwnerType() { return _ownerType; }
        public void setOwnerType( String ownerType ) { _ownerType = ownerType; }

        private String getController() { return _controller; }
        public void setController( String controller ) { _controller = controller; }

        private String getAction() { return _action; }
        public void setAction( String action ) { _action = action; }

        private int getAppId() { return _appId; }
        public void setAppId( int appId ) { _appId = appId; }

        private int getPage() { return _page; }
        public void setPage( int page ) { _page = page; }

        private String getQuery() { return _query; }
        public void setQuery( String query ) { _query = query; }

        public String getCleanUrl() { return _clearUrl; }
        public void setCleanUrl( String cleanUrl ) { _clearUrl = cleanUrl; }

        public String getItem( String key ) {
            Object result = _items[key];
            return result == null ? null : result.ToString();
        }

        public void setItem( String key, int val ) {
            _items.Set( key, val );
        }

        public void setItem( String key, String val ) {
            _items.Set( key, val );
        }

        public String getCleanUrlWithoutOwner( MvcContext ctx ) {

            //if (ctx.Owner().obj is Site) return this.getCleanUrl();
            if (ctx.owner.obj.GetType().Equals( ConstString.SiteTypeFullName )) return this.getCleanUrl();

            if (this.getCleanUrl().Equals( ctx.owner.obj.Url )) return String.Empty;

            return strUtil.TrimStart( this.getCleanUrl(), MemberPath.GetPath( this.getOwnerType() ) + RouteTool.Separator[0] + ctx.owner.obj.Url )
                .TrimStart( RouteTool.Separator[0] )
                .TrimEnd( RouteTool.Separator[0] );
        }

        /// <summary>
        /// 包括根命名空间，以及 Controller 后缀
        /// </summary>
        /// <returns></returns>
        public String getControllerFullName() {

            String ns = getNamespace();

            if (strUtil.IsNullOrEmpty( ns ))
                return addRootNamespace( this.getController() );
            else
                return addRootNamespace( ns + "." + this.getController() );
        }

        public String getControllerNameWithoutRootNamespace() {
            String ns = getNamespace();

            if (strUtil.IsNullOrEmpty( ns ))
                return this.getController();
            else
                return ns + "." + this.getController();
        }

        /// <summary>
        /// 完整的controller和action名称，分隔符为点号.
        /// </summary>
        /// <returns></returns>
        public String getControllerAndActionFullName() {
            return strUtil.Join( getControllerFullName(), this.getAction(), "." );
        }


        /// <summary>
        /// 不包括根命名空间，不包括Controller后缀；路径之间使用“/”
        /// </summary>
        /// <returns></returns>
        public String getControllerAndActionPath() {
            String ns = getNamespace();
            String result;
            if (strUtil.IsNullOrEmpty( ns )) {
                result = this.getController() + "." + this.getAction();
            }
            else {
                result = ns + "." + this.getController() + "." + this.getAction();
            }
            return result.Replace( ".", "/" );
        }

        private String _rootNamespace;

        /// <summary>
        /// ControllerFactory.InitController 的时候设置
        /// </summary>
        /// <param name="rootNamespace">当前controller的根命名空间</param>
        public void setRootNamespace( String rootNamespace ) {
            _rootNamespace = rootNamespace;
        }

        /// <summary>
        /// 获取当前route中的当前controller的根命名空间，并非load中的controller的命名空间
        /// </summary>
        /// <returns></returns>
        public String getRootNamespace() {
            return _rootNamespace;
        }

        public String getRootNamespace( String typeFullName ) {

            foreach (String rootNs in MvcConfig.Instance.RootNamespace) {
                if (typeFullName.StartsWith( rootNs )) return rootNs;
            }

            return _rootNamespace;
        }

        private String addRootNamespace( String controller ) {
            return getRootNamespace() + "." + controller + "Controller";
        }

        private String _ns;

        public String ns { get { return getNamespace(); } }

        /// <summary>
        /// 不包括根命名空间，路径分隔符 / 已经被替换成点号 .
        /// </summary>
        /// <returns></returns>
        private String getNamespace() {
            return _ns;
        }

        public void setNs( String ns ) {
            _ns = ns;
        }

        public Boolean isInPath( String item ) {
            String ns = this.getNamespace();
            if (strUtil.IsNullOrEmpty( ns )) return false;
            return ns.IndexOf( item ) >= 0;
        }


        public Boolean isAdmin {
            get {

                String ns = this.getNamespace();

                if (strUtil.IsNullOrEmpty( ns )) return false;
                return isAdminPrivate( ns );
            }
        }

        private static bool isAdminPrivate( String ns ) {

            if (MvcConfig.Instance.IsUrlToLower) {
                if (ns.ToLower().Equals( "admin" )
                    || ns.ToLower().StartsWith( "admin." )
                    || ns.ToLower().EndsWith( ".admin" )) return true;
                return ns.ToLower().IndexOf( ".admin." ) >= 0;
            }
            else {

                if (ns.Equals( "Admin" ) || ns.StartsWith( "Admin." ) || ns.EndsWith( ".Admin" )) return true;
                return ns.IndexOf( ".Admin." ) >= 0;

            }
        }

        public Boolean isUserDataAdmin {

            get {

                String ns = this.getNamespace();

                if (strUtil.IsNullOrEmpty( ns )) return false;

                return ns.Equals( "Admin.Apps" ) || ns.StartsWith( "Admin.Apps." );

            }
        }


        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 将实际映射到的目标path存入 httpContext，方便 PageHelper 生成翻页链接
        /// </summary>
        /// <param name="path"></param>
        public static void setRoutePath( String path ) {
            if (path.StartsWith( "http" ) == false && path.StartsWith( "/" ) == false) {
                path = "/" + path;
            }
            CurrentRequest.setItem( "trueHttpPath", path );
        }

        public static String getRoutePath() {
            return getString( "trueHttpPath" );
        }

        private static String getString( String key ) {
            Object result = CurrentRequest.getItem( key );
            return result == null ? null : result.ToString();
        }



    }
}

