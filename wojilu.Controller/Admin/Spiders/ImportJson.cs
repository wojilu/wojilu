/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Spider.Domain;

namespace wojilu.Web.Controller.Admin.Spiders {

    public class ImportJson {

        public ImportJson( SpiderImport it ) {

            this.Id = it.Id;
            this.Name = it.Name;
            this.DataSrcIds = cvtToString( it.DataSourceIds );
            this.TargetIds = cvtToString( it.SectionIds );
            this.Creator = it.Creator.Name;
            this.IsApprove = it.IsApprove == 1;
        }

        // 转换为字符串之后，jquery 可以直接使用 $("#multiple").val(["Multiple2", "Multiple3"]);
        private List<String> cvtToString( String ids ) {
            int[] arrIds = cvt.ToIntArray( ids );
            List<String> list = new List<string>();
            foreach (int id in arrIds) {
                list.Add( id.ToString() );
            }
            return list;
        }

        public int Id { get; set; }
        public String Name { get; set; }
        public List<String> DataSrcIds { get; set; }
        public List<String> TargetIds { get; set; }
        public String Creator { get; set; }
        public Boolean IsApprove { get; set; }


    }

}
