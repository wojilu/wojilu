using System;
using System.Collections.Generic;
using System.Text;

using wojilu.IO;
using wojilu.Drawing;
using wojilu.Serialization;
using wojilu.Web.Utils;

using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Domain;
using wojilu.Web;

namespace wojilu.Apps.Shop.Service
{
    public class ShopCategoryService:IShopCategoryService
    {

        public virtual ShopCategory GetById(int id)
        {
            return cdb.findById<ShopCategory>(id);
        }

        public virtual List<ShopCategory> GetAll()
        {
            List<ShopCategory> list = cdb.findAll<ShopCategory>();
            list.Sort();
            return list;
        }

        public virtual List<ShopCategory> GetRootList()
        {

            List<ShopCategory> list = GetAll();

            List<ShopCategory> results = new List<ShopCategory>();
            foreach (ShopCategory f in list)
            {
                if (f.ParentId == 0) results.Add(f);
            }

            return results;
        }

        public virtual List<ShopCategory> GetSubCategories()
        {
            List<ShopCategory> list = GetAll();

            List<ShopCategory> results = new List<ShopCategory>();
            foreach (ShopCategory f in list)
            {
                if (f.ParentId > 0) results.Add(f);
            }

            return results;
        }

        public virtual List<ViewCategory> GetSubCatsForSelect()
        {

            List<ViewCategory> results = new List<ViewCategory>();
            List<ShopCategory> list = GetSubCategories();
            foreach (ShopCategory f in list)
            {
                ViewCategory c = new ViewCategory();
                c.Id = f.Id;
                c.Name = f.Name;
                c.ParentId = f.ParentId;
                results.Add(c);
            }

            return results;
        }

        public virtual String GetSubCatsJson()
        {

            List<ViewCategory> subcats = GetSubCatsForSelect();
            String jsons = SimpleJsonString.ConvertList(subcats);
            return jsons;
        }

        public virtual List<ShopCategory> GetByParentId(int parentId)
        {
            List<ShopCategory> list = GetAll();

            List<ShopCategory> results = new List<ShopCategory>();
            foreach (ShopCategory f in list)
            {
                if (f.ParentId == parentId) results.Add(f);
            }

            return results;
        }


        public virtual string GetName(int id)
        {
            return cdb.findById<ShopCategory>(id).Name;
        }

        public virtual string GetParentName(int id)
        {
            ShopCategory c = cdb.findById<ShopCategory>(id);
            return cdb.findById<ShopCategory>(c.ParentId).Name;
        }

        public virtual int GetParentId(int id)
        {
            ShopCategory c = cdb.findById<ShopCategory>(id);
            return c.ParentId;
        }
    }
}
