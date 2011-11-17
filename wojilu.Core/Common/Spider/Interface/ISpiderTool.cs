using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Spider.Domain;

namespace wojilu.Common.Spider.Interface {

    public interface ISpiderTool {

        void DownloadPage( SpiderTemplate s, StringBuilder log, int[] arrSleep );
    }

}
