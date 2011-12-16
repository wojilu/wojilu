using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Shop.Domain;

namespace wojilu.Apps.Shop.Interface
{
    public interface IShopCategoryService
    {
        ShopCategory GetById(int id);
        List<ShopCategory> GetAll();
        List<ShopCategory> GetRootList();
        List<ShopCategory> GetSubCategories();
        List<ViewCategory> GetSubCatsForSelect();
        String GetSubCatsJson();
        List<ShopCategory> GetByParentId(int parentId);
        string GetName(int id);
        string GetParentName(int id);
        int GetParentId(int id);
    }
}
