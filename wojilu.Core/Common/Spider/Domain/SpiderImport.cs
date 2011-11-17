using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase;

namespace wojilu.Common.Spider.Domain {

    /// <summary>
    /// 导入计划项
    /// </summary>
    public class SpiderImport : ObjectBase<SpiderImport>, ISort {

        public String Name { get; set; }

        /// <summary>
        /// 数据源，SpiderTemplate 的 ids
        /// </summary>
        public String DataSourceIds { get; set; }

        /// <summary>
        /// 需要导入的目标区块的ids
        /// </summary>
        public String SectionIds { get; set; }

        public DateTime Created { get; set; }

        /// <summary>
        /// 默认投递人
        /// </summary>
        public User Creator { get; set; }

        /// <summary>
        /// 最后导入的文章id
        /// </summary>
        public int LastImportId { get; set; }



        /// <summary>
        /// 是否需要审核
        /// </summary>
        public int IsApprove { get; set; }
        
        /// <summary>
        /// 是否暂停
        /// </summary>
        public int IsDelete { get; set; }




        public int OrderId { get; set; }

        public void updateOrderId() {
            this.update( "OrderId" );
        }

    }

}
