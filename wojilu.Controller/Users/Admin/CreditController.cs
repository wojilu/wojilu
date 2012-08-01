/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;

using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Interface;

namespace wojilu.Web.Controller.Users.Admin {

    public class CreditController : ControllerBase {

        public IUserIncomeService incomeService { get; set; }
        public ISiteRoleService roleService { get; set; }
        public ICurrencyService currencyService { get; set; }

        public CreditController() {
            incomeService = new UserIncomeService();
            roleService = new SiteRoleService();
            currencyService = new CurrencyService();
        }

        public override void Layout() {
            set( "keyIncomeLink", to( My ) );
            set( "ruleLink", to( IncomeRule ) );
            set( "rankLink", to( Rank ) );
            set( "incomeLink", to( IncomeLog, 0 ) );
        }

        public void My() {

            User user = ctx.owner.obj as User;

            set( "user.Name", user.Name );

            set( "user.RoleName", user.Role.Name );
            set( "user.RankName", user.Rank.Name );
            set( "user.StarHtml", user.Rank.StarHtml );

            set( "user.Created", user.Created );
            set( "user.LastLogin", user.LastLoginTime );
            set( "user.LastUpdate", user.LastUpdateTime );
            set( "user.PostCount", user.PostCount );


            List<UserIncome> incomes = incomeService.GetUserIncome( user.Id );
            incomes.Sort( UserIncome.ByCurrency );
            bindList( "list", "c", incomes );
        }

        private static readonly int tempKeyCurrencyId = 99999999;

        public void IncomeLog( int currencyId ) {

            User user = ctx.owner.obj as User;

            if (currencyId == 0) currencyId = -1;
            if (currencyId == tempKeyCurrencyId) currencyId = 0;

            DataPage<UserIncomeLog> logs = incomeService.GetUserIncomeLog( user.Id, currencyId );
            bindList( "list", "x", logs.Results, bindCurrencyLink );
            set( "pager", logs.PageBar );
        }

        private void bindCurrencyLink( IBlock block, String lbl, Object data ) {
            UserIncomeLog x = data as UserIncomeLog;
            int currencyId = x.CurrencyId == 0 ? tempKeyCurrencyId : x.CurrencyId;
            block.Set( "x.CurrencyLink", to( IncomeLog, currencyId ) );
        }

        public void IncomeRule() {

            IList currencyList = currencyService.GetCurrencyAll();
            IList actions = currencyService.GetUserActions();

            StringBuilder builder = new StringBuilder();
            builder.Append( "<table style='width:100%;' id='dataAdminList' cellspacing='0'>" );

            // header
            builder.Append( "<tr class='tableHeader'><td></td>" );
            builder.AppendFormat( "<td><strong>{0}</strong>(" + lang( "keyCurrency" ) + ")</td>", KeyCurrency.Instance.Name );


            foreach (ICurrency c in currencyList) {
                builder.AppendFormat( "<td>{0}</td>", c.Name );
            }
            builder.Append( "</tr>" );
            builder.Append( Environment.NewLine );

            // init value
            builder.Append( "<tr class='tableItems'>" );
            builder.AppendFormat( "<td>{0}</td>", lang( "initValueWithReg" ) );
            builder.AppendFormat( "<td>{0}</td>", KeyCurrency.Instance.InitValue );
            foreach (ICurrency c in currencyList) {
                builder.AppendFormat( "<td>{0}</td>", c.InitValue );
            }

            // items
            foreach (UserAction a in actions) {
                builder.Append( "<tr class='tableItems'>" );
                builder.AppendFormat( "<td>{0}</td>", a.Name );

                // keyCurrency rule
                KeyIncomeRule keyrule = currencyService.GetKeyIncomeRulesByAction( a.Id );
                builder.AppendFormat( "<td>{0}</td>", keyrule.Income );


                IList rules = currencyService.GetRulesByAction( a.Id );
                foreach (IncomeRule rule in rules) {
                    builder.AppendFormat( "<td>{0}</td>", rule.Income );
                }

                builder.Append( "</tr>" );
                builder.Append( Environment.NewLine );
            }

            builder.Append( "</table>" );

            set( "ruleTable", builder );
        }

        public void Rank() {

            set( "baseCurrency.Name", KeyCurrency.Instance.Name );

            IBlock rankBlock = getBlock( "ranks" );
            IList ranks = roleService.GetRankAll();
            SiteRank firstRank = ranks[0] as SiteRank;
            foreach (SiteRank rank in ranks) {

                rankBlock.Set( "r.Name", rank.Name );
                rankBlock.Set( "r.Credit", rank.Credit );
                rankBlock.Set( "r.StarHtml", rank.StarHtml );
                rankBlock.Next();
            }
        }


    }

}
