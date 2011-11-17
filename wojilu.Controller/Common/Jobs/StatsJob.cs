using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Jobs;
using wojilu.Apps.Blog.Interface;
using wojilu.Apps.Blog.Domain;
using wojilu.Data;
using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Photo.Domain;
using wojilu.Common.Tags;
using wojilu.Members.Users.Domain;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Apps.Blog.Service;
using wojilu.Apps.Photo.Service;

namespace wojilu.Web.Controller.Common.Jobs {

    public class StatsJob : IWebJobItem {

        public IBlogService blogService { get; set; }
        public IPhotoService photoService { get; set; }
        public ISiteRoleService roleService { get; set; }

        public StatsJob() {
            blogService = new BlogService();
            photoService = new PhotoService();
            roleService = new SiteRoleService();
        }

        public void Execute() {

            // 4点多执行
            if (DateTime.Now.Hour <= 3 || DateTime.Now.Hour >= 5) return;


            List<BlogApp> blogs = blogService.GetBlogAppAll();
            List<PhotoApp> photos = photoService.GetAppAll();
            List<Tag> tags = Tag.findAll();
            //List<User> users = db.findAll<User>();

            foreach (BlogApp blog in blogs) {
                blogService.UpdateCount( blog );
                blogService.UpdateCommentCount( blog );
                DbContext.closeConnectionAll(); // 每执行一个app即关闭，以免超时
            }

            foreach (PhotoApp app in photos) {
                photoService.UpdateCount( app );
                photoService.UpdateCommentCount( app );
                DbContext.closeConnectionAll();
            }

            foreach (Tag tag in tags) {

                int count = DataTagShip.find( "Tag.Id=" + tag.Id ).count();
                tag.DataCount = count;
                tag.update( "DataCount" );
                DbContext.closeConnectionAll();
            }


            //// 得到所有用户
            //foreach (User user in users) {

            //    // 根据帖子数，算出积分：按照每帖5分计算
            //    user.Credit = user.PostCount * 5;
            //    db.update( user, "Credit" );

            //    // 更新 UserIncome 表中的中心货币的收入
            //    UserIncomeService incomeService = new UserIncomeService();
            //    UserIncome income = incomeService.GetUserIncome( user.Id, KeyCurrency.Instance.Id );
            //    income.Income = user.Credit;
            //    db.update( income, "Income" );

            //    // 更新等级
            //    int newRankId = roleService.GetRankByCredit( user.Credit ).Id;
            //    if (user.RankId != newRankId) {
            //        user.RankId = newRankId;
            //        db.update( user, "RankId" );
            //    }
            //    DbContext.closeConnectionAll();

            //}


        }

        public void End() {
        }

    }

}
