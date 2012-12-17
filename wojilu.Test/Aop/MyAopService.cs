using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wojilu.Test.Aop {

    public class MyAopService {

        public virtual void Save() {
            Console.WriteLine( "--------save--------" );
        }

        public virtual int Update( int id ) {
            Console.WriteLine( "--------update--------" );
            return 0;
        }

        public virtual void GetBy( String name, int id ) {
            Console.WriteLine( "--------get--------" );
        }

    }

}
