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
    public class LicenseController : ControllerBase {


        public void List() {

            set( "addLink", to( Add ) );
            set( "sortAction", to( SaveSort ) );

            List<LicenseType> list = LicenseType.GetAll();
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

            LicenseType data = LicenseType.GetById( id );

            List<LicenseType> list = LicenseType.GetAll();

            if (cmd == "up") {

                new SortUtil<LicenseType>( data, list ).MoveUp();
                echoRedirect( "ok" );
            }
            else if (cmd == "down") {

                new SortUtil<LicenseType>( data, list ).MoveDown();
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
                echoError( "请填写名称" );
                return;
            }

            LicenseType lt = new LicenseType();
            lt.Name = name;
            lt.insert();

            echoToParentPart( lang( "opok" ) );
        }

        public void Edit( int id ) {
            target( Update, id );

            LicenseType lt = LicenseType.GetById( id );
            set( "Name", lt.Name );
        }

        [HttpPost]
        public void Update( int id ) {

            string name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name )) {
                echoError( "请填写名称" );
                return;
            }

            LicenseType lt = LicenseType.GetById( id );
            lt.Name = name;
            lt.update();

            echoToParentPart( lang( "opok" ) );
        }

        [HttpDelete]
        public void Delete( int id ) {

            LicenseType lt = LicenseType.GetById( id );
            if (lt != null) {
                lt.delete();
                redirect( List );
            }

        }
    }

}
