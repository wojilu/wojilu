/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Collections;

using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Money.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Money.Interface {

    public interface IUserIncomeService {

        IMemberAppService appService { get; set; }
        ICurrencyService currencyService { get; set; }
        SiteRoleService roleService { get; set; }

        Boolean HasEnoughKeyIncome( int userId, int income );

        void AddIncome( User user, int currencyId, int income, String msg );
        void AddIncome( User user, int actionId, String msg );
        void AddIncomeReverse( User user, int actionId, String msg );
        void AddKeyIncome( User user, int income, String msg );
        void AddKeyIncome( int userId, int income, String msg );

        IList GetIncomeList( String userIds );
        UserIncome GetUserIncome( int userId, int currencyId );
        List<UserIncome> GetUserIncome( int userId );
        DataPage<UserIncomeLog> GetUserIncomeLog( int userId, int currencyId );

        void InitUserIncome( User user );
        void InsertIncome( UserIncome income );

        void UpdateUserIncome( UserIncome userIncome );

    }
}
