using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;
using wojilu.Web.Mvc;
using wojilu;
using wojilu.Serialization;
using wojilu.Members.Users.Domain;

namespace wojilu.Test.Common.Jsons {


    // 本测试用于解析json字符串
    [TestFixture]
    public class JsonTest {












        [Test]
        public void testValue() {

            string json = "376";
            Assert.AreEqual( 376, JsonParser.Parse( json ) );

            json = " false   ";
            Assert.AreEqual( false, JsonParser.Parse( json ) );

            json = " 356   ";
            Assert.AreEqual( 356, JsonParser.Parse( json ) );

            json = " 3.17   ";
            Assert.AreEqual( 3.17, JsonParser.Parse( json ) );

            json = " dfaflddak_dfaol   ";
            Assert.AreEqual( "dfaflddak_dfaol", JsonParser.Parse( json ) );

            json = " dfaflddak=dfaol   ";
            Assert.AreEqual( "dfaflddak=dfaol", JsonParser.Parse( json ) );

            json = " dfaflddak#dfaol   ";
            Assert.AreEqual( "dfaflddak#dfaol", JsonParser.Parse( json ) );

            json = " dfaflddak;dfaol   ";
            Assert.AreEqual( "dfaflddak;dfaol", JsonParser.Parse( json ) );

            json = " dfaflddak\\\"dfaol   ";
            Assert.AreEqual( "dfaflddak\"dfaol", JsonParser.Parse( json ) );
        }

        [Test]
        public void testArray() {

            string json = "  [ \"zhangsan\", 3, false, \"lisi\"]  ";
            testArraySimple( json );

            testArraySimple2();

            json = "  [ \"zhangsan\", 3, false, { Name : \"zhangsan\",  Gender:\"male\"} ]  ";
            testObjectArray( json );
        }

        private void testObjectArray( string json ) {

            List<object> list = JsonParser.Parse( json ) as List<object>;
            Assert.AreEqual( 4, list.Count );
            Assert.AreEqual( "zhangsan", list[0] );
            Assert.AreEqual( 3, list[1] );
            Assert.AreEqual( false, list[2] );

            Dictionary<string, object> map = list[3] as Dictionary<string, object>;
            Assert.IsNotNull( map );

            Assert.AreEqual( 2, map.Count );
            foreach (KeyValuePair<string, object> pair in map) {
                Console.WriteLine( pair.Key + ":" + pair.Value );
            }

        }

        private void testArraySimple( string json ) {
            List<object> list = JsonParser.Parse( json ) as List<object>;
            Assert.AreEqual( 4, list.Count );
            Assert.AreEqual( "zhangsan", list[0] );
            Assert.AreEqual( 3, list[1] );
            Assert.AreEqual( false, list[2] );
            Assert.AreEqual( "lisi", list[3] );
        }

        private void testArraySimple2() {

            string json = " [erieroe  , 38  , lakkk]   ";
            List<object> list = JsonParser.Parse( json ) as List<object>;
            Assert.AreEqual( 3, list.Count );
            Assert.AreEqual( "erieroe", list[0] );
            Assert.AreEqual( 38, list[1] );
            Assert.AreEqual( "lakkk", list[2] );

        }

        [Test]
        public void testArraySimple3() {
            string json = "  [ \"zhang\\\"san\", 3, false, \"lisi\"]  ";

            List<object> list = JsonParser.Parse( json ) as List<object>;
            Assert.AreEqual( 4, list.Count );
            Assert.AreEqual( "zhang\"san", list[0] );
            Assert.AreEqual( 3, list[1] );
            Assert.AreEqual( false, list[2] );
            Assert.AreEqual( "lisi", list[3] );
        }


        [Test]
        public void testObject() {

            string json = "{ \"Name\" : \"zhangsan\",  \"Gender\":\"male\"}";
            testObjectPrivate( json );

            json = "{ Name : \"zhangsan\",  Gender:\"male\"}";
            testObjectPrivate( json );

            json = "{ \"Name\" : \"zhangsan\",  \"Gender\":\"male\", \"Address\" : { \"Name\":\"changan\", \"zip\":\"100000\"  } }";
            testsecondObject( json );

            json = "{ \"Name\" : \"zhangsan\",  \"Gender\":\"male\", \"Address\" : { \"Name\":{\"cname\":\"长安\", \"ename\":\"changan\"}, \"zip\":\"100000\"  } }";
            testthirdObject( json );
        }

        private void testObjectPrivate( string json ) {

            Dictionary<string, object> map = JsonParser.Parse( json ) as Dictionary<string, object>;

            Assert.AreEqual( 2, map.Count );

            foreach (KeyValuePair<string, object> pair in map) {
                Console.WriteLine( pair.Key + ":" + pair.Value );
            }
            Console.WriteLine( "------------------------------------------------" );
        }

        private void testsecondObject( string json ) {

            Dictionary<string, object> map = JsonParser.Parse( json ) as Dictionary<string, object>;

            Assert.AreEqual( 3, map.Count );

            Assert.IsTrue( map.ContainsKey( "Address" ) );
            Dictionary<string, object> obj = map["Address"] as Dictionary<string, object>;
            Assert.IsNotNull( obj );

            Assert.AreEqual( 2, obj.Count );

            foreach (KeyValuePair<string, object> pair in obj) {
                Console.WriteLine( pair.Key + ":" + pair.Value );
            }
            Console.WriteLine( "------------------------------------------------" );

        }

        private void testthirdObject( string json ) {

            Dictionary<string, object> map = JsonParser.Parse( json ) as Dictionary<string, object>;

            Assert.AreEqual( 3, map.Count );

            Assert.IsTrue( map.ContainsKey( "Address" ) );
            Dictionary<string, object> addr = map["Address"] as Dictionary<string, object>;
            Assert.IsNotNull( addr );

            Assert.AreEqual( 2, addr.Count );

            Assert.IsTrue( addr.ContainsKey( "Name" ) );
            Dictionary<string, object> name = addr["Name"] as Dictionary<string, object>;
            Assert.IsNotNull( name );

            Assert.AreEqual( 2, name.Count );


            foreach (KeyValuePair<string, object> pair in name) {
                Console.WriteLine( pair.Key + ":" + pair.Value );
            }
            Console.WriteLine( "------------------------------------------------" );
        }

        [Test]
        public void StringToObject() {

            string str = "{Id:2,  Name:\"诺基亚n78\", Weight:300, Owner:6}";

            object obj = JSON.ToObject( str, typeof( MyPhone ) );
            Assert.IsNotNull( obj );
            MyPhone phone = obj as MyPhone;
            Assert.IsNotNull( phone );
            Assert.AreEqual( 6, phone.Owner.Id );

        }


        [Test]
        public void JsonStringToList() {
            string result = @"
[
	{ Id:0, Name:""新闻大事690501468"", Weight:0, Owner:""2"" },
	{ Id:1, Name:""新闻大事690501468"", Weight:0, Owner:""2"" },
	{ Id:2, Name:""新闻大事690501468"", Weight:0, Owner:""2"" },
	{ Id:3, Name:""新闻大事690501468"", Weight:0, Owner:""2"" }
]
";
            List<MyPhone> list = JSON.ToList<MyPhone>( result );
            Assert.AreEqual( 4, list.Count );

            for (int i = 0; i < list.Count; i++) {
                MyPhone phone = list[i] as MyPhone;
                Assert.AreEqual( i, phone.Id );
            }

        }

        [Test]
        public void StringToHashtable2() {
            string str = @"
[
{ Id:3, Name:""zhangsan"", Age:25 },
{ Id:5, Name:""lisi"", Age:18 }
]
";

            //object obj = JsonParser.Parse( str );

            List<Dictionary<string, object>> lists = JSON.ToDictionaryList( str );
            Assert.AreEqual( 2, lists.Count );

            Dictionary<string, object> dic = lists[0];
            Assert.AreEqual( 3, dic.Keys.Count );
            Assert.AreEqual( 3, dic["Id"] );
            Assert.AreEqual( "zhangsan", dic["Name"] );
            Assert.AreEqual( 25, dic["Age"] );

            Dictionary<string, object> dic2 = lists[1];
            Assert.AreEqual( 5, dic2["Id"] );
            Assert.AreEqual( "lisi", dic2["Name"] );
            Assert.AreEqual( 18, dic2["Age"] );


        }

        [Test]
        public void StringToHashtable() {
            string str = @"

[

       { Id:3, Name:""zhangsan"", Age:25 }  
]
";

            Dictionary<string, object> dic = JSON.ToDictionary( str );

            Assert.AreEqual( 3, dic.Keys.Count );
            Assert.AreEqual( 3, dic["Id"] );
            Assert.AreEqual( "zhangsan", dic["Name"] );
            Assert.AreEqual( 25, dic["Age"] );

        }



        [Test]
        public void testDbConfig() {
            string str = @"{

    ConnectionString : {
        default:""Provider=Microsoft.Jet.OLEDB.4.0;Data Source=wojilu.mdb"",
        db2:""server=localhost;uid=wojilu;pwd=abcd123;database=wojilu;""
    },
    
    IsCheckDatabase : true,
    MappingTablePrefix :"""",
    EnableContextCache : true,
    EnableApplicationCache : true,
    AssemblyList : [""wojilu.Core"",""wojilu.Apps""],
    MetaDLL : """"
    
}";
            Dictionary<string, object> dic = JsonParser.Parse( str ) as Dictionary<string, object>;
            Assert.AreEqual( 7, dic.Count );

            Assert.AreEqual( dic["IsCheckDatabase"], true );
            Assert.AreEqual( dic["MappingTablePrefix"], "" );
            Assert.AreEqual( dic["EnableContextCache"], true );
            Assert.AreEqual( dic["EnableApplicationCache"], true );
            Assert.AreEqual( dic["MetaDLL"], "" );

            List<object> list = dic["AssemblyList"] as List<object>;
            Assert.IsNotNull( list );
            Assert.AreEqual( list.Count, 2 );
            Assert.AreEqual( list[0], "wojilu.Core" );
            Assert.AreEqual( list[1], "wojilu.Apps" );

            Dictionary<string, object> objcn = dic["ConnectionString"] as Dictionary<string, object>;
            Assert.IsNotNull( objcn );
            Assert.AreEqual( objcn["default"], "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=wojilu.mdb" );
            Assert.AreEqual( objcn["db2"], "server=localhost;uid=wojilu;pwd=abcd123;database=wojilu;" );
        }

        [Test]
        public void testDbConfigExpress() {
            string str = @"{ 
ConnectionStringTable : {
default:""Server = ./sqlexpress;uid=sa;pwd=gl;database=wojilu;Pooling=true;""
}, 

DbType : { 
default:""SqlServer"" 
}, 

AssemblyList : [""wojilu.Core"",""wojilu.Apps""] 
} ";
            Dictionary<string, object> dic = JsonParser.Parse( str ) as Dictionary<string, object>;

            List<object> list = dic["AssemblyList"] as List<object>;
            Assert.IsNotNull( list );
            Assert.AreEqual( list.Count, 2 );
            Assert.AreEqual( list[0], "wojilu.Core" );
            Assert.AreEqual( list[1], "wojilu.Apps" );

            Dictionary<string, object> objcn = dic["ConnectionStringTable"] as Dictionary<string, object>;
            Assert.IsNotNull( objcn );
            Assert.AreEqual( objcn["default"], "Server = ./sqlexpress;uid=sa;pwd=gl;database=wojilu;Pooling=true;" );
        }

        [Test]
        public void testDbConfigWithPort() {
            string str = @"{ 
ConnectionStringTable : {
default:""server=192.168.15.121:1433;uid=test;pwd=test;database=mydb;""
}, 

DbType : { 
default:""SqlServer"" 
}, 

AssemblyList : [""wojilu.Core"",""wojilu.Apps""] 
} ";
            Dictionary<string, object> dic = JsonParser.Parse( str ) as Dictionary<string, object>;

            List<object> list = dic["AssemblyList"] as List<object>;
            Assert.IsNotNull( list );
            Assert.AreEqual( list.Count, 2 );
            Assert.AreEqual( list[0], "wojilu.Core" );
            Assert.AreEqual( list[1], "wojilu.Apps" );

            Dictionary<string, object> objcn = dic["ConnectionStringTable"] as Dictionary<string, object>;
            Assert.IsNotNull( objcn );
            Assert.AreEqual( objcn["default"], "server=192.168.15.121:1433;uid=test;pwd=test;database=mydb;" );
        }


        [Test]
        public void testONe() {

            string str = "{topic: \"<a href='/bv/Forum1/Topic/Show/4440.aspx'>亚裔美国文学研究的新起点</a>\"}";

            Dictionary<string, object> dic = JsonParser.Parse( str ) as Dictionary<string, object>;
            Assert.AreEqual( 1, dic.Count );

            Assert.AreEqual( "<a href='/bv/Forum1/Topic/Show/4440.aspx'>亚裔美国文学研究的新起点</a>", dic["topic"] );
        }

        [Test]
        public void testMultiline() {

            string str = @"
{
    
    name : ""sunzhongshan"",
    age : 99,
    gender : ""male""

}
";
            Dictionary<string, object> dic = JsonParser.Parse( str ) as Dictionary<string, object>;
            Assert.AreEqual( 3, dic.Count );
            Assert.AreEqual( dic["name"], "sunzhongshan" );
            Assert.AreEqual( dic["age"], 99 );
            Assert.AreEqual( dic["gender"], "male" );
        }

        [Test]
        public void testMultiObjects() {
            string str = @"
{
    
    name : [""sunwen"",  ""袁世凯""],
    age : 99,
    gender : ""male""

}
";
            Dictionary<string, object> dic = JsonParser.Parse( str ) as Dictionary<string, object>;
            Assert.AreEqual( 3, dic.Count );

            object obj = dic["name"];

            List<object> list = dic["name"] as List<object>;
            Assert.IsNotNull( list );
            Assert.AreEqual( list.Count, 2 );
            Assert.AreEqual( list[0], "sunwen" );
            Assert.AreEqual( list[1], "袁世凯" );

            Assert.AreEqual( dic["age"], 99 );
            Assert.AreEqual( dic["gender"], "male" );
        }





        [Test]
        public void testEmotions() {
            String ems = @"{
    	'$001':'微笑','$002':'大笑','$003':'抛媚眼','$004':'惊讶','$005':'吐舌头、扮鬼脸','$006':'热烈','$007':'生气、黑脸','$008':'困惑','$009':'尴尬','$010':'悲伤',
    	'$011':'狂笑','$012':'晕、难以理解','$013':'扮酷','$014':'吐','$015':'偷笑','$016':'色、流口水','$017':'hoho','$018':'咬牙切齿','$019':'悄悄话','$020':'撞墙',
    	'$021':'大哭','$022':'书呆子','$023':'打哈欠','$024':'砸、敲头','$025':'汗','$026':'拍手','$027':'狂怒、砍人','$028':'捂嘴偷笑','$029':'不是、不要','$030':'orz (失意体前屈, 或五体投地)',
    	'$031':'厉害、牛、强','$032':'差劲、弱','$033':'握手、或英雄所见略同','$034':'竖起中指(fuck you)','$035':'OK、好的','$036':'女孩','$037':'男孩','$038':'左侧拥抱','$039':'右侧拥抱','$040':'骷髅头',
    	'$041':'爱你、真心','$042':'心碎','$043':'红唇、飞吻','$044':'鲜花、玫瑰','$045':'花儿凋零','$046':'沉睡的月亮、或晚安','$047':'星星','$048':'太阳','$049':'彩虹','$050':'雨伞',
    	'$051':'咖啡','$052':'蛋糕','$053':'音乐','$054':'电影','$055':'电视、视频','$056':'汽车','$057':'飞机','$058':'照相机','$059':'时钟','$060':'礼物',
    	'$061':'狗狗','$062':'小猫','$063':'猪头','$064':'蜗牛','$065':'岛屿','$066':'足球','$067':'电话','$068':'灯泡','$069':'臭大粪、shit'
    }";

            Dictionary<string, object> map = JsonParser.Parse( ems ) as Dictionary<string, object>;
            Assert.IsNotNull( map );
            Assert.AreEqual( 69, map.Count );



        }


        private string encode( string str ) {
            if (strUtil.IsNullOrEmpty( str )) return str;
            return str.Replace( ":", @"\:" ).Replace( "\"", "\\\"" ).Replace( ",", "\\," ).Replace( "'", "\\'" );
        }

        [Test]
        public void testEncode() {

            string str = "can't find matching route : Url=_vti_bin/owssvr.dl";

            string result = encode( str );
            string expected = "can\\'t find matching route \\: Url=_vti_bin/owssvr.dl";
            Assert.AreEqual( expected, result );
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

            MyDbConfig cf = JSON.ToObject<MyDbConfig>( str );

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
        public void testQuote() {
            string str = "{post: \"<a href='/bv/Forum1/Post/Show/17355.aspx'>re:天下之大，无奇不有</a>\"}";
            Dictionary<string, object> dic = JSON.ToDictionary( str );
            Assert.AreEqual( 1, dic.Count );
            Assert.AreEqual( "<a href='/bv/Forum1/Post/Show/17355.aspx'>re:天下之大，无奇不有</a>", dic["post"] );
        }

        [Test]
        public void testDic() {

            //string str = @"{ blog:""<a href='/space/sgzwiz/Blog574/Post/95'>\framework\views\Common\Admin\AppBase\</a>"" }";
            string str = @"{ blog:""<a href=\""/space/sgzwiz/Blog574/Post/95\"">\\framework\\views\\Common\\Admin\\AppBase\\</a>"" }";
            Dictionary<string, object> dic = JSON.ToDictionary( str );
            Assert.AreEqual( 1, dic.Count );

        }

        [Test]
        public void testVarCount() {

            string str = "{#actor#} 发表了 {#count#} 篇日志，最近的是 {#blogTitle#}";

            int scount = getVarCount( str );
            Assert.AreEqual( 3, scount );

        }

        private static int getVarCount( string str ) {
            char[] arrChar = str.ToCharArray();
            int scount = 0;

            for (int i = 0; i < arrChar.Length; i++) {
                if (i == 0) continue;
                if (arrChar[i - 1] == '{' && arrChar[i] == '#') scount++;
            }
            return scount;
        }

    }



}
