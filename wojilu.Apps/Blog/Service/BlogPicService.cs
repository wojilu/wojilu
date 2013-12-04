using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Blog.Domain;
using wojilu.Common.Upload;

namespace wojilu.Apps.Blog.Service {

    public interface IBlogPicService {
        IUserFileService fileService { get; set; }
        List<BlogPicPick> GetSysNew( int count );
        void PickPic( string ids );
        void UnPickPic( string ids );
        void UnPickPicOne( UserFile x );
        void PickPicOne( UserFile x );
        bool IsPick( UserFile x );
        void Delete( string ids );
    }

    public class BlogPicService : IBlogPicService {

        public virtual IUserFileService fileService { get; set; }

        public BlogPicService() {
            fileService = new UserFileService();
        }

        public virtual List<BlogPicPick> GetSysNew( int count ) {

            return BlogPicPick.find( "" ).list( count );
        }

        public virtual void PickPic( string ids ) {

            List<UserFile> files = fileService.GetPicByIds( ids, typeof( BlogPost ) );
            foreach (UserFile x in files) {
                PickPicOne( x );
            }
        }

        public virtual void UnPickPic( string ids ) {
            List<UserFile> files = fileService.GetPicByIds( ids, typeof( BlogPost ) );
            foreach (UserFile x in files) {
                UnPickPicOne( x );
            }
        }

        public virtual void UnPickPicOne( UserFile x ) {

            BlogPicPick pick = BlogPicPick.find( "FileId=" + x.Id ).first();
            if (pick != null) {
                pick.delete();
            }

        }

        public virtual void PickPicOne( UserFile x ) {

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

        public virtual bool IsPick( UserFile x ) {
            BlogPicPick pick = BlogPicPick.find( "FileId=" + x.Id ).first();
            return pick != null;
        }

        public virtual void Delete( string ids ) {
            List<UserFile> files = fileService.GetByIds( ids, typeof( BlogPost ) );
            foreach (UserFile x in files) {
                fileService.Delete( x.Id );
            }
        }



    }

}
