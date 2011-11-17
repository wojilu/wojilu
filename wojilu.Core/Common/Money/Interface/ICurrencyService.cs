/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using wojilu.Common.Money.Domain;
using System.Collections.Generic;

namespace wojilu.Common.Money.Interface {

    public interface ICurrencyService {

        List<Currency> GetCurrencyAll();
        Currency GetCurrencyById( int currencyId );
        List<ICurrency> GetICurrencyAll();
        ICurrency GetICurrencyById( int currencyId );
        List<ICurrency> GetForumRateCurrency();


        List<KeyIncomeRule> GetKeyIncomeRules();
        KeyIncomeRule GetKeyIncomeRulesByAction( int actionId );
        KeyIncomeRule GetKeyRuleById( int ruleId );

        IncomeRule GetRuleById( int ruleId );
        IncomeRule GetRuleByActionAndCurrency( int actionId, int currencyId );
        List<IncomeRule> GetRulesByAction( int actionId );
        List<IncomeRule> GetIncomeRules( int currencyId );
        List<IncomeRule> GetSavedRules( int currencyId );

        List<UserAction> GetUserActions();

        void Save( IncomeRule rule );
        void Save( KeyIncomeRule rule );

    }
}
