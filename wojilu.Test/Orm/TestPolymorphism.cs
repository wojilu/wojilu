using System;
using System.Collections;
using System.Collections.Generic;
using wojilu.Test.Orm.Entities;
using wojilu.Test.Orm.Utils;
using NUnit.Framework;

namespace wojilu.Test.Orm {

    // 多态关联和多态检索测试
    [TestFixture]
    public class TestPolymorphism {


        // 可以直接右键此处开始测试！（不用考虑数据库初始化）





        [Test]
        public void testAbstractFind() {

            // 抽象基类是 TAbCategory

            List<TAbNewCategory> list = db.findAll<TAbNewCategory>();
            Assert.AreEqual( 9, list.Count );

            Assert.AreEqual( "标题啊_1", list[0].Title );

            List<TAbNewCategory2> list2 = db.findAll<TAbNewCategory2>();
            Assert.AreEqual( 9, list2.Count );

            Assert.AreEqual( "标题啊2_1", list2[0].Title2 );


            TAbNewCategory c = db.findById<TAbNewCategory>( 2 );
            Assert.IsNotNull( c );
            Assert.AreEqual( 4, c.Hits );

            List<TAbNewCategory> qlist = db.find<TAbNewCategory>( "Hits=6" ).list();
            Assert.AreEqual( 1, qlist.Count );
        }



        [Test]
        public void FindById() {
            ConsoleTitleUtil.ShowTestTitle( "FindById" );

            // 基类TDataRoot，子类TPost和TTopic

            //子类简单查询
            TPost post = TPost.findById( 3 ) as TPost;
            Assert.IsNotNull( post );
            Assert.AreEqual( 3, post.Id );
            Console.WriteLine( "Id:{0}  Title:{1}", post.Id, post.Title );

            TPost post2 = TPost.findById( 11 ) as TPost;
            Assert.IsNotNull( post2 );


            //多态查询：正确判断结果是相应子类
            TDataRoot root = TDataRoot.findById( 6 );
            Assert.IsNotNull( root );
            Assert.AreEqual( 6, root.Id );
            Assert.AreEqual( typeof( TTopic ), root.GetType() );

            //针对属性的多态查询
            Assert.AreEqual( typeof( TTopicCategory ), root.Category.GetType() );
        }

        [Test]
        public void FindById_FromRoot() {

            // 1、分类的多态
            //---------------------------------------

            Console.WriteLine( "---------- 分类多态 -----------" );

            // 也可以直接创建父类
            TCategory category = new TCategory();
            category.Name = "父类的分类之FindById_FromRoot";
            db.insert( category );

            Assert.Greater( category.Id, 0 );

            // 多态查询：正确判断具体的结果是父类
            TCategory cat = TCategory.findById( category.Id );
            Assert.AreEqual( category.Name, cat.Name );
            Assert.AreEqual( typeof( TCategory ), cat.GetType() );

            Console.WriteLine( "---------- 多态关联 -----------" );


            // 2、 数据上面 关联到 多态的分类
            //---------------------------------------
            TDataRoot root = new TDataRoot();
            root.Title = "我是父类之一";
            root.Body = "父类的内容之一";
            root.Category = category;
            db.insert( root );

            Assert.Greater( root.Id, 0 );
            TDataRoot data = TDataRoot.findById( root.Id );
            Assert.IsNotNull( data );
            Assert.AreEqual( root.Title, data.Title );
            Assert.AreEqual( category.Id, data.Category.Id );
            Assert.AreEqual( category.Name, data.Category.Name );
        }

        public void FindAll() {
            ConsoleTitleUtil.ShowTestTitle( "FindAll" );

            Console.WriteLine( "__tpost__" );

            // 子类直接查询：这个最简单，没有额外考虑
            //IList list = TPost.findAll();
            IList list = db.findAll<TPost>();

            Assert.AreEqual( list.Count, 20 );
            foreach (TPost post in list) {
                Assert.AreEqual( typeof( TPostCategory ), post.Category.GetType() ); //检索实体属性的时候，使用了FindBy方法
                Console.WriteLine( "Id:{0}  Title:{1}", post.Id, post.Title );
            }

            Console.WriteLine( "__ttopic__" );

            //IList topicList = TTopic.findAll();
            IList topicList = db.findAll<TTopic>();

            Assert.AreEqual( topicList.Count, 23 );
            foreach (TTopic post in topicList) {
                Assert.AreEqual( typeof( TTopicCategory ), post.Category.GetType() );
                Console.WriteLine( "Id:{0}  Title:{1}", post.Id, post.Title );
            }

            Console.WriteLine( "__findAll__" );

            //多态查询：将子类查询结果合并，并且根据多态查询所属分类
            IList list2 = TDataRoot.findAll();
            Assert.AreEqual( 46, list2.Count );

            int rootDataCount = 0;

            foreach (TDataRoot root in list2) {
                if (root.GetType() == typeof( TPost )) {
                    Assert.AreEqual( typeof( TPostCategory ), root.Category.GetType() );
                }
                else if (root.GetType() == typeof( TTopic )) {
                    Assert.AreEqual( typeof( TTopicCategory ), root.Category.GetType() );
                }
                else if (root.GetType() == typeof( TDataRoot )) {
                    rootDataCount += 1;
                    Assert.AreEqual( typeof( TCategory ), root.Category.GetType() );
                }

                Console.WriteLine( "id:{1} [type]{0} [categoryType]:{3} title:{2}", Entity.GetInfo( root ).FullName, root.Id, root.Title, root.Category.GetType() );
            }
            Assert.AreEqual( 3, rootDataCount );

        }

        [Test]
        public void FindBy() {
            ConsoleTitleUtil.ShowTestTitle( "Find" );

            IList list = TDataRoot.find( "Category.Name=:cname" ).set( "cname", "post帖子分类" ).select( "Id,Title,Body,Category.Name" ).list();
            Assert.AreEqual( 23, list.Count );
            Console.WriteLine( "共有结果：" + list.Count );

            int rootDataCount = 0;
            foreach (TDataRoot root in list) {
                Assert.AreEqual( "post帖子分类", root.Category.Name );

                // 因为是多态，结果有三种类型

                if (root.GetType() == typeof( TPost )) {
                    Assert.AreEqual( typeof( TPostCategory ), root.Category.GetType() ); //每种类型的属性：“分类”也是多态查询得到的，各不相同，包括相应的父类或子类
                }
                else if (root.GetType() == typeof( TTopic )) {
                    Assert.AreEqual( typeof( TTopicCategory ), root.Category.GetType() );
                }
                else if (root.GetType() == typeof( TDataRoot )) {
                    rootDataCount += 1;
                    Assert.AreEqual( typeof( TCategory ), root.Category.GetType() );
                }


                Console.WriteLine( "id:{1} type:{0} title:{2}", Entity.GetInfo( root ).FullName, root.Id, root.Title );
            }
            Assert.AreEqual( 3, rootDataCount );
        }


        public static IList FindPage() {
            return null; //TODO
        }


        // 创建表的时候，一个父类、两个子类都要创建表；
        // 1、子类建表的时候，将ID的自增属性去掉，仅仅Int即可
        [Test]
        public void A_Insert() {


            wojiluOrmTestInit.ClearLog();
            wojiluOrmTestInit.InitMetaData();

            ConsoleTitleUtil.ShowTestTitle( "Insert" );

            //wojilu.file.Delete( "log.txt" );

            // 此处应该先向基类添加数据
            // 然后向子类添加数据（同时调整insert sql，插入Id的值）

            // 总共添加46条数据
            for (int i = 0; i < 20; i++) {

                // 在插入数据的时候，多态关联没有特别需要注意的

                TPostCategory pcat = new TPostCategory();
                pcat.Name = "post帖子分类";
                pcat.Hits = new Random().Next( 1, 100 );
                db.insert( pcat );
                Assert.Greater( pcat.Id, 0 );

                TTopicCategory tcat = new TTopicCategory();
                tcat.Name = "topic主题分类";
                tcat.ReplyCount = new Random().Next( 1, 200 );
                db.insert( tcat );
                Assert.Greater( tcat.Id, 0 );

                TPost post = new TPost();
                post.Title = "post_34名美国议员联名致函";
                post.Body = "希腊深化的过去的发恩持大扽肯炉衬扽拉歘称";
                post.Uid = "张三";
                post.Category = pcat; // 多态关联添加
                post.Hits = new Random().Next();
                db.insert( post );
                Assert.Greater( post.Id, 0 );

                TTopic topic = new TTopic();
                topic.Title = "topic_我是主题帖子";
                topic.Body = "标题似乎很奇怪，秘密嘛，自然是不能说的。于是乎“不能说的秘密”便成了一个病句。";
                topic.Uid = "李四";
                topic.Category = tcat;
                topic.Hits = new Random().Next( 34, 10039343 );
                topic.ReplyCount = 3;
                db.insert( topic );
                Assert.Greater( topic.Id, 0 );


            }

            for (int i = 0; i < 3; i++) {

                // 添加主题帖子，让其分类名称和帖子分类名称相同，便于下面测试的时候看是否也在多态查询结果中
                TTopicCategory tcatfake = new TTopicCategory();
                tcatfake.Name = "zzTopic帖子分类";
                tcatfake.ReplyCount = new Random().Next( 1, 200 );
                db.insert( tcatfake );
                Assert.Greater( tcatfake.Id, 0 );

                TTopic topicfake = new TTopic();
                topicfake.Title = "zzTopic我是主题帖子";
                topicfake.Body = "标题似乎很奇怪，秘密嘛，自然是不能说的。于是乎“不能说的秘密”便成了一个病句。";
                topicfake.Uid = "李四";
                topicfake.Category = tcatfake;
                topicfake.Hits = new Random().Next( 34, 10039343 );
                topicfake.ReplyCount = 3;
                db.insert( topicfake );
                Assert.Greater( topicfake.Id, 0 );

                // 直接添加父类的具体数据
                TCategory category = new TCategory();
                category.Name = "post帖子分类";
                db.insert( category );

                TDataRoot root = new TDataRoot();
                root.Title = "zzParent我是父类之init初始化";
                root.Body = "父类的内容之init初始化";
                root.Category = category;
                db.insert( root );
            }

            insertAbstractTest();

        }


        private void insertAbstractTest() {

            for (int i = 1; i < 10; i++) {
                TAbNewCategory abCategory = new TAbNewCategory();
                abCategory.Name = "我是继承的名称_" + i;
                abCategory.Title = "标题啊_" + i;
                abCategory.Hits = 2 + i;
                db.insert( abCategory );
                Assert.AreEqual( abCategory.Id, i );

                TAbNewCategory2 abCategory2 = new TAbNewCategory2();
                abCategory2.Name = "我是继承的名称2_" + i;
                abCategory2.Title2 = "标题啊2_" + i;
                db.insert( abCategory2 );
                Assert.AreEqual( abCategory2.Id, i );
            }
        }

        [Test]
        public void X_UpdateAndDelete() {

            Update();
            Delete();

        }


        public void Delete() {
            ConsoleTitleUtil.ShowTestTitle( "Delete" );

            //子类删除：删除子类的同时，也要删除父类中的数据
            TPost post = TPost.findById( 11 ) as TPost;
            Assert.IsNotNull( post );
            db.delete( post );

            post = TPost.findById( 11 ) as TPost;
            Assert.IsNull( post );

            //父类删除：同时删除子类
            TDataRoot.delete( 13 );
            Assert.IsNull( TDataRoot.findById( 13 ) );
        }

        public void Update() {
            ConsoleTitleUtil.ShowTestTitle( "Update" );

            //子类更新：父类相应属性也要更新
            TPost post = TPost.findById( 3 ) as TPost;
            post.Title = "**更新之后的子类";
            db.update( post );

            string sql = "select title from Tdataroot where id=3";
            string title = wojilu.Data.EasyDB.ExecuteScalar( sql, wojilu.Data.DbContext.getConnection(post.GetType()) ) as string;

            Assert.IsNotNull( title );
            Assert.AreEqual( post.Title, title );

            //父类更新：其实是真实的底层子类更新，同上
        }


        [TestFixtureSetUp]
        public void Init() {
            wojiluOrmTestInit.ClearLog();
            wojiluOrmTestInit.InitMetaData();

        }

        [TestFixtureTearDown]
        public void clear() {


            wojiluOrmTestInit.ClearTables();

        }


    }
}
