using System;
using wojilu.Members.Users.Interface;
using wojilu.Common.Msg.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Domain;

namespace wojilu.Members.Groups.Interface {

    public interface IInviteService {

        Result Invite( User inviter, string receiver, Group group, string msg, string inviteUrl );

        DataPage<GroupInvite> GetPage( int groupId );

        IMemberGroupService mgrService { get; set; }
        IMessageService msgService { get; set; }
        IUserService userService { get; set; }

    }
}
