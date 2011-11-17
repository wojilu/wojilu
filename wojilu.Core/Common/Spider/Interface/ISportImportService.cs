using System;
using System.Collections.Generic;
using wojilu.Common.Spider.Domain;

namespace wojilu.Common.Spider.Interface {

    public interface ISportImportService {

        List<SpiderImport> GetAll();
        SpiderImport GetById( int id );

        void Insert( SpiderImport s );

        void Start( int id );
        void Stop( int id );
        void Update( SpiderImport s );
        void Delete( int id );
    }

}
