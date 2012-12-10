/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using wojilu.Log;

namespace wojilu.Data {

    /// <summary>
    /// 简易数据库操作工具，兼容多种数据库，可执行sql，返回DataReader等
    /// </summary>
    public class EasyDB {

        private static readonly ILog logger = LogManager.GetLogger( typeof( EasyDB ) );

        public static int Execute( String sql, IDbConnection cn ) {
            logger.Info( LoggerUtil.SqlPrefix+"execute sql : " + sql );
            IDbCommand cmd = DataFactory.GetCommand( sql, cn );
            int result = cmd.ExecuteNonQuery();
            logger.Info( "affected : " + result );
            return result;
        }

        public static IDataReader ExecuteReader( String sql, IDbConnection cn ) {
            logger.Info( LoggerUtil.SqlPrefix+"execute sql：" + sql );
            return DataFactory.GetCommand( sql, cn ).ExecuteReader();
        }

        public static Object ExecuteScalar( String sql, IDbConnection cn ) {
            Object result = null;
            logger.Info( LoggerUtil.SqlPrefix+"execute sql：" + sql );
            result = DataFactory.GetCommand( sql, cn ).ExecuteScalar();
            if (result == DBNull.Value) {
                return null;
            }
            return result;
        }

        public static DataTable ExecuteTable( String sql, IDbConnection cn ) {
            DataTable dataTable = new DataTable();
            logger.Info(LoggerUtil.SqlPrefix+ "execute sql：" + sql );
            DataFactory.GetAdapter( sql, cn ).Fill( dataTable );
            return dataTable;
        }

        //-----------------------------------------------------------------------------------------------------------

        public static Hashtable LoadDicFromString( String targetString ) {
            Hashtable hashtable = new Hashtable();
            if (!strUtil.IsNullOrEmpty( targetString )) {
                String target = "";
                String[] strArray = targetString.Split( '=' );
                if (strArray.Length == 2) {
                    target = strArray[1];
                }
                if (strUtil.HasText( target )) {
                    foreach (String item in target.Split( '|' )) {
                        String[] arr = item.Split( ':' );
                        hashtable[arr[0]] = arr[1];
                    }
                }
            }
            return hashtable;
        }

        public static Object LoadFromFile( String absFilePath ) {
            if (!wojilu.IO.File.Exists( absFilePath )) {
                return null;
            }
            IFormatter formatter = new BinaryFormatter();
            Stream serializationStream = new FileStream( absFilePath, FileMode.Open, FileAccess.Read, FileShare.Read );
            Object result = formatter.Deserialize( serializationStream );
            serializationStream.Close();
            return result;
        }

        public static Object LoadFromString( String targetString, Type targetType ) {
            XmlSerializer serializer = new XmlSerializer( targetType );
            TextReader textReader = new StringReader( targetString );
            Object result = serializer.Deserialize( textReader );
            textReader.Close();
            return result;
        }

        public static Object LoadFromXml( String filePath, Type targetType ) {
            if (!System.IO.File.Exists( filePath )) {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer( targetType );
            Stream stream = new FileStream( filePath, FileMode.Open );
            Object result = serializer.Deserialize( stream );
            stream.Close();
            return result;
        }
        //-----------------------------------------------------------------------------------------------------------

        public static String SaveDicToString( Hashtable tbl ) {
            if (tbl == null) {
                return null;
            }
            String str = "HashTable";
            StringBuilder builder = new StringBuilder();
            builder.Append( str );
            builder.Append( "=" );
            foreach (DictionaryEntry entry in tbl) {
                builder.Append( entry.Key.ToString() + ":" + entry.Value.ToString() + "|" );
            }
            return builder.ToString().TrimEnd( new char[] { '|' } );
        }

        public static void SaveToFile( Object target, String absFilePath ) {
            IFormatter formatter = new BinaryFormatter();
            Stream serializationStream = new FileStream( absFilePath, FileMode.Create, FileAccess.Write, FileShare.None );
            formatter.Serialize( serializationStream, target );
            serializationStream.Close();
        }

        public static String SaveToString( Object target ) {
            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer( target.GetType() );
            TextWriter textWriter = new StringWriter( sb );
            serializer.Serialize( textWriter, target );
            textWriter.Close();
            return sb.ToString();
        }

        public static void SaveToXml( String filePath, Object target ) {
            XmlSerializer serializer = new XmlSerializer( target.GetType() );
            TextWriter textWriter = new StreamWriter( filePath );
            serializer.Serialize( textWriter, target );
            textWriter.Close();
        }
        //-----------------------------------------------------------------------------------------------------------


    }
}

