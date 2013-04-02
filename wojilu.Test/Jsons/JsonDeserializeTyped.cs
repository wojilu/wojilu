using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Reflection;
using System.Collections;
using wojilu.Apps.Content.Domain;

namespace wojilu.Test.Common.Jsons {


    [TestFixture]
    public class JsonDeserializeTyped {










        [Test]
        public void testDeserializeType() {

            TJEntity x = new TJEntity();
            x.Id = 2;
            x.Name = "x02";
            x.TMoney = 2.92M;
            x.Area = 22222222222222222L;
            x.TDouble = 22222.2222222299;
            x.IsBlack = true;
            x.Created = DateTime.Now.AddDays( -2 );

            x.TPhone = new TPhone { Id = 1, Name = "p01", Weight = 11 };
            x.JsonObject = new JsonObject();
            x.JsonObject.Add( "key1", "v1" );
            x.JsonObject.Add( "key2", "v2" );

            x.IntArray = new int[] { 2, 6, 8 };
            x.Id_List = new List<int> { 3, 4, 5 };
            x.Name_List = new List<string> { "x03", "x04", "x05" };
            x.TMoney_List = new List<decimal> { 3.93M, 4.94M, 5.95M };
            x.Area_List = new List<long> { 3333333333333333333L, 444444444444444444L, 555555555555555555L };
            x.TValue_List = new List<double> { 33333.2222222299, 44444.2222222299, 55555.2222222299 };
            x.IsBlack_List = new List<bool> { false, true, false };
            x.Created_List = new List<DateTime> { DateTime.Now.AddDays( -3 ), DateTime.Now.AddDays( -4 ), DateTime.Now.AddDays( -5 ) };

            x.TPhone_List = new List<TPhone> {
                new TPhone { Id=2, Name = "p02", Weight=22 },
                new TPhone { Id=3, Name = "p03", Weight=33 },
                new TPhone { Id=4, Name = "p04", Weight=44 }
            };

            x.TPhone_Dic = new Dictionary<string, TPhone>();
            x.TPhone_Dic.Add( "phone5", new TPhone { Id = 5, Name = "p05", Weight = 55 } );
            x.TPhone_Dic.Add( "phone6", new TPhone { Id = 6, Name = "p06", Weight = 66 } );
            x.TPhone_Dic.Add( "phone7", new TPhone { Id = 7, Name = "p07", Weight = 77 } );

            x.TPhone_Array = new TPhone[] {
                new TPhone { Id=7, Name = "p07", Weight=77 },
                new TPhone { Id=8, Name = "p08", Weight=88 },
                new TPhone { Id=9, Name = "p09", Weight=99 }
            };


            String jsonString = Json.ToString( x );
            Console.WriteLine( jsonString );

            // 反序列化测试
            TJEntity k = Json.Deserialize<TJEntity>( jsonString );
            Assert.IsNotNull( k );

            // 基础类型的属性
            Assert.AreEqual( x.Id, k.Id );
            Assert.AreEqual( x.Name, k.Name );
            Assert.AreEqual( x.Area, k.Area );

            Assert.AreEqual( x.TMoney, k.TMoney );
            Assert.AreEqual( x.TDouble, k.TDouble );
            Assert.AreEqual( x.IsBlack, k.IsBlack );
            Assert.IsTrue( isTimeEqual( x.Created, k.Created ) );

            // 其他对象类型
            Assert.IsNotNull( k.TPhone );
            Assert.AreEqual( x.TPhone.Id, k.TPhone.Id );
            Assert.AreEqual( x.TPhone.Name, k.TPhone.Name );
            Assert.AreEqual( x.TPhone.Weight, k.TPhone.Weight );

            // 原始 JsonObject 对象
            Assert.IsNotNull( k.JsonObject );
            Assert.AreEqual( "v1", k.JsonObject.Get( "key1" ) );
            Assert.AreEqual( "v2", k.JsonObject.Get( "key2" ) );

            // List列表属性
            Assert.AreEqual( x.Id_List.Count, k.Id_List.Count );
            Assert.AreEqual( x.Id_List[0], k.Id_List[0] );
            Assert.AreEqual( x.Id_List[1], k.Id_List[1] );
            Assert.AreEqual( x.Id_List[2], k.Id_List[2] );

            Assert.AreEqual( x.IntArray.Length, k.IntArray.Length );
            Assert.AreEqual( x.IntArray[0], k.IntArray[0] );
            Assert.AreEqual( x.IntArray[1], k.IntArray[1] );
            Assert.AreEqual( x.IntArray[2], k.IntArray[2] );

            Assert.AreEqual( x.Name_List.Count, k.Name_List.Count );
            Assert.AreEqual( x.Name_List[0], k.Name_List[0] );
            Assert.AreEqual( x.Name_List[1], k.Name_List[1] );
            Assert.AreEqual( x.Name_List[2], k.Name_List[2] );

            Assert.AreEqual( x.Area_List.Count, k.Area_List.Count );
            Assert.AreEqual( x.Area_List[0], k.Area_List[0] );
            Assert.AreEqual( x.Area_List[1], k.Area_List[1] );
            Assert.AreEqual( x.Area_List[2], k.Area_List[2] );

            Assert.AreEqual( x.TMoney_List.Count, k.TMoney_List.Count );
            Assert.AreEqual( x.TMoney_List[0], k.TMoney_List[0] );
            Assert.AreEqual( x.TMoney_List[1], k.TMoney_List[1] );
            Assert.AreEqual( x.TMoney_List[2], k.TMoney_List[2] );

            Assert.AreEqual( x.TValue_List.Count, k.TValue_List.Count );
            Assert.AreEqual( x.TValue_List[0], k.TValue_List[0] );
            Assert.AreEqual( x.TValue_List[1], k.TValue_List[1] );
            Assert.AreEqual( x.TValue_List[2], k.TValue_List[2] );

            Assert.AreEqual( x.IsBlack_List.Count, k.IsBlack_List.Count );
            Assert.AreEqual( x.IsBlack_List[0], k.IsBlack_List[0] );
            Assert.AreEqual( x.IsBlack_List[1], k.IsBlack_List[1] );
            Assert.AreEqual( x.IsBlack_List[2], k.IsBlack_List[2] );

            Assert.AreEqual( x.Created_List.Count, k.Created_List.Count );
            Assert.IsTrue( isTimeEqual( x.Created_List[0], k.Created_List[0] ) );
            Assert.IsTrue( isTimeEqual( x.Created_List[1], k.Created_List[1] ) );
            Assert.IsTrue( isTimeEqual( x.Created_List[2], k.Created_List[2] ) );

            // List<TypedObject>属性
            Assert.AreEqual( x.TPhone_List.Count, k.TPhone_List.Count );
            Assert.AreEqual( x.TPhone_List[0].Id, k.TPhone_List[0].Id );
            Assert.AreEqual( x.TPhone_List[0].Name, k.TPhone_List[0].Name );
            Assert.AreEqual( x.TPhone_List[0].Weight, k.TPhone_List[0].Weight );

            // Array<TypedObject>属性
            Assert.AreEqual( x.TPhone_Array.Length, k.TPhone_Array.Length );
            Assert.AreEqual( x.TPhone_Array[0].Id, k.TPhone_Array[0].Id );
            Assert.AreEqual( x.TPhone_Array[0].Name, k.TPhone_Array[0].Name );
            Assert.AreEqual( x.TPhone_Array[0].Weight, k.TPhone_Array[0].Weight );

            // Dictionary<String, TypedObject>属性
            Assert.AreEqual( x.TPhone_Dic.Count, k.TPhone_Dic.Count );
            foreach (KeyValuePair<String, TPhone> kv in x.TPhone_Dic) {
                Assert.AreEqual( kv.Value.Id, k.TPhone_Dic[kv.Key].Id );
                Assert.AreEqual( kv.Value.Name, k.TPhone_Dic[kv.Key].Name );
                Assert.AreEqual( kv.Value.Weight, k.TPhone_Dic[kv.Key].Weight );
            }


        }

        private bool isTimeEqual( DateTime x, DateTime y ) {

            return x.Year == y.Year &&
                x.Month == y.Month &&
                x.Day == y.Day &&
                x.Hour == y.Hour &&
                x.Minute == y.Minute &&
                x.Second == y.Second;

        }



        [Test]
        public void testDeserializeTypeSimple() {

            string str = "{Id:2,  Name:\"诺基亚n78\", Weight:300, Owner:{Id:88,Name:\"ownerName\",Age:\"999\"}}";

            MyPhone phone = Json.Deserialize<MyPhone>( str );

            Assert.IsNotNull( phone );
            Assert.AreEqual( 88, phone.Owner.Id );
            Assert.AreEqual( "ownerName", phone.Owner.Name );
            Assert.AreEqual( "999", phone.Owner.Age );
        }


        [Test]
        public void testDeserializeListSimple() {

            String str = "  [1, 2, 3, 6, 8]  ";
            List<int> list = Json.DeserializeList<int>( str );
            Assert.AreEqual( 5, list.Count );
            Assert.AreEqual( 2, list[1] );
            Assert.AreEqual( 3, list[2] );
            Assert.AreEqual( 6, list[3] );
            Assert.AreEqual( 8, list[4] );

            str = "  [\"zhangsan\", \"lisi\", \"孙中山\", \"袁世凯\"]  ";
            List<String> strList = Json.DeserializeList<String>( str );
            Assert.AreEqual( 4, strList.Count );
            Assert.AreEqual( "zhangsan", strList[0] );
            Assert.AreEqual( "lisi", strList[1] );
            Assert.AreEqual( "孙中山", strList[2] );
            Assert.AreEqual( "袁世凯", strList[3] );

            str = @"
[
	{ Id:1, Name:""新闻大事001"", Weight:1 },
	{ Id:2, Name:""新闻大事002"", Weight:2 },
	{ Id:3, Name:""新闻大事003"", Weight:3 },
	{ Id:4, Name:""新闻大事004"", Weight:4 }
]";
            List<MyPhone> xlist = Json.DeserializeList<MyPhone>( str );
            Assert.AreEqual( 4, xlist.Count );

            Assert.AreEqual( 1, xlist[0].Id );
            Assert.AreEqual( "新闻大事001", xlist[0].Name );

            Assert.AreEqual( 2, xlist[1].Id );
            Assert.AreEqual( "新闻大事002", xlist[1].Name );

            Assert.AreEqual( 3, xlist[2].Id );
            Assert.AreEqual( "新闻大事003", xlist[2].Name );

            Assert.AreEqual( 4, xlist[3].Id );
            Assert.AreEqual( "新闻大事004", xlist[3].Name );

        }





        [Test]
        public void testDeserializeObject() {

            string json = @"{
  'Email': 'abcd@example.com',
  'Active': true,
  'CreatedDate': '2013-01-20',
  'Roles': [
    'User',
    'Admin'
  ]
}";

            Account account = Json.Deserialize<Account>( json );

            Assert.AreEqual( "abcd@example.com", account.Email );
            Assert.AreEqual( true, account.Active );

            Assert.AreEqual( 2, account.Roles.Count );
            Assert.AreEqual( "User", account.Roles[0] );
            Assert.AreEqual( "Admin", account.Roles[1] );
        }


        [Test]
        public void testReadDbConfig() {

            string str = @"{

    ConnectionStringTable : {
        default:""Provider=Microsoft.Jet.OLEDB.4.0;Data Source=wojilu.mdb"",
        db2:""server=localhost;uid=wojilusyy;pwd=test123;database=syyWojilu;""
    },
    
    IsCheckDatabase : true,
    MappingTablePrefix :"""",
    EnableContextCache : true,
    EnableApplicationCache : true,
    AssemblyList : [""wojilu.Core"",""wojilu.Apps""],
    MetaDLL : """"
    
}";

            MyDbConfig cf = Json.Deserialize<MyDbConfig>( str );

            Assert.IsNotNull( cf );

            Assert.AreEqual( 2, cf.ConnectionStringTable.Count );
            Assert.AreEqual( "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=wojilu.mdb", cf.ConnectionStringTable["default"] );
            Assert.AreEqual( "server=localhost;uid=wojilusyy;pwd=test123;database=syyWojilu;", cf.ConnectionStringTable["db2"] );

            Assert.AreEqual( 2, cf.AssemblyList.Count );
            Assert.AreEqual( "wojilu.Core", cf.AssemblyList[0] );
            Assert.AreEqual( "wojilu.Apps", cf.AssemblyList[1] );

            Assert.IsNull( cf.Interceptor );
        }


        [Test]
        public void testSetting() {

            String str = @"{ ""AllowComment"":1, ""AllowAnonymousComment"":1, ""EnableSubmit"":1, ""ListPostPerPage"":12, ""ListPicPerPage"":12, ""ListVideoPerPage"":15, ""RankPosts"":8, ""RankPics"":6, ""ArticleListMode"":0, ""SummaryLength"":150, ""StaticDir"":"""", ""MetaKeywords"":"""", ""MetaDescription"":"""", ""IsAutoHtml"":0, ""RankVideos"":6, ""CacheSeconds"":0 }";

            ContentSetting s = Json.Deserialize<ContentSetting>( str );
            Assert.AreEqual( 1, s.AllowComment );
            Assert.AreEqual( 12, s.ListPostPerPage );
            Assert.AreEqual( 150, s.SummaryLength );

            Assert.AreEqual( 12, s.ListPicPerPage );
            Assert.AreEqual( 15, s.ListVideoPerPage );
            Assert.AreEqual( 8, s.RankPosts );
            Assert.AreEqual( 6, s.RankPics );

            Assert.AreEqual( "", s.MetaKeywords );
            Assert.AreEqual( "", s.MetaDescription );

        }


    }
}
