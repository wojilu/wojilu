using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using wojilu.Aop;

namespace wojilu.Test.Aop {


    // 监控器是单例存在，下面的变量 version 在多次调用的时候，值会递增
    public class MyMethodObserver1 : MethodObserver {

        private int runCount = 1;

        public override void ObserveMethods() {

            observe( typeof( MyAopService ), "Save" );
            observe( typeof( MyAopService ), "Update" );
            observe( typeof( MyAopService ), "GetBy" );

        }

        public override void Before( MethodInfo method, Object[] args, Object target ) {
            Console.Write( runCount + "test before...type=" + this.GetType().Name + "...arg=" );
            String str = "";
            foreach (Object x in args) {
                str += x + ",";
            }
            Console.WriteLine( str );
            runCount++;
        }

        public override void After( Object returnValue, MethodInfo method, Object[] args, Object target ) {
            Console.WriteLine( runCount + "test after..." );
            runCount++;
        }

        public override Object Invoke( MethodInfo method, Object[] args, Object target ) {

            Object ressult;

            Console.WriteLine( "test invoke before..." );

            // 调用基类的方法运行，而不是代理类。因为本方法就是代理类传递过来的
            ressult = base.Invoke( method, args, target );

            Console.WriteLine( "test invoke after..." );

            return ressult;
        }

    }
}
