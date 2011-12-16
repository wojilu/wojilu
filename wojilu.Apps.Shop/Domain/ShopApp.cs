/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Web;
using wojilu.Serialization;
using wojilu.Common.Resource;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Shop.Domain {

    // TODO 每个区块可以选择风格

    [Serializable]
    public class ShopApp : ObjectBase<ShopApp>, IApp, IAccessStatus {

        public ShopApp() {
            this.Style = "#row1_column1 {width:48%;margin:5px 5px 5px 10px;}" + Environment.NewLine
                 + "#row1_column2 {width:48%;margin:5px;}";
        }

        public int OwnerId { get; set; }
        public String OwnerUrl { get; set; }
        public String OwnerType { get; set; }

        // 可视化修改css
        [LongText]
        public String Style { get; set; }

        // 手动定义css样式
        [LongText]
        public String SkinStyle { get; set; }

        private int _skinId;
        // 皮肤
        public int SkinId {
            get { 
                if (_skinId == 0) return 1; 
                return _skinId; 
            }
            set { _skinId = value; }
        } 

        public String Layout { get; set; }

        [TinyInt]
        public int AccessStatus { get; set; }
        public DateTime Created { get; set; }

        [NotSave]
        public string[] RowList {
            get {
                if (strUtil.IsNullOrEmpty( Layout )) {
                    initLayoutString();
                }
                return Layout.Split( new char[] { ',', '/', '|' } );
            }
        }


        public String Settings { get; set; }

        public ShopSetting GetSettingsObj() {
            if (strUtil.IsNullOrEmpty( this.Settings )) return new ShopSetting();
            ShopSetting s = JSON.ToObject<ShopSetting>( this.Settings );
            s.SetDefaultValue();
            return s;
        }

        private void initLayoutString() {
            this.Layout = "2";
            this.update( "Layout" );
        }

        public void DeleteRow( int rowId, int appId ) {
            List<string> list = new List<string>();
            string[] rowList = RowList;

            if (rowId <= rowList.Length) {

                if (rowId < rowList.Length) {
                    ShopSection.updateBatch( "RowId=RowId-1", "RowId>" + rowId + " and AppId=" + appId );
                }
                for (int i = 1; i <= rowList.Length; i++) {
                    if (rowId != i) {
                        list.Add( rowList[i - 1] );
                    }
                }
                String strLayout = string.Empty;
                foreach (String str in list) {
                    strLayout = strLayout + str + "/";
                }
                strLayout = strLayout.TrimEnd( '/' );
                this.Layout = strLayout;
                this.update( "Layout" );
            }

        }

    }
}

