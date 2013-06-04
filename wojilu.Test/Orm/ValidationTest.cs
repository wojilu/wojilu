using System;
using wojilu.Test.Orm.Entities;
using wojilu.Test.Orm.Utils;
using NUnit.Framework;
using System.Data.OleDb;
using wojilu.Data;
using System.Data;
using System.Collections.Generic;

namespace wojilu.Test.Orm {






    [TestFixture]
    public class ValidationTest {

        // 可以直接右键此处开始测试！（不用考虑数据库初始化）







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
        public void NotNull_Insert() {
            TValidateData d = new TValidateData();
            Result result = db.insert( d );

            Assert.IsFalse( result.IsValid );
            Console.WriteLine( result.ErrorsText );


            //自定义错误信息
            TValidateData2 d2 = new TValidateData2();
            Result result2 = db.insert( d2 );

            Assert.IsFalse( result2.IsValid );
            Assert.AreEqual( "请填写内容", result2.ErrorsText.Trim() );
            Console.WriteLine( result2.ErrorsText );
        }

        [Test]
        public void NotNull_Update() {
            TValidateData d = new TValidateData();
            d.Body = "body";
            Result result = db.insert( d );

            Assert.IsTrue( result.IsValid );

            d.Body = "";
            Result updateResult = db.update( d );
            Assert.IsFalse( updateResult.IsValid );
            Console.WriteLine( updateResult.ErrorsText );


            //自定义错误信息
            TValidateData2 d2 = new TValidateData2();
            d2.Body = "body";
            Result result2 = db.insert( d2 );
            Assert.IsTrue( result2.IsValid );
            d2.Body = "";
            Result updateResult2 = db.update( d2 );


            Assert.IsFalse( updateResult2.IsValid );
            Assert.AreEqual( "请填写内容", updateResult2.ErrorsText.Trim() );
            Console.WriteLine( updateResult2.ErrorsText );
        }

        [Test]
        public void RegFormat_Insert() {
            TValidateData3 d3 = new TValidateData3();
            d3.Email = "fdafkfeii";
            Result result3 = db.insert( d3 );

            Assert.IsFalse( result3.IsValid );
            Console.WriteLine( result3.ErrorsText );

            //自定义错误信息
            TValidateData4 d4 = new TValidateData4();
            d4.Email = "fdafkfeii";
            Result result4 = db.insert( d4 );

            Assert.IsFalse( result4.IsValid );
            Assert.AreEqual( "请正确填写电子邮件", result4.ErrorsText.Trim() );
            Console.WriteLine( result4.ErrorsText );
        }

        [Test]
        public void RegFormat_Update() {
            TValidateData3 d3 = new TValidateData3();
            d3.Email = "ss@ee.com";
            Result result3 = db.insert( d3 );
            Assert.IsTrue( result3.IsValid );

            d3.Email = "faofeif";
            Result resultUpdate = db.update( d3 );

            Assert.IsFalse( resultUpdate.IsValid );
            Console.WriteLine( resultUpdate.ErrorsText );

            //自定义错误信息
            TValidateData4 d4 = new TValidateData4();
            d4.Email = "ss@ee.com";
            Result result4 = db.insert( d4 );
            Assert.IsTrue( result4.IsValid );

            d4.Email = "fafoefi";
            Result resultUpdateCustom = db.update( d4 );

            Assert.IsFalse( resultUpdateCustom.IsValid );
            Assert.AreEqual( "请正确填写电子邮件", resultUpdateCustom.ErrorsText.Trim() );
            Console.WriteLine( resultUpdateCustom.ErrorsText );
        }

        [Test]
        public void Unique_Insert() {

            //正常添加
            TValidateData5 d = new TValidateData5();
            d.Name = "zhangsan";
            Result result = db.insert( d );
            Assert.IsTrue( result.IsValid );
            Assert.Greater( d.Id, 0 );

            //重复添加
            TValidateData5 d5 = new TValidateData5();
            d5.Name = "zhangsan";
            Result result5 = db.insert( d5 );

            Assert.IsFalse( result5.IsValid );
            Assert.AreEqual( 0, d5.Id );
            Console.WriteLine( result5.ErrorsText );

            //--------------自定义错误信息--------------

            //正常添加
            TValidateData6 d2 = new TValidateData6();
            d2.Name = "zhangsan";
            Result result2 = db.insert( d2 );
            Assert.IsTrue( result2.IsValid );
            Assert.Greater( d2.Id, 0 );

            //重复添加
            TValidateData6 d6 = new TValidateData6();
            d6.Name = "zhangsan";
            Result result6 = db.insert( d6 );

            Assert.IsFalse( result6.IsValid );
            Assert.AreEqual( 0, d6.Id );
            Assert.AreEqual( "用户名重复", result6.ErrorsText.Trim() );
            Console.WriteLine( result6.ErrorsText );
        }

        [Test]
        public void Unique_Update() {

            //添加
            TValidateData5 d = new TValidateData5();
            d.Name = "lisiwang";
            Result result = db.insert( d );
            Assert.IsTrue( result.IsValid );
            Assert.Greater( d.Id, 0 );

            //再次添加
            TValidateData5 dnext = new TValidateData5();
            dnext.Name = "wanger";
            Result resultNext = db.insert( dnext );
            Assert.IsTrue( resultNext.IsValid );
            Assert.Greater( dnext.Id, 0 );

            //修改第二次添加的数据，和第一次的相同
            dnext.Name = d.Name;
            Result resultUpdate = db.update( dnext );
            Assert.IsFalse( resultUpdate.IsValid );
            Console.WriteLine( resultUpdate.ErrorsText );

            //--------------自定义错误信息--------------

            //正常添加
            TValidateData6 d2 = new TValidateData6();
            d2.Name = "liswang";
            Result result2 = db.insert( d2 );
            Assert.IsTrue( result2.IsValid );
            Assert.Greater( d2.Id, 0 );

            //再次添加
            TValidateData6 datanext = new TValidateData6();
            datanext.Name = "wanger";
            Result insertResultNext = db.insert( datanext );
            Assert.IsTrue( insertResultNext.IsValid );
            Assert.Greater( datanext.Id, 0 );

            //修改第二次添加的数据，和第一次的相同
            datanext.Name = d2.Name;
            Result updateResultNext = db.update( datanext );
            Assert.IsFalse( updateResultNext.IsValid );
            Assert.AreEqual( "用户名重复", updateResultNext.ErrorsText.Trim() );
            Console.WriteLine( updateResultNext.ErrorsText );

        }

    }
}
