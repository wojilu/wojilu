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
    

    [Table( "SampleUsers" )]
    public class SampleUser : ObjectBase<SampleUser> {

        public String Name { get; set; }
        public String Pwd { get; set; }
        public String Email { get; set; }
        public DateTime Created { get; set; }

        public static SampleUser GetByNameAndPwd( string name, string pwd ) {

            if (string.IsNullOrEmpty( name ) || string.IsNullOrEmpty( pwd )) {
                return null;
            }

            return SampleUser.find( "Name=:name and Pwd=:pwd" )
                .set( "name", name )
                .set( "pwd", pwd )
                .first();
        }

    }
}
