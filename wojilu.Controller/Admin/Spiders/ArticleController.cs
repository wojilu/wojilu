/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.Spider.Domain;
using wojilu.Common.Spider.Service;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Admin.Spiders {

    public class ArticleController : ControllerBase {


        public void List( int id ) {

            String condition = id > 0 ? "SpiderTemplateId=" + id : "";

            set( "OperationUrl", to( Admin ) );

            String refreshCmd = "";
            if (id > 0) refreshCmd = string.Format( "<a href=\"{0}\">现在刷新采集</a>", to( new TemplateController().DoRefresh, id ) );
            set( "refreshCmd", refreshCmd );

            DataPage<SpiderArticle> list = SpiderArticle.findPage( condition );
            IBlock block = getBlock( "list" );
            foreach (SpiderArticle p in list.Results) {

                block.Set( "post.Id", p.Id );
                block.Set( "post.Title", p.Title );

                if (p.SpiderTemplate == null || p.SpiderTemplate.SiteName == null) {
                    block.Set( "post.Category", "" );
                    block.Set( "post.CategoryLink", "#" );
                }
                else {
                    block.Set( "post.Category", p.SpiderTemplate.SiteName );
                    block.Set( "post.CategoryLink", to( List, p.SpiderTemplate.Id ) );
                }

                block.Set( "post.Link", to( Show, p.Id ) );
                block.Set( "post.Created", p.Created );
                block.Set( "post.EditUrl", to( Edit, p.Id ) );
                block.Next();
            }
            set( "page", list.PageBar );
        }

        public void Edit( int id ) {
            target( Update, id );
            SpiderArticle article = SpiderArticle.findById( id );
            bind( "data", article );
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {
            SpiderArticle article = SpiderArticle.findById( id );
            article = ctx.PostValue( article ) as SpiderArticle;
            article.update();
            echoRedirect( lang( "opok" ), to( Show, id ) );
        }

        [HttpPost, DbTransaction]
        public void Admin() {
            String ids = ctx.PostIdList( "choice" );
            String action = ctx.Post( "action" );

            if (strUtil.IsNullOrEmpty( ids )) {
                echoError( "请先选择" );
                return;
            }

            if ("delete".Equals( action )) {
                SpiderArticle.deleteBatch( "Id in (" + ids + ")" );
            }

            echoAjaxOk();
        }

        public void Show( int id ) {

            SpiderArticle post = SpiderArticle.findById( id );
            bind( "post", post );
            set( "refreshPageLink", to( DoRefresh, id ) );
            set( "editLink", to( Edit, id ) );
        }

        [HttpPost, DbTransaction]
        public void DoRefresh( int id ) {

            SpiderArticle post = SpiderArticle.findById( id );

            StringBuilder log = new StringBuilder();
            String content = new PagedDetailSpider().GetContent( post.Url, post.SpiderTemplate, log );
            if (strUtil.HasText( content )) {
                post.Body = content;
                post.update( "Body" );
                echoJsonMsg( "刷新成功", true, "" );
            }
            else {
                errors.Add( log.ToString().Replace( Environment.NewLine, "" ).Trim() );
                echoError();
            }


        }

    }

}
