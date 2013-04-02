using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Data;
using wojilu.ORM;

namespace wojilu.Test.Data {

    // 简单对象
    public class TCacheS : CacheObject {

        //base.Id
        //base.Name
        public String Address { get; set; }
        public Boolean IsDone { get; set; }
    }

    // 多种数据类型
    public class TCacheType : CacheObject {

        public String Address { get; set; }
        public Boolean IsDone { get; set; }

        public long DataLong { get; set; }
        public decimal DataDecimal { get; set; }
        public double DataDouble { get; set; }
        public DateTime Created { get; set; }
    }

    // 属性是数组
    public class TCacheArray : CacheObject {

        public String[] Address { get; set; }
        public Boolean[] IsDone { get; set; }
        public long[] DataLong { get; set; }
        public decimal[] DataDecimal { get; set; }
        public double[] DataDouble { get; set; }
        public DateTime[] Created { get; set; }
    }

    // 属性是List列表
    public class TCacheList : CacheObject {

        public List<String> Address { get; set; }
        public List<Boolean> IsDone { get; set; }
        public List<long> DataLong { get; set; }
        public List<decimal> DataDecimal { get; set; }
        public List<double> DataDouble { get; set; }
        public List<DateTime> Created { get; set; }
    }

    // 子对象测试
    public class TCacheSub : CacheObject {

        public String Address { get; set; }
        public Boolean IsDone { get; set; }

        public TCacheChild Sub { get; set; }
    }

    public class TCacheChild {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
    }


    public class TCurrency : CacheObject {

        public decimal ExchangeRate { get; set; }


    }


    public class TProperty : CacheObject {

        public String Address { get; set; }
        public Boolean IsDone { get; set; }

        // 只读
        public int IsRead {
            get { return 66; }
        }

        private int _isWrite = 1;

        // 只写
        public int IsWrite {
            set { _isWrite = value; }
        }

        [NotSave]
        public String Product { get; set; }


    }

}
