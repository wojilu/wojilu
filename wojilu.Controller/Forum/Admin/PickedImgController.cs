using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Forum.Admin {

    [App( typeof( ForumApp ) )]
    public class PickedImgController : ControllerBase {

        public void Index() {

            set( "addUrl", to( Add ) );
            DataPage<ForumPickedImg> list = ForumPickedImg.findPage( "AppId=" + ctx.app.Id );

            IBlock block = getBlock( "list" );
            foreach (ForumPickedImg f in list.Results) {
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

            ForumPickedImg f = ctx.PostValue<ForumPickedImg>();
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

            ForumPickedImg f = ForumPickedImg.findById( id );
            if (f == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            bind( "f", f );
        }

        [HttpPost]
        public void Update( int id ) {

            ForumPickedImg f = ForumPickedImg.findById( id );
            if (f == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            f = ctx.PostValue( f ) as ForumPickedImg;
            Result result = f.update();
            if (result.HasErrors)
                run( Add );
            else
                echoToParentPart( lang( "opok" ) );

        }

        [HttpDelete]
        public void Delete( int id ) {

            ForumPickedImg f = ForumPickedImg.findById( id );
            if (f == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            f.delete();

            redirect( Index );
        
        }

    }

}
