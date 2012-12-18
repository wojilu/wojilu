using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace wojilu.Aop {

    public class MethodInvocation : IMethodInvocation {

        public MemberInfo Method { get; set; }
        public Object[] Args { get; set; }
        public Object Target { get; set; }

        public Object Proceed() {
            return rft.CallMethod( Target, AopCoder.baseMethodPrefix + Method.Name, Args );
        }
    }

}
