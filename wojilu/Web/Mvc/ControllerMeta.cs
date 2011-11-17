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
using System.Reflection;
using wojilu.DI;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Mvc {


    /// <summary>
    /// 缓存 controller 元数据信息
    /// </summary>
    public class ControllerMeta {

        private static readonly Dictionary<String, ControllerMeta> _metas = loadControllerMeta();
        private static readonly Dictionary<String, IActionCache> _actionCacheInfos = loadIActionCaches();
        private static readonly Dictionary<String, IPageCache> _pageCacheInfos = loadIPageCaches();
        private static readonly Dictionary<String, List<IActionCache>> _cacheInfoByUpdate = loadCachesByUpdate();
        private static readonly Dictionary<String, List<IPageCache>> _pageCacheInfoByUpdate = loadPageCacheInfoByUpdate();

        public static Dictionary<String, ControllerMeta> GetMetaDB() {
            return _metas;
        }

        public static ControllerMeta GetController( String controllerType ) {
            ControllerMeta cm;
            _metas.TryGetValue( controllerType, out cm );
            return cm;
        }

        public static IActionCache GetActionCache( Type controllerType, String action ) {
            IActionCache obj;
            _actionCacheInfos.TryGetValue( controllerType.FullName + "_" + action, out obj );
            return obj;
        }

        public static IPageCache GetPageCache( Type controllerType, String action ) {
            IPageCache obj;
            _pageCacheInfos.TryGetValue( controllerType.FullName + "_" + action, out obj );
            return obj;
        }

        public static List<IActionCache> GetActionCacheByUpdate( Type controllerType, String action ) {

            List<IActionCache> relatedCaches;
            String key = controllerType.FullName + "_" + action;
            _cacheInfoByUpdate.TryGetValue( key, out relatedCaches );
            return relatedCaches;
        }

        public static List<IPageCache> GetPageCacheByUpdate( Type actionCacheType ) {

            List<IPageCache> relatedCaches;
            String key = actionCacheType.FullName;
            _pageCacheInfoByUpdate.TryGetValue( key, out relatedCaches );
            return relatedCaches;
        }

        //-----------------------------------------------------------------------------------------------------------

        public Type ControllerType { get; set; }
        public List<Attribute> AttributeList { get; set; }
        public Dictionary<String, ControllerAction> ActionMaps { get; set; }

        //-----------------------------------------------------------------------------------------------------------

        private static Dictionary<String, ControllerMeta> loadControllerMeta() {

            Dictionary<String, ControllerMeta> metas = new Dictionary<String, ControllerMeta>();

            foreach (KeyValuePair<String, Type> kv in ObjectContext.Instance.TypeList) {

                if (kv.Value.IsAbstract) continue;
                if (kv.Value.IsSubclassOf( typeof( ControllerBase ) ) == false) continue;

                ControllerMeta cm = loadControllerSingle( kv.Value );

                metas.Add( cm.ControllerType.FullName, cm );

            }

            return metas;
        }

        private static ControllerMeta loadControllerSingle( Type controllerType ) {

            ControllerMeta cm = new ControllerMeta();
            cm.ControllerType = controllerType;

            object[] attrs = rft.GetAttributes( cm.ControllerType );
            cm.AttributeList = new List<Attribute>();
            foreach (Attribute a in attrs) {
                cm.AttributeList.Add( a );
            }

            MethodInfo[] mi = rft.GetMethods( cm.ControllerType );
            cm.ActionMaps = loadActionMaps( mi );
            return cm;
        }

        private static Dictionary<String, ControllerAction> loadActionMaps( MethodInfo[] mi ) {
            Dictionary<String, ControllerAction> dic = new Dictionary<String, ControllerAction>();
            foreach (MethodInfo m in mi) {
                dic.Add( m.Name, loadControllerAction( m ) );
            }
            return dic;
        }

        private static ControllerAction loadControllerAction( MethodInfo m ) {

            ControllerAction action = new ControllerAction();
            action.MethodInfo = m;
            action.ActionName = m.Name;

            action.AttributeList = new List<Attribute>();
            object[] attrs = rft.GetAttributes( m );

            foreach (Attribute attr in attrs) {
                action.AttributeList.Add( attr );
            }

            return action;
        }

        //---------------------------------------------------------------------------------------------------------


        private static Dictionary<String, IActionCache> loadIActionCaches() {

            Dictionary<String, IActionCache> results = new Dictionary<String, IActionCache>();

            Dictionary<String, ControllerMeta> metas = ControllerMeta.GetMetaDB();

            foreach (KeyValuePair<String, ControllerMeta> kv in metas) {

                ControllerMeta controller = kv.Value;

                foreach (KeyValuePair<String, ControllerAction> caKv in controller.ActionMaps) {

                    ControllerAction action = caKv.Value;

                    foreach (Attribute a in action.AttributeList) {

                        CacheActionAttribute cachedInfo = a as CacheActionAttribute;
                        if (cachedInfo == null) continue;

                        IActionCache obj = (IActionCache)ObjectContext.GetByType( cachedInfo.Type );

                        results.Add( controller.ControllerType.FullName + "_" + action.ActionName, obj );

                    }

                }

            }

            return results;
        }


        private static Dictionary<String, List<IActionCache>> loadCachesByUpdate() {

            Dictionary<String, List<IActionCache>> dicResults = new Dictionary<String, List<IActionCache>>();

            List<Type> subControllers = new List<Type>();

            foreach (KeyValuePair<String, Type> kv in ObjectContext.Instance.TypeList) {

                if (kv.Value.IsAbstract) continue;

                if (kv.Value.IsSubclassOf( typeof( ControllerBase ) ) && kv.Value.BaseType != typeof( ControllerBase )) subControllers.Add( kv.Value );

                if (rft.IsInterface( kv.Value, typeof( IActionCache ) )) {

                    IActionCache obj = (IActionCache)ObjectContext.GetByType( kv.Value );
                    addActions( dicResults, obj );
                }

            }

            return processSubControllers( subControllers, dicResults );
        }

        private static void addActions( Dictionary<String, List<IActionCache>> dic, IActionCache obj ) {

            Dictionary<Type, String> actions = obj.GetRelatedActions();

            foreach (KeyValuePair<Type, String> kv in actions) {


                String[] arrActions = kv.Value.Split( '/' );
                foreach (String action in arrActions) {

                    String key = kv.Key.FullName + "_" + action;

                    List<IActionCache> clist = dic.ContainsKey( key ) ? dic[key] : new List<IActionCache>();
                    if( clist.Contains( obj)==false )
                        clist.Add( obj );

                    dic[key] = clist;
                }
            }


        }

        private static Dictionary<String, List<IActionCache>> processSubControllers( List<Type> subControllers, Dictionary<String, List<IActionCache>> dic ) {


            Dictionary<String, List<IActionCache>> newDic = new Dictionary<String, List<IActionCache>>( dic );

            foreach (Type t in subControllers) {
                
                addBaseTypeActions( dic, newDic, t.BaseType, t );

            }

            return newDic;

        }

        private static void addBaseTypeActions( Dictionary<String, List<IActionCache>> dic, Dictionary<String, List<IActionCache>> newDic, Type baseType, Type subType ) {
            

            foreach (KeyValuePair<String, List<IActionCache>> kv in dic) {
                
                String controller_action = kv.Key;

                String baseControllerInfo = baseType.FullName + "_";

                if (controller_action.StartsWith( baseControllerInfo )) {


                    String actionInfo = strUtil.TrimStart( controller_action, baseControllerInfo );
                    String subController_action = subType.FullName + "_" + actionInfo;


                    if (dic.ContainsKey( subController_action ) == false) {
                        newDic[subController_action] = kv.Value; // 将 controller 父类的相关信息拷贝过来
                    }
                    else {
                        newDic[subController_action] = getMergeActionCaches( kv.Value, dic[subController_action] );
                    }

                }

            }

        }

        private static List<IActionCache> getMergeActionCaches( List<IActionCache> parentActionCaches, List<IActionCache> subActionCaches ) {

            List<IActionCache> results = new List<IActionCache>( subActionCaches );

            foreach (IActionCache ac in parentActionCaches) {
                if (subContains( subActionCaches, ac ) == false) results.Add( ac );
            }

            return results;
        }

        private static bool subContains( List<IActionCache> subActionCaches, IActionCache ac ) {

            foreach (IActionCache subAc in subActionCaches) {

                if (subAc.GetType() == ac.GetType()) return true;

            }

            return false;
        }




        //---------------------------------------------------------------------------------------------------------


        private static Dictionary<String, IPageCache> loadIPageCaches() {

            Dictionary<String, IPageCache> results = new Dictionary<String, IPageCache>();

            Dictionary<String, ControllerMeta> metas = ControllerMeta.GetMetaDB();

            foreach (KeyValuePair<String, ControllerMeta> kv in metas) {

                ControllerMeta controller = kv.Value;

                foreach (KeyValuePair<String, ControllerAction> caKv in controller.ActionMaps) {

                    ControllerAction action = caKv.Value;

                    foreach (Attribute a in action.AttributeList) {

                        CachePageAttribute cachedInfo = a as CachePageAttribute;
                        if (cachedInfo == null) continue;

                        IPageCache obj = (IPageCache)ObjectContext.GetByType( cachedInfo.Type );

                        results.Add( controller.ControllerType.FullName + "_" + action.ActionName, obj );

                    }

                }

            }

            return results;
        }


        private static Dictionary<String, List<IPageCache>> loadPageCacheInfoByUpdate() {

            Dictionary<String, List<IPageCache>> dic = new Dictionary<String, List<IPageCache>>();

            foreach (KeyValuePair<String, Type> kv in ObjectContext.Instance.TypeList) {

                if (kv.Value.IsAbstract) continue;

                if (rft.IsInterface( kv.Value, typeof( IPageCache ) )) {

                    IPageCache obj = (IPageCache)ObjectContext.GetByType( kv.Value );
                    List<Type> actions = obj.GetRelatedActions();
                    addActions( dic, actions, obj );
                }

            }

            return dic;
        }

        private static void addActions( Dictionary<String, List<IPageCache>> dic, List<Type> actionCacheInfos, IPageCache obj ) {

            foreach (Type t in actionCacheInfos) {

                String key = t.FullName;

                List<IPageCache> clist = dic.ContainsKey( key ) ? dic[key] : new List<IPageCache>();
                clist.Add( obj );

                dic[key] = clist;


            }

        }






    }


}
