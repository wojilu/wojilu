/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;

namespace wojilu.Members.Users.Service {

    public class BlacklistService : IBlacklistService {

        public IUserService userService { get; set; }
        public IFriendService friendService { get; set; }
        public IFollowerService followerService { get; set; }

        public BlacklistService() {
            userService = new UserService();
            friendService = new FriendService();
            followerService = new FollowerService();
        }

        public DataPage<Blacklist> GetPage( int ownerId, int pageSize ) {
            return Blacklist.findPage( "UserId=" + ownerId, pageSize );
        }

        public Boolean IsBlack( int ownerId, int targetId ) {
            return Blacklist.find( "User.Id=" + ownerId + " and Target.Id=" + targetId ).first() != null;
        }

        public Blacklist GetById( int id, int ownerId ) {
            Blacklist b = Blacklist.findById( id );
            if (b == null) return null;
            if (b.User == null) return null;
            if (b.User.Id != ownerId) return null;
            return b;
        }

        public Result Delete( int id, int ownerId ) {
            Result result = new Result();
            Blacklist b = GetById( id, ownerId );
            if (b == null) {
                result.Add( lang.get( "exDataNotFound" ) );
                return result;
            }

            b.delete();

            return result;
        }

        public Result Create( int ownerId, string targetUserName ) {

            Result result = new Result();
            if (strUtil.IsNullOrEmpty( targetUserName )) {
                result.Add( lang.get( "exUserName" ) );
                return result;
            }

            User target = userService.GetByName( targetUserName );
            if (target == null) {
                result.Add( lang.get( "exUser" ) );
                return result;
            }

            Blacklist b = new Blacklist();
            b.User = new User( ownerId );
            b.Target = target;

            result = b.insert();

            if (result.IsValid) {

                friendService.DeleteFriendByBlacklist( ownerId, target.Id );
                followerService.DeleteFollow( target.Id, ownerId );

            }

            return result;
        }        

    }
}
