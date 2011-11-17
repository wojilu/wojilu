/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Common.Resource;
using wojilu.Serialization;

using wojilu.Web;
using wojilu.Web.Context;
using wojilu.Web.Mvc;

namespace wojilu.Config {

    /// <summary>
    /// 配置表单工具
    /// </summary>
    public class SettingFormTool {

        public static void BindTemplate( Template template, IList list, String categoryName, String actionUrl ) {
            template.Set( "settingCategory.Name", categoryName );
            template.Set( "ActionUrl", actionUrl );
            IBlock block = template.GetBlock( "list" );
            foreach (ISetting setting in list) {
                block.Set( "setting.Id", setting.Id );
                block.Set( "setting.Title", setting.Name );
                SetInput( block, setting );
                SetNote( block, setting );
                block.Next();
            }
        }



        private static void SetInput( IBlock block, ISetting setting ) {

            String lbl = "setting.inputControl";
            String inputName = GetInputName( setting.Id );
            String settingValue = setting.SettingValue;
            int width = 500;
            if (setting.DataType == SettingType.Int.ToString()) {
                block.Set( lbl, Html.TextInput( inputName, settingValue, "width:40px;" ) );
            }
            else if (setting.DataType == SettingType.Bool.ToString()) {
                block.Set( lbl, Html.CheckBox( inputName, "", "1", Convert.ToBoolean( setting.SettingValue ) ) );
            }
            else if (setting.DataType == SettingType.Droplist.ToString()) {
                block.Set( lbl, Html.DropList( getDropOptions(setting.Options), inputName, "Name", "Value", setting.ValueInt ) );
            }
            else if (setting.DataType == SettingType.ShortText.ToString()) {
                block.Set( lbl, Html.TextInput( inputName, settingValue, "width:" + width + "px;" ) );
            }
            else if (setting.DataType == SettingType.BigText.ToString()) {
                block.Set( lbl, Html.TextArea( inputName, settingValue, "width:" + width + "px;height:300px;" ) );
            }
            else {
                block.Set( lbl, Html.TextArea( inputName, settingValue, "width:" + width + "px;height:50px;" ) );
            }
        }

        private static PropertyCollection getDropOptions( String Options ) {

            PropertyCollection result = new PropertyCollection();
            if (strUtil.IsNullOrEmpty( Options )) return result;

            Dictionary<String, object> dic = JsonParser.Parse( Options ) as Dictionary<String, object>;
            foreach (KeyValuePair<String, object> pair in dic) {
                result.Add( new PropertyItem( pair.Key, cvt.ToInt( pair.Value ) ) );
            }
            return result;
        }

        private static void SetNote( IBlock block, ISetting setting ) {
            if (strUtil.HasText( setting.Description )) {
                if ((setting.DataType == SettingType.Int.ToString()) || (setting.DataType == SettingType.Bool.ToString())) {
                    block.Set( "setting.Note", "<span class=\"note\">(" + setting.Description + ")</span>" );
                }
                else {
                    block.Set( "setting.Note", "<div class=\"note\">(" + setting.Description + ")</div>" );
                }
            }
            else {
                block.Set( "setting.Note", "" );
            }
        }

        //------------------------------------------------------------------------------------

        public static void UpdateSettings( IList list, MvcContext ctx ) {
            for (int i = 0; i < list.Count; i++) {
                ISettingValue s = list[i] as ISettingValue;
                String target = ctx.Post( GetInputName( s.Id ) );
                if (strUtil.HasText( target )) {
                    if (s.DataType == SettingType.Bool.ToString()) {
                        updateSetting( s, cvt.ToBool( target ).ToString() );
                    }
                    else if (!(s.DataType == SettingType.Int.ToString()) || cvt.IsInt( target )) {
                        updateSetting( s, target );
                    }
                }
            }
        }

        private static void updateSetting( ISettingValue s, String val ) {
            if ((val != null) && (s.SettingValue.ToLower() != val.ToLower())) {
                s.SettingValue = val;
                s.Update( "SettingValue" );
            }
        }

        private static String GetInputName( int settingId ) {
            return ("SettingValue" + settingId);
        }

    }
}

