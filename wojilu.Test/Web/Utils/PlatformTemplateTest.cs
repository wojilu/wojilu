using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Serialization;
using wojilu.Web;

namespace wojilu.Test.Common {

    [TestFixture]
    public class PlatformTemplateTest {









        [Test]
        public void testGetVars() {
            testVars1();
            testVars2();
        }

        private void testVars1() {
            string template = "{*actor*} 发表了日志 {*blogpost*}";
            List<string> vars = PlatformTemplate.GetVars( template );
            Assert.AreEqual( 2, vars.Count );
            Assert.AreEqual( "actor", vars[0] );
            Assert.AreEqual( "blogpost", vars[1] );
        }

        private void testVars2() {
            string template = "你的好友 {*actor*} 发表了照片 {*photo*} 在昨天";
            List<string> vars = PlatformTemplate.GetVars( template );
            Assert.AreEqual( 2, vars.Count );
            Assert.AreEqual( "actor", vars[0] );
            Assert.AreEqual( "photo", vars[1] );
        }

        [Test]
        public void testGetVarData() {

            string tpl = "{*actor*} 发表了日志 {*blogpost*}";
            string data = "{actor:\"zhangsan\", blogpost:\"<a href='post/2.aspx'>我的第一篇日志</a>\", content:\"日志内容\"}";
            string varData = PlatformTemplate.GetVarData( tpl, data );
            Assert.AreEqual( "{actor:\"zhangsan\", blogpost:\"<a href='post/2.aspx'>我的第一篇日志</a>\"}", varData );


            string tpl2 = "{*actor*} 上传了{*photoCount*}张 新照片。";
            string data2 = "{photoCount:3, photos:\"<a href='post/2.aspx'><img src=/></a>\", content:\"发布于。。。\"}";
            string varData2 = PlatformTemplate.GetVarData( tpl2, data2 );
            Assert.AreEqual( "{photoCount:\"3\"}", varData2 );

        }

    
    }

}
