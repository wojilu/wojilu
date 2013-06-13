/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Common;

namespace wojilu.Web.Controller.Users.Admin {

    public class InviteController : ControllerBase {

        public IInviteService inviteService { get; set; }

        public InviteController() {
            inviteService = new InviteService();
        }

        public override void CheckPermission() {

            Boolean isUserInviteClose = Component.IsClose( typeof( UserInviteApp ) );
            if (isUserInviteClose) {
                echo( "对不起，本功能已经停用" );
            }
        }

        public void Index() {

            target( SendMail );

            User user = ctx.owner.obj as User;

            String inviteLink = getInviteLink( user );
            ctx.SetItem( "inviteLink", inviteLink );

            set( "inviteLink", inviteLink );

            load( "previewHtml", MailBody );
        }

        private String getInviteLink( User user ) {
            String code = inviteService.GetCodeByUser( user.Id );
            String inviteLink = Link.To( Site.Instance, new wojilu.Web.Controller.RegisterController().Invite, user.Id ) + "?code=" + code;
            inviteLink = getFullUrl( inviteLink );
            return inviteLink;
        }

        [HttpPost, DbTransaction]
        public void SendMail() {

            String EmailList = ctx.Post( "EmailList" );
            if (strUtil.IsNullOrEmpty( EmailList )) {
                echoError( "请填写email" );
                return;
            }

            String[] arrMail = EmailList.Split( new char[] { ',', '，' } );
            List<String> list = new List<string>();
            foreach (String mailStr in arrMail) {
                if (strUtil.IsNullOrEmpty( mailStr )) continue;
                String mail = strUtil.SubString( mailStr.Trim(), RegPattern.EmailLength );
                if (RegPattern.IsMatch( mail, RegPattern.Email )) list.Add( mail );
            }



            String myWords = strUtil.CutString( ctx.Post( "myWords"), 200 );

            User user = ctx.owner.obj as User;
            String inviteLink = getInviteLink( user );
            ctx.SetItem( "inviteLink", inviteLink );
            String mailBody = loadHtml( MailBody);
            mailBody = mailBody.Replace( "<div id=\"myWordsPreview\"></div>", "<div id=\"myWordsPreview\">" + myWords + "</div>" );

            inviteService.AddMail( user, list, mailBody );

            echoRedirect( lang( "opok" ), Index );
        }

        [NonVisit]
        public void MailBody() {

            User user = ctx.owner.obj as User;
            String userLink = getFullUrl( toUser( user ) );

            set( "inviteLink", ctx.GetItem( "inviteLink" ) );
            set( "userName", user.Name );

            String userPic = user.PicM;
            if (userPic.StartsWith( "/" )) userPic = getFullUrl( userPic );

            set( "userPic", userPic );
            set( "userLink", userLink );

            set( "siteName", config.Instance.Site.SiteName );
            set( "siteLink", ctx.url.SiteAndAppPath );
            
        }

        private String getFullUrl( String url ) {
            if (url == null) return "";
            if (url.StartsWith( "http" )) return url;
            return strUtil.Join( ctx.url.SiteAndAppPath, url );
        }

    }

}
