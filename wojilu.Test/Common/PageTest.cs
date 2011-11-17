using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace wojilu.Test.Common {

    [TestFixture]
    public class PageTest {





        [Test]
        public void test() {

            int pageSize = 20;

            int count = 3;
            int page = getPage( count, pageSize );
            Assert.AreEqual( 1, page );

            count = 0;
            page = getPage( count, pageSize );
            Assert.AreEqual( 1, page );


            count = 11;
            page = getPage( count, pageSize );
            Assert.AreEqual( 1, page );

            count = 20;
            page = getPage( count, pageSize );
            Assert.AreEqual( 1, page );

            //---------------------

            count = 21;
            page = getPage( count, pageSize );
            Assert.AreEqual( 2, page );


            count = 25;
            page = getPage( count, pageSize );
            Assert.AreEqual( 2, page );

            count = 40;
            page = getPage( count, pageSize );
            Assert.AreEqual( 2, page );

            //---------------------

            count = 41;
            page = getPage( count, pageSize );
            Assert.AreEqual( 3, page );

        }

        private int getPage( int count, int pageSize ) {

            if (count == 0) return 1;

            int mod = count % pageSize;
            if (mod == 0) return count / pageSize;


            return count / pageSize+1;

        }


    }

}
