using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Microblogs.Domain;

namespace wojilu.Common.Microblogs.Service {

    public class SysMicroblogCommentService {

        public DataPage<MicroblogComment> GetSysPage( int pageSize ) {
            return MicroblogComment.findPage( "", pageSize );
        }

        public void DeleteTrueBatch( string ids ) {
            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;

            MicroblogComment.deleteBatch( "id in (" + ids + ")" );
        }

        public DataPage<MicroblogComment> GetPageByCondition( string condition ) {
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
