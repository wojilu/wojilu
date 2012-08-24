using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Apps.Blog.Service;

namespace wojilu.Web.Controller.Admin.Apps.Blog {

    [App( typeof( BlogApp ) )]
    public class BlogPickController : ControllerBase {

        public IBlogPickService pickService { get; set; }
        public ISysBlogService postService { get; set; }

        public BlogPickController() {
            pickService = new BlogPickService();
            postService = new SysBlogService();
        }

        public void Index() {

            set( "lnkPicAdd", to( new PickedImgController().Add ) );
            set( "lnkPicAdmin", to( new PickedImgController().Index ) );

            set( "lnkAddPin", to( PinAd ) );
            set( "lnkRestore", to( RestoreList ) );

            int imgCount = 6;
            List<BlogPickedImg> pickedImg = BlogPickedImg.find( "" ).list( imgCount );
            bindImgs( pickedImg );

            List<BlogPost> newPosts = postService.GetSysNew( 0, 21 );
            List<MergedPost> results = pickService.GetAll( newPosts );

            bindCustomList( results );
        }

        private void bindImgs( List<BlogPickedImg> list ) {
            IBlock block = getBlock( "pickedImg" );
            foreach (BlogPickedImg f in list) {
                block.Set( "f.Title", f.Title );
                block.Set( "f.Url", f.Url );
                block.Set( "f.ImgUrl", f.ImgUrl );
                block.Next();
            }
        }

        private void bindCustomList( List<MergedPost> list ) {

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

        private void bindPick( MergedPost x, IBlock block, int index ) {
            if (x.Topic != null) {
                bindPickTopic( x, block, index );
            }
            else {
                bindPickCustom( x, block, index );
            }
        }

        private void bindPickCustom( MergedPost x, IBlock block, int index ) {
            block.Set( "x.Id", index );
            block.Set( "x.Title", x.Title );
            block.Set( "x.Summary", x.Summary );
            block.Set( "x.LinkShow", x.Link );

            block.Set( "x.EditLink", to( EditPin, index ) );
            block.Set( "x.DeleteLink", to( DeletePinConfirm, index ) );
            block.Set( "x.PinLink", to( EditPin, index ) );

            block.Set( "x.Icon", string.Format( "<img src=\"{0}\" />", strUtil.Join( sys.Path.Img, "sticky2.gif" ) ) );

            block.Next();
        }

        private void bindPickTopic( MergedPost x, IBlock block, int index ) {
            block.Set( "x.Id", index );
            block.Set( "x.Title", x.Title );
            block.Set( "x.Summary", x.Summary );

            block.Set( "x.LinkShow", alink.ToAppData( x.Topic ) );

            block.Set( "x.EditLink", to( EditTopic, x.Topic.Id ) );
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

            set( "indexIds", pickService.GetIndexIds( index ) );

            BlogPick customPost = pickService.GetPinPostByIndex( index );
            if (customPost == null) throw new NullReferenceException( "bind BlogCustomPick edit form" );

            set( "x.Title", strUtil.EncodeTextarea( customPost.Title ) );
            set( "x.Link", customPost.Link );
            set( "x.Summary", customPost.Summary );

            set( "x.PinIndex", customPost.PinIndex );
        }

        public void EditTopic( int topicId ) { // 普通topic编辑

            target( UpdateTopic, topicId );

            BlogPost topic = getTopic( topicId );
            if (topic == null) throw new NullReferenceException( "bind BlogPost edit form" );

            BlogPick customPost = pickService.GetEditTopic( topicId );

            if (customPost == null) {

                set( "x.Title", topic.Title );
                set( "x.Link", strUtil.Join( ctx.url.SiteAndAppPath, alink.ToAppData( topic ) ) );
                set( "x.Summary", strUtil.ParseHtml( topic.Content, 300 ) );
            }
            else {
                set( "x.Title", strUtil.EncodeTextarea( customPost.Title ) );
                set( "x.Link", customPost.Link );
                set( "x.Summary", customPost.Summary );
            }
        }

        [HttpPost]
        public void UpdatePin( int index ) {

            BlogPick customPost = pickService.GetPinPostByIndex( index );
            if (customPost == null) throw new NullReferenceException( "bind BlogCustomPick edit form" );

            pickService.EditPinPost( customPost, ctx.PostInt( "Index" ),
                ctx.PostHtml( "Title" ),
                ctx.Post( "Link" ),
                ctx.PostHtml( "Summary" )
                );

            echoToParentPart( lang( "opok" ) );
        }

        [HttpPost]
        public void UpdateTopic( int topicId ) {

            pickService.EditTopic( topicId,
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

            set( "indexIds", pickService.GetIndexIds( 0 ) );
        }

        [HttpPost]
        public void SavePinAd() { // 保存普通广告pin

            pickService.AddPinPost( ctx.PostInt( "Index" ),
                ctx.PostHtml( "Title" ),
                ctx.Post( "Link" ),
                ctx.PostHtml( "Summary" )
                );

            echoToParentPart( lang( "opok" ) );
        }

        public void PinTopic( int topicId ) { // 主题pin

            target( SavePinTopic, topicId );

            set( "indexIds", pickService.GetIndexIds( 0 ) );

            BlogPost topic = getTopic( topicId );
            if (topic == null) throw new NullReferenceException( "PinTopic" );

            set( "x.Title", topic.Title );
            set( "x.Link", strUtil.Join( ctx.url.SiteAndAppPath, alink.ToAppData( topic ) ) );
        }

        [HttpPost]
        public void SavePinTopic( int topicId ) {

            BlogPost topic = getTopic( topicId );
            if (topic == null) throw new NullReferenceException( "PinTopic" );

            pickService.AddPinTopic( topic, ctx.PostInt( "Index" ) );
            echoToParentPart( lang( "opok" ) );
        }

        // 3）删除与恢复
        //-----------------------------------------------------------------------
        public void DeletePinConfirm( int index ) {
            target( DeletePin, index );
        }

        [HttpPost]
        public void DeletePin( int index ) {
            pickService.DeletePinPost( index );
            echoToParentPart( lang( "opok" ) );
        }

        public void DeleteTopicConfirm( int topicId ) {
            target( DeleteTopic, topicId );
        }

        [HttpPost]
        public void DeleteTopic( int topicId ) {

            pickService.DeleteTopic( topicId );

            echoToParentPart( lang( "opok" ) );
        }

        public void RestoreList() {

            DataPage<BlogPick> list = pickService.GetDeleteList();

            IBlock block = getBlock( "list" );
            foreach (BlogPick x in list.Results) {
                block.Set( "x.Title", x.Title );
                block.Set( "x.Link", x.Link );
                block.Set( "x.Id", x.DeleteId );

                BlogPost topic = getTopic( x.DeleteId );
                block.Set( "x.User", topic.Creator.Name );
                block.Set( "x.UserLink", Link.ToMember( topic.Creator ) );

                block.Set( "x.LinkShow", alink.ToAppData( topic ) );
                block.Set( "x.Created", topic.Created );


                block.Set( "x.RestoreLink", to( Restore, x.DeleteId ) );
                block.Next();
            }

            set( "page", list.PageBar );
        }

        public void Restore( int id ) {

            pickService.RestoreTopic( id );

            echoToParentPart( lang( "opok" ) );

        }

        private BlogPost getTopic( int topicId ) {
            if (topicId <= 0) return null;
            return BlogPost.findById( topicId );
        }


    }

}
