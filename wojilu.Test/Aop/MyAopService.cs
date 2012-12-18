using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wojilu.Test.Aop {

    public class MyCat {
        public int Id { get; set; }
        public String Name { get; set; }
    }

    public class MyAopService {

        public static readonly int UpdateResult = 98;

        public virtual void Save() {
            Console.WriteLine( "--------Save--------" );
        }

        public virtual int Update( int id ) {
            Console.WriteLine( "--------Update--------" );
            return UpdateResult;
        }

        public virtual void GetBy( String name, int id ) {
            Console.WriteLine( "--------GetBy--------" );
        }

        public virtual List<String> GetCat() {
            Console.WriteLine( "--------GetCat--------" );
            return null;
        }

        public virtual List<String> GetDog() {
            Console.WriteLine( "--------GetDog--------" );
            return new List<String>();
        }

        public virtual List<MyCat> GetCat2() {
            Console.WriteLine( "--------GetCat2--------" );

            List<MyCat> cats = new List<MyCat> {
                new MyCat { Id=1, Name = "cat1" },
                new MyCat { Id=1, Name = "cat2" },
                new MyCat { Id=1, Name = "cat3" }
            };

            return cats;
        }

    }

}
