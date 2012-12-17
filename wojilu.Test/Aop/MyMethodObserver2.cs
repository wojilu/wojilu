using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using wojilu.Aop;

namespace wojilu.Test.Aop {


    public class MyMethodObserver2 : MethodObserver {

        public override void ObserveMethods() {

            observe( typeof( MyAopService ), "GetBy" );

        }

        public override void Before( MethodInfo method, Object[] args, Object target ) {
            Console.Write( "test before...type=" + this.GetType().Name + "...arg=" );
            String str = "";
            foreach (Object x in args) {
                str += x + ",";
            }
            Console.WriteLine( str );
        }

        public override void After( Object returnValue, MethodInfo method, Object[] args, Object target ) {
            Console.WriteLine( "test after..." );
        }
    }

}
