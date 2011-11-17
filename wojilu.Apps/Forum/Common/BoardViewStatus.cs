/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Apps.Forum.Domain {

    [Serializable]
    public class BoardViewStatus {

        public static readonly int Normal = 0; // vertical
        public static readonly int Horizontal = 1;

        private static List<BoardViewStatus> _allView = getAllView();


        public static String GetDropList( String ctlName, int val ) {

            return Html.RadioList( GetAll(), ctlName, "Name", "Id", val );
        }

        public String Name { get; set; }
        public int Id { get; set; }


        private static List<BoardViewStatus> getAllView() {
            List<BoardViewStatus> list = new List<BoardViewStatus> {
                new BoardViewStatus { Id=0, Name=alang.get(typeof(ForumApp),"boardViewV")},
                new BoardViewStatus { Id=1, Name=alang.get(typeof(ForumApp),"boardViewH")},
            };

            return list;
        }

        public static List<BoardViewStatus> GetAll() {
            return _allView;
        }

    }

}
