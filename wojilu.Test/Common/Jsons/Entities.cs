using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Data;
using wojilu.Serialization;

namespace wojilu.Test.Common.Jsons {

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
        public JsonObject ConnectionStringTable { get; set; }
        public List<object> AssemblyList { get; set; }
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
        public List<Object> Roles { get; set; }
    }

}
