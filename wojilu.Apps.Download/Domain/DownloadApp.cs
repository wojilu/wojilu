using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Apps.Download.Domain {

    public class DownloadApp : ObjectBase<DownloadApp>, IApp {

        public int OwnerId { get; set; }
        public String OwnerUrl { get; set; }
        public String OwnerType { get; set; }

        public DateTime Created { get; set; }

    }

}
