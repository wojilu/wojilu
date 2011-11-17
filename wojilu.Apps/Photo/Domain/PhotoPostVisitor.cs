/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Common.Visitors;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;

namespace wojilu.Apps.Photo.Domain {

    [Serializable]
    public class PhotoPostVisitor : ObjectBase<PhotoPostVisitor>, IDataVisitor {

        public User Visitor { get; set; }
        public PhotoPost Target { get; set; }
        public DateTime Created { get; set; }


        public void setVisitor( IMember member ) {
            this.Visitor = member as User;
        }

        public IUser getVisitor() {
            return this.Visitor;
        }

        public void setTarget( IAppData data ) {
            this.Target = data as PhotoPost;
        }

        public Type getTargetType() {
            return typeof( PhotoPost );
        }


    }

}
