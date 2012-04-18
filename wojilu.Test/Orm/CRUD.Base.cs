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
    public class CRUDBase {


        // ע�⣺���� EnableContextCache=false����ȫ�����Գɹ�
        // ����ֱ���Ҽ��˴���ʼ���ԣ������ÿ������ݿ��ʼ����
        // (�߼����ԣ��̳в��ֲ��Լ�TestPolymorphism)

        





        private static readonly ILog logger = LogManager.GetLogger( typeof( CRUDGeneric ) );
        [TestFixtureSetUp]
        public void InitData() {

            wojiluOrmTestInit.ClearLog();
            wojiluOrmTestInit.InitMetaData();

            InsertMember();
            InsertCat();
            InsertBoard();
            InsertArticle();
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
        public void FindPropertyNull() {

            string artIds = "";
            string catIds = "";
            ArrayList list = new ArrayList();
            IList arts = new ArrayList();
            for (int i = 0; i < 5; i++) {
                TCat cat = new TCat();
                cat.Name = "tempCat1";

                cat.insert();

                list.Add( cat );
                catIds += cat.Id + ",";

                TArticle art = new TArticle();
                art.Title = "����ʲô�ط��Ĳ���" + i;
                art.Cat = cat;

                art.insert();

                arts.Add( art );
                artIds += art.Id + ",";

                cat.delete();

            }
            catIds = catIds.TrimEnd( ',' );
            artIds = artIds.TrimEnd( ',' );

            IList<TArticle> results = TArticle.find( "Id in (" + artIds + ")" ).list();
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

            List<TCat> cats = TCat.find( "Id>:myid" ).set( "myid", 5 ).select( "Id,Name,ArticleCount" ).list();
            Assert.AreEqual( 7, cats.Count );

            foreach (TCat cat in cats) {
                Assert.Greater( cat.Id, 0 );
                Assert.IsNotNull( cat.Name );
            }
        }


        [Test]
        public void FindCondition_Params() {

            ConsoleTitleUtil.ShowTestTitle( "FindCondition_Params" );

            List<TCat> cats = TCat.find( "Id in (:id1,:id2,:id3)" )
                .set( "id1", 6 )
                .set( "id2", 7 )
                .set( "id3", 8 )
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


            List<TCat> cats = TCat.find( "Name like '%'+:t+'%'" ).set( "t", "����" ).list();

            Assert.AreEqual( 2, cats.Count );

            Assert.AreEqual( "��������", cats[0].Name );
            Assert.AreEqual( "��������", cats[1].Name );

        }

        [Test]
        public void FindCondition_Simply2() {
            ConsoleTitleUtil.ShowTestTitle( "FindCondition_Simply" );

            IList<TCat> cats = TCat.find( "Id>" + 5 ).select( "Id,Name,ArticleCount" ).list();

            Assert.AreEqual( 7, cats.Count );

            foreach (TCat cat in cats) {
                Assert.Greater( cat.Id, 0 );
                Assert.IsNotNull( cat.Name );
            }
        }

        [Test]
        public void FindCondition_Simply3() {
            ConsoleTitleUtil.ShowTestTitle( "FindCondition_Simply" );

            TCat mycat = TCat.find( "Id=" + 3 ).first();
            Assert.IsNotNull( mycat );
            Assert.AreEqual( "���ֵ�Ӱ", mycat.Name );
        }

        [Test]
        public void FindCondition_Simply4() {
            ConsoleTitleUtil.ShowTestTitle( "FindCondition_Simply" );

            IList<TArticle> articles = TArticle.find( "OrderId=" + 9 ).list();
            Assert.AreEqual( 1, articles.Count );

            TArticle art = articles[0];
            Assert.AreEqual( "����", art.Author );
        }


        [Test]
        public void FindCondition_WithEqual() {

            TArticle art = new TArticle();
            art.Member = TMember.findById( 8 );
            art.Cat = TCat.findById( 7 );
            art.Board = TBoard.findById( 3 );
            art.Author = "163��";
            art.Title = "myurl=www.163.com";
            art.CreateTime = DateTime.Now;
            art.ChannelId = 4;
            art.IsDelete = 0;
            art.OrderId = 15;

            art.insert();

            IList<TArticle> articles = TArticle.find( "Title=:t" ).set( "t", art.Title ).list();
            Assert.AreEqual( 1, articles.Count );

            logger.Info( "===========================================================================" );
            IList<TArticle> articles2 = TArticle.find( "Title='" + art.Title + "'" ).list();
            Assert.AreEqual( 1, articles2.Count );


        }

        [Test]
        public void FindCondition_PropertyId() {

            IList<TArticle> articles = TArticle.find( "Member.Id=7" )
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
            IList<TArticle> articles = TArticle.find( "Member.Id=:mid" ).set( "mid", 7 ).select( "Id,Title,Member.Id,Cat.Name,Cat.ArticleCount,Board.Name" ).list();

            Assert.AreEqual( 1, articles.Count );
            TArticle article = articles[0];
            Assert.IsNotNull( article );
            Assert.IsNotNull( article.Title );

            //sql�������
            IList<TArticle> articles2 = TArticle.find( "Member.Id=:mid" ).set( "mid", 7 ).select( "Id,Title,Member.Id,Cat.Name,Cat.ArticleCount,Board.Name" ).list();

            Assert.AreEqual( 1, articles2.Count );
            TArticle article2 = articles2[0];
            Assert.IsNotNull( article2 );
            Assert.IsNotNull( article2.Title );

        }

        [Test]
        public void FindCondition_JoinTable() {
            ConsoleTitleUtil.ShowTestTitle( "FindCondition_JoinTable" );

            // ��Ҫ�����ѯ
            IList<TArticle> articles = TArticle.find( "Member.Id=:mid and Cat.Name=:catname order by Id desc, Member.Id asc" ).set( "mid", 7 ).set( "catname", "��������" ).select( "Id,Title,Member.Id,Cat.Name,Cat.ArticleCount,Board.Name" ).list();

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

            IList<TArticle> articles = TArticle.find( "" ).list();

            Assert.AreEqual( 4, articles.Count );
            foreach (TArticle myart in articles) {
                Assert.IsNotNull( myart.Title );
                Assert.IsNotNull( myart.Cat.Name );
                Assert.IsNotNull( myart.Board.Name );
            }
        }

        //------------------------- FindAll -------------------------

        [Test]
        public void FindAll_Simply() {
            ConsoleTitleUtil.ShowTestTitle( "FindAll_Simply" );

            IList<TCat> results = TCat.findAll();
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

            IList<TArticle> arts = TArticle.findAll();
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

        //------------------------- FindById -------------------------

        [Test]
        public void FindById_Simply() {
            ConsoleTitleUtil.ShowTestTitle( "FindById_Simply" );

            //����find
            TCat cat = TCat.findById( 3 );

            Console.WriteLine( "cat name is : " + cat.Name );
            Assert.AreEqual( "���ֵ�Ӱ", cat.Name );

            TCat nullCat = TCat.findById( 99999999 );
            Assert.IsNull( nullCat );
        }

        [Test]
        public void FindById_ContextCache() {
            ConsoleTitleUtil.ShowTestTitle( "FindById_ContextCache" );

            //���ӹ���find
            TArticle art = TArticle.findById( 1 );
            Console.WriteLine( "article 1 title is : " + art.Title );
            Assert.AreEqual( "�����ռ�", art.Title );

            //һ������
            TMember member = TMember.findById( 13 );
            Console.WriteLine( "member 13 name is : " + member.Name );
            Assert.AreEqual( "Ԭ����", member.Name );

            TCat cat = TCat.findById( 8 );
            Console.WriteLine( "cat 8 name is " + cat.Name );
            Assert.AreEqual( "����С˵", cat.Name );

            TBoard board = TBoard.findById( 9 );
            Console.WriteLine( "board 9 name is : " + board.Name );
            Assert.AreEqual( "board_��������", board.Name );

        }

        [Test]
        public void Count() {

            int count = TCat.count();
            Assert.AreEqual( 12, count );

            count = TCat.count( "Id=3" );
            Assert.AreEqual( 1, count );

            count = TArticle.count( "Member.Id=7" );
            Assert.AreEqual( 1, count );

            count = TArticle.count( "Author='������'  " );
            Assert.AreEqual( 1, count );

            // ����ֱ��ʹ��Count���ٶȱȽϿ죬��Ϊֱ������ select count(*) from TCat ���

            // �����count�ٶ������� select Id from Syy_Article where ( MemberId = 7 )  order by Id desc ��Ȼ�����������
            // ����ڶ��ַ���Ҳ���ŵ㣬���ǿ���ʹ�ò�������ѯ

            //count = TArticle.find( "Member.Id=:mid" ).set( "mid", 7 ).count();
            //Assert.AreEqual( 1, count );
        }

        //------------------------- findPage -------------------------

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


            //------------- ��nҳ ---------------

            for (int i = 0; i < 100; i++) {

                TBoard board = new TBoard();
                board.Name = "board_pageTest_"+ i;
                board.insert();
            }


            current = 2;

            CurrentRequest.setCurrentPage( current );

            list = TBoard.findPage( "Id>0 order by Id asc", 10 );
            Assert.AreEqual( current, list.Current );

            Assert.AreEqual( 112, list.RecordCount );
            Assert.AreEqual( 12, list.PageCount );
            Assert.AreEqual( 10, list.Results.Count );

            String ids = "";
            foreach (TBoard bd in list.Results) {
                ids += bd.Id+ ", " + bd.Name + Environment.NewLine;
            }
            Console.WriteLine( ids );

            String bar = strUtil.ParseHtml( list.PageBar ).Replace("&nbsp;", ",");
            Console.WriteLine( bar );


        }


        //------------------------- Update -------------------------

        [Test]
        public void UpdateACat() {

            ConsoleTitleUtil.ShowTestTitle( "UpdateCat" );

            TCat cat = TCat.findById( 2 );
            Assert.AreEqual( "��������", cat.Name );
            cat.Name = "���ﲻ������";
            cat.update();

            TCat newcat = TCat.findById( cat.Id );
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
            member.insert();

            member.Name = "��ԭ";
            member.insert();

            member.Name = "���";
            member.insert();

            member.Name = "����";
            member.insert();

            member.Name = "ׯ��";
            member.insert();

            member.Name = "�Ÿ�";
            member.insert();

            member.Name = "����";
            member.insert();

            member.Name = "������";
            member.insert();

            member.Name = "�����";
            member.insert();

            member.Name = "�ܲ�";
            member.insert();

            member.Name = "˾��Ǩ";
            member.insert();

            member.Name = "����ɽ";
            member.insert();

            member.Name = "Ԭ����";
            member.insert();

            member.Name = "ë��";
            member.insert();

            Console.WriteLine( "��� member �ɹ���" );

        }

        public void InsertCat() {
            ConsoleTitleUtil.ShowTestTitle( "InsertCat" );


            TCat cat = new TCat();
            cat.Name = "��������";
            cat.insert();

            cat.Name = "��������";
            cat.insert();

            cat.Name = "���ֵ�Ӱ";
            cat.insert();

            cat.Name = "��������";
            cat.insert();

            cat.Name = "��Ϸ�㳡";
            cat.insert();

            cat.Name = "��ѧ����";
            cat.insert();

            cat.Name = "���Ӿ籾";
            cat.insert();

            cat.Name = "����С˵";
            cat.insert();

            cat.Name = "��������";
            cat.insert();

            cat.Name = "�������";
            cat.insert();

            cat.Name = "Ц����Ĭ";
            cat.insert();

            cat.Name = "�����ڻ�";
            cat.insert();

            Console.WriteLine( "��� cat �ɹ���" );
        }


        public void InsertBoard() {
            ConsoleTitleUtil.ShowTestTitle( "InsertBoard" );


            TBoard board = new TBoard();
            board.Name = "board_��������";
            board.insert();

            board.Name = "board_��������";
            board.insert();

            board.Name = "board_���ֵ�Ӱ";
            board.insert();

            board.Name = "board_��������";
            board.insert();

            board.Name = "board_��Ϸ�㳡";
            board.insert();

            board.Name = "board_��ѧ����";
            board.insert();

            board.Name = "board_���Ӿ籾";
            board.insert();

            board.Name = "board_����С˵";
            board.insert();

            board.Name = "board_��������";
            board.insert();

            board.Name = "board_�������";
            board.insert();

            board.Name = "board_Ц����Ĭ";
            board.insert();

            board.Name = "board_�����ڻ�";
            board.insert();

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
