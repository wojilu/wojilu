/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Common.Msg.Domain {

    [Serializable]
    public class MessageStats {
        private int _all;
        private int _new;
        private int _sended;
        private int _trash;

        public int All {
            get {
                return _all;
            }
            set {
                _all = value;
            }
        }

        public int New {
            get {
                return _new;
            }
            set {
                _new = value;
            }
        }

        public int Sended {
            get {
                return _sended;
            }
            set {
                _sended = value;
            }
        }

        public int Trash {
            get {
                return _trash;
            }
            set {
                _trash = value;
            }
        }
    }
}

