using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Domain;

namespace wojilu.Members.Users.Service {

    public class UserTagService : IUserTagService {

        public virtual UserTagShip GetById(long id) {
            UserTagShip u = UserTagShip.findById( id );
            return u;
        }

        public virtual List<UserTagShip> GetPage(long ownerId) {
            List<UserTagShip> us = UserTagShip.find( "UserId=" + ownerId ).list();
            return us;
        }


        public virtual void SaveTags(string tagList, long viewerId, User owner) {

            String[] arrTags = tagList.Split( new char[] { ',', '，', '、' } );

            foreach (String tag in arrTags) {

                if (strUtil.IsNullOrEmpty( tag )) continue;

                String name = strUtil.SqlClean( tag.Trim(), 10 );

                UserTag ut = GetTagByName( name );
                if (ut == null) {
                    ut = new UserTag();
                    ut.CreatorId = viewerId;
                    ut.Name = strUtil.SubString( tag.Trim(), 10 );
                    ut.insert();
                }

                UserTagShip uts = UserTagShip.find( "UserId=" + owner.Id + " and TagId=" + ut.Id ).first();
                if (uts != null) continue;


                uts = new UserTagShip();
                uts.User = owner;
                uts.Tag = ut;
                uts.insert();

                ut.UserCount++;
                ut.update();

            }
        }


        public virtual void DeleteUserTag( UserTagShip u ) {
            u.delete();

            int count = UserTagShip.count( "TagId=" + u.Tag.Id );
            u.Tag.UserCount = count;
            u.Tag.update();
        }



        public virtual UserTag GetTagById(long id) {
            return UserTag.findById( id );
        }

        public virtual DataPage<User> GetPageByTag(long tagId) {
            DataPage<UserTagShip> list = UserTagShip.findPage( "TagId=" + tagId );
            return list.Convert<User>( populateUsers( list.Results ) );
        }

        private List<User> populateUsers( List<UserTagShip> list ) {

            List<User> users = new List<User>();
            foreach (UserTagShip ut in list) {
                if (ut.User != null && ut.User.Id > 0) users.Add( ut.User );
            }

            return users;
        }


        public virtual UserTag GetTagByName( string tName ) {
            if (strUtil.IsNullOrEmpty( tName )) return null;

            tName = strUtil.SqlClean( tName, 10 );
            return UserTag.find( "Name=:name" ).set( "name", tName ).first();
        }


    }
}
