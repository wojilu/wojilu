using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.AppBase.Interface;
using wojilu.Common.Picks;

namespace wojilu.Web.Controller.Common {

    public abstract class PickDataBaseController<TData, TPick> : ControllerBase
        where TData : IAppData, IEntity
        where TPick : DataPickBase, new() {

        public DataPickService<TData, TPick> pickService { get; set; }

        public PickDataBaseController() {
            pickService = new DataPickService<TData, TPick>();
        }

        public abstract List<TData> GetNewPosts();
        public abstract String GetImgAddLink();
        public abstract String GetImgListLink();
        public abstract int GetImgCount();

        public void Index() {

            set( "lnkPicAdd", GetImgAddLink() );
            set( "lnkPicAdmin", GetImgListLink() );

            set( "lnkAddPin", to( PinAd ) );
            set( "lnkRestore", to( RestoreList ) );


            IList pickedImg = ndb.find( new TPick().GetImgType(), "AppId=" + ctx.app.Id ).list( this.GetImgCount() );
            bindImgs( pickedImg );

            List<TData> newPosts = this.GetNewPosts();
            List<MergedData> results = pickService.GetAll( newPosts, ctx.app.Id );

            bindCustomList( results );
        }

        private void bindImgs( IList list ) {
            IBlock block = getBlock( "pickedImg" );
            foreach (ImgPickBase f in list) {
                block.Set( "f.Title", f.Title );
                block.Set( "f.Url", f.Url );
                block.Set( "f.ImgUrl", f.ImgUrl );
                block.Next();
            }
        }

        private void bindCustomList( List<MergedData> list ) {

            IBlock hBlock = getBlock( "hotPick" );
            IBlock pBlock = getBlock( "pickList" );

            // 绑定第一个
            if (list.Count == 0) return;
            bindPick( list[0], hBlock, 1 );

            // 绑定列表
            if (list.Count == 1) return;
            for (int i = 1; i < list.Count; i++) {
                bindPick( list[i], pBlock, i + 1 );
            }
        }

        private void bindPick( MergedData x, IBlock block, int index ) {
            if (x.Topic != null) {
                bindPickTopic( x, block, index );
            }
            else {
                bindPickCustom( x, block, index );
            }
        }

        private void bindPickCustom( MergedData x, IBlock block, int index ) {

            block.Set( "x.Id", index );
            block.Set( "x.Title", x.Title );
            block.Set( "x.Summary", x.Summary );
            block.Set( "x.LinkShow", x.Link );

            block.Set( "x.EditLink", to( EditPin, index ) );
            block.Set( "x.DeleteCmd", "取消固定" );
            block.Set( "x.DeleteLink", to( DeletePinConfirm, index ) );
            block.Set( "x.PinLink", to( EditPin, index ) );

            block.Set( "x.Icon", string.Format( "<img src=\"{0}\" />", strUtil.Join( sys.Path.Img, "sticky2.gif" ) ) );

            block.Next();
        }

        private void bindPickTopic( MergedData x, IBlock block, int index ) {

            block.Set( "x.Id", index );
            block.Set( "x.Title", x.Title );
            block.Set( "x.Summary", x.Summary );

            block.Set( "x.LinkShow", alink.ToAppData( x.Topic ) );

            block.Set( "x.EditLink", to( EditTopic, x.Topic.Id ) );
            block.Set( "x.DeleteCmd", "隐藏此帖" );
            block.Set( "x.DeleteLink", to( DeleteTopicConfirm, x.Topic.Id ) );
            block.Set( "x.PinLink", to( PinTopic, x.Topic.Id ) );

            if (x.IsEdit == 1) {
                block.Set( "x.Icon", string.Format( "<img src=\"{0}\" />", strUtil.Join( sys.Path.Img, "doc.gif" ) ) );
            }
            else {
                block.Set( "x.Icon", "" );
            }

            block.Next();
        }

        // 1）编辑
        //-----------------------------------------------------------------------
        public void EditPin( int index ) { // 广告编辑

            target( UpdatePin, index );

            set( "indexIds", pickService.GetIndexIds( ctx.app.Id, index ) );

            TPick customPost = pickService.GetPinPostByIndex( ctx.app.Id, index );
            if (customPost == null) throw new NullReferenceException( "EditPin" );

            set( "x.Title", strUtil.EncodeTextarea( customPost.Title ) );
            set( "x.Link", customPost.Link );
            set( "x.Summary", customPost.Summary );

            set( "x.PinIndex", customPost.PinIndex );
        }

        public void EditTopic( int topicId ) { // 普通topic编辑

            target( UpdateTopic, topicId );

            TData topic = getTopic( topicId );
            if (topic == null) throw new NullReferenceException( "bind edit form" );

            TPick customPost = pickService.GetEditTopic( ctx.app.Id, topicId );

            if (customPost == null) {

                set( "x.Title", topic.Title );
                set( "x.Link", strUtil.Join( ctx.url.SiteAndAppPath, alink.ToAppData( topic ) ) );

                String summary = pickService.getSummary( topic );

                set( "x.Summary", strUtil.ParseHtml( summary, 300 ) );
            }
            else {
                set( "x.Title", strUtil.EncodeTextarea( customPost.Title ) );
                set( "x.Link", customPost.Link );
                set( "x.Summary", customPost.Summary );
            }
        }

        [HttpPost]
        public void UpdatePin( int index ) {

            TPick customPost = pickService.GetPinPostByIndex( ctx.app.Id, index );
            if (customPost == null) throw new NullReferenceException( "UpdatePin " );

            pickService.EditPinPost( customPost, ctx.PostInt( "Index" ),
                ctx.PostHtml( "Title" ),
                ctx.Post( "Link" ),
                ctx.PostHtml( "Summary" )
                );

            echoToParentPart( lang( "opok" ) );
        }

        [HttpPost]
        public void UpdateTopic( int topicId ) {

            pickService.EditTopic( ctx.app.Id, topicId,
                ctx.PostHtml( "Title" ),
                ctx.Post( "Link" ),
                ctx.PostHtml( "Summary" )
                );

            echoToParentPart( lang( "opok" ) );
        }

        // 2）固定
        //-----------------------------------------------------------------------
        public void PinAd() { // 普通广告pin
            target( SavePinAd );

            set( "indexIds", pickService.GetIndexIds( ctx.app.Id ) );
        }

        [HttpPost]
        public void SavePinAd() { // 保存普通广告pin

            int idx = ctx.PostInt( "Index" );
            if (idx <= 0) {
                echoError( "请选择一个固定位置" );
                return;
            }

            Result ret = pickService.AddPinPost( ctx.app.Id, idx,
                ctx.PostHtml( "Title" ),
                ctx.Post( "Link" ),
                ctx.PostHtml( "Summary" )
                );

            if (ret.HasErrors) {
                echoError( ret );
            }
            else {
                echoToParentPart( lang( "opok" ) );
            }
        }

        public void PinTopic( int topicId ) { // 主题pin

            target( SavePinTopic, topicId );

            set( "indexIds", pickService.GetIndexIds( ctx.app.Id ) );

            TData topic = getTopic( topicId );
            if (topic == null) throw new NullReferenceException( "PinTopic" );

            set( "x.Title", topic.Title );
            set( "x.Link", strUtil.Join( ctx.url.SiteAndAppPath, alink.ToAppData( topic ) ) );
        }

        [HttpPost]
        public void SavePinTopic( int topicId ) {

            int idx = ctx.PostInt( "Index" );
            if (idx <= 0) {
                echoError( "请选择一个固定位置" );
                return;
            }

            TData topic = getTopic( topicId );
            if (topic == null) {
                echoError( lang( "exDataNotFound" ) + "=PinTopic" );
                return;
            }

            Result ret = pickService.AddPinTopic( topic, idx );
            if (ret.HasErrors) {
                echoError( ret );
            }
            else {
                echoToParentPart( lang( "opok" ) );
            }
        }

        // 3）删除与恢复
        //-----------------------------------------------------------------------
        public void DeletePinConfirm( int index ) {
            target( DeletePin, index );
        }

        [HttpPost]
        public void DeletePin( int index ) {
            pickService.DeletePinPost( ctx.app.Id, index );
            echoToParentPart( lang( "opok" ) );
        }

        public void DeleteTopicConfirm( int topicId ) {
            target( DeleteTopic, topicId );
        }

        [HttpPost]
        public void DeleteTopic( int topicId ) {

            pickService.DeleteTopic( ctx.app.Id, topicId );

            echoToParentPart( lang( "opok" ) );
        }

        public void RestoreList() {

            DataPage<TPick> list = pickService.GetDeleteList( ctx.app.Id );

            IBlock block = getBlock( "list" );
            foreach (TPick x in list.Results) {
                block.Set( "x.Title", x.Title );
                block.Set( "x.Link", x.Link );
                block.Set( "x.Id", x.DeleteId );

                TData topic = getTopic( x.DeleteId );
                block.Set( "x.User", topic.Creator.Name );
                block.Set( "x.UserLink", toUser( topic.Creator ) );

                block.Set( "x.LinkShow", alink.ToAppData( topic ) );
                block.Set( "x.Created", topic.Created );


                block.Set( "x.RestoreLink", to( Restore, x.DeleteId ) );
                block.Next();
            }

            set( "page", list.PageBar );
        }

        public void Restore( int id ) {

            pickService.RestoreTopic( ctx.app.Id, id );

            echoToParentPart( lang( "opok" ) );

        }

        private TData getTopic( int topicId ) {
            if (topicId <= 0) return default( TData );
            return (TData)db.findById<TData>( topicId );
        }



    }

}
