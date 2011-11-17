///*
// * Copyright 2010 www.wojilu.com
// * 
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// * 
// *      http://www.apache.org/licenses/LICENSE-2.0
// * 
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//using System;
//using System.Collections.Generic;
//using System.Text;
//using wojilu.DI;

//namespace wojilu.Web.Mvc {

//    internal class ActionCacheDB {

//        internal static readonly Dictionary<String, List<IActionCache>> Caches = loadCaches();


//        private static Dictionary<String, List<IActionCache>> loadCaches() {

//            Dictionary<String, List<IActionCache>> dic = new Dictionary<String, List<IActionCache>>();

//            foreach (KeyValuePair<String, Type> kv in ObjectContext.Instance.TypeList) {

//                if (kv.Value.IsAbstract) continue;

//                if (rft.IsInterface( kv.Value, typeof( IActionCache ) )) {

//                    IActionCache obj = (IActionCache)ObjectContext.CreateObject( kv.Value );
//                    Dictionary<Type, String> actions = obj.GetRelatedActions();
//                    addActions( dic, actions, obj );
//                }

//            }

//            return dic;
//        }

//        private static void addActions( Dictionary<String, List<IActionCache>> dic, Dictionary<Type, String> actions, IActionCache obj ) {

//            foreach (KeyValuePair<Type, String> kv in actions) {

//                String[] arrActions = kv.Value.Split( '/' );
//                foreach (String action in arrActions) {

//                    String key = kv.Key.FullName + "_" + action;

//                    List<IActionCache> clist = dic.ContainsKey( key ) ? dic[key] : new List<IActionCache>();
//                    clist.Add( obj );

//                    dic[key] = clist;

//                }

//            }

//        }

//    }





//}
