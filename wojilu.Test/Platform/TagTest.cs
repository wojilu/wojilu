using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace wojilu.Test.Platform {


    [TestFixture]
    public class TagTest {

        private static String[] splitTags( String tagString ) {
            return wojilu.Common.Tags.TagService.GetTags( tagString );
        }

        [Test]
        public void testSplitTag() {

            String[] arr = null;

            arr = splitTags( "abc" );
            Assert.AreEqual( 1, arr.Length );

            //-------------------------------------------------------

            arr = splitTags( "abc xyz" );
            Assert.AreEqual( 2, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "xyz", arr[1] );

            arr = splitTags( "abc    ccc" );
            Assert.AreEqual( 2, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "ccc", arr[1] );

            arr = splitTags( "  abc    ccc xyz" );
            Assert.AreEqual( 3, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "ccc", arr[1] );
            Assert.AreEqual( "xyz", arr[2] );

            //-------------------------------------------------------

            arr = splitTags( "abc,xyz" );
            Assert.AreEqual( 2, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "xyz", arr[1] );

            arr = splitTags( "abc  ,  ccc" );
            Assert.AreEqual( 2, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "ccc", arr[1] );

            arr = splitTags( "  abc  ,  ccc, xyz" );
            Assert.AreEqual( 3, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "ccc", arr[1] );
            Assert.AreEqual( "xyz", arr[2] );

            //-------------------------------------------------------

            arr = splitTags( "abc，xyz" );
            Assert.AreEqual( 2, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "xyz", arr[1] );

            arr = splitTags( "abc  ，  ccc" );
            Assert.AreEqual( 2, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "ccc", arr[1] );

            arr = splitTags( "  abc  ， ccc，xyz" );
            Assert.AreEqual( 3, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "ccc", arr[1] );
            Assert.AreEqual( "xyz", arr[2] );

            //-------------------------------------------------------

            arr = splitTags( "abc/ xyz" );
            Assert.AreEqual( 2, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "xyz", arr[1] );

            arr = splitTags( "abc /  ccc" );
            Assert.AreEqual( 2, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "ccc", arr[1] );

            arr = splitTags( "  abc / ccc/xyz" );
            Assert.AreEqual( 3, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "ccc", arr[1] );
            Assert.AreEqual( "xyz", arr[2] );

            //-------------------------------------------------------

            arr = splitTags( "  abc ,   ccc      ,  xyz, ppp" );
            Assert.AreEqual( 4, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "ccc", arr[1] );
            Assert.AreEqual( "xyz", arr[2] );
            Assert.AreEqual( "ppp", arr[3] );

            arr = splitTags( "  体育 ,   国际新闻ccc      ,  娱乐, windows7" );
            Assert.AreEqual( 4, arr.Length );
            Assert.AreEqual( "体育", arr[0] );
            Assert.AreEqual( "国际新闻ccc", arr[1] );
            Assert.AreEqual( "娱乐", arr[2] );
            Assert.AreEqual( "windows7", arr[3] );

            arr = splitTags( "体育 国际新闻ccc 娱乐 windows7  " );
            Assert.AreEqual( 4, arr.Length );
            Assert.AreEqual( "体育", arr[0] );
            Assert.AreEqual( "国际新闻ccc", arr[1] );
            Assert.AreEqual( "娱乐", arr[2] );
            Assert.AreEqual( "windows7", arr[3] );

            //-------------------------------------------------------

            arr = splitTags( "abc \"win 8\"" );
            Assert.AreEqual( 2, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "win 8", arr[1] );

            arr = splitTags( "\"win 8\"" );
            Assert.AreEqual( 1, arr.Length );
            Assert.AreEqual( "win 8", arr[0] );

            arr = splitTags( " \"win 8\"" );
            Assert.AreEqual( 1, arr.Length );
            Assert.AreEqual( "win 8", arr[0] );

            arr = splitTags( " \"win 8\" " );
            Assert.AreEqual( 1, arr.Length );
            Assert.AreEqual( "win 8", arr[0] );

            arr = splitTags( "abc \"win 8\" \"win 7\"  " );
            Assert.AreEqual( 3, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "win 8", arr[1] );
            Assert.AreEqual( "win 7", arr[2] );

            arr = splitTags( "abc，\"win 8\"，\"win 7\"  " );
            Assert.AreEqual( 3, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "win 8", arr[1] );
            Assert.AreEqual( "win 7", arr[2] );

            //-------------------------------------------------------

            arr = splitTags( "abc 'win 8'" );
            Assert.AreEqual( 2, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "win 8", arr[1] );

            arr = splitTags( "'win 8'" );
            Assert.AreEqual( 1, arr.Length );
            Assert.AreEqual( "win 8", arr[0] );

            arr = splitTags( " 'win 8'" );
            Assert.AreEqual( 1, arr.Length );
            Assert.AreEqual( "win 8", arr[0] );

            arr = splitTags( " 'win 8' " );
            Assert.AreEqual( 1, arr.Length );
            Assert.AreEqual( "win 8", arr[0] );

            arr = splitTags( "abc 'win 8' 'win 7'  " );
            Assert.AreEqual( 3, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "win 8", arr[1] );
            Assert.AreEqual( "win 7", arr[2] );

            arr = splitTags( "abc，'win 8'，'win 7'  " );
            Assert.AreEqual( 3, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "win 8", arr[1] );
            Assert.AreEqual( "win 7", arr[2] );

            //-------------------------------------------------------

            // 不支持嵌套
            arr = splitTags( " \"'win 8'\" " );
            Assert.AreEqual( 2, arr.Length );
            Assert.AreEqual( "win", arr[0] );
            Assert.AreEqual( "8", arr[1] );

            //-------------------------------------------------------

            arr = splitTags( "abc,win 8, win 7 " );
            Assert.AreEqual( 3, arr.Length );
            Assert.AreEqual( "abc", arr[0] );
            Assert.AreEqual( "win 8", arr[1] );
            Assert.AreEqual( "win 7", arr[2] );

            //-------------------------------------------------------

            arr = splitTags( "" );
            Assert.AreEqual( 0, arr.Length );

            arr = splitTags( "  " );
            Assert.AreEqual( 0, arr.Length );

            arr = splitTags( null );
            Assert.AreEqual( 0, arr.Length );

        }

    }


}
