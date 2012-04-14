using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Feeds.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;

namespace wojilu.Common.Feeds.Interface
{
    /// <summary>
    /// 定义微博同步发送接口
    /// </summary>
    public interface IMicroblogSync
    {
        /// <summary>
        /// 同步微博
        /// </summary>
        /// <param name="user">同步的用户</param>
        /// <param name="text">微博内容，禁止加上html标签</param>
        /// <param name="picUrl">上传图片的本地地址如 c:\1.jpg这样的</param>
        void Sync(IUser user,string text,string picUrl);
    }
}
