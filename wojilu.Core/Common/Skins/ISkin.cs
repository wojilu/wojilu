/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Common.Skins {

    public interface ISkin {

        int Id { get; set; }

        /// <summary>
        /// 创建人或自定义人
        /// </summary>
        int MemberId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// 自定义样式表内容
        /// </summary>
        String Body { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// 样式表路径(存储的是相对路径，完整路径请使用GetSkinPath方法)
        /// </summary>
        String StylePath { get; set; }

        /// <summary>
        /// 截屏效果的缩略图路径(存储的是相对路径，完整路径请使用GetThumbPath方法)
        /// </summary>
        String ThumbUrl { get; set; }

        /// <summary>
        /// 点击数
        /// </summary>
        int Hits { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        int Replies { get; set; }

        /// <summary>
        /// 使用人数
        /// </summary>
        int MemberCount { get; set; }

        /// <summary>
        /// 状态(预留的属性)
        /// </summary>
        int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }

        /// <summary>
        /// 截屏效果图片的路径
        /// </summary>
        /// <returns></returns>
        String GetScreenShotPath();

        /// <summary>
        /// 返回样式表的完整路径
        /// </summary>
        /// <returns></returns>
        String GetSkinPath();

        /// <summary>
        /// 返回缩略图的完整路径
        /// </summary>
        /// <returns></returns>
        String GetThumbPath();

        /// <summary>
        /// 获取样式内容，如果自定义了，返回自定义内容；否则返回样式表内容
        /// </summary>
        /// <returns></returns>
        String GetSkinContent();

    }

}
