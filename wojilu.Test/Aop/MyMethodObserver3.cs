using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Aop;

namespace wojilu.Test.Aop {

    public class MyMethodObserver3 : MethodObserver {

        public override void ObserveMethods() {
            observe( typeof( MyAopService ), "GetCat2" );
        }

        public override object Invoke( wojilu.Aop.IMethodInvocation invocation ) {

            List<MyCat> cats = invocation.Proceed() as List<MyCat>;

            cats.Add( new MyCat { Id=999, Name="cat999" } );
            Console.WriteLine( "add cat999" );

            return cats;


        }
    }

}
