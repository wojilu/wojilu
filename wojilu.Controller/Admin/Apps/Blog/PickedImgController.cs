using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Blog.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin.Apps.Blog {

    [App( typeof( BlogApp ) )]
    public class PickedImgController : ControllerBase {

        public void Index() {

            set( "addUrl", to( Add ) );

            int imgCount = 6;
            DataPage<BlogPickedImg> list = BlogPickedImg.findPage( "", imgCount );

            IBlock block = getBlock( "list" );
            foreach (BlogPickedImg f in list.Results) {
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

            BlogPickedImg f = ctx.PostValue<BlogPickedImg>();
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

            BlogPickedImg f = BlogPickedImg.findById( id );
            if (f == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            bind( "f", f );
        }

        [HttpPost]
        public void Update( int id ) {

            BlogPickedImg f = BlogPickedImg.findById( id );
            if (f == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            f = ctx.PostValue( f ) as BlogPickedImg;
            Result result = f.update();
            if (result.HasErrors)
                run( Add );
            else
                echoToParentPart( lang( "opok" ) );

        }

        [HttpDelete]
        public void Delete( int id ) {

            BlogPickedImg f = BlogPickedImg.findById( id );
            if (f == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            f.delete();

            echoRedirect( lang( "opok" ) );

        }

    }

}
