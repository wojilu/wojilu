using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace wojilu.Test.Common {


    [TestFixture]
    public class ReflectionTest {




        [Test]
        public void testIsInterface() {

            Type objType1 = typeof( ObjTemp1 );
            Type objType2 = typeof( ObjTemp2 );

            Type interfaceType = typeof( IObjTemp );


            Assert.IsTrue( rft.IsInterface( objType1, interfaceType ) );
            Assert.IsFalse( rft.IsInterface( objType2, interfaceType ) );
        }

        [Test]
        public void testMethodInInterface() {

            Type objType = typeof( ObjTemp1 );
            Type interfaceType = typeof( IObjTemp );

            Assert.IsTrue( rft.IsMethodInInterface( objType.GetMethod( "Save" ), objType, interfaceType ) );
            Assert.IsTrue( rft.IsMethodInInterface( objType.GetMethod( "Update" ), objType, interfaceType ) );
            Assert.IsTrue( rft.IsMethodInInterface( objType.GetMethod( "GetById" ), objType, interfaceType ) );
            Assert.IsFalse( rft.IsMethodInInterface( objType.GetMethod( "Buy" ), objType, interfaceType ) );


        }

    }


    //----------------------------------------------------------------------------

    public interface IObjTemp {
        void Save();
        int Update( String Name );
        String GetById( int id );
    }

    public class ObjTemp1 : IObjTemp {

        public void Save() { }
        public int Update( String Name ) { return 0; }
        public String GetById( int id ) { return ""; }

        public void Buy() {
        }

    }

    public class ObjTemp2 {
    }


}
