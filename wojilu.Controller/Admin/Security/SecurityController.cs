/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Service;
using wojilu.Common.Security;
using wojilu.Members.Sites.Interface;
using wojilu.Web.Controller.Security;
using wojilu.Members.Interface;

namespace wojilu.Web.Controller.Admin.Security {

    public partial class SecurityController : ControllerBase {

        public ISiteRoleService roleService;
        public IAdminLogService<SiteLog> logService { get; set; }

        public SecurityController() {
            roleService = new SiteRoleService();
            logService = new SiteLogService();
        }

        public void Index() {

            bindRoleRankLink();

            List<SiteRole> adminRoles = roleService.GetAdminRoles();
            List<SiteRole> normalRoles = roleService.GetNormalRoles();
            List<SiteRank> ranks = roleService.GetRankAll();

            bindAdminRoles( adminRoles );
            bindNormalRoles( normalRoles );
            bindRankList( ranks );
        }

        //----------------------------------- role --------------------------------

        public void AddRole() {
            target( SaveRole );
        }

        public void AddAdminRole() {
            target( SaveAdminRole );
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {

            if (id == SiteRole.Administrator.Id || id == SiteRole.NormalMember.Id || id == SiteRole.Guest.Id) {
                echoRedirect( lang( "exRoleDeleteForbidden" ) );
                return;
            }

            SiteRole role = roleService.GetById( id );
            if (role != null) {
                roleService.DeleteSiteRole( role );
                log( SiteLogString.DeleteSiteRole(), role );
                echoRedirect( lang( "deleted" ) );
            }
            else {
                echoRedirect( lang( "exRoleNotFound" ) );

            }
        }

        [HttpPost, DbTransaction]
        public void SaveRole() {

            // TODO 给新加的角色赋予前台权限，标准：一般会员
            // 对于其他应用程序如何处理？难道广播给其他所有app？

            String target = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( target )) {
                errors.Add( lang( "exName" ) );
                run( AddRole );
            }
            else {
                SiteRole role = new SiteRole();
                role.Name = target;
                role.GroupId = RoleGroup.Normal;
                roleService.InsertSiteRole( role );
                log( SiteLogString.InsertSiteRole(), role );

                echoToParentPart( lang( "opok" ) );
            }
        }

        [HttpPost, DbTransaction]
        public void SaveAdminRole() {
            String target = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( target )) {
                errors.Add( lang( "exName" ) );
                run( AddAdminRole );
            }
            else {
                SiteRole role = new SiteRole();
                role.Name = target;
                role.GroupId = RoleGroup.Admin;
                roleService.InsertSiteRole( role );
                log( SiteLogString.InsertSiteAdminRole(), role );

                echoToParentPart( lang( "opok" ) );
            }
        }


        public void Rename( int id ) {
            target( UpdateName, id );
            SiteRole role = roleService.GetById( id );
            set( "r.Name", role.Name );
        }

        [HttpPost, DbTransaction]
        public void UpdateName( int id ) {
            SiteRole role = roleService.GetById( id );
            String target = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( target )) {
                errors.Add( lang( "exName" ) );
                run( Rename, id );
                return;
            }
            role.Name = target;
            roleService.UpdateSiteRole( role );
            log( SiteLogString.UpdateSiteRoleName(), role );

            echoToParentPart( lang( "opok" ) );
        }

        //--------------------------------- rank ----------------------------------


        public void AddRank() {
            target( CreateRank );
            set( "baseCurrency.Name", KeyCurrency.Instance.Name );
        }

        [HttpPost, DbTransaction]
        public void CreateRank() {

            String target = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( target )) {
                errors.Add( lang( "exName" ) );
                run( AddRank );
                return;
            }

            int credit = ctx.PostInt( "Credit" );
            List<SiteRank> ranks = roleService.GetRankAll();
            SiteRank lastRank = ranks[ranks.Count - 1];
            if (credit <= lastRank.Credit) {
                String msg = lang( "exRankError" );
                errors.Add( string.Format( msg, KeyCurrency.Instance.Name, lastRank.Credit ) );
                run( AddRank );
                return;
            }

            SiteRank rank = new SiteRank();
            rank.Name = target;
            rank.Credit = credit;
            roleService.InsertRank( rank );
            log( SiteLogString.InsertSiteRank(), rank );

            echoToParentPart( lang( "opok" ) );
        }

        [HttpDelete, DbTransaction]
        public void DeleteRank( int id ) {
            SiteRank rank = roleService.GetRankById( id );
            if (rank != null) {
                roleService.DeleteRank( rank );
                log( SiteLogString.DeleteSiteRank(), rank );

                echoRedirect( lang( "deleted" ) );
            }
            else {
                echoRedirect( lang( "exDataNotFound" ) );
            }
        }

        public void CreditEdit( int id ) {
            target( UpdateCredit, id );
            SiteRank rank = roleService.GetRankById( id );

            String inputBox = string.Format( "<input name=\"Credit\" type=\"text\" value=\"{0}\" style=\"width:40px;\" />", rank.Credit );
            String lbl = string.Format( lang( "lblRankRequirement" ), KeyCurrency.Instance.Name, inputBox );
            set( "rankRequirement", lbl );

            set( "r.Name", rank.Name );
        }

        [HttpPost, DbTransaction]
        public void UpdateCredit( int id ) {

            SiteRank rank = roleService.GetRankById( id );
            int credit = ctx.PostInt( "Credit" );

            SiteRank preRank = roleService.GetPreRank( rank );
            if (preRank == null) {
                echoToParentPart( lang( "exCreditFirst" ) );
                return;
            }

            if (credit <= 0) {
                errors.Add( lang( "exCreditGreat0" ) );
                run( CreditEdit, id );
                return;
            }

            int nextCredit = int.MaxValue;
            SiteRank nextRank = roleService.GetNextRank( rank );
            if (nextRank != null) nextCredit = nextRank.Credit;

            if (!(credit > preRank.Credit && credit < nextCredit)) {
                String msg = lang( "exCreditBetween" );
                errors.Add( string.Format( msg, preRank.Credit, nextCredit ) );
                run( CreditEdit, id );
                return;
            }

            rank.Credit = credit;
            roleService.UpdateRank( rank );
            log( SiteLogString.UpdateCredit(), rank );

            echoToParentPart( lang( "opok" ) );
        }


        public void RenameRank( int id ) {
            view( "Rename" );
            SiteRank rank = roleService.GetRankById( id );
            set( "r.Name", rank.Name );
            target( UpdateRankName, id );
        }

        [HttpPost, DbTransaction]
        public void UpdateRankName( int id ) {
            SiteRank rank = roleService.GetRankById( id );
            String target = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( target )) {
                errors.Add( lang( "exName" ) );
                run( RenameRank, id );
                return;
            }
            rank.Name = target;
            roleService.UpdateRank( rank );
            log( SiteLogString.RenameRank(), rank );

            echoToParentPart( lang( "opok" ) );
        }


        public void SetRankStar( int id ) {
            target( UpdateRankStar, id );
            SiteRank rank = roleService.GetRankById( id );
            bindRankStar( rank );
        }

        [HttpPost, DbTransaction]
        public void UpdateRankStar( int id ) {

            String starPath = ctx.Post( "StarPath" );
            int starCount = ctx.PostInt( "StarCount" );

            SiteRank rank = roleService.GetRankById( id );
            rank.StarPath = starPath;
            rank.StarCount = starCount;

            roleService.UpdateRank( rank );
            log( SiteLogString.UpdateRankStar(), rank );

            echoToParentPart( lang( "opok" ) );
        }

        //-------------------------------------------------------------------

        //public void ResetRank() {
        //    if (!checkServerTime()) return;
        //    set( "saveResetRank", to( UpdateRankAll ) );
        //}

        //[HttpPut, DbTransaction]
        //public void UpdateRankAll() {

        //    if (!checkServerTime()) return;

        //    // 得到所有用户
        //    List<User> users = db.findAll<User>();
        //    foreach (User user in users) {

        //        // 根据帖子数，算出积分：按照每帖5分计算
        //        user.Credit = user.PostCount * 5;
        //        db.update( user, "Credit" );

        //        // 更新 UserIncome 表中的中心货币的收入
        //        UserIncomeService incomeService = new UserIncomeService();
        //        UserIncome income = incomeService.GetUserIncome( user.Id, KeyCurrency.Instance.Id );
        //        income.Income = user.Credit;
        //        db.update( income, "Income" );

        //        // 更新等级
        //        int newRankId = roleService.GetRankByCredit( user.Credit ).Id;
        //        if (user.RankId != newRankId) {
        //            user.RankId = newRankId;
        //            db.update( user, "RankId" );
        //        }

        //    }
        //    log( SiteLogString.UpdateRankAll() );

        //    echoRedirect( lang( "opok" ) );
        //}

        //private Boolean checkServerTime() {

        //     TODO 可配置服务器操作时间
        //    int amHour = 8;
        //    int pmHour = 23;

        //    if (DateTime.Now.Hour >= amHour && DateTime.Now.Hour < pmHour) {
        //        String msg = lang( "exUpdateTimeLimit" );
        //        echoToParentPart( string.Format( msg, amHour, pmHour ) );
        //        return false;
        //    }

        //    return true;
        //}

        //-------------------------------------------------------------------

        public void RankOther() {
            List<SiteRankOther> allRankOther = roleService.GetRankOther();
            bindOtherRank( allRankOther );
        }

        [HttpPut, DbTransaction]
        public void SetRanksByOther( int id ) {
            SiteRankOther otherRank = roleService.GetRankOtherById( id );
            roleService.UpdateRankByOther( otherRank );
            log( SiteLogString.SetRanksByOther() );

            echoToParentPart( lang( "opok" ) );
        }

    }
}

