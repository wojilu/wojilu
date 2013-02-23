using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Aop;
using System.Reflection;

namespace wojilu.Test.Aop {


    public class TObservedTarget {

        public int count = 0;

        public virtual int Save( int id ) {
            Console.WriteLine( "--------save " + id + "--------" );
            return count + id;
        }
    }

    // 监控 TObservedTarget
    public class TObserver1 : MethodObserver {

        public override void ObserveMethods() {
            observe( typeof( TObservedTarget ), "Save" );
        }

        public override void Before( MethodInfo method, object[] args, object target ) {
            args[0] = (int)args[0] + 3;
            Console.WriteLine( "TObserver1: before" );
        }

        public override void After( object returnValue, MethodInfo method, object[] args, object target ) {
            Console.WriteLine( "TObserver1: after" );
        }
    }

    // 监控 TObserver1
    public class TObserver2 : MethodObserver {

        public override void ObserveMethods() {
            observe( typeof( TObserver1 ), "After" );
        }

        public override void Before( MethodInfo method, object[] args, object target ) {
            Console.WriteLine( "TObserver2: before" );
        }

        public override void After( object returnValue, MethodInfo method, object[] args, object target ) {
            Console.WriteLine( "TObserver2: after" );
        }
    }

    // 监控 TObserver2
    public class TObserver3 : MethodObserver {

        public override void ObserveMethods() {
            observe( typeof( TObserver2 ), "After" );
        }

        public override void Before( MethodInfo method, object[] args, object target ) {
            Console.WriteLine( "TObserver3: before" );
        }

        public override void After( object returnValue, MethodInfo method, object[] args, object target ) {
            Console.WriteLine( "TObserver3: after" );
        }
    }

    // 监控 TObserver3
    public class TObserver4 : MethodObserver {

        public override void ObserveMethods() {
            observe( typeof( TObserver3 ), "After" );
        }

        public override void Before( MethodInfo method, object[] args, object target ) {
            Console.WriteLine( "TObserver4: before" );
        }

        public override void After( object returnValue, MethodInfo method, object[] args, object target ) {
            Console.WriteLine( "TObserver4: after" );
        }
    }

}
