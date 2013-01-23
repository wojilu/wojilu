using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.ORM;

namespace wojilu.Common.Comments {


    // OpenComment 再增加 TargetDataType/TargetId 属性
    // 在子评论中回复某人：1)@xxx人  2)增加atId 3)增加atContent

    public class CommentDto {
        public int Id { get; set; }
        public String UserName { get; set; }
        public String UserFace { get; set; }
        public String AuthorText { get; set; }
        public String Created { get; set; }
        public String Content { get; set; }
    }

    public class NullCommentTarget : ObjectBase<NullCommentTarget> {

        public String Name { get; set; }
    }    

    public class OpenComment : ObjectBase<OpenComment> {

        /// <summary>
        /// 子回复缓存的数量
        /// </summary>
        public static readonly int subCacheSize = 8;

        public String TargetUrl { get; set; }// 通知中的原始 url，供用户点击

        // 筛选加载的过滤标准
        public String TargetDataType { get; set; }
        public int TargetDataId { get; set; }

        public String TargetTitle { get; set; }
        public int TargetUserId { get; set; }

        public int AppId { get; set; }

        public int ParentId { get; set; }
        public int AtId { get; set; } // 被at的评论，不是被at的作者

        public User Member { get; set; }

        public String Author { get; set; }
        public String AuthorEmail { get; set; }

        public String Title { get; set; }

        [LongText]
        public String Content { get; set; }


        public String Ip { get; set; }


        public DateTime Created { get; set; }

        public int Replies { get; set; }
        public String FirstReplyIds { get; set; }
        public String LastReplyIds { get; set; }

        private List<OpenComment> _replyList = new List<OpenComment>();

        public List<OpenComment> GetReplyList() {
            return _replyList;
        }

        public void SetReplyList( List<OpenComment> list ) {
            if (list != null) _replyList = list;
        }

    }

    // 临时数据迁移对象
    public class OpenCommentTrans : ObjectBase<OpenCommentTrans> {

        public int CommentId { get; set; }
        public String CommentType { get; set; }

        public int OpenCommentId { get; set; }
        public DateTime Created { get; set; }


    }

}
