/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Enum;
using wojilu.Common.AppBase;

namespace wojilu.Common.Comments {

    public class CommentAdminService {

        private IComment comment;
        public void setComment( IComment c ) {
            this.comment = c;
        }

        private Type thisType() { return comment.GetType(); }

        public IPageList GetPage( int appId ) {
            return ndb.findPage( thisType(), "AppId=" + appId, 40 );
        }


        public IPageList GetPageAll( String condition ) {
            return ndb.findPage( thisType(), condition );
        }

        public void DeleteBatch( String ids ) {

            if (strUtil.IsNullOrEmpty( ids )) return;

            int[] arrId = cvt.ToIntArray( ids );
            foreach (int id in arrId) {

                IEntity c = ndb.findById( thisType(), id );
                if (c == null) continue;

                // 删除评论
                db.delete( c );

                // 重新统计父帖的数量
                String typeFullName = ((IComment)c).GetTargetType().FullName;
                Type ttype = ((IComment)c).GetTargetType();
                IEntity parent = ndb.findById( ttype, ((IComment)c).RootId );
                if (parent == null) continue;

                String property = "Replies";
                int replies = (int)parent.get( property );
                parent.set( property, (replies - 1) );
                db.update( parent, property );

            }
        }

    }

}
