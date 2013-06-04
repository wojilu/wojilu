using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Data;
using System.Collections.Generic;

using NUnit.Framework;

using wojilu.ORM;
using wojilu.Serialization;
using System.Threading;
using System.Data.OleDb;
using wojilu.Data;
using wojilu.Test.Orm.Entities;

namespace wojilu.Test.Orm.Utils {

    public class wojiluOrmTestInit {

        private static readonly ILog logger = LogManager.GetLogger( typeof( wojiluOrmTestInit ) );

        public static void InitMetaData() {

            SpeedUtil.Start();

            MappingClass.Clear();

            int count = MappingClass.Instance.ClassList.Count;

            resetConnection();
            Console.WriteLine( "初始化成功！" );
            Console.WriteLine();
            SpeedUtil.Stop();

        }

        private static void resetConnection() {

            IDbConnection connection = DbContext.getConnection( typeof( TValidateData ) );

            if (connection.State == ConnectionState.Closed) {
                connection.Open();
            }
            else {
                connection.Close();
                connection.Open();
            }
        }

        public static void ClearLog() {
            wojilu.file.Delete( "log.txt" );
        }

        public static void ClearTables() {

            foreach (DictionaryEntry entry in MappingClass.Instance.ClassList) {
                EntityInfo ei = entry.Value as EntityInfo;
                string deleteTable = string.Format( "drop table {0}", ei.TableName );

                wojilu.Data.EasyDB.Execute( deleteTable, wojilu.Data.DbContext.getConnection( ei ) );
            }
        }

    }
}
