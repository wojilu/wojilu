using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.ORM;
using NUnit.Framework;

namespace wojilu.Test.Orm {

    [TestFixture]
    public class ClassCountTest {

        [Test]
        public static void loadTest() {


            int count = MappingClass.Instance.ClassList.Count;
            Console.WriteLine( "初始化成功！类书目："+count );

        }

    }


}
