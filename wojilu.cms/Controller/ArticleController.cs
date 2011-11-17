using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.cms.Domain;
using wojilu.Web;

namespace wojilu.cms.Controller {

    public class ArticleController : ControllerBase {

        public override void Layout() {
            bindLocation();
            bindNewArticle();
        }

        private void bindLocation() {

            Category c = ctx.GetItem( "category" ) as Category;
            Article a = ctx.GetItem( "article" ) as Article;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( "<a href=\"{0}\">首页</a>", ctx.url.SiteAndAppPath );
            sb.Append( " > " );
            sb.AppendFormat( "<a href=\"{0}\">{1}</a>", to( new CategoryController().Show, c.Id ), c.Name );

            if (a != null) {
                sb.Append( " > " );
                sb.Append( a.Title );
            }

            set( "location", sb );
        }

        private void bindNewArticle() {

            Article ca = ctx.GetItem( "article" ) as Article;
            int id = ca == null ? 0 : ca.Id;

            List<Article> list = Article.find( "order by Id desc" ).list( 8 );
            IBlock block = getBlock( "list" );
            foreach (Article a in list) {
                block.Set( "a.Title", strUtil.CutString( a.Title, 15 ) );
                block.Set( "a.ShowLink", to( new ArticleController().Show, a.Id ) );

                block.Set( "a.Selected", a.Id == id ? "class='fselected'" : "" );

                block.Next();
            }
        }

        public void Show( int id ) {

            Article a = Article.findById( id );
            bind( "a", a );

            ctx.SetItem( "article", a );
            ctx.SetItem( "category", a.Category );
        }

    }

}
