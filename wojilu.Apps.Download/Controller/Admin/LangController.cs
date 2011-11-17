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
    public class LangController : ControllerBase {


        public void List() {

            set( "addLink", to( Add ) );
            set( "sortAction", to( SaveSort ) );

            List<FileLang> list = FileLang.GetAll();
            bindList( "list", "data", list, bindLink );
        }

        private void bindLink( IBlock block, int id ) {
            block.Set( "data.LinkEdit", to( Edit, id ) );
            block.Set( "data.LinkDelete", to( Delete, id ) );
        }

        [HttpPost]
        public virtual void SaveSort() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            FileLang data = FileLang.GetById( id );

            List<FileLang> list = FileLang.GetAll();

            if (cmd == "up") {

                new SortUtil<FileLang>( data, list ).MoveUp();
                echoRedirect( "ok" );
            }
            else if (cmd == "down") {

                new SortUtil<FileLang>( data, list ).MoveDown();
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

            FileLang pf = new FileLang();
            pf.Name = name;
            pf.insert();

            echoRedirect( lang( "opok" ), List );
        }

        public void Edit( int id ) {
            target( Update, id );

            FileLang pf = FileLang.GetById( id );
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

            FileLang pf = FileLang.GetById( id );
            pf.Name = name;
            pf.update();

            echoRedirect( lang( "opok" ), List );
        }

        [HttpDelete]
        public void Delete( int id ) {

            FileLang f = FileLang.GetById( id );
            if (f != null) {
                f.delete();
                redirect( List );
            }

        }
    }

}

