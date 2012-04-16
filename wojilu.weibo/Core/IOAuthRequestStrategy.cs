using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.weibo.Domain;
using wojilu.Web.Mvc;

namespace wojilu.weibo.Core
{
    /// <summary>
    /// 获取用户授权策略接口
    /// </summary>
    public interface IOAuthRequestStrategy
    {
        /// <summary>
        /// 将用户导向用户授权页面
        /// </summary>
        /// <param name="ctx"></param>
        void RedirectToAuthorizationUri(ControllerBase controller);

        /// <summary>
        /// 处理微博服务商的回调
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        void ProcessCallback(ControllerBase controller);
    }
}
