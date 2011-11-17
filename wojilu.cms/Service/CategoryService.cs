using System;
using System.Collections.Generic;
using System.Text;
using wojilu.cms.Domain;
using wojilu.cms.Interface;

namespace wojilu.cms.Service {

    public class CategoryService : ICategoryService {

        public Category GetById( int id ) {
            return db.findById<Category>( id );
        }

        public List<Category> GetAll() {
            return db.findAll<Category>();
        }

        public Result Insert( Category c ) {
            return db.insert( c );
        }

        public Result Update( Category c ) {
            return db.update( c );
        }

    }

}
