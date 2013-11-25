using System;
using System.Collections.Generic;
using wojilu.Common.Spider.Domain;

namespace wojilu.Common.Spider.Interface {

    public interface ISpiderTemplateService {

        List<SpiderTemplate> GetAll();
        SpiderTemplate GetById(long id);

        void Start(long id);
        void Stop(long id);

        void Insert( SpiderTemplate s );
        void Update( SpiderTemplate s );
        void Delete(long id);

    }

}
