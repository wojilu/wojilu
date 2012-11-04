using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Mvc.Attr {

    /// <summary>
    /// 用于标注链接生成对象，提供开发查找的方便，对实际运行暂时没有意义。
    /// </summary>
    [Serializable, AttributeUsage( AttributeTargets.Class )]
    public class MvcLinkAttribute : Attribute {
    }

}
