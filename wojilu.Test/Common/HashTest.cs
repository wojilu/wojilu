using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace wojilu.Test.Common {


    [TestFixture]
    public class HashTest {

        [Test]
        public void getSalt() {
            string pwd = "aaa";

            HashTool hashTool = new HashTool();

            Console.WriteLine( "myMd5=" + hashTool.Get( pwd, HashType.MD5_16 ) );
            Console.WriteLine( "myMd5=" + hashTool.Get( pwd, HashType.MD5 ) );
            Console.WriteLine( "mySHA1=" + hashTool.Get( pwd, HashType.SHA1 ) );
            Console.WriteLine( "mySHA2=" + hashTool.Get( pwd, HashType.SHA384 ) );
            Console.WriteLine( "mySHA5=" + hashTool.Get( pwd, HashType.SHA512 ) );

            Console.WriteLine( "GetRandomPassword=" + hashTool.GetRandomPassword( 10 ) );
            Console.WriteLine( "GetRandomPassword=" + hashTool.GetRandomPassword( 10, false ) );

            for (int i = 1; i < 11; i++) {
                Console.WriteLine( "GetSalt=" + hashTool.GetSalt( i ) );
            }
        }

        public void sha2() {
            string pwd = "1234";

            HashTool hashTool = new HashTool();

            Console.WriteLine( hashTool.Get( pwd, HashType.SHA384 ) );
            // 9198EAB4
            Console.WriteLine( hashTool.Get( pwd + "9198EAB4", HashType.SHA384 ) );
            //Console.WriteLine( HashTool.GetSalt( 4 ) );


            Console.WriteLine(  );

            Console.WriteLine( hashTool.GetBySalt( pwd, "9198EAB4", HashType.SHA384 ) );

        }

        [Test]
        public void md5() {
            string pwd = "1234";

            HashTool hashTool = new HashTool();

            Console.WriteLine( hashTool.Get( pwd, HashType.MD5_16 ) );
            Console.WriteLine( hashTool.Get( pwd, HashType.MD5 ) );
        }


    }
}
