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
        ISiteRoleService roleService { get; set; }

        bool HasEnoughKeyIncome(long userId, int income);

        void AddIncome(User user, long currencyId, int income, string msg);
        void AddIncome(User user, long actionId, string msg);
        void AddIncomeReverse(User user, long actionId, string msg);
        void AddKeyIncome(User user, int income, string msg);
        void AddKeyIncome(long userId, int income, string msg);

        IList GetIncomeList( String userIds );
        UserIncome GetUserIncome(long userId, long currencyId);
        List<UserIncome> GetUserIncome(long userId);
        DataPage<UserIncomeLog> GetUserIncomeLog(long userId, long currencyId);

        void InitUserIncome( User user );
        void InsertIncome( UserIncome income );

        void UpdateUserIncome( UserIncome userIncome );

    }
}
