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
            set( "incomeLogLink", to( IncomeLogLink ) );
            set( "ruleLink", to( IncomeRule ) );
            set( "rankLink", to( Rank ) );
            //set( "postLink", to( Posts ) );
        }

        public void My() {

            User user = ctx.owner.obj as User;

            set( "user.Credit", user.Credit );
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



        public void IncomeLogLink() {

            User user = ctx.owner.obj as User;

            DataPage<UserIncomeLog> logs = incomeService.GetUserIncomeLog( user.Id );
            bindList( "list", "c", logs.Results );
            set( "pager", logs.PageBar );
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

        //public void Posts() {


        //    User user = ctx.owner.obj as User;

        //    ForumBoard b = new ForumBoard();

        //    ForumPost p = new ForumPost();
        //    IPageList posts = p.findPage( "Creator.Id=" + user.Id );
        //    IBlock block = getBlock( "list" );
        //    foreach (ForumPost post in posts.Results) {
        //        block.Set( "post.Title", post.Title );
        //        block.Set( "post.Created", post.Created );
        //        block.Set( "post.LinkShow", alink.ToAppData( post ) );

        //        ForumBoard board = b.findById( post.ForumBoardId ) as ForumBoard;
        //        block.Set( "post.BoardName", board.Name );
        //        block.Set( "post.BoardLink", alink.ToAppData( board ) );
        //        block.Next();
        //    }
        //    set( "pager", posts.PageBar );
        //}

    }

}
