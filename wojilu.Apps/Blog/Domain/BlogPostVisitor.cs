/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Common.Visitors;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Apps.Blog.Domain {

    [Serializable]
    public class BlogPostVisitor : ObjectBase<BlogPostVisitor>, IDataVisitor {

        public User Visitor { get; set; }
        public BlogPost Target { get; set; }

        public DateTime Created { get; set; }



        public void setVisitor( IMember member ) {
            this.Visitor = member as User;
        }

        public IUser getVisitor() {
            return this.Visitor;
        }

        public void setTarget( IAppData data ) {
            this.Target = data as BlogPost;
        }

        public Type getTargetType() {
            return typeof( BlogPost );
        }

    }

}
