/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Url;

using wojilu.Common.Menus;
using wojilu.Common.MemberApp;
using wojilu.Common.AppBase;
using wojilu.Common.Security;

using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Service;
using wojilu.Members.Groups.Service;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;
using System.Collections.Generic;
using wojilu.DI;
using wojilu.Common;

namespace wojilu.Web.Context {


    public class AppContext : wojilu.Web.Context.IAppContext {

        public ISecurity SecurityObject { get; set; }

        private int _Id;
        private IMenu _menu;
        private object _obj;
        private String _url;
        private IMemberApp _useApp;

        private MvcContext ctx;

        public void setContext( MvcContext wctx ) {
            ctx = wctx;
        }

        public int Id {
            get { return this._Id; }
            set { this._Id = value; }
        }

        public object obj {
            get { return this._obj; }
            set { this._obj = value; }
        }

        public String Name {
            get {
                if (this.Menu != null) return this.Menu.Name;
                if (this.UserApp != null) return this.UserApp.Name;
                return "";
            }
        }
        private Type _appType;
        public Type getAppType() { return _appType; }
        public void setAppType( Type t ) { _appType = t; }

        //----------------------------------------------------------------------------------------

        public IMenu Menu {
            get {
                if (this._menu == null) {

                    if (this.UserApp == null) return null;

                    String userAppUrl = UrlConverter.clearUrl( this.UserApp, ctx );
                    this._menu = ServiceMap.GetMenuService( ctx.owner.obj.GetType() ).FindByApp( userAppUrl );
                }
                return this._menu;
            }
        }

        public String Url {
            get {
                if (this._url == null) {
                    this._url = this.getUrl();
                }
                return this._url;
            }
        }

        private String getUrl() {
            String str = this.getUrlFromMenu();
            if (str == null) {
                str = alink.ToApp( (IApp)this.obj, ctx );
            }
            return str;
        }

        private String getUrlFromMenu() {
            IMenu menu = this.Menu;
            if ((menu != null) && strUtil.HasText( menu.Url )) {
                return (strUtil.Join( sys.Path.Root, menu.Url ) + MvcConfig.Instance.UrlExt);
            }
            return null;
        }

        public IMemberApp UserApp {
            get {
                if (this._useApp == null) {
                    if (this.obj == null) return null;
                    this._useApp = ServiceMap.GetUserAppService( ctx.owner.obj.GetType() ).GetByApp( (IApp)this.obj );
                }
                return this._useApp;
            }
        }





    }
}

