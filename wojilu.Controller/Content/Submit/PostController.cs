using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Section;
using wojilu.Apps.Content.Enum;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Interface;

namespace wojilu.Web.Controller.Content.Submit {

    [App( typeof( ContentApp ) )]
    public class PostController : ControllerBase {

        public IContentSectionService sectionService { get; set; }
        public IContentPostService postService { get; set; }
        public ContentTempPostService tempPostService { get; set; }

        public PostController() {
            sectionService = new ContentSectionService();
            postService = new ContentPostService();
            tempPostService = new ContentTempPostService();
        }

        public void Index() {

            List<ContentSection> sections = sectionService.GetInputSectionsByApp( ctx.app.Id );
            List<ContentSection> allowedSections = getAllowedSections( sections );

            load( "WarningTip", WarningTip );

            IBlock block = getBlock( "list" );
            foreach (ContentSection s in allowedSections) {

                block.Set( "s.Title", s.Title );

                if (isVideo( s )) {
                    block.Set( "s.FormLink", to( SubmitVideo, s.Id ) );
                    block.Set( "s.ImgType", strUtil.Join( sys.Path.Img, "video.gif" ) );
                }
                else {
                    block.Set( "s.FormLink", to( SubmitPost, s.Id ) );
                    block.Set( "s.ImgType", strUtil.Join( sys.Path.Img, "folder.gif" ) );
                }

                block.Next();
            }
        }

        public void Show( int id ) {
            ContentTempPost p = tempPostService.GetById( id );
            if (p == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            if (hasViewPermission( p ) == false) {
                echo( "对不起，你没有查看的权限" );
                return;
            }

            bind( "p", p );
        }

        private bool hasViewPermission( ContentTempPost p ) {
            if (hasAdminPermission()) return true;
            if (ctx.viewer.obj.Id == p.Creator.Id) return true;
            return false;
        }

        //----------------------------------------------------------------------

        public void SubmitPost( int sectionId ) {

            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            set( "section.Name", s.Title );

            target( SavePost, sectionId );
        }

        public void SavePost( int sectionId ) {

            if (shouldApprove( ctx.viewer.obj ) ) {
                saveTempPost( sectionId, null );
                if (ctx.HasErrors) echoError();
                return;
            }

            ContentPost post = ContentValidator.SetValueBySection( sectionService.GetById( sectionId, ctx.app.Id ), ctx );
            ContentValidator.ValidateTitleBody( post, ctx );

            if (strUtil.HasText( post.ImgLink )) {
                post.CategoryId = PostCategory.Img;
                post.Width = 100;
                post.Height = 85;
            }

            if (ctx.HasErrors) {
                run( SubmitPost, sectionId );
            }
            else {
                postService.Insert( post, ctx.Post( "TagList" ) );

                echoRedirect( "发布成功，谢谢", to( new ContentController().Index ) );
            }
        }




        //----------------------------------------------------------------------

        public void SubmitVideo( int sectionId ) {
            target( SaveVideo, sectionId );

            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );

            set( "section.Name", s.Title );
        }

        public void SaveVideo( int sectionId ) {

            if (shouldApprove( ctx.viewer.obj ) ) {
                saveTempPost( sectionId, typeof( ContentVideo ) );
                if (ctx.HasErrors) echoError();
                return;
            }

            ContentPost post = ContentValidator.SetValueBySection( sectionService.GetById( sectionId, ctx.app.Id ), ctx );
            ContentValidator.ValidateVideo( post, ctx );

            if (ctx.HasErrors) {
                run( SubmitVideo, sectionId );
            }
            else {
                postService.Insert( post, ctx.Post( "TagList" ) );
                
                echoRedirect( "发布成功，谢谢", to( new ContentController().Index ) );
            }

        }


        //----------------------------------------------------------------------



        public void WarningTip() {
            IBlock block = getBlock( "warning" );
            IBlock tblock = getBlock( "tip" );
            if (shouldApprove( ctx.viewer.obj ) ) {
                block.Next();
            }
            else {
                tblock.Next();
            }
        }


        private List<ContentSection> getAllowedSections( List<ContentSection> sections ) {

            List<String> notAllowdList = getNotAllowed();

            List<ContentSection> list = new List<ContentSection>();

            foreach (ContentSection s in sections) {

                if (notAllowdList.Contains( s.SectionType )) continue;
                list.Add( s );

            }

            return list;
        }

        private List<String> getNotAllowed() {

            List<String> list = new List<String>();

            list.Add( typeof( TextController ).FullName );
            list.Add( typeof( PollController ).FullName );
            list.Add( typeof( TalkController ).FullName );
            list.Add( typeof( ImgController ).FullName );

            return list;
        }

        private bool isVideo( ContentSection s ) {
            if (s.SectionType.Equals( typeof( VideoController ).FullName )) return true;
            if (s.SectionType.Equals( typeof( VideoShowController ).FullName )) return true;
            return false;
        }

        private Boolean shouldApprove( IUser objUser ) {

            User user = (User)objUser;

            if (SiteRole.IsInAdminGroup( user.RoleId )) return false;

            ContentSubmitter s = ContentSubmitter.find( "User.Id=" + user.Id + " and AppId=" + ctx.app.Id ).first();
            return s == null;
        }

        private bool hasAdminPermission() {
            if (ctx.viewer.IsAdministrator()) return true;
            return false;
        }


        private void saveTempPost( int sectionId, Type postType ) {

            ContentTempPost post = new ContentTempPost();
            post.Creator = (User)ctx.viewer.obj;
            post.OwnerId = ctx.owner.Id;
            post.OwnerType = ctx.owner.obj.GetType().FullName;
            post.AppId = ctx.app.Id;
            post.SectionId = sectionId;

            post.Title = ctx.Post( "Title" );
            if (postType != null)
                post.TypeName = postType.FullName;

            post.Author = ctx.Post( "Author" );
            post.SourceLink = ctx.Post( "SourceLink" );
            post.Content = ctx.PostHtml( "Content" );
            post.Summary = ctx.Post( "Summary" );
            post.ImgLink = sys.Path.GetPhotoRelative( ctx.Post( "ImgLink" ) );
            post.TagList = ctx.Post( "TagList" );

            post.Ip = ctx.Ip;

            if (strUtil.IsNullOrEmpty( post.Title ))
                errors.Add( lang( "exTitle" ) );

            if (strUtil.IsNullOrEmpty( post.Content ) && strUtil.IsNullOrEmpty( post.SourceLink ))
                errors.Add( alang( "exContentLink" ) );

            if (ctx.HasErrors) return;


            tempPostService.Insert( post );

            echoRedirect( "投递成功", to( new MyListController().Index ) );

        }

    }

}
