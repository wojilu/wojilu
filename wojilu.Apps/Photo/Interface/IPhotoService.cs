using System;
using wojilu.Apps.Photo.Domain;
using System.Collections.Generic;

namespace wojilu.Apps.Photo.Interface {

    public interface IPhotoService {

        PhotoApp GetByUser( int userId );

        void UpdateCommentCount( PhotoApp app );
        void UpdateCount( PhotoApp app );

        List<PhotoApp> GetAppAll();
    }

}
