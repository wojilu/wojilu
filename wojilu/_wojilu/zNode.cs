using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu {

    public class zNode {

        public String name { get; set; }
        public String url { get; set; }
        public String target { get; set; }
        public List<zNode> children { get; set; }
        public Boolean open { get; set; }

        public static zNode New( String nodeName ) {
            return new zNode { name = nodeName };
        }

        public static zNode New( String nodeName, String url ) {
            return new zNode { name = nodeName, url = url, target = "_self" };
        }

        public static zNode New( String nodeName, String url, String target ) {
            return new zNode { name = nodeName, url = url, target = target };
        }

        public zNode AddSub( String name, String url ) {

            if (this.children == null) {
                this.children = new List<zNode>();
            }

            this.children.Add( New( name, url ) );

            return this;
        }

        public zNode AddSub( String name ) {
            return AddSub( name, "" );
        }

        public zNode AddSub( zNode node ) {

            if (this.children == null) {
                this.children = new List<zNode>();
            }

            this.children.Add( node );

            return this;
        }

        public zNode Open() {
            this.open = true;
            return this;
        }

    }

}
