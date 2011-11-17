using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.cms.Controller.Admin;
using wojilu.cms.Domain;
using wojilu.Web;

namespace wojilu.cms.Controller {

    public class MainController : ControllerBase {


        public void Index() {

            List<Article> recent = Article.find( "" ).list( 5 );
            IBlock block = getBlock( "recent" );
            foreach (Article a in recent) {
                block.Set( "a.Title", a.Title );
                block.Set( "a.ShowLink", to( new ArticleController().Show, a.Id ) );
                block.Next();
            }

            bind( "cfg", SystemConfig.Instance );
        }

    }

}
