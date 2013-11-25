using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace wojilu.Test.Data {


    [TestFixture]
    public class MemoryDBTest {



        [Test]
        public void testFindById() {
            Book book = new Book().findById( 5 ) as Book;
            Assert.IsNotNull( book );
            Assert.AreEqual( "zhangsan5", book.Name );

            Book nullBook = new Book().findById( 999999 ) as Book;
            Assert.IsNull( nullBook );
        }

        [Test]
        public void testFindAll() {

            IList list = new Book().findAll();
            Assert.AreEqual( 10, list.Count );
        }

        // 每次FindAll 得到的，都是缓存的副本，
        // 并且 book.Delete() 只是移除缓存中的数据，对当前的副本(集合)并无影响
        [Test]
        public void testDeleteAll() {

            IList all = new Book().findAll();

            foreach (Book book in all) {
                book.delete();
            }

            int result = new Book().findAll().Count;

            Assert.AreEqual( result, 0 );
        }

        // 随机删除
        [Test]
        public void testDelete() {
            Random rd = new Random();

            for (int i = 0; i < 20; i++) {
                int id = rd.Next( 1, 11 );
                Console.WriteLine( "随机ID:" + id );
                Book book = new Book().findById( id ) as Book;
                if (book != null) {
                    book.delete();
                    Console.WriteLine( "删除：" + id );
                }
            }

        }

        [Test]
        public void testDelete3() {

            Book mybook = new Book();
            IList all = mybook.findAll();
            Assert.AreEqual( 10, all.Count );

            int deleteCount = 0;
            int bookCount = all.Count;
            for (int i = 0; i < bookCount; i++) {
                Book book = all[i] as Book;
                book.delete();
                deleteCount++;
            }

            Assert.AreEqual( bookCount, deleteCount );

            IList results = mybook.findAll();
            Assert.AreEqual( 0, results.Count );
        }


        // findBy (index test)
        //----------------------------------------------------------------------------------------------

        [Test]
        public void testFindBy() {

            List<Book> books = cdb.findBy<Book>( "Name", "zhangsan5" );
            Assert.AreEqual( 1, books.Count );
            Book book = books[0];
            Assert.AreEqual( "hao10", book.Weather );

            List<Book> books2 = cdb.findBy<Book>( "Weather", "hao10" );
            Assert.AreEqual( 1, books2.Count );
            Book book2 = books2[0];
            Assert.AreEqual( "zhangsan5", book2.Name );

        }

        [Test]
        public void testFindBy_Insert() {

            Book newBook = new Book {
                Name = "zhangsan5",
                Weather = "hao6"
            };
            newBook.insert();

            IList newBooks = new Book().findBy( "Name", "zhangsan5" );
            Assert.AreEqual( 2, newBooks.Count );
            Assert.AreEqual( "hao6", ((Book)newBooks[1]).Weather );

            IList newBooks2 = new Book().findBy( "Weather", "hao6" );
            Assert.AreEqual( 2, newBooks2.Count );
            Assert.AreEqual( "zhangsan5", ((Book)newBooks2[1]).Name );

        }

        [Test]
        public void testFindBy_Update() {

            IList books = new Book().findBy( "Name", "zhangsan5" );
            Assert.AreEqual( 1, books.Count );
            Book book = books[0] as Book;

            book.Name = "zhangsan8";
            book.update();

            IList newBooks = new Book().findBy( "Name", "zhangsan5" );
            Assert.AreEqual( 0, newBooks.Count );

            IList newBooks2 = new Book().findBy( "Name", "zhangsan8" );
            Assert.AreEqual( 2, newBooks2.Count );
        }

        [Test]
        public void testFindBy_Delete() {

            IList books = new Book().findBy( "Name", "zhangsan5" );
            Assert.AreEqual( 1, books.Count );
            Book book = books[0] as Book;

            book.delete();

            IList newBooks = new Book().findBy( "Name", "zhangsan5" );
            Assert.AreEqual( 0, newBooks.Count );

            int allCount = new Book().findAll().Count;
            Assert.AreEqual( 9, allCount );

        }

        [SetUp]
        public void initData() {
            Console.WriteLine( "------------------------------init data-------------------------------" );
            for (int i = 0; i < 10; i++) {
                Book book = new Book();
                book.Name = "zhangsan" + (i + 1);
                book.Weather = "hao" + ((i + 1) * 2);
                book.insert();
            }
        }

        [TearDown]
        public void deleteAll() {
            IList all = new Book().findAll();

            foreach (Book book in all) {
                book.delete();
            }

        }



    }
}
