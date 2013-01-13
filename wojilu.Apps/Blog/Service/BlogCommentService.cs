/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;

namespace wojilu.Apps.Blog.Service {

    public class BlogCommentService : IBlogCommentService {

        public virtual List<BlogPostComment> GetByPost( int blogPostId ) {
            return db.find<BlogPostComment>( "RootId=" + blogPostId ).list();
        }

        public virtual DataPage<BlogPostComment> GetPageAll( String condition ) {
            return db.findPage<BlogPostComment>( condition );
        }


        public virtual void DeleteBatch( String ids ) {

            if (strUtil.IsNullOrEmpty( ids )) return;

            int[] arrId = cvt.ToIntArray( ids );
            foreach (int id in arrId) {

                BlogPostComment c = db.findById<BlogPostComment>( id );
                if (c == null) continue;

                // 删除评论
                db.delete( c );

                // 重新统计父帖的数量

                Type et = Entity.GetType( c.GetTargetType().FullName );
                IEntity parent = ndb.findById( et, c.RootId );
                if (parent == null) continue;

                String property = "Replies";
                int replies = (int)parent.get( property );
                parent.set( property, (replies-1) );
                db.update( parent, property );

            }


        }

    }

}
