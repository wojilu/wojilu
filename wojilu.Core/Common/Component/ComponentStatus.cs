using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Common {


    public class ComponentStatus {

        private static ComponentStatus _run = new ComponentStatus() { Id = 0, Name = "运行" };
        private static ComponentStatus _close = new ComponentStatus() { Id = 1, Name = "禁用" };

        public static ComponentStatus Run {
            get { return _run; }
        }

        public static ComponentStatus Close {
            get { return _close; }
        }

        public static List<ComponentStatus> GetAllStatus() {


            List<ComponentStatus> list = new List<ComponentStatus>();
            list.Add( _run );
            list.Add( _close );

            return list;
        }

        public static ComponentStatus GetById( int id ) {
            if (id == 0) return _run;
            if (id == 1) return _close;
            return null;
        }

        public static String GetStatusName( int id ) {
            ComponentStatus m = GetById( id );
            return m == null ? "" : m.Name;
        }

        public int Id { get; set; }
        public String Name { get; set; }



    }

}
