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
    public class JsonDeserializeTest {









        [Test]
        public void testCommon() {

            // 解析对象
            string str = @"
{    
    name : ['蒋介石', 'Chiang Kai-shek',  '常凯申'],
    age : 125,
    gender : 'male',
    hasGun : true,
    birthday : '1887-10-31',
    wife : {
        name : '宋美龄',
        birthday : '1897-3-5'
    },
    friends : [
        {name:'sun', address:'xxxxxxxxxxxxx1' },
        {name:'mao', address:'xxxxxxxxxxxxx2' },
        {name:'dai', address:'xxxxxxxxxxxxx3' }
    ],
    other : 993439419349934,
    other2 : 88.99,
    other3 : [ 123, 'kkkkk',  {p1:'p1-value', p2:'p2-value' }]
}
";

            JsonObject obj = Json.ParseJson( str );
            // 等同于 JsonObject obj = Json.Deserialize( str ) as JsonObject;

            List<String> names = obj.GetList<String>( "name" );
            Assert.AreEqual( 3, names.Count );
            Assert.AreEqual( "蒋介石", names[0] );
            Assert.AreEqual( "Chiang Kai-shek", names[1] );
            Assert.AreEqual( "常凯申", names[2] );

            Assert.AreEqual( 125, obj.Get<int>( "age" ) );

            Assert.AreEqual( "male", obj.Get( "gender" ) );
            Assert.IsTrue( obj.Get<bool>( "hasGun" ) );

            DateTime birthday = obj.Get<DateTime>( "birthday" );
            Assert.AreEqual( 1887, birthday.Year );
            Assert.AreEqual( 10, birthday.Month );
            Assert.AreEqual( 31, birthday.Day );

            JsonObject wife1 = obj.GetJson( "wife" );
            Assert.AreEqual( "宋美龄", wife1.Get( "name" ) );
            // GetJson( "wife" ) 等效于 Get<JsonObject>( "wife" )
            JsonObject wife2 = obj.Get<JsonObject>( "wife" );
            Assert.AreEqual( "宋美龄", wife2.Get( "name" ) );

            DateTime wifeBirthday = wife1.Get<DateTime>( "birthday" );

            Assert.AreEqual( 1897, wifeBirthday.Year );
            Assert.AreEqual( 3, wifeBirthday.Month );
            Assert.AreEqual( 5, wifeBirthday.Day );

            List<JsonObject> friends = obj.GetList<JsonObject>( "friends" );

            Assert.AreEqual( "sun", friends[0].Get( "name" ) );
            Assert.AreEqual( "xxxxxxxxxxxxx1", friends[0].Get( "address" ) );

            Assert.AreEqual( "mao", friends[1].Get( "name" ) );
            Assert.AreEqual( "xxxxxxxxxxxxx2", friends[1].Get( "address" ) );

            Assert.AreEqual( "dai", friends[2].Get( "name" ) );
            Assert.AreEqual( "xxxxxxxxxxxxx3", friends[2].Get( "address" ) );

            Assert.AreEqual( 993439419349934, obj.Get<long>( "other" ) );
            Assert.AreEqual( 88.99, obj.Get<decimal>( "other2" ) );

            // 因为列表内的数据类型不一致 ，所以此处只能GetList() 而不是泛型方法 GetList<T>()
            List<Object> list3 = obj.GetList( "other3" );
            Assert.AreEqual( 123, list3[0] );
            Assert.AreEqual( "kkkkk", list3[1] );

            JsonObject obj3 = list3[2] as JsonObject;
            Assert.AreEqual( "p1-value", obj3.Get( "p1" ) );
            Assert.AreEqual( "p2-value", obj3.Get( "p2" ) );


            // 解析数组
            str = "[ 'xxx1', 88, false, {name:'孙中山', gender:'male' }]";
            List<Object> list = Json.ParseList( str );
            //List<Object> list = Json.Deserialize( str ) as List<Object>;

            Assert.AreEqual( 4, list.Count );
            Assert.AreEqual( "xxx1", list[0] );
            Assert.AreEqual( 88, list[1] );
            Assert.AreEqual( false, list[2] );

            JsonObject j = list[3] as JsonObject;
            Assert.AreEqual( "孙中山", j.Get( "name" ) );
            Assert.AreEqual( "male", j.Get( "gender" ) );

            // 解析其它类型
            string json = "376";
            Assert.AreEqual( 376, Json.Parse( json ) );

            json = " false";
            Assert.AreEqual( false, Json.Parse( json ) );
        }

        // ParseJson 等效于 Deserialize<JsonObject>
        // ParseList<JsonObject> 等效于 DeserializeList<JsonObject>
        [Test]
        public void testDeserializeAndParseEqual() {

            String str = "{ \"Name\" : \"zhangsan\",  \"Gender\":\"male\", \"Age\":66,  \"Address\" : { \"Name\":\"changan\", \"zip\":\"100000\"  } }";

            JsonObject obj1 = Json.ParseJson( str );
            JsonObject obj2 = Json.Deserialize<JsonObject>( str );

            Assert.AreEqual( obj1.Get( "Name" ), "zhangsan" );
            Assert.AreEqual( obj1.Get( "Name" ), obj2.Get( "Name" ) );

            Assert.AreEqual( obj1.Get( "Gender" ), "male" );
            Assert.AreEqual( obj1.Get( "Gender" ), obj2.Get( "Gender" ) );

            Assert.AreEqual( obj1.Get<int>( "Age" ), 66 );
            Assert.AreEqual( obj1.Get<int>( "Age" ), obj2.Get<int>( "Age" ) );

            JsonObject addr1 = obj1.GetJson( "Address" );
            JsonObject addr2 = obj2.GetJson( "Address" );

            Assert.AreEqual( addr1.Get( "Name" ), "changan" );
            Assert.AreEqual( addr1.Get( "Name" ), addr2.Get( "Name" ) );

            Assert.AreEqual( addr1.Get( "zip" ), "100000" );
            Assert.AreEqual( addr1.Get( "zip" ), addr2.Get( "zip" ) );


            //---------------------------------------------------------------------


            // list object test
            str = @"
[
{ Id:3, Name:""zhangsan"", Age:25 },
{ Id:5, Name:""lisi"", Age:18 }
]
";

            List<JsonObject> list1 = Json.ParseList<JsonObject>( str );
            List<JsonObject> list2 = Json.DeserializeList<JsonObject>( str );

            Assert.AreEqual( 2, list1.Count );
            Assert.AreEqual( 2, list2.Count );
            //-------------------

            Assert.AreEqual( 3, list1[0].Get<int>( "Id" ) );
            Assert.AreEqual( 3, list2[0].Get<int>( "Id" ) );

            Assert.AreEqual( "zhangsan", list1[0].Get( "Name" ) );
            Assert.AreEqual( "zhangsan", list2[0].Get( "Name" ) );

            Assert.AreEqual( 25, list1[0].Get<int>( "Age" ) );
            Assert.AreEqual( 25, list2[0].Get<int>( "Age" ) );

            //-------------------
            Assert.AreEqual( 5, list1[1].Get<int>( "Id" ) );
            Assert.AreEqual( 5, list2[1].Get<int>( "Id" ) );

            Assert.AreEqual( "lisi", list1[1].Get( "Name" ) );
            Assert.AreEqual( "lisi", list2[1].Get( "Name" ) );

            Assert.AreEqual( 18, list1[1].Get<int>( "Age" ) );
            Assert.AreEqual( 18, list2[1].Get<int>( "Age" ) );



        }


        [Test]
        public void testValue() {

            string json = "376";
            Assert.AreEqual( 376, Json.Parse( json ) );

            json = " false   ";
            Assert.AreEqual( false, Json.Parse( json ) );

            json = " 356   ";
            Assert.AreEqual( 356, Json.Parse( json ) );

            json = " 3.17   ";
            Assert.AreEqual( 3.17, Json.Parse( json ) );

            json = "993439419349934";
            Assert.AreEqual( 993439419349934, Json.Parse( json ) );


            json = " dfaflddak_dfaol   ";
            Assert.AreEqual( "dfaflddak_dfaol", Json.Parse( json ) );

            json = " dfaflddak=dfaol   ";
            Assert.AreEqual( "dfaflddak=dfaol", Json.Parse( json ) );

            json = " dfaflddak#dfaol   ";
            Assert.AreEqual( "dfaflddak#dfaol", Json.Parse( json ) );

            json = " dfaflddak;dfaol   ";
            Assert.AreEqual( "dfaflddak;dfaol", Json.Parse( json ) );

            json = " dfaflddak\\\"dfaol   ";
            Assert.AreEqual( "dfaflddak\"dfaol", Json.Parse( json ) );
        }

        [Test]
        public void testArray() {

            string json = "  [ \"zhangsan\", 3, false, \"lisi\"]  ";
            testArraySimple( json );

            testArraySimple2();

            json = "  [ \"zhangsan\", 3, false, { Name : \"zhangsan\",  Gender:\"male\"} ]  ";
            testObjectArray( json );

            json = "[ 'abc', 'def', 'xxxxxxxxx', 'hhhhhhhhhhh3' ]";
            List<String> strList = Json.DeserializeList<String>( json );
            Assert.AreEqual( "abc", strList[0] );
            Assert.AreEqual( "def", strList[1] );
            Assert.AreEqual( "xxxxxxxxx", strList[2] );
            Assert.AreEqual( "hhhhhhhhhhh3", strList[3] );

        }

        private void testObjectArray( string json ) {

            List<Object> list = Json.ParseList( json );
            Assert.AreEqual( 4, list.Count );
            Assert.AreEqual( "zhangsan", list[0] );
            Assert.AreEqual( 3, list[1] );
            Assert.AreEqual( false, list[2] );

            JsonObject obj = list[3] as JsonObject;
            Assert.IsNotNull( obj );

            Assert.AreEqual( 2, obj.Count );

            Assert.AreEqual( "zhangsan", obj.Get( "Name" ) );
            Assert.AreEqual( "male", obj.Get( "Gender" ) );


        }

        private void testArraySimple( string json ) {
            List<Object> list = Json.ParseList( json );
            Assert.AreEqual( 4, list.Count );
            Assert.AreEqual( "zhangsan", list[0] );
            Assert.AreEqual( 3, list[1] );
            Assert.AreEqual( false, list[2] );
            Assert.AreEqual( "lisi", list[3] );
        }

        private void testArraySimple2() {

            string json = " [erieroe  , 38  , lakkk]   ";
            List<Object> list = Json.ParseList( json );
            Assert.AreEqual( 3, list.Count );
            Assert.AreEqual( "erieroe", list[0] );
            Assert.AreEqual( 38, list[1] );
            Assert.AreEqual( "lakkk", list[2] );

        }

        [Test]
        public void testArraySimple3() {
            string json = "  [ \"zhang\\\"san\", 3, false, \"lisi\"]  ";

            List<Object> list = Json.ParseList( json );
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

            JsonObject map = Json.ParseJson( json );

            Assert.AreEqual( 2, map.Count );

            Assert.AreEqual( "zhangsan", map.Get( "Name" ) );
            Assert.AreEqual( "male", map.Get( "Gender" ) );


            foreach (KeyValuePair<string, Object> pair in map) {
                Console.WriteLine( pair.Key + ":" + pair.Value );
            }
            Console.WriteLine( "------------------------------------------------" );
        }

        private void testsecondObject( string json ) {

            JsonObject map = Json.ParseJson( json );

            Assert.AreEqual( 3, map.Count );

            Assert.IsTrue( map.ContainsKey( "Address" ) );
            JsonObject obj = map.GetJson( "Address" );
            Assert.IsNotNull( obj );

            Assert.AreEqual( 2, obj.Count );

            foreach (KeyValuePair<string, Object> pair in obj) {
                Console.WriteLine( pair.Key + ":" + pair.Value );
            }
            Console.WriteLine( "------------------------------------------------" );

        }

        private void testthirdObject( string json ) {

            JsonObject map = Json.ParseJson( json );

            Assert.AreEqual( 3, map.Count );

            Assert.IsTrue( map.ContainsKey( "Address" ) );
            JsonObject addr = map["Address"] as JsonObject;
            Assert.IsNotNull( addr );

            Assert.AreEqual( 2, addr.Count );

            Assert.IsTrue( addr.ContainsKey( "Name" ) );
            JsonObject name = addr["Name"] as JsonObject;
            Assert.IsNotNull( name );

            Assert.AreEqual( 2, name.Count );


            foreach (KeyValuePair<string, Object> pair in name) {
                Console.WriteLine( pair.Key + ":" + pair.Value );
            }
            Console.WriteLine( "------------------------------------------------" );
        }


        [Test]
        public void testParseTypedList() {
            string str = @"
[
{ Id:3, Name:""zhangsan"", Age:25 },
{ Id:5, Name:""lisi"", Age:18 }
]
";

            List<JsonObject> lists = Json.ParseList<JsonObject>( str );
            Assert.AreEqual( 2, lists.Count );

            // 弱类型获取值
            JsonObject dic = lists[0];
            Assert.AreEqual( 3, dic.Keys.Count );
            Assert.AreEqual( 3, dic["Id"] );
            Assert.AreEqual( "zhangsan", dic["Name"] );
            Assert.AreEqual( 25, dic["Age"] );

            // 强类型获取值
            Assert.AreEqual( 3, dic.Get<int>( "Id" ) );
            Assert.AreEqual( "zhangsan", dic.Get( "Name" ) );
            Assert.AreEqual( 25, dic.Get<int>( "Age" ) );

            JsonObject dic2 = lists[1];
            Assert.AreEqual( 5, dic2.Get<int>( "Id" ) );
            Assert.AreEqual( "lisi", dic2.Get( "Name" ) );
            Assert.AreEqual( 18, dic2.Get<int>( "Age" ) );


            str = @"[3, 5, 6, 99]";
            List<int> intList = Json.ParseList<int>( str );
            Assert.AreEqual( 3, intList[0] );
            Assert.AreEqual( 5, intList[1] );
            Assert.AreEqual( 6, intList[2] );
            Assert.AreEqual( 99, intList[3] );

            str = "  [  \"zhangsan\",  \"lisi\", \"fname2\", \"zname88\" ] ";
            List<String> strList = Json.ParseList<String>( str );
            Assert.AreEqual( "zhangsan", strList[0] );
            Assert.AreEqual( "lisi", strList[1] );
            Assert.AreEqual( "fname2", strList[2] );
            Assert.AreEqual( "zname88", strList[3] );

        }


        [Test]
        public void StringToHashtable() {
            string str = @"



       { Id:3, Name:""zhangsan"", Age:25 }  

";

            JsonObject dic = Json.ParseJson( str );

            Assert.AreEqual( 3, dic.Keys.Count );
            Assert.AreEqual( 3, dic["Id"] );
            Assert.AreEqual( "zhangsan", dic["Name"] );
            Assert.AreEqual( 25, dic["Age"] );

            // 强类型获取值
            Assert.AreEqual( 3, dic.Get<int>( "Id" ) );
            Assert.AreEqual( "zhangsan", dic.Get( "Name" ) );
            Assert.AreEqual( 25, dic.Get<int>( "Age" ) );

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
            JsonObject obj = Json.ParseJson( str );
            Assert.AreEqual( 7, obj.Count );

            Assert.AreEqual( obj.Get<bool>( "IsCheckDatabase" ), true );
            Assert.AreEqual( obj.Get( "MappingTablePrefix" ), "" );
            Assert.AreEqual( obj.Get<bool>( "EnableContextCache" ), true );
            Assert.AreEqual( obj.Get<bool>( "EnableApplicationCache" ), true );
            Assert.AreEqual( obj.Get( "MetaDLL" ), "" );

            List<Object> list = obj.GetList( "AssemblyList" );
            Assert.IsNotNull( list );
            Assert.AreEqual( list.Count, 2 );
            Assert.AreEqual( list[0], "wojilu.Core" );
            Assert.AreEqual( list[1], "wojilu.Apps" );

            JsonObject objcn = obj.GetJson( "ConnectionString" );
            Assert.IsNotNull( objcn );
            Assert.AreEqual( objcn.Get( "default" ), "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=wojilu.mdb" );
            Assert.AreEqual( objcn.Get( "db2" ), "server=localhost;uid=wojilu;pwd=abcd123;database=wojilu;" );
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
            JsonObject obj = Json.ParseJson( str );

            List<Object> list = obj.GetList( "AssemblyList" );
            Assert.IsNotNull( list );
            Assert.AreEqual( list.Count, 2 );
            Assert.AreEqual( list[0], "wojilu.Core" );
            Assert.AreEqual( list[1], "wojilu.Apps" );

            JsonObject objcn = obj.GetJson( "ConnectionStringTable" );
            Assert.IsNotNull( objcn );
            Assert.AreEqual( objcn.Get( "default" ), "Server = ./sqlexpress;uid=sa;pwd=gl;database=wojilu;Pooling=true;" );
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
            JsonObject obj = Json.ParseJson( str );

            List<Object> list = obj.GetList( "AssemblyList" );
            Assert.IsNotNull( list );
            Assert.AreEqual( list.Count, 2 );
            Assert.AreEqual( list[0], "wojilu.Core" );
            Assert.AreEqual( list[1], "wojilu.Apps" );

            JsonObject objcn = obj.GetJson( "ConnectionStringTable" );
            Assert.IsNotNull( objcn );
            Assert.AreEqual( objcn["default"], "server=192.168.15.121:1433;uid=test;pwd=test;database=mydb;" );
        }


        [Test]
        public void testONe() {

            string str = "{topic: \"<a href='/bv/Forum1/Topic/Show/4440.aspx'>亚裔美国文学研究的新起点</a>\"}";

            JsonObject dic = Json.ParseJson( str );
            Assert.AreEqual( 1, dic.Count );

            Assert.AreEqual( "<a href='/bv/Forum1/Topic/Show/4440.aspx'>亚裔美国文学研究的新起点</a>", dic.Get( "topic" ) );
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
            JsonObject obj = Json.ParseJson( str );
            Assert.AreEqual( 3, obj.Count );
            Assert.AreEqual( obj.Get( "name" ), "sunzhongshan" );
            Assert.AreEqual( obj.Get<int>( "age" ), 99 );
            Assert.AreEqual( obj.Get( "gender" ), "male" );
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
            JsonObject obj = Json.ParseJson( str );
            Assert.AreEqual( 3, obj.Count );

            List<Object> list = obj.GetList( "name" );

            Assert.IsNotNull( list );
            Assert.AreEqual( list.Count, 2 );
            Assert.AreEqual( list[0], "sunwen" );
            Assert.AreEqual( list[1], "袁世凯" );

            Assert.AreEqual( obj.Get<int>( "age" ), 99 );
            Assert.AreEqual( obj.Get( "gender" ), "male" );
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

            JsonObject objem = Json.ParseJson( ems );
            Assert.IsNotNull( objem );
            Assert.AreEqual( 69, objem.Count );

            Assert.AreEqual( "岛屿", objem.Get( "$065" ) );
            Assert.AreEqual( "灯泡", objem.Get( "$068" ) );


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
        public void testQuote() {
            string str = "{post: \"<a href='/bv/Forum1/Post/Show/17355.aspx'>re:天下之大，无奇不有</a>\"}";
            JsonObject dic = Json.ParseJson( str );
            Assert.AreEqual( 1, dic.Count );
            Assert.AreEqual( "<a href='/bv/Forum1/Post/Show/17355.aspx'>re:天下之大，无奇不有</a>", dic["post"] );
        }

        [Test]
        public void testDic() {

            //string str = @"{ blog:""<a href='/space/sgzwiz/Blog574/Post/95'>\framework\views\Common\Admin\AppBase\</a>"" }";
            string str = @"{ blog:""<a href=\""/space/sgzwiz/Blog574/Post/95\"">\\framework\\views\\Common\\Admin\\AppBase\\</a>"" }";
            JsonObject dic = Json.ParseJson( str );
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
