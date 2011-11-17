/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;

namespace wojilu.Apps.Photo.Domain {

    [Serializable]
    public class PhotoPostPicked : ObjectBase<PhotoPostPicked> {

        public PhotoPost Post { get; set; }
        public int Status { get; set; }
        public String TitleStyle { get; set; }
        public String Abstract { get; set; }

    }



}
