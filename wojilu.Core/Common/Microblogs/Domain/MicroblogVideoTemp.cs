using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Net.Video;

namespace wojilu.Common.Microblogs.Domain {

    public class MicroblogVideoTemp : ObjectBase<MicroblogVideoTemp> {

        public MicroblogVideoTemp() {
        }

        public MicroblogVideoTemp( VideoInfo vi ) {
            this.PageUrl = vi.PlayUrl;
            this.FlashUrl = vi.FlashUrl;
            this.Title = vi.Title;
            this.PicUrl = vi.PicUrl;
        }

        public String PageUrl { get; set; }

        public String FlashUrl { get; set; }

        public String PicUrl { get; set; }

        public String Title { get; set; }


        public DateTime Created { get; set; }



    }

}
