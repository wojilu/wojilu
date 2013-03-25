/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Common.Tags;
using wojilu.Common.AppBase.Interface;
using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller {

    public class TagController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( TagController ) );

        public override void Layout() {
            // 当前app/module所有页面，所属的首页
            ctx.SetItem( "_moduleUrl", to( Index ) );

            set( "tagAdminLink", to( new wojilu.Web.Controller.Admin.TagAdminController().Index ) );
        }

        public void Index() {

            ctx.Page.Title = "tag";

            Tag maxTag = Tag.find( "order by DataCount desc" ).first();
            int count = maxTag == null ? 0 : maxTag.DataCount;
            set( "maxCount", count );

            DataPage<Tag> list = Tag.findPage( "order by DataCount desc, Id desc", 200 );
            list.Results.Sort( compareTag );

            bindTagList( count, list.Results, "tags" );
            set( "page", list.PageBar );
        }

        public void Show( String tagName ) {

            bindMyTags();

            set( "tagName", tagName );

            set( "allLink", to( Index ) );

            ctx.Page.SetTitle( tagName, "tag" );
            ctx.Page.Keywords = tagName;

            Tag tag = Tag.find( "Name=:name" ).set( "name", tagName ).first();

            DataPage<DataTagShip> list;
            if (tag == null) {
                list = DataPage<DataTagShip>.GetEmpty();
            }
            else {
                list = DataTagShip.findPage( "TagId=" + tag.Id );
            }

            bindResults( list );
        }

        private void bindMyTags() {

            Tag maxTag = Tag.find( "order by DataCount desc" ).first();
            int count = maxTag == null ? 0 : maxTag.DataCount;


            // 常用tag一览
            List<Tag> tags = Tag.find( "order by DataCount desc, Id desc" ).list( 20 ); // 最常用的tag
            tags.Sort( compareTag ); // 再按先后顺序排序
            bindTagList( count, tags, "maxTag" );

            // 最新tag一览
            List<Tag> recentTags = Tag.find( "order by Id desc" ).list( 20 ); // 最新tag
            bindTagList( count, recentTags, "recentTag" );

            // 加上更多链接
        }

        private void bindTagList( int count, List<Tag> tags, String blockName ) {
            IBlock block = getBlock( blockName );
            foreach (Tag tag in tags) {

                block.Set( "tag.Name", tag.Name );
                block.Set( "tag.Link", alink.ToTag( tag.Name ) );
                block.Set( "tag.DataCount", tag.DataCount );

                block.Set( "tag.FontSize", getFontSize( tag, count ) );

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

        private int compareTag( Tag t1, Tag t2 ) {
            if (t1.Id > t2.Id) return -1;
            if (t1.Id == t2.Id) return 0;
            return 1;
        }

        private void bindResults( DataPage<DataTagShip> list ) {

            IBlock block = getBlock( "list" );
            foreach (DataTagShip dt in list.Results) {

                EntityInfo ei = Entity.GetInfo( dt.TypeFullName );
                if (ei == null) continue;

                IAppData obj = ndb.findById( ei.Type, dt.DataId ) as IAppData;
                if (obj == null) continue;

                block.Set( "data.Title", obj.Title );
                block.Set( "data.Link", alink.ToAppData( obj ) );
                block.Set( "data.Created", obj.Created );

                String author = obj.Creator != null && obj.Creator.Id > 0 ? string.Format( "作者：<a href=\"{0}\">{1}</a>", Link.ToMember( obj.Creator ), obj.Creator.Name ) : "";
                block.Set( "data.Author", author );

                String typeName = getTypeName( obj );
                if (strUtil.HasText( typeName )) typeName = "类型：" + typeName;

                block.Set( "data.TypeName", typeName );

                block.Set( "data.Summary", getDataSummary( obj, ei ) );


                block.Next();
            }

            set( "page", list.PageBar );
        }

        private string getDataSummary( IAppData obj, EntityInfo ei ) {

            IEntity data = obj as IEntity;
            if (data == null) return "";

            String summary = getPropertyValue( data, ei, "Summary" );
            if (strUtil.HasText( summary )) return summary;

            return strUtil.ParseHtml( getPropertyValue( data, ei, "Content" ), 150 );
        }


        private string getPropertyValue( IEntity data, EntityInfo ei, String propertyName ) {

            if (ei.GetProperty( propertyName ) == null) return "";

            Object summary = data.get( propertyName );

            return summary == null ? "" : summary.ToString();
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
