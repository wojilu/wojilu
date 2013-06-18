/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.Pages.Service;
using wojilu.Common.Pages.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Common.Pages.Interface;
using wojilu.Common.AppBase;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Common.Msg.Service;

namespace wojilu.Web.Controller.Admin.Sys {

    public class PageCategoryController : ControllerBase {

        public IPageService pageService { get; set; }
        public IUserService userService { get; set; }

        public PageCategoryController() {
            pageService = new PageService();
            userService = new UserService();
            this.LayoutControllerType = typeof( PageController );
        }

        public void List() {
            target( Add );
            set( "sortAction", to( SaveSort ) );
            List<PageCategory> list = pageService.GetCategories( ctx.owner.obj );
            bindList( "list", "d", list, bindLink );
        }

        private void bindLink( IBlock tpl, int id ) {
            tpl.Set( "d.PageLink", to( new PageController().List, id ) );
            tpl.Set( "d.LinkEdit", to( Edit, id ) );
            tpl.Set( "d.LinkDelete", to( Delete, id ) );
        }

        [HttpPost]
        public virtual void SaveSort() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            PageCategory data = pageService.GetCategoryById( id, ctx.owner.obj );
            List<PageCategory> list = pageService.GetCategories( ctx.owner.obj );

            if (cmd == "up") {
                new SortUtil<PageCategory>( data, list ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {
                new SortUtil<PageCategory>( data, list ).MoveDown();
                echoJsonOk();
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }
        }

        public void Add() {
            target( Create );

            Dictionary<String, String> dic = getOptions();
            radioList( "OpenStatus", dic, "0" );

        }

        private static Dictionary<String, String> getOptions() {
            Dictionary<String, String> dic = new Dictionary<String, String>();
            dic.Add( "关闭(只允许管理员编辑)", OpenStatus.Close.ToString() );
            dic.Add( "开放(任何登录用户都可以编辑)", OpenStatus.Open.ToString() );
            dic.Add( "仅允许受邀用户编辑", OpenStatus.Editor.ToString() );

            return dic;
        }

        [HttpPost, DbTransaction]
        public void Create() {
            PageCategory data = validate( new PageCategory() );
            if (ctx.HasErrors) { run( Add ); return; }

            data.OwnerId = ctx.owner.Id;
            data.OwnerType = ctx.owner.obj.GetType().FullName;
            data.OwnerUrl = ctx.owner.obj.Url;
            data.Creator = (User)ctx.viewer.obj;

            addNotification( data.EditorIds, data.Name );

            db.insert( data );

            echoRedirect( lang( "opok" ), List );
        }



        public void Edit( int id ) {
            target( Update, id );
            PageCategory data = pageService.GetCategoryById( id, ctx.owner.obj );
            if (data == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            bind( "data", data );

            Dictionary<String, String> dic = getOptions();
            radioList( "OpenStatus", dic, data.OpenStatus.ToString() );

            String displayClass = data.OpenStatus == OpenStatus.Editor ? "" : "hide";
            set( "displayClass", displayClass );

            String userList = getUserList( data );
            set( "userList", userList );

            String chkstr = data.IsShowWiki == 1 ? "checked" : "";
            set( "checked", chkstr );
        }

        private string getUserList( PageCategory data ) {

            if (strUtil.IsNullOrEmpty( data.EditorIds )) return "";

            List<User> users = userService.GetByIds( data.EditorIds );
            if (users.Count == 0) return "";

            String ulist = "";
            int[] arrIds = cvt.ToIntArray( data.EditorIds );
            for (int i = 0; i < arrIds.Length; i++) {

                String userName = getUserName( arrIds[i], users );
                if (strUtil.IsNullOrEmpty( userName )) continue;

                ulist += userName;
                if (i < arrIds.Length - 1) ulist += ", ";

            }

            return ulist.Trim().TrimEnd( ',' );
        }


        private string getUserName( int id, List<User> users ) {
            foreach (User u in users) {
                if (u.Id == id) return u.Name;
            }
            return null;
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {

            PageCategory data = pageService.GetCategoryById( id, ctx.owner.obj );
            if (data == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }
            String userIds = data.EditorIds;

            data = validate( data );
            if (ctx.HasErrors) { run( Edit, id ); return; }

            db.update( data );

            // 给新增加的编辑发送通知
            String newEditorIds = getNewEditorIds( userIds, data.EditorIds );
            addNotification( newEditorIds, data.Name );

            echoRedirect( lang( "opok" ), List );
        }

        private void addNotification( string userIds, String categoryName ) {
            MessageService msgService = new MessageService();
            int[] arrIds = cvt.ToIntArray( userIds );
            foreach (int id in arrIds) {
                User user = userService.GetById( id );
                String body = user.Name + "：<br/><br/>您好！您已被邀请成为网站 “" + categoryName + "” 栏目的特约 wiki 编辑。现在您可以——<br/>1）添加新页面<br/>2）修改现有页面。<br/><br/>欢迎您的参与。<br/><br/>如有其他意见，请给网站管理员发送站内短信。";
                msgService.SiteSend( "您已成为网站 wiki 特约编辑", body, user );
            }
        }

        private string getNewEditorIds( string userIds, string newIds ) {
            int[] arrIds = cvt.ToIntArray( userIds );
            int[] arrNew = cvt.ToIntArray( newIds );

            String ids = "";
            foreach (int nid in arrNew) {
                if (contains( arrIds, nid )) continue;
                ids += nid + ",";
            }
            return ids.Trim().TrimEnd( ',' );
        }

        private bool contains( int[] arrIds, int nid ) {
            foreach (int id in arrIds) {
                if (id == nid) return true;
            }
            return false;
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {

            PageCategory data = pageService.GetCategoryById( id, ctx.owner.obj );
            if (data == null) { echoRedirect( lang( "exDataNotFound" ) ); return; }

            db.delete( data );
            redirect( List );
        }

        private PageCategory validate( PageCategory data ) {

            int orderid = ctx.PostInt( "OrderId" );
            int parentid = ctx.PostInt( "ParentId" );
            String name = ctx.Post( "Name" );
            String description = ctx.Post( "Description" );
            String logo = ctx.Post( "Logo" );

            if (strUtil.IsNullOrEmpty( name )) errors.Add( lang( "exName" ) );

            data.OrderId = orderid;
            data.ParentId = parentid;
            data.Name = name;
            data.Description = description;
            data.Logo = logo;

            data.OpenStatus = ctx.PostInt( "OpenStatus" );
            data.EditorIds = getEditorIds( data );
            data.IsShowWiki = ctx.PostIsCheck( "IsShowWiki" );

            return data;
        }

        private string getEditorIds( PageCategory data ) {

            if (data.OpenStatus != OpenStatus.Editor) return "";

            String userList = ctx.Post( "userList" );
            if (strUtil.IsNullOrEmpty( userList )) return "";

            String[] arrUser = userList.Split( new char[] { ',', '，' } );
            String ids = "";
            for (int i = 0; i < arrUser.Length; i++) {

                User u = userService.IsExist( arrUser[i].Trim() );
                if (u == null) continue;

                ids += u.Id + ", ";

            }
            return ids.Trim().TrimEnd( ',' );
        }


    }
}
