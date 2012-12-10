using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Microblogs.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Microblogs.Service {

    public class MicroblogFavoriteService {


        public DataPage<MicroblogFavorite> GetFavoritePage( int ownerId, int pageSize ) {
            return db.findPage<MicroblogFavorite>( "UserId=" + ownerId, pageSize );
        }

        public DataPage<Microblog> GetBlogPage( int ownerId, int pageSize ) {
            DataPage<MicroblogFavorite> list = GetFavoritePage( ownerId, pageSize );

            List<Microblog> mlist = new List<Microblog>();
            foreach (MicroblogFavorite mf in list.Results) {
                mlist.Add( mf.Microblog );
            }

            return list.Convert<Microblog>( mlist );
        }

        public void SaveFavorite( int userId, Microblog blog ) {

            MicroblogFavorite f = MicroblogFavorite.find( "UserId=" + userId + " and MicroblogId=" + blog.Id ).first();
            if (f != null) return;


            MicroblogFavorite mf = new MicroblogFavorite();
            mf.UserId = userId;
            mf.Microblog = blog;
            mf.insert();
        }

        public void CancelFavorite( int userId, Microblog blog ) {
            MicroblogFavorite f = MicroblogFavorite.find( "UserId=" + userId + " and MicroblogId=" + blog.Id ).first();
            if (f == null) return;

            f.delete();

        }


        public List<MicroblogVo> CheckFavorite( List<Microblog> list, int viewId ) {


            List<MicroblogVo> mvList = new List<MicroblogVo>();
            if (list.Count == 0) return mvList;

            String ids = getBlogIds( list );
            if (strUtil.IsNullOrEmpty( ids )) return mvList;


            List<MicroblogFavorite> mfs = MicroblogFavorite.find( "UserId=" + viewId + " and MicroblogId in (" + ids + ")" ).list();
            foreach (Microblog blog in list) {
                if (blog == null) continue;
                MicroblogVo mv = new MicroblogVo();
                mv.Microblog = blog;
                if (hasFavorite( blog, mfs )) mv.IsFavorite = true;
                mvList.Add( mv );
            }

            return mvList;
        }

        private bool hasFavorite( Microblog blog, List<MicroblogFavorite> mfs ) {
            foreach (MicroblogFavorite mf in mfs) {
                if (mf.Microblog.Id == blog.Id) return true;
            }
            return false;
        }

        private string getBlogIds( List<Microblog> list ) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++) {
                if (list[i] == null) continue;
                sb.Append( list[i].Id );
                if (i < list.Count - 1) sb.Append( "," );
            }
            return sb.ToString();
        }


        public bool IsFavorite( User user, int blogId ) {
            return MicroblogFavorite.find( "UserId=" + user.Id + " and MicroblogId=" + blogId ).first() != null;
        }




    }

}
