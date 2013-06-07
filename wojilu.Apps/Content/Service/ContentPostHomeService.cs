using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Enum;

namespace wojilu.Apps.Content.Service {

    public class ContentPostHomeService {


        public List<ContentPost> GetPicked( String ids, int count, int appId ) {

            if (strUtil.IsNullOrEmpty( ids ) || ids == "0") {
                ids = appId.ToString();
            }

            String sids = checkIds( ids );
            if (strUtil.IsNullOrEmpty( sids )) {
                return new List<ContentPost>();
            }

            if (count <= 0) count = 10;

            List<ContentPost> list = ContentPost.find( "AppId in (" + sids + ")  and PickStatus<" + PickStatus.Focus + " order by PickStatus desc, Id desc" ).list( count );
            list.Sort();

            ContentPost post = ContentPost.find( "AppId in (" + sids + ")  and PickStatus=" + PickStatus.Focus ).first();

            List<ContentPost> results = new List<ContentPost>();

            addFocusFirst( list, post, results );

            foreach (ContentPost a in list) results.Add( a );

            return results;
        }


        private String checkIds( String ids ) {

            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return null;

            String sids = "";
            for (int i = 0; i < arrIds.Length; i++) {
                if (arrIds[i] == 0) continue;
                sids += arrIds[i];
                if (i < arrIds.Length - 1) sids += ",";
            }

            return sids;
        }

        // 在第一条存入头条
        private static void addFocusFirst( List<ContentPost> list, ContentPost post, List<ContentPost> mylist ) {

            if (post != null) {
                mylist.Add( post );
                return;
            }


            if (list.Count > 0) {
                mylist.Add( list[0] );
            }

        }


    }

}
