using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin {

    public class AdController : ControllerBase {

        public void Index() {
            List( 0 );
        }

        public void List( int id ) {
            view( "Index" );

            set( "item.IndexUrl", to( Index ) );

            IBlock block = getBlock( "adcategories" );
            IBlock listBlock = getBlock( "listcat" );

            List<AdCategory> list = cdb.findAll<AdCategory>();
            foreach (AdCategory ac in list) {
                block.Set( "item.Name", ac.Name );
                block.Set( "item.AddUrl", to( Add, ac.Id ) );
                block.Next();
            }

            foreach (AdCategory ac in list) {
                listBlock.Set( "item.Name", ac.Name );
                listBlock.Set( "item.ListUrl", to( List, ac.Id ) );
                listBlock.Next();
            }

            IBlock adsBlock = getBlock( "list" );
            String condition = id > 0 ? "CategoryId=" + id : "";
            DataPage<AdItem> items = AdItem.findPage( condition );


            foreach (AdItem item in items.Results) {

                adsBlock.Set( "item.Name", item.Name );
                adsBlock.Set( "item.CategoryName", item.CategoryName );
                adsBlock.Set( "item.ScopeName", item.ScopeName );
                adsBlock.Set( "item.CreatorName", item.CreatorName );
                adsBlock.Set( "item.StartTime", item.StartStr );
                adsBlock.Set( "item.EndTime", item.EndStr );

                adsBlock.Set( "item.LinkEdit", to( Edit, item.Id ) );
                adsBlock.Set( "item.LinkDelete", to( Delete, item.Id ) );

                adsBlock.Set( "item.StatusStr", item.StatusStr );

                if (item.IsStopped == 1) {
                    adsBlock.Set( "item.StopCmd", "启用" );
                    adsBlock.Set( "item.StatusClass", "stopped" );
                    adsBlock.Set( "item.LinkStop", to( Start, item.Id ) );
                }
                else {
                    adsBlock.Set( "item.StopCmd", "停用" );
                    adsBlock.Set( "item.StatusClass", "" );
                    adsBlock.Set( "item.LinkStop", to( Stop, item.Id ) );
                }

                adsBlock.Next();


            }

            set( "page", items.PageBar );

        }

        public void Add( int categoryId ) {

            target( Create );

            set( "categoryName", AdCategory.GetName( categoryId ) );

            set( "categoryId", categoryId );

            dropList( "adItem.ScopeId", cdb.findAll<AdScope>(), "Name=Id", null );

        }

        [HttpPost, DbTransaction]
        public void Create( ) {

            AdItem item = ctx.PostValue<AdItem>();

            validateItem( item );

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            item.Creator = ctx.viewer.obj as User;
            item.insert();

            echoToParentPart( lang( "opok" ) );
        }

        public void Edit( int id ) {
            target( Update, id );

            AdItem item = AdItem.findById( id );
            if (item == null) {
                echoError( "广告不存在" );
                return;
            }

            set( "categoryId", item.CategoryId );
            set( "adCode", strUtil.EncodeTextarea( item.AdCode ) );

            dropList( "adItem.ScopeId", cdb.findAll<AdScope>(), "Name=Id", item.ScopeId );

            bind( "item", item );
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {

            AdItem item = AdItem.findById( id );
            if (item == null) {
                echoError( "广告不存在" );
                return;
            }

            item = ctx.PostValue( item ) as AdItem;

            validateItem( item );

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            item.update();

            echoToParentPart( lang( "opok" ) );

        }

        private void validateItem( AdItem item ) {

            if (strUtil.IsNullOrEmpty( ctx.Post( "adItem.StartTime" ) )) item.StartTime = new DateTime( 1900, 1, 1 );
            if (strUtil.IsNullOrEmpty( ctx.Post( "adItem.EndTime" ) )) item.EndTime = new DateTime( 2999, 1, 1 );

            item.AdCode = ctx.PostHtmlAll( "adItem.AdCode" );

            if (strUtil.IsNullOrEmpty( item.Name )) errors.Add( "请填写名称" );
        }

        [HttpPost, DbTransaction]
        public void Stop( int id ) {


            AdItem item = AdItem.findById( id );
            if (item == null) {
                echoError( "广告不存在" );
                return;
            }

            item.IsStopped = 1;
            item.update();

            echoRedirectPart( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void Start( int id ) {


            AdItem item = AdItem.findById( id );
            if (item == null) {
                echoError( "广告不存在" );
                return;
            }

            item.IsStopped = 0;
            item.update();

            echoRedirectPart( lang( "opok" ) );
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {

            AdItem item = AdItem.findById( id );
            if (item == null) {
                echoError( "广告不存在" );
            }
            else {
                item.delete();
                echoRedirectPart( lang( "opok" ) );
            }

        }

    }

}
