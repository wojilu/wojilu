using System;
using System.Collections;
using System.Collections.Generic;
using wojilu.Test.Orm.Entities;
using wojilu.Data;
using wojilu.Web;
using wojilu.Test.Orm.Utils;
using NUnit.Framework;

namespace wojilu.Test.Orm {

    [TestFixture]
    public class CRUDGeneric {



        // 注意：必须 EnableContextCache=false才能全部测试成功
        // 可以直接右键此处开始测试！（不用考虑数据库初始化）
        // (高级特性：继承部分测试见TestPolymorphism)












        [TestFixtureSetUp]
        public void InitData() {

            wojiluOrmTestInit.ClearLog();
            wojiluOrmTestInit.InitMetaData();

            InsertMember();
            InsertCat();
            InsertBoard();
            InsertArticle();
            InsertTNews();
        }

        [TestFixtureTearDown]
        public void clear() {
            wojiluOrmTestInit.ClearTables();
            String dbName = "default";
            ConnectionString cnstr = DbConfig.Instance.GetConnectionStringMap()[dbName];
            Console.WriteLine( "当前运行数据库类型：" + cnstr.DbType );
            Console.WriteLine("当前运行数据库ID类型：" + DbConfig.Instance.IdType);
            Console.WriteLine( "当前运行数据库类型：" + cnstr.StringContent );
        }

        [Test]
        public void testDecimal() {

            TMoney m = new TMoney();
            m.MyMoney = 36.83m;
            m.DecimalNum = 777.1234m;

            m.DubleNum = 988.123456;
            m.SingleNum = 999.12f;
            m.insert();

            TMoney mb = new TMoney();
            mb.MyMoney = 99.1268m; // money类型可以最多存储4位数
            mb.DecimalNum = 777.12345m; // TMoney批注里自定义了5位小数

            mb.DubleNum = 988.1234567899;
            mb.SingleNum = 999.128f; // mysql float默认3位
            mb.insert();

            List<TMoney> list = TMoney.findAll();

            Assert.AreEqual( 2, list.Count );

            TMoney m1 = list[0];
            Assert.AreEqual( m1.MyMoney, m.MyMoney );
            Assert.AreEqual( m1.DecimalNum, m.DecimalNum );
            Assert.AreEqual( m1.DubleNum, m.DubleNum );
            Assert.AreEqual( m1.SingleNum, m.SingleNum );

            TMoney m2 = list[1];
            Assert.AreEqual( m2.MyMoney, mb.MyMoney );
            Assert.AreEqual( m2.DecimalNum, mb.DecimalNum );
            Assert.AreEqual( m2.DubleNum, mb.DubleNum );
            Assert.AreEqual( m2.SingleNum, mb.SingleNum );

            foreach (TMoney tm in list) {
                Console.WriteLine( tm.MyMoney + "\t" + tm.SingleNum + "\t" + tm.DubleNum + "\t" + tm.DecimalNum );
            }
        }


        [Test]
        public void multiDatabase() {

            TOtherDB obj = new TOtherDB { Name = "sunwen", Age = 18 };
            obj.insert();

            TOtherDB objNew = TOtherDB.find( "" ).first();
            Assert.IsNotNull( objNew );
            Assert.AreEqual( "sunwen", objNew.Name );

        }

        [Test]
        public void FindPropertyNull() {

            string artIds = "";
            string catIds = "";
            ArrayList list = new ArrayList();
            IList arts = new ArrayList();
            for (int i = 0; i < 5; i++) {
                TCat cat = new TCat();
                cat.Name = "tempCat1";

                db.insert( cat );

                list.Add( cat );
                catIds += cat.Id + ",";

                TArticle art = new TArticle();
                art.Title = "这是什么地方的测试" + i;
                art.Cat = cat;

                db.insert( art );

                arts.Add( art );
                artIds += art.Id + ",";

                db.delete( cat );

            }
            catIds = catIds.TrimEnd( ',' );
            artIds = artIds.TrimEnd( ',' );

            IList<TArticle> results = db.find<TArticle>( "Id in (" + artIds + ")" ).list();
            foreach (TArticle art in results) {
                Assert.IsNull( art.Cat );
                Console.WriteLine( art.Title );
                //Console.WriteLine( "Id:" + art.Cat.Id + " catNull:" + art.Cat.IsNull() + " nameNull:" + strUtil.IsNullOrEmpty( art.Cat.Name ) );
            }


        }


        //------------------------- FindCondition -------------------------

        [Test]
        public void FindCondition_Simply() {
            ConsoleTitleUtil.ShowTestTitle( "FindCondition_Simply" );

            List<TCat> cats = db.find<TCat>( "Id>:myid" ).set( "myid", 5 ).select( "Id,Name,ArticleCount" ).list();
            Assert.AreEqual( 7, cats.Count );

            foreach (TCat cat in cats) {
                Assert.Greater( cat.Id, 0 );
                Assert.IsNotNull( cat.Name );
                Console.WriteLine( cat.Id );
            }


        }

        [Test]
        public void FindCondition_Params() {

            ConsoleTitleUtil.ShowTestTitle( "FindCondition_Params" );

            List<TCat> cats = db.find<TCat>( "Id in (:id1,:id2,:id3)" )
                .set( "id1", 6 )
                .set( "id2", 7 )
                //.set( "id8", 8 )
                .set("id3", 8)
                .list();


            Assert.AreEqual( 3, cats.Count );

            foreach (TCat cat in cats) {
                Assert.Greater( cat.Id, 0 );
                Assert.IsNotNull( cat.Name );
                Console.WriteLine( cat.Id );
            }
        }



        [Test]
        public void FindCondition_LikeParams() {

            ConsoleTitleUtil.ShowTestTitle( "FindCondition_LikeParams" );


            List<TCat> cats = db.find<TCat>( "Name like '%'+:t+'%'" ).set( "t", "新闻" ).list();

            Assert.AreEqual( 2, cats.Count );

            Assert.AreEqual( "国际新闻", cats[0].Name );
            Assert.AreEqual( "国内新闻", cats[1].Name );

        }

        [Test]
        public void FindCondition_Simply2() {
            ConsoleTitleUtil.ShowTestTitle( "FindCondition_Simply2" );


            IList<TCat> cats = db.find<TCat>( "Id>" + 5 ).select( "Id,Name,ArticleCount" ).list();

            Assert.AreEqual( 7, cats.Count );

            foreach (TCat cat in cats) {
                Assert.Greater( cat.Id, 0 );
                Assert.IsNotNull( cat.Name );
            }

        }

        [Test]
        public void FindCondition_Simply3() {
            ConsoleTitleUtil.ShowTestTitle( "FindCondition_Simply3" );

            TCat mycat = db.find<TCat>( "Id=" + 3 ).first();
            Assert.IsNotNull( mycat );
            Assert.AreEqual( "娱乐电影", mycat.Name );
        }

        [Test]
        public void FindCondition_Simply4() {
            ConsoleTitleUtil.ShowTestTitle( "FindCondition_Simply4" );

            IList<TArticle> articles = db.find<TArticle>( "OrderId=" + 9 ).list();
            Assert.AreEqual( 1, articles.Count );

            TArticle art = articles[0];
            Assert.AreEqual( "嵇康", art.Author );

            IList<TCat> cats = TArticle.find( "OrderId=" + 9 ).listChildren<TCat>( "Cat" );
            Assert.AreEqual( 1, cats.Count );
            Assert.AreEqual( "音像世界", cats[0].Name );

        }

        [Test]
        public void FindCondition_OrderBy() {
            ConsoleTitleUtil.ShowTestTitle( "FindCondition_OrderBy" );


            IList<TCat> results = db.find<TCat>( "order by Id desc" ).list();
            Assert.AreEqual( 12, results.Count );
            foreach (TCat cat in results) {
                Assert.Greater( cat.Id, 0 );
                Assert.IsNotNull( cat.Name );
                Console.WriteLine( "TCat {0} - {1}", cat.Id, cat.Name );
            }


        }


        [Test]
        public void FindCondition_WithEqual() {

            TArticle art = new TArticle();
            art.Member = db.findById<TMember>( 8 );
            art.Cat = db.findById<TCat>( 7 );
            art.Board = db.findById<TBoard>( 3 );
            art.Author = "163羽化";
            art.Title = "myurl=www.163.com";
            art.CreateTime = DateTime.Now;
            art.ChannelId = 4;
            art.IsDelete = 0;
            art.OrderId = 15;

            db.insert( art );

            IList<TArticle> articles = db.find<TArticle>( "Title=:t" ).set( "t", art.Title ).list();
            Assert.AreEqual( 1, articles.Count );

            //logger.Info( "===========================================================================" );
            IList<TArticle> articles2 = db.find<TArticle>( "Title='" + art.Title + "'" ).list();
            Assert.AreEqual( 1, articles2.Count );


        }

        [Test]
        public void FindCondition_PropertyId() {

            IList<TArticle> articles = db.find<TArticle>( "Member.Id=7" )
                .select( "Id,Title,Member.Id,Cat.Name,Cat.ArticleCount,Board.Name" )
                .list();

            Assert.AreEqual( 1, articles.Count );
            TArticle article = articles[0];
            Assert.IsNotNull( article );
            Assert.IsNotNull( article.Title );
        }

        [Test]
        public void FindCondition_NoJoinTable() {
            ConsoleTitleUtil.ShowTestTitle( "FindCondition_NoJoinTable" );

            // 不联表查询
            IList<TArticle> articles = db.find<TArticle>( "Member.Id=:mid" ).set( "mid", 7 ).select( "Id,Title,Member.Id,Cat.Name,Cat.ArticleCount,Board.Name" ).list();
            //art.Select( "Id,Title,Member.Id,Cat.Name,Cat.ArticleCount,Board.Name" );
            //IList articles = art.Find( "Member.Id=:mid" ).Set( "mid",7 ).List();

            Assert.AreEqual( 1, articles.Count );
            TArticle article = articles[0];
            Assert.IsNotNull( article );
            Assert.IsNotNull( article.Title );

            //sql缓存测试
            IList<TArticle> articles2 = db.find<TArticle>( "Member.Id=:mid" ).set( "mid", 7 ).select( "Id,Title,Member.Id,Cat.Name,Cat.ArticleCount,Board.Name" ).list();
            //art2.Select( "Id,Title,Member.Id,Cat.Name,Cat.ArticleCount,Board.Name" );
            //IList articles2 = art2.Find( "Member.Id=:mid" ).Set( "mid",7 ).List();

            Assert.AreEqual( 1, articles2.Count );
            TArticle article2 = articles2[0];
            Assert.IsNotNull( article2 );
            Assert.IsNotNull( article2.Title );

        }

        [Test]
        public void FindCondition_JoinTable() {
            ConsoleTitleUtil.ShowTestTitle( "FindCondition_JoinTable" );

            // 需要联表查询
            IList<TArticle> articles = db.find<TArticle>( "Member.Id=:mid and Cat.Name=:catname order by Id desc, Member.Id asc" ).set( "mid", 7 ).set( "catname", "音像世界" ).select( "Id,Title,Member.Id,Cat.Name,Cat.ArticleCount,Board.Name" ).list();

            Assert.AreEqual( 1, articles.Count );
            TArticle article = articles[0];
            Assert.IsNotNull( article );
            Assert.IsNotNull( article.Title );
            Assert.IsNotNull( article.Cat.Name );
            Assert.IsNotNull( article.Board.Name );

        }

        [Test]
        public void FindCondition_IncludeAll() {
            ConsoleTitleUtil.ShowTestTitle( "FindCondition_IncludeAll" );

            IList<TArticle> articles = db.find<TArticle>( "" ).list();

            Assert.AreEqual( 4, articles.Count );
            foreach (TArticle myart in articles) {
                Assert.IsNotNull( myart.Title );
                Assert.IsNotNull( myart.Cat.Name );
                Assert.IsNotNull( myart.Board.Name );
            }
        }

        // 直接使用 Include——本方法废弃
        //[Test]
        //public void FindCondition_IncludeItem() {
        //    TestUtil.ShowTestTitle("FindCondition_IncludeAll");

        //    TArticle art = new TArticle();
        //    art.Include( "Cat" );
        //    IList articles = art.FindAll();

        //    Assert.AreEqual( 4, articles.Count );
        //    foreach( TArticle myart in articles ) {
        //        Assert.IsNotNull( myart.Title );
        //        Assert.IsNotNull( myart.Cat.Name );
        //        Assert.IsNull( myart.Board.Name );
        //    }
        //}



        //------------------------- FindAll -------------------------

        [Test]
        public void FindAll_Simply() {
            ConsoleTitleUtil.ShowTestTitle( "FindAll_Simply" );

            IList<TCat> results = db.findAll<TCat>();
            Assert.AreEqual( 12, results.Count );
            foreach (TCat cat in results) {
                Assert.Greater( cat.Id, 0 );
                Assert.IsNotNull( cat.Name );
                Console.WriteLine( "TCat {0} - {1}", cat.Id, cat.Name );
            }

        }

        [Test]
        public void FindAll_IncludeAll() {
            ConsoleTitleUtil.ShowTestTitle( "FindAll_Include" );

            //art.Include( "Cat,Board" );
            //art.IncludeAll();
            IList<TArticle> arts = db.findAll<TArticle>();
            Assert.AreEqual( 4, arts.Count );
            foreach (TArticle a in arts) {

                Assert.Greater( a.Id, 0 );

                Assert.IsNotNull( a.Title );
                Assert.IsNotNull( a.Member.Name );
                Assert.IsNotNull( a.Cat.Name );
                Assert.IsNotNull( a.Board.Name );

                Console.WriteLine( a.Title + "\t" + a.Member.Name + "\t" + a.Cat.Name + "\t" + a.Board.Name );
            }
        }

        //// 直接使用 Select——本方法废弃
        ////[Test]
        ////public void FindAll_Select() {
        ////    TestUtil.ShowTestTitle("FindAll_Select");

        ////    TArticle art = new TArticle();
        ////    art.Select( "Id,Title,Member.Name,Board.Name" );
        ////    IList arts = art.FindAll();
        ////    Assert.AreEqual( 4, arts.Count );
        ////    foreach( TArticle a in arts ) {

        ////        Assert.Greater( a.Id, 0 );

        ////        Assert.IsNotNull( a.Title );
        ////        Assert.IsNotNull( a.Member.Name );
        ////        Assert.IsNotNull( a.Board.Name );

        ////        Assert.IsNull( a.Cat.Name );

        ////        Console.WriteLine( a.Title + "\t" + a.Member.Name + "\t" + a.Cat.Name + "\t" + a.Board.Name );
        ////    }
        ////}

        //------------------------- FindById -------------------------

        [Test]
        public void FindById_Simply() {
            ConsoleTitleUtil.ShowTestTitle( "FindById_Simply" );

            //简易find
            TCat cat = db.findById<TCat>( 3 );

            Console.WriteLine( "cat name is : " + cat.Name );
            Assert.AreEqual( "娱乐电影", cat.Name );

            TCat nullCat = db.findById<TCat>( 99999999 );
            Assert.IsNull( nullCat );
        }

        [Test]
        public void FindById_ContextCache() {
            ConsoleTitleUtil.ShowTestTitle( "FindById_ContextCache" );

            //复杂关联find
            TArticle art = db.findById<TArticle>( 1 );
            Console.WriteLine( "article 1 title is : " + art.Title );
            Assert.AreEqual( "狂人日记", art.Title );

            //一级缓存
            TMember member = db.findById<TMember>( 13 );
            Console.WriteLine( "member 13 name is : " + member.Name );
            Assert.AreEqual( "袁世凯", member.Name );

            TCat cat = db.findById<TCat>( 8 );
            Console.WriteLine( "cat 8 name is " + cat.Name );
            Assert.AreEqual( "武侠小说", cat.Name );

            TBoard board = db.findById<TBoard>( 9 );
            Console.WriteLine( "board 9 name is : " + board.Name );
            Assert.AreEqual( "board_互联网络", board.Name );

        }

        [Test]
        public void Count() {

            int count = db.count<TCat>();
            Assert.AreEqual( 12, count );

            count = db.count<TCat>( "Id=3" );
            Assert.AreEqual( 1, count );

            count = db.count<TArticle>( "Member.Id=7" );
            Assert.AreEqual( 1, count );

            count = db.count<TArticle>( "Author='周做人'  " );
            Assert.AreEqual( 1, count );

            // 上面直接使用Count的速度比较快，因为直接运行 select count(*) from TCat 语句

            // 下面的count速度慢，先 select Id from Syy_Article where ( MemberId = 7 )  order by Id desc ，然后计算数量。
            // 但这第二种方法也有优点，就是可以使用参数化查询

            count = db.find<TArticle>( "Member.Id=:mid" ).set( "mid", 7 ).count();
            Assert.AreEqual( 1, count );
        }


        [Test]
        public void findPage() {

            int pageSize = 5;

            //------------- 第1页 ---------------
            int current = 1;
            CurrentRequest.setCurrentPage( current );

            DataPage<TBoard> list = TBoard.findPage( "Id>0", pageSize );

            Assert.AreEqual( current, list.Current );
            Assert.AreEqual( 12, list.RecordCount );
            Assert.AreEqual( 3, list.PageCount );
            Assert.AreEqual( pageSize, list.Size );
            Assert.AreEqual( pageSize, list.Results.Count );

            //------------- 第2页 ---------------
            current = 2;
            CurrentRequest.setCurrentPage( current );

            list = TBoard.findPage( "Id>0", pageSize );

            Assert.AreEqual( current, list.Current );
            Assert.AreEqual( 12, list.RecordCount );
            Assert.AreEqual( 3, list.PageCount );
            Assert.AreEqual( pageSize, list.Size );
            Assert.AreEqual( pageSize, list.Results.Count );


            //------------- 第3页 ---------------

            current = 3;
            CurrentRequest.setCurrentPage( current );

            list = TBoard.findPage( "Id>0", pageSize );

            Assert.AreEqual( current, list.Current );
            Assert.AreEqual( 12, list.RecordCount );
            Assert.AreEqual( 3, list.PageCount );
            Assert.AreEqual( pageSize, list.Size );
            Assert.AreEqual( 2, list.Results.Count );
        }

        [Test]
        public void find_ObjectRelation() {

            List<TNews> list = TNews.findAll();
            Assert.AreEqual( list.Count, 10 );

            Assert.AreEqual( list[0].Author.Name, "屈原" );
            Assert.AreEqual( list[3].Author.Name, "陶渊明" );
            Assert.AreEqual( list[6].Author.Name, "顾炎武" );
            Assert.AreEqual( list[9].Author.Name, "余秋雨" );

            Assert.AreEqual( list[0].Author.Category.Name, "作家" );
            Assert.AreEqual( list[3].Author.Category.Name, "作家" );
            Assert.AreEqual( list[6].Author.Category.Name, "学者" );
            Assert.AreEqual( list[9].Author.Category.Name, "明星" );

            /* 生成的 sql 语句——
[findAll]
wojilu.ORM.Operation.FindAllOperation - [TNews_FindAll]select * from TNews 
wojilu.ORM.Query - [FindBy]select * from TAuthor where (Id in (1,2,3,4)) order by Id desc 
wojilu.ORM.Query - [FindBy]select * from TAuthorCategory where (Id in (3,2,1)) order by Id desc 
             * 
[find("").list()]
wojilu.ORM.Query - [FindBy]select * from TNews 
wojilu.ORM.Query - [FindBy]select * from TAuthor where (Id in (1,2,3,4)) order by Id desc 
wojilu.ORM.Query - [FindBy]select * from TAuthorCategory where (Id in (3,2,1)) order by Id desc 
             * 
            */


        }

        //------------------------- Update -------------------------

        [Test]
        public void UpdateACat() {
            TCat cat = db.findById<TCat>( 2 );
            Assert.AreEqual( "国际新闻", cat.Name );
            cat.Name = "这里不是新闻";
            db.update( cat );

            TCat newcat = db.findById<TCat>( cat.Id );
            Assert.AreEqual( "这里不是新闻", newcat.Name );
        }


        [Test]
        public void UpdateBatch() {


            ConsoleTitleUtil.ShowTestTitle( "UpdateBatch" );

            String author = "zhangsan007";

            TArticle.updateBatch( "set Author='" + author + "'", "Id>2" );

            List<TArticle> list = TArticle.findAll();
            foreach (TArticle art in list) {
                if (art.Id <= 2) continue;

                Assert.AreEqual( author, art.Author );
                Console.WriteLine( "updateBatch=" + art.Id );
            }

        }

        [Test]
        public void ZDelete() {

            ConsoleTitleUtil.ShowTestTitle( "Delete" );


            int id = 2;
            TBoard bd = TBoard.findById( id );
            Assert.IsNotNull( bd );
            Assert.AreEqual( id, bd.Id );
            db.delete( bd );

            TBoard myboard = TBoard.findById( id );
            Assert.IsNull( myboard );

        }

        [Test]
        public void ZDeleteBatch() {


            ConsoleTitleUtil.ShowTestTitle( "DeleteBatch" );

            int count = TBoard.count();
            int intVal = 6;

            String whereStr = "Id>=" + intVal;
            int wcount = TBoard.count( whereStr );

            TBoard.deleteBatch( whereStr );

            int bdCount = TBoard.count();

            Assert.AreEqual( (count - wcount), bdCount );

            TBoard bd = TBoard.findById( intVal + intVal );
            Assert.IsNull( bd );

            bd = TBoard.findById( intVal + intVal );
            Assert.IsNull( bd );


        }


        //------------------------- Insert -------------------------

        public void InsertArticle() {

            ConsoleTitleUtil.ShowTestTitle( "InsertArticle" );

            TArticle art = new TArticle();
            art.Member = TMember.findById( 13 );
            art.Cat = TCat.findById( 8 );
            art.Board = TBoard.findById( 9 );
            art.Author = "54鲁迅";
            art.Title = "狂人日记";
            art.CreateTime = DateTime.Now;
            art.ChannelId = 18;
            art.IsDelete = 0;
            art.OrderId = 5;
            art.insert();
            int id = art.Id;

            art.Member = TMember.findById( 2 );
            art.Cat = TCat.findById( 10 );
            art.Board = TBoard.findById( 11 );
            art.Author = "周做人";
            art.Title = "论雷锋大的倒掉";
            art.CreateTime = DateTime.Now;
            art.ChannelId = 12;
            art.IsDelete = 0;
            art.OrderId = 8;
            art.insert();
            id = id + 1;
            Assert.AreEqual( id, art.Id );

            art.Member = TMember.findById( 7 );
            art.Cat = TCat.findById( 4 );
            art.Board = TBoard.findById( 4 );
            art.Author = "嵇康";
            art.Title = "与善举员绝交数";
            art.CreateTime = DateTime.Now;
            art.ChannelId = 4;
            art.IsDelete = 0;
            art.OrderId = 9;
            art.insert();
            id = id + 1;
            Assert.AreEqual( id, art.Id );

            art.Member = TMember.findById( 8 );
            art.Cat = TCat.findById( 7 );
            art.Board = TBoard.findById( 3 );
            art.Author = "羽化";
            art.Title = "许三管买血迹";
            art.CreateTime = DateTime.Now;
            art.ChannelId = 4;
            art.IsDelete = 0;
            art.OrderId = 15;
            art.insert();
            id = id + 1;
            Assert.AreEqual( id, art.Id );

            Console.WriteLine( "添加 article 成功！" );
        }

        public void InsertMember() {
            ConsoleTitleUtil.ShowTestTitle( "InsertMember" );

            TMember member = new TMember();

            member.Name = "张三";
            db.insert( member );

            member.Name = "屈原";
            db.insert( member );

            member.Name = "李白";
            db.insert( member );

            member.Name = "嵇康";
            db.insert( member );

            member.Name = "庄子";
            db.insert( member );

            member.Name = "杜甫";
            db.insert( member );

            member.Name = "苏轼";
            db.insert( member );

            member.Name = "李世民";
            db.insert( member );

            member.Name = "诸葛亮";
            db.insert( member );

            member.Name = "曹操";
            db.insert( member );

            member.Name = "司马迁";
            db.insert( member );

            member.Name = "孙中山";
            db.insert( member );

            member.Name = "袁世凯";
            db.insert( member );

            member.Name = "毛泽东";
            db.insert( member );

            Console.WriteLine( "添加 member 成功！" );

        }

        public void InsertCat() {
            ConsoleTitleUtil.ShowTestTitle( "InsertCat" );


            TCat cat = new TCat();
            cat.Name = "国内新闻";
            db.insert( cat );

            cat.Name = "国际新闻";
            db.insert( cat );

            cat.Name = "娱乐电影";
            db.insert( cat );

            cat.Name = "音像世界";
            db.insert( cat );

            cat.Name = "游戏广场";
            db.insert( cat );

            cat.Name = "文学艺术";
            db.insert( cat );

            cat.Name = "电视剧本";
            db.insert( cat );

            cat.Name = "武侠小说";
            db.insert( cat );

            cat.Name = "互联网络";
            db.insert( cat );

            cat.Name = "软件服务";
            db.insert( cat );

            cat.Name = "笑话幽默";
            db.insert( cat );

            cat.Name = "金融期货";
            db.insert( cat );

            Console.WriteLine( "添加 cat 成功！" );
        }


        public void InsertBoard() {
            ConsoleTitleUtil.ShowTestTitle( "InsertBoard" );


            TBoard board = new TBoard();
            board.Name = "board_国内新闻";
            db.insert( board );

            board.Name = "board_国际新闻";
            db.insert( board );

            board.Name = "board_娱乐电影";
            db.insert( board );

            board.Name = "board_音像世界";
            db.insert( board );

            board.Name = "board_游戏广场";
            db.insert( board );

            board.Name = "board_文学艺术";
            db.insert( board );

            board.Name = "board_电视剧本";
            db.insert( board );

            board.Name = "board_武侠小说";
            db.insert( board );

            board.Name = "board_互联网络";
            db.insert( board );

            board.Name = "board_软件服务";
            db.insert( board );

            board.Name = "board_笑话幽默";
            db.insert( board );

            board.Name = "board_金融期货";
            db.insert( board );

            Console.WriteLine( "添加 board 成功！" );
        }



        public void InsertTNews() {

            TAuthorCategory c1 = new TAuthorCategory() { Name = "作家" }; c1.insert();
            TAuthorCategory c2 = new TAuthorCategory() { Name = "学者" }; c2.insert();
            TAuthorCategory c3 = new TAuthorCategory() { Name = "明星" }; c3.insert();

            TAuthor a1 = new TAuthor() { Name = "屈原", Category = c1 }; a1.insert();
            TAuthor a2 = new TAuthor() { Name = "陶渊明", Category = c1 }; a2.insert();
            TAuthor a3 = new TAuthor() { Name = "顾炎武", Category = c2 }; a3.insert();
            TAuthor a4 = new TAuthor() { Name = "余秋雨", Category = c3 }; a4.insert();

            new TNews() { Title = "离骚", Author = a1 }.insert();
            new TNews() { Title = "天问", Author = a1 }.insert();

            new TNews() { Title = "读山海经", Author = a2 }.insert();
            new TNews() { Title = "闲情赋", Author = a2 }.insert();
            new TNews() { Title = "五柳先生传", Author = a2 }.insert();

            new TNews() { Title = "天下郡国利病书", Author = a3 }.insert();
            new TNews() { Title = "日知录", Author = a3 }.insert();

            new TNews() { Title = "文化苦旅", Author = a4 }.insert();
            new TNews() { Title = "山居笔记", Author = a4 }.insert();
            new TNews() { Title = "霜冷长河", Author = a4 }.insert();


        }



    }
}
