using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.Upload;
using wojilu.Apps.Blog.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Blog.Service;

namespace wojilu.Web.Controller.Admin.Apps.Blog {

    [App( typeof( BlogApp ) )]
    public class BlogFileController : ControllerBase {

        private static readonly int picType = 1;
        private static readonly int fileType = 2;

        public UserFileService fileService { get; set; }
        public BlogPicService pickService { get; set; }

        public BlogFileController() {
            fileService = new UserFileService();
            pickService = new BlogPicService();
        }

        public void Index( int typeId ) {

            target( Admin );

            set( "lnkAll", to( Index, 0 ) );
            set( "lnkPic", to( Index, picType ) );
            set( "lnkFile", to( Index, fileType ) );

            DataPage<UserFile> list = getByType( typeId );
            List<BlogPost> blogPosts = getBlogPosts( list.Results );

            list.Results.ForEach( x => {
                x.data["info"] = getFileInfo( x );
                x.data.delete = to( Delete, x.Id );
                x.data.show = getDataLink( blogPosts, x );
                x.data["pickicon"] = getPickIcon( blogPosts, x );
            } );
            bindList( "list", "x", list.Results );

            set( "page", list.PageBar );
        }

        private string getPickIcon( List<BlogPost> blogPosts, UserFile x ) {
            Boolean isPick = pickService.IsPick( x );
            if (isPick) {
                return string.Format( "<img src=\"{0}star.gif\" />", sys.Path.Img );
            }
            else {
                return "";
            }
        }

        private List<BlogPost> getBlogPosts( List<UserFile> list ) {

            if (list.Count == 0) return new List<BlogPost>();

            String ids = "";
            foreach (UserFile x in list) {
                ids += x.DataId + ",";
            }

            ids = ids.TrimEnd( ',' );

            return BlogPost.find( "Id in (" + ids + ")" ).list();
        }

        private string getDataLink( List<BlogPost> blogPosts, UserFile x ) {
            foreach (BlogPost post in blogPosts) {
                if (post.Id == x.DataId) return alink.ToAppData( post );
            }
            return "";
        }

        private DataPage<UserFile> getByType( int id ) {

            if (id == picType) return fileService.GetPicByType( typeof( BlogPost ) );
            if (id == fileType) return fileService.GetFileByType( typeof( BlogPost ) );
            return fileService.GetByType( typeof( BlogPost ) );
        }

        private string getFileInfo( UserFile x ) {
            if (x.IsPic == 1) {
                return string.Format( "<a href='{0}' target='_blank'><img src='{1}' /></a>", x.PicO, x.PicS );
            }
            return string.Format( "<a href='{0}' target='_blank'>{1}</a>", x.PathFull, "下载" );
        }

        public void Delete( int id ) {

            fileService.Delete( id );

            echoRedirectPart( lang( "opok" ) );

            logAdmin( "delete", id.ToString() );

        }

        [HttpPost, DbTransaction]
        public void Admin() {

            String ids = ctx.PostIdList( "choice" );
            String cmd = ctx.Post( "action" );

            if (strUtil.IsNullOrEmpty( cmd )) {
                echoErrorCmd();
                return;
            }

            Boolean cmdValid = true;

            if (cmd.Equals( "pick" )) {
                pickService.PickPic( ids );
            }
            else if (cmd.Equals( "unpick" )) {
                pickService.UnPickPic( ids );
            }
            else if (cmd.Equals( "delete" )) {
                pickService.Delete( ids );
            }
            else {
                cmdValid = false;
            }

            // echo
            if (cmdValid == false) {
                echoErrorCmd();
            }
            else {
                logAdmin( cmd, ids );
                echoAjaxOk();
            }

        }

        private void logAdmin( string cmd, string ids ) {
            // TODO log admin
        }

        private void echoErrorCmd() {
            echoText( lang( "exUnknowCmd" ) );
        }


    }

}
