/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Common.Resource;
using System.Collections;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.AppBase;
using wojilu.Members.Users.Enum;
using wojilu.Common.Money.Domain;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Users.Admin {


    public partial class UserProfileController : ControllerBase {


        private void bindFace( User user ) {
            if (strUtil.IsNullOrEmpty( user.Pic )) {
                set( "memberFace", "<span class=\"warning\">" + lang( "exNotUploadFace" ) + "</span>" );
            }
            else {
                set( "memberFace", string.Format( "<img src=\"{0}\">", user.PicM ) );
            }
        }

        private void bindInterest( User m ) {

            set( "m.Music", m.Profile.Music );
            set( "m.Movie", m.Profile.Movie );
            set( "m.Book", m.Profile.Book );
            set( "m.Sport", m.Profile.Sport );
            set( "m.Eat", m.Profile.Eat );
            set( "m.OtherHobby", m.Profile.OtherHobby );

        }

        private void bindProfile( User m ) {

            set( "m.Id", m.Id );
            set( "m.Name", m.RealName );
            set( "m.NickName", m.Name );
            set( "m.Title", m.Title );

            String kv = "Name=Value";
            dropList( "Year", AppResource.GetInts( 1910, 2009 ), kv, m.BirthYear );
            dropList( "Month", AppResource.GetInts( 1, 12 ), kv, m.BirthMonth );
            dropList( "Day", AppResource.GetInts( 1, 31 ), kv, m.BirthDay );


            dropList( "Gender", AppResource.Gender, kv, m.Gender );
            dropList( "Relationship", AppResource.Relationship, kv, m.Relationship );
            dropList( "Blood", AppResource.Blood, kv, m.Blood );
            dropList( "Degree", AppResource.Degree, kv, m.Degree );
            dropList( "Zodiac", AppResource.Zodiac, kv, m.Zodiac );

            dropList( "ProvinceId1", AppResource.Province, kv, m.ProvinceId1 );
            dropList( "ProvinceId2", AppResource.Province, kv, m.ProvinceId2 );

            set( "m.City1", m.City1 );
            set( "m.City2", m.City2 );

            set( "m.Birthday", string.Format( "{0}-{1}-{2}", m.BirthYear, m.BirthMonth, m.BirthDay ) );

            checkboxList( "Purpose", AppResource.Purpose, "Name=Name", m.Profile.Purpose );
            dropList( "ContactCondition", AppResource.ContactCondition, kv, m.Profile.ContactCondition );

            IBlock block = getBlock( "sexyInfo" );
            if (config.Instance.Site.ShowSexyInfoInProfile) {
                bindSexyInfo( block, m );
            }
            set( "m.Description", m.Profile.Description );
            set( "m.Signature", m.Signature );

            set( "m.UserDescriptionMin", config.Instance.Site.UserDescriptionMin );
            set( "m.UserDescriptionMax", config.Instance.Site.UserDescriptionMax );
            set( "m.UserSignatureMin", config.Instance.Site.UserSignatureMin );
            set( "m.UserSignatureMax", config.Instance.Site.UserSignatureMax );

        }


        private void bindContact( User m ) {

            set( "m.Email", m.Email );
            set( "m.EmailNotify", Html.RadioList( AppResource.EmailNotify, "EmailNotify", "Name", "Value", m.Profile.EmailNotify ) );

            set( "m.QQ", m.QQ );
            set( "m.MSN", m.MSN );

            set( "m.Address", m.Profile.Address );
            set( "m.Tel", m.Profile.Tel );
            set( "m.IM", m.Profile.IM );
            set( "m.WebSite", m.Profile.WebSite );

            String confirmTip = "";

            if (m.IsEmailConfirmed == EmailConfirm.Confirmed) {

                confirmTip = string.Format( "<img src=\"{0}\"/> 邮箱已激活", strUtil.Join( sys.Path.Img, "ok.gif" ) );
            }
            else if (config.Instance.Site.EnableEmail) {
                KeyIncomeRule rule = currencyService.GetKeyIncomeRulesByAction( 18 );
                confirmTip = string.Format( "<img src=\"{3}\"/> 提醒：您的邮箱尚未激活。激活<span class=\"red\">可奖励{0}{1}</span>；<br/><span class=\"left20\">请查看您的邮箱(包括垃圾箱)是否有激活邮件，如果没有，请 <a href=\"{2}\" target=\"_blank\">点击此处</a> 重发激活邮件</span>",
                    rule.Income, rule.CurrencyUnit, Link.To( Site.Instance, new Common.ActivationController().SendEmailLogin ),
                    strUtil.Join( sys.Path.Img, "info.gif")
                    );
            }

            set( "confirmTip", confirmTip );

        }

        private void bindSexyInfo( IBlock block, User m ) {
            String kv = "Name=Value";
            radioList( "Sexuality", AppResource.Sexuality, kv, m.Profile.Sexuality );
            dropList( "Smoking", AppResource.Smoking, kv, m.Profile.Smoking );
            dropList( "Sleeping", AppResource.Sleeping, kv, m.Profile.Sleeping );
            dropList( "Body", AppResource.Body, kv, m.Profile.Body );
            dropList( "Hair", AppResource.Hair, kv, m.Profile.Hair );
            dropList( "Height", AppResource.Height, kv, m.Profile.Height );
            dropList( "Weight", AppResource.Weight, kv, m.Profile.Weight );

            block.Set( "m.OtherInfo", m.Profile.OtherInfo );
            block.Next();
        }

        //----------------------------------------------------------------------------------------------------------


        private void bindAppList( IList apps ) {

            IBlock block = getBlock( "list" );
            foreach (IMemberApp app in apps) {

                if (app.AppInfo.IsInstanceClose( ctx.owner.obj.GetType() )) continue;


                block.Set( "app.Name", app.Name );
                block.Set( "app.Status", app.Status );
                block.Set( "app.Permission", getPermission( app ) );
                block.Set( "app.StatusStyle", getStatusStyle( app ) );
                block.Set( "app.OrderId", app.OrderId );
                block.Set( "app.TypeName", app.AppInfo.Name );
                block.Set( "app.StateStr", getAppState( app ) );
                block.Set( "app.EditPermissionLink", to( EditPermission, app.Id ) );

                block.Next();
            }
        }

        private String getPermission( IMemberApp app ) {
            if (app.AccessStatus == (int)AccessStatus.Public)
                return lang( "statusPublic" );
            if (app.AccessStatus == (int)AccessStatus.Friend)
                return lang( "statusFriend" );
            if (app.AccessStatus == (int)AccessStatus.Private)
                return lang( "statusPrivate" );
            return lang( "statusPublic" );
        }


        private String getAppState( IMemberApp app ) {
            if (app.IsStop == 1) return lang( "stopped" );
            return lang( "running" );
        }

        private object getStatusStyle( IMemberApp app ) {
            if (app.IsStop == 1) {
                return "stop";
            }
            return "";
        }

    }
}

