/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Collections;
using wojilu.ORM;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Security;

namespace wojilu.Common.Security.Utils {

    public class ActionVo {
        public String Name { get; set; }
        public String Value { get; set; }
        public String Checked { get; set; }
    }


    public class SecurityTool {

        public SecurityTool( ISecurity objSecurity, ISecurityAction securityAction ) {
            _objSecurity = objSecurity;
            _securityString = objSecurity.Security;
            objAction = securityAction;
        }

        public SecurityTool( ISecurity objSecurity, ISecurityAction securityAction, IList roles ) {
            _objSecurity = objSecurity;
            _securityString = objSecurity.Security;
            objAction = securityAction;
            _rolesAll = roles;
        }

        public SecurityTool( ISecurity objSecurity, ISecurityAction securityAction, IList allActions, IList roles ) {
            _objSecurity = objSecurity;
            _securityString = objSecurity.Security;
            objAction = securityAction;
            _actionsAll = allActions;
            _rolesAll = roles;
        }

        protected String _securityString;
        protected ISecurity _objSecurity;
        protected IDictionary _roleActionsAll;
        public ISecurityAction objAction;
        private IList _actionsAll;

        public IList GetActionAll() {
            if( _actionsAll == null )
                return objAction.findAll();
            return _actionsAll;
        }

        private IList _rolesAll;

        public IList GetActionsByRole( IRole role ) {
            if (role == null) return new ArrayList();
            return getActionsByKey( SecurityString.GetRoleKey( role.Role.GetType().FullName, role.Role.Id ) );
        }


        protected IList getActionsByKey( String key ) {
            IDictionary actionAll = GetRoleActionsAll();
            if (actionAll[key] == null) return new ArrayList();
            return actionAll[key] as IList;
        }

        public virtual IDictionary GetRoleActionsAll() {

            if (_roleActionsAll != null) return _roleActionsAll;
            if (strUtil.IsNullOrEmpty( _securityString )) return new Hashtable();

            _roleActionsAll = new Hashtable();
            IList actions = GetActionAll();
            string[] arrRolePermission = _securityString.Split( SecurityString.roleSeperator );

            if (arrRolePermission.Length > 0) {
                for (int i = 0; i < arrRolePermission.Length; i++) {

                    if (strUtil.IsNullOrEmpty( arrRolePermission[i] )) continue;

                    SecurityString ss = new SecurityString( arrRolePermission[i] );
                    if (ss.IsError()) continue;

                    _roleActionsAll.Add( ss.GetKey(), ss.GetActions( actions ) );

                }
            }

            return _roleActionsAll;
        }

        //-------------------------------------------

        public virtual void SaveActionAll( string[] actionIds ) {

            Hashtable results = new Hashtable();
            foreach (String actionId in actionIds) {

                string[] arrItem = actionId.Split( '_' );
                if (arrItem.Length != 3) continue;

                String typeFullName = arrItem[0];
                int roleId = cvt.ToInt( arrItem[1] );
                int aid = cvt.ToInt( arrItem[2] );
                if (roleId < 0 || aid <= 0) continue;
                if (Entity.GetType(typeFullName) == null) continue;

                String rkey = SecurityString.GetRoleKey( typeFullName, roleId );
                addActionToRole( results, rkey, aid );

            }

            _roleActionsAll = results;

            savePermissions();
        }

        protected void addActionToRole( Hashtable results, String rkey, int aid ) {

            IList actions = results[rkey] as IList;
            if (actions == null) {
                actions = new ArrayList();
                results[rkey] = actions;
            }

            ISecurityAction action = objAction.GetById( aid );
            if (action != null) actions.Add( action );
        }

        //-------------------------------------------

        protected virtual void savePermissions() {

            StringBuilder sb = new StringBuilder();

            // 默认使用 SiteRole
            IList roles = GetRoles();
            foreach (IRole role in roles) {
                IList roleActions = GetActionsByRole( role );

                //SecurityString ss = new SecurityString( role.TypeFullName, role.TypeId, roleActions );
                SecurityString ss = new SecurityString( role.Role.GetType().FullName, role.Role.Id, roleActions );

                sb.Append( ss.ToString() );
                sb.Append( SecurityString.roleSeperator );
            }

            _securityString = sb.ToString();
            if (strUtil.HasText( _securityString )) _securityString = _securityString.TrimEnd( SecurityString.roleSeperator );
            _objSecurity.Security = _securityString;

            _objSecurity.update();
        }


        //-------------------------------------------

        protected Boolean hasAction( IList actions, ISecurityAction action ) {

            foreach (ISecurityAction ma in actions) {
                if (ma.Id == action.Id) return true;
            }
            return false;
        }

        public IList GetRoles() {
            if (_rolesAll != null) return _rolesAll;
            IList roles = new SiteRole().findAll();
            return roles;
        }

        public Boolean IsForbiddenAction( String controllerAndActionPath ) {

            if (strUtil.IsNullOrEmpty( controllerAndActionPath )) return false;

            IList list = GetActionAll();
            foreach (ISecurityAction a in list) {

                if (a == null) continue;
                if (a.Url.IndexOf( controllerAndActionPath ) >= 0) return true;
            }

            return false;
        }

        //----------------------------------------------------------

        public IList GetCheckBoxList( IRole role ) {

            IList actions = GetActionAll();
            String inputName = objAction.GetType().Name;

            IList results = new ArrayList();
            foreach (ISecurityAction a in actions) {

                if (a == null) continue;
                ActionVo av = new ActionVo();
                av.Name = a.Name;
                av.Value = getInputValue( role, a );
                if (HasAction( role, a )){
                    av.Checked = "checked=\"checked\"";
                }
                results.Add( av );
            }
            return results;
        }

        // TODO 删除此方法
        public String GetActionStringAll( IRole role ) {

            StringBuilder sb = new StringBuilder();
            sb.Append( "<table><tr>" );

            IList actions = GetActionAll();
            String inputName = objAction.GetType().Name;

            int i = 0;
            foreach (ISecurityAction a in actions) {
                if (a == null) continue;
                if (i % 9 == 0 && i > 0) sb.Append( "</tr></tr>" );

                String valString = getInputValue( role, a );


                sb.AppendFormat( "<td title=\"{0}\"><input name=\"{1}\" id=\"{2}\" type=\"checkbox\" value=\"{2}\" ", a.Name, inputName, valString );
                if (HasAction( role, a ))
                    sb.AppendFormat( "checked=\"checked\" /><label for=\"{1}\">{0}</lable></td>", a.Name, valString );
                else
                    sb.AppendFormat( "/><label for=\"{1}\">{0}</lable></td>", a.Name, valString );


                i++;
            }
            sb.Append( "</tr></table>" );
            return sb.ToString();
        }

        public Boolean HasAction( IRole role, ISecurityAction a ) {

            IList actions = GetActionsByRole( role );
            foreach (ISecurityAction action in actions) {
                if (action.Id == a.Id) return true;
            }
            return false;
        }

        private String getInputValue( IRole role, ISecurityAction action ) {
            //return string.Format( "{0}_{1}_{2}", role.TypeFullName, role.TypeId, action.Id );
            return string.Format( "{0}_{1}_{2}", role.Role.GetType().FullName, role.Role.Id, action.Id );

        }

    }

}
