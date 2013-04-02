using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Members.Sites.Domain;

namespace wojilu.Test.Data {

    [TestFixture]
    public class CacheObjectTest {






        [Test]
        public void testSimple() {

            TCacheS obj1 = new TCacheS();
            obj1.Name = "n1";
            obj1.Address = "a1";
            obj1.IsDone = false;
            obj1.insert();

            TCacheS obj2 = new TCacheS();
            obj2.Name = "n2";
            obj2.Address = "a2";
            obj2.IsDone = true;
            obj2.insert();

            List<TCacheS> list = cdb.findAll<TCacheS>();
            Assert.AreEqual( 2, list.Count );

            Assert.AreEqual( obj1.Name, list[0].Name );
            Assert.AreEqual( obj1.Address, list[0].Address );
            Assert.AreEqual( obj1.IsDone, list[0].IsDone );

            Assert.AreEqual( obj2.Name, list[1].Name );
            Assert.AreEqual( obj2.Address, list[1].Address );
            Assert.AreEqual( obj2.IsDone, list[1].IsDone );

        }


        [Test]
        public void testType() {

            TCacheType obj = new TCacheType();
            obj.Name = "n1";
            obj.Address = "a1";
            obj.IsDone = false;

            obj.DataLong = 22222222222222222L;
            obj.DataDecimal = 2.92M;
            obj.DataDouble = 22222.2222222299;
            obj.Created = DateTime.Now.AddDays( -2 );

            obj.insert();

            List<TCacheType> list = cdb.findAll<TCacheType>();
            Assert.AreEqual( 1, list.Count );

            Assert.AreEqual( obj.Name, list[0].Name );
            Assert.AreEqual( obj.Address, list[0].Address );
            Assert.AreEqual( obj.IsDone, list[0].IsDone );

            Assert.AreEqual( obj.DataLong, list[0].DataLong );
            Assert.AreEqual( obj.DataDecimal, list[0].DataDecimal );
            Assert.AreEqual( obj.DataDouble, list[0].DataDouble );
            Assert.IsTrue( isTimeEqual( obj.Created, list[0].Created ) );

        }

        private bool isTimeEqual( DateTime x, DateTime y ) {

            return x.Year == y.Year &&
                x.Month == y.Month &&
                x.Day == y.Day &&
                x.Hour == y.Hour;

        }


        [Test]
        public void testArray() {

            TCacheArray obj = new TCacheArray();
            obj.Address = new string[] { "a1", "a3", "a6" };
            obj.IsDone = new bool[] { true, false, false };

            obj.DataLong = new long[] { 22222222222222222L, 3333333333333333333L };
            obj.DataDecimal = new decimal[] { 2.92M, 883.666M };
            obj.DataDouble = new double[] { 22222.2222222299, 38132.5555555555 };
            obj.Created = new DateTime[] { DateTime.Now.AddDays( -2 ), DateTime.Now.AddDays( -5 ) };

            obj.insert();

            List<TCacheArray> list = cdb.findAll<TCacheArray>();
            Assert.AreEqual( 1, list.Count );
            TCacheArray x = list[0];

            for (int i = 0; i < obj.Address.Length; i++) {
                Assert.AreEqual( obj.Address[i], x.Address[i] );
            }

            for (int i = 0; i < obj.IsDone.Length; i++) {
                Assert.AreEqual( obj.IsDone[i], x.IsDone[i] );
            }

            for (int i = 0; i < obj.DataLong.Length; i++) {
                Assert.AreEqual( obj.DataLong[i], x.DataLong[i] );
            }

            for (int i = 0; i < obj.DataDecimal.Length; i++) {
                Assert.AreEqual( obj.DataDecimal[i], x.DataDecimal[i] );
            }

            for (int i = 0; i < obj.DataDouble.Length; i++) {
                Assert.AreEqual( obj.DataDouble[i], x.DataDouble[i] );
            }

            for (int i = 0; i < obj.Created.Length; i++) {
                Assert.IsTrue( isTimeEqual( obj.Created[i], x.Created[i] ) );
            }

        }

        [Test]
        public void testList() {

            TCacheList obj = new TCacheList();
            obj.Address = new List<string> { "a1", "a3", "a6" };
            obj.IsDone = new List<bool> { true, false, false };

            obj.DataLong = new List<long> { 22222222222222222L, 3333333333333333333L };
            obj.DataDecimal = new List<decimal> { 2.92M, 883.666M };
            obj.DataDouble = new List<double> { 22222.2222222299, 38132.5555555555 };
            obj.Created = new List<DateTime> { DateTime.Now.AddDays( -2 ), DateTime.Now.AddDays( -5 ) };

            obj.insert();

            List<TCacheList> list = cdb.findAll<TCacheList>();
            Assert.AreEqual( 1, list.Count );
            TCacheList x = list[0];

            for (int i = 0; i < obj.Address.Count; i++) {
                Assert.AreEqual( obj.Address[i], x.Address[i] );
            }

            for (int i = 0; i < obj.IsDone.Count; i++) {
                Assert.AreEqual( obj.IsDone[i], x.IsDone[i] );
            }

            for (int i = 0; i < obj.DataLong.Count; i++) {
                Assert.AreEqual( obj.DataLong[i], x.DataLong[i] );
            }

            for (int i = 0; i < obj.DataDecimal.Count; i++) {
                Assert.AreEqual( obj.DataDecimal[i], x.DataDecimal[i] );
            }

            for (int i = 0; i < obj.DataDouble.Count; i++) {
                Assert.AreEqual( obj.DataDouble[i], x.DataDouble[i] );
            }

            for (int i = 0; i < obj.Created.Count; i++) {
                Assert.IsTrue( isTimeEqual( obj.Created[i], x.Created[i] ) );
            }

        }


        [Test]
        public void testSubObject() {

            TCacheSub obj = new TCacheSub();
            obj.Address = "a1";
            obj.IsDone = true;

            obj.Sub = new TCacheChild();
            obj.Sub.Id = 33;
            obj.Sub.Name = "s1";
            obj.Sub.Weight = 88;

            obj.insert();


            List<TCacheSub> list = cdb.findAll<TCacheSub>();
            Assert.AreEqual( 1, list.Count );
            TCacheSub x = list[0];

            Assert.AreEqual( obj.Name, x.Name );
            Assert.AreEqual( obj.Address, x.Address );
            Assert.AreEqual( obj.IsDone, x.IsDone );

            Assert.IsNotNull( x.Sub );
            Assert.AreEqual( obj.Sub.Id, x.Sub.Id );
            Assert.AreEqual( obj.Sub.Name, x.Sub.Name );
            Assert.AreEqual( obj.Sub.Weight, x.Sub.Weight );

        }


        [Test]
        public void testCurrency() {

            TCurrency obj = new TCurrency();
            obj.ExchangeRate = 1;
            obj.insert();

            List<TCurrency> list = cdb.findAll<TCurrency>();
            Assert.AreEqual( 1, list.Count );
            TCurrency x = list[0];

            Assert.AreEqual( 1, x.ExchangeRate );

        }

        [Test]
        public void testProperty() {

            TProperty obj1 = new TProperty();
            obj1.Name = "n1";
            obj1.Address = "a1";
            obj1.IsDone = false;

            obj1.IsWrite = 99;
            obj1.Product = "product1";

            obj1.insert();

            TProperty obj2 = new TProperty();
            obj2.Name = "n2";
            obj2.Address = "a2";
            obj2.IsDone = true;

            obj2.IsWrite = 99;
            obj2.Product = "product1";

            obj2.insert();

            List<TProperty> list = cdb.findAll<TProperty>();
            Assert.AreEqual( 2, list.Count );

            Assert.AreEqual( obj1.Name, list[0].Name );
            Assert.AreEqual( obj1.Address, list[0].Address );
            Assert.AreEqual( obj1.IsDone, list[0].IsDone );
            Assert.AreEqual( 66, list[0].IsRead );
            Assert.AreEqual( "product1", list[0].Product );

            Assert.AreEqual( obj2.Name, list[1].Name );
            Assert.AreEqual( obj2.Address, list[1].Address );
            Assert.AreEqual( obj2.IsDone, list[1].IsDone );

            Assert.AreEqual( 66, list[1].IsRead );
            Assert.AreEqual( "product1", list[1].Product );

        }



        [TestFixtureTearDown]
        public void clear() {

            List<TCacheS> list1 = cdb.findAll<TCacheS>();
            foreach (TCacheS x in list1) {
                cdb.delete( x );
            }

            List<TCacheType> list2 = cdb.findAll<TCacheType>();
            foreach (TCacheType x in list2) {
                cdb.delete( x );
            }

            List<TCacheList> list3 = cdb.findAll<TCacheList>();
            foreach (TCacheList x in list3) {
                cdb.delete( x );
            }

            List<TCacheArray> list4 = cdb.findAll<TCacheArray>();
            foreach (TCacheArray x in list4) {
                cdb.delete( x );
            }

            List<TCacheSub> list5 = cdb.findAll<TCacheSub>();
            foreach (TCacheSub x in list5) {
                cdb.delete( x );
            }

            List<TCurrency> list6 = cdb.findAll<TCurrency>();
            foreach (TCurrency x in list6) {
                cdb.delete( x );
            }

            List<TProperty> list7 = cdb.findAll<TProperty>();
            foreach (TProperty x in list7) {
                cdb.delete( x );
            }
        }

    }

}
