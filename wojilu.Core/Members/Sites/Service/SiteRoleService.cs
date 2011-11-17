/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Members.Sites.Domain;
using wojilu.Common.Security;

namespace wojilu.Members.Sites.Service {

    public class SiteRoleService : ISiteRoleService {

        public virtual SiteRole GetById( int id ) {
            return cdb.findById<SiteRole>( id );
        }

        public virtual void InsertSiteRole( SiteRole role ) {
            role.insert();
        }

        public virtual void UpdateSiteRole( SiteRole role ) {
            role.update();
        }

        public virtual void DeleteSiteRole( SiteRole role ) {
            role.delete();
        }

        //----------------------- 角色 -----------------------

        public virtual List<SiteRole> GetAllRoles() {
            return cdb.findAll<SiteRole>();
        }

        public virtual List<SiteRole> GetRolesWithotGuest() {
            List<SiteRole> list = GetAllRoles();
            List<SiteRole> results = new List<SiteRole>();
            foreach (SiteRole role in list) {
                if (role.Id != SiteRole.Guest.Id) {
                    results.Add( role );
                }
            }
            return results;
        }

        public virtual List<SiteRole> GetAdminRoles() {
            return this.getRoleByGroupId( RoleGroup.Admin );
        }

        public virtual List<SiteRole> GetNormalRoles() {
            return this.getRoleByGroupId( RoleGroup.Normal );
        }

        private List<SiteRole> getRoleByGroupId( int groupId ) {
            List<SiteRole> list = GetAllRoles();
            List<SiteRole> results = new List<SiteRole>();
            foreach (SiteRole role in list) {
                if (role.GroupId == groupId) {
                    results.Add( role );
                }
            }
            return results;
        }



        //--------------------- 等级 -------------------------

        public virtual List<SiteRank> GetRankAll() {
            return cdb.findAll<SiteRank>();
        }

        public virtual SiteRank GetRankById( int id ) {
            return cdb.findById<SiteRank>( id );
        }

        public virtual void InsertRank( SiteRank rank ) {
            rank.insert();
        }

        public virtual void UpdateRank( SiteRank rank ) {
            rank.update();
        }

        public virtual void DeleteRank( SiteRank rank ) {
            rank.delete();
        }

        public virtual SiteRank GetRankByCredit( int credit ) {

            List<SiteRank> ranks = GetRankAll();

            SiteRank lastRank = new SiteRank { Credit = Int32.MaxValue };

            for (int i = 0; i < ranks.Count; i++) {

                SiteRank rank = ranks[i];

                SiteRank nextRank = null;
                if (i < ranks.Count - 1) {
                    nextRank = ranks[i + 1];
                }
                else
                    nextRank = lastRank;

                if (credit >= rank.Credit && credit < nextRank.Credit) return rank;
            }

            return lastRank;
        }

        public virtual SiteRank GetNextRank( SiteRank rank ) {
            List<SiteRank> ranks = GetRankAll();
            SiteRank lastRank = ranks[ranks.Count - 1];
            if (rank.Id == lastRank.Id) return null;

            for (int i = 0; i < ranks.Count; i++) {
                SiteRank r = ranks[i];
                if (r.Id == rank.Id) {
                    return ranks[i + 1];
                }
            }
            return null;
        }

        public virtual SiteRank GetPreRank( SiteRank rank ) {
            List<SiteRank> ranks = GetRankAll();
            SiteRank firstRank = ranks[0];
            if (rank.Id == firstRank.Id) return null;

            for (int i = 0; i < ranks.Count; i++) {
                SiteRank r = ranks[i];
                if (r.Id == rank.Id) {
                    return ranks[i - 1];
                }
            }
            return null;
        }


        //---------------------- 其他等级体系 ------------------------

        public virtual List<SiteRankOther> GetRankOther() {
            return cdb.findAll<SiteRankOther>();
        }

        public virtual SiteRankOther GetRankOtherById( int id ) {
            return cdb.findById<SiteRankOther>( id );
        }

        public virtual void UpdateRankByOther( SiteRankOther otherRank ) {

            List<SiteRank> rankAll = this.GetRankAll();
            for (int i = 0; i < rankAll.Count; i++) {
                IRole rank = rankAll[i];
                this.updateRankByOther_private( rank, i, otherRank );
            }

            if (otherRank.RankCount > rankAll.Count) {
                for (int i = rankAll.Count; i < otherRank.RankCount; i++) {
                    SiteRank rank = new SiteRank();
                    rank.Name = otherRank.GetName( i );
                    InsertRank( rank );
                }
            }
        }

        private void updateRankByOther_private( IRole rank, int i, SiteRankOther otherRank ) {
            string[] arrRanks = otherRank.Ranks.Split( '/' );
            if (arrRanks.Length > i) {
                rank.Name = arrRanks[i];
                UpdateRank( rank as SiteRank );
            }
        }

        //---------------------- 合并角色和等级 ------------------------

        public virtual List<IRole> GetRoleAndRank() {

            List<SiteRole> roles = GetAllRoles();
            List<SiteRank> ranks = GetRankAll();

            List<IRole> results = new List<IRole>();
            int i = addRoleTo( results, roles, 1 );
            addRoleTo( results, ranks, i );

            return results;
        }

        private int addRoleTo( List<IRole> results, IList roles, int i ) {
            foreach (IRole role in roles) {
                RoleProxy rr = new RoleProxy();
                rr.Id = i;
                rr.Name = role.Name;
                rr.Role = role;
                results.Add( rr );
                i++;
            }
            return i;
        }

        public virtual List<IRole> GetRoleAndRank( IList newRoles ) {
            List<IRole> results = new List<IRole>();
            List<IRole> sysRoles = GetRoleAndRank();

            foreach (IRole role in newRoles) {
                results.Add( role );
            }

            foreach (IRole role in sysRoles) {
                results.Add( role );
            }
            return results;
        }



    }




}

