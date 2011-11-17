/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;


namespace wojilu.Common.Feeds.Interface {


    public interface IFeed {

        int Id { get; set; }

        User Creator { get; set; }
        String DataType { get; set; }

        String TitleTemplate { get; set; }
        String TitleData { get; set; }

        String BodyTemplate { get; set; }
        String BodyData { get; set; }
        String BodyGeneral { get; set; }

        DateTime Created { get; set; }


    }

}
