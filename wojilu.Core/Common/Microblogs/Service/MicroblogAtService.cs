using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Microblogs.Domain;

namespace wojilu.Common.Microblogs.Service {

    public class MicroblogAtService {

        public DataPage<Microblog> GetByUser( int userId, int pageSize ) {

            DataPage<MicroblogAt> list = MicroblogAt.findPage( "UserId=" + userId, pageSize );
            return list.Convert<Microblog>( getResults( list.Results ) );
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
