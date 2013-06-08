using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Drawing;
using NUnit.Framework;

namespace wojilu.Test.Platform {



    [TestFixture]
    public class ConfigTest {

        [Test]
        public void testThumbConfig() {

            String cfgString = "s=width:75|height:75|mode:auto, sx=width:200|height:200|mode:auto, m=width:600|height:600|mode:auto, b=width:1024|height:1024|mode:auto";
            testConfigString( cfgString );

            // 加上多余空格
            cfgString = "   s=width:75|height:75|mode:auto,    sx=width:200|height:200|mode:auto, m=width:600|height:600|mode:auto,   b=width:1024|height:1024|mode:auto    ";
            testConfigString( cfgString );

            // 加上大小写
            cfgString = " s = width:75 | HeiGht:75        | mode:auto,   SX=width:200|height:200|mode:auto, m=width   :  600  |height:600| Mode :  auto,   b  = width:1024  | height:1024| mode:auto ";
            testConfigString( cfgString );

            // 边界测试
            cfgString = "   ";
            Dictionary<String, ThumbInfo> cfgList = ThumbConfig.ReadString( cfgString );
            Assert.AreEqual( 0, cfgList.Count );

            cfgString = null;
            cfgList = ThumbConfig.ReadString( cfgString );
            Assert.AreEqual( 0, cfgList.Count );

            cfgString = " aaaaaaaaaaaaa,   SX=width:200|height:200|mode:auto, m=width   :  600  |height:600| Mode :  cut, o ";
            cfgList = ThumbConfig.ReadString( cfgString );
            Assert.AreEqual( 2, cfgList.Count );
            ThumbInfo x = cfgList["sx"];
            Assert.AreEqual( 200, x.Width );
            Assert.AreEqual( 200, x.Height );
            Assert.AreEqual( SaveThumbnailMode.Auto, x.Mode );

            x = cfgList["m"];
            Assert.AreEqual( 600, x.Width );
            Assert.AreEqual( 600, x.Height );
            Assert.AreEqual( SaveThumbnailMode.Cut, x.Mode );


        }

        private void testConfigString( String cfgString ) {
            Dictionary<String, ThumbInfo> cfgList = ThumbConfig.ReadString( cfgString );

            Assert.AreEqual( 4, cfgList.Count );

            ThumbInfo x = cfgList["s"];
            Assert.AreEqual( 75, x.Width );
            Assert.AreEqual( 75, x.Height );
            Assert.AreEqual( SaveThumbnailMode.Auto, x.Mode );

            x = cfgList["sx"];
            Assert.AreEqual( 200, x.Width );
            Assert.AreEqual( 200, x.Height );
            Assert.AreEqual( SaveThumbnailMode.Auto, x.Mode );

            x = cfgList["m"];
            Assert.AreEqual( 600, x.Width );
            Assert.AreEqual( 600, x.Height );
            Assert.AreEqual( SaveThumbnailMode.Auto, x.Mode );

            x = cfgList["b"];
            Assert.AreEqual( 1024, x.Width );
            Assert.AreEqual( 1024, x.Height );
            Assert.AreEqual( SaveThumbnailMode.Auto, x.Mode );
        }



    }

}
