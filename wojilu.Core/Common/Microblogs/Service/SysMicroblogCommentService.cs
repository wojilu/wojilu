using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Microblogs.Domain;

namespace wojilu.Common.Microblogs.Service {

    public interface ISysMicroblogCommentService {
        DataPage<MicroblogComment> GetSysPage( int pageSize );
        void DeleteTrueBatch( string ids );
        DataPage<MicroblogComment> GetPageByCondition( string condition );
    }

    public class SysMicroblogCommentService : ISysMicroblogCommentService {

        public virtual DataPage<MicroblogComment> GetSysPage( int pageSize ) {
            return MicroblogComment.findPage( "", pageSize );
        }

        public virtual void DeleteTrueBatch( string ids ) {
            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;

            MicroblogComment.deleteBatch( "id in (" + ids + ")" );
        }

        public virtual DataPage<MicroblogComment> GetPageByCondition( string condition ) {
            DataPage<MicroblogComment> list;
            if (strUtil.HasText( condition )) {
                list = db.findPage<MicroblogComment>( condition );
            }
            else {
                list = DataPage<MicroblogComment>.GetEmpty();
            }
            return list;
        }
    }
}
