using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Aop;

namespace wojilu.Test.Aop {




    [TestFixture]
    public class AopTest {

        [Test]
        public void testCode() {



            MyAopService a = AopContext.CreateProxy<MyAopService>();
            a.Save();
            Console.WriteLine();
            Console.WriteLine();

            a.Update( 88 );
            Console.WriteLine();
            Console.WriteLine();

            a.GetBy( "myname", 3 );




        }

    }
}
