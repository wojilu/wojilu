/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Sites.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Interface;
using wojilu.Web.Mvc;
using wojilu.Web.UI;

namespace wojilu.Members.Sites.Service {

    public class SiteSkinService : ISiteSkinService {

        public List<SiteSkin> GetSysAll() {
            List<SiteSkin> list = GetAll();
            List<SiteSkin> results = new List<SiteSkin>();
            foreach (SiteSkin skin in list) {
                if (skin.Name.Equals( customName ) == false) results.Add( skin );
            }
            return results;
        }

        private List<SiteSkin> GetAll() {
            return cdb.findAll<SiteSkin>();
        }

        public String GetSkin() {
            return getSkin( config.Instance.Site.SkinId, MvcConfig.Instance.CssVersion );
        }

        public String GetSkin( int querySkinId, String cssVersion ) {

            int skinId = (querySkinId == 0 ? getDefaultSkinId() : querySkinId);
            return getSkin( skinId, cssVersion );
        }

        private String getSkin( int skinId, String cssVersion ) {
            SiteSkin skin = GetById( skinId );

            if (skin == null) skin = GetById( 1 );

            String skinPath = skin.GetSkinPath() + "?v=" + cssVersion;
            String result = string.Format( "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", skinPath );
            if (skin.Name.Equals( customName ) && strUtil.HasText( skin.Body )) {
                result += Environment.NewLine;
                result += string.Format( "<style>{0}</style>", skin.Body );
                result += Environment.NewLine;
            }
            return result;
        }

        private int getDefaultSkinId() {

            int id = config.Instance.Site.SkinId;

            if (id == 0) {

                SiteSkin skin = GetById( 1 );
                if (skin == null) skin = createSkin();

                setCurrentSiteSkin( skin );

                return skin.Id;
            }

            return id;
        }

        private static void setCurrentSiteSkin( SiteSkin skin ) {
            config.Instance.Site.SkinId = skin.Id;
            config.Instance.Site.Update( "SkinId", skin.Id );
        }

        private static SiteSkin createSkin() {
            SiteSkin s = new SiteSkin();
            s.Name = lang.get( "default" );
            s.StylePath = "1/skin.css";
            s.insert();
            return s;
        }


        public SiteSkin GetById( int skinId ) {
            return cdb.findById<SiteSkin>( skinId );
        }

        public SiteSkin GetCurrent() {
            return GetById( config.Instance.Site.SkinId );
        }

        //---------------------------------------------------------------------------------------------------

        public Boolean IsUserCustom() {
            SiteSkin current = GetCurrent();
            if (current == null) return true;
            return current.Name.Equals( customName );
        }

        public void CustomBg( IMember member, string ele, string bgString ) {

            string newStyle = ele + " {"+bgString+"}";
            string oStyle = getCssValues( member );
            string nStyle = CssFormUtil.MergeStyle( oStyle, newStyle );

            saveCustomCss( nStyle );            
        }

        private string getCssValues( IMember owner ) {
            SiteSkin skin = GetCurrent();
            return skin.Body;
        }


        private void saveCustomCss( String newCss ) {

            SiteSkin current = GetCurrent();

            SiteSkin skin = getCustomerCss();

            if (skin == null) {
                skin = new SiteSkin();
                skin.Name = customName;
                skin.ThumbUrl = current.ThumbUrl;
                skin.StylePath = current.StylePath;
                skin.Body = newCss;
                skin.insert();
            }
            else {
                skin.Body = newCss;
                skin.update();
            }

            setCurrentSiteSkin( skin );

        }

        private SiteSkin getCustomerCss() {
            List<SiteSkin> list = GetAll();
            foreach (SiteSkin skin in list) {
                if (skin.Name.Equals( customName )) return skin;
            }
            return null;
        }

        private static readonly String customName = "__custom";

    }

}
