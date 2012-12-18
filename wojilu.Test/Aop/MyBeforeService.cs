using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Aop;

namespace wojilu.Test.Aop {

    public class MyArgService {

        public virtual int Buy( int num ) {

            Console.WriteLine( "-----------------Buy----------------------" );

            return num * 2;
        }

    }

    public class MyArgServiceObserver : MethodObserver {

        public override void ObserveMethods() {

            observe( "wojilu.Test.Aop.MyArgService", "Buy" );
        }

        public override void Before( System.Reflection.MethodInfo method, object[] args, object target ) {

            Console.WriteLine( "-----------------before Buy----------------------" );

            // 修改参数，将所有 num 加1
            int num = (int)args[0];
            args[0] = num + 1;
        }

    }

}
