/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Sites.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Security;
using wojilu.Members.Interface;
using wojilu.Common.Skins;
using wojilu.Web.Utils;
using System.IO;
using wojilu.Web.Controller.Users.Admin;
using wojilu.Web.Controller.Admin.Sys;

namespace wojilu.Web.Controller.Admin {

    public class SiteSkinController : ControllerBase {

        public virtual ISiteSkinService skinService { get; set; }
        public virtual IAdminLogService<SiteLog> logService { get; set; }

        public SiteSkinController() {
            skinService = new SiteSkinService();
            logService = new SiteLogService();
        }

        public override void Layout() {
            set( "lnkSkin", to( List ) );
            set( "lnkResourceTexture", to( ResourceList, ResourceType.Texture ) );
            set( "lnkResourceIcon", to( ResourceList, ResourceType.Icon ) );
            set( "lnkResourcePic", to( ResourceList, ResourceType.Pic ) );

            set( "lnkTemplateFile", to( new ViewsFileController().Index ) );
            set( "lnkTemplateSearch", to( new ViewsFileController().Search ) );

            set( "lnkCssFile", to( new CssFileController().Index ) );
            set( "lnkSkinFile", to( new SkinFileController().Index ) );

        }

        public virtual void ResourceList(long typeId) {

            target( ResourceSave, typeId );

            set( "resourceTypeName", ResourceType.GetTypeName( (int)typeId ) );

            DataPage<Resource> list = Resource.GetPage( (int)typeId, 50 );
            bindList( "list", "r", list.Results, bindLink );
            set( "page", list.PageBar );
        }

        private void bindLink( IBlock block, long id ) {
            block.Set( "r.DeleteLink", to( ResourceDelete, id ) );
        }

        [HttpDelete, DbTransaction]
        public virtual void ResourceDelete( long id ) {

            Resource r = Resource.findById( id );
            if (r == null) {
                echoRedirect( lang( "dataNotFound" ) );
                return;
            }

            Resource.Delete( r );

            redirect( typeId => ResourceList(typeId), r.TypeId );
        }

        [HttpPost, DbTransaction]
        public virtual void ResourceSave( long typeId ) {

            HttpFile postedFile = ctx.GetFileSingle();
            Result result = Uploader.SaveImg( postedFile );
            if (result.HasErrors) {
                echoRedirect( result.ErrorsHtml );
                return;
            }

            Resource r = new Resource();
            r.TypeId = (int)typeId;
            r.Url = result.Info.ToString();

            String name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name )) name = Path.GetFileName( r.Url );
            r.Name = name;

            r.insert();

            redirect( typeId1 => ResourceList(typeId1), typeId );
        }

        //------------------------------------------------------------------------------------------------
        public virtual void CustomBg() {
            bindLayout();
        }

        private void bindLayout() {
            set( "bgLink", to( CustomBg ) );
            set( "headerLink", to( CustomHeader ) );
            set( "mainLink", to( CustomMain ) );
            set( "footerLink", to( CustomFooter ) );
            set( "navLink", to( CustomNav ) );

            set( "myPicsLink", to( MyPics ) );
            set( "picUrlLink", to( AddPicUrl ) );
            set( "uploadLink", to( UploadForm ) );
            set( "saveUrl", to( SaveBg ) );

            load( "autoSaveScript", script );
        }

        public virtual void SaveBg() {

            String ele = ctx.Post( "ele" );
            String kvItem = ctx.Post( "kv" );

            skinService.CustomBg( ctx.owner.obj, ele, kvItem );
            echoAjaxOk();
        }

        public virtual void CustomHeader() {
            bindLayout();
        }

        public virtual void CustomMain() {
            bindLayout();
        }

        public virtual void CustomFooter() {
            bindLayout();
        }

        public virtual void CustomNav() {
            bindLayout();
        }

        public virtual void script() {
        }

        public virtual void MyPics() {

            DataPage<Resource> list = Resource.GetPage( ResourceType.Texture, 8 );
            
            IBlock block = getBlock( "list" );
            foreach (Resource post in list.Results) {
                block.Set( "img.Thumb", post.ImgThumbUrl );
                block.Set( "img.OriginalUrl", post.ImgUrl );
                block.Next();
            }
            set( "page", list.PageBar );
        }


        public virtual void AddPicUrl() {
        }

        public virtual void UploadForm() {
            target( SavePic );
        }

        public virtual void SavePic() {

            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveImg( postedFile );
            if (result.HasErrors) {
                ctx.errors.Join( result );
                run( UploadForm );
                return;
            }

            String imgUrl = result.Info.ToString();

            Resource rpost = new Resource();
            rpost.Url = imgUrl;
            rpost.insert();

            set( "picThumbUrl", rpost.ImgThumbUrl );
            set( "picUrl", rpost.ImgUrl );


        }

        //--------------------------------------------------------------------------------

        public virtual void List() {

            String customLink = Link.ToMember( Site.Instance ) + "?skin=custom";
            set( "customLink", customLink );

            Boolean isCustom = skinService.IsUserCustom();
            String customInfo = isCustom ? lang( "userSkinTip" ) : "";
            set( "customInfo", customInfo );


            List<SiteSkin> lists = skinService.GetSysAll();

            IBlock block = getBlock( "list" );
            foreach (SiteSkin skin in lists) {
                block.Set( "t.Name", skin.Name );
                block.Set( "t.Thumb", skin.GetThumbPath() );
                block.Set( "t.ActionUrl", getApplyCmd( skin.Id ) );
                block.Next();
            }
        }

        private String getApplyCmd( long id ) {

            if (config.Instance.Site.SkinId == id) {
                return "<span class=\"currentSkin\">" + lang( "currentSkin" ) + "</span>";
            }

            String url = to( Apply, id );
            return "<a href=\"" + url + "\" class=\"putCmd\">&rsaquo;&rsaquo; " + lang( "apply" ) + "</a>";
        }

        [HttpPut]
        public virtual void Apply( long id ) {

            SiteSkin skin = skinService.GetById( id );
            if (skin == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            config.Instance.Site.SkinId = skin.Id;
            config.Instance.Site.Update( "SkinId", skin.Id );

            log( skin );

            echoRedirect( lang( "opok" ), List );

        }

        private void log( SiteSkin skin ) {
            String dataInfo = "{Id:" + skin.Id + ", Name:'" + skin.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, SiteLogString.ApplySkin(), dataInfo, typeof( SiteSkin ).FullName, ctx.Ip );
        }


    }

}
