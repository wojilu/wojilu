/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Common.Msg.Domain;

namespace wojilu.Common.Msg.Interface {

    public interface IFeedbackService {

        INotificationService nfService { get; set; }

        Feedback GetById( int id );
        List<Feedback> GetRecent( int count, int userId );
        DataPage<Feedback> GetPageList( int userId );

        void Delete( Feedback f );

        void Insert( Feedback feedback, String lnkReplyList );
        void Reply( Feedback parent, Feedback feedback, String lnkReplyList );
    }

}
