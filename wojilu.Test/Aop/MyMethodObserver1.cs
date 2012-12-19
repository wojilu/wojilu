using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using wojilu.Aop;

namespace wojilu.Test.Aop {


    // 监控器生命周期：当方法开始，监控器被创建；当方法结束，监控器也结束。
    // 可以在 Before / Invoke / After 之间共享数据。下面的 runCount 在 after 之后为3。
    public class MyMethodObserver1 : MethodObserver {

        private int runCount = 1;

        public override void ObserveMethods() {

            // 可以重复添加，但相同的监控器，最后只保留一份
            observe( typeof( MyAopService ), "Save" );
            observe( typeof( MyAopService ), "Update" );
            observe( typeof( MyAopService ), "GetBy" );
            observe( typeof( MyAopService ), "GetCat" );
            observe( typeof( MyAopService ), "GetDog" );

            // 因为上面被监控的类型相同，方法不同，所以可以合并为下面的方式。
            observe( typeof( MyAopService ), "Save/Update/GetBy/GetCat/GetDog" );

            // 但单独添加的好处是，可以添加第三个参数，用于区别名称相同、但参数不同的方法(重载/override)
            // TODO

            // 如果添加不存在的方法，框架会抛出异常
            //observe( typeof( MyAopService ), "NoMethod" );

        }

        // 1）先运行 "前置 advice"
        public override void Before( MethodInfo method, Object[] args, Object target ) {
            Console.Write( runCount + "test before...type=" + this.GetType().Name + "...arg=" );
            String str = "";
            foreach (Object x in args) {
                str += x + ",";
            }
            Console.WriteLine( str );
            runCount++;
        }

        // 2）然后运行 "环绕 advice"
        public override Object Invoke( IMethodInvocation invocation ) {

            Object result = null;

            Console.WriteLine( runCount + "test invoke before..." );

            // 运行原始方法
            result = invocation.Proceed();

            Console.WriteLine( runCount + "test invoke after..." );
            runCount++;

            return result;
        }

        // 3）最后运行 "后置 advice"
        public override void After( Object returnValue, MethodInfo method, Object[] args, Object target ) {
            Console.WriteLine( runCount + "test after...return value=" + returnValue );
        }

    }
}
