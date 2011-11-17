/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu;
using wojilu.ORM;

namespace wojilu.Common.Msg.Domain {


    [Serializable]
    public class MessageDraft : ObjectBase<MessageDraft> {
        private String _body;
        private DateTime _created;
        private int _senderId;
        private String _title;
        private String _toName;

        public MessageDraft() {
        }

        public MessageDraft( int id ) {
            base.Id = id;
        }

        [LongText]
        public String Body {
            get {
                return _body;
            }
            set {
                _body = value;
            }
        }

        public DateTime Created {
            get {
                return _created;
            }
            set {
                _created = value;
            }
        }

        public int SenderId {
            get {
                return _senderId;
            }
            set {
                _senderId = value;
            }
        }

        public String Title {
            get {
                return _title;
            }
            set {
                _title = value;
            }
        }

        public String ToName {
            get {
                return _toName;
            }
            set {
                _toName = value;
            }
        }
    }
}

