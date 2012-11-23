/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Groups.Interface;
using wojilu.Members.Groups.Service;
using wojilu.Members.Groups.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Groups {

    public class MemberController : ControllerBase {

        public IGroupService groupService { get; set; }
        public IMemberGroupService mgrService { get; set; }

        public MemberController() {
            groupService = new GroupService();
            mgrService = new MemberGroupService();

        }


        public void List() {

            ctx.Page.Title = lang( "memberList" );

            IBlock block = getBlock( "list" );
            DataPage<GroupUser> list = mgrService.GetMembersApproved( ctx.owner.Id );
            foreach (GroupUser gu in list.Results) {
                if (gu.Member == null) continue;
                block.Set( "m.Name", gu.Member.Name );
                block.Set( "m.Face", gu.Member.PicSmall );
                block.Set( "m.Url", Link.ToMember( gu.Member ) );

                String lblValue = "";
                if (gu.Member.Id > 0) 
                    lblValue = strUtil.CutString( gu.Member.Profile.Description, 50 );
                block.Set( "m.Info", lblValue );
                block.Next();
            }
            set( "page", list.PageBar );
        }



    }

}
