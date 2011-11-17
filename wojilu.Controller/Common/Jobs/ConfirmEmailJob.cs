/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Threading;

using wojilu.Net;
using wojilu.Web.Mvc;
using wojilu.Web.Jobs;
using wojilu.Web.Utils;
using wojilu.Web.Mvc.Utils;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Data;

namespace wojilu.Web.Controller.Common {

    public class ConfirmEmailJob : IWebJobItem {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ConfirmEmailJob ) );

        public IUserService userService { get; set; }
        public IConfirmEmail confirmEmail { get; set; }

        public ConfirmEmailJob() {
            userService = new UserService();
            confirmEmail = new ConfirmEmail();
        }

        private static Random rd = new Random();
        public void Execute() {

            if (config.Instance.Site.EnableEmail == false) return;


            IList unconfirmUsers = userService.GetUnSendConfirmEmailUsers();
            if (unconfirmUsers.Count > 0)
                logger.Info( "UnConfirm Users Count：" + unconfirmUsers.Count );

            DbContext.closeConnectionAll();

            foreach (User user in unconfirmUsers) {

                confirmEmail.SendEmail( user, null, null );
                DbContext.closeConnectionAll();

                int sleepSecond = rd.Next( 10, 60 );
                Thread.Sleep( sleepSecond * 1000 );
            }

        }

        public void End() {
        }


    }

}
