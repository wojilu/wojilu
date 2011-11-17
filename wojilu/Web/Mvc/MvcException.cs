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
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// mvc “Ï≥£
    /// </summary>
    public class MvcException : Exception {


        private String msg;
        public String Status { get; set; }

        public MvcException() {
        }

        public MvcException( String msg ) {
            this.msg = msg;
        }

        public MvcException( String httpStatus, String msg ) {
            this.Status = httpStatus;
            this.msg = msg;
        }

        public MvcException( String msg, Exception inner )
            : base( msg, inner ) {
            this.msg = msg;
        }

        public override String Message {
            get {
                return this.msg;
            }
        }

        public Boolean hasStatus() {
            return strUtil.HasText( this.Status );
        }

        public int getStatusCode() {
            if (strUtil.IsNullOrEmpty( this.Status )) return 0;
            return cvt.ToInt( this.Status.Split( ' ' )[0] );
        }


        public override String ToString() {
            return base.ToString() + Environment.NewLine + this.msg;
        }

    }

}
