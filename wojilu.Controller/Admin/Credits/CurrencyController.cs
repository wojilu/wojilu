/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Web.Controller.Security;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Interface;

namespace wojilu.Web.Controller.Admin.Credits {

    public partial class CurrencyController : ControllerBase {

        public ICurrencyService currencyService { get; set; }
        public IAdminLogService<SiteLog> logService { get; set; }

        public CurrencyController() {
            currencyService = new CurrencyService();
            logService = new SiteLogService();
        }

        public void Index() {
            set( "addUrl", to( Add ) );
            KeyCurrency c = KeyCurrency.Instance;
            IList currencyAll = currencyService.GetCurrencyAll();
            bindCurrencyAll( c, currencyAll );
        }

        public void EditKeyCurrency() {
            target( UpdateKeyCurrency  );
            KeyCurrency c = KeyCurrency.Instance;
            bindKeyCurrencyEdit( c );
        }

        [HttpPost, DbTransaction]
        public void UpdateKeyCurrency() {
            KeyCurrency c = KeyCurrency.Instance;
            c.Name = ctx.Post( "Name" );
            c.Unit = ctx.Post( "Unit" );
            c.InitValue = ctx.PostInt( "InitValue" );
            c.IsShow = ctx.PostIsCheck( "IsShow" );
            c.CanRate = ctx.PostIsCheck( "CanRate" );
            if (strUtil.IsNullOrEmpty( c.Name )) {
                errors.Add( lang( "exName" ) );
                run( EditKeyCurrency );
            }
            else {
                c.update();
                log( SiteLogString.UpdateKeyCurrency(), typeof( KeyCurrency ) );
                echoToParentPart( lang( "saved" ) );
            }
        }

        public void EditCurrency( int currencyId ) {

            target( UpdateCurrency, currencyId  );
            Currency c = currencyService.GetCurrencyById( currencyId );
            bindCurrencyEdit( c );
        }

        [HttpPost, DbTransaction]
        public void UpdateCurrency( int currencyId ) {
            Currency c = currencyService.GetCurrencyById( currencyId );
            c = validate( c );
            if (errors.HasErrors) {
                run( EditCurrency, currencyId );
            }
            else {
                c.update();
                log( SiteLogString.UpdateCurrency(), typeof( Currency ) );
                echoToParentPart( lang( "saved" ) );
            }
        }

        public void Add() {
            target( Create  );
        }

        [HttpPost, DbTransaction]
        public void Create() {
            Currency currency = validate( null );
            if (errors.HasErrors) {
                run( Add );
            }
            else {
                currency.CanDelete = 1;
                currency.Guid = Guid.NewGuid().ToString();
                currency.insert();
                log( SiteLogString.AddCurrency(), typeof( Currency ) );
                echoToParentPart( lang( "saved" ) );
            }
        }

        [HttpDelete, DbTransaction]
        public void Delete( int currencyId ) {
            currencyService.GetCurrencyById( currencyId ).delete();
            IList incomeRules = currencyService.GetIncomeRules( currencyId );
            foreach (IncomeRule rule in incomeRules) {
                rule.delete();
            }
            log( SiteLogString.DeleteCurrency(), typeof( Currency ) );
            echoRedirectPart( lang( "deleted" ) );
        }

    }
}
