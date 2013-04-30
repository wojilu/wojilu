/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web;
using wojilu.Web.Utils;

namespace wojilu.Members.Groups {

    public class GroupHelper {

        /// <summary>
        /// 保存群组 logo
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="groupUrlName"></param>
        /// <returns></returns>
        public static Result SaveGroupLogo( HttpFile postedFile, String groupUrlName ) {
            return Uploader.SaveImg( sys.Path.DiskGroupLogo, postedFile, groupUrlName, GroupSetting.Instance.LogoWidth, GroupSetting.Instance.LogoHeight, Drawing.SaveThumbnailMode.Cut );
        }
    }

}
