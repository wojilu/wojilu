using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace wojilu.Aop {

    public class MethodInvocation : IMethodInvocation {

        public MemberInfo Method { get; set; }
        public Object[] Args { get; set; }
        public Object Target { get; set; }

        public Boolean IsSubClass { get; set; }

        public Object Proceed() {

            if (this.IsSubClass) {
                return rft.CallMethod( Target, AopCoderDialect.GetBasePrefixOne() + Method.Name, Args );
            }
            else {
                return rft.CallMethod( Target, Method.Name, Args );
            }

        }


    }

}
