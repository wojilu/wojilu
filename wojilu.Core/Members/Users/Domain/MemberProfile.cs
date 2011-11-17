/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.ORM;
using System.Collections.Generic;

namespace wojilu.Members.Users.Domain {

    [Serializable]
    public class MemberProfile : ObjectBase<MemberProfile> {



        public String Address { get; set; }

        public String Mobile { get; set; }

        public String Zip { get; set; }

        public String District { get; set; }

        [Column( Length = 50 )]
        public String Answer { get; set; }

        [LongText]
        public int Body { get; set; }

        public String Book { get; set; }

        public String Company { get; set; }

        public int ContactCondition { get; set; }

        [LongText]
        public String Description { get; set; }

        public String Eat { get; set; }

        public int EmailNotify { get; set; }

        public int Hair { get; set; }

        public int Height { get; set; }

        public String IM { get; set; }

        public String Job { get; set; }

        public String Movie { get; set; }

        public String Music { get; set; }

        // 公告
        [LongText]
        public String Notice { get; set; } 

        public String OtherHobby { get; set; }

        public String OtherInfo { get; set; }

        public String Purpose { get; set; }

        [Column( Length = 50 )]
        public String Question { get; set; }


        public int Sexuality { get; set; }

        public int Sleeping { get; set; }

        public int Smoking { get; set; }

        public String Sport { get; set; }

        //空间主题说明（副标题）
        public String Subject { get; set; }

        public String Tel { get; set; }

        public String WebSite { get; set; }

        public int Weight { get; set; }


        public List<String> GetTags() {

            List<String> list = new List<string>();
            addTags( list, this.Music );
            addTags( list, this.Movie );
            addTags( list, this.Book );
            addTags( list, this.Sport );
            addTags( list, this.Eat );
            addTags( list, this.OtherHobby );

            return list;
        }

        private void addTags( List<string> list, string info ) {

            if (strUtil.IsNullOrEmpty( info )) return;

            String[] arrTags = info.Split( new char[] { ',', '，' } );

            foreach (String tag in arrTags) {

                if (strUtil.IsNullOrEmpty( tag )) continue;

                list.Add( tag );
            }


        }


    }
}

