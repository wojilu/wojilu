using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.DI;

namespace wojilu.Common {


    /// <summary>
    /// 提供常用 service 获取快捷方式。比如根据 ownerType 获取 menuService
    /// </summary>
    public class ServiceMap {


        private static Dictionary<Type, IMenuService> _menuServiceMap = getMenuServiceMap();
        private static Dictionary<Type, IMemberAppService> _mappServiceMap = getMemberAppServiceMap();

        public static Dictionary<Type, IMenuService> GetMenuServiceMap() {
            return _menuServiceMap;
        }

        public static Dictionary<Type, IMemberAppService> GetAppServiceMap() {
            return _mappServiceMap;
        }

        /// <summary>
        /// 根据 onwerType 获取 IMenuService
        /// </summary>
        /// <param name="ownerType"></param>
        /// <returns></returns>
        public static IMenuService GetMenuService( Type ownerType ) {

            IMenuService obj = null;
            GetMenuServiceMap().TryGetValue( ownerType, out obj );

            return obj;
        }

        /// <summary>
        /// 根据 onwerType 获取 IMemberAppService
        /// </summary>
        /// <param name="ownerType"></param>
        /// <returns></returns>
        public static IMemberAppService GetUserAppService( Type ownerType ) {

            IMemberAppService obj = null;
            GetAppServiceMap().TryGetValue( ownerType, out obj );

            return obj;
        }



        private static Dictionary<Type, IMemberAppService> getMemberAppServiceMap() {
            Dictionary<Type, IMemberAppService> map = new Dictionary<Type, IMemberAppService>();

            foreach (KeyValuePair<String, Type> kv in ObjectContext.Instance.TypeList) {

                if (kv.Value.IsAbstract) continue;
                if (rft.IsInterface( kv.Value, typeof( IMemberAppService ) ) == false) continue;

                IMemberAppService obj = ObjectContext.CreateObject( kv.Value ) as IMemberAppService;

                map.Add( obj.GetMemberType(), obj );

            }

            return map;
        }

        private static Dictionary<Type, IMenuService> getMenuServiceMap() {

            Dictionary<Type, IMenuService> map = new Dictionary<Type, IMenuService>();

            foreach (KeyValuePair<String, Type> kv in ObjectContext.Instance.TypeList) {

                if (kv.Value.IsAbstract) continue;
                if (rft.IsInterface( kv.Value, typeof( IMenuService ) ) == false) continue;

                IMenuService obj = ObjectContext.CreateObject( kv.Value ) as IMenuService;

                map.Add( obj.GetMemberType(), obj );

            }

            return map;
        }
    }

}
