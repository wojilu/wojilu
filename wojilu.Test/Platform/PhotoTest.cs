using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Helper;

namespace wojilu.Test.Platform {


    [TestFixture]
    public class PhotoTest {


        [Test]
        public void testSizeInfo() {

            Dictionary<String, PhotoInfo> dic = new Dictionary<String, PhotoInfo>();

            dic.Add( "s", new PhotoInfo { Width = 68, Height = 68 } );
            dic.Add( "sx", new PhotoInfo { Width = 80, Height = 120 } );
            dic.Add( "m", new PhotoInfo { Width = 180, Height = 500 } );

            String str = ObjectContext.Create<PhotoInfoHelper>().ConvertString( dic );
            Assert.AreEqual( "s=68/68,sx=80/120,m=180/500", str );
        }


        [Test]
        public void testRead() {

            // s=68/68,sx=80/120,m=180/500

            String str = "s=68/68,sx=80/120,m=180/500";
            Dictionary<String, PhotoInfo> dic = ObjectContext.Create<PhotoInfoHelper>().GetInfo( str );

            Assert.AreEqual( dic.Count, 3 );

            Assert.AreEqual( 68, dic["s"].Width );
            Assert.AreEqual( 68, dic["s"].Height );

            Assert.AreEqual( 80, dic["sx"].Width );
            Assert.AreEqual( 120, dic["sx"].Height );


            Assert.AreEqual( 180, dic["m"].Width );
            Assert.AreEqual( 500, dic["m"].Height );


        }


    }
}
