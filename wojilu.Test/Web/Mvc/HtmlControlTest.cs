using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Test.Web.Mvc {

    public class HtmlControlTest {


        public void test() {

            Dictionary<String, String> dic = new Dictionary<string, string>();
            dic.Add( "不滚动", "" );
            dic.Add( "朝上滚动", "up" );
            dic.Add( "朝左滚动", "left" );
            dic.Add( "朝下滚动", "down" );
            dic.Add( "朝右滚动", "right" );

            String html = Html.CheckBoxList( dic, "chk", "" );
            Console.WriteLine( html );


        }

    }

}
