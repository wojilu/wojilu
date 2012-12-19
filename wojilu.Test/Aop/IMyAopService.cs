using System;
namespace wojilu.Test.Aop {
    public interface IMyAopService {
        void GetBy( string name, int id );
        System.Collections.Generic.List<string> GetCat();
        System.Collections.Generic.List<MyCat> GetCat2();
        System.Collections.Generic.List<string> GetDog();
        void NormalMethod();
        void NormalVirtualMethod();
        void Save();
        int Update( int id );
    }
}
