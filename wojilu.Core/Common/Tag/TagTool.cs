/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Common.Tags {


    [Serializable]
    public class TagTool {

        private IAppData _data;
        private String _tagHtmlString;
        private String _tagTextString;

        private List<Tag> _tags;
        private List<DataTagShip> _dataTags;

        public TagTool( IAppData data ) {
            _data = data;
        }


        public String HtmlString {
            get {
                if (_tagHtmlString == null) {
                    _tagHtmlString = TagService.GetHtml( List );
                }
                return _tagHtmlString;
            }
        }


        /// <summary>
        /// 用逗号分开显示
        /// </summary>
        public String TextString {
            get {
                if (_tagTextString == null) {
                    _tagTextString = TagService.GetText( this.List );
                }
                return _tagTextString;
            }
        }

        public List<DataTagShip> DataTagList {
            get {
                if (_dataTags == null) {
                    initDataTagList();
                }
                return _dataTags;
            }
        }

        public List<Tag> List {
            get {
                if (_tags == null) {
                    initDataTagList();
                }
                return _tags;
            }
        }


        public String TagIds {
            get {
                return strUtil.GetIds( this.List );
            }
        }


        public void Save( String rawTagString ) {

            if (strUtil.IsNullOrEmpty( rawTagString )) return;

            if (TagService.tagEqual( rawTagString, this.TextString )) return;
            TagService.SaveDataTag( _data, rawTagString );
            refreshTagCache();
        }

        private void refreshTagCache() {
            _tags = null;
            _tagTextString = null;
            _tagHtmlString = null;
        }


        public void DeleteTags() {
            IList dataTagList = DataTagList;
            foreach (DataTagShip ship in dataTagList) {
                db.delete( ship );
            }
        }

        private void initDataTagList() {

            List<DataTagShip> list = DataTagShip.find( "DataId=" + _data.Id + " and TypeFullName=:tname order by Id" ).set( "tname", _data.GetType().FullName ).list();

            _dataTags = list;

            _tags = new List<Tag>();
            foreach (DataTagShip dt in list) {
                _tags.Add( dt.Tag );
            }

        }


    }
}

