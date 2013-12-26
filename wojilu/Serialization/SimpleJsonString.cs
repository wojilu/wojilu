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
            return ConvertList( list, false );
        }

        /// <summary>
        /// 将对象列表转换成 json 字符串
        /// </summary>
        /// <param name="list"></param>
        /// <param name="allowNotSave">有NotSave批注的属性，不会序列化</param>
        /// <returns></returns>
        public static String ConvertList( IList list, Boolean allowNotSave ) {
            StringBuilder sb = new StringBuilder( "[\r\n" );
            for (int i = 0; i < list.Count; i++) {
                sb.Append( "\t" );
                sb.Append( ConvertObject( list[i], allowNotSave ) );
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
        public static String ConvertObject( Object obj, Boolean allowNotSave ) {

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

                if (isSkip( info, allowNotSave )) {
                    continue;
                }


                Object propertyValue = ReflectionUtil.GetPropertyValue( obj, info.Name );
                if (propertyValue == null) continue;

                if (info.PropertyType == typeof( int ) || info.PropertyType == typeof( long ) || info.PropertyType == typeof( decimal )) {
                    builder.AppendFormat( "{0}:{1}", info.Name, propertyValue );
                }
                else if (info.PropertyType == typeof( Boolean )) {
                    builder.AppendFormat( "{0}:{1}", info.Name, propertyValue.ToString().ToLower() );
                }
                else if (ReflectionUtil.IsBaseType( info.PropertyType )) {
                    builder.AppendFormat( "{0}:\"{1}\"", info.Name, EncodeQuoteAndClearLine( strUtil.ConverToNotNull( propertyValue ) ) );
                }
                else if (info.PropertyType.IsArray) {
                    builder.AppendFormat( "{0}:{1}", info.Name, JsonString.ConvertArray( propertyValue, allowNotSave ) );
                }
                else if (rft.IsInterface( info.PropertyType, typeof( IList ) )) {
                    builder.AppendFormat( "{0}:{1}", info.Name, JsonString.ConvertList( (IList)propertyValue, false, allowNotSave ) );
                }
                else {
                    builder.AppendFormat( "{0}:{1}", info.Name, JsonString.ConvertObject( propertyValue, false, true, allowNotSave ) );
                }
                builder.Append( ", " );
            }
            return (builder.ToString().Trim().TrimEnd( ',' ) + " }");
        }

        private static Boolean isSkip( PropertyInfo info, Boolean allowNotSave ) {
            if (info.CanRead == false) {
                return true;
            }

            if (info.IsDefined( typeof( NotSerializeAttribute ), false )) {
                return true;
            }

            if (allowNotSave && info.IsDefined( typeof( NotSaveAttribute ), false )) {
                return true;
            }

            return false;
        }

        private static String EncodeQuoteAndClearLine( String src ) {
            return JsonString.ClearNewLine( src );
        }

    }
}
