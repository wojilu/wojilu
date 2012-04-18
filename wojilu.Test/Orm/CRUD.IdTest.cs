using System;
using System.Collections;
using System.Collections.Generic;
using wojilu.Test.Orm.Entities;
using wojilu.Data;
using wojilu.Web;
using wojilu.Test.Orm.Utils;
using NUnit.Framework;

namespace wojilu.Test.Orm
{
    /// <summary>
    /// 非自增编号记录加入测试
    /// </summary>
    [TestFixture]
    public class CRUDIdTest
    {
        [TestFixtureSetUp]
        public void InitData()
        {
            // 强制实体键为整型
            DbConfig.Instance.IdType = IdType.Int;

            wojiluOrmTestInit.ClearLog();
            wojiluOrmTestInit.InitMetaData();

            InsertCat();
            UpdateCat();
            GetCat();            
            DeleteCat();
        }

        [TestFixtureTearDown]
        public void clear()
        {
            wojiluOrmTestInit.ClearTables();
            String dbName = "default";
            ConnectionString cnstr = DbConfig.Instance.GetConnectionStringMap()[dbName];
            Console.WriteLine("当前运行数据库类型：" + cnstr.DbType);
            Console.WriteLine("当前运行数据库ID类型：" + DbConfig.Instance.IdType);
            Console.WriteLine("当前运行数据库类型：" + cnstr.StringContent);
            
            // 复位实体键类型
            DbConfig.Instance.IdType = IdType.Auto;            
        }

        public void InsertCat()
        {
            ConsoleTitleUtil.ShowTestTitle("InsertCat");
            
            TCat cat = new TCat();
            cat.Id = 10;
            cat.Name = "国内新闻";
            db.insert(cat);

            cat.Name = "国际新闻";
            cat.Id = 20;
            db.insert(cat);                       

            cat.Name = "金融期货";
            cat.Id = 30;
            db.insert(cat);

            Console.WriteLine("\n添加 cat 成功！\n\n");
        }
        
        public void UpdateCat()
        {
            ConsoleTitleUtil.ShowTestTitle("UpdateCat");

            TCat cat = TCat.findById(10);

            Console.WriteLine("\n读取 TCat{{Id:{0},Name:{1}}} code:{2} \n",cat.Id,cat.Name,cat.GetHashCode());

            cat.Name = "糊播新闻";            
            db.update(cat);

            Console.WriteLine("\n更新 TCat{{Id:{0},Name:{1}}} code:{2} \n", cat.Id, cat.Name, cat.GetHashCode());

            cat = TCat.findById(10);

            Console.WriteLine("\n重新读取 TCat{{Id:{0},Name:{1}}} code:{2} \n", cat.Id, cat.Name, cat.GetHashCode());
        }

        public void GetCat()
        {
            ConsoleTitleUtil.ShowTestTitle("GetCat");

            TCat cat = TCat.findById(10);

            Console.WriteLine("\n读取 TCat{{Id:{0},Name:{1}}} code:{2} \n", cat.Id, cat.Name, cat.GetHashCode());                       
        }

        public void DeleteCat()
        {
            ConsoleTitleUtil.ShowTestTitle("DeleteCat");

            int retval = TCat.delete(30);

            Console.WriteLine("\n删除 {0} 条记录 \n", retval);
        }
    }
}
