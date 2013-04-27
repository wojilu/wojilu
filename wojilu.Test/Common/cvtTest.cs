using System;
using System.Collections;
using System.Text;
using NUnit.Framework;
using wojilu;

namespace wojilu.Test.Common {

    [TestFixture]
    public class cvtTest {












        [Test]
        public void testIsDecimal() {

            Assert.IsTrue( cvt.IsDecimal( "12" ) );
            Assert.IsTrue( cvt.IsDecimal( "12.3" ) );
            Assert.IsTrue( cvt.IsDecimal( "-12.899" ) );
            Assert.IsTrue( cvt.IsDecimal( "0" ) );
            Assert.IsTrue( cvt.IsDecimal( "0.322" ) );

            Assert.IsFalse( cvt.IsDecimal( null ) );
            Assert.IsFalse( cvt.IsDecimal( "" ) );
            Assert.IsFalse( cvt.IsDecimal( "abc" ) );


        }

        [Test]
        public void testIsBool() {

            Assert.IsTrue( cvt.IsBool( "true" ) );
            Assert.IsTrue( cvt.IsBool( "TRUE" ) );
            Assert.IsTrue( cvt.IsBool( "TrUe" ) );
            Assert.IsTrue( cvt.IsBool( "false" ) );
            Assert.IsTrue( cvt.IsBool( "FALSE" ) );
            Assert.IsTrue( cvt.IsBool( "fAlSe" ) );

            Assert.IsFalse( cvt.IsBool( null ) );
            Assert.IsFalse( cvt.IsBool( "22" ) );
            Assert.IsFalse( cvt.IsBool( "abc" ) );
            Assert.IsFalse( cvt.IsBool( "" ) );
            Assert.IsFalse( cvt.IsBool( " " ) );

        }


        [Test]
        public void testTime() {

            DateTime t  = cvt.ToTime( "2009-3-3 16:33" );
            Console.WriteLine( cvt.ToTimeString(t) );

             t = cvt.ToTime( "2009-5-18 16:33" );
             Console.WriteLine( cvt.ToTimeString( t ) );

             t = cvt.ToTime( "2009-5-19 16:33" );
             Console.WriteLine( cvt.ToTimeString( t ) );

             t = cvt.ToTime( "2009-5-20 6:33" );
             Console.WriteLine( cvt.ToTimeString( t ) );

             t = cvt.ToTime( "2009-5-20 7:33" );
             Console.WriteLine( cvt.ToTimeString( t ) );

             t = cvt.ToTime( "2009-5-20 8:33" );
             Console.WriteLine( cvt.ToTimeString( t ) );


             t = cvt.ToTime( "2009-5-20 9:00" );
             Console.WriteLine( cvt.ToTimeString( t ) );

        }

        [Test]
        public void testIsInt() {

            Assert.IsTrue( cvt.IsInt("235") );
            Assert.IsTrue( cvt.IsInt( "0" ) );
            Assert.IsTrue( cvt.IsInt( "-235" ) );
            Assert.IsFalse( cvt.IsInt( "fdd" ) );

            Assert.IsFalse( cvt.IsInt( "2354656982" ) );
            Assert.IsTrue( cvt.IsInt( "354656982" ) );
            Assert.IsTrue( cvt.IsInt( "-1354656982" ) );



        }

        [Test]
        public void testToString() {

            int[] arr = new int[5];
            arr[0] = 3;
            arr[1] = -355;
            arr[2] = 13;
            arr[3] = 0;
            arr[4] = 999;

            string str = cvt.ToString( arr );
            Assert.AreEqual( "3,-355,13,0,999", str );

            int[] arr2 = null;
            Assert.AreEqual( "", cvt.ToString( arr2 ) );

            int[] arr3 = new int[] { };
            Assert.AreEqual( "", cvt.ToString( arr3 ) );

        }

        [Test]
        public void testToIntArray() {

            string idString = "  3,8,0,343,     98,-34 ";
            int[] ids = cvt.ToIntArray( idString );

            Assert.AreEqual( 6, ids.Length );

            Assert.AreEqual( 3, ids[0] );
            Assert.AreEqual( 8, ids[1] );
            Assert.AreEqual( 0, ids[2] );
            Assert.AreEqual( 343, ids[3] );
            Assert.AreEqual( 98, ids[4] );
            Assert.AreEqual( -34, ids[5] );

            idString = "   ";
            ids = cvt.ToIntArray( idString );
            Assert.AreEqual( 0, ids.Length );

        }


        [Test]
        public void testTimeSubstract() {

            DateTime x1 = cvt.ToTime( "2013/1/16 15:22:11" );
            DateTime x2 = cvt.ToTime( "2013/1/16 15:22:19" );
            Assert.AreEqual( 8, x2.Subtract( x1 ).Seconds );

            x1 = cvt.ToTime( "2013/1/16 15:22:11" );
            x2 = cvt.ToTime( "2013/1/16 15:23:19" );
            Assert.AreEqual( 8, x2.Subtract( x1 ).Seconds );
            Assert.AreEqual( 68, x2.Subtract( x1 ).TotalSeconds );
            Assert.AreEqual( 1, x2.Subtract( x1 ).Minutes );

        }

    }
}
