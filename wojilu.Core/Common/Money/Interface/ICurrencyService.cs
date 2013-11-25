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
        Currency GetCurrencyById(long currencyId);
        List<ICurrency> GetICurrencyAll();
        ICurrency GetICurrencyById(long currencyId);
        List<ICurrency> GetForumRateCurrency();


        List<KeyIncomeRule> GetKeyIncomeRules();
        KeyIncomeRule GetKeyIncomeRulesByAction(long actionId);
        KeyIncomeRule GetKeyRuleById(long ruleId);

        IncomeRule GetRuleById(long ruleId);
        IncomeRule GetRuleByActionAndCurrency(long actionId, long currencyId);
        List<IncomeRule> GetRulesByAction(long actionId);
        List<IncomeRule> GetIncomeRules(long currencyId);
        List<IncomeRule> GetSavedRules(long currencyId);

        List<UserAction> GetUserActions();

        void Save( IncomeRule rule );
        void Save( KeyIncomeRule rule );

    }
}
