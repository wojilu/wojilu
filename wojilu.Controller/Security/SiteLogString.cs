/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Controller.Security {

    public class SiteLogString {

        public static String LoginOk() { return "登录成功"; }
        public static String LoginError() { return "登录失败"; }
        public static String Logout() { return "注销登录"; }

        public static String AddFooterMenu() { return "添加页脚菜单"; }
        public static String UpdateFooterMenu() { return "修改页脚菜单"; }
        public static String DeleteFooterMenu() { return "删除页脚菜单"; }

        public static String AddPage() { return "添加页面"; }
        public static String UpdatePage() { return "修改页面"; }
        public static String DeletePage() { return "删除页面"; }

        public static String AdminUser( String cmd ) {
            return "用户管理->" + getUserCmd( cmd );
        }

        private static String getUserCmd( String cmd ) {
            if ("pick" == cmd)
                return "推荐";
            else if ("unpick" == cmd)
                return "取消推荐";
            else if ("delete" == cmd)
                return "放入回收站";
            else if ("undelete" == cmd)
                return "从回收站还原";
            else if ("deletetrue" == cmd)
                return "彻底删除";

            else if ("category" == cmd)
                return "设置角色";
            else
                return "用户管理";
        }


        public static String UpdateUserPwd() { return "修改用户密码"; }
        public static String SendUserEmail() { return "给用户发送email"; }

        public static String DeleteSiteRole() { return "删除角色"; }
        public static String InsertSiteRole() { return "添加角色"; }
        public static String InsertSiteAdminRole() { return "添加管理角色"; }
        public static String UpdateSiteRoleName() { return "角色改名"; }
        public static String InsertSiteRank() { return "添加等级"; }
        public static String DeleteSiteRank() { return "删除等级"; }
        public static String UpdateCredit() { return "修改等级的积分要求"; }
        public static String RenameRank() { return "等级改名"; }
        public static String UpdateRankStar() { return "修改等级的星号"; }
        public static String UpdateRankAll() { return "重新统计用户的所用积分和等级"; }
        public static String SetRanksByOther() { return "切换到其他等级系列"; }

        public static String UpdateAdminPermission() { return "修改后台权限"; }
        public static String UpdateAppAdminPermission() { return "修改后台app管理权限"; }
        public static String UpdateUserDataPermission() { return "修改后台用户数据管理权限"; }
        public static String UpdateFrontPermission() { return "修改前台app访问权限"; }

        public static String UpdateKeyCurrency() { return "修改中心货币"; }
        public static String UpdateCurrency() { return "修改货币"; }
        public static String AddCurrency() { return "添加货币"; }
        public static String DeleteCurrency() { return "删除货币"; }

        public static String UpdateKeyCurrencyInit() { return "修改中心货币初始值"; }
        public static String UpdateKeyCurrencyRule() { return "修改中心货币积分规则"; }
        public static String UpdateIncomeRule() { return "修改中心货币积分规则"; }
        public static String UpdateCurrencyInit() { return "修改货币初始值"; }

        public static String UpdateLogo() { return "修改网站logo"; }
        public static String DeleteLogo() { return "删除网站logo"; }
        public static String EditSiteSettingBase() { return "修改网站配置之基本信息"; }
        public static String EditSiteSettingUser() { return "修改网站配置之用户配置"; }
        public static String EditSiteSettingEmail() { return "修改网站email配置"; }
        public static String EditSiteSettingFilter() { return "修改网站关键词过滤器"; }

        public static String SendMsgToGroupAdministrator() { return "给群组管理员发消息"; }
        public static String LockGroup() { return "锁定群组"; }
        public static String HideGroup() { return "隐藏群组"; }
        public static String DeleteGroup() { return "删除群组"; }

        public static String ApplySkin() { return "使用新皮肤"; }

        public static String PickBlogPost() { return "推荐博客"; }
        public static String UnPickBlogPost() { return "取消推荐博客"; }
        public static String SystemDeleteBlogPost() { return "删除博客到系统回收站"; }
        public static String SystemUnDeleteBlogPost() { return "从系统回收站还原博客"; }
        public static string DeleteBlogPost() { return "博客批量删入系统回收站"; }
        public static string DeleteBlogPostTrue() { return "批量彻底删除博客"; }
        public static string UnDeleteBlogPost() { return "从回收站恢复博客"; }
        public static string MoveBlogPost() { return "修改博客的系统分类"; }

        public static String SystemDeleteContentPost() { return "删除文章到系统回收站"; }
        public static String SystemUnDeleteContentPost() { return "从系统回收站还原文章"; }


        public static String PickPhotoPost() { return "推荐图片"; }
        public static String UnPickPhotoPost() { return "取消推荐图片"; }
        public static String DeletePhotoPost() { return "删除图片"; }
        public static String MovePhotoPost() { return "修改图片的系统分类"; }
        public static String DeleteSysPhotoPost() { return "删除图片到系统回收站"; }
        public static String UnDeleteSysPhotoPost() { return "从系统回收站还原图片"; }
        public static String DeleteSysPhotoPostTrue() { return "删除图片到系统回收站"; }

        public static String InsertPhotoSysCategory() { return "添加图片系统分类"; }
        public static String UpdatePhotoSysCategory() { return "修改图片系统分类"; }
        public static String DeletePhotoSysCategory() { return "删除图片系统分类"; }

        public static string UpdateBlogSysCategory() { return "修改博客系统分类"; }
        public static string InsertBlogSysCategory() { return "添加博客系统分类"; }
        public static string DeleteBlogSysCategory() { return "删除博客系统分类"; }

        public static String AddMenu() { return "添加菜单"; }
        public static String UpdateMenu() { return "修改菜单"; }
        public static String DeleteMenu() { return "删除菜单"; }

        public static String InsertApp() { return "添加应用程序"; }
        public static String StartApp() { return "启动应用程序"; }
        public static String StopApp() { return "暂停应用程序"; }
        public static String UpdateApp() { return "修改应用程序"; }
        public static String DeleteApp() { return "删除应用程序"; }

        public static String UpdateGroupInfo() { return "修改群组资料"; }
        public static String UpdateGroupLogo() { return "上传群组logo"; }
        public static String ApproveUser() { return "群组成员审核通过"; }
        public static String DeleteUser() { return "删除群组成员"; }
        public static String AddOfficer() { return "添加群组管理员"; }
        public static String RemoveOfficer() { return "删除群组管理员"; }
        public static String AddFriendGroup() { return "添加友情群组"; }
        public static String DeleteFriendGroup() { return "删除友情群组"; }








    }

}
