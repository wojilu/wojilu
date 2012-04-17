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



        // ע�⣺���� EnableContextCache=false����ȫ�����Գɹ�
        // ����ֱ���Ҽ��˴���ʼ���ԣ������ÿ������ݿ��ʼ����
        // (�߼����ԣ��̳в��ֲ��Լ�TestPolymorphism)












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
            Console.WriteLine( "��ǰ�������ݿ����ͣ�" + cnstr.DbType );
            Console.WriteLine("��ǰ�������ݿ�ID���ͣ�" + DbConfig.Instance.IdType);
            Console.WriteLine( "��ǰ�������ݿ����ͣ�" + cnstr.StringContent );
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
            mb.MyMoney = 99.1268m; // money���Ϳ������洢4λ��
            mb.DecimalNum = 777.12345m; // TMoney��ע���Զ�����5λС��

            mb.DubleNum = 988.1234567899;
            mb.SingleNum = 999.128f; // mysql floatĬ��3λ
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
                art.Title = "����ʲô�ط��Ĳ���" + i;
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


            List<TCat> cats = db.find<TCat>( "Name like '%'+:t+'%'" ).set( "t", "����" ).list();

            Assert.AreEqual( 2, cats.Count );

            Assert.AreEqual( "��������", cats[0].Name );
            Assert.AreEqual( "��������", cats[1].Name );

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
            Assert.AreEqual( "���ֵ�Ӱ", mycat.Name );
        }

        [Test]
        public void FindCondition_Simply4() {
            ConsoleTitleUtil.ShowTestTitle( "FindCondition_Simply4" );

            IList<TArticle> articles = db.find<TArticle>( "OrderId=" + 9 ).list();
            Assert.AreEqual( 1, articles.Count );

            TArticle art = articles[0];
            Assert.AreEqual( "����", art.Author );

            IList<TCat> cats = TArticle.find( "OrderId=" + 9 ).listChildren<TCat>( "Cat" );
            Assert.AreEqual( 1, cats.Count );
            Assert.AreEqual( "��������", cats[0].Name );

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
            art.Author = "163��";
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

            // �������ѯ
            IList<TArticle> articles = db.find<TArticle>( "Member.Id=:mid" ).set( "mid", 7 ).select( "Id,Title,Member.Id,Cat.Name,Cat.ArticleCount,Board.Name" ).list();
            //art.Select( "Id,Title,Member.Id,Cat.Name,Cat.ArticleCount,Board.Name" );
            //IList articles = art.Find( "Member.Id=:mid" ).Set( "mid",7 ).List();

            Assert.AreEqual( 1, articles.Count );
            TArticle article = articles[0];
            Assert.IsNotNull( article );
            Assert.IsNotNull( article.Title );

            //sql�������
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

            // ��Ҫ�����ѯ
            IList<TArticle> articles = db.find<TArticle>( "Member.Id=:mid and Cat.Name=:catname order by Id desc, Member.Id asc" ).set( "mid", 7 ).set( "catname", "��������" ).select( "Id,Title,Member.Id,Cat.Name,Cat.ArticleCount,Board.Name" ).list();

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

        // ֱ��ʹ�� Include��������������
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

        //// ֱ��ʹ�� Select��������������
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

            //����find
            TCat cat = db.findById<TCat>( 3 );

            Console.WriteLine( "cat name is : " + cat.Name );
            Assert.AreEqual( "���ֵ�Ӱ", cat.Name );

            TCat nullCat = db.findById<TCat>( 99999999 );
            Assert.IsNull( nullCat );
        }

        [Test]
        public void FindById_ContextCache() {
            ConsoleTitleUtil.ShowTestTitle( "FindById_ContextCache" );

            //���ӹ���find
            TArticle art = db.findById<TArticle>( 1 );
            Console.WriteLine( "article 1 title is : " + art.Title );
            Assert.AreEqual( "�����ռ�", art.Title );

            //һ������
            TMember member = db.findById<TMember>( 13 );
            Console.WriteLine( "member 13 name is : " + member.Name );
            Assert.AreEqual( "Ԭ����", member.Name );

            TCat cat = db.findById<TCat>( 8 );
            Console.WriteLine( "cat 8 name is " + cat.Name );
            Assert.AreEqual( "����С˵", cat.Name );

            TBoard board = db.findById<TBoard>( 9 );
            Console.WriteLine( "board 9 name is : " + board.Name );
            Assert.AreEqual( "board_��������", board.Name );

        }

        [Test]
        public void Count() {

            int count = db.count<TCat>();
            Assert.AreEqual( 12, count );

            count = db.count<TCat>( "Id=3" );
            Assert.AreEqual( 1, count );

            count = db.count<TArticle>( "Member.Id=7" );
            Assert.AreEqual( 1, count );

            count = db.count<TArticle>( "Author='������'  " );
            Assert.AreEqual( 1, count );

            // ����ֱ��ʹ��Count���ٶȱȽϿ죬��Ϊֱ������ select count(*) from TCat ���

            // �����count�ٶ������� select Id from Syy_Article where ( MemberId = 7 )  order by Id desc ��Ȼ�����������
            // ����ڶ��ַ���Ҳ���ŵ㣬���ǿ���ʹ�ò�������ѯ

            count = db.find<TArticle>( "Member.Id=:mid" ).set( "mid", 7 ).count();
            Assert.AreEqual( 1, count );
        }


        [Test]
        public void findPage() {

            int pageSize = 5;

            //------------- ��1ҳ ---------------
            int current = 1;
            CurrentRequest.setCurrentPage( current );

            DataPage<TBoard> list = TBoard.findPage( "Id>0", pageSize );

            Assert.AreEqual( current, list.Current );
            Assert.AreEqual( 12, list.RecordCount );
            Assert.AreEqual( 3, list.PageCount );
            Assert.AreEqual( pageSize, list.Size );
            Assert.AreEqual( pageSize, list.Results.Count );

            //------------- ��2ҳ ---------------
            current = 2;
            CurrentRequest.setCurrentPage( current );

            list = TBoard.findPage( "Id>0", pageSize );

            Assert.AreEqual( current, list.Current );
            Assert.AreEqual( 12, list.RecordCount );
            Assert.AreEqual( 3, list.PageCount );
            Assert.AreEqual( pageSize, list.Size );
            Assert.AreEqual( pageSize, list.Results.Count );


            //------------- ��3ҳ ---------------

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

            Assert.AreEqual( list[0].Author.Name, "��ԭ" );
            Assert.AreEqual( list[3].Author.Name, "��Ԩ��" );
            Assert.AreEqual( list[6].Author.Name, "������" );
            Assert.AreEqual( list[9].Author.Name, "������" );

            Assert.AreEqual( list[0].Author.Category.Name, "����" );
            Assert.AreEqual( list[3].Author.Category.Name, "����" );
            Assert.AreEqual( list[6].Author.Category.Name, "ѧ��" );
            Assert.AreEqual( list[9].Author.Category.Name, "����" );

            /* ���ɵ� sql ��䡪��
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
            Assert.AreEqual( "��������", cat.Name );
            cat.Name = "���ﲻ������";
            db.update( cat );

            TCat newcat = db.findById<TCat>( cat.Id );
            Assert.AreEqual( "���ﲻ������", newcat.Name );
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
            art.Author = "54³Ѹ";
            art.Title = "�����ռ�";
            art.CreateTime = DateTime.Now;
            art.ChannelId = 18;
            art.IsDelete = 0;
            art.OrderId = 5;
            art.insert();
            int id = art.Id;

            art.Member = TMember.findById( 2 );
            art.Cat = TCat.findById( 10 );
            art.Board = TBoard.findById( 11 );
            art.Author = "������";
            art.Title = "���׷��ĵ���";
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
            art.Author = "����";
            art.Title = "���ƾ�Ա������";
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
            art.Author = "��";
            art.Title = "��������Ѫ��";
            art.CreateTime = DateTime.Now;
            art.ChannelId = 4;
            art.IsDelete = 0;
            art.OrderId = 15;
            art.insert();
            id = id + 1;
            Assert.AreEqual( id, art.Id );

            Console.WriteLine( "��� article �ɹ���" );
        }

        public void InsertMember() {
            ConsoleTitleUtil.ShowTestTitle( "InsertMember" );

            TMember member = new TMember();

            member.Name = "����";
            db.insert( member );

            member.Name = "��ԭ";
            db.insert( member );

            member.Name = "���";
            db.insert( member );

            member.Name = "����";
            db.insert( member );

            member.Name = "ׯ��";
            db.insert( member );

            member.Name = "�Ÿ�";
            db.insert( member );

            member.Name = "����";
            db.insert( member );

            member.Name = "������";
            db.insert( member );

            member.Name = "�����";
            db.insert( member );

            member.Name = "�ܲ�";
            db.insert( member );

            member.Name = "˾��Ǩ";
            db.insert( member );

            member.Name = "����ɽ";
            db.insert( member );

            member.Name = "Ԭ����";
            db.insert( member );

            member.Name = "ë��";
            db.insert( member );

            Console.WriteLine( "��� member �ɹ���" );

        }

        public void InsertCat() {
            ConsoleTitleUtil.ShowTestTitle( "InsertCat" );


            TCat cat = new TCat();
            cat.Name = "��������";
            db.insert( cat );

            cat.Name = "��������";
            db.insert( cat );

            cat.Name = "���ֵ�Ӱ";
            db.insert( cat );

            cat.Name = "��������";
            db.insert( cat );

            cat.Name = "��Ϸ�㳡";
            db.insert( cat );

            cat.Name = "��ѧ����";
            db.insert( cat );

            cat.Name = "���Ӿ籾";
            db.insert( cat );

            cat.Name = "����С˵";
            db.insert( cat );

            cat.Name = "��������";
            db.insert( cat );

            cat.Name = "�������";
            db.insert( cat );

            cat.Name = "Ц����Ĭ";
            db.insert( cat );

            cat.Name = "�����ڻ�";
            db.insert( cat );

            Console.WriteLine( "��� cat �ɹ���" );
        }


        public void InsertBoard() {
            ConsoleTitleUtil.ShowTestTitle( "InsertBoard" );


            TBoard board = new TBoard();
            board.Name = "board_��������";
            db.insert( board );

            board.Name = "board_��������";
            db.insert( board );

            board.Name = "board_���ֵ�Ӱ";
            db.insert( board );

            board.Name = "board_��������";
            db.insert( board );

            board.Name = "board_��Ϸ�㳡";
            db.insert( board );

            board.Name = "board_��ѧ����";
            db.insert( board );

            board.Name = "board_���Ӿ籾";
            db.insert( board );

            board.Name = "board_����С˵";
            db.insert( board );

            board.Name = "board_��������";
            db.insert( board );

            board.Name = "board_�������";
            db.insert( board );

            board.Name = "board_Ц����Ĭ";
            db.insert( board );

            board.Name = "board_�����ڻ�";
            db.insert( board );

            Console.WriteLine( "��� board �ɹ���" );
        }



        public void InsertTNews() {

            TAuthorCategory c1 = new TAuthorCategory() { Name = "����" }; c1.insert();
            TAuthorCategory c2 = new TAuthorCategory() { Name = "ѧ��" }; c2.insert();
            TAuthorCategory c3 = new TAuthorCategory() { Name = "����" }; c3.insert();

            TAuthor a1 = new TAuthor() { Name = "��ԭ", Category = c1 }; a1.insert();
            TAuthor a2 = new TAuthor() { Name = "��Ԩ��", Category = c1 }; a2.insert();
            TAuthor a3 = new TAuthor() { Name = "������", Category = c2 }; a3.insert();
            TAuthor a4 = new TAuthor() { Name = "������", Category = c3 }; a4.insert();

            new TNews() { Title = "��ɧ", Author = a1 }.insert();
            new TNews() { Title = "����", Author = a1 }.insert();

            new TNews() { Title = "��ɽ����", Author = a2 }.insert();
            new TNews() { Title = "���鸳", Author = a2 }.insert();
            new TNews() { Title = "����������", Author = a2 }.insert();

            new TNews() { Title = "���¿���������", Author = a3 }.insert();
            new TNews() { Title = "��֪¼", Author = a3 }.insert();

            new TNews() { Title = "�Ļ�����", Author = a4 }.insert();
            new TNews() { Title = "ɽ�ӱʼ�", Author = a4 }.insert();
            new TNews() { Title = "˪�䳤��", Author = a4 }.insert();


        }



    }
}
