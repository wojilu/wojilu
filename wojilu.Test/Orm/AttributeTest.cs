using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

using NUnit.Framework;

using wojilu.ORM;
using wojilu.Test.Orm.Attr;
using wojilu.Test.Orm.Utils;

namespace wojilu.Test.Orm {





    [TestFixture]
    public class AttributeTest {











        [TestFixtureSetUp]
        public void InitData() {
            wojiluOrmTestInit.ClearLog();
            wojiluOrmTestInit.InitMetaData();
        }

        [TestFixtureTearDown]
        public void clear() {
            wojiluOrmTestInit.ClearTables();
        }


        [Test]
        public void NoPropertyAttribute() {

            TAttNoSaveAll objNoSave = new TAttNoSaveAll();
            Assert.AreEqual( 2, Entity.GetInfo( objNoSave ).SavedPropertyList.Count ); // object has a default property "Id"

            TAttWithSaveAll objSaved = new TAttWithSaveAll();
            Assert.AreEqual( 4, Entity.GetInfo( objSaved ).SavedPropertyList.Count );

            string nameValue = "zhangsan";

            objNoSave.Name = nameValue;
            db.insert( objNoSave );

            objSaved.Name = nameValue;
            db.insert( objSaved );

            TAttNoSaveAll objZhang = TAttNoSaveAll.findById( 1 );
            Assert.IsNotNull( objZhang );
            Assert.IsNull( objZhang.Name );

            TAttWithSaveAll objZhangsan = TAttWithSaveAll.findById( 1 );
            Assert.AreEqual( nameValue, objZhangsan.Name );

        }





    }

}
