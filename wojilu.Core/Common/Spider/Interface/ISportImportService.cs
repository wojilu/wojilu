using System;
using System.Collections.Generic;
using wojilu.Common.Spider.Domain;

namespace wojilu.Common.Spider.Interface {

    public interface ISportImportService {

        List<SpiderImport> GetAll();
        SpiderImport GetById(long id);

        void Insert( SpiderImport s );

        void Start(long id);
        void Stop(long id);
        void Update( SpiderImport s );
        void Delete(long id);
    }

}
