using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Spider.Domain;
using wojilu.Common.Spider.Interface;

namespace wojilu.Common.Spider.Service {

    public class SportImportService : ISportImportService {

        public virtual List<SpiderImport> GetAll() {
            return SpiderImport.find( "order by OrderId desc, Id asc" ).list();
        }

        public virtual SpiderImport GetById(long id) {
            return SpiderImport.findById( id );
        }

        public virtual void Stop(long id) {
            SpiderImport s = SpiderImport.findById( id );
            s.IsDelete = 1;
            s.update( "IsDelete" );
        }

        public virtual void Start(long id) {
            SpiderImport s = SpiderImport.findById( id );
            s.IsDelete = 0;
            s.update( "IsDelete" );
        }

        public virtual void Delete(long id) {
            SpiderImport s = SpiderImport.findById( id );
            s.IsDelete = 1;
            s.update( "IsDelete" );
        }

        public virtual void Insert( SpiderImport s ) {
            s.insert();
        }

        public virtual void Update( SpiderImport s ) {
            s.update();
        }


    }

}
