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
        void DeletePostCount( int creatorId );

        int GetUserCount();
        User GetCurrent();

        DataPage<User> GetAll( int pageSize );
        DataPage<User> GetAllValid( int pageSize );

        User GetById( int id );
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
        List<User> GetRankedToMakeFriends( int count, List<int> ids );
        List<User> GetUnSendConfirmEmailUsers();
        DataPage<User> SearchBy( String condition, int pageSize );

        User IsExist( String name );
        User IsExistUrl( String url );
        User IsNamePwdCorrect( String name, String pwd );
        User IsNameEmailPwdCorrect( String nameOrEmail, String pwd );
        Boolean IsPwdCorrect( User user, String pwd );
        Boolean IsEmailExist( string email );
        Boolean IsEmailExist( int userId, string email );

        String HashPwd( String pwd, String salt );

        Boolean IsNameReservedOrExist( String inputName );
        Boolean IsUrlReservedOrExist( String inputUrl );

        Boolean IsUserDeleted( User user );

        User Register( User user, MvcContext ctx );
        Result RegisterNoPwd( User user );

        void SendConfirmEmail( User user );

        void UpdatePwd( User user, String pwd );
        void UpdateEmail( User user, string email );
        Result CreateEmail( User user, string email );

        void UpdateAvatar( User user, String newPic );


        List<User> SearchByName( String name );


    }
}
