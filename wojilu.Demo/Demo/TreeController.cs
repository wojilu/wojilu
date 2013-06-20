using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Demo {

    public class TreeController : ControllerBase {

        public void Index() {

        }

        public void Simple() {

            List<zNode> nodes = getTreeNodeList();
            set( "jsonData", Json.ToString( nodes ) );
        }

        private List<zNode> getTreeNodeList() {

            List<zNode> list = new List<zNode>();

            list.Add( zNode.New( "普通节点1", "http://www.163.com" ) );
            list.Add( zNode.New( "普通节点2", "http://www.baidu.com" ) );

            list.Add(
                zNode.New( "父节点1" )
                    .AddSub( "sub1", "http://www.qq.com" )
                    .AddSub( "sub2", "http://www.microsoft.com" )
                    .AddSub( "sub3", "http://www.google.com" )
                    .Open()
            );

            list.Add( zNode.New( "普通节点3", "http://www.yahoo.com" ) );

            return list;
        }

    }

}
