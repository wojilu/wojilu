/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;


namespace wojilu.Apps.Shop.Domain {

    [Serializable]
    public class ShopItemImg : ObjectBase<ShopItemImg> {


        public ShopItem Item { get; set; }
        public String ImgUrl { get; set; }
        public String Description { get; set; }
        public DateTime Created { get; set; }

        public String GetImgUrl() {
            return sys.Path.GetPhotoOriginal( this.ImgUrl );
        }

        public String GetThumb() {
            return sys.Path.GetPhotoThumb( this.ImgUrl );
        }

    }
}

