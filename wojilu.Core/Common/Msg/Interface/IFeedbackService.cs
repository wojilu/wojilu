/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Common.Feeds.Interface;
using wojilu.Common.Msg.Domain;

namespace wojilu.Common.Msg.Interface {

    public interface IFeedbackService {

        IFeedService feedService { get; set; }
        INotificationService nfService { get; set; }

        Feedback GetById( int id );
        List<Feedback> GetRecent( int count, int userId );
        DataPage<Feedback> GetPageList( int userId );

        void Insert( Feedback f );
        void Reply( Feedback parent, Feedback feedback );

        void Delete( Feedback f );

    }

}
