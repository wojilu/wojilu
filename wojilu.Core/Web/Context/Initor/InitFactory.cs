using System;
using System.Collections.Generic;
using System.Text;
using wojilu.DI;

namespace wojilu.Web.Context.Initor {

    public class InitFactory {

        public static IContextInit GetViewer() {
            return ObjectContext.GetByType( typeof( ViewerInit ) ) as IContextInit;
        }

        public static IContextInit GetOwner() {
            return ObjectContext.GetByType( typeof( OwnerInit ) ) as IContextInit;
        }

        public static IContextInit GetController() {
            return ObjectContext.GetByType( typeof( ControllerInit ) ) as IContextInit;
        }

        public static IContextInit GetApp() {
            return ObjectContext.GetByType( typeof( AppInit ) ) as IContextInit;
        }

        public static IContextInit GetOnlineUser() {
            return ObjectContext.GetByType( typeof( OnlineUserInit ) ) as IContextInit;
        }


    }

}
