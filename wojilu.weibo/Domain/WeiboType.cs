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

       public bool Enable { get; set; }

       [NotNull]
       [Column(Length=50)]
       public string AppKey { get; set; }

       [NotNull]
       [Column(Length=50)]
       public string AppSecret { get; set; }

       [Column(Length=100)]
       public string LogoUrl { get; set; }

       [NotNull] 
       public string CallbackUrl { get; set; }

       [NotNull]
       public string AuthUrl { get; set; }

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

       public static List<WeiboType> GetAll()
       {
           return cdb.findAll<WeiboType>();
       }
    }
}
