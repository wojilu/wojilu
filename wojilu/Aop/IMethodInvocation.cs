using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace wojilu.Aop {

    public interface IMethodInvocation {

        MemberInfo Method { get; set; }
        Object[] Args { get; set; }
        Object Target { get; set; }

        Object Proceed();

        Boolean IsSubClass { get; set; }
    }

}
