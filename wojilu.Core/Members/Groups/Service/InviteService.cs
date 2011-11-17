/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Domain;
using wojilu.Members.Users.Service;
using wojilu.Web.Mvc;
using wojilu.Common.Msg.Service;
using wojilu.Members.Users.Interface;
using wojilu.Common.Msg.Interface;
using wojilu.Members.Groups.Interface;

namespace wojilu.Members.Groups.Service {

    public class InviteService : wojilu.Members.Groups.Interface.IInviteService {

        public IUserService userService { get; set; }
        public IMessageService msgService { get; set; }
        public IMemberGroupService mgrService { get; set; }

        public InviteService() {
            userService = new UserService();
            msgService = new MessageService();
            mgrService = new MemberGroupService();
        }

        public virtual DataPage<GroupInvite> GetPage( int groupId ) {

            return GroupInvite.findPage( "OwnerId=" + groupId );
        }

        public virtual Result Invite( User inviter, String receiver, Group group, String msg, String inviteUrl ) {

            Result result = this.getReceivers( receiver, group.Id );
            if (result.HasErrors) {
                return result;
            }

            List<User> users = result.Info as List<User>;

            foreach (User user in users) {

                GroupInvite g = new GroupInvite();
                g.Receiver = user;
                g.Inviter = inviter;

                g.OwnerId = group.Id;
                g.Msg = msg;
                g.Code = Guid.NewGuid().ToString();

                g.insert();

                //-----------send msg--------------------

                String lnkInvite = inviteUrl.Replace( "999", g.Id.ToString() ) + "?code=" + g.Code;

                String msgTitle = inviter.Name + " 诚邀您加入群组";

                String msgBody = string.Format( "<a href=\"{1}\" target=\"_blank\">{0}</a> 诚邀您加入群组 “{2}”， <a href=\"{3}\" target=\"_blank\">请点击此处接受邀请</a>。 ", inviter.Name, Link.ToMember( inviter ), group.Name, lnkInvite );
                msgBody += "<br/><br/>邀请信息：" + g.Msg;

                msgService.SendMsg( inviter, user.Name, msgTitle, msgBody );

            }

            return new Result();

        }


        private Result getReceivers( String rawReceiver, int groupId ) {

            Result result = new Result();
            if (strUtil.IsNullOrEmpty( rawReceiver )) {
                result.Add( lang.get( "exReceiverNotFound" ) );
                return result;
            }

            List<User> list = new List<User>();
            string[] strArray = rawReceiver.Trim().Split( separator );
            for (int i = 0; i < strArray.Length; i++) {
                User user = userService.IsExist( strArray[i] );
                if (user == null) {
                    result.Add( lang.get( "exReceiverNotFound" ) + ": \"" + strArray[i] + "\" " );
                    return result;
                }

                if (mgrService.IsGroupMember( user.Id, groupId )) {
                    result.Add( "用户 " + user.Name + " 已经是群组成员" );
                    return result;
                }

                list.Add( user );
            }
            result.Info = list;
            return result;
        }

        private static readonly char[] separator = new char[] { ',', '，', '、', '/', '|' };


    }

}
