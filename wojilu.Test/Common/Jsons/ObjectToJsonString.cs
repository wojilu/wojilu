using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Serialization;
using System.Reflection;
using wojilu.Reflection;
using wojilu.ORM;
using System.Collections;

namespace wojilu.Test.Common.Jsons {
    

    [TestFixture]
    public class ObjectToJsonString {







        [Test]
        public void testHashtable() {

            Hashtable dic = new Hashtable();
            dic.Add( "name", "sunweb" );
            dic.Add( "age", 99 );
            dic.Add( "gender", "male" );

            string str = Json.SerializeDic( dic, false );
            Assert.AreEqual( "{ \"name\":\"sunweb\", \"age\":99, \"gender\":\"male\" }", str );

            MyPhone phone = new MyPhone();
            phone.Name = "新闻大事690501468";
            phone.Owner = new PhoneOwner { Id = 2 };
            dic.Add( "phone", phone );

            str = Json.SerializeDic( dic, false );
            Console.WriteLine( str );
        }

        [Test]
        public void testDictionary() {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add( "name", "sunweb" );
            dic.Add( "age", 99 );
            dic.Add( "gender", "male" );

            string str = Json.SerializeDic( dic, false );
            Assert.AreEqual( "{ \"name\":\"sunweb\", \"age\":99, \"gender\":\"male\" }", str );

            // 将对象放入dic中
            MyPhone phone = new MyPhone();
            phone.Name = "新闻大事690501468";
            phone.Owner = new PhoneOwner { Id = 2 };
            dic.Add( "phone", phone );

            str = Json.SerializeDic( dic, false );
            Console.WriteLine( str );
            Assert.AreEqual( "{ \"name\":\"sunweb\", \"age\":99, \"gender\":\"male\", \"phone\":{ \"Id\":0, \"Name\":\"新闻大事690501468\", \"Weight\":0, \"Owner\":{ \"Id\":2, \"Name\":\"\", \"Age\":\"\" } } }", str );
        }

        [Test]
        public void testDicToString() {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["title"] = "abcd";
            dic["user"] = "john";
            dic["body"] = "mycontent";
            Assert.AreEqual( "{ \"title\":\"abcd\", \"user\":\"john\", \"body\":\"mycontent\" }", Json.SerializeDic( dic ) );

            string lnk = "<a href=\"http://www.163.com/news.html\">这是文章标题</a>"; // 双引号需要转义
            dic["postLink"] = lnk;

            string expected = @"{ ""title"":""abcd"", ""user"":""john"", ""body"":""mycontent"", ""postLink"":""<a href=\""http://www.163.com/news.html\"">这是文章标题</a>"" }";

            Console.WriteLine( Json.SerializeDic( dic ) );
            Assert.AreEqual( expected, Json.SerializeDic( dic ) );

            Dictionary<string, object> result = Json.DeserializeDic( expected );
            Assert.AreEqual( 4, result.Count );
            Assert.AreEqual( lnk, result["postLink"] );

        }

        [Test]
        public void testEscape() {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["title"] = "abcd";
            dic["user"] = "john";
            dic["body"] = "my'content"; // 单引号不需要转义

            Assert.AreEqual( "{ \"title\":\"abcd\", \"user\":\"john\", \"body\":\"my'content\" }", Json.SerializeDic( dic ) );

            dic["other"] = "my name is:china"; // 冒号不需要转义
            String expected = "{ \"title\":\"abcd\", \"user\":\"john\", \"body\":\"my'content\", \"other\":\"my name is:china\" }";

            String actual = Json.SerializeDic( dic );
            Console.WriteLine( expected );
            Console.WriteLine( actual );
            Assert.AreEqual( expected, actual );

            Dictionary<string, object> result = Json.DeserializeDic( expected );
            Assert.AreEqual( 4, result.Count );
            Assert.AreEqual( "my'content", result["body"] );
            Assert.AreEqual( "my name is:china", result["other"] );

        }

        [Test]
        public void testEscapeEscape() {

            String blog = string.Format( "<a href=\"{0}\">{1}</a>", "/space/abcde/Blog22/Post/195", @"\framework\views\Common" );
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add( "blog", blog );
            String str = Json.SerializeDic( dic, false );

            Console.WriteLine( str );
            Assert.AreEqual( @"{ ""blog"":""<a href=\""/space/abcde/Blog22/Post/195\"">\\framework\\views\\Common</a>"" }", str );

            Dictionary<String, object> mydic = Json.DeserializeDic( str );
            Assert.AreEqual( "<a href=\"/space/abcde/Blog22/Post/195\">\\framework\\views\\Common</a>", mydic["blog"] );

        }

        // 换行内容测试
        [Test]
        public void testBreakLine() {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["title"] = "abcd";
            dic["user"] = "john";
            dic["body"] = "my"+Environment.NewLine+"content"; //字符串里面有换行

            String expected = Json.SerializeDic( dic );

            Assert.AreEqual( "{ \"title\":\"abcd\", \"user\":\"john\", \"body\":\"mycontent\" }", expected );

            Dictionary<string, object> result = Json.DeserializeDic( expected );
            Assert.AreEqual( 3, result.Count );


        }


        [Test]
        public void testList() {

            List<string> list = new List<string>();
            list.Add( "123" );
            list.Add( "abc" );
            list.Add( "" );
            list.Add( "name" );

            string str = Json.SerializeList( list );
            Assert.AreEqual( "[ \"123\", \"abc\", \"\", \"name\" ]", str );

            string newStr = Json.Serialize( list );
            Assert.AreEqual( str, newStr );

            ArrayList nlist = new ArrayList();
            nlist.Add( 123 );
            nlist.Add( "abc" );
            nlist.Add( "" );
            nlist.Add( "name" );
            string nstr = Json.Serialize( nlist );
            Assert.AreEqual( "[ 123, \"abc\", \"\", \"name\" ]", nstr );

            MyPhone phone = new MyPhone();
            phone.Name = "新闻大事690501468";
            phone.Owner = new PhoneOwner { Id = 2 };

            ArrayList mylist = new ArrayList();
            mylist.Add( "abc" );
            mylist.Add( 123 );
            mylist.Add( phone );

            string mystr = Json.Serialize( mylist );

            Assert.AreEqual( "[ \"abc\", 123, { \"Id\":0, \"Name\":\"新闻大事690501468\", \"Weight\":0, \"Owner\":{ \"Id\":2, \"Name\":\"\", \"Age\":\"\" } } ]", mystr );


        }

        [Test]
        public void testSimpleObject() {

            MyPhone phone = new MyPhone();
            phone.Name = "新闻大事690501468";
            phone.Owner = new PhoneOwner { Id = 2 };

            string strJson = Json.SerializeObject( phone );

            Console.WriteLine( strJson );

            string result = " { \"Id\":0, \"Name\":\"新闻大事690501468\", \"Weight\":0, \"Owner\":{ \"Id\":2, \"Name\":\"\", \"Age\":\"\" } }";

            Assert.AreEqual( result.Trim(), strJson.Trim() );
        }


        [Test]
        public void testArray() {
            string[] arr = new string[] {
                "123", "abc", "", "name"
            };

            string str = Json.SerializeArray( arr );
            Assert.AreEqual( "[ \"123\", \"abc\", \"\", \"name\" ]", str );

            string newStr = Json.Serialize( arr );
            Assert.AreEqual( str, newStr );
        }


        [Test]
        public void testNumber() {

            string str = Json.Serialize( 123 );
            Assert.AreEqual( str, "123" );
            str = Json.Serialize( 12.33 );
            Assert.AreEqual( str, "12.33" );
            str = Json.Serialize( -3.5 );
            Assert.AreEqual( str, "-3.5" );
        }

        [Test]
        public void testString() {

            string str = Json.Serialize( "" );
            Assert.AreEqual( str, "\"\"" );
            str = Json.Serialize( null );
            Assert.AreEqual( str, "\"\"" );
            str = Json.Serialize( "123" );
            Assert.AreEqual( str, "\"123\"" );

            str = Json.Serialize( @"\framework\views\Common" );
            Console.WriteLine( str );
            Assert.AreEqual( str, @"""\\framework\\views\\Common""" );


        }

        [Test]
        public void testBool() {

            string str = Json.Serialize( false );
            Assert.AreEqual( "false", str );
            str = Json.Serialize( true );
            Assert.AreEqual( "true", str );
        }

        [Test]
        public void testDateTime() {

            string str = Json.Serialize( new DateTime( 1999, 12, 6, 17, 25, 58 ) );
            //Assert.AreEqual( "\"1999-12-6 17:25:58\"", str );
            Console.WriteLine( str);
        }

        //---------------------------------------------------------------------------------

        [Test]
        public void ObjectsToString() {

            IList results = new ArrayList();

            results.Add( getPhoneList() );
            results.Add( getPhoneList() );
            results.Add( getPhoneList() );
            results.Add( getPhoneList() );

            string strJson = Json.SerializeListSimple( results );

            Console.WriteLine( strJson );

            string result = @"
[
	{ Id:0, Name:""新闻大事690501468"", Weight:0, Owner:""2"" },
	{ Id:0, Name:""新闻大事690501468"", Weight:0, Owner:""2"" },
	{ Id:0, Name:""新闻大事690501468"", Weight:0, Owner:""2"" },
	{ Id:0, Name:""新闻大事690501468"", Weight:0, Owner:""2"" }
]
";

            Assert.AreEqual( result.Trim(), strJson.Trim() );

        }

        private static MyPhone getPhoneList() {
            MyPhone phone = new MyPhone();
            phone.Name = "新闻大事690501468";
            phone.Owner = new PhoneOwner { Id = 2 };
            return phone;
        }

        [Test]
        public void testSubObjects() {

            AdminMenu m = new AdminMenu {
                 Id = "3", Name = "新闻", Url = "/abc.aspx"
            };

            String str = Json.SerializeObjectSimple( m );
            Console.WriteLine( str );

            AdminMenuGroup g = new AdminMenuGroup();
            g.Id = "118";
            g.Name = "菜单组";

            g.AdminMenus = new List<AdminMenu>();
            g.AdminMenus.Add( m );

            //String subStr = JSON.ObjectToJSON( g );
            String subStr = Json.Serialize( g );

            Console.WriteLine( subStr );


        }


        [Test]
        public void testDbConfig() {

            MyDbConfig cf = new MyDbConfig();

            cf.ConnectionStringTable = new Dictionary<string, object>();
            cf.ConnectionStringTable.Add( "default", "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=wojilu.mdb" );
            cf.ConnectionStringTable.Add( "db2", "server=localhost;uid=wojilusyy;pwd=test123;database=syyWojilu;" );

            cf.AssemblyList = new List<object>();
            cf.AssemblyList.Add( "wojilu.Core" );
            cf.AssemblyList.Add( "wojilu.Apps" );

            string str = Json.Serialize( cf, true );
            Console.WriteLine( str );

        }
    }

    public class AdminMenuGroup {
        public List<AdminMenu> AdminMenus { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class AdminMenu {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Info { get; set; }
    }

}
