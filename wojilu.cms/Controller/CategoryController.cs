using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.cms.Domain;
using wojilu.Web;

namespace wojilu.cms.Controller {

    public class CategoryController : ControllerBase {

        public CategoryController() {
            base.LayoutControllerType = typeof(ArticleController);
        }

        public void Show( int id ) {

            Category c = Category.findById( id );
            bind( "c", c );
            ctx.SetItem( "category", c );


            DataPage<Article> list = Article.findPage( "CategoryId=" +id );
            IBlock block = getBlock( "list" );
            foreach (Article a in list.Results) {
                block.Set( "a.Title", a.Title );
                block.Set( "a.ShowLink", to( new ArticleController().Show, a.Id ) );
                block.Next();
            }

            set( "page", list.PageBar );
        }

        public void List( int id ) {

            Category c = Category.findById( id );
            DataPage<Article> list = Article.findPage( "CategoryId=" + id, 1 );
            bind( "c", c );

            set( "listContent", loadHtml( ListContent, id ) );

            set( "page", list.PageBar );
        }

        public void ListContent( int id ) {

            Category c = Category.findById( id );
            bind( "c", c );
            ctx.SetItem( "category", c );


            DataPage<Article> list = Article.findPage( "CategoryId=" + id, 1 );
            IBlock block = getBlock( "list" );
            foreach (Article a in list.Results) {
                block.Set( "a.Title", a.Title );
                block.Set( "a.ShowLink", to( new ArticleController().Show, a.Id ) );
                block.Next();
            }
        }

    }

}
