using System;
using System.Collections.Generic;
using System.Text;
using wojilu.weibo.Interface;
using wojilu.weibo.Domain;

namespace wojilu.weibo.Service {

    public class UserWeiboSettingService : IUserWeiboSettingService {

        public Result Bind( UserWeiboSetting setting ) {
            return db.insert( setting );
        }

        public Result Update( UserWeiboSetting setting ) {
            return db.update( setting );
        }

        public int Unbind( int userId, int weiboTypeId ) {
            return db.deleteBatch<UserWeiboSetting>( string.Format( "userId={0} and weiboType={1}", userId, weiboTypeId ) );
        }

        public void Sync( int userId, int weiboTypeId ) {
            db.updateBatch<UserWeiboSetting>( "set IsSync=1", string.Format( "userId={0} and weiboType={1}", userId, weiboTypeId ) );
        }

        public void UnSync( int userId, int weiboTypeId ) {
            db.updateBatch<UserWeiboSetting>( "set IsSync=0", string.Format( "userId={0} and weiboType={1}", userId, weiboTypeId ) );
        }

        public bool ExistBy( int userId, int weiboTypeId ) {
            return db.count<UserWeiboSetting>( string.Format( "userId={0} and weiboType={1}", userId, weiboTypeId ) ) > 0;
        }

        public IList<UserWeiboSetting> FindByUserId( int userId ) {
            return db.find<UserWeiboSetting>( "UserId=:id" ).set( "id", userId ).list();
        }

        public int Count() {
            return db.count<UserWeiboSetting>();
        }

        public DataPage<UserWeiboSetting> FindByPage( string condition, int current, int pageSize ) {
            return db.findPage<UserWeiboSetting>( condition, current, pageSize );
        }

        public UserWeiboSetting Find( int userId, int weiboType ) {
            return db.find<UserWeiboSetting>( "UserId =:id and WeiboType=:type" ).set( "id", userId ).set( "type", weiboType ).first();
        }

        public UserWeiboSetting Find( int weiboTypeId, string tokenKey, string tokenSecret ) {

            if (tokenKey == null) throw new ArgumentNullException( "tokenKey" );
            if (tokenSecret == null) tokenSecret = string.Empty;

            return db.find<UserWeiboSetting>( "WeiboType=:id and AccessToken=:token and AccessSecret=:secret" )
                     .set( "id", weiboTypeId )
                     .set( "token", tokenKey )
                     .set( "secret", tokenSecret ).first();
        }

    }

}
