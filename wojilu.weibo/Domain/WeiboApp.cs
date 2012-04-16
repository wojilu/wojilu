using System;
using wojilu.Common.AppBase.Interface;
using wojilu.Serialization;
using wojilu.Web.Mvc.Attr;

namespace wojilu.weibo.Domain
{
    public class WeiboApp : ObjectBase<WeiboApp>, IApp
    {
        #region IApp Members

        public int OwnerId { get; set; }

        public string OwnerUrl { get; set; }

        public string OwnerType { get; set; }

        #endregion

    }
}