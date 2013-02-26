using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.Serialization;
using wojilu.cms.Domain;
using wojilu.cms.Service;
using wojilu.cms.Interface;

namespace wojilu.cms.Controller.Admin {

    public class CategoryController : ControllerBase {

        //public CategoryService categoryService { get; set; }
        public ICategoryService categoryService { get; set; }

        public CategoryController() {
            categoryService = new CategoryService();
        }

        public void Show( int id ) {
            Category c = Category.findById( id );
            string jsonString = Json.ToString( c );
            ctx.RenderJson( jsonString );
        }

        public void ListAll() {
            List<Category> list = Category.findAll();
            string jsonString = Json.ToString( list );
            ctx.RenderJson( jsonString );
        }

        public void Index() {
            //List<Category> list = Category.findAll();
            List<Category> list = categoryService.GetAll();
            bindList( "list", "c", list, bindLink );
        }

        private void bindLink( IBlock block, int id ) {
            block.Set( "c.EditLink", to( Edit, id ) );
            block.Set( "c.DeleteLink", to( Delete, id ) );
        }

        public void Add() {
            target( Create );
        }

        public void Create() {
            Category c = ctx.PostValue<Category>();
            Result result = db.insert( c );
            if (result.HasErrors) {
                errors.Join( result );
                run( Add );
            }
            else
                redirect( Index );
        }

        public void Edit( int id ) {
            target( Update, id );
            Category c = Category.findById( id );
            bind( c );
        }

        public void Update( int id ) {
            Category c = Category.findById( id );
            c = ctx.PostValue( c ) as Category;
            db.update( c );
            redirect( Index );
        }

        public void Delete( int id ) {
            Category c = Category.findById( id );
            db.delete( c );
            redirect( Index );
        }
    }

}
