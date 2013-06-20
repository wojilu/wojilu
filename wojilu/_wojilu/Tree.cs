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
using System.Web;
using wojilu.Web;

namespace wojilu {

    /// <summary>
    /// 树状节点接口
    /// </summary>
    public interface INode {

        /// <summary>
        /// 节点的 Id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// 节点的名称
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// 上级节点的 Id
        /// </summary>
        int ParentId { get; set; }
    }

    /// <summary>
    /// 节点绑定器
    /// </summary>
    public interface INodeBinder {
        String Bind( INode node );
        Boolean IsOpen( INode node );
        String GetTarget( INode node );
        String GetUrl( INode node );
        string GetName( INode node );
    }

    /// <summary>
    /// 树状节点(将 T 做了封装，便于操作)
    /// </summary>
    /// <typeparam name="T">节点必须实现了 INode 接口</typeparam>
    public class Node<T> where T : INode {

        public Node() {
        }

        public Node( T node ) {
            _rawNode = node;
        }

        private T _rawNode;

        /// <summary>
        /// 获取原始节点数据
        /// </summary>
        /// <returns></returns>
        public T getNode() {
            return _rawNode;
        }

        //-----------------------------------------------------

        private Tree<T> _tree;

        public void setTree( Tree<T> tree ) {
            _tree = tree;
        }

        private Node<T> _parent;

        /// <summary>
        /// 获取上级节点
        /// </summary>
        /// <returns></returns>
        public Node<T> getParent() {
            if (this.getNode().ParentId == 0) return null;
            if (_parent == null) {
                _parent = _tree.FindById( this.getNode().ParentId );
            }
            return _parent;
        }

        private int _depth = -1;

        /// <summary>
        /// 获取节点的深度
        /// </summary>
        /// <returns></returns>
        public int getDepth() {

            if (_depth < 0) {

                if (this.getNode().ParentId == 0) {
                    _depth = 0;
                }
                else {

                    if (this.getParent() == null) {
                        _depth = 0;
                    }
                    else {

                        _depth = this.getParent().getDepth() + 1;
                    }
                }

            }

            return _depth;
        }

        private List<Node<T>> _children = new List<Node<T>>();

        /// <summary>
        /// 获取所有子节点
        /// </summary>
        /// <returns></returns>
        public List<Node<T>> getChildren() {
            return _children;
        }

        internal void addChildren( Node<T> node ) {
            _children.Add( node );
        }

        //-------------------------------------------------------------

        private Node<T> _prevNode;
        private Node<T> _nextNode;

        /// <summary>
        /// 获取前一个节点
        /// </summary>
        /// <returns></returns>
        public Node<T> getPrev() {
            return _prevNode;
        }

        /// <summary>
        /// 获取后一个节点
        /// </summary>
        /// <returns></returns>
        public Node<T> getNext() {
            return _nextNode;
        }

        internal void setPrev( Node<T> node ) {
            _prevNode = node;
        }

        internal void setNext( Node<T> node ) {
            _nextNode = node;
        }

        internal Boolean indent() {
            if (getPrev() == null) return true;
            return getDepth() > getPrev().getDepth();
        }

        internal Boolean outdent() {
            if (getPrev() == null) return false;
            return getDepth() < getPrev().getDepth();
        }

        internal int getOutdentCount() {
            return getPrev().getDepth() - getDepth();
        }



    }

    /// <summary>
    /// 树状结构
    /// </summary>
    /// <typeparam name="T">节点必须实现了 INode 接口</typeparam>
    public class Tree<T> where T : INode {

        private static readonly ILog logger = LogManager.GetLogger( "wojilu.Tree" );

        private List<T> _rawList;

        public Tree( List<T> nodeList ) {
            _rawList = nodeList;
            initProxyList();
        }

        //------------------------------------------------------------

        /// <summary>
        /// 根据 Id 检索节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Node<T> FindById( int id ) {
            return getById( id );
        }

        /// <summary>
        /// 根据 Id 获取它的上级节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Node<T> FindParent( int id ) {
            Node<T> proxy = getById( id );
            if (proxy == null) return default( Node<T> );
            return proxy.getParent();
        }

        /// <summary>
        /// 根据 Id，获取它的节点路径(从根级开始到当前节点)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Node<T>> FindPath( int id ) {

            List<Node<T>> nodePath = new List<Node<T>>();

            Node<T> proxy = getById( id );
            if (proxy == null) return nodePath;

            nodePath.Add( proxy );

            Node<T> currentNode = proxy;
            while (true) {
                Node<T> parent = currentNode.getParent();
                if (parent == null) break;

                nodePath.Add( parent );
                currentNode = parent;
            }

            nodePath.Reverse();

            return nodePath;
        }

        /// <summary>
        /// 获取所有根节点
        /// </summary>
        /// <returns></returns>
        public List<Node<T>> FindRoots() {
            return getRoots();
        }

        /// <summary>
        /// 根据 Id，获取所有子节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Node<T>> FindChildren( int id ) {
            Node<T> proxy = getById( id );
            if (proxy == null) return new List<Node<T>>();
            return proxy.getChildren();
        }

        private List<Node<T>> _allOrdered;

        /// <summary>
        /// 获取所有节点(经过排序)
        /// </summary>
        /// <returns></returns>
        public List<Node<T>> FindAllOrdered() {

            if (_allOrdered == null) {

                List<Node<T>> results = new List<Node<T>>();
                List<Node<T>> roots = this.FindRoots();
                foreach (Node<T> node in roots) {
                    addSubProxyNodes( results, node );
                }

                _allOrdered = results;

            }
            return _allOrdered;
        }

        private void addSubProxyNodes( List<Node<T>> results, Node<T> parentnode ) {
            results.Add( parentnode );
            List<Node<T>> subnodes = parentnode.getChildren();
            foreach (Node<T> node in subnodes) {
                addSubProxyNodes( results, node );
            }
        }


        //------------------------------------------------------------

        /// <summary>
        /// 根据 Id 获取节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById( int id ) {
            Node<T> proxy = getById( id );
            if (proxy != null) return proxy.getNode();
            return default( T );
        }

        /// <summary>
        /// 根据 Id 获取节点的深度
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetDepth( int id ) {
            Node<T> proxy = getById( id );
            if (proxy != null) return proxy.getDepth();
            return 0;
        }

        /// <summary>
        /// 根据 Id 获取上级节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetParent( int id ) {
            Node<T> proxy = getById( id );
            if (proxy == null) return default( T );
            Node<T> parentProxy = proxy.getParent();
            if (parentProxy != null) return parentProxy.getNode();
            return default( T );
        }

        /// <summary>
        /// 根据 Id，获取节点的路径(从根级开始到当前节点)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<T> GetPath( int id ) {

            List<T> nodePath = new List<T>();

            Node<T> proxy = getById( id );
            if (proxy == null) return nodePath;

            nodePath.Add( proxy.getNode() );

            Node<T> tempNode = proxy;
            while (true) {
                Node<T> parent = tempNode.getParent();
                if (parent == null) break;

                nodePath.Add( parent.getNode() );
                tempNode = parent;
            }

            nodePath.Reverse();

            return nodePath;
        }

        /// <summary>
        /// 根据 Id，获取所有下级节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<T> GetChildren( int id ) {
            Node<T> proxy = getById( id );
            if (proxy == null) return new List<T>();
            List<Node<T>> children = proxy.getChildren();
            List<T> results = new List<T>();
            foreach (Node<T> px in children) results.Add( px.getNode() );
            return results;
        }

        /// <summary>
        /// 获取所有根节点
        /// </summary>
        /// <returns></returns>
        public List<T> GetRoots() {

            List<Node<T>> nodes = this.FindRoots();
            List<T> results = new List<T>();
            foreach (Node<T> px in nodes) {
                results.Add( px.getNode() );
            }
            return results;
        }

        private List<T> _allOrderedNode;

        /// <summary>
        /// 获取所有排序过的节点
        /// </summary>
        /// <returns></returns>
        public List<T> GetAllOrdered() {

            if (_allOrderedNode == null) {

                List<Node<T>> nodes = this.FindAllOrdered();
                List<T> results = new List<T>();
                foreach (Node<T> px in nodes) {
                    results.Add( px.getNode() );
                }

                _allOrderedNode = results;

            }
            return _allOrderedNode;
        }

        //----------------------------------------------------------------

        /// <summary>
        /// 树状下拉列表控件
        /// </summary>
        /// <param name="dropName">下拉列表name</param>
        /// <param name="selectValue">当前选定的值</param>
        /// <returns></returns>
        public String DropList( String dropName, int selectValue ) {
            return DropList( dropName, selectValue, 0, null );
        }

        /// <summary>
        /// 获取下拉列表select
        /// </summary>
        /// <param name="dropName">下拉列表的name和id</param>
        /// <param name="selectValue">当前选中项的值</param>
        /// <param name="nodeId">当select用于设置父节点之时，此参数表示将节点自己排除在下拉列表之外，防止将自己作为自己的父节点</param>
        /// <param name="rootSelectName">根节点(并不存在，但你可以给它取个名称)</param>
        /// <returns></returns>
        public String DropList( String dropName, int selectValue, int nodeId, String rootSelectName ) {

            List<Node<T>> list = this.FindAllOrdered();

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat( "<select name=\"{0}\" id=\"{0}\">", dropName );
            int selval = getSelectedValue( dropName, selectValue );

            if (strUtil.HasText( rootSelectName )) {
                String strSel = selectValue == 0 ? "selected" : "";
                builder.AppendFormat( "<option value=\"{0}\" {1}>{2}</option>", 0, strSel, rootSelectName );
                builder.AppendLine();
            }

            for (int i = 0; i < list.Count; i++) {

                if (list[i].getNode().Id == nodeId) continue;

                int depth = list[i].getDepth();
                String name = getBlank( depth ) + list[i].getNode().Name;

                String strSel = (selval == list[i].getNode().Id ? "selected" : "");
                builder.AppendFormat( "<option value=\"{0}\" {1}>{2}</option>", list[i].getNode().Id, strSel, name );
                builder.AppendLine();
            }
            builder.Append( "</select>" );
            return builder.ToString();

        }

        private static String getBlank( int depth ) {
            StringBuilder builder = new StringBuilder();
            int length = depth * 4;
            for (int i = 0; i <= length; i++) {
                builder.Append( "&nbsp;" );
            }
            return builder.ToString();
        }

        private static int getSelectedValue( String dropName, int val ) {
            if (CurrentRequest.getHttpMethod().Equals( "POST" )) {
                return cvt.ToInt( CurrentRequest.getForm( dropName ) );
            }
            return val;
        }

        /// <summary>
        /// 获取树状结构的 html
        /// </summary>
        /// <param name="treeId"></param>
        /// <returns></returns>
        public String RenderList( String treeId ) {
            return RenderList( treeId, true, null, 0 );
        }

        public List<zNode> GetZNodeList( INodeBinder binder) {

            List<zNode> results = new List<zNode>();

            List<Node<T>> list = this.FindAllOrdered();

            for (int i = 0; i < list.Count; i++) {

                Node<T> node = list[i];
                if (node.getParent() != null) continue;
                results.Add( getZNode( node, binder ) );
            }

            return results;
        }

        private zNode getZNode( Node<T> node, INodeBinder binder ) {

            zNode x = new zNode();

            String name = binder.GetName( node.getNode() );
            if (strUtil.IsNullOrEmpty( name )) name = node.getNode().Name;

            x.name = name;
            x.url = binder.GetUrl( node.getNode() );
            x.open = binder.IsOpen( node.getNode() );
            x.target = binder.GetTarget( node.getNode() );

            if (node.getChildren().Count>0) {

                foreach (Node<T> sub in node.getChildren()) {
                    zNode subZ = getZNode( sub, binder );
                    x.AddSub( subZ );
                }

            }

            return x;
        }

        /// <summary>
        /// 获取树状结构的 html
        /// </summary>
        /// <param name="treeId"></param>
        /// <param name="showChildren"></param>
        /// <param name="binder"></param>
        /// <param name="currentNodeId"></param>
        /// <returns></returns>
        public String RenderList( String treeId, Boolean showChildren, INodeBinder binder, int currentNodeId ) {

            List<Node<T>> list = this.FindAllOrdered();

            StringBuilder builder = new StringBuilder();
            int indent = 0;

            for (int i = 0; i < list.Count; i++) {

                Node<T> node = list[i];

                // 是否添加 ul 以及是否隐藏
                if (i == 0) {
                    builder.Append( "<ul class=\"wTree\" id=\"" );
                    builder.Append( treeId );
                    builder.Append( "\">" );
                    indent++;
                }
                else if (node.indent()) {

                    String displayCls = getDisplayCls( showChildren, currentNodeId, node.getNode().Id );

                    builder.Append( "<ul" );
                    builder.Append( displayCls );
                    builder.Append( ">" );
                    indent++;
                }
                else if (node.outdent()) {
                    int outdentCount = node.getOutdentCount();
                    for (int x = 0; x < outdentCount; x++) {
                        builder.Append( "</ul>" );
                        indent--;
                    }
                }

                // li 的 class
                builder.Append( "<li" );
                String cls = getListClass( showChildren, currentNodeId, node );
                if (strUtil.HasText( cls )) {
                    builder.Append( " class=\"" );
                    builder.Append( cls );
                    builder.Append( "\"" );
                }
                builder.Append( ">" );

                String item = (binder == null ? node.getNode().Name : binder.Bind( node.getNode() ));
                builder.Append( item );
                builder.Append( "</li>" );
            }

            for (int y = 0; y < indent; y++) {
                builder.Append( "</ul>" );
            }

            return builder.ToString();
        }

        private String getListClass( Boolean showChildren, int currentNodeId, Node<T> node ) {

            String cls = "";

            if (node.getChildren().Count > 0) {
                cls += "parentNode";
                if (isOpenedNode( currentNodeId, node.getNode().Id, showChildren ))
                    cls += " collapseNode";
                else
                    cls += " expandNode";
            }
            if (node.getNode().Id == currentNodeId) cls += " currentNode";
            return cls.Trim();
        }

        private String getDisplayCls( Boolean showChildren, int currentNodeId, int nodeId ) {

            if (showChildren) return "";

            List<Node<T>> path = FindPath( currentNodeId );
            foreach (Node<T> parent in path) {

                if (parent.getNext() == null) continue;

                if (nodeId == parent.getNext().getNode().Id) return "";
            }

            return " class=\"hide\"";
        }

        private Boolean isOpenedNode( int currentNodeId, int nodeId, Boolean showChildren ) {

            if (showChildren) return true;

            List<Node<T>> path = FindPath( currentNodeId );
            foreach (Node<T> parent in path) {
                if (nodeId == parent.getNode().Id) return true;
            }
            return false;
        }

        //----------------------------------------------------------------

        private Node<T> getById( int id ) {

            if (getIdCache().ContainsKey( id ) == false) return default( Node<T> );

            return getIdCache()[id];
        }

        //----------------------------------------------------------------

        private List<Node<T>> _proxyList;
        private Dictionary<int, Node<T>> _idcache;
        private List<Node<T>> _roots;

        private List<Node<T>> getNodeList() {
            return _proxyList;
        }

        private Dictionary<int, Node<T>> getIdCache() {
            return _idcache;
        }

        private List<Node<T>> getRoots() {
            return _roots;
        }

        private void initProxyList() {

            _proxyList = new List<Node<T>>();
            _idcache = new Dictionary<int, Node<T>>();
            _roots = new List<Node<T>>();

            // 缓存id
            foreach (T node in _rawList) {

                Node<T> proxy = new Node<T>( node );
                proxy.setTree( this );
                _proxyList.Add( proxy );
                _idcache.Add( node.Id, proxy );
            }

            // 缓存children
            foreach (Node<T> node in _proxyList) {

                if (node.getNode().ParentId == 0) {
                    _roots.Add( node );
                }
                else {

                    Node<T> p = node.getParent();

                    if (p == null) {
                        logger.Error( string.Format( "parent does not exist: id={0}, name={1}, parentId={2}", node.getNode().Id, node.getNode().Name, node.getNode().ParentId ) );
                    }
                    else {

                        p.addChildren( node );
                    }
                }
            }

            // 排序并缓存pre和next
            List<Node<T>> orderedList = FindAllOrdered();

            for (int i = 0; i < orderedList.Count; i++) {

                if (i == 0) continue;

                Node<T> node = orderedList[i];
                Node<T> pre = orderedList[i - 1];

                node.setPrev( pre );
                pre.setNext( node );
            }

        }


    }
}
