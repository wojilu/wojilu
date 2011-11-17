using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace wojilu.Test.Web.Mvc {

    [TestFixture]
    public class aActionTest {

        public void myAction() {
            Console.WriteLine( "myAction" );
        }

        public void myActionWithId( int id ) {
            Console.WriteLine( "myActionWithId+"+id );
        }

        [Test]
        public void test() {

            aAction a = myAction;
            aActionWithId b = myActionWithId;

            object action = a;
            object actionB = b;

            testAction( action, 9 );
            testAction( actionB, 19 );



        }

        private void testAction( object obj, int id ) {

            aAction action = obj as aAction;
            if (action != null) {
                action();
                return;
            }

            aActionWithId actionb = obj as aActionWithId;
            if (actionb != null) actionb(id);


        }

    }

}
