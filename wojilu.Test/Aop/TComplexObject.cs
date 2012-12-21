using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wojilu.Test.Aop {

    public class TComplexObject {

        public IMyAopService myAopService { get; set; }

        public TComplexObject() {
            myAopService = new MyAopService();
        }

        public void Save() {
            Console.WriteLine( "run begin..." );
            myAopService.Save();
            Console.WriteLine( "run end..." );
        }

    }

}
