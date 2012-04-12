﻿using System;
using wojilu.Common.AppBase.Interface;
using wojilu.Serialization;
using wojilu.Web.Mvc.Attr;

namespace wojilu.weibo.Domain
{
    public class WeiboApp : ObjectBase<WeiboApp>, IApp
    {
        public DateTime Created { get; set; }

        public bool Enable { get; set; }

        #region IApp Members

        public int OwnerId { get; set; }

        public string OwnerUrl { get; set; }

        public string OwnerType { get; set; }

        #endregion

    }
}