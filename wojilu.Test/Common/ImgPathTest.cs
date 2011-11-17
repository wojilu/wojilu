using System;
using System.Collections;
using System.Text;
using NUnit.Framework;
using wojilu;
using wojilu.Drawing;

namespace wojilu.Test.Common {



    [TestFixture]
    public class ImgPathTest {





        [Test]
        public void testGetOriginalPath() {

            Assert.AreEqual( "face/zhangsan.jpg", img.GetOriginalPath( "face/zhangsan_s.jpg" ) );
            Assert.AreEqual( "img/old/zhangsan.jpg", img.GetOriginalPath( "img/old/zhangsan_s.jpg" ) );
            Assert.AreEqual( "img/2008-8-18/20119171.jpg", img.GetOriginalPath( "img/2008-8-18/20119171_s.jpg" ) );

            Assert.AreEqual( "http://static.abc.net/upload/face/guest.jpg",
                img.GetOriginalPath( "http://static.abc.net/upload/face/guest_s.jpg" ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/old/zhangsan.jpg",
                img.GetOriginalPath( "http://static.abc.net/upload/img/old/zhangsan_s.jpg" ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/2008-8-18/20119171.jpg",
                img.GetOriginalPath( "http://static.abc.net/upload/img/2008-8-18/20119171_s.jpg" ) );

        }

        [Test]
        public void testOneDefault() {

            Assert.AreEqual( "face/zhangsan_s.jpg", img.GetThumbPath( "face/zhangsan.jpg" ) );
            Assert.AreEqual( "img/old/zhangsan_s.jpg", img.GetThumbPath( "img/old/zhangsan.jpg" ) );
            Assert.AreEqual( "img/2008-8-18/20119171_s.jpg", img.GetThumbPath( "img/2008-8-18/20119171.jpg" ) );

            Assert.AreEqual( "http://static.abc.net/upload/face/guest_s.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/face/guest.jpg" ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/old/zhangsan_s.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/old/zhangsan.jpg" ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/2008-8-18/20119171_s.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/2008-8-18/20119171.jpg" ) );
        }

        [Test]
        public void testChangeType() {

            ThumbnailType ttype = ThumbnailType.Medium;

            Assert.AreEqual( "face/zhangsan_m.jpg", img.GetThumbPath( "face/zhangsan_s.jpg", ttype ) );
            Assert.AreEqual( "img/old/zhangsan_m.jpg", img.GetThumbPath( "img/old/zhangsan_s.jpg", ttype ) );
            Assert.AreEqual( "img/2008-8-18/20119171_m.jpg", img.GetThumbPath( "img/2008-8-18/20119171_s.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/face/guest_m.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/face/guest_s.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/old/zhangsan_m.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/old/zhangsan_s.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/2008-8-18/20119171_m.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/2008-8-18/20119171_s.jpg", ttype ) );

        }

        [Test]
        public void testOneSmall() {

            ThumbnailType ttype = ThumbnailType.Small;
            Assert.AreEqual( "face/zhangsan_s.jpg", img.GetThumbPath( "face/zhangsan.jpg", ttype ) );
            Assert.AreEqual( "img/old/zhangsan_s.jpg", img.GetThumbPath( "img/old/zhangsan.jpg", ttype ) );
            Assert.AreEqual( "img/2008-8-18/20119171_s.jpg", img.GetThumbPath( "img/2008-8-18/20119171.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/face/guest_s.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/face/guest.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/old/zhangsan_s.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/old/zhangsan.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/2008-8-18/20119171_s.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/2008-8-18/20119171.jpg", ttype ) );

            //---------------------------------------------

            Assert.AreEqual( "face/zhangsan_s.jpg", img.GetThumbPath( "face/zhangsan_s.jpg", ttype ) );
            Assert.AreEqual( "img/old/zhangsan_s.jpg", img.GetThumbPath( "img/old/zhangsan_s.jpg", ttype ) );
            Assert.AreEqual( "img/2008-8-18/20119171_s.jpg", img.GetThumbPath( "img/2008-8-18/20119171_s.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/face/guest_s.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/face/guest_s.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/old/zhangsan_s.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/old/zhangsan_s.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/2008-8-18/20119171_s.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/2008-8-18/20119171_s.jpg", ttype ) );

        }

        [Test]
        public void testOneMedium() {

            ThumbnailType ttype = ThumbnailType.Medium;

            Assert.AreEqual( "face/zhangsan_m.jpg", img.GetThumbPath( "face/zhangsan.jpg", ttype ) );
            Assert.AreEqual( "img/old/zhangsan_m.jpg", img.GetThumbPath( "img/old/zhangsan.jpg", ttype ) );
            Assert.AreEqual( "img/2008-8-18/20119171_m.jpg", img.GetThumbPath( "img/2008-8-18/20119171.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/face/guest_m.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/face/guest.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/old/zhangsan_m.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/old/zhangsan.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/2008-8-18/20119171_m.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/2008-8-18/20119171.jpg", ttype ) );

            //---------------------------------------------

            Assert.AreEqual( "face/zhangsan_m.jpg", img.GetThumbPath( "face/zhangsan_m.jpg", ttype ) );
            Assert.AreEqual( "img/old/zhangsan_m.jpg", img.GetThumbPath( "img/old/zhangsan_m.jpg", ttype ) );
            Assert.AreEqual( "img/2008-8-18/20119171_m.jpg", img.GetThumbPath( "img/2008-8-18/20119171_m.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/face/guest_m.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/face/guest_m.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/old/zhangsan_m.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/old/zhangsan_m.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/2008-8-18/20119171_m.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/2008-8-18/20119171_m.jpg", ttype ) );

        }

        [Test]
        public void testOneBig() {

            ThumbnailType ttype = ThumbnailType.Big;
            Assert.AreEqual( "face/zhangsan_b.jpg", img.GetThumbPath( "face/zhangsan.jpg", ttype ) );
            Assert.AreEqual( "img/old/zhangsan_b.jpg", img.GetThumbPath( "img/old/zhangsan.jpg", ttype ) );
            Assert.AreEqual( "img/2008-8-18/20119171_b.jpg", img.GetThumbPath( "img/2008-8-18/20119171.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/face/guest_b.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/face/guest.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/old/zhangsan_b.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/old/zhangsan.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/2008-8-18/20119171_b.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/2008-8-18/20119171.jpg", ttype ) );

            //---------------------------------------------

            Assert.AreEqual( "face/zhangsan_b.jpg", img.GetThumbPath( "face/zhangsan_b.jpg", ttype ) );
            Assert.AreEqual( "img/old/zhangsan_b.jpg", img.GetThumbPath( "img/old/zhangsan_b.jpg", ttype ) );
            Assert.AreEqual( "img/2008-8-18/20119171_b.jpg", img.GetThumbPath( "img/2008-8-18/20119171_b.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/face/guest_b.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/face/guest_b.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/old/zhangsan_b.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/old/zhangsan_b.jpg", ttype ) );

            Assert.AreEqual( "http://static.abc.net/upload/img/2008-8-18/20119171_b.jpg",
                img.GetThumbPath( "http://static.abc.net/upload/img/2008-8-18/20119171_b.jpg", ttype ) );

        }

    }

    public class img : Img {
    }

}
