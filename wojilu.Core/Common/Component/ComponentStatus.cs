using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Groups.Domain;

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

        internal static List<ComponentStatus> GetDefaultStatusList() {

            List<ComponentStatus> list = new List<ComponentStatus>();
            list.Add( _run );
            list.Add( _close );

            return list;
        }

        /// <summary>
        /// 根据组件的类型得到自定义的状态
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static List<ComponentStatus> GetStatusList( String typeFullName ) {
            return ComponentCustomStatus.GetByType( typeFullName );
        }

        public static String GetStatusName( String typeFullName, int statusId ) {
            List<ComponentStatus> list = GetStatusList( typeFullName );
            foreach (ComponentStatus cs in list) {
                if (cs.Id == statusId) return cs.Name;
            }
            return "";
        }

        public int Id { get; set; }
        public String Name { get; set; }

    }

    internal class ComponentCustomStatus {

        internal static List<ComponentStatus> GetByType( string typeFullName ) {

            if (typeFullName == typeof( Group ).FullName) {

                List<ComponentStatus> list = new List<ComponentStatus>();
                list.Add( ComponentStatus.Run );

                ComponentStatus cs1 = new ComponentStatus();
                cs1.Id = 1;
                cs1.Name = "彻底禁用";
                list.Add( cs1 );

                ComponentStatus cs2 = new ComponentStatus();
                cs2.Id = 2;
                cs2.Name = "(启用)但禁止用户创建群组";
                list.Add( cs2 );

                return list;
            }

            return ComponentStatus.GetDefaultStatusList();
        }
    }

}
