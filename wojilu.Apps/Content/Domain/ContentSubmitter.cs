using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Content.Domain {

    // 只存储 Submitter 和 AdvancedSubmitter 两种用户
    [Serializable]
    public class ContentSubmitter : ObjectBase<ContentSubmitter> {

        public User User { get; set; }
        public int AppId { get; set; }
        public int RoleId { get; set; }

        public int PostCount { get; set; } // 投递文章数量

        public DateTime Created { get; set; }
        public DateTime AdvancedCreated { get; set; } // 成为高级记者时间
        
    }

    [Serializable]
    public class ContentSubmitterRole {


        // 4个头衔，2种权限：
        // 1）可以投递，但必须审核，角色名称：见习记者
        // 2）不用审核直接发送，名称：记者
        // 3）不用审核直接发送（有管理权限的），名称：编辑
        // 4）不用审核直接发送（荣誉称号），名称：业界专家/高级记者

        public ContentSubmitterRole() {
            if (strUtil.IsNullOrEmpty( this.NeedApproval )) this.NeedApproval = alang.get( typeof( ContentApp ), "NeedApproval" );
            if (strUtil.IsNullOrEmpty( this.Submitter )) this.Submitter = alang.get( typeof( ContentApp ), "Submitter" );
            if (strUtil.IsNullOrEmpty( this.AdvancedSubmitter )) this.AdvancedSubmitter = alang.get( typeof( ContentApp ), "AdvancedSubmitter" );
            if (strUtil.IsNullOrEmpty( this.Editor )) this.Editor = alang.get( typeof( ContentApp ), "Editor" );
        }

        public String NeedApproval { get; set; }
        public String Submitter { get; set; }
        public String AdvancedSubmitter { get; set; }
        public String Editor { get; set; }

        public static int SubmitterValue = 0;
        public static int AdvancedSubmitterValue = 1;

        public String getName( int roleId ) {
            if (roleId == SubmitterValue) return this.Submitter;
            if (roleId == AdvancedSubmitterValue) return this.AdvancedSubmitter;
            return "";
        }


    }

}
