using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Task.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc;
using wojilu.Members.Interface;

namespace wojilu.Apps.Task.Core
{
    public interface ITaskProcessor
    {
        /// <summary>
        /// 当前任务ID
        /// </summary>
        TaskInfo Task { get; set; }

        /// <summary>
        /// 是否可以领取奖励
        /// </summary>
        /// <param name="info"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Result CanReceiveReward(IUser user);

        /// <summary>
        /// 当前用户点击支付任务或领取奖金时的处理程序
        /// </summary>
        /// <param name="controller"></param>
        void ProcessTask(ControllerBase controller);

        /// <summary>
        /// 显示当前任务信息
        /// </summary>
        /// <param name="controller"></param>
        /// <returns>返回一段HtmlString</returns>
        string ShowTaskHtml(ControllerBase controller);

        /// <summary>
        /// 领奖
        /// </summary>
        /// <param name="c"></param>
        void GetReward(ControllerBase c);

        
    }
}
