using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;

namespace wojilu.Test.Orm {

    public class TTPost : ObjectBase<TTPost> {

        public String Title { get; set; }
    }


    [TestFixture]
    public class ConcurrentTest {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ConcurrentTest ) );


        [Test]
        public void test() {


            Console.WriteLine( "按任意键开始" );
            Console.ReadLine();


            Console.WriteLine( "主线程开始" );


            for (int i = 0; i < 20; i++) {

                Thread t = new Thread( insertData );
                t.Start();

            }

            Console.WriteLine( "主线程结束" );

            Console.ReadLine();

        }

        public void insertData() {

            TTPost x = new TTPost();
            x.Title = DateTime.Now.ToString();
            x.insert();
            Console.WriteLine( string.Format( "{0}=>{1}", x.Id, x.Title ) );
            
        }

    }

}
