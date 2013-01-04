using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Data;
using wojilu.Common.AppBase;

namespace wojilu.weibo.Domain
{
   public class WeiboType : CacheObject,ISort
    {
       [NotNull]
       [Column(Length=25)]
       public string Name { get; set; }

       /// <summary>
       /// 友好名称
       /// </summary>
       [NotNull]
       [Column(Length=20)]
       public string FriendName { get; set; }

       public int Enable { get; set; }

       [NotNull]
       [Column(Length=50)]
       public string AppKey { get; set; }

       [NotNull]
       [Column(Length=50)]
       public string AppSecret { get; set; }

       /// <summary>
       /// Logo图片地址
       /// </summary>
       [Column(Length=100)]
       public string Logo { get; set; }

       [NotNull] 
       public string CallbackUrl { get; set; }

       public int OrderId
       {
           get;
           set;
       }

       public void updateOrderId()
       {
           this.update();
       }

       public static WeiboType GetById(int id)
       {
           return cdb.findById<WeiboType>(id);
       }

       public static WeiboType GetByName(string name)
       {
           List<WeiboType> types =  cdb.findByName<WeiboType>(name);
           if (types!=null && types.Count==1)
           {
               return types[0];
           }
           return null;
       }

       public static List<WeiboType> GetAll()
       {
           return cdb.findAll<WeiboType>();
       }

       public static void Delete(int id)
       {
           WeiboType w = GetById(id);
           if (w != null)
               cdb.delete(w);
       }
    }
}
