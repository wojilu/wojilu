/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;


namespace wojilu.Apps.Content.Domain {

    [Serializable]
    public class ContentImg : ObjectBase<ContentImg> {


        public ContentPost Post { get; set; }
        public String ImgUrl { get; set; }
        public String Description { get; set; }
        public DateTime Created { get; set; }

        public String GetImgOriginal() {
            return sys.Path.GetPhotoOriginal( this.ImgUrl );
        }

        public String GetThumbS() {
            return sys.Path.GetPhotoThumb( this.ImgUrl );
        }

        public Boolean IsWebPic() {

            if (strUtil.IsNullOrEmpty( this.ImgUrl )) return true;
            if (this.ImgUrl.ToLower().StartsWith( "http://" )) return true;
            if (this.ImgUrl.StartsWith( "/" )) return true;

            return false;
        }

    }
}

