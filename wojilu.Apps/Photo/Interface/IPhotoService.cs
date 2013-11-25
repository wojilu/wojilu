using System;
using wojilu.Apps.Photo.Domain;
using System.Collections.Generic;

namespace wojilu.Apps.Photo.Interface {

    public interface IPhotoService {

        PhotoApp GetByUser(long userId);

        void UpdateCount( PhotoApp app );

        List<PhotoApp> GetAppAll();
    }

}
