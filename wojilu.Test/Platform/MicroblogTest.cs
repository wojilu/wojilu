using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Common.Microblogs.Parser;

namespace wojilu.Test.Platform {

    [TestFixture]
    public class MicroblogTest {






        [Test]
        public void testMicroblogAt() {

            String str = ProcessAt( "@张三: 你这个事情不觉得有问题" );
            Assert.AreEqual( "<a href=\"/t/张三\">@张三</a>: 你这个事情不觉得有问题", str );

            str = ProcessAt( "@z:张三" );
            Assert.AreEqual( "<a href=\"/t/z\">@z</a>:张三", str );

            str = ProcessAt( " @张三: 你这个事情不觉得有问题" );
            Assert.AreEqual( " <a href=\"/t/张三\">@张三</a>: 你这个事情不觉得有问题", str );

            // 禁止太长的用户名
            str = ProcessAt( "@ 张三你这个事情不觉得有问题这个事情不觉得有问题这个事情不觉得有问题" );
            Assert.AreEqual( "@ 张三你这个事情不觉得有问题这个事情不觉得有问题这个事情不觉得有问题", str );

            str = ProcessAt( "@ 张三你这个事情不觉得有问题这个事情不觉得有问题这个事情不觉得有问题 @你这个事情不觉得有问题这个事情不觉得有问题这个事情不觉得有问题" ); // 用户名太长无效
            Assert.AreEqual( "@ 张三你这个事情不觉得有问题这个事情不觉得有问题这个事情不觉得有问题 @你这个事情不觉得有问题这个事情不觉得有问题这个事情不觉得有问题", str );


            str = ProcessAt( " @张三      :  你这个事情不觉得有问题" );
            Assert.AreEqual( " <a href=\"/t/张三\">@张三</a>      :  你这个事情不觉得有问题", str );

            str = ProcessAt( " @ 张三: 你这个事情不觉得有问题" );
            Assert.AreEqual( " <a href=\"/t/张三\">@张三</a>: 你这个事情不觉得有问题", str );

            str = ProcessAt( " @   张三: 你这个事情不觉得有问题" );
            Assert.AreEqual( " <a href=\"/t/张三\">@张三</a>: 你这个事情不觉得有问题", str );

            str = ProcessAt( "RT @张三: 你这个事情不觉得有问题" );
            Assert.AreEqual( "RT <a href=\"/t/张三\">@张三</a>: 你这个事情不觉得有问题", str );

            str = ProcessAt( "RT @张三： 你这个事情不觉得有问题" );
            Assert.AreEqual( "RT <a href=\"/t/张三\">@张三</a>： 你这个事情不觉得有问题", str );

            str = ProcessAt( "RT @张三 ： 你这个事情不觉得有问题" );
            Assert.AreEqual( "RT <a href=\"/t/张三\">@张三</a> ： 你这个事情不觉得有问题", str );


            MicroblogParser mp = new MicroblogParser( "RT @张三: 你这个事情不觉得有问题，同时 @ 李四：我也觉得你需要 @张三 考虑", new SimpleMicroblogBinder() );
            String result = mp.Process();
            Assert.AreEqual( "RT <a href=\"/t/张三\">@张三</a>: 你这个事情不觉得有问题，同时 <a href=\"/t/李四\">@李四</a>：我也觉得你需要 <a href=\"/t/张三\">@张三</a> 考虑", result );
            Assert.AreEqual( 2, mp.GetUserList().Count );
            Assert.AreEqual( "张三", mp.GetUserList()[0] );
            Assert.AreEqual( "李四", mp.GetUserList()[1] );


            str = ProcessAt( "@@@@" );
            Assert.AreEqual( "@@@@", str );

            str = ProcessAt( "@" );
            Assert.AreEqual( "@", str );

            str = ProcessAt( "@@" );
            Assert.AreEqual( "@@", str );

            str = ProcessAt( "@@@张三：这是" );
            Assert.AreEqual( "@@<a href=\"/t/张三\">@张三</a>：这是", str );

            str = ProcessAt( "@@ @张三：这是" );
            Assert.AreEqual( "@@ <a href=\"/t/张三\">@张三</a>：这是", str );

            str = ProcessAt( "@@    @张三：这是" );
            Assert.AreEqual( "@@    <a href=\"/t/张三\">@张三</a>：这是", str );

            str = ProcessAt( " @  @    @张三：这是" );
            Assert.AreEqual( " @  @    <a href=\"/t/张三\">@张三</a>：这是", str );

            str = ProcessAt( "你知道吗 @@飞刀@@@" );
            Assert.AreEqual( "你知道吗 @<a href=\"/t/飞刀\">@飞刀</a>@@@", str );

            str = ProcessAt( "你们知道吗@kkk,@张三、@lisi 都是什么人" );
            Assert.AreEqual( "你们知道吗<a href=\"/t/kkk\">@kkk</a>,<a href=\"/t/张三\">@张三</a>、<a href=\"/t/lisi\">@lisi</a> 都是什么人", str );

            str = ProcessAt( "@张三@李四" );
            Assert.AreEqual( "<a href=\"/t/张三\">@张三</a><a href=\"/t/李四\">@李四</a>", str );

            str = ProcessAt( "@张三@李四@wang@sunzhong" );
            Assert.AreEqual( "<a href=\"/t/张三\">@张三</a><a href=\"/t/李四\">@李四</a><a href=\"/t/wang\">@wang</a><a href=\"/t/sunzhong\">@sunzhong</a>", str );



            //-------------------------------------------------------

            str = ProcessAt( "你#奥运#不觉得有问题" );
            Assert.AreEqual( "你<a href=\"/tag/奥运\">#奥运#</a>不觉得有问题", str );

            str = ProcessAt( "你#奥 运#不觉得有问题" );
            Assert.AreEqual( "你<a href=\"/tag/奥 运\">#奥 运#</a>不觉得有问题", str );

            str = ProcessAt( "你#奥运 #不觉得有问题" );
            Assert.AreEqual( "你<a href=\"/tag/奥运\">#奥运#</a>不觉得有问题", str );

            str = ProcessAt( "你#  奥运 #不觉得有问题" );
            Assert.AreEqual( "你<a href=\"/tag/奥运\">#奥运#</a>不觉得有问题", str );

            str = ProcessAt( "#奥运#不觉得有问题" );
            Assert.AreEqual( "<a href=\"/tag/奥运\">#奥运#</a>不觉得有问题", str );

            str = ProcessAt( "  #奥运#不觉得有问题" );
            Assert.AreEqual( "  <a href=\"/tag/奥运\">#奥运#</a>不觉得有问题", str );


            str = ProcessAt( "你#奥运#不觉得 #天气# 有问题" );
            Assert.AreEqual( "你<a href=\"/tag/奥运\">#奥运#</a>不觉得 <a href=\"/tag/天气\">#天气#</a> 有问题", str );

            MicroblogParser pm = new MicroblogParser( "你#奥运#不觉得 #天气# 有问题；还是看看#奥运#好吗", new SimpleMicroblogBinder() );
            pm.Process();
            Assert.AreEqual( "你<a href=\"/tag/奥运\">#奥运#</a>不觉得 <a href=\"/tag/天气\">#天气#</a> 有问题；还是看看<a href=\"/tag/奥运\">#奥运#</a>好吗", pm.ToString() );
            Assert.AreEqual( 2, pm.GetTagList().Count );
            Assert.AreEqual( "奥运", pm.GetTagList()[0] );
            Assert.AreEqual( "天气", pm.GetTagList()[1] );

            MicroblogParser pm2 = new MicroblogParser( "你#奥[抛媚眼]运#不觉得", new SimpleMicroblogBinder() );
            pm2.Process();
            Assert.AreEqual( 0, pm2.GetTagList().Count );
            Assert.AreEqual( "你#奥[抛媚眼]运#不觉得", pm2.ToString() );

            MicroblogParser pm3 = new MicroblogParser( "#奥 [抛媚眼]运# 不觉得", new SimpleMicroblogBinder() );
            pm3.Process();
            Assert.AreEqual( 0, pm3.GetTagList().Count );
            Assert.AreEqual( "#奥 [抛媚眼]运# 不觉得", pm3.ToString() );

            MicroblogParser pm4 = new MicroblogParser( "你#奥 @张三 运#不觉得", new SimpleMicroblogBinder() );
            pm4.Process();
            Assert.AreEqual( "你#奥 @张三 运#不觉得", pm4.ToString() );


            //----------------------------------------------------------------------------

            str = ProcessAt( " @张三: 你#奥运#不觉得 #天气# 有问题" );
            Assert.AreEqual( " <a href=\"/t/张三\">@张三</a>: 你<a href=\"/tag/奥运\">#奥运#</a>不觉得 <a href=\"/tag/天气\">#天气#</a> 有问题", str );

            // tag中的@不会被解析
            str = ProcessAt( "你#奥@张三运#不觉得有问题" );
            Assert.AreEqual( "你#奥@张三运#不觉得有问题", str );

            // 用户名中的tag不会被解析
            str = ProcessAt( "这里 @张#奥运#三: 你不觉得有问题" );
            Assert.AreEqual( "这里 <a href=\"/t/张#奥运#三\">@张#奥运#三</a>: 你不觉得有问题", str );


            //----------------------------------------------------------------------------

            str = ProcessAt( "推荐网址http://www.google.com/page.html 可以吧" );
            Assert.AreEqual( "推荐网址<a href=\"http://www.google.com/page.html\">http://www.google.com/page.html</a> 可以吧", str );

            str = ProcessAt( "推荐网址http://www.google.com/page.html"+Environment.NewLine+"可以吧" );
            Assert.AreEqual( "推荐网址<a href=\"http://www.google.com/page.html\">http://www.google.com/page.html</a> 可以吧", str );


            str = ProcessAt( "http://www.google.com/page.html 可以吧" );
            Assert.AreEqual( "<a href=\"http://www.google.com/page.html\">http://www.google.com/page.html</a> 可以吧", str );

            str = ProcessAt( "http://www.google.com/page.html" );
            Assert.AreEqual( "<a href=\"http://www.google.com/page.html\">http://www.google.com/page.html</a>", str );

            str = ProcessAt( "http://www.go ogle.com/page.html" );
            Console.WriteLine( str );

            str = ProcessAt( "连续网址http://www.google.com/page.html http://www.google.com/page2.html" );
            Assert.AreEqual( "连续网址<a href=\"http://www.google.com/page.html\">http://www.google.com/page.html</a> <a href=\"http://www.google.com/page2.html\">http://www.google.com/page2.html</a>", str );

            str = ProcessAt( "技术搜集：http://www.searchtb.com/2010/11/etao-tech-overview.html http://apple4.us/2010/11/misc-thoughts-about-3q-war.html" );
            Console.WriteLine( str );
        }


        public static String ProcessAt( String str ) {
            return new MicroblogParser( str, new SimpleMicroblogBinder() ).Process();
        }
    }

    public class SimpleMicroblogBinder : IMicroblogBinder {

        public String GetLink( String userName ) {
            return string.Format( "<a href=\"/t/{0}\">@{0}</a>", userName );
        }

        public String GetTagLink( String tag ) {
            return string.Format( "<a href=\"/tag/{0}\">#{0}#</a>", tag.Trim() );
        }

        public String GetUrlLink( String url ) {
            return string.Format( "<a href=\"{0}\">{0}</a>", url.Trim() );
        }

    }


}
