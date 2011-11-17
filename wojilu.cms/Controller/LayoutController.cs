using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.cms.Domain;

namespace wojilu.cms.Controller {

    public class LayoutController : ControllerBase {

        public override void Layout() {

            set( "adminLink", to( new Admin.ArticleController().Index ) ); // 后台管理首页的链接
            set( "loginLink", to( new Admin.LoginController().Login ) );


            List<Category> categories = Category.findAll();
            bindCategories( categories );

            List<Footer> footers = cdb.findAll<Footer>();
            bindFooters( footers );

            bindNavCurrent();
        }

        private void bindNavCurrent() {
            string s = "";
            string path = ctx.url.Path;
            if (path == "" || path == "/" || path.ToLower() == "/default.aspx") s = "class=\"selected\"";
            set( "homeSelected", s );
        }

        private void bindFooters( List<Footer> footers ) {
            IBlock block = getBlock( "footers" );
            foreach (Footer f in footers) {
                block.Set( "f.Name", f.Name );
                block.Set( "f.ShowLink", to( new FooterController().Show, f.Id ) );
                block.Next();
            }
        }

        private void bindCategories( List<Category> categories ) {

            Category current = ctx.GetItem( "category" ) as Category;

            IBlock cblock = getBlock( "categories" );
            foreach (Category c in categories) {
                cblock.Set( "c.Name", c.Name );
                cblock.Set( "c.ShowLink", to( new CategoryController().Show, c.Id ) );

                string selected = getSelected( current, c );
                cblock.Set( "c.Selected", selected );

                cblock.Next();
            }

        }

        private static string getSelected( Category current, Category c ) {

            string selected = "class=\"selected\"";

            if (current == null) return "";
            if (current.Id == c.Id) return selected;
            return "";
        }

    }

}
