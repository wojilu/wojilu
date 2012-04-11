using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using wojilu.weibo.Common;
using wojilu.weibo.Data.Sina;
using System.Diagnostics;

namespace wojilu.weibo.Test
{
    [TestClass]
    public class UnitTest1
    {
        public static string AppKey = "1424217769";

        public static string AppSecret = "4d0ac2511e53743db50224957d698072";

        public static string Token = "2.00mIyVoB2Gs4YB1f435e23798bUHBD";

        private SinaWeibo weibo;

        public UnitTest1()
        {
            weibo = new SinaWeibo(AppKey, AppSecret);
            weibo.SetToken(Token);
        }

        [TestMethod]
        public void TestWeiboLogin()
        {
        }

        [TestMethod]
        public void TestTimeLine()
        {
           var list = weibo.GetPublicStatuses();
           Assert.AreNotEqual(0,list.Items.Count);
        }
    }
}
