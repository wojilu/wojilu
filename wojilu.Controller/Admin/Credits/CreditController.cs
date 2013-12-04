/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using wojilu.Common.Money.Interface;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Domain;
using wojilu.Members.Sites.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Web.Controller.Security;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Admin.Credits {

    public partial class CreditController : ControllerBase {

        public virtual ICurrencyService currencyService { get; set; }
        public virtual IAdminLogService<SiteLog> logService { get; set; }

        public CreditController() {
            currencyService = new CurrencyService();
            logService = new SiteLogService();
        }

        public virtual void IncomeRule() {
            IList currencyList = currencyService.GetCurrencyAll();
            IList actions = currencyService.GetUserActions();
            bindIncomeRule( currencyList, actions );
        }

        public virtual void EditKeyRule( long ruleId ) {
            target( UpdateKeyRule, ruleId );
            KeyIncomeRule rule = currencyService.GetKeyRuleById( ruleId );
            bind( "rule", rule );
        }

        [HttpPost, DbTransaction]
        public virtual void UpdateKeyRule( long ruleId ) {
            KeyIncomeRule rule = currencyService.GetKeyRuleById( ruleId );
            rule.Income = ctx.PostInt( "Income" );
            rule.update();
            log( SiteLogString.UpdateKeyCurrencyRule(), typeof( KeyIncomeRule ) );
            echoToParentPart( lang( "saved" ) );
        }

        public virtual void EditKeyInit() {
            target( UpdateKeyInit );
            KeyCurrency c = KeyCurrency.Instance;
            bind( "c", c );
        }

        [HttpPost, DbTransaction]
        public virtual void UpdateKeyInit() {
            KeyCurrency c = KeyCurrency.Instance;
            c.InitValue = ctx.PostInt( "InitValue" );
            c.update();
            log( SiteLogString.UpdateKeyCurrencyInit(), typeof( KeyCurrency ) );
            echoToParentPart( lang( "saved" ) );
        }

        //-----------------------------------------------------------------------------------

        public virtual void EditRule( long ruleId ) {
            target( UpdateRule, ruleId );
            IncomeRule rule = currencyService.GetRuleById( ruleId );
            bind( "rule", rule );
        }

        [HttpPost, DbTransaction]
        public virtual void UpdateRule( long ruleId ) {
            IncomeRule rule = currencyService.GetRuleById( ruleId );
            rule.Income = ctx.PostInt( "Income" );
            rule.update();
            log( SiteLogString.UpdateIncomeRule(), typeof( IncomeRule ) );
            echoToParentPart( lang( "saved" ) );
        }

        public virtual void EditInit( long currencyId ) {
            target( UpdateInit, currencyId );
            Currency c = currencyService.GetCurrencyById( currencyId );
            bind( "c", c );
        }

        [HttpPost, DbTransaction]
        public virtual void UpdateInit( long currencyId ) {
            Currency c = currencyService.GetCurrencyById( currencyId );
            c.InitValue = ctx.PostInt( "InitValue" );
            c.update();
            log( SiteLogString.UpdateCurrencyInit(), typeof( Currency ) );
            echoToParentPart( lang( "saved" ) );
        }

    }
}