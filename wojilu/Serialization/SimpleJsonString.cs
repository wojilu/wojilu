using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using wojilu.Reflection;
using wojilu.ORM;

namespace wojilu.Serialization {

    /// <summary>
    /// 将简单的对象转换成 json 字符串
    /// </summary>
    public class SimpleJsonString {

        /// <summary>
        /// 将对象列表转换成 json 字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static String ConvertList( IList list ) {
            StringBuilder sb = new StringBuilder( "[\r\n" );
            for (int i = 0; i < list.Count; i++) {
                sb.Append( "\t" );
                sb.Append( ConvertObject( list[i] ) );
                if (i < list.Count - 1)
                    sb.Append( "," );
                sb.Append( "\r\n" );
            }
            sb.Append( "]" );
            return sb.ToString();
        }

        /// <summary>
        /// 将对象转换成 json 字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String ConvertObject( Object obj ) {

            StringBuilder builder = new StringBuilder();
            builder.Append( "{ " );
            PropertyInfo[] properties = obj.GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance );

            Boolean isIdFind = false;
            Boolean isNameFind = false;
            Object idValue = "";
            Object nameValue = "";
            IList propertyList = new ArrayList();
            foreach (PropertyInfo info in properties) {
                if (info.Name.Equals( "Id" )) {
                    isIdFind = true;
                    idValue = ReflectionUtil.GetPropertyValue( obj, "Id" );
                }
                else if (info.Name.Equals( "Name" )) {
                    isNameFind = true;
                    nameValue = ReflectionUtil.GetPropertyValue( obj, "Name" );
                }
                else {
                    propertyList.Add( info );
                }
            }
            if (isIdFind) builder.AppendFormat( "Id:{0}, ", idValue );
            if (isNameFind) builder.AppendFormat( "Name:\"{0}\", ", nameValue );

            foreach (PropertyInfo info in propertyList) {

                if (info.CanRead == false) {
                    continue;
                }

                if (info.IsDefined( typeof( NotSerializeAttribute ), false )) {
                    continue;
                }

                if (info.IsDefined( typeof( NotSaveAttribute ), false )) {
                    continue;
                }

                Object propertyValue = ReflectionUtil.GetPropertyValue( obj, info.Name );
                if (propertyValue == null) continue;

                if (info.PropertyType == typeof( int ) || info.PropertyType == typeof( decimal )) {
                    builder.AppendFormat( "{0}:{1}", info.Name, propertyValue );
                }
                else if (info.PropertyType == typeof( Boolean )) {
                    builder.AppendFormat( "{0}:{1}", info.Name, propertyValue.ToString().ToLower() );
                }
                else if (ReflectionUtil.IsBaseType( info.PropertyType )) {
                    builder.AppendFormat( "{0}:\"{1}\"", info.Name, EncodeQuoteAndClearLine( strUtil.ConverToNotNull( propertyValue ) ) );
                }
                else if (info.PropertyType.IsArray) {
                    builder.AppendFormat( "{0}:{1}", info.Name, JsonString.ConvertArray( propertyValue ) );
                }
                else if (rft.IsInterface( info.PropertyType, typeof( IList ) )) {
                    builder.AppendFormat( "{0}:{1}", info.Name, JsonString.ConvertList( (IList)propertyValue, false ) );
                }
                else {
                    builder.AppendFormat( "{0}:{1}", info.Name, JsonString.ConvertObject( propertyValue ) );
                }
                builder.Append( ", " );
            }
            return (builder.ToString().Trim().TrimEnd( ',' ) + " }");
        }

        private static String EncodeQuoteAndClearLine( String src ) {
            //return src.Replace( "\"", "\\\"" ).Replace( "\r\n", "" ).Replace( "\n", "" ).Replace( "\r", "" ).Replace( "\r\n", "" );
            return JsonString.ClearNewLine( src );
        }

    }
}
