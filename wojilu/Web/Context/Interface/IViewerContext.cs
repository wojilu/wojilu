/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections;
using wojilu.Members.Interface;

namespace wojilu.Web.Context {

    /// <summary>
    /// 当前访问者的接口
    /// </summary>
    public interface IViewerContext {

        int Id { get; set; }

        /// <summary>
        /// 当前访问者
        /// </summary>
        IUser obj { get; set; }

        /// <summary>
        /// 发送站内私信
        /// </summary>
        /// <param name="ownerName"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Result SendMsg( String ownerName, String title, String body );

        /// <summary>
        /// 加为好友
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="msg"></param>
        Result AddFriend( int ownerId, String msg );

        /// <summary>
        /// 检索隐私配置，当前 viewer 对 owner 的某个item是否具有访问权限
        /// </summary>
        /// <param name="owner">被访问者</param>
        /// <param name="item"></param>
        /// <returns></returns>
        Boolean HasPrivacyPermission( IMember owner, String item );

        /// <summary>
        /// 是否网站管理员
        /// </summary>
        /// <returns></returns>
        Boolean IsAdministrator();

        /// <summary>
        /// 是否是当前访问的owner的管理员
        /// </summary>
        /// <returns></returns>
        Boolean IsOwnerAdministrator( IMember owner );

        /// <summary>
        /// 是否关注了某人
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Boolean IsFollowing( int ownerId );

        /// <summary>
        /// 和某人是否是朋友
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Boolean IsFriend( int ownerId );

        /// <summary>
        /// 是否已经登录
        /// </summary>
        Boolean IsLogin { get; set; }

        IList Menus { get; set; }
    }

}
