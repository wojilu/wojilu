using System;
using System.Collections.Generic;

namespace wojilu.Test.Aop {

    public interface IMyAopService {

        String ServiceName { get; set; }

        void GetBy( string name, int id );
        List<string> GetCat();
        List<MyCat> GetCat2();
        List<string> GetDog();
        void NormalMethod();
        void NormalVirtualMethod();
        void Save();
        int Update( int id );

        int NormalReturnMethod();
        List<String> NormalReturnMethod2();
    }
}
