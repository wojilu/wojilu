/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Members.Interface;
using System.Collections.Generic;
using wojilu.Web.UI;

namespace wojilu.Common.Skins {


    public class SkinService {

        private ISkin objSkin;
        private Object skinTool;

        private Type thisType() { return skinTool.GetType(); }

        public void SetSkin( ISkin skin ) {
            objSkin = skin;
            skinTool = skin;
        }

        public String GetUserSkin( IMember user, int queryStringSkinId, String cssVersion ) {

            int skinId = getSkinId( user, queryStringSkinId );
            ISkin skin = GetById( skinId );

            String skinPath = skin.GetSkinPath() + "?v=" + cssVersion;
            String result = string.Format( "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", skinPath );
            if (skin.MemberId > 0) {
                result += Environment.NewLine;
                result += string.Format( "<style>{0}</style>", skin.Body );
                result += Environment.NewLine;
            }
            return result;
        }

        private int getSkinId( IMember user, int queryStringSkinId ) {
            int skinId = user.TemplateId;
            if (skinId == 0) {
                int defaultSkinId = GetDefaultSkin().Id;
                user.TemplateId = defaultSkinId;
                db.update( user, "TemplateId" );
                skinId = defaultSkinId;
            }

            if (queryStringSkinId > 0) skinId = queryStringSkinId;
            return skinId;
        }

        public int GetSkinCount() {
            return ndb.count( thisType() ); ;
        }

        public ISkin GetById( int id ) {
            return ndb.findById( thisType(), id ) as ISkin;
        }

        public ISkin GetById( int id, int memberId ) {
            ISkin result = GetById( id );
            if (result != null && result.MemberId != memberId) return null;
            return result;
        }

        public ISkin GetDefaultSkin() {
            return GetById( 1 );
        }

        public IPageList GetPage() {
            return ndb.findPage( thisType(), "Status=" + SkinStatus.Public + " and MemberId=0 order by Id", 50 );
        }


        public String GetSkinContent( int skinId ) {
            ISkin skin = GetById( skinId );
            if (skin == null) return null;

            return skin.GetSkinContent();
        }

        public IList GetMy( int userId ) {
            return ndb.find( thisType(), "MemberId=" + userId ).list();
        }

        //---------------------------------------------------------------------------------------

        public void ApplySystemSkin( IMember member, int skinId ) {
            member.TemplateId = skinId;
            db.update( member, "TemplateId" );

            // 清空自定义设置
            ISkin customSkin = getByMember( member.Id );
            if (customSkin != null) {
                customSkin.Body = "";
                db.update( customSkin, "Body" );
            }
        }

        public void Insert( ISkin skin ) {
            db.insert( skin );
        }

        public void Delete( ISkin skin ) {
            db.delete( skin );
        }

        public void Update( ISkin skin ) {
            db.update( skin );
        }

        //---------------------------------------------------------------------------------------


        public void CustomBg( IMember member, String ele, String bgString ) {

            string newStyle = ele + " {" + bgString + "}";
            string oStyle = getCssValues( member );
            string nStyle = CssFormUtil.MergeStyle( oStyle, newStyle );

            saveCustomCss( member, nStyle );
        }

        private string getCssValues( IMember member ) {
            ISkin skin = getByMember( member.Id );
            if (skin == null) return "";
            return skin.Body;
        }

        private void saveCustomCss( IMember member, String newCss ) {

            ISkin skin = getByMember( member.Id );

            if (skin == null) {
                skin = objSkin;
                skin.Name = member.Name;
                skin.MemberId = member.Id;
                skin.Body = newCss;
                db.insert( skin );
            }

            ISkin sysUserSkin = GetById( member.TemplateId );

            skin.Body = newCss;
            skin.ThumbUrl = sysUserSkin.ThumbUrl;
            skin.StylePath = sysUserSkin.StylePath;

            db.update( skin );

            member.TemplateId = skin.Id;

            db.update( member, "TemplateId" );
        }

        private ISkin getByMember( int memberId ) {
            return ndb.find( thisType(), "MemberId=" + memberId ).first() as ISkin;
        }


        public Boolean IsUserCustom( IMember member ) {
            ISkin skin = GetById( member.TemplateId );
            return skin.MemberId>0;
        }
    }
}

