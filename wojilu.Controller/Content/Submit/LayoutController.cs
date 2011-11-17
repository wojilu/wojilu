using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Domain;
using wojilu.Members.Interface;
using wojilu.Apps.Content.Interface;

namespace wojilu.Web.Controller.Content.Submit {

    public class LayoutController  : ControllerBase{

        public ContentTempPostService tempPostService { get; set; }
        public IContentPostService postService { get; set; }

        public LayoutController() {
            tempPostService = new ContentTempPostService();
            postService = new ContentPostService();
        }

        public override void Layout() {
            set( "submitLink", to( new PostController().Index ) );
            set( "mySubmitLink", to( new MyListController().Index ) );
            set( "rankLink", to( new MyListController().Rank ) );

            int submitCount = tempPostService.CountByCreator( ctx.viewer.Id, ctx.owner.obj, ctx.app.Id );
            int approvedCount = postService.CountByCreator( ctx.viewer.Id, ctx.owner.obj, ctx.app.Id );
            set( "submitCount", submitCount );
            set( "approvedCount", approvedCount );

            IBlock ablock = getBlock( "admin" );
            if (hasAdminPermission()) {

                ablock.Set( "postCount", tempPostService.GetSubmitCount( ctx.owner.obj, ctx.app.Id ) );
                ablock.Set( "adminLink", to( new AdminController().Index ) );
                ablock.Next();
            }
        }

        private bool hasAdminPermission() {
            if (ctx.viewer.IsAdministrator()) return true;
            return false;
        }

    }

}
