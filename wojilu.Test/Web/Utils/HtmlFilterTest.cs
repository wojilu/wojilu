using System;
using System.Collections.Generic;
using wojilu.Web.Utils;
using wojilu.Web.Utils.Tags;
using NUnit.Framework;

namespace wojilu.Test.Web.Utils {

    [TestFixture]
    public class HtmlFilterTest {







        [Test]
        public void testBase() {

            String input = "这是脚本<script>引用了</script>内容";

            String result = HtmlFilter.Filter( input );
            Assert.AreEqual( "这是脚本内容", result );

            Console.WriteLine( result );

            input = "<P>　　<FONT face=楷体_GB2312>从1990年至2000年期间 </FONT></P> ";
            result = HtmlFilter.Filter( input ); // 过滤之后，会给属性加上引号            
            Assert.AreEqual( "<P>　　<FONT face=\"楷体_GB2312\">从1990年至2000年期间 </FONT></P> ", result );

        }

        [Test]
        public void testAllowedFlash() {

            String input = "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" data=\"http://player.youku.com/player.php/sid/XMTg0MTI0NjU2/v.swf\" type=\"application/x-shockwave-flash\" width=\"300\" height=\"255\"><param name=\"movie\" value=\"http://player.youku.com/player.php/sid/XMTg0MTI0NjU2/v.swf\"></object>";
            String str = HtmlFilter.Filter( input );
            Assert.AreEqual( input, str );

            input = "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0\" width=\"95\" height=\"360\">  <param name=\"movie\" value=\"a.swf\" />  <param name=\"quality\" value=\"high\" />  <embed src=\"a.swf\" quality=\"high\" pluginspage=\"http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash\" type=\"application/x-shockwave-flash\" width=\"300\" height=\"255\"></embed></object>";
            str = HtmlFilter.Filter( input );
            Assert.AreEqual( input, str );

            String flash = WebHelper.GetFlash( "www.abc.com/x.swf", 480, 360 );
            Console.WriteLine( flash );
            str = HtmlFilter.Filter( flash );
            Assert.AreEqual( flash, str );
        }

        [Test]
        public void testTableTag() {

            String input = "<table></table>";
            String str = HtmlFilter.Filter( input );
            Assert.AreEqual( input, str );

            input = "<table><tr><td>abc</td></tr></table>";
            str = HtmlFilter.Filter( input );
            Assert.AreEqual( input, str );

            input = "<table ><tr ><td > abc </td></tr></table>";
            str = HtmlFilter.Filter( input );
            Assert.AreEqual( input, str );

            input = "<table border=\"5\"><tr ><td > abc </td></tr></table>";
            str = HtmlFilter.Filter( input );
            Assert.AreEqual( input, str );

            // 不在白名单的属性会被删除
            String newInput = "<table border=\"5\" data-new=\"88\"><tr ><td > abc </td></tr></table>";
            str = HtmlFilter.Filter( newInput );
            Assert.AreEqual( input, str );

        }

        [Test]
        public void testAllowedTags() {

            String input = "这是多种<span>内容1</span>标签和<strong>内容2</strong>风格，还有<img src=\"\" />图片与<a href=\"abc.html\">链接</a>对吗？";
            String result = HtmlFilter.Filter( input, "strong" );
            Assert.AreEqual( "这是多种内容1标签和<strong>内容2</strong>风格，还有图片与链接对吗？", result );

            result = HtmlFilter.Filter( input, "img" );
            Assert.AreEqual( "这是多种内容1标签和内容2风格，还有<img src=\"\" />图片与链接对吗？", result );

            result = HtmlFilter.Filter( input, "a" );
            Assert.AreEqual( "这是多种内容1标签和内容2风格，还有图片与<a href=\"abc.html\">链接</a>对吗？", result );

            result = HtmlFilter.Filter( input, "img,a" );
            Assert.AreEqual( "这是多种内容1标签和内容2风格，还有<img src=\"\" />图片与<a href=\"abc.html\">链接</a>对吗？", result );

            // 对换行的测试
            result = HtmlFilter.Filter( "这是多种内容1标签和<br>内容2风格", "br" );
            Assert.AreEqual( "这是多种内容1标签和<br>内容2风格", result );

            result = HtmlFilter.Filter( "这是多种内容1标签和<br  >内容2风格", "br" );
            Assert.AreEqual( "这是多种内容1标签和<br  >内容2风格", result );

            result = HtmlFilter.Filter( "这是多种内容1标签和<br />内容2风格", "br" );
            Assert.AreEqual( "这是多种内容1标签和<br />内容2风格", result );

            result = HtmlFilter.Filter( "这是多种内容1标签和<br/>内容2风格", "br" );
            Assert.AreEqual( "这是多种内容1标签和<br/>内容2风格", result );

            result = HtmlFilter.Filter( "这是<strong>多种</strong>内容1标签和<br>内容2风格", "strong" );
            Assert.AreEqual( "这是<strong>多种</strong>内容1标签和内容2风格", result );

            //-----------------------------------------

            input = "文字<style>div{color:red;}</style>内容";
            result = HtmlFilter.Filter( input, "style" );
            Assert.AreEqual( input, result );

            Dictionary<String, String> dic = new Dictionary<String, String>();
            dic.Add( "script", "src" );
            input = "文字<script src=\"aa\">some code</script>内容";
            result = HtmlFilter.Filter( input, dic );
            Assert.AreEqual( input, result );

            dic = new Dictionary<String, String>();
            dic.Add( "script", "src" ); // 没有的属性会被过滤
            input = "文字<script src=\"aa\" xdata=99>some code</script>内容";
            result = HtmlFilter.Filter( input, dic );
            Assert.AreEqual( "文字<script src=\"aa\">some code</script>内容", result );

        }

        [Test]
        public void testComment() {

            // 默认是允许注释的
            String input = "<ul><!-- BEGIN list --><li>这是注释</li><!-- END list --></ul>";

            String result = HtmlFilter.Filter( input );
            Assert.AreEqual( input, result );

            input = "<!--abcd<script>alert( 'abcd' );</script>123-->";

            result = HtmlFilter.Filter( input );
            Console.WriteLine( result );
            Assert.AreEqual( "<!--abcd123-->", result );
        }


        [Test]
        public void testGetAttr() {

            string attr = @"class=""Apple-style-span"" style=""font-size: x-large;""";
            Dictionary<String, String> dic = TagHelper.getAttributes( attr );
            Assert.AreEqual( 2, dic.Count );
            Assert.AreEqual( "Apple-style-span", dic["class"] );
            Assert.AreEqual( "font-size: x-large;", dic["style"] );

            attr = @"  class=""Apple-style-span"" style=""font-size : x-large;""";
            dic = TagHelper.getAttributes( attr );
            Assert.AreEqual( 2, dic.Count );
            Assert.AreEqual( "Apple-style-span", dic["class"] );
            Assert.AreEqual( "font-size : x-large;", dic["style"] );

            attr = @" class = ""Apple-style-span"" style = ""font-size:x-large;""";
            dic = TagHelper.getAttributes( attr );
            Assert.AreEqual( 2, dic.Count );
            Assert.AreEqual( "Apple-style-span", dic["class"] );
            Assert.AreEqual( "font-size:x-large;", dic["style"] );

            attr = @"  class=""Apple-style-span"" style=""font-size : x-large;"" width=""350"" target = _blank    height=240";
            dic = TagHelper.getAttributes( attr );
            Assert.AreEqual( 5, dic.Count );
            Assert.AreEqual( "Apple-style-span", dic["class"] );
            Assert.AreEqual( "font-size : x-large;", dic["style"] );
            Assert.AreEqual( "350", dic["width"] );
            Assert.AreEqual( "_blank", dic["target"] );
            Assert.AreEqual( "240", dic["height"] );

            attr = @"  WIDTH=88 height=""99"" data-link=""http://www.abc.com/x.html""";
            dic = TagHelper.getAttributes( attr );
            Assert.AreEqual( 3, dic.Count );
            Assert.AreEqual( "88", dic["WIDTH"] );
            Assert.AreEqual( "99", dic["height"] );
            Assert.AreEqual( "http://www.abc.com/x.html", dic["data-link"] );

            attr = @"  alt ='myalt' title =  ""xtitle""";
            dic = TagHelper.getAttributes( attr );
            Assert.AreEqual( 2, dic.Count );
            Assert.AreEqual( "myalt", dic["alt"] );
            Assert.AreEqual( "xtitle", dic["title"] );

            attr = " classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0\" width=\"95\" height=\"360\"";
            dic = TagHelper.getAttributes( attr );
            Assert.AreEqual( 4, dic.Count );
            Assert.AreEqual( "clsid:D27CDB6E-AE6D-11cf-96B8-444553540000", dic["classid"] );
            Assert.AreEqual( "http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0", dic["codebase"] );
            Assert.AreEqual( "95", dic["width"] );
            Assert.AreEqual( "360", dic["height"] );


        }

        public string getEndTagName( string tag ) {

            if (strUtil.IsNullOrEmpty( tag )) return null;
            if (tag.StartsWith( "<" ) == false) return null;

            bool isSecondSlash = false;
            string str = "";
            for (int i = 1; i < tag.Length - 1; i++) {
                if (tag[i] == ' ') {
                    continue;
                }

                if (isSecondSlash) {
                    str += tag[i];
                    continue;
                }

                if (tag[i] != '/') {
                    return null;
                }

                if (tag[i] == '/') {
                    isSecondSlash = true;
                    continue;
                }
            }
            return str.Trim();
        }

        [Test]
        public void testGetEndTag() {

            Assert.AreEqual( getEndTagName( "</font>" ), "font" );
            Assert.AreEqual( getEndTagName( "</ ol>" ), "ol" );
            Assert.AreEqual( getEndTagName( "</ li>" ), "li" );
            Assert.AreEqual( getEndTagName( "</          div>" ), "div" );
            Assert.AreEqual( getEndTagName( "< /ol>" ), "ol" );
            Assert.AreEqual( getEndTagName( "<  /li>" ), "li" );
            Assert.AreEqual( getEndTagName( "<div>" ), null );
        }


        [Test]
        public void testGetTags() {

            string input = @"这是<span style=""font-weight: bold;"">一位著名的</span>作家在离<span style=""font-weight: bold;"">开了家乡</span>之后，<br>来到<span style=""font-style: italic;"">这个荒</span>无人烟的<span style=""text-decoration: underline;"">沙漠和</span>古城，也就是敦煌，<br>也<span style=""font-family: 微软雅黑;"">就是中国中古</span>时期的、<br>世界的<font size=""5"">丝绸之路</font>的必经之路。<br>他<span style=""color: rgb(255, 0, 0);"">没有水，没有</span>粮食，<br>但他<span style=""background-color: rgb(0, 128, 128);"">有坚持下</span>来的决心……<br>这是一位<img src=""/static/js/editor/skin/em/011.gif"">著名的作家在离开了家乡之后，来到<span style=""text-decoration: line-through;"">这个荒无人烟</span>的沙漠和古城，也就是敦煌，也就是中国<br><table style=""width: 60%; border-collapse: collapse;"" border=""1""> <tbody><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr> <tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr> </tbody></table><br><hr style=""width: 100%; height: 2px;"">&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<br><div style=""text-align: center;"">中古时期的、世界<br></div><div style=""text-align: right;"">的丝绸之路的必经之路。<br></div><br>他没有水，没有<sup> 粮食</sup>，但他有<sub>坚持</sub>下来的决心……<br><br><ul><li>这是一位著名的作家</li><li>在离开了家乡之后，</li><li>来到这个荒无人烟的沙漠和古城，< /li></ul><br>也就是敦煌，也就是中国中古时期的、世界的丝绸之路的必经之路。他没有水，没有粮食，但他有坚持下来的决心……这是一位著名的作家在离开了家乡之后，<br><br><ol><li>来到这个荒无人烟</li><li>的沙漠和古城，</li><li>也就是敦煌，</li>< /ol><br>也就是中国中古时期 <br>";

            Console.WriteLine( HtmlFilter.Filter( input ) );
        }

        [Test]
        public void testScriptAndIFrame() {


            string val = HtmlFilter.Filter( "国灭亡论，是中国人就得看！！！——日本宣称将于2015年灭亡中国。本文并非在下所写，只是基于一个爱国者的心，将此文贴于此。（这是一个在日本人气极旺的帖子，感谢一位不知名的网友，翻译过来。本着“奇文共欣赏，疑义相与析”的原则，转载过来是给大家一个对“日本”这个禽兽国度清醒的" );
            Assert.AreEqual( val, target );

            val = HtmlFilter.Filter( "国灭亡论，是中国人就得看！！！——日本宣称将于2015年灭亡中国。本文并非在下所写，只是基于一个爱国者的心，将此文贴于此。（这是<script> alert(  'kkkkk'); </script>一个在日本人气极旺的帖子，感谢一位不知名的网友，翻译过来。本着“奇文共欣赏，疑义相与析”的原则，转载过来是给大家一个对“日本”这个禽兽国度清醒的" );
            Assert.AreEqual( val, target );

            val = HtmlFilter.Filter( "国灭亡论，是中国人就得看！！！——日本宣称将于2015年灭亡中国。本文并非在下所写，只是基于一个爱国者的心，将此文贴于此。（这是<iframe src=abc.php width=500> </iframe>一个在日本人气极旺的帖子，感谢一位不知名的网友，翻译过来。本着“奇文共欣赏，疑义相与析”的原则，转载过来是给大家一个对“日本”这个禽兽国度清醒的" );
            Assert.AreEqual( val, target );

            val = HtmlFilter.Filter( "国灭亡论，是中国人就得看！！！——日本宣称将于2015年灭亡中国。本文并非在下所写，只是基于一个爱国者的心，将此文贴于此。（这是<scRIPT> alert(  'kkkkk'); </SCRIPT>一个在日本人气极旺的帖子，感谢一位不知名的网友，翻译过来。本着“奇文共欣赏，疑义相与析”的原则，转载过来是给大家一个对“日本”这个禽兽国度清醒的" );
            Assert.AreEqual( val, target );

            val = HtmlFilter.Filter( "国灭亡论，是中国人就得看！！！——日本宣称将于2015年灭亡中国。本文并非在下所写，只是基于一个爱国者的心，将此文贴于此。（这是<ifRAme src=abc.php width=500> </IfRAMe>一个在日本人气极旺的帖子，感谢一位不知名的网友，翻译过来。本着“奇文共欣赏，疑义相与析”的原则，转载过来是给大家一个对“日本”这个禽兽国度清醒的" );
            Assert.AreEqual( val, target );

        }

        [Test]
        public void testLink() {

            String str = "aaa<a href=\"xxx.html\">ccc</a>zzz";
            string val = HtmlFilter.Filter( str );
            Assert.AreEqual( val, str );

            str = "AAA<a href=\"XXX.html\">CCC</a>ZZZ";
            val = HtmlFilter.Filter( str );
            Assert.AreEqual( val, str );
        }


        [Test]
        public void testStyle() {

            String input = @"<style id=""abc"">
.h1 {
	FONT-WEIGHT: bold; TEXT-JUSTIFY: inter-ideograph; FONT-SIZE: 22pt; MARGIN: 17pt 0cm 16.5pt; LINE-HEIGHT: 240%; TEXT-ALIGN: justify
}
.h2 {
	FONT-WEIGHT: bold; TEXT-JUSTIFY: inter-ideograph; FONT-SIZE: 16pt; MARGIN: 13pt 0cm; LINE-HEIGHT: 173%; TEXT-ALIGN: justify
}
.h3 {
	FONT-WEIGHT: bold; TEXT-JUSTIFY: inter-ideograph; FONT-SIZE: 16pt; MARGIN: 13pt 0cm; LINE-HEIGHT: 173%; TEXT-ALIGN: justify
}
.h1 {
	FONT-WEIGHT: bold; TEXT-JUSTIFY: inter-ideograph; FONT-SIZE: 22pt; MARGIN: 17pt 0cm 16.5pt; LINE-HEIGHT: 240%; TEXT-ALIGN: justify
}
.h2 {
	FONT-WEIGHT: bold; TEXT-JUSTIFY: inter-ideograph; FONT-SIZE: 16pt; MARGIN: 13pt 0cm; LINE-HEIGHT: 173%; TEXT-ALIGN: justify
}
.h3 {
	FONT-WEIGHT: bold; TEXT-JUSTIFY: inter-ideograph; FONT-SIZE: 16pt; MARGIN: 13pt 0cm; LINE-HEIGHT: 173%; TEXT-ALIGN: justify
}
</style>" + @"<P>　　<FONT face=楷体_GB2312>从1990年至2000年期间，人文社会科学领域学术期刊的价格涨幅高达185.9%，科技和医学领域学术期刊价格涨幅分别达178.3%和184.3%。全球范围内出现的“期刊危机”，倒逼科技界提出一项全新的出版理念——“开放获取”。 </FONT></P> 
<P><FONT face=楷体_GB2312>　　所谓“开放获取”(Open Access)，是指科学研究信息在网络环境中，免费供公众自由获取。开放获取有两种实现形式：一为开放出版，即期刊或会议论文出版后立即开放获取；二是开放存储，即论文出版后存储到相关知识库，一段时间后开放获取。</FONT></P> ";

            String result = HtmlFilter.Filter( input );

            String str = @"<P>　　<FONT face=""楷体_GB2312"">从1990年至2000年期间，人文社会科学领域学术期刊的价格涨幅高达185.9%，科技和医学领域学术期刊价格涨幅分别达178.3%和184.3%。全球范围内出现的“期刊危机”，倒逼科技界提出一项全新的出版理念——“开放获取”。 </FONT></P> 
<P><FONT face=""楷体_GB2312"">　　所谓“开放获取”(Open Access)，是指科学研究信息在网络环境中，免费供公众自由获取。开放获取有两种实现形式：一为开放出版，即期刊或会议论文出版后立即开放获取；二是开放存储，即论文出版后存储到相关知识库，一段时间后开放获取。</FONT></P> ";
            Console.WriteLine( result );

            Assert.AreEqual( str, result );
        }

        private string target = "国灭亡论，是中国人就得看！！！——日本宣称将于2015年灭亡中国。本文并非在下所写，只是基于一个爱国者的心，将此文贴于此。（这是一个在日本人气极旺的帖子，感谢一位不知名的网友，翻译过来。本着“奇文共欣赏，疑义相与析”的原则，转载过来是给大家一个对“日本”这个禽兽国度清醒的";
    }




}
