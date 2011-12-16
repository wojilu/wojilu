using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Enum;

namespace wojilu.Apps.Shop.Service {

    public class ShopItemHomeService {


        public List<ShopItem> GetPicked( int count ) {

            List<ShopItem> list = ShopItem.find( "PickStatus<" + PickStatus.Focus + " order by PickStatus desc, Id desc" ).list( count );
            ShopItem post = ShopItem.find( "PickStatus=" + PickStatus.Focus ).first();
            list.Sort();

            List<ShopItem> results = new List<ShopItem>();

            addFocusFirst( list, post, results );

            foreach (ShopItem a in list) results.Add( a );

            return results;
        }

        // 在第一条存入头条
        private static void addFocusFirst( List<ShopItem> list, ShopItem post, List<ShopItem> mylist ) {

            if (post != null) {
                mylist.Add( post );
                return;
            }


            if (list.Count > 0) {
                mylist.Add( list[0] );
            }

        }


    }

}
