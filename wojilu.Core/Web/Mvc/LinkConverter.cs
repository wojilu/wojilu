using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// 将链接转换为符合域名映射规则的形式
    /// </summary>
    public class LinkConverter {


        public static String Get( String url ) {

            // 特定域名映射
            List<DomainMap> dmaps = cdb.findAll<DomainMap>();
            foreach (DomainMap dm in dmaps) {
                if (dm.Url.Equals( url )) return dm.Name;
            }

            //// 通配符映射
            //// TODO：关键是重构Link，插入IsDomainMap的检查，如果启用域名映射，那么不再MemberPath.GetPath，而是MemberPath.GetDomain拼接
            //List<DomainWildcardMap> dwmaps = cdb.findAll<DomainWildcardMap>();
            //if (dwmaps.Count > 0) {
            //    DomainWildcardMap dw = dwmaps[0]; //目前仅支持一条规则
            //}


            return url;
        }


    }

}
