using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Comments;

using wojilu.DI;
using wojilu.Web.Mvc;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin.Upgrade {

    public class ImportHelper<TComment, TTarget>
        where TComment : IEntity
        where TTarget : IEntity {

        private static readonly ILog logger = LogManager.GetLogger( "ImportHelper" );

        public void Import() {

            List<TComment> clist = db.findAll<TComment>();
            foreach (TComment x in clist) {

                // 如果已经导入，那么就删除已有的OpenComment
                OpenComment oc = hasImport( x );
                if (oc != null) {
                    deleteOpenComment( oc );
                    deleteTransLog( oc );
                }

                OpenComment comment = getOpenComment( x );

                OpenCommentService service = ObjectContext.Create<OpenCommentService>();
                service.Import( comment );

                logTransInfo( x, comment );
            }

        }

        private void deleteTransLog( OpenComment oc ) {
            OpenCommentTrans.deleteBatch( "OpenCommentId=" + oc.Id );
        }

        private OpenComment hasImport( TComment x ) {

            OpenCommentTrans trans = OpenCommentTrans.find( "CommentId=:cid and CommentType=:ctype" )
                .set( "cid", x.Id )
                .set( "ctype", typeof( TComment ).FullName )
                .first();

            if (trans == null) return null;

            return OpenComment.findById( trans.OpenCommentId );
        }

        private void deleteOpenComment( OpenComment x ) {

            OpenCommentService service = ObjectContext.Create<OpenCommentService>();

            service.Delete( x );
        }

        private void logTransInfo( TComment x, OpenComment comment ) {

            OpenCommentTrans trans = new OpenCommentTrans();
            trans.CommentId = x.Id;
            trans.CommentType = x.GetType().FullName;
            trans.OpenCommentId = comment.Id;

            trans.insert();
        }

        private OpenComment getOpenComment( TComment obj ) {

            IComment x = obj as IComment;

            OpenComment comment = new OpenComment();
            comment.AppId = x.AppId;

            comment.Title = x.Title;
            comment.Content = x.Content;

            comment.Author = x.Author;
            comment.Member = x.Member;
            comment.Ip = x.Ip;
            comment.Created = x.Created;

            comment.ParentId = getParentId( obj );

            IEntity p = ndb.findById( typeof( TTarget ), x.RootId );
            if (p == null) {
                comment.TargetDataId = 0;
                comment.TargetDataType = typeof( TTarget ).FullName;
                comment.TargetTitle = "--null-";
                comment.TargetUserId = 0;
            }
            else {
                comment.TargetDataId = p.Id;
                comment.TargetDataType = typeof( TTarget ).FullName;
                comment.TargetTitle = getTitle( p );
                comment.TargetUserId = getCreatorId( p );
            }

            return comment;
        }

        private string getTitle( IEntity p ) {

            if (p == null) return "--null--";

            Object obj = p.get( "Title" );
            if (obj == null) return "";
            return obj.ToString();
        }

        private int getCreatorId( IEntity p ) {

            if (p == null) return 0;

            User user = p.get( "Creator" ) as User;
            if (user == null) return 0;

            return user.Id;
        }

        private int getParentId( TComment obj ) {

            IComment x = obj as IComment;

            if (x.ParentId <= 0) return 0;

            int parentId = getOldParentId( x.ParentId );

            OpenCommentTrans trans = OpenCommentTrans.find( "CommentId=:cid and CommentType=:ctype" )
                .set( "cid", parentId )
                .set( "ctype", x.GetType().FullName )
                .first();

            if (trans == null) return 0;

            if (trans.OpenCommentId <= 0) return 0;

            return trans.OpenCommentId;
        }

        private int getOldParentId( int id ) {

            TComment p = db.findById<TComment>( id );
            if (p == null) return 0;

            IComment comment = p as IComment;
            if (comment == null) return 0;

            if (comment.ParentId == 0) return id;
            if (comment.ParentId == id) return id;

            return getOldParentId( comment.ParentId );
        }


    }
}
