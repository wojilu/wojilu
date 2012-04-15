using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Data;

namespace wojilu.Common.Domain
{
    public class MicroblogSyncConfig : CacheObject
    {
        public MicroblogSyncConfig()
        {
          
        }

        /// <summary>
        /// 类的完整名称，比如wojilu.Common.Jobs.RefreshServerJob
        /// </summary>
        public string Type { get; set; }
    }
}
