using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Web;
using wojilu.Serialization;
using wojilu.Web.UI;

namespace wojilu.Test.Web.Utils {

    [TestFixture]
    public class CssStyleTest {





        // css规则：
        // 1、每一行一个样式
        // 2、各项之间用单个空格分开
        // 3、分号左右没有空格
        // 4、每一项的结尾都要带分号
/*
#row1_column1 { background:#aaa; width:526px; margin-left:; margin-right:; margin-top:; margin-bottom:; }
#row1_column2 { background:red; width:98%; margin-left:; margin-right:; margin-top:; margin-bottom:; }
*/
        [Test]
        public void testCssToStringSingle() {

            Dictionary<string, string> css = new Dictionary<string, string>();
            css.Add( "background", "#ccc" );
            css.Add( "width", "75%");
            css.Add( "margin-left", "10px");
            css.Add( "margin-right", "10px");
            css.Add( "margin-top", "10px");
            css.Add( "margin-bottom", "10px" );

            string result = "{ background:#ccc; width:75%; margin-left:10px; margin-right:10px; margin-top:10px; margin-bottom:10px; }";
            Console.WriteLine( Css.ToItem(css) );
            Assert.AreEqual( result, Css.ToItem( css ) );            
        }

        [Test]
        public void testCssToStringSingleEmpty() {
            
            Dictionary<string, string> css = new Dictionary<string, string>();
            css.Add( "background", "#ccc" );
            css.Add( "width", "" );
            css.Add( "margin-left", "" );
            css.Add( "margin-right", "" );
            css.Add( "margin-top", "" );
            css.Add( "margin-bottom", "10px" );

            string result = "{ background:#ccc; margin-bottom:10px; }";
            Console.WriteLine( Css.ToItem( css ) );
            Assert.AreEqual( result, Css.ToItem( css ) );
        }

        
        [Test]
        public void testCssToString() {


            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();

            Dictionary<string, string> item1 = new Dictionary<string, string>();
            item1.Add( "background", "#aaa" );
            item1.Add( "width", "526px" );

            Dictionary<string, string> item2 = new Dictionary<string, string>();
            item2.Add( "background", "red" );
            item2.Add( "width", "98%" );


            dic.Add( "#row1_column1", item1 );
            dic.Add( "#row1_column2", item2 );

            string cssStr = 
@"#row1_column1 { background:#aaa; width:526px; }
#row1_column2 { background:red; width:98%; }
";

            string result = Css.To( dic );
            Console.WriteLine( result );
            Assert.AreEqual( cssStr, result );
        }


        //------------------------------------------------------------------------------------------------------------

        //[Test]
        //public void testParserSingle() {

        //    string str = "{ background:#ccc; width:75%; margin-left:10px; margin-right:52px; margin-top:10px; margin-bottom:10px; }";

        //    Dictionary<string, string> css = Css.FromItem( str );
        //    Assert.IsNotNull( css );
        //    Assert.AreEqual( css["background"], "#ccc" );
        //    Assert.AreEqual( css["width"], "75%" );
        //    Assert.AreEqual( css["margin-left"], "10px" );
        //    Assert.AreEqual( css["margin-top"], "10px" );
        //    Assert.AreEqual( css["margin-right"], "52px" );
        //    Assert.AreEqual( css["margin-bottom"], "10px" );
        //}


        [Test]
        public void testParse() {
            string cssStr =
@"  #row1_column1 { background:#aaa; width:526px; margin-left:; margin-right:; margin-top:; margin-bottom  :; }     #row1_column2    {   background:red;   width:98%;   margin-left:; margin-right:; margin-top:8px; margin-bottom:9px;    }  


";

            Dictionary<string, Dictionary<string, string>> dic = Css.FromAndFill( cssStr );
            Assert.IsNotNull( dic );
            Assert.AreEqual( 2, dic.Count );

            Assert.IsTrue( dic.ContainsKey( "#row1_column1" ) );
            Assert.IsTrue( dic.ContainsKey( "#row1_column2" ) );

            Dictionary<string, string> css1 = dic["#row1_column1"];
            Assert.IsNotNull( css1 );
            Assert.AreEqual( "#aaa", css1["background"] );
            Assert.AreEqual( "526px", css1["width"] );
            Assert.AreEqual( css1["margin-left"], "" );
            Assert.AreEqual( css1["margin-top"], "" );
            Assert.AreEqual( css1["margin-right"], "" );
            Assert.AreEqual( css1["margin-bottom"], "" );

            Dictionary<string, string> css2 = dic["#row1_column2"];
            Assert.IsNotNull( css2 );
            Assert.AreEqual( "red", css2["background"] );
            Assert.AreEqual( "98%", css2["width"] );
            Assert.AreEqual( css2["margin-left"], "" );
            Assert.AreEqual( css2["margin-top"], "8px" );
            Assert.AreEqual( css2["margin-right"], "" );
            Assert.AreEqual( css2["margin-bottom"], "9px" );
        }

        [Test]
        public void testParse2() {

            string cssStr =
@"  #row1_column1 {   background:#aaa;  width:526px; margin:0px 0px 5px 5px;  }     #row1_column2 { background:red; width:98%; margin:0px 0px 5px 5px; }  
";

            Dictionary<string, Dictionary<string, string>> dic2 = Css.FromAndFill( cssStr );
            Assert.IsNotNull( dic2 );
            Assert.AreEqual( 2, dic2.Count );

            Assert.IsTrue( dic2.ContainsKey( "#row1_column1" ) );
            Assert.IsTrue( dic2.ContainsKey( "#row1_column2" ) );

            Dictionary<string, string> cssx1 = dic2["#row1_column1"];
            Assert.IsNotNull( cssx1 );
            Assert.AreEqual( "#aaa", cssx1["background"] );
            Assert.AreEqual( "526px", cssx1["width"] );
            //Assert.IsFalse( cssx1.ContainsKey( "margin-left" ) );
            //Assert.IsFalse( cssx1.ContainsKey( "margin-top" ) );

            Dictionary<string, string> cssx2 = dic2["#row1_column2"];
            Assert.IsNotNull( cssx2 );
            Assert.AreEqual( "red", cssx2["background"] );
            Assert.AreEqual( "98%", cssx2["width"] );
            //Assert.IsFalse( cssx2.ContainsKey( "margin-left" ) );
            //Assert.IsFalse( cssx2.ContainsKey( "margin-top" ) );
        }

        [Test]
        public void testParse3() {
            string cssStr =
@"#row1_column1 {width:65%;margin-left:10px;margin-right:10px} #row1_column2 {width:30%;}";

            Dictionary<string, Dictionary<string, string>> dic = Css.FromAndFill( cssStr );
            Assert.IsNotNull( dic );
            Assert.AreEqual( 2, dic.Count );

            Assert.IsTrue( dic.ContainsKey( "#row1_column1" ) );
            Assert.IsTrue( dic.ContainsKey( "#row1_column2" ) );

            Dictionary<string, string> css1 = dic["#row1_column1"];
            Assert.IsNotNull( css1 );
            Assert.AreEqual( "65%", css1["width"] );

            Assert.AreEqual( css1["margin-left"], "10px" );
            Assert.AreEqual( css1["margin-right"], "10px" );

            Dictionary<string, string> css2 = dic["#row1_column2"];
            Assert.IsNotNull( css2 );
            Assert.AreEqual( "30%", css2["width"] );
        }

        [Test]
        public void testBackgroundUrl() {

            string str = @"#row1 { height:; background-color:; background-image:url(http://img4.cache.netease.com/photo/0001/2010-01-22/5TLVBLA619BR0001.jpg); background-position:; background-repeat:; border-width:; border-color:; border-style:; font-size:; font-family:; font-style:; font-weight:; text-decoration:; display:; width:; }";

            Dictionary<string, Dictionary<string, string>> dic = Css.FromAndFill( str );
            Assert.IsNotNull( dic );
            Assert.AreEqual( 1, dic.Count );

            Dictionary<string, string> cssx1 = dic["#row1"];
            Assert.IsNotNull( cssx1 );
            Assert.AreEqual( "url(http://img4.cache.netease.com/photo/0001/2010-01-22/5TLVBLA619BR0001.jpg)", cssx1["background-image"] );
            
        }

        [Test]
        public void testBackgroundUrl2() {

            string str = @"#row1 { height:; background-color:; background-image:url(http://localhost:2001/photo/0001/2010-01-22/5TLVBLA619BR0001.jpg); background-position:; background-repeat:; border-width:; border-color:; border-style:; font-size:; font-family:; font-style:; font-weight:; text-decoration:; display:; width:; }";

            Dictionary<string, Dictionary<string, string>> dic = Css.FromAndFill( str );
            Assert.IsNotNull( dic );
            Assert.AreEqual( 1, dic.Count );

            Dictionary<string, string> cssx1 = dic["#row1"];
            Assert.IsNotNull( cssx1 );
            Assert.AreEqual( "url(http://localhost:2001/photo/0001/2010-01-22/5TLVBLA619BR0001.jpg)", cssx1["background-image"] );

        }

        [Test]
        public void testMergeStyle() {

            string oStyle = @"#row1_column1 { width:32%; margin:5px 5px 5px 10px; }
#row1_column2 { width:40%; margin:5px; }
#row1_column3 { width:24%; margin:5px; }
#row2_column1 { width:100%; height:; background-color:; background-image:; background-position:; background-repeat:; border-width:; border-color:; border-style:; font-size:; font-family:; font-style:; font-weight:; text-decoration:; text-align:; margin-left:; margin-top:; margin-right:; margin-bottom:; padding-left:; padding-top:; padding-right:; padding-bottom:; display:; }
";
            string newStyle = @"#row2_column1 { width:98%; margin-top:5px; margin-right:5px; margin-bottom:5px; margin-left:10px; }";


            string merged = CssFormUtil.mergeStyle( oStyle, newStyle );
            Console.WriteLine( merged );

            int oIndex = merged.IndexOf( "#row2_column1 { width:100%; " );
            int nIndex = merged.IndexOf( "#row2_column1 { width:98%; " );
            Assert.IsTrue( oIndex < 0 );
            Assert.IsTrue( nIndex > 0 );


        }

        [Test]
        public void testMergeNoFill() {
            string oStyle = @"body { background:#33333d; }
#nav { background:url(/static/upload/image/2010-6-17/171715580107549.jpg); }
";
            string newStyle = @"#nav {color:#64648f;}";

            string merged = CssFormUtil.MergeStyle( oStyle, newStyle );
            Console.WriteLine( merged );

            int oIndex = merged.IndexOf( "color" );
            int nIndex = merged.IndexOf( "background:url(" );
            Assert.IsTrue( oIndex > 0 );
            Assert.IsTrue( nIndex > 0 );

        }


        [Test]
        public void testMergeNoFill2() {
            string oStyle = @"body { background:#33333d; }#nav { color:#565699; background:#18182e; }";
            string newStyle = @"#nav a {color:#46eb1d;}";

            string merged = CssFormUtil.MergeStyle( oStyle, newStyle );
            Console.WriteLine( merged );

            int oIndex = merged.IndexOf( "#nav {" );
            int nIndex = merged.IndexOf( "#nav a {" );
            Assert.IsTrue( oIndex > 0 );
            Assert.IsTrue( nIndex > 0 );

        }



    }
}
