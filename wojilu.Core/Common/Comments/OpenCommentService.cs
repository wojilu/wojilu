using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Common.Comments {


    public class OpenCommentService {



        public DataPage<OpenComment> GetByUrlDesc( String url ) {

            DataPage<OpenComment> datas = OpenComment.findPage( "TargetUrl='" + strUtil.SqlClean( url, 50 ) + "' and ParentId=0" );

            datas.Results = addSubList( datas.Results, true );

            return datas;
        }

        public DataPage<OpenComment> GetByUrlAsc( String url ) {

            DataPage<OpenComment> datas = OpenComment.findPage( "TargetUrl='" + strUtil.SqlClean( url, 50 ) + "' and ParentId=0 order by Id asc" );

            datas.Results = addSubList( datas.Results, false );

            return datas;
        }

        //----------------------------------------------------------------------------------------------


        private List<OpenComment> addSubList( List<OpenComment> list, Boolean isDesc ) {

            String subIds = "";
            foreach (OpenComment c in list) {
                if (isDesc) {
                    subIds = strUtil.Join( subIds, c.LastReplyIds, "," );
                }
                else {
                    subIds = strUtil.Join( subIds, c.FirstReplyIds, "," );
                }
            }

            subIds = subIds.Trim().TrimStart( ',' ).TrimEnd( ',' );
            if (strUtil.IsNullOrEmpty( subIds )) return list;

            List<OpenComment> totalSubList = OpenComment.find( "Id in (" + subIds + ")" ).list();
            foreach (OpenComment c in list) {
                c.SetReplyList( getSubListFromTotal( c, totalSubList ) );
            }

            return list;
        }

        private List<OpenComment> getSubListFromTotal( OpenComment parent, List<OpenComment> totalSubList ) {

            List<OpenComment> results = new List<OpenComment>();
            int iCount = 0;
            foreach (OpenComment c in totalSubList) {

                if (iCount >= OpenComment.subCacheSize) break;

                if (c.ParentId == parent.Id) {
                    results.Add( c );
                    iCount = iCount + 1;
                }
            }

            return results;
        }

        //----------------------------------------------------------------------------------------------

        public Result Create( OpenComment c ) {

            Result result = c.insert();
            if (result.IsValid) {
                updateParentReplies( c );
                return result;
            }
            else {
                return result;
            }

        }


        private static void updateParentReplies( OpenComment c ) {

            if (c.ParentId == 0) return;

            OpenComment p = OpenComment.findById( c.ParentId );
            if (p == null) {
                c.ParentId = 0;
                c.update();
                return;
            }

            //------------------------------------------------
            p.Replies = OpenComment.count( "ParentId=" + p.Id );

            //-------------------------------------------------
            List<OpenComment> subFirst = OpenComment.find( "ParentId=" + p.Id + " order by Id asc" ).list( OpenComment.subCacheSize );
            List<OpenComment> subLast = OpenComment.find( "ParentId=" + p.Id + " order by Id desc" ).list( OpenComment.subCacheSize );

            p.FirstReplyIds = strUtil.GetIds( subFirst );
            p.LastReplyIds = strUtil.GetIds( subLast );

            p.update();

        }


        public List<OpenComment> GetMore( int parentId, int startId, int replyPageSize, string sort ) {

            String condition = "";

            if (sort == "asc") {
                condition = "ParentId=" + parentId + " and Id>" + startId + " order by Id asc";
            }
            else {
                condition = "ParentId=" + parentId + " and Id<" + startId + " order by Id desc";               
            }

            return OpenComment.find( condition ).list( replyPageSize );
        }


    }
}
