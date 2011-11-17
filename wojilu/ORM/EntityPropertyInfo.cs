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
using System.Collections.Generic;
using System.Reflection;
using wojilu.Reflection;

namespace wojilu.ORM {

    /// <summary>
    /// 实体类某个属性的元数据信息
    /// </summary>
    [Serializable]
    public class EntityPropertyInfo {

        public EntityPropertyInfo() {
            this.IsList = false;
            this.IsAbstractEntity = false;
            this.IsEntity = false;
            this.ValidationAttributes = new List<ValidationAttribute>();
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 对应的数据列名
        /// </summary>
        public String ColumnName { get; set; }

        /// <summary>
        /// 属性的类型，比如是int，还是string等等
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 属性信息(系统自带的元数据)
        /// </summary>
        public PropertyInfo Property { get; set; }

        /// <summary>
        /// 本属性所属的实体类信息，比如Blog有一个属性Title，则Title这个属性的ParentEntityInfo就是Blog
        /// </summary>
        public EntityInfo ParentEntityInfo { get; set; }

        /// <summary>
        /// 当本属性是实体属性时，此实体属性的信息。比如Blog的实体属性Category的EntityInfo。如果不是实体属性，则为null
        /// </summary>
        public EntityInfo EntityInfo { get; set; }

        /// <summary>
        /// 是否保存到数据库(是否打上了NotSave批注)
        /// </summary>
        public Boolean SaveToDB { get; set; }

        /// <summary>
        /// 是否是列表类型，列表类型不会保存到数据库
        /// </summary>
        public Boolean IsList { get; set; }

        /// <summary>
        /// 是否是实体类属性
        /// </summary>
        public Boolean IsEntity { get; set; }

        /// <summary>
        /// 是否是抽象类型实体
        /// </summary>
        public Boolean IsAbstractEntity { get; set; }

        /// <summary>
        /// 当前属性的 ColumnAttribute
        /// </summary>
        public ColumnAttribute SaveAttribute { get; set; }

        /// <summary>
        /// 当前属性的 LongTextAttribute
        /// </summary>
        public LongTextAttribute LongTextAttribute { get; set; }

        /// <summary>
        /// 当前属性的 MoneyAttribute
        /// </summary>
        public MoneyAttribute MoneyAttribute { get; set; }

        /// <summary>
        /// 当前属性的 DecimalAttribute
        /// </summary>
        public DecimalAttribute DecimalAttribute { get; set; }

        /// <summary>
        /// 当前属性的 DefaultAttribute
        /// </summary>
        public DefaultAttribute DefaultAttribute { get; set; }

        /// <summary>
        /// 当前属性的 ValidationAttribute 的列表
        /// </summary>
        public List<ValidationAttribute> ValidationAttributes { get; set; }

        /// <summary>
        /// 当前属性的赋值/取值器，可以避免反射的低效
        /// </summary>
        internal IPropertyAccessor PropertyAccessor { get; set; }

        internal static EntityPropertyInfo Get( PropertyInfo property ) {

            EntityPropertyInfo ep = new EntityPropertyInfo();

            object[] arrAttr = property.GetCustomAttributes( typeof( ValidationAttribute ), true );
            foreach (Object at in arrAttr) {
                ep.ValidationAttributes.Add( at as ValidationAttribute );
            }

            ep.SaveAttribute = ReflectionUtil.GetAttribute( property, typeof( ColumnAttribute ) ) as ColumnAttribute;
            ep.LongTextAttribute = ReflectionUtil.GetAttribute( property, typeof( LongTextAttribute ) ) as LongTextAttribute;
            ep.MoneyAttribute = ReflectionUtil.GetAttribute( property, typeof( MoneyAttribute ) ) as MoneyAttribute;
            ep.DecimalAttribute = ReflectionUtil.GetAttribute( property, typeof( DecimalAttribute ) ) as DecimalAttribute;
            ep.DefaultAttribute = ReflectionUtil.GetAttribute( property, typeof( DefaultAttribute ) ) as DefaultAttribute;

            ep.Property = property;
            ep.Name = property.Name;
            ep.Type = property.PropertyType;
            ep.SaveToDB = !property.IsDefined( typeof( NotSaveAttribute ), false );

            if (property.PropertyType is IList) {
                ep.IsList = true;
                ep.SaveToDB = false;
            }


            return ep;
        }

        /// <summary>
        /// 获取obj的当前属性的值
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Object GetValue( Object target ) {
            return PropertyAccessor.Get( target );
        }

        /// <summary>
        /// 给obj的当前属性赋值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public void SetValue( Object target, Object value ) {
            PropertyAccessor.Set( target, value );
        }

        /// <summary>
        /// 是否是长文本
        /// </summary>
        public Boolean IsLongText {
            get {
                if (Type != typeof( String )) return false;
                return LongTextAttribute != null || ((SaveAttribute != null) && (SaveAttribute.Length > 255));
            }
        }

        /// <summary>
        /// 获取属性的label(用在表单中)
        /// </summary>
        public String Label {
            get {
                if (SaveAttribute == null) return Name;
                if (strUtil.IsNullOrEmpty( SaveAttribute.Label )) return Name;
                return SaveAttribute.Label;
            }
        }

    }
}

