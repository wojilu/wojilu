using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Common.Picks;

namespace wojilu.Apps.Forum.Service {

    public class ForumPickService : DataPickService<ForumTopic, ForumPick>, IForumPickService {

    }

}
