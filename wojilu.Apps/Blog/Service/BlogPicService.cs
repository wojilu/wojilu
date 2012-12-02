using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Blog.Domain;
using wojilu.Common.Upload;

namespace wojilu.Apps.Blog.Service {

    public class BlogPicService {

        public UserFileService fileService { get; set; }

        public BlogPicService() {
            fileService = new UserFileService();
        }

        public List<BlogPicPick> GetSysNew( int count ) {

            return BlogPicPick.find( "" ).list( count );
        }

        public void PickPic( string ids ) {

            List<UserFile> files = fileService.GetPicByIds( ids, typeof( BlogPost ) );
            foreach (UserFile x in files) {
                PickPicOne( x );
            }
        }

        public void UnPickPic( string ids ) {
            List<UserFile> files = fileService.GetPicByIds( ids, typeof( BlogPost ) );
            foreach (UserFile x in files) {
                UnPickPicOne( x );
            }
        }

        public void UnPickPicOne( UserFile x ) {

            BlogPicPick pick = BlogPicPick.find( "FileId=" + x.Id ).first();
            if (pick != null) {
                pick.delete();
            }

        }

        public void PickPicOne( UserFile x ) {

            BlogPicPick pick = BlogPicPick.find( "FileId=" + x.Id ).first();

            if (pick == null) {
                pick = new BlogPicPick();
            }

            pick.FileId = x.Id;
            pick.BlogPost = BlogPost.findById( x.DataId );
            pick.Title = x.Name;
            pick.Pic = x.PathRelative;

            pick.insert();
        }

        public bool IsPick( UserFile x ) {
            BlogPicPick pick = BlogPicPick.find( "FileId=" + x.Id ).first();
            return pick != null;
        }

        public void Delete( string ids ) {
            List<UserFile> files = fileService.GetByIds( ids, typeof( BlogPost ) );
            foreach (UserFile x in files) {
                fileService.Delete( x.Id );
            }
        }



    }

}
