/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Context;

using wojilu.Common;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Money.Interface;

using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Domain;

namespace wojilu.Members.Users.Interface {

    public interface IUserService {

        ICurrencyService currencyService { get; set; }
        IUserIncomeService userIncomeService { get; set; }
        ISiteRoleService roleService { get; set; }

        void AddPostCount( User user );
        void ConfirmEmailIsError( User user );
        void DeletePostCount( long creatorId );

        int GetUserCount();
        User GetCurrent();

        DataPage<User> GetAll( int pageSize );
        DataPage<User> GetAllValid( int pageSize );

        User GetById( long id );
        List<User> GetByIds( String idsStr );
        User GetByMail( String email );
        User GetByName( String name );
        User GetByUrl( String friendUrl );
        String GetLastUserName();

        List<User> GetNewList( int count );
        List<User> GetNewListValid( int count );
        List<User> GetHitsList( int count );

        List<IBinderValue> GetNewListWithAvatar( int count );
        List<User> GetNewLoginList( int count );
        List<User> GetPickedList( int count );
        List<User> GetRanked( String sortBy, int count );
        List<User> GetRanked( int count );
        List<User> GetRankedToMakeFriends( int count, List<long> ids );
        List<User> GetUnSendConfirmEmailUsers();
        DataPage<User> SearchBy( String condition, int pageSize );

        User IsExist( String name );
        User IsExistUrl( String url );
        User IsNamePwdCorrect( String name, String pwd );
        User IsNameEmailPwdCorrect( String nameOrEmail, String pwd );
        Boolean IsPwdCorrect( User user, String pwd );
        Boolean IsEmailExist( string email );
        Boolean IsEmailExist( long userId, string email );

        String HashPwd( String pwd, String salt );

        Boolean IsNameReservedOrExist( String inputName );
        Boolean IsUrlReservedOrExist( String inputUrl );

        Boolean IsUserDeleted( User user );

        User Register( User user, MvcContext ctx );
        Result RegisterNoPwd( User user );

        void SendConfirmEmail( User user );

        void UpdatePwd( User user, String pwd );
        void UpdateEmail( User user, string email );
        void UpdateEmailAndResetConfirmStatus( User user, string email );
        Result CreateEmail( User user, string email );

        void UpdateAvatar( User user, String newPic );

        /// <summary>
        /// 根据用户名检索(使用 like %% 检索)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<User> SearchByName( String name );



        /// <summary>
        /// 保存图像、不会增加积分、不会发送邮件鼓励；给管理员发通知
        /// </summary>
        /// <param name="user"></param>
        /// <param name="p"></param>
        void UpdateAvatarWhenError( User user, string newPic );

        /// <summary>
        /// 仅仅保存图像。不会增加积分、不会发送邮件鼓励、不给管理员发通知
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newPic"></param>
        void UpdateAvatarOnly( User user, string newPic );
    }
}
