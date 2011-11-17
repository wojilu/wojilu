/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;
using wojilu.Common.Money.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin.Credits {

    public partial class CurrencyController : ControllerBase {

        private void bindCurrencyAll( KeyCurrency c, IList currencyAll ) {
            set( "c.Name", c.Name );
            set( "c.Unit", c.Unit );
            set( "c.InitValue", c.InitValue );
            set( "c.EditUrl", to( EditKeyCurrency ) );
            set( "c.IsShow", (c.IsShow == 1) ? "√" : "×" );
            set( "c.CanRate", (c.CanRate == 1) ? "√" : "×" );

            IBlock block = getBlock( "list" );
            foreach (Currency currency in currencyAll) {
                block.Set( "s.Name", currency.Name );
                block.Set( "s.Unit", currency.Unit );
                block.Set( "s.ExchangeRate", currency.ExchangeRate );
                block.Set( "s.InitValue", currency.InitValue );
                block.Set( "s.IsShow", (currency.IsShow == 1) ? "√" : "×" );
                block.Set( "s.CanDeal", (currency.CanDeal == 1) ? "√" : "×" );
                block.Set( "s.CanRate", (currency.CanRate == 1) ? "√" : "×" );
                block.Set( "s.SetActionUrl", to( EditCurrency, currency.Id ) );

                String deleteLink = "";
                if (currency.CanDelete == 1) {
                    deleteLink = string.Format( "<a href=\"{0}\" class=\"deleteCmd\">" + lang( "delete" ) + "</a>", to( Delete, currency.Id ) );
                }
                block.Set( "s.DeleteUrl", deleteLink );
                block.Next();
            }

        }

        private void bindKeyCurrencyEdit( KeyCurrency c ) {
            set( "c.Name", c.Name );
            set( "c.Unit", c.Unit );
            set( "c.InitValue", c.InitValue );
            set( "c.IsShow", (c.IsShow == 1) ? "checked='checked'" : "" );
            set( "c.CanRate", (c.CanRate == 1) ? "checked='checked'" : "" );
        }

        private void bindCurrencyEdit( Currency c ) {
            set( "c.Name", c.Name );
            set( "c.Unit", c.Unit );
            set( "c.ExchangeRate", c.ExchangeRate );
            set( "c.InitValue", c.InitValue );
            set( "c.IsShow", (c.IsShow == 1) ? "checked='checked'" : "" );
            set( "c.CanDeal", (c.CanDeal == 1) ? "checked='checked'" : "" );
            set( "c.CanRate", (c.CanRate == 1) ? "checked='checked'" : "" );
        }


        public Currency validate( Currency c ) {
            String name = ctx.Post( "Name" );
            String strUnit = ctx.Post( "Unit" );
            if (strUtil.IsNullOrEmpty( name )) {
                errors.Add( lang( "exName" ) );
            }
            if (c == null) {
                c = new Currency();
            }
            c.Name = name;
            c.Unit = strUnit;
            c.ExchangeRate = cvt.ToDecimal( ctx.Post( "ExchangeRate" ) );
            c.InitValue = ctx.PostInt( "InitValue" );
            if (c.ExchangeRate == 0) {
                c.ExchangeRate = 1;
            }
            c.IsShow = ctx.PostIsCheck( "IsShow" );
            c.CanDeal = ctx.PostIsCheck( "CanDeal" );
            c.CanRate = ctx.PostIsCheck( "CanRate" );
            return c;
        }

        private void log( String msg, Type t ) {
            logService.Add( (User)ctx.viewer.obj, msg, "", t.FullName, ctx.Ip );
        }

    }
}

