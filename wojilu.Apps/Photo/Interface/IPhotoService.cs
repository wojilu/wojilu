using System;
using wojilu.Apps.Photo.Domain;
namespace wojilu.Apps.Photo.Interface {

    public interface IPhotoService {
        void UpdateCommentCount( PhotoApp app );
        void UpdateCount( PhotoApp app );

        System.Collections.Generic.List<PhotoApp> GetAppAll();
    }

}
