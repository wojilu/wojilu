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

//using System;
//using System.Text;

//namespace wojilu.Web.Mvc {

//    public class ReturnMsg {

//        private String _msg;
//        private String _returnBox;
//        private String _returnId;
//        private int _returnTime;
//        private String _returnType;
//        private String _returnUrl;
//        private String _returnValue;

//        public override String ToString() {

//            StringBuilder builder = new StringBuilder();
//            builder.Append( "<div style='text-align:center;'>" );
//            builder.AppendFormat( "<div id=\"ajaxReturn\" returnId=\"{0}\" returnTime=\"{1}\" returnUrl=\"{2}\" returnBox=\"{3}\" returnType=\"{4}\" >{5}</div>", ReturnId, ReturnTime, ReturnUrl, ReturnBox, ReturnType, Msg );
//            builder.AppendFormat( "<div id=\"returnValue\">{0}</div>", ReturnValue );
//            builder.Append( "</div>" );
//            return builder.ToString();
//        }

//        public String Msg {
//            get { return _msg; }
//            set { _msg = value; }
//        }

//        public String ReturnBox {
//            get { return _returnBox; }
//            set { _returnBox = value; }
//        }

//        public String ReturnId {
//            get { return _returnId; }
//            set { _returnId = value; }
//        }

//        public int ReturnTime {
//            get { return _returnTime; }
//            set { _returnTime = value; }
//        }

//        public String ReturnType {
//            get { return _returnType; }
//            set { _returnType = value; }
//        }

//        public String ReturnUrl {
//            get { return _returnUrl; }
//            set { _returnUrl = value; }
//        }

//        public String ReturnValue {
//            get { return _returnValue; }
//            set { _returnValue = value; }
//        }

//    }
//}

