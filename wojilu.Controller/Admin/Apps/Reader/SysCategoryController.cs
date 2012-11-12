/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.AppBase.Interface;
using wojilu.Apps.Reader.Domain;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Admin.Apps.Reader {

    [App( typeof( ReaderApp ) )]
    public class SysCategoryController : CategoryBaseController<FeedSysCategory> {

        //public IPhotoSysCategoryService categoryService { get; set; }
        //public IAdminLogService<SiteLog> logService { get; set; }

        //public SysCategoryController() {
        //    categoryService = new PhotoSysCategoryService();
        //    logService = new SiteLogService();
        //}

        //private void log( String msg, PhotoSysCategory c ) {
        //    String dataInfo = "{Id:" + c.Id + ", Name:'" + c.Name + "'}";
        //    logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( PhotoSysCategory ).FullName, ctx.ip );
        //}

        //public void List() {
        //    target( Add );
        //    List<PhotoSysCategory> categories = categoryService.GetAll();
        //    bindList( "list", "category", categories, bindLink );
        //    set( "sortAction", to( SaveSort ) );
        //}

        //private void bindLink( IBlock tpl, int id ) {
        //    tpl.Set( "category.LinkEdit", to( Edit, id ) );
        //    tpl.Set( "category.LinkDelete", to( Delete, id ) );
        //}



        //[HttpPost, DbTransaction]
        //public virtual void SaveSort() {

        //    int id = ctx.PostInt( "id" );
        //    String cmd = ctx.Post( "cmd" );

        //    PhotoSysCategory target = categoryService.GetById( id );
        //    List<PhotoSysCategory> list = categoryService.GetAll();

        //    if (cmd == "up") {

        //        new SortUtil<PhotoSysCategory>( target, list ).MoveUp();
        //        echoJsonOk();
        //    }
        //    else if (cmd == "down") {

        //        new SortUtil<PhotoSysCategory>( target, list ).MoveDown();
        //        echoJsonOk();
        //    }
        //    else {
        //        errors.Add( lang( "exUnknowCmd" ) );
        //        echoAjaxError();
        //    }

        //}

        //public void Add() {
        //    target( Create );
        //}

        //[HttpPost, DbTransaction]
        //public void Create() {

        //    PhotoSysCategory c = validate( null );
        //    if (ctx.HasErrors()) {
        //        run( Add );
        //        return;
        //    }

        //    db.insert( c );
        //    log( SiteLogString.InsertPhotoSysCategory(), c );

        //    echoToParent( lang("opok") );
        //}

        //public void Edit( int id ) {

        //    target( Update, id );

        //    PhotoSysCategory c = categoryService.GetById( id );
        //    if (c == null) {
        //        echoRedirect( lang( "exDataNotFound" ) );
        //        return;
        //    }

        //    bind( "category", c );
        //}

        //[HttpPost, DbTransaction]
        //public void Update( int id ) {

        //    PhotoSysCategory c = categoryService.GetById( id );
        //    if (c == null) {
        //        echoRedirect( lang( "exDataNotFound" ) );
        //        return;
        //    }

        //    c = validate( c );
        //    if (ctx.HasErrors()) {
        //        run( Edit, id );
        //        return;
        //    }

        //    db.update( c );
        //    log( SiteLogString.UpdatePhotoSysCategory(), c );

        //    echoToParent( lang( "opok" ) );
        //}

        //[HttpDelete, DbTransaction]
        //public void Delete( int id ) {

        //    PhotoSysCategory c = categoryService.GetById( id );
        //    if (c == null) {
        //        echoRedirect( lang( "exDataNotFound" ) );
        //        return;
        //    }
        //    db.delete( c );
        //    log( SiteLogString.DeletePhotoSysCategory(), c );

        //    redirect( List );
        //}

        //private PhotoSysCategory validate( PhotoSysCategory c ) {
        //    if (c == null) c = new PhotoSysCategory();
        //    c = ctx.PostValue( c ) as PhotoSysCategory;
        //    if (strUtil.IsNullOrEmpty( c.Name )) errors.Add( lang( "exName" ) );
        //    return c;
        //}

    }
}
