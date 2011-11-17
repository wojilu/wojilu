using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace wojilu.ORM {

    /// <summary>
    /// 验证批注的抽象基类。如果要自定义验证批注，请通过继承此基类扩展
    /// </summary>
    [Serializable, AttributeUsage( AttributeTargets.Property )]
    public abstract class ValidationAttribute : Attribute {

        private String _message;

        /// <summary>
        /// 错误提示信息
        /// </summary>
        public String Message {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// 多国语言化的错误提示信息
        /// </summary>
        public String Lang {
            set {
                _message = lang.get( value );
            }
            get { return _message; }
        }

        /// <summary>
        /// 可以扩展的验证方法
        /// </summary>
        /// <param name="action">当前操作：update或insert</param>
        /// <param name="target">需要验证的实体对象</param>
        /// <param name="info">当前属性信息</param>
        /// <param name="result">验证结果</param>
        public abstract void Validate( String action, IEntity target, EntityPropertyInfo info, Result result );


        public virtual void Validate( String action, Object target, String propertyName, Result result ) {
            if (target == null) return;
            IEntity entity = target as IEntity;
            if (entity == null) throw new NotImplementedException( "NotNullAttribute can only implement to IEntity. Currenty object's type:" + target.GetType().FullName );

            ;

            EntityPropertyInfo ep = Entity.GetInfo( entity ).GetProperty( propertyName );
            Validate( action, entity, ep, result );
        }

    }

}
