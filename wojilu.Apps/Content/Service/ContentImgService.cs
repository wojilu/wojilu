/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Drawing;
using wojilu.Common.AppBase;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;

namespace wojilu.Apps.Content.Service {

    public class ContentImgService : IContentImgService {

        private static readonly int detailPageSize = 10;

        public virtual List<ContentPost> GetByCategory( int sectionId, int categoryId, int appId ) {
            return GetByCategory( sectionId, categoryId, appId, 2 );
        }

        public virtual List<ContentPost> GetByCategory( int sectionId, int categoryId, int appId, int count ) {
            List<ContentPost> list = ContentPost.find( "AppId=" + appId + " and PageSection.Id=" + sectionId + " and CategoryId=" + categoryId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).list( count );
            list.Sort();
            return list;
        }

        public virtual ContentPost GetTopImg( int sectionId, int categoryId, int appId ) {
            return ContentPost.find( "AppId=" + appId + " and PageSection.Id=" + sectionId + " and CategoryId=" + categoryId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).first();
        }

        public virtual ContentImg GetImgById( int imgId ) {
            return db.findById<ContentImg>( imgId );
        }

        public virtual List<ContentImg> GetImgList( int postId ) {
            return db.find<ContentImg>( "Post.Id=" + postId + " order by Id" ).list();
        }

        public virtual int GetImgCount( int postId ) {
            return db.count<ContentImg>( "PostId=" + postId );
        }

        public virtual DataPage<ContentImg> GetImgPage( int postId ) {
            return db.findPage<ContentImg>( "Post.Id=" + postId + " order by Id", detailPageSize );
        }

        public virtual DataPage<ContentImg> GetImgPage( int postId, int currentPage ) {
            return db.findPage<ContentImg>( "Post.Id=" + postId + " order by Id", currentPage, detailPageSize );
        }

        private void setNextImgLogo( ContentImg articleImg ) {
            ContentImg img = db.find<ContentImg>( "Id>" + articleImg.Id + " order by Id" ).first();
            if (img != null) {
                articleImg.Post.ImgLink = img.ImgUrl;
                this.UpdateImgLogo( articleImg.Post );
            }
            else {
                articleImg.Post.ImgLink = "";
                this.UpdateImgLogo( articleImg.Post );
            }
        }

        private void setPreImgLogo( ContentImg articleImg ) {
            ContentImg img = db.find<ContentImg>( "Id<" + articleImg.Id + " and Post.Id=" + articleImg.Post.Id + " order by Id desc" ).first();
            if (img != null) {
                articleImg.Post.ImgLink = img.ImgUrl;
                this.UpdateImgLogo( articleImg.Post );
            }
            else {
                //this.setNextImgLogo( articleImg );
                articleImg.Post.ImgLink = "";
                this.UpdateImgLogo( articleImg.Post );
            }
        }


        public virtual void UpdateImgLogo( ContentPost post ) {
            db.update( post, "ImgLink" );
        }


        public virtual void CreateImg( ContentImg img ) {
            db.insert( img );
        }

        public virtual void DeleteImgOne( ContentImg articleImg ) {
            db.delete( articleImg );

            if (articleImg.IsWebPic() == false) {
                Img.DeleteImgAndThumb( strUtil.Join( sys.Path.DiskPhoto, articleImg.ImgUrl ) );
            }

            if (articleImg.ImgUrl.Equals( articleImg.Post.ImgLink )) {
                this.setPreImgLogo( articleImg );
            }
        }

    }
}
