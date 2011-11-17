using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Net;
using System.Text.RegularExpressions;
using wojilu.Net.Video;
using wojilu.Serialization;
using System.Xml;

namespace wojilu.Test.Net {

    [TestFixture]
    [Ignore( "本方法是网络测试，需要先连接到互联网才能进行。联网之后，请删除本批注" )]
    public class VideoSpiderTest {








        [Test]
        public void testYouku() {

            IVideoSpider spider = new WojiluVideoSpider();

            VideoInfo vi;

            vi = spider.GetInfo( "http://v.youku.com/v_show/id_XMjI0OTY4MzEy.html" );
            Assert.AreEqual( "http://player.youku.com/player.php/sid/XMjI0OTY4MzEy/v.swf", vi.FlashUrl );
            Assert.AreEqual( "http://g3.ykimg.com/0100641F464CED4FCEB2B80136E60600173307-35C8-EFF6-FDBD-39E08C062677", vi.PicUrl );
            Assert.AreEqual( "一堆睡觉的猫咪 亮点在后面", vi.Title );

            vi = spider.GetInfo( "http://v.youku.com/v_show/id_XMjMzMTY1MjI4.html" );

            Assert.AreEqual( vi.Title, "张国立欲与曾志伟试比高 《一代宗师》梁朝伟很受伤 101229 每日文娱播报" );
            Assert.AreEqual( vi.PicUrl, "http://g4.ykimg.com/01270F1F464D1B2FC7E7C5000000006CB58714-5F6A-E603-B3A1-3EE2625F5906" );
            Assert.AreEqual( vi.FlashUrl, "http://player.youku.com/player.php/sid/XMjMzMTY1MjI4/v.swf" );


            vi = spider.GetInfo( "http://v.youku.com/v_show/id_XMjI5OTA5MjI0.html" );
            Assert.AreEqual( vi.Title, "可爱师妹撼动跆拳道馆" );
            Assert.AreEqual( vi.PicUrl, "http://g2.ykimg.com/0100641F464D0861E54D1F00DD713DC795B776-AF28-1C21-A151-4B7CCF964842" );
            Assert.AreEqual( vi.FlashUrl, "http://player.youku.com/player.php/sid/XMjI5OTA5MjI0/v.swf" );


            vi = spider.GetInfo( "http://v.youku.com/v_show/id_XMjMxNjg5NzQ0.html" );
            Assert.AreEqual( vi.Title, "搭车人" );
            Assert.AreEqual( vi.PicUrl, "http://g2.ykimg.com/0100641F464D12FE99AADD019750A5E76F7C71-9AAF-AD3A-634F-36A28DC2C07B" );
            Assert.AreEqual( vi.FlashUrl, "http://player.youku.com/player.php/sid/XMjMxNjg5NzQ0/v.swf" );

            vi = spider.GetInfo( "http://v.youku.com/v_show/id_XMjIwNTk1ODA4.html" );
            Assert.AreEqual( vi.Title, "上海美女教你吃大闸蟹，嗲思特了" );
            Assert.AreEqual( vi.PicUrl, "http://g2.ykimg.com/0100641F464CD602CD0A210177D0176103096F-C242-AD2F-708A-E53ACDEE5696" );
            Assert.AreEqual( vi.FlashUrl, "http://player.youku.com/player.php/sid/XMjIwNTk1ODA4/v.swf" );
        }

        [Test]
        public void testTudou() {

            IVideoSpider spider = new WojiluVideoSpider();

            VideoInfo vi;

            vi = spider.GetInfo( "http://www.tudou.com/programs/view/w9ial_Y0aOA/" );
            Assert.AreEqual( "http://www.tudou.com/v/w9ial_Y0aOA/v.swf", vi.FlashUrl );
            Assert.AreEqual( "http://i1.tdimg.com/064/198/736/m15.jpg", vi.PicUrl );
            Assert.AreEqual( "笑死不偿命哦,一段动物的搞笑片段", vi.Title );


            vi = spider.GetInfo( "http://www.tudou.com/programs/view/deuXg6s3F7k/" );
            Assert.AreEqual( vi.Title, "独立纪录片《马克》宣传片 赤程传媒出品" );
            Assert.AreEqual( vi.PicUrl, "http://i1.tdimg.com/067/444/768/m10.jpg" );
            Assert.AreEqual( vi.FlashUrl, "http://www.tudou.com/v/deuXg6s3F7k/v.swf" );
        }

        [Test]
        public void test56Com() {

            IVideoSpider spider = new WojiluVideoSpider();

            VideoInfo vi;

            vi = spider.GetInfo( "http://www.56.com/u20/v_NTczODI3ODU.html" );
            Assert.AreEqual( vi.Title, "看看贪吃的小孩是怎么变脸的" );
            Assert.AreEqual( vi.PicUrl, "http://img.v140.56.com/images/7/13/wangyue5200i56olo56i56.com_zhajm_129311887711hd.jpg" );
            Assert.AreEqual( vi.FlashUrl, "http://player.56.com/v_NTczODI3ODU.swf" );
        }

        [Test]
        public void testKu6() {

            IVideoSpider spider = new WojiluVideoSpider();

            VideoInfo vi;

            vi = spider.GetInfo( "http://v.ku6.com/show/XdOJWbJHwhc1RDPv.html" );
            Assert.AreEqual( "http://player.ku6.com/refer/XdOJWbJHwhc1RDPv/v.swf", vi.FlashUrl );
            Assert.AreEqual( "http://i0.ku6img.com/encode/picpath/2010/11/25/17/1293744262316/5.jpg", vi.PicUrl );
            Assert.AreEqual( "爆笑 会站着走的兔子 在线观看", vi.Title );
        }

        [Test]
        public void testSina() {

            IVideoSpider spider = new WojiluVideoSpider();

            VideoInfo vi;

            vi = spider.GetInfo( "http://video.sina.com.cn/p/sports/o/v/2011-07-31/194561430003.html" );
            Assert.AreEqual( "视频-1500米孙杨破尘封10年世界纪录 超大优势夺金", vi.Title );
            Assert.AreEqual( "http://you.video.sina.com.cn/api/sinawebApi/outplayrefer.php/vid=57758788_6_OE3jHSFsWm7K+l1lHz2stqkM7KQNt6njnynt71+iJApaVQyOZIrfO4kK5S7RBcdF+WQ/s.swf", vi.FlashUrl );
            //Assert.AreEqual( "http://p1.v.iask.com/514/12/57758788_1.jpg", vi.PicUrl );

            Console.WriteLine( vi.PicUrl );
        }

        [Test]
        public void testIfeng() {

            IVideoSpider spider = new WojiluVideoSpider();

            VideoInfo vi;

            vi = spider.GetInfo( "http://v.ifeng.com/history/wenhuashidian/201107/c7316727-4850-435c-9f45-5fe24a7bb130.shtml" );
            Assert.AreEqual( "杜维明：儒家传统核心价值的现代意义", vi.Title );
            Assert.AreEqual( "http://img.ifeng.com/itvimg/2011/07/30/9ea3cac9-9c6e-492f-84a1-5da27ad4a3f1140.jpg", vi.PicUrl );
            Assert.AreEqual( "http://v.ifeng.com/include/exterior.swf?guid=c7316727-4850-435c-9f45-5fe24a7bb130&AutoPlay=false", vi.FlashUrl );

            vi = spider.GetInfo( "http://v.ifeng.com/news/society/201108/40a7e13d-43d0-4592-871e-0ac0f2a375bf.shtml" );
            Assert.AreEqual( "保育员将男童绑旗杆上", vi.Title );
            Assert.AreEqual( "http://img.ifeng.com/itvimg/2011/08/01/0c8f9bf0-68c2-4d92-8c6d-49e69d047d4e140.jpg", vi.PicUrl );
            Assert.AreEqual( "http://v.ifeng.com/include/exterior.swf?guid=40a7e13d-43d0-4592-871e-0ac0f2a375bf&AutoPlay=false", vi.FlashUrl );

            vi = spider.GetInfo( "http://v.ifeng.com/v/shanhun/index.shtml#bb2e6ca3-d31f-4e31-b866-fb1d52bd2dfb" );
            Assert.AreEqual( "大S流产指媒体造假 揭秘女星流产内幕", vi.Title );
            Assert.AreEqual( "http://img.ifeng.com/itvimg//2011/07/31/a52412c4-8e47-49d2-bd57-59e6d5914860.jpg", vi.PicUrl );
            Assert.AreEqual( "http://v.ifeng.com/include/exterior.swf?guid=bb2e6ca3-d31f-4e31-b866-fb1d52bd2dfb&AutoPlay=false", vi.FlashUrl );

            vi = spider.GetInfo( "http://v.ifeng.com/v/halibote/index.shtml#5d9ff98d-3e85-47b2-80a9-6e359b8da962" );
            Assert.AreEqual( "大卫-科波菲尔曝《哈利波特》抄袭自己的人生", vi.Title );
            Assert.AreEqual( "http://img.ifeng.com/itvimg//2011/08/01/29314047-e9eb-4c77-b3b3-62bbe8cb4841.jpg", vi.PicUrl );
            Assert.AreEqual( "http://v.ifeng.com/include/exterior.swf?guid=5d9ff98d-3e85-47b2-80a9-6e359b8da962&AutoPlay=false", vi.FlashUrl );

        }





    }
}
