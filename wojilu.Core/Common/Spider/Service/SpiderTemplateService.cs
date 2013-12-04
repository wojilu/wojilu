using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Spider.Domain;
using wojilu.Common.Spider.Interface;

namespace wojilu.Common.Spider.Service {

    public class SpiderTemplateService : ISpiderTemplateService {


        public virtual List<SpiderTemplate> GetAll() {
            return SpiderTemplate.find( "order by OrderId desc, Id asc" ).list();
        }

        public virtual SpiderTemplate GetById(long id) {
            return SpiderTemplate.findById( id );
        }

        public virtual void Stop(long id) {
            SpiderTemplate s = SpiderTemplate.findById( id );
            s.IsDelete = 1;
            s.update( "IsDelete" );
        }

        public virtual void Start(long id) {
            SpiderTemplate s = SpiderTemplate.findById( id );
            s.IsDelete = 0;
            s.update( "IsDelete" );
        }

        public virtual void Delete(long id) {
            SpiderTemplate s = SpiderTemplate.findById( id );
            s.IsDelete = 1;
            s.update( "IsDelete" );
        }

        public virtual void Insert( SpiderTemplate s ) {
            s.insert();
        }

        public virtual void Update( SpiderTemplate s ) {
            s.update();
        }

    }

}
