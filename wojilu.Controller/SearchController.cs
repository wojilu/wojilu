using System;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Photo.Domain;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Feeds.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Domain;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller {

    public class SearchController : ControllerBase {

        public IFeedService feedService { get; set; }

        public SearchController() {
            feedService = new FeedService();
        }

        public void Index() {

            String key = strUtil.SqlClean( ctx.Get( "q" ), 10 );
            String ctype = ctx.Get( "ctype" );
            set( "key", key );
            set( "ctype", strUtil.IsNullOrEmpty( ctype ) ? "blog" : ctype );

            if (strUtil.IsNullOrEmpty( key )) {
                bindNullResults();
                return;
            }

            DataPage<IEntity> list = null;
            if (ctype == "blog") {
                list = searchBlog( key );
            }
            else if (ctype == "forum") {
                list = searchForum( key );
            }
            else if (ctype == "article") {
                list = searchArticle( key );
            }
            else if (ctype == "photo") {
                list = searchPhoto( key );
            }
            else if (ctype == "microblog") {
                list = searchMicroblog( key );
            }
            else if (ctype == "share") {
                list = searchShare( key );
            }
            else if (ctype == "user") {
                list = searchUser( key );
            }
            else if (ctype == "group") {
                list = searchGroup( key );
            }
            else {
                bindNullResults();
                return;
            }

            bindResults( list.Results );
            if (list.RecordCount == 0) {
                set( "page", "" );
            }
            else {
                set( "page", list.PageBar );
            }
            set( "resultCount", list.RecordCount );
        }



        private void bindNullResults() {
            bindResults( new List<IEntity>() );
            set( "page", "" );
            set( "resultCount", 0 );
        }

        private DataPage<IEntity> searchBlog( string key ) {
            DataPage<BlogPost> list = BlogPost.findPage( "Title like '%" + key + "%'" );
            return new DataPage<IEntity>( list );
        }

        private DataPage<IEntity> searchForum( string key ) {
            DataPage<ForumTopic> list = ForumTopic.findPage( "Title like '%" + key + "%'" );
            return new DataPage<IEntity>( list );
        }

        private DataPage<IEntity> searchArticle( string key ) {

            DataPage<ContentPost> list = ContentPost.findPage( "Title like '%" + key + "%'" );

            return new DataPage<IEntity>( list );
        }

        private DataPage<IEntity> searchPhoto( string key ) {
            DataPage<PhotoPost> list = PhotoPost.findPage( "Title like '%" + key + "%'" );
            return new DataPage<IEntity>( list );
        }

        private DataPage<IEntity> searchUser( string key ) {
            DataPage<User> list = User.findPage( "Name like '%" + key + "%'" );
            return new DataPage<IEntity>( list );
        }

        private DataPage<IEntity> searchGroup( string key ) {
            DataPage<Group> list = Group.findPage( "Name like '%" + key + "%'" );
            return new DataPage<IEntity>( list );
        }

        private DataPage<IEntity> searchMicroblog( string key ) {
            DataPage<Microblog> list = Microblog.findPage( "Content like '%" + key + "%'" );
            return new DataPage<IEntity>( list );
        }

        private DataPage<IEntity> searchShare( string key ) {
            DataPage<Share> list = Share.findPage( "BodyData like '%" + key + "%'" );
            return new DataPage<IEntity>( list );
        }

        private void bindResults( List<IEntity> list ) {

            IBlock block = getBlock( "searchList" );
            foreach (IEntity obj in list) {

                if (obj is Share) {
                    bindShareSingle( block, obj );
                }
                else if (obj is User) {
                    bindUser( block, obj );
                }
                else if (obj is Group) {
                    bindGroup( block, obj );
                }
                else {
                    bindAppData( block, obj );
                }

                block.Next();
            }
        }

        private void bindUser( IBlock block, IEntity obj ) {

            User user = obj as User;

            block.Set( "s.Title", "用户："+user.Name );
            block.Set( "s.Link", Link.ToMember(user) );
            block.Set( "s.Summary", string.Format( "<img src=\"{0}\" style=\"width:50px;height:50px;\" />", user.PicSmall ) );
            block.Set( "s.LinkStyle", "display:none;" );
        }

        private void bindGroup( IBlock block, IEntity obj ) {

            Group group = obj as Group;

            block.Set( "s.Title", "群组：" + group.Name );
            block.Set( "s.Link", Link.ToMember( group ) );
            block.Set( "s.Summary", string.Format( "<img src=\"{0}\" style=\"width:120px;height:120px;\" />", group.LogoSmall ) );
            block.Set( "s.LinkStyle", "display:none;" );
        }

        private void bindAppData( IBlock block, IEntity obj ) {
            block.Set( "s.Title", getTitle( obj ) );
            block.Set( "s.Link", getLink( obj ) );
            block.Set( "s.Summary", getSummary( obj ) );
            block.Set( "s.LinkStyle", "" );
        }

        private void bindShareSingle( IBlock block, IEntity obj ) {
            Share share = obj as Share;

            String creatorInfo = getCreatorInfos( share.Creator );
            String feedTitle = feedService.GetHtmlValue( share.TitleTemplate, share.TitleData, creatorInfo );
            block.Set( "s.Title", feedTitle );

            String feedBody = feedService.GetHtmlValue( share.BodyTemplate, share.BodyData, creatorInfo );
            block.Set( "s.Summary", feedBody );

            block.Set( "s.LinkStyle", "display:none;" );
        }


        private String getCreatorInfos( User user ) {
            return string.Format( "<a href='{0}'>{1}</a>", Link.ToMember( user ), user.Name );
        }

        private string getTitle( IEntity obj ) {
            String strValue = getPropertyValue( obj, "Title" );
            if (strUtil.HasText( strValue )) return strValue;

            strValue = getPropertyValue( obj, "Content" );
            if (strUtil.HasText( strValue )) return strValue;

            return getPropertyValue( obj, "BodyGeneral" );
        }

        private string getLink( IEntity obj ) {

            if (obj is IAppData) {
                return strUtil.Join( ctx.url.SiteUrl, alink.ToAppData( obj as IAppData ) );
            }


            return to( Index ) + "#";
        }

        private string getSummary( IEntity obj ) {

            if (obj is PhotoPost) {
                return string.Format( "<img src=\"{0}\" style=\"width:120px;height:120px;\" />", obj.get( "ImgThumbUrl" ) );
            }

            String summary = getPropertyValue( obj, "Summary" );
            if (strUtil.HasText( summary )) return summary;

            return strUtil.ParseHtml( getPropertyValue( obj, "Content" ), 150 );
        }

        private string getPropertyValue( IEntity data, String propertyName ) {

            EntityInfo ei = Entity.GetInfo( data );

            if (ei.GetProperty( propertyName ) == null) return "";

            Object summary = data.get( propertyName );

            return summary == null ? "" : summary.ToString();
        }

    }

}
