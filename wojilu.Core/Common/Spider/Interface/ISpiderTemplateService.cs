using System;
using System.Collections.Generic;
using wojilu.Common.Spider.Domain;

namespace wojilu.Common.Spider.Interface {

    public interface ISpiderTemplateService {

        List<SpiderTemplate> GetAll();
        SpiderTemplate GetById( int id );

        void Start( int id );
        void Stop( int id );

        void Insert( SpiderTemplate s );
        void Update( SpiderTemplate s );
        void Delete( int id );

    }

}
