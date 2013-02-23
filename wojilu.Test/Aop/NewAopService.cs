using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Aop;

namespace wojilu.Test.Aop {

    // 多个重载方法
    public class NewAopService {

        public int count = 0;

        public virtual int Save() {
            Console.WriteLine( "--------Save--------" );
            return count;
        }

        public virtual int Save( int id ) {
            Console.WriteLine( "--------save " + id + "--------" );
            return count + id;
        }

        public virtual String Save( int id, String name ) {
            Console.WriteLine( "--------save " + id + ", name " + name + "--------" );
            return (count + id) + name;
        }

    }

    public class NewServiceObserver : MethodObserver {

        public override void ObserveMethods() {

            // 监控多个重载方法
            observe( typeof( NewAopService ), "Save" );
            observe( typeof( NewAopService ), "Save", new Type[] { typeof( int ) } );
            observe( typeof( NewAopService ), "Save", new Type[] { typeof( int ), typeof( string ) } );

        }

        public override void Before( System.Reflection.MethodInfo method, object[] args, object target ) {

            Console.WriteLine( "before=>" + target.GetType().FullName + ", method=" + method.Name );

            // 修改参数
            if (args.Length > 0) {
                int num = (int)args[0];
                args[0] = num / 2;
            }

        }

    }

}
