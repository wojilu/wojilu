using System;
using System.Collections;

using wojilu.ORM;

namespace wojilu.Test.Orm.Entities {

    public class TValidateData : ObjectBase<TValidateData> {
        [NotNull]
        public string Body { get; set; }
    }

    public class TValidateData2 : ObjectBase<TValidateData2> {
        [NotNull( "请填写内容" )]
        public string Body { get; set; }
    }

    public class TValidateData3 : ObjectBase<TValidateData3> {

        [Email]
        public string Email { get; set; }
    }

    public class TValidateData4 : ObjectBase<TValidateData4> {

        [Email( "请正确填写电子邮件" )]
        public string Email { get; set; }
    }

    public class TValidateData5 : ObjectBase<TValidateData5> {

        [Unique]
        public string Name { get; set; }
    }

    public class TValidateData6 : ObjectBase<TValidateData6> {

        [Unique( "用户名重复" )]
        public string Name { get; set; }
    }
}
