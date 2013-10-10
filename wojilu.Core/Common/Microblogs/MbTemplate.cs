using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Common.Microblogs {

    /// <summary>
    /// 动态消息的模板
    /// </summary>
    public class MbTemplate {

        /// <summary>
        /// 获取feed模板信息
        /// </summary>
        /// <param name="actionName">写了博客、加入了小组等……</param>
        /// <param name="title">博客标题、小组标题等……</param>
        /// <param name="lnk">链接</param>
        /// <param name="summary">摘要</param>
        /// <param name="pic">图片</param>
        /// <returns></returns>
        public static String GetFeed( String actionName, String title, String lnk, String summary, String pic ) {

            String msg = string.Format( "<div class=\"feed-item-title\">{0} <a href=\"{1}\">{2}</a></div>", actionName, lnk, title );
            msg += string.Format( "<div class=\"feed-item-body\">{0}</div>", summary );

            if (strUtil.HasText( pic )) {
                msg += string.Format( "<div class=\"feed-item-pic\"><a href=\"{1}\"><img src=\"{0}\"/></a></div>", pic, lnk );
            }

            return msg;

        }


    }
}
