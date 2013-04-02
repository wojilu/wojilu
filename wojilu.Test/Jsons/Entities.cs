using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Data;
using wojilu.Serialization;

namespace wojilu.Test.Common.Jsons {

    public class TPhone {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
    }

    public class TJEntity {

        // 支持 7 种基础类型 int, string, decimal, long, double, bool, DateTime
        // 其中 long 会被序列化为字符串
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TMoney { get; set; }
        public long Area { get; set; }
        public double TDouble { get; set; }
        public bool IsBlack { get; set; }
        public DateTime Created { get; set; }

        // 支持其他对象
        public TPhone TPhone { get; set; }

        // 也支持原始 JsonObject 对象
        public JsonObject JsonObject { get; set; }

        // 支持List泛型
        public List<int> Id_List { get; set; }
        public List<string> Name_List { get; set; }
        public List<decimal> TMoney_List { get; set; }
        public List<long> Area_List { get; set; }
        public List<double> TValue_List { get; set; }
        public List<bool> IsBlack_List { get; set; }
        public List<DateTime> Created_List { get; set; }

        // 支持数组
        public int[] IntArray { get; set; }
        public TPhone[] TPhone_Array { get; set; }

        // 支持List泛型
        public List<TPhone> TPhone_List { get; set; }

        // 支持Dictionary泛型，但key必须是String类型
        public Dictionary<String, TPhone> TPhone_Dic { get; set; }


    }



    public class PhoneOwner {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
    }

    public class MyPhone {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public PhoneOwner Owner { get; set; }
    }

    // 注意：为了能正常反序列化，Dictionary的value只能是object类型，List的项也必须是object类型
    public class MyDbConfig {
        public Dictionary<String,String> ConnectionStringTable { get; set; }
        public List<String> AssemblyList { get; set; }
        public bool IsCheckDatabase { get; set; }
        public string MappingTablePrefix { get; set; }
        public bool EnableContextCache { get; set; }
        public bool EnableApplicationCache { get; set; }
        public bool IsSqlServer2000 { get; set; }
        public string MetaDLL { get; set; }

        public List<object> Interceptor { get; set; }
    }


    public class Account {
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<String> Roles { get; set; }
    }

}
