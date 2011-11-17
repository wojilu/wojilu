/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Domain;
using wojilu.Common.Skins;
using System.Web.UI.WebControls;


namespace wojilu.Web.Controller.Common {

    public class SkinBaseController : ControllerBase {

        public virtual SkinService skinService {
            get { return null; }
        }

        public virtual ISkin objSkin {
            get { return null; }    
        }

        public override void Layout() {
        }


        [Login]
        public void My() {
            set( "lnkCustom", to( Add ) );

            IList myskins = skinService.GetMy( ctx.owner.Id );
            ctx.SetItem( "list", myskins );
            load( "mylist", List );

            IBlock lbl = getBlock( "lbl" );
            if (myskins.Count > 0) lbl.Next();

            IPageList pages = skinService.GetPage();
            ctx.SetItem( "list", pages.Results );
            load( "list", List );
            set( "page", pages.PageBar );
        }

        [NonVisit]
        public void List() {
            IList lists = ctx.GetItem( "list" ) as IList;
            IBlock block = getBlock( "list" );
            foreach (ISkin skin in lists) {
                block.Set( "t.Name", skin.Name );
                block.Set( "t.Thumb", skin.GetThumbPath() );
                block.Set( "t.PreviewUrl", getPreviewUrl( skin.Id ) );
                block.Set( "t.ActionUrl", getSkinActionUrl( skin, ctx.owner.obj.TemplateId ) );
                block.Next();
            }
        }


        [HttpPut, Login]
        public void SaveTemplate( int id ) {
            skinService.ApplySystemSkin( ctx.owner.obj, id );
            //redirect( My );
            echoRedirect( lang( "opok" ), My );
        }

        [Login]
        public void Add() {

            target( Create );
            set( "lnkList", to( My ) );

            int skinId = ctx.owner.obj.TemplateId;
            String skinContent = skinService.GetSkinContent( skinId );
            set( "skinContent", skinContent );
        }

        [HttpPost, Login, DbTransaction]
        public void Create() {

            ISkin skin = validate( null );
            if (ctx.HasErrors) {
                run( Add ); return;
            }

            skin.MemberId = ctx.owner.obj.Id;
            //skin.MemberUrl = ctx.owner.obj.Url;
            //skin.MemberName = ctx.owner.obj.Name;

            skinService.Insert( skin );
            redirect( My );
        }

        [Login]
        public void Edit( int id ) {
            target( Update, id );
            set( "lnkList", to( My ) );

            ISkin skin = skinService.GetById( id, ctx.owner.obj.Id );
            if (skin == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            set( "skin.Name", skin.Name );
            set( "skin.Body", skin.Body );
        }

        [HttpPut, Login, DbTransaction]
        public void Update( int id ) {

            // TODO 暂不支持直接修改css样式表
            //ISkin skin = skinService.GetById( id, ctx.owner.obj.Id );
            //if (skin == null) {
            //    echoRedirect( lang( "exDataNotFound" ) );
            //    return;
            //}

            //skin = validate( skin );
            //if (ctx.HasErrors()) {
            //    run( Edit, id ); return;
            //}

            //skinService.Update( skin );
            //redirect( My );
        }

        [HttpDelete, Login, DbTransaction]
        public void Delete( int id ) {
            ISkin skin = skinService.GetById( id, ctx.owner.obj.Id );
            if (skin == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            skinService.Delete( skin );
            redirect( My );
        }

        private ISkin validate( ISkin skin ) {

            if (skin == null) skin = objSkin;

            skin.Name = ctx.Post( "Name" );
            skin.Body = ctx.Post( "SkinContent" );
            if (strUtil.IsNullOrEmpty( skin.Body )) errors.Add( lang( "exContent" ) );

            // TODO 过滤script内容
            if (strUtil.HasText( skin.Name )) skin.Name = skin.Name.Replace( "<", "&lt;" ).Replace( ">", "&gt;" );
            if (strUtil.HasText( skin.Body )) skin.Body = skin.Body.Replace( "<", "&lt;" ).Replace( ">", "&gt;" );

            return skin;
        }

        private String getSkinActionUrl( ISkin skin, int templateId ) {

            if (skin.MemberId == ctx.owner.obj.Id) {
                if (skin.Id == templateId) {
                    return string.Format( "<span class=\"strong currentItem\">" + lang( "currentSkin" ) + "</span> <a href=\"{0}\" style=\"margin-left:5px;\">" + lang( "edit" ) + "</a>", to( Edit, skin.Id ) );
                }
                else {
                    String lnk = "<span href=\"{0}\" class=\"putCmd\">&raquo;" + lang( "apply" ) + "</span> <a href=\"{1}\" style=\"margin-left:5px;\">" + lang( "edit" ) + "</a> <span href=\"{2}\" class=\"deleteCmd\">" + lang( "delete" ) + "</span>";
                    return string.Format( lnk, to( SaveTemplate, skin.Id ), to( Edit, skin.Id ), to( Delete, skin.Id ) );
                }
            }

            if (skin.Id == templateId)
                return "<span class=\"strong currentItem\">" + lang( "currentSkin" ) + "</span>";
            else
                return string.Format( "<span href=\"{0}\" class=\"putCmd\">&raquo;" + lang( "apply" ) + "</span>", to( SaveTemplate, skin.Id ) );
        }

        private object getPreviewUrl( int skinId ) {
            return Link.ToMember(ctx.owner.obj) + "?skinId=" + skinId;
        }

    }

}
