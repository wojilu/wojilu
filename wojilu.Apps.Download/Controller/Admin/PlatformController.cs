using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Download.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;


namespace wojilu.Web.Controller.Download.Admin {

    [App( typeof( DownloadApp ) )]
    public class PlatformController : ControllerBase {


        public void List() {

            set( "addLink", to( Add ) );
            set( "sortAction", to( SaveSort ) );

            List<Platform> list = Platform.GetAll();
            bindList( "list", "data", list, bindLink );

            //DataPage<Platform> re = cdb.findPage<Platform>( 6 );
            //bindList( "list", "data", re.Results, bindLink );
            //set( "page", re.PageBar );

        }

        private void bindLink( IBlock block, int id ) {
            block.Set( "data.LinkEdit", to( Edit, id ) );
            block.Set( "data.LinkDelete", to( Delete, id ) );
        }

        [HttpPost]
        public virtual void SaveSort() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            Platform data = Platform.GetById( id );

            List<Platform> list = Platform.GetAll();

            if (cmd == "up") {

                new SortUtil<Platform>( data, list ).MoveUp();
                echoRedirect( "ok" );
            }
            else if (cmd == "down") {

                new SortUtil<Platform>( data, list ).MoveDown();
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
            string name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name )) {
                errors.Add( "请填写名称" );
                run( Add );
                return;
            }

            Platform pf = new Platform();
            pf.Name = name;
            pf.insert();

            echoRedirect( lang( "opok" ), List );
        }

        public void Edit( int id ) {
            target( Update, id );

            Platform pf = Platform.GetById( id );
            set( "Name", pf.Name );
        }

        [HttpPost]
        public void Update( int id ) {

            string name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name )) {
                errors.Add( "请填写名称" );
                run( Edit, id );
                return;
            }

            Platform pf = Platform.GetById( id );
            pf.Name = name;
            pf.update();

            echoRedirect( lang( "opok" ), List );
        }

        [HttpDelete]
        public void Delete( int id ) {

            Platform f = Platform.GetById( id );
            if (f != null) {
                f.delete();
                redirect( List );
            }

        }
    }

}

