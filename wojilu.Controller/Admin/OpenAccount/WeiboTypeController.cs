using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.weibo.Domain;
using wojilu.weibo.Core;

namespace wojilu.Web.Controller.Admin.OpenAccount {

    public class WeiboTypeController : ControllerBase {

        public void List() {
            List<WeiboType> types = WeiboType.GetAll();
            bindListShow( types );
            set( "addLink", to( Add ) );
        }

        private void bindListShow( List<WeiboType> types ) {
            IBlock block = getBlock( "list" );
            foreach (WeiboType a in types) {
                string callbackUrl = string.Empty;
                if (a.Name.ToLower() == "sina") {
                    callbackUrl = ctx.url.SiteUrl.TrimEnd( '/' ) + to( new ConnectController().SinaWeiboCallback );
                }
                block.Set( "w.Name", a.Name );
                block.Set( "w.FriendName", a.FriendName );
                block.Set( "w.Id", a.Id );
                block.Set( "w.Enable", a.Enable == 1 ? "checked='checked'" : string.Empty );
                block.Set( "w.AppKey", a.AppKey );
                block.Set( "w.AppSecret", a.AppSecret );
                block.Set( "w.Logo", a.Logo );
                block.Set( "w.CallbackUrl", callbackUrl );
                block.Set( "w.OrderId", a.OrderId );
                block.Set( "w.EditLink", to( Edit, a.Id ) );
                block.Set( "w.AddLink", to( Add ) );
                block.Set( "w.DeleteLink", to( Delete, a.Id ) );
                block.Next();
            }
            set( "bindUserLink", to( new UserWeiboSettingController().Index ) );
        }

        public void Add() {
            target( Create );
            dropList( "weiboType.Name", SupportWeiboType.GetSupports().ToArray(), "请选择" );
        }

        public void Create() {
            WeiboType w = ctx.PostValue<WeiboType>();
            w.Enable = Convert.ToInt32( ctx.Post( "weiboType.Enable" ) );
            w.insert();
            echoToParent( "添加成功" );
        }

        public void Edit( int id ) {
            target( Update, id );
            WeiboType a = WeiboType.GetById( id );
            bind( a );
            set( "weiboType.EnableCheckbox", Html.CheckBox( "weiboType.Enable", "", a.Enable.ToString(), a.Enable == 1 ) );
            dropList( "weiboTypeName", SupportWeiboType.GetSupports().ToArray(), a.Name );
        }
        public void Update( int id ) {
            WeiboType a = WeiboType.GetById( id );
            a = ctx.PostValue( a ) as WeiboType;
            a.Enable = Convert.ToInt32( ctx.Post( "weiboType.Enable" ) );
            Result result = a.update();

            if (result.HasErrors) {
                errors.Join( result );
                run( Add );
            }
            else
                echoToParent( "修改成功" );
        }

        [HttpDelete]
        public void Delete( int id ) {
            WeiboType.Delete( id );
            echoRedirect( "删除成功", List );
        }
    }
}
