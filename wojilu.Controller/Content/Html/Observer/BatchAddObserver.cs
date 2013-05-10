using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Htmls {

    public class BatchAddObserver : ActionObserver {


        public override void ObserveActions() {
            Submit.AdminController submit = new Submit.AdminController();
            observe( submit.SaveAdmin );
        }

        public override void AfterAction( MvcContext ctx ) {


            List<ContentPost> xList = HtmlHelper.GetPostListFromContext( ctx );
            if (xList == null || xList.Count == 0) return;

            List<int> ids = new List<int>();
            foreach (ContentPost x in xList) {
                ids.Add( x.Id );
            }

            JobManager.ImportPost( ids );

        }


    }

}
