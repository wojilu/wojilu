using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Spider.Domain;
using wojilu.Common.Spider.Interface;

namespace wojilu.Common.Spider.Service {

    public class SportImportService : ISportImportService {

        public List<SpiderImport> GetAll() {
            return SpiderImport.find( "order by OrderId desc, Id asc" ).list();
        }

        public SpiderImport GetById( int id ) {
            return SpiderImport.findById( id );
        }

        public void Stop( int id ) {
            SpiderImport s = SpiderImport.findById( id );
            s.IsDelete = 1;
            s.update( "IsDelete" );
        }

        public void Start( int id ) {
            SpiderImport s = SpiderImport.findById( id );
            s.IsDelete = 0;
            s.update( "IsDelete" );
        }

        public void Delete( int id ) {
            SpiderImport s = SpiderImport.findById( id );
            s.IsDelete = 1;
            s.update( "IsDelete" );
        }

        public void Insert( SpiderImport s ) {
            s.insert();
        }

        public void Update( SpiderImport s ) {
            s.update();
        }


    }

}
