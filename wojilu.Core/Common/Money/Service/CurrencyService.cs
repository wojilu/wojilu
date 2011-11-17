/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Interface;
using System.Collections.Generic;

namespace wojilu.Common.Money.Service {


    public class CurrencyService : ICurrencyService {

        // 可评分的货币
        public virtual List<ICurrency> GetForumRateCurrency() {
            List<ICurrency> list = new List<ICurrency>();
            if (KeyCurrency.Instance.CanRate == 1) {
                list.Add( KeyCurrency.Instance );
            }
            IList currencyAll = this.GetCurrencyAll();
            foreach (Currency currency in currencyAll) {
                if (currency.CanRate == 1) {
                    list.Add( currency );
                }
            }
            return list;
        }

        // 所有货币
        public virtual List<Currency> GetCurrencyAll() {
            return cdb.findAll<Currency>();
        }

        public virtual List<ICurrency> GetICurrencyAll() {
            List<ICurrency> results = new List<ICurrency>();
            results.Add( KeyCurrency.Instance );
            IList clist = GetCurrencyAll();
            foreach (ICurrency c in clist) results.Add( c );
            return results;
        }

        public virtual Currency GetCurrencyById( int currencyId ) {
            return cdb.findById<Currency>( currencyId );
        }

        public virtual ICurrency GetICurrencyById( int currencyId ) {
            if (currencyId == KeyCurrency.Instance.Id) {
                return KeyCurrency.Instance;
            }
            return cdb.findById<Currency>( currencyId );
        }

        //----------------------------------- UserAction --------------------------------------------


        public virtual List<UserAction> GetUserActions() {
            return cdb.findAll<UserAction>();
        }

        //----------------------------------- 收入规则 --------------------------------------------

        public virtual IncomeRule GetRuleByActionAndCurrency( int actionId, int currencyId ) {

            List<IncomeRule> rules = cdb.findAll<IncomeRule>();
            foreach (IncomeRule r in rules) {
                if (r.CurrencyId == currencyId && r.ActionId == actionId) return r;
            }

            // 如果不存在这条规则，则创建
            IncomeRule rule = new IncomeRule();
            rule.CurrencyId = currencyId;
            rule.ActionId = actionId;
            rule.Income = 0; // 初始值是0
            rule.insert();
            return rule;
        }

        public virtual List<IncomeRule> GetRulesByAction( int actionId ) {

            List<Currency> clist = GetCurrencyAll();
            List<IncomeRule> results = new List<IncomeRule>();
            foreach (Currency c in clist)
                results.Add( GetRuleByActionAndCurrency( actionId, c.Id ) );
            return results;
        }

        public virtual List<IncomeRule> GetIncomeRules( int currencyId ) {

            List<UserAction> actions = cdb.findAll<UserAction>();
            List<IncomeRule> results = new List<IncomeRule>();
            foreach (UserAction action in actions)
                results.Add( GetRuleByActionAndCurrency( action.Id, currencyId ) );
            return results;
        }

        public virtual List<IncomeRule> GetSavedRules( int currencyId ) {
            List<IncomeRule> list = cdb.findAll<IncomeRule>();
            List<IncomeRule> results = new List<IncomeRule>();
            foreach (IncomeRule rule in list) {
                if (rule.CurrencyId == currencyId) {
                    results.Add( rule );
                }
            }
            return results;
        }

        public virtual void Save( IncomeRule rule ) {
            if (rule == null) return;
            List<IncomeRule> savedRules = this.GetSavedRules( rule.CurrencyId );
            foreach (IncomeRule savedRule in savedRules) {
                if (rule.ActionId == savedRule.ActionId) {
                    if (rule.Income != savedRule.Income) {
                        savedRule.Income = rule.Income;
                        savedRule.update();
                    }
                    return;
                }
            }
            rule.insert();
        }

        //---------------------------------- (中心货币的)收入规则操作 ---------------------------------------------

        public virtual KeyIncomeRule GetKeyIncomeRulesByAction( int actionId ) {
            List<KeyIncomeRule> savedRules = cdb.findAll<KeyIncomeRule>();
            foreach (KeyIncomeRule r in savedRules) {
                if (r.ActionId == actionId) return r;
            }

            KeyIncomeRule rule = new KeyIncomeRule();
            rule.ActionId = actionId;
            rule.insert();
            return rule;
        }

        public virtual List<KeyIncomeRule> GetKeyIncomeRules() {
            List<KeyIncomeRule> results = new List<KeyIncomeRule>();
            List<UserAction> actions = cdb.findAll<UserAction>();
            foreach (UserAction action in actions) {
                results.Add( GetKeyIncomeRulesByAction( action.Id ) );
            }
            return results;
        }

        public virtual void Save( KeyIncomeRule rule ) {
            if (rule == null) return;

            List<KeyIncomeRule> list = cdb.findAll<KeyIncomeRule>();
            foreach (KeyIncomeRule savedRule in list) {
                if (rule.ActionId == savedRule.ActionId) {
                    if (rule.Income != savedRule.Income) {
                        savedRule.Income = rule.Income;
                        savedRule.update();
                    }
                    return;
                }
            }
            rule.insert();
        }



        public virtual IncomeRule GetRuleById( int ruleId ) {
            return new IncomeRule().findById( ruleId ) as IncomeRule;
        }

        public virtual KeyIncomeRule GetKeyRuleById( int ruleId ) {
            return new KeyIncomeRule().findById( ruleId ) as KeyIncomeRule;
        }
    }
}

