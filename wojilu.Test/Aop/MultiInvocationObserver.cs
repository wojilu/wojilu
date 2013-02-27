using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Aop;

namespace wojilu.Test.Aop {

    public class TMultiInvocationTarget {

        public int count = 0;

        public virtual int Save( int id ) {
            Console.WriteLine( "--------save " + id + "--------" );
            return count + id;
        }
    }

    public class MultiInvocation1Observer : MethodObserver {

        public override void ObserveMethods() {
            observe( typeof( TMultiInvocationTarget ), "Save" );
        }

        public override void Before( System.Reflection.MethodInfo method, object[] args, object target ) {
            Console.WriteLine( "before advice 1 running..." );
        }

        public override object Invoke( IMethodInvocation invocation ) {
            Console.WriteLine( "around advice 1 running before..." );
            object ret = base.Invoke( invocation );
            Console.WriteLine( "around advice 1 running after..." );
            return ret;
        }

        public override void After( object returnValue, System.Reflection.MethodInfo method, object[] args, object target ) {
            Console.WriteLine( "after advice 1 running..." );
        }

    }

    public class MultiInvocation2Observer : MethodObserver {

        public override void ObserveMethods() {
            observe( typeof( TMultiInvocationTarget ), "Save" );
        }

        public override void Before( System.Reflection.MethodInfo method, object[] args, object target ) {
            Console.WriteLine( "before advice 2 running..." );
        }

        public override object Invoke( IMethodInvocation invocation ) {
            Console.WriteLine( "around advice 2 running before..." );
            object ret = base.Invoke( invocation );
            Console.WriteLine( "around advice 2 running after..." );
            return ret;
        }

        public override void After( object returnValue, System.Reflection.MethodInfo method, object[] args, object target ) {
            Console.WriteLine( "after advice 2 running..." );
        }
    }

    public class MultiInvocation3Observer : MethodObserver {

        public override void ObserveMethods() {
            observe( typeof( TMultiInvocationTarget ), "Save" );
        }

        public override void Before( System.Reflection.MethodInfo method, object[] args, object target ) {
            Console.WriteLine( "before advice 3 running..." );
        }

        public override object Invoke( IMethodInvocation invocation ) {
            Console.WriteLine( "around advice 3 running before..." );
            object ret = base.Invoke( invocation );
            Console.WriteLine( "around advice 3 running after..." );
            return ret;
        }

        public override void After( object returnValue, System.Reflection.MethodInfo method, object[] args, object target ) {
            Console.WriteLine( "after advice 3 running..." );
        }
    }









}
