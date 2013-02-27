/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;

namespace wojilu.OAuth {

    public class UserConnectService : IUserConnectService {

        public virtual UserConnect GetById( int id ) {
            return UserConnect.findById( id );
        }

        public virtual bool HasBind( int userId, String typeFullName ) {
            UserConnect x = GetConnectInfo( userId, typeFullName );
            return x != null;
        }

        public virtual Result Create( User user, String connectType, AccessToken token ) {

            UserConnect connect = GetConnectInfo( token.Uid, connectType );
            if (connect != null) {
                return new Result( "本帐号已经绑定: uid=" + token.Uid + ",connectType=" + connectType );
            }

            UserConnect x = new UserConnect();

            x.User = user;
            x.ConnectType = connectType;

            x.Uid = token.Uid;
            x.Name = token.Name;

            x.AccessToken = token.Token;
            x.RefreshToken = token.RefreshToken;
            x.ExpiresIn = token.ExpiresIn;
            x.Scope = token.Scope;

            Result result = x.insert();

            if (result.IsValid) {
                user.IsBind = 1;
                user.update();
            }

            return result;
        }

        public virtual UserConnect GetConnectInfo( String connectUid, String connectType ) {

            UserConnect x = UserConnect.find( "ConnectType=:ctype and Uid=:uid" )
                .set( "ctype", connectType )
                .set( "uid", connectUid )
                .first();

            return x;
        }

        public virtual UserConnect GetConnectInfo( int userId, String connectType ) {

            return UserConnect.find( "UserId=:userId and ConnectType=:ctype" )
                .set( "userId", userId )
                .set( "ctype", connectType )
                .first();
        }

        public virtual Result UnBind( int userId, String connectType ) {

            Result result = new Result();
            
            User user = User.findById( userId );
            if (user == null) {
                result.Add( "用户不存在，无法取消绑定" );
                return result;
            }

            AuthConnect connect = AuthConnectFactory.GetConnect( connectType );
            if (connect == null) {
                result.Add( "此连接类型不存在:" + connectType );
                return result;
            }

            UserConnect x = GetConnectInfo( userId, connect.GetType().FullName );

            if (x == null) {
                result.Add( "对不起，您没有绑定过" );
                return result;
            }

            // 如果这是最后一个绑定，并且用户没有补充过用户名和密码，那么禁止取消绑定。
            if (strUtil.IsNullOrEmpty( user.Pwd ) && isLastBind( user )) {
                result.Add( "这是您的最后一个绑定，取消之后将无法登录。请在“修改密码”页面补充密码之后，再取消绑定。" );
                return result;
            }

            int rowAffected = x.delete();

            result.Info = rowAffected;

            if (rowAffected==1) {
                user.IsBind = 0;
                user.update();
            }

            return result;
        }

        private bool isLastBind( User user ) {

            return UserConnect.find( "UserId=:userId" )
                .set( "userId", user.Id )
                .count() == 1;
        }

        public virtual Result Sync( int userId, String connectType, int isSync ) {

            Result result = new Result();

            AuthConnect connect = AuthConnectFactory.GetConnect( connectType );
            if (connect == null) {
                result.Add( "此连接类型不存在:" + connectType );
                return result;
            }

            UserConnect x = GetConnectInfo( userId, connect.GetType().FullName );

            if (x == null) {
                result.Add( "对不起，您没有绑定过" );
                return result;
            }

            x.NoSync = (isSync == 0 ? 1 : 0);
            x.update();

            return result;
        }

    }
}
