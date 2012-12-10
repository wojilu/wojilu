using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Enum;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Content.Submit {

    [App( typeof( ContentApp ) )]
    public class MyListController : ControllerBase {

        public ContentTempPostService tpostService { get; set; }
        public IContentPostService postService { get; set; }

        public MyListController() {
            tpostService = new ContentTempPostService();
            postService = new ContentPostService();
        }

        public override void Layout() {
            set( "currentPostLink", to( Index ) );
            set( "approvedPostLink", to( Approved ) );
        }

        public void Rank() {

            HideLayout( typeof( MyListController ) );

            int submitCount = postService.CountByCreator( ctx.viewer.Id, ctx.owner.obj, ctx.app.Id );

            set( "submitCount", submitCount );
            set( "roleName", getRoleName( submitCount ) );

            set( "approvedLink", to( Approved ) );

        }

        private String getRoleName( int submitCount ) {

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSubmitterRole sr = app.GetSubmitterRoleObj();

            if (SiteRole.IsInAdminGroup( ctx.viewer.obj.RoleId )) return sr.Editor;

            ContentSubmitter s = ContentSubmitter.find( "User.Id=" + ctx.viewer.Id + " and AppId=" + ctx.app.Id ).first();
            if (s != null) return sr.getName( s.RoleId );

            if (submitCount > 0) return sr.NeedApproval;

            return "无";
        }

        public void Index() {
            DataPage<ContentTempPost> list = tpostService.GetByCreator( ctx.viewer.Id, ctx.owner.obj, ctx.app.Id );
            IBlock block = getBlock( "list" );
            foreach (ContentTempPost p in list.Results) {
                block.Bind( "post", p );

                String statusStr = p.Status == PostSubmitStatus.Normal ? "<span class=\"waiting\">等待审核</span>" : "<span class=\"unpass\">未通过</span>";
                block.Set( "post.StatusStr", statusStr );
                block.Set( "post.DeleteLink", to( Delete, p.Id ) );
                block.Set( "post.ShowLink", to( new PostController().Show, p.Id ) );
                block.Next();
            }
            set( "page", list.PageBar );
        }

        public void Approved() {

            DataPage<ContentPost> list = postService.GetPageByCreator( ctx.viewer.Id, ctx.owner.obj, ctx.app.Id );

            IBlock block = getBlock( "list" );
            foreach (ContentPost p in list.Results) {

                String title = strUtil.HasText( p.Title ) ? p.Title : "无标题";
                block.Set( "post.Title", title );
                block.Bind( "post", p );


                block.Set( "post.ShowLink", alink.ToAppData( p ) );
                block.Set( "post.DeleteLink", to( DeleteApproved, p.Id ) );
                block.Next();
            }
            set( "page", list.PageBar );

        }

        public void DeleteApproved( int id ) {
            ContentPost p = postService.GetById( id, ctx.owner.Id);
            if (p == null) {
                echoError( lang( "exDataNotFound" ) );
                return;
            }
            postService.Delete( p );

            redirect();
        }

        public void Delete( int id ) {

            ContentTempPost p = tpostService.GetById( id );
            if (p == null) {
                echoError( lang( "exDataNotFound" ) );
                return;
            }

            tpostService.Delete( p );

            redirect();

        }

    }

}
