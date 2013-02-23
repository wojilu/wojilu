using System;
using System.Collections.Generic;
using System.Text;
using wojilu.OAuth;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Admin.Sys {

    public class ConnectAdminController : ControllerBase {

        public void Index() {

            List<AuthConnectConfig> list = AuthConnectConfig.GetAll();

            list.ForEach( x => {
                x.data["StatusStr"] = (x.IsStop == 1 ? "已停用" : "");
                x.data["StopCmd"] = (x.IsStop == 1 ? "启用" : "停用");
                x.data["stop"] = to( Stop, x.Id );

                x.data["PickStr"] = (x.IsPick == 1 ? "★" : "");
                x.data["PickCmd"] = (x.IsPick == 1 ? "取消前置" : "前置");
                x.data["pick"] = to( Pick, x.Id );

                x.data["LoginName"] = strUtil.HasText( x.LoginName ) ? "<span class=\"note\">("+x.LoginName+")</span>" : "";

                x.data.edit = to( Edit, x.Id );
                x.data.delete = to( Delete, x.Id );
            } );

            set( "addLink", to( Add ) );
            set( "sortAction", to( SaveSort ) );
            bindList( "list", "x", list );
            set( "totalCount", list.Count );
        }

        [HttpPost, DbTransaction]
        public virtual void SaveSort() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            AuthConnectConfig data = AuthConnectConfig.GetById( id );
            String condition = (ctx.app == null ? "" : "AppId=" + ctx.app.Id);

            List<AuthConnectConfig> list = AuthConnectConfig.GetAll();

            if (cmd == "up") {

                new SortUtil<AuthConnectConfig>( data, list ).MoveUp();
                echoRedirect( "ok" );
            }
            else if (cmd == "down") {

                new SortUtil<AuthConnectConfig>( data, list ).MoveDown();
                echoRedirect( "ok" );
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }

        }


        public void Add() {
            target( Create );
        }

        [HttpPost]
        public void Create() {

            AuthConnectConfig x = ctx.PostValue<AuthConnectConfig>( "x" );
            validatePost( x );

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            x.insert();

            echoToParentPart( lang( "opok" ) );
        }

        public void Edit( int id ) {
            AuthConnectConfig x = AuthConnectConfig.GetById( id );
            bind( "x", x );
            target( Update, id );
        }

        [HttpPost]
        public void Update( int id ) {
            AuthConnectConfig x = AuthConnectConfig.GetById( id );
            x = ctx.PostValue( x, "x" ) as AuthConnectConfig;
            validatePost( x );

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            x.update();

            echoToParentPart( lang( "opok" ) );
        }

        private void validatePost( AuthConnectConfig x ) {
            if (strUtil.IsNullOrEmpty( x.Name )) errors.Add( "请填写名称" );
            if (strUtil.IsNullOrEmpty( x.TypeFullName )) errors.Add( "请填写TypeFullName" );
            if (strUtil.IsNullOrEmpty( x.ConsumerKey )) errors.Add( "请填写ConsumerKey" );
            if (strUtil.IsNullOrEmpty( x.ConsumerSecret )) errors.Add( "请填写ConsumerSecret" );
        }

        [HttpPost]
        public void Stop( int id ) {
            AuthConnectConfig x = AuthConnectConfig.GetById( id );

            if (x.IsStop == 1) {
                x.IsStop = 0;
            }
            else {
                x.IsStop = 1;
            }

            x.update();

            echoResult( null );
        }


        [HttpPost]
        public void Pick( int id ) {
            AuthConnectConfig x = AuthConnectConfig.GetById( id );

            if (x.IsPick == 1) {
                x.IsPick = 0;
            }
            else {
                x.IsPick = 1;
            }

            x.update();

            echoResult( null );
        }

        [HttpDelete]
        public void Delete( int id ) {
            AuthConnectConfig x = AuthConnectConfig.GetById( id );
            x.delete();
            echoResult( null );
        }

    }

}
