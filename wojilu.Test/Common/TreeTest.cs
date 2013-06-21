using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Web.Mvc;
using NUnit.Framework;

namespace wojilu.Test.Common {

    [TestFixture]
    public class TreeTest {






        [Test]
        public void testTreeData() {

            List<Node> nodes = getNodesAll();

            Tree<Node> tree = new Tree<Node>( nodes );

            // GetById
            INode node = tree.GetById( 13 );
            Assert.IsNotNull( node );
            Assert.AreEqual( "node13", node.Name );

            INode nullNode = tree.GetById( 9999 );
            Assert.IsNull( nullNode );

            // GetParent
            INode parent = tree.GetParent( node.Id );
            Assert.IsNotNull( parent );
            Assert.AreEqual( 5, parent.Id );

            // GetDepth
            Assert.AreEqual( 0, tree.GetDepth( 2 ) );
            Assert.AreEqual( 1, tree.GetDepth( 7 ) );
            Assert.AreEqual( 2, tree.GetDepth( 13 ) );
            Assert.AreEqual( 2, tree.GetDepth( 14 ) );

            // GetPath
            List<Node> path = tree.GetPath( 13 );
            Assert.AreEqual( 3, path.Count );

            Assert.AreEqual( 1, path[0].Id );
            Assert.AreEqual( 5, path[1].Id );
            Assert.AreEqual( 13, path[2].Id );


            // GetChildren
            Assert.AreEqual( 0, tree.GetChildren( 4 ).Count );
            Assert.AreEqual( 0, tree.GetChildren( 7 ).Count );

            List<Node> children = tree.GetChildren( 5 );
            Assert.AreEqual( 3, children.Count );
            Assert.AreEqual( 13, children[0].Id );
            Assert.AreEqual( 14, children[1].Id );
            Assert.AreEqual( 15, children[2].Id );

            // GetRoots
            List<Node> roots = tree.GetRoots();
            Assert.AreEqual( 3, roots.Count );
            Assert.AreEqual( 1, roots[0].Id );
            Assert.AreEqual( 2, roots[1].Id );
            Assert.AreEqual( 3, roots[2].Id );

        }

        [Test]
        public void testDroplist() {

            List<Node> nodes = getNodesAll();
            Tree<Node> tree = new Tree<Node>( nodes );

            String rthml = tree.DropList( "myDrop", 0 );

            Console.WriteLine( rthml );

        }

        [Test]
        public void testTreeHtml() {

            List<Node> nodes = getNodesAll();
            Tree<Node> tree = new Tree<Node>( nodes );

            string html = @"<ul class=""wTree"" id=""myTree""><li class=""parentNode expandNode"">node1</li><ul class=""hide""><li>node4</li><li class=""parentNode expandNode"">node5</li><ul class=""hide""><li>node13</li><li>node14</li><li>node15</li></ul><li>node6</li></ul><li class=""parentNode expandNode"">node2</li><ul class=""hide""><li>node7</li><li>node8</li><li>node9</li></ul><li class=""parentNode expandNode"">node3</li><ul class=""hide""><li>node10</li><li>node11</li><li>node12</li></ul></ul>";
            string rthml = tree.RenderList( "myTree", false, null, 0 );

            Assert.AreEqual( html, rthml );

            Console.WriteLine( rthml );
        }

        [Test]
        public void testFormatLoop() {
            
            List<Node> nodes = getNodesAll();

            string results = Html.Tree( nodes, getItemHtml );
            Console.WriteLine( results );
        }

        private string getItemString( INode node, int depth ) {
            string tabPrefix = "";
            for (int i = 0; i < depth; i++) {
                tabPrefix += "\t";
            }
            return tabPrefix + node.Name;
        }

        private string getItemHtml( INode node, int depth ) {
            return string.Format( "<div style='margin-left:{0}px;'>{1}</div>", (depth + 1) * 15, node.Name );
        }

        //------------------------------------------------------------------------

        private List<Node> getNodesAll() {

            List<Node> nodes = new List<Node>();

            nodes.Add( new Node { Id = 1, ParentId = 0, Name = "node1" } );
            nodes.Add( new Node { Id = 2, ParentId = 0, Name = "node2" } );
            nodes.Add( new Node { Id = 3, ParentId = 0, Name = "node3" } );

            nodes.Add( new Node { Id = 4, ParentId = 1, Name = "node4" } );
            nodes.Add( new Node { Id = 5, ParentId = 1, Name = "node5" } );
            nodes.Add( new Node { Id = 6, ParentId = 1, Name = "node6" } );

            nodes.Add( new Node { Id = 7, ParentId = 2, Name = "node7" } );
            nodes.Add( new Node { Id = 8, ParentId = 2, Name = "node8" } );
            nodes.Add( new Node { Id = 9, ParentId = 2, Name = "node9" } );

            nodes.Add( new Node { Id = 10, ParentId = 3, Name = "node10" } );
            nodes.Add( new Node { Id = 11, ParentId = 3, Name = "node11" } );
            nodes.Add( new Node { Id = 12, ParentId = 3, Name = "node12" } );

            nodes.Add( new Node { Id = 13, ParentId = 5, Name = "node13" } );
            nodes.Add( new Node { Id = 14, ParentId = 5, Name = "node14" } );
            nodes.Add( new Node { Id = 15, ParentId = 5, Name = "node15" } );


            return nodes;
        }

    }


    public class Node : INode {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }

        public string Title {
            get { return this.Name + this.Id + "....."; }
        }
    }

    public class SelectBinder : INodeBinder {

        public string Bind( INode node ) {
            return string.Format( "<option value=\"{0}\">{1}</option>" + Environment.NewLine, node.Id, node.Name );
        }


        public bool IsOpen( INode node ) {
            return true;
        }

        public string GetTarget( INode node ) {
            return "";
        }

        public string GetUrl( INode node ) {
            return "";
        }

        public string GetName( INode node ) {
            return node.Name;
        }
    }
}
