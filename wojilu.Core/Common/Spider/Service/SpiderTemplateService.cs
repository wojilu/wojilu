using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Spider.Domain;
using wojilu.Common.Spider.Interface;

namespace wojilu.Common.Spider.Service {

    public class SpiderTemplateService : ISpiderTemplateService {


        public List<SpiderTemplate> GetAll() {
            return SpiderTemplate.find( "order by OrderId desc, Id asc" ).list();
        }

        public SpiderTemplate GetById( int id ) {
            return SpiderTemplate.findById( id );
        }

        public void Stop( int id ) {
            SpiderTemplate s = SpiderTemplate.findById( id );
            s.IsDelete = 1;
            s.update( "IsDelete" );
        }

        public void Start( int id ) {
            SpiderTemplate s = SpiderTemplate.findById( id );
            s.IsDelete = 0;
            s.update( "IsDelete" );
        }

        public void Delete( int id ) {
            SpiderTemplate s = SpiderTemplate.findById( id );
            s.IsDelete = 1;
            s.update( "IsDelete" );
        }

        public void Insert( SpiderTemplate s ) {
            s.insert();
        }

        public void Update( SpiderTemplate s ) {
            s.update();
        }

    }

}
