using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using wojilu.ORM;

namespace wojilu.OpenSample.Domain {

    [Table( "SampleArticle" )]
    public class SampleArticle : ObjectBase<SampleArticle> {

        public String Title { get; set; }
        public DateTime Created { get; set; }

    }

}
