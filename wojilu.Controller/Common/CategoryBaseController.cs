/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.Categories;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Common {

    public class CategoryBaseController<T> : ControllerBase where T : CategoryBase {

        public virtual void List() {

            set( "addLink", to( Add ) );
            set( "sortAction", to( SaveSort ) );

            String condition = getCondition() + " and ParentId=0";
            List<T> list = db.find<T>( condition + " order by OrderId desc, Id asc" ).list();

            bindCategories( list );
        }

        private void bindCategories( List<T> list ) {
            IBlock block = getBlock( "list" );
            foreach (T cat in list) {
                block.Set( "category.Id", cat.Id );
                block.Set( "category.Title", cat.Name );
                block.Set( "category.OrderId", cat.OrderId );
                block.Set( "category.Description", cat.Description );
                block.Set( "category.EditUrl", to( Edit, cat.Id ) );
                block.Set( "category.DeleteUrl", to( Delete, cat.Id ) );
                block.Next();
            }
        }

        private String getCondition() {
            if (ctx.app == null)                 
                return "AppId=" + ctx.app.Id;
            else
                return "OwnerId=" + ctx.owner.Id + " and AppId=" + ctx.app.Id;
        }

        [HttpPost, DbTransaction]
        public virtual void SaveSort() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            T acategory = db.findById<T>( id );
            String condition = getCondition();
            List<T> list = db.find<T>( condition + " order by OrderId desc, Id asc" ).list();

            if (cmd == "up") {

                new SortUtil<T>( acategory, list ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {

                new SortUtil<T>( acategory, list ).MoveDown();
                echoJsonOk();
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }

        }

        public virtual void Add() {
            target( Create );
        }

        [HttpPost, DbTransaction]
        public virtual void Create() {

            int appId = ctx.app == null ? 0 : ctx.app.Id;

            T category = Entity.New( typeof( T ).FullName ) as T;
            category.Name = ctx.Post( "Name" );
            category.Description = ctx.Post( "Description" );
            category.OwnerId = ctx.owner.obj.Id;
            category.OwnerUrl = ctx.owner.obj.Url;
            category.AppId = appId;

            if (strUtil.IsNullOrEmpty( category.Name )) errors.Add( lang( "exName" ) );
            if (ctx.HasErrors) {
                echoError();
                return;
            }

            Result result = db.insert( category );

            echoToParentPart( lang( "opok" ) );

        }

        public virtual void Edit( int id ) {

            target( Update, id );

            T acategory = db.findById<T>( id );
            set( "category.Title", acategory.Name );
            set( "category.Description", acategory.Description );
        }


        [HttpPost, DbTransaction]
        public virtual void Update( int id ) {

            T acategory = db.findById<T>( id );
            acategory.Name = ctx.Post( "Name" );
            acategory.Description = ctx.Post( "Description" );
            Result result = db.update( acategory );
            if (result.HasErrors) {
                echoError( result );
            }
            else
                echoToParentPart( lang( "opok" ) );
        }

        [HttpDelete, DbTransaction]
        public virtual void Delete( int id ) {
            T acategory = db.findById<T>( id );
            if (acategory != null) {
                db.delete( acategory );
                redirect( List );
            }
            else
                echoRedirect( lang( "exDataNotFound" ), List );
        }

    }

}
