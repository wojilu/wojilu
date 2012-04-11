using System;
using wojilu.Common.AppBase.Interface;
using wojilu.Serialization;

namespace wojilu.weibo.Domain
{
    public class WeiboApp : ObjectBase<WeiboApp>, IApp
    {
        public DateTime Created { get; set; }

        public string Settings { get; set; }

        #region IApp Members

        public int OwnerId { get; set; }

        public string OwnerUrl { get; set; }

        public string OwnerType { get; set; }

        #endregion

        public WeiboSetting GetSettingsObj()
        {
            if (strUtil.IsNullOrEmpty(Settings)) return new WeiboSetting();
            var s = JSON.ToObject<WeiboSetting>(Settings);
            return s;
        }
    }
}