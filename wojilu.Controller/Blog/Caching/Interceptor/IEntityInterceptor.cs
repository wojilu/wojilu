using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Controller.Common.Caching {


    public interface IEntityInterceptor {

        void Insert( IEntity entity );
        void Update( IEntity entity );
        void Delete( IEntity entity );

        void UpdateBatch( Type t, string action, string condition );

        void DeleteBatch( Type t, string condition );


    }

}
