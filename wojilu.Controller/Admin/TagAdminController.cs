using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.Tags;
using wojilu.ORM;
using wojilu.Common.AppBase.Interface;
using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Admin {

    public class TagAdminController : ControllerBase {

        public override void Layout() {

            set( "ListUrl", to( Index ) );

            bindNavLink();

        }

        public void Toolbar() {

            set( "tagName", ctx.Get( "tag" ) );

            bindNavLink();

            target( DataList );
        }

        private void bindNavLink() {
            set( "recentLink", to( Index ) );
            set( "countDescLink", to( ListDesc ) );
            set( "countAscLink", to( ListAsc ) );
        }

        public void Index() {
            bindTags( "" );
        }

        public void ListDesc() {
            view( "Index" );
            bindTags( "order by DataCount desc, Id desc" );
        }

        public void ListAsc() {
            view( "Index" );
            bindTags( "order by DataCount asc, Id asc" );
        }

        private void bindTags( String condition ) {
            load( "toolbar", Toolbar );

            Tag maxTag = Tag.find( "order by DataCount desc" ).first();
            int count = maxTag == null ? 0 : maxTag.DataCount;
            set( "maxCount", count );

            DataPage<Tag> list = Tag.findPage( condition, 200 );

            bindTagList( count, list.Results, "tags" );
            set( "page", list.PageBar );
        }

        public void DataList(  ) {

            load( "toolbar", Toolbar );
            set( "tagName", ctx.Get( "tag" ) );

            Tag tag = Tag.find( "Name=:name" ).set( "name", ctx.Get( "tag" ) ).first();
            if (tag == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            DataPage<DataTagShip> list = DataTagShip.findPage( "TagId=" + tag.Id );

            bindResults( list );
        }

        [HttpDelete, DbTransaction]
        public void DeleteTag( int id ) {

            TagService.DeleteTag( id );
            echoAjaxOk();
        }

        public void DeleteData( int dataTagId ) {

            TagService.DeleteDataTag( dataTagId );
            echoAjaxOk();
        }
        

        private void bindTagList( int count, List<Tag> tags, String blockName ) {
            IBlock block = getBlock( blockName );
            foreach (Tag tag in tags) {

                block.Set( "tag.Id", tag.Id );
                block.Set( "tag.Name", tag.Name );
                block.Set( "tag.Link", alink.ToTag( tag.Name ) );
                block.Set( "tag.DataCount", tag.DataCount );
                block.Set( "tag.FontSize", getFontSize( tag, count ) );

                block.Set( "tag.Link", to( DataList ) + "?tag=" + tag.Name );

                block.Set( "tag.DeleteLink", to( DeleteTag, tag.Id ) );

                block.Next();
            }
        }

        private string getFontSize( Tag tag, int maxCount ) {

            int grade1 = maxCount / 3;

            String fs;
            if (tag.DataCount / 1 <= grade1) {
                fs = "1";
            }
            else if (tag.DataCount / 1 < maxCount) {
                fs = "1.5";
            }
            else {
                fs = "2";
            }

            return fs;
        }



        private void bindResults( DataPage<DataTagShip> list ) {
            IBlock block = getBlock( "list" );
            foreach (DataTagShip dt in list.Results) {

                EntityInfo ei = Entity.GetInfo( dt.TypeFullName );
                if (ei == null) continue;

                IAppData obj = ndb.findById( ei.Type, dt.DataId ) as IAppData;
                if (obj == null) continue;

                block.Set( "data.Id", obj.Id );
                block.Set( "data.Title", obj.Title );
                block.Set( "data.Link", alink.ToAppData( obj ) );
                block.Set( "data.TypeName", getTypeName( obj ) );
                block.Set( "data.DeleteLink", to( DeleteData, dt.Id ) );

                block.Next();
            }

            set( "page", list.PageBar );
        }

        private string getTypeName( IAppData obj ) {

            if (obj.GetType() == typeof( BlogPost )) return lang( "blog" );
            if (obj.GetType() == typeof( PhotoPost )) return lang( "photo" );
            if (obj.GetType() == typeof( ForumTopic )) return lang( "forumPost" );
            if (obj.GetType() == typeof( ContentPost )) return lang( "article" );

            return "";

        }



    }

}
