/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;

using wojilu.Web.Mvc;

using wojilu.Members.Sites.Domain;
using wojilu.Common.Money.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin.Security {

    public partial class SecurityController : ControllerBase {


        private void bindRoleRankLink() {
            set( "baseCurrency.Name", KeyCurrency.Instance.Name );
            set( "otherRankList", to( RankOther ) );

            set( "addAdminRoleLink", to( AddAdminRole ) );
            set( "addRoleLink", to( AddRole ) );
            set( "addRankLink", to( AddRank ) );

        }

        private void bindAdminRoles( List<SiteRole> adminRoles ) {
            IBlock adminBlock = getBlock( "adminRoles" );
            foreach (SiteRole role in adminRoles) {
                bindRoleLine( adminBlock, role, "siteRole" );
            }
        }

        private void bindNormalRoles( List<SiteRole> normalRoles ) {
            IBlock normalBlock = getBlock( "normalRoles" );
            foreach (SiteRole role in normalRoles) {
                bindRoleLine( normalBlock, role, "role" );
            }
        }

        private void bindRoleLine( IBlock block, SiteRole role, String lbl ) {

            block.Set( lbl + ".Name", role.Name );
            block.Set( lbl + ".RenameLink", to( Rename, role.Id ) );

            String deleteLink = "";
            if (role.Id != SiteRole.Administrator.Id && role.Id != SiteRole.NormalMember.Id && role.Id != SiteRole.Guest.Id)
                deleteLink = string.Format( " <span href='{0}' class='deleteCmd'>" + lang( "delete" ) + "</span>", to( Delete, role.Id ) );
            block.Set( lbl + ".DeleteLink", deleteLink );

            block.Next();
        }

        private void bindRankList( List<SiteRank> ranks ) {

            IBlock rankBlock = getBlock( "ranks" );
            SiteRank firstRank = ranks[0];
            foreach (SiteRank rank in ranks) {

                rankBlock.Set( "baseCurrency.Name", KeyCurrency.Instance.Name );

                rankBlock.Set( "r.Name", rank.Name );
                rankBlock.Set( "r.Credit", rank.Credit );
                rankBlock.Set( "r.RenameUrl", to( RenameRank, rank.Id ) );
                rankBlock.Set( "r.CreditEditUrl", to( CreditEdit, rank.Id ) );
                rankBlock.Set( "r.DeleteUrl", to( DeleteRank, rank.Id ) );

                rankBlock.Set( "r.EditCreditClass", rank.Id == firstRank.Id ? "hide" : "" );

                rankBlock.Set( "r.StarHtml", rank.StarHtml );
                rankBlock.Set( "r.SetRankStarLink", to( SetRankStar, rank.Id ) );

                rankBlock.Next();
            }
        }


        private void bindRankStar( SiteRank rank ) {
            String starPath = strUtil.Join( sys.Path.DiskImg, "star" );
            string[] starImgs = Directory.GetFiles( PathHelper.Map( starPath ) );

            IBlock block = getBlock( "star" );

            foreach (String img in starImgs) {
                String imgName = Path.GetFileName( img );
                String imgPath = strUtil.Join( starPath, imgName );
                block.Set( "star.ImgPath", imgPath );
                block.Set( "star.FileName", imgName );
                block.Set( "star.Checked", rank.StarPath == imgName ? "checked=\"checked\"" : "" );
                block.Next();
            }

            String starCount = Html.DropList( new string[] { "1", "2", "3", "4", "5" }, "StarCount", rank.StarCount );
            set( "starCount", starCount );
        }

        private void bindOtherRank( List<SiteRankOther> allRankOther ) {
            IBlock block = getBlock( "roleList" );
            foreach (SiteRankOther rank in allRankOther) {
                block.Set( "list.Name", rank.Name );
                block.Set( "list.Roles", rank.GetRankAllString( "->" ) );
                block.Set( "list.UseUrl", to( SetRanksByOther, rank.Id ) );
                block.Next();
            }
        }

        //---------------------------------------------------------------------------------------------

        private void log( String msg, SiteRole data ) {
            String dataInfo = "{Id:" + data.Id + ", Name:'" + data.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( SiteRole ).FullName, ctx.Ip );
        }

        private void log( String msg, SiteRank data ) {
            String dataInfo = "{Id:" + data.Id + ", Name:'" + data.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( SiteRank ).FullName, ctx.Ip );
        }

        private void log( String msg ) {
            logService.Add( (User)ctx.viewer.obj, msg, "", typeof( SiteRank ).FullName, ctx.Ip );
        }

    }
}

