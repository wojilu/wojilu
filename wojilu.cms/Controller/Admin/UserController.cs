using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.cms.Domain;

namespace wojilu.cms.Controller.Admin {

    public class UserController : ControllerBase {

        public void Index() {
            List<User> list = User.findAll();
            bindList( "list", "c", list, bindLink );
        }

        private void bindLink( IBlock block, int id ) {
            block.Set( "c.EditLink", to( Edit, id ) );
            block.Set( "c.DeleteLink", to( Delete, id ) );
        }

        public void Add() {
            target( Create );
        }

        public void Create() {
            User c = ctx.PostValue<User>();
            if (ctx.HasErrors) {
                run( Add );
                return;
            }
            db.insert( c );
            redirect( Index );
        }

        public void Edit( int id ) {
            target( Update, id );
            User c = User.findById( id );
            bind( c );
        }

        public void Update( int id ) {
            User c = User.findById( id );
            c = ctx.PostValue( c ) as User;
            db.update( c );
            redirect( Index );
        }

        public void Delete( int id ) {
            User c = User.findById( id );
            db.delete( c );
            redirect( Index );
        }
    }

}
