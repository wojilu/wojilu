using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.ORM {

    
    /// <summary>
    /// 用于自定义精度数据，也可以存储自定义精度的货币数值。
    /// </summary>
    [Serializable, AttributeUsage( AttributeTargets.Property )]
    public class DecimalAttribute : Attribute {

        /// <summary>
        /// 数值的精度，即小数点左右的总共位数，但不包括小数点。
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// 小数点右侧的位数
        /// </summary>
        public int Scale { get; set; }

    }
}
