using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Domain;
using wojilu.Common.Picks;

namespace wojilu.Web.Controller.Common {

    public abstract class PickImgBaseController<T> : ControllerBase where T : ImgPickBase {


        public abstract IPageList GetPage();


        public void Index() {

            set( "addUrl", to( Add ) );

            IPageList list = GetPage();

            IBlock block = getBlock( "list" );
            foreach (T f in list.Results) {
                block.Set( "f.Id", f.Id );
                block.Set( "f.Title", f.Title );
                block.Set( "f.Url", f.Url );
                block.Set( "f.ImgUrl", f.ImgUrl );
                block.Set( "f.Created", f.Created );
                block.Set( "f.EditLink", to( Edit, f.Id ) );
                block.Set( "f.DeleteLink", to( Delete, f.Id ) );
                block.Next();
            }
            set( "page", list.PageBar );

        }

        public void Add() {
            target( Create );
        }


        public void Create() {

            T f = ctx.PostValue<T>( "x" );
            f.AppId = ctx.app.Id;
            f.Creator = ctx.viewer.obj as User;


            Result result = f.insert();
            if (result.HasErrors)
                run( Add );
            else
                echoToParentPart( lang( "opok" ) );
        }


        public void Edit( int id ) {
            target( Update, id );

            T f = db.findById<T>( id );
            if (f == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            bind( "f", f );
        }

        [HttpPost]
        public void Update( int id ) {

            T f = db.findById<T>( id );
            if (f == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            f = ctx.PostValue( f, "x" ) as T;
            Result result = f.update();
            if (result.HasErrors)
                run( Add );
            else
                echoToParentPart( lang( "opok" ) );

        }

        [HttpDelete]
        public void Delete( int id ) {

            T f = db.findById<T>( id );
            if (f == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            f.delete();

            echoRedirect( lang( "opok" ) );

        }

    }

}
