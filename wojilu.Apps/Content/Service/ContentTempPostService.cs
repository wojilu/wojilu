using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using wojilu.Members.Interface;
using wojilu.Apps.Content.Enum;

namespace wojilu.Apps.Content.Service {

    public class ContentTempPostService {

        public int GetSubmitCount( int creatorId, IMember owner, int appId ) {
            String condition = string.Format( "CreatorId={0} and AppId={1} and OwnerId={2} and OwnerType='{3}' ", creatorId, appId, owner.Id, owner.GetType().FullName );
            return ContentTempPost.count( condition );
        }

        public DataPage<ContentTempPost> GetByCreator( int creatorId, IMember owner, int appId ) {
            String condition = string.Format( "CreatorId={0} and AppId={1} and OwnerId={2} and OwnerType='{3}' ", creatorId, appId, owner.Id, owner.GetType().FullName );
            return ContentTempPost.findPage( condition );
        }

        public int CountByCreator( int creatorId, IMember owner, int appId ) {
            String condition = string.Format( "CreatorId={0} and AppId={1} and OwnerId={2} and OwnerType='{3}' ", creatorId, appId, owner.Id, owner.GetType().FullName );
            return ContentTempPost.count( condition );
        }

        public int GetSubmitCount( IMember owner, int appId ) {
            String condition = string.Format( "AppId={0} and OwnerId={1} and OwnerType='{2}' and Status={3}", appId, owner.Id, owner.GetType().FullName, PostSubmitStatus.Normal );
            return ContentTempPost.count( condition );
        }

        public DataPage<ContentTempPost> GetPage( IMember owner, int appId ) {
            String condition = string.Format( "AppId={0} and OwnerId={1} and OwnerType='{2}' and Status={3}", appId, owner.Id, owner.GetType().FullName, PostSubmitStatus.Normal );
            return ContentTempPost.findPage( condition );
        }

        public ContentTempPost GetById( int id ) {
            return ContentTempPost.findById( id );
        }

        public void NoPass( string ids ) {
            ContentTempPost.updateBatch( "set Status=" + PostSubmitStatus.Deleted, "Id in (" + ids + ")" );
        }

        public void NoPass( ContentTempPost p ) {
            p.Status = PostSubmitStatus.Deleted;
            p.update( "Status" );
        }

        public Result Insert( ContentTempPost post ) {
            return post.insert();
        }

        public void Delete( ContentTempPost post ) {
            post.delete();
        }

        public ContentPost GetBySubmitPost( ContentTempPost p, IMember owner ) {

            ContentPost post = new ContentPost();

            post.Creator = p.Creator;
            post.CreatorUrl = p.Creator.Url;
            post.OwnerId = owner.Id;
            post.OwnerType = owner.GetType().FullName;
            post.OwnerUrl = owner.Url;
            post.AppId = p.AppId;

            ContentSection section = new ContentSection();
            section.Id = p.SectionId;
            post.PageSection = section;

            post.Title = p.Title;
            post.TypeName = p.TypeName;


            post.Author = p.Author;
            post.SourceLink = p.SourceLink;
            post.Content = p.Content;
            post.Summary = strUtil.CutString( p.Summary, 250 );
            post.ImgLink = p.ImgLink;

            if (typeof( ContentVideo ).FullName.Equals( post.TypeName )) {
                post.CategoryId = PostCategory.Video;
            }
            else if (strUtil.HasText( post.ImgLink )) {
                post.CategoryId = PostCategory.Img;
                post.Width = 100;
                post.Height = 85;
            }

            post.Ip = p.Ip;

            return post;
        }






    }

}
