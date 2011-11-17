using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Microblogs.Domain;

namespace wojilu.Common.Microblogs.Service {

    public class MicroblogAtService {

        public DataPage<Microblog> GetByUser( int userId, int pageSize ) {

            DataPage<MicroblogAt> list = MicroblogAt.findPage( "UserId=" + userId, pageSize );
            DataPage<Microblog> blogList = new DataPage<Microblog>();
            blogList.CopyStats( list );
            blogList.Results = getResults( list.Results );
            return blogList;
        }

        private List<Microblog> getResults( List<MicroblogAt> maList ) {

            List<Microblog> list = new List<Microblog>();
            foreach (MicroblogAt ma in maList) {
                list.Add( ma.Microblog );
            }

            return list;

        }

    }

}
