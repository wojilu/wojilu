/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Members.Users.Domain {

    /// <summary>
    /// 头像审核未通过后，重新上传日志
    /// </summary>
    public class UserErrorPic : ObjectBase<UserErrorPic> {


        public static readonly int StatusFirstUpload = 0;
        public static readonly int StatusWaitingUpload = 1;
        public static readonly int StatusWaitingApprove = 2;
        public static readonly int StatusOk = 3;

        public long UserId { get; set; }
        public DateTime Created { get; set; }
        public String Ip { get; set; }

        /// <summary>
        /// 管理员对本次上传的审核意见
        /// </summary>
        public String ReviewMsg { get; set; }

        /// <summary>
        /// 是否审核通过
        /// </summary>
        public int IsPass { get; set; }

        /// <summary>
        /// 用户下次上传，必须通过审核才能访问网站
        /// </summary>
        public int IsNextAutoPass { get; set; }



    }
}
