/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Reflection;
using System.Text;

using wojilu.DI;
using wojilu.Web.Mvc;
using wojilu.Common.Security;
using System.Collections.Generic;

namespace wojilu.Web.Controller.Security {


    public class SiteAdminOperation : ISecurityAction {

        private static readonly String rootNamespace = "wojilu.Web.Controller";

        public long Id { get; set; }
        public String Name { get; set; }
        public long ParentId { get; set; }

        // 保留属性，暂时无作用
        public int Depth { get; set; } 
        public String Url { get; set; }
        public String Tag { get; set; }

        public SiteAdminOperation() { }

        public SiteAdminOperation(long id, string name, long menuId, string url) {
            init( id, name, menuId );
            this.Url = url;
            checkLowerUrl();
        }

        public SiteAdminOperation(long id, string name, long menuId, Type controllerType) {

            init( id, name, menuId );
            initOperations( controllerType );
        }

        public SiteAdminOperation(long id, string name, long menuId, Type[] controllerTypes) {

            init( id, name, menuId );
            initOperations( controllerTypes );
        }

        public SiteAdminOperation(long id, string name, long menuId, aActionWithId action, string rootNamespace) {
            init( id, name, menuId );
            this.Url = SecurityUtils.getPath( action.Method, rootNamespace );
            checkLowerUrl();
        }

        public SiteAdminOperation(long id, string name, long menuId, aAction action, string rootNamespace) {
            init( id, name, menuId );
            this.Url = SecurityUtils.getPath( action.Method, rootNamespace );
            checkLowerUrl();
        }

        //--------------------------------------------------------------------------------------

        private void init(long id, string name, long parentId) {
            this.Id = id;
            this.Name = name;
            this.ParentId = parentId;
        }

        private void initOperations( Type controllerType ) {
            StringBuilder sb = new StringBuilder();
            addUrl( sb, controllerType, rootNamespace );
            this.Url = sb.ToString().TrimEnd( ';' );
            checkLowerUrl();
        }


        private void initOperations( Type[] controllerTypes ) {

            StringBuilder sb = new StringBuilder();
            foreach (Type controllerType in controllerTypes) {
                addUrl( sb, controllerType, rootNamespace );
            }
            this.Url = sb.ToString().TrimEnd( ';' );
            checkLowerUrl();
        }

        private static void addUrl( StringBuilder sb, Type controllerType, String rootNamespace ) {
            MethodInfo[] methods = rft.GetMethodsWithInheritance( controllerType );
            foreach (MethodInfo method in methods) {
                String path = SecurityUtils.getPath( method, rootNamespace );
                sb.Append( path );
                sb.Append( ";" );
            }
        }


        private void checkLowerUrl() {
            if (MvcConfig.Instance.IsUrlToLower) {
                this.Url = this.Url.ToLower();
            }
        }


        //--------------------------------------------------------------------------------------

        private List<string> _urls;

        public virtual List<string> GetUrlList() {
            if (_urls == null) {
                List<string> results = getUrlList();
                _urls = results;
            }
            return _urls;

        }

        private List<string> getUrlList() {
            List<string> results = new List<string>();
            if (strUtil.HasText( this.Url )) {
                string[] arr = this.Url.Split( ';' );
                for (int i = 0; i < arr.Length; i++) {
                    if (strUtil.IsNullOrEmpty( arr[i] )) continue;
                    results.Add( arr[i].Trim() );
                }

            }
            return results;
        }

        public virtual String GetFirstUrl() {
            List<string> urls = GetUrlList();
            if (urls.Count > 0) {
                return urls[0];
            }
            return "#";
        }


        //-----------------------------------------------------------------------------
        
        public virtual ISecurityAction GetById(long id) {
            foreach (SiteAdminOperation op in OperationDB.GetInstance().SiteAdminOperations) {
                if (op.Id == id) return op as ISecurityAction;
            }
            return null;
        }

        public virtual IList findAll() {
            return OperationDB.GetInstance().SiteAdminOperations;
        }

        public virtual void insert() {
        }

        public virtual Result update() {
            return new Result();
        }

        public virtual void delete() {
        }


        public static List<SiteAdminOperation> GetOperationsByMenu(List<SiteAdminOperation> userActions, long menuId) {
            List<SiteAdminOperation> results = new List<SiteAdminOperation>();
            foreach (SiteAdminOperation action in userActions) {
                if (action.ParentId == menuId ) results.Add( action );
            }
            return results;
        }




    }

}
