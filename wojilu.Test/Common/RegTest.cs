using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace wojilu.Test.Common {

    [TestFixture]
    public class RegTest {


        [Test]
        public void testUrl() {

            Assert.IsTrue( RegPattern.IsMatch( "http://www.163.com", RegPattern.Url ) );
            Assert.IsTrue( RegPattern.IsMatch( "http://163.com", RegPattern.Url ) );
            Assert.IsTrue( RegPattern.IsMatch( "http://www.sohu.com", RegPattern.Url ) );
            Assert.IsTrue( RegPattern.IsMatch( "http://sohu.com", RegPattern.Url ) );
            Assert.IsTrue( RegPattern.IsMatch( "http://www.sina.com.cn", RegPattern.Url ) );
            Assert.IsTrue( RegPattern.IsMatch( "http://sina.com.cn", RegPattern.Url ) );


            Assert.IsFalse( RegPattern.IsMatch( "天气预报", RegPattern.Url ) );
            Assert.IsFalse( RegPattern.IsMatch( "http://天气预报", RegPattern.Url ) );
            Assert.IsFalse( RegPattern.IsMatch( "abc", RegPattern.Url ) );
            //Assert.IsFalse( RegPattern.IsMatch( "http://abc", RegPattern.Url ) );


        }

        public void testSoaParam() {

            String param = "param0=3;param1=strA;param2=2234;";
            List<String> arr = splitParams( param );

            Assert.AreEqual( "param0=3", arr[0] );
            Assert.AreEqual( "param1=strA", arr[1] );
            Assert.AreEqual( "param2=2234", arr[2] );

            String param2 = "param0=http://www.aaaa.com/rss.php?rssid=11;param1=5";
            List<String> arr2 = splitParams( param2 );
            Assert.AreEqual( "param0=http://www.aaaa.com/rss.php?rssid=11", arr2[0] );
            Assert.AreEqual( "param1=5", arr2[1] );




        }

        private List<String> splitParams( String param ) {
            String[] arr = param.Split( new string[] { ";param" }, StringSplitOptions.RemoveEmptyEntries );
            List<String> list = new List<string>();
            foreach (String str in arr) {

                if (str.StartsWith( "param" ))
                    list.Add( str.TrimEnd( ';' ) );
                else
                    list.Add( "param" + str.TrimEnd( ';' ) );
            }
            return list;
        }


    }
}
