using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Interface;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Microblogs {

    public class MbLink {

        public static String ToBlog( IMember owner, int id ) {
            //return Link.To( owner, new Microblogs.MicroblogController().Show, id );
            return Link.To( owner, new Users.HomeController().Info, id );
        }

    }
}
