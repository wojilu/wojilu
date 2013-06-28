using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Web.Mvc;
using NUnit.Framework;

namespace wojilu.Test.Web.Mvc {

    public class ChkItem {
        public String Name { get; set; }
        public int Age { get; set; }
    }

    public class HtmlControlTest {


        public void test() {

            // 使用 Dictionary
            Dictionary<String, String> dic = new Dictionary<string, string>();
            dic.Add( "不滚动", "" );
            dic.Add( "朝上滚动", "up" );
            dic.Add( "朝左滚动", "left" );
            dic.Add( "朝下滚动", "down" );
            dic.Add( "朝右滚动", "right" );

            String html = Html.CheckBoxList( dic, "chk", "" );
            Console.WriteLine( html );
            String str = "<label class=\"checkbox\" id=\"lbl-chk0\"><input type=\"checkbox\" id=\"chk0\" name=\"chk\" value=\"\" /> 不滚动</label> <label class=\"checkbox\" id=\"lbl-chk1\"><input type=\"checkbox\" id=\"chk1\" name=\"chk\" value=\"up\" /> 朝上滚动</label> <label class=\"checkbox\" id=\"lbl-chk2\"><input type=\"checkbox\" id=\"chk2\" name=\"chk\" value=\"left\" /> 朝左滚动</label> <label class=\"checkbox\" id=\"lbl-chk3\"><input type=\"checkbox\" id=\"chk3\" name=\"chk\" value=\"down\" /> 朝下滚动</label> <label class=\"checkbox\" id=\"lbl-chk4\"><input type=\"checkbox\" id=\"chk4\" name=\"chk\" value=\"right\" /> 朝右滚动</label> ";
            Assert.AreEqual( str.Trim(), html.Trim() );

            // 使用 array
            String[] items = new String[] { "春节", "端午", "中秋" };
            html = Html.CheckBoxList( items, "hday", "端午" );
            Console.WriteLine( html );

            str = "<label class=\"checkbox\" id=\"lbl-hday0\"><input type=\"checkbox\" id=\"hday0\" name=\"hday\" value=\"春节\" /> 春节</label> <label class=\"checkbox\" id=\"lbl-hday1\"><input type=\"checkbox\" id=\"hday1\" name=\"hday\" value=\"端午\" checked=\"checked\"/> 端午</label> <label class=\"checkbox\" id=\"lbl-hday2\"><input type=\"checkbox\" id=\"hday2\" name=\"hday\" value=\"中秋\" /> 中秋</label> ";
            Assert.AreEqual( str.Trim(), html.Trim() );

            // 使用 list
            List<ChkItem> list = new List<ChkItem>();
            list.Add( new ChkItem { Name = "n1", Age = 8 } );
            list.Add( new ChkItem { Name = "n2", Age = 12 } );
            list.Add( new ChkItem { Name = "n3", Age = 19 } );

            html = Html.CheckBoxList( list, "yname", "Name", "Age", "8,12" );
            Console.WriteLine( html );

            str = "<label class=\"checkbox\" id=\"lbl-yname0\"><input type=\"checkbox\" id=\"yname0\" name=\"yname\" value=\"8\" checked=\"checked\"/> n1</label> <label class=\"checkbox\" id=\"lbl-yname1\"><input type=\"checkbox\" id=\"yname1\" name=\"yname\" value=\"12\" checked=\"checked\"/> n2</label> <label class=\"checkbox\" id=\"lbl-yname2\"><input type=\"checkbox\" id=\"yname2\" name=\"yname\" value=\"19\" /> n3</label>";
            Assert.AreEqual( str.Trim(), html.Trim() );


        }

    }

}
