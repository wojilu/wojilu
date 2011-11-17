using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Data;
using NUnit.Framework;

namespace wojilu.Test.Orm {

    [TestFixture]
    public class ConnectionStringTest {











        [Test]
        public void getDatabase() {

            String str = "server=localhost\\SQL2005;uid=abctest;pwd=***;database=dbnametest;";

            String dbname = new SQLServerDialect().GetConnectionItem( str, ConnectionItemType.Database );

            Assert.AreEqual( dbname, "dbnametest" );


        }

    }

}
