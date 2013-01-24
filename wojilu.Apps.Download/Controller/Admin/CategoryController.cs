/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Download.Domain;
using wojilu.Web.Controller.Common;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Download.Admin {


    [App( typeof( DownloadApp ) )]
    public class CategoryController : ControllerBase {

        public void List() {

            set( "addLink", to( Add ) );
            set( "sortAction", to( SaveSort ) );

            List<FileCategory> list = FileCategory.GetRootList();
            bindList( "list", "data", list, bindLink );
        }

        private void bindLink( IBlock block, int id ) {
            block.Set( "data.LinkEdit", to( Edit, id ) );
            block.Set( "data.LinkDelete", to( Delete, id ) );
        }

        [HttpPost]
        public virtual void SaveSort() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            FileCategory acategory = FileCategory.GetById( id );

            List<FileCategory> list = FileCategory.GetRootList();

            if (cmd == "up") {

                new SortUtil<FileCategory>( acategory, list ).MoveUp();
                echoRedirect( "ok" );
            }
            else if (cmd == "down") {

                new SortUtil<FileCategory>( acategory, list ).MoveDown();
                echoRedirect( "ok" );
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }

        }


        public void Add() {
            target( Create );
        }

        [HttpPost]
        public void Create() {
            string name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name )) {
                echoError( "请填写名称" );
                return;
            }

            FileCategory cat = new FileCategory();
            cat.Name = name;
            cat.IsThumbView = ctx.PostIsCheck( "IsThumbView" );
            cat.insert();

            echoToParentPart( lang( "opok" ) );
        }

        public void Edit( int id ) {
            target( Update, id );

            FileCategory cat = FileCategory.GetById( id );
            set( "Name", cat.Name );

            String chkstr = "";
            if (cat.IsThumbView == 1) chkstr = "checked=\"checked\"";
            set( "checked", chkstr );

        }

        [HttpPost]
        public void Update( int id ) {

            string name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name )) {
                echoError( "请填写名称" );
                return;
            }

            FileCategory cat = FileCategory.GetById( id );
            cat.Name = name;
            cat.IsThumbView = ctx.PostIsCheck( "IsThumbView" );
            cat.update();

            echoToParentPart( lang( "opok" ) );
        }

        [HttpDelete]
        public void Delete( int id ) {
            FileCategory cat = FileCategory.GetById( id );
            if (cat != null) {
                cat.delete();
                redirect( List );
            }
        }

    }

}
