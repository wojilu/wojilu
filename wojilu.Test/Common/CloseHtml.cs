using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace wojilu.Test.Common {

    [TestFixture]
    public class CloseHtml {



        [Test]
        public void testCloseHtml() {

            String str = "<div><p>　　他在北大申请书中写道：“虽然考取了台湾最好的大学，虽然台湾是祖国的一部分，但是我想到祖国最好的大学念书，因为希望我是它的一部分。”</p> <p>　　选择北大也是因为家族" +
                Environment.NewLine
                + "渊源。“北大是我在大陆选择的唯一学校，”李戡说，“我祖父、大姑、二姑都曾在北大就读，父亲也以未上北大为憾。因此，我的选择也是在为父亲弥补遗憾吧。”</p> <div>　　因为受父亲影响，李戡对文史哲很有兴趣，但他选择的大学专业却是经济。“文史知识，我会去关注，但不太想从事这方面的学术研究。我对投资更感兴趣。我觉得接下来中国将是世界经济龙头，在北京学一点经济，这是很有意义的事情。”</div> <p>　　<strong>成年前著书</strong></p> <p><strong>　　戡台湾教育之乱</strong></p></div>";

            Console.WriteLine( "截取0=>" + strUtil.CutHtmlAndColse( str, 0 ) );
            Console.WriteLine( "截取1=>" + strUtil.CutHtmlAndColse( str, 1 ) );

            // 这种截取是有问题的，会导致tag嵌套错位
            Console.WriteLine( "截取200=>" + strUtil.CutHtmlAndColse( str, 150 ) );

            Assert.AreEqual( str, strUtil.CutHtmlAndColse( str, 900 ) );

            // 前面补齐
            String str2 = "　　他在北大申请书中写道：“虽然考取了台湾最好的大学，虽然台湾是祖国的一部分，但是我想到祖国最好的大学念书，因为希望我是它的一部分。”</p> <p>　　选择北大也是因为家族";
            Console.WriteLine( "补齐头部=>" + strUtil.CloseHtml( str2 ) );
        }

        public void parseHtml() {

            // 补全为xhtml
            // 用xml解析，方法：

            for (int i = 0; i < 101; i++) {
                string ch = char.ConvertFromUtf32( i ); // 得到数字对应的char字符
                char cha = char.Parse( ch );
                int val = (int)cha; // 将字符转换成相应的数字
                Console.WriteLine( i + "__" + ch );
            }
        }


    }


}
