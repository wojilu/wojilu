using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.cms.Domain;
using wojilu.Web;
using wojilu.Web.Mvc.Attr;

namespace wojilu.cms.Controller {

    public class PostIndexCache : ActionCache {

        public override string GetCacheKey( wojilu.Web.Context.MvcContext ctx, string actionName ) {
            return "post_index";
        }


    }


    public class PostController : ControllerBase {

 
        [CacheAction( typeof( PostIndexCache ) )]
        public void Index() {

            List<Article> list = Article.findAll();

            IBlock block = getBlock( "list" );
            foreach (Article a in list) {

                block.Set( "a.Title", a.Title );
                block.Set( "a.Content", a.Content );
                block.Next();
            }


        }

    }



}
