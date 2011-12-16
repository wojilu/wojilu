using System;
using System.Collections.Generic;
using System.Text;

using wojilu.IO;
using wojilu.Drawing;
using wojilu.Web.Utils;

using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Domain;
using wojilu.Web;

namespace wojilu.Apps.Shop.Service
{
    public class ShopDeliverService:IShopDeliverService
    {
        public virtual DataPage<ShopDeliverContacts> FindContactPage(int userid)
        {
            return ShopDeliverContacts.findPage("CreatorId=" + userid);
        }

        public virtual DataPage<ShopDeliverContacts> FindContactPage(int userid, String key)
        {
            if (strUtil.IsNullOrEmpty(key))
                return ShopDeliverContacts.findPage("CreatorId=" + userid);
            else
                return ShopDeliverContacts.findPage("CreatorId=" + userid + " and Title like '%" + key + "%'");
        }
        public virtual List<ShopDeliverContacts> FindContact(int userid) {
            return ShopDeliverContacts.find("CreatorId=" + userid + " and Enabled=1").list();
        }
        public virtual List<ShopDeliverContacts> FindContact(int userid, String key)
        {
            if (strUtil.IsNullOrEmpty(key))
                return ShopDeliverContacts.find("CreatorId=" + userid).list();
            else
                return ShopDeliverContacts.find("CreatorId=" + userid + " and (Title LIKE '%" + key + "%' or Contact LIKE '%" + key + "%' or AddressFirst LIKE '%" + key + "%' or AddressSecond LIKE '%" + key + "%') and Enabled=1").list();
        }
        public virtual ShopDeliverContacts LoadContact(int attid)
        {
            return ShopDeliverContacts.findById(attid);
        }
        public virtual ShopDeliverContacts LoadContactDef(int userid)
        {
            return ShopDeliverContacts.find("CreatorId=" + userid + " and IsDefault=1 and Enabled=1").first();
        }
        public virtual ShopDeliverContacts LoadContact(int userid, string name) {
            return ShopDeliverContacts.find("CreatorId=" + userid + " and Contact='"+name+"' and Enabled=1").first();
        }
        public virtual void UpdateContact(ShopDeliverContacts att)
        {
            if (att.IsDefault == 1)
            {
                db.updateBatch<ShopDeliverContacts>("set IsDefault=0", "Id!=" + att.Id);
            }
            att.update();
        }
        public virtual void InsertContact(ShopDeliverContacts att)
        {
            if (att.IsDefault == 1)
            {
                db.updateBatch<ShopDeliverContacts>("set IsDefault=0", "Id!=0");
            }
            att.insert();
        }

        public virtual DataPage<ShopDeliver> FindDeliverPage()
        {
            return ShopDeliver.findPage(string.Empty);
        }

        public virtual DataPage<ShopDeliver> FindDeliverPage(String key)
        {
            if (strUtil.IsNullOrEmpty(key))
                return ShopDeliver.findPage(string.Empty);
            else
                return ShopDeliver.findPage("Title like '%" + key + "%'");
        }
        public virtual List<ShopDeliver> ListDeliver()
        {
            return ShopDeliver.find(string.Empty).list();
        }

        public virtual List<ShopDeliver> ListDeliver(String key)
        {
            if (strUtil.IsNullOrEmpty(key))
                return ShopDeliver.find(string.Empty).list();
            else
                return ShopDeliver.find("Title like '%" + key + "%'").list();
        }

        public virtual void DeleteDeliver(ShopDeliver f)
        {
            //int providerId = f.Id;
            f.delete();
            //ShopItem.deleteBatch("ProviderId=" + providerId);

        }

        public virtual DataPage<ShopDeliverProvider> FindProviderPage()
        {
            return ShopDeliverProvider.findPage(string.Empty);
        }

        public virtual DataPage<ShopDeliverProvider> FindProviderPage(String key)
        {
            if (strUtil.IsNullOrEmpty(key))
                return ShopDeliverProvider.findPage(string.Empty);
            else
                return ShopDeliverProvider.findPage("Title like '%" + key + "%'");
        }

        public virtual void DeleteProvider(ShopDeliverProvider f)
        {
            //int providerId = f.Id;
            f.delete();
            //ShopItem.deleteBatch("ProviderId=" + providerId);

        }
    }
}
