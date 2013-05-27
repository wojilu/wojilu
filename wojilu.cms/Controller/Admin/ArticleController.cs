using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.cms.Domain;
using wojilu.Web.Mvc.Attr;

namespace wojilu.cms.Controller.Admin {

    public partial class ArticleController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ArticleController ) );        

        public void List() {
            List<Article> list = Article.findAll();
            bindListShow( list );
        }

        public void LoopList() {
            List<Category> categories = Category.findAll();
            List<Article> articles = Article.findAll();

            bindCategoryAndArticlet( categories, articles );
        }

        public void Index() {
            logger.Info( "开始获取文章列表" );
            DataPage<Article> list = Article.findPage( "", SystemConfig.Instance.PageSize );
            logger.Info( "开始绑定列表" );
            bindAdminList( list );
        }

        private void bindAdminList( DataPage<Article> list ) {
            bindList( "list", "article", list.Results, bindLink );
            set( "page", list.PageBar );

            set( "operationUrl", to( Admin ) ); // 工具栏操作保存的网址
            set( "searchAction", to( Search ) ); // 搜索提交的网址
            set( "key", ctx.Get( "key" ) ); // 用户当前查询的关键词

            List<Category> categories = Category.findAll();
            categories.Insert( 0, new Category { Id = 0, Name = "转移到分类..." } ); // 给了所有分类增加一个文字提示
            dropList( "adminDropCategoryList", categories, "Name=Id", 0 );
        }


        public void Search() {

            view( "Index" ); // 使用Index方法的视图

            string key = ctx.Get( "key" ); // 获取关键词
            key = strUtil.SqlClean( key, 10 ); // 进行sql过滤，保证安全

            int qtype = ctx.GetInt( "qtype" );

            DataPage<Article> list = new DataPage<Article>();
            if (qtype == 1) {
                list = Article.findPage( "Title like '%" + key + "%' " );
            }

            bindAdminList( list );
        }

        public void Admin() {

            string choiceIds = ctx.PostIdList( "choice" );
            string action = ctx.Post( "action" );
            int categoryId = ctx.PostInt( "categoryId" );

            validateInput( choiceIds, action, categoryId );
            if (ctx.HasErrors) {
                echoError();
                return;
            }

            string condition = string.Format( "Id in ({0})", choiceIds );

            if ("delete".Equals( action )) {
                db.deleteBatch<Article>( condition );
            }
            else if ("category".Equals( action )) {
                db.updateBatch<Article>( "set CategoryId=" + categoryId, condition );
            }

            echoAjaxOk();
        }

        private void validateInput( string choiceIds, string action, int categoryId ) {
            if (strUtil.IsNullOrEmpty( choiceIds )) errors.Add( "未选择文章" );
            if (!"delete".Equals( action ) && !"category".Equals( action )) errors.Add( "错误的命令" );
            if ("category".Equals( action ) && categoryId <= 0) errors.Add( "分类不正确" );
        }

        public void Add() {
            target( Create );
            dropList( "categoryId", Category.findAll(), "Name=Id", 0 );
        }

        public void Create() {
            Article a = ctx.PostValue<Article>();
            a.Category = new Category { Id = ctx.PostInt( "categoryId" ) };
            a.Content = ctx.PostHtml( "article.Content" );
            Result result = db.insert( a );
            if (result.HasErrors) {
                errors.Join( result );
                run( Add );
            }
            else
                redirect( Index );
        }

        public void Edit( int id ) {
            target( Update, id );
            Article a = Article.findById( id );
            bind( a );
            int selected = a.Category == null ? 0 : a.Category.Id;
            dropList( "categoryId", Category.findAll(), "Name=Id", selected );
            set( "article.Content", a.Content );
        }

        public void Update( int id ) {
            Article a = Article.findById( id );
            a = ctx.PostValue( a ) as Article;
            a.Category = new Category { Id = ctx.PostInt( "categoryId" ) };
            a.Content = ctx.PostHtml( "article.Content" );
            Result result = db.update( a );
            if (result.HasErrors) {
                errors.Join( result );
                run( Add );
            }
            else
                redirect( Index );
        }

        [HttpDelete]
        public void Delete( int id ) {
            Article a = Article.findById( id );
            db.delete( a );
            redirect( Index );
        }
    }
}
