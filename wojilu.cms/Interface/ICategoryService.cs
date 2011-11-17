using System;
using System.Collections.Generic;
using wojilu.cms.Domain;

namespace wojilu.cms.Interface {

    public interface ICategoryService {

        List<Category> GetAll();
        Category GetById( int id );

        Result Insert( Category c );
        Result Update( Category c );

    }

}
