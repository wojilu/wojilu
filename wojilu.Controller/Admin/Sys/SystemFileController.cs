/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using wojilu.IO;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Admin.Sys {


    public class CssFileController : SystemFileController {
        public CssFileController() { LayoutControllerType = typeof( SiteSkinController ); }
        public override String getRootPath() { return "/static/css/"; }
    }

    public class SkinFileController : SystemFileController {
        public SkinFileController() { LayoutControllerType = typeof( SiteSkinController ); }
        public override String getRootPath() { return "/static/skin/"; }
    }

    public class ConfigFileController : SystemFileController {
        public override String getRootPath() { return "/framework/config/"; }
        public override String getEditTip() { return lang( "editConfigTip" ); }
    }

    public class CacheFileController : SystemFileController {
        public override String getRootPath() { return "/framework/data/"; }
        public override String getEditTip() { return lang( "editConfigTip" ); }
    }


    public class SystemFileController : ControllerBase {

        public SystemFileController() {
            LayoutControllerType = typeof( SiteConfigController );
        }

        public virtual String getRootPath() {
            return "/";
        }

        public virtual String getEditTip() {
            return lang( "editFileTip" );
        }

        public virtual void afterUpdate() {
        }

        public void Index() {

            String absRootDir = PathHelper.Map( getRootPath() );

            String paramDir = ctx.Get( "dir" );
            Result result = validateDir( paramDir );
            if (result.HasErrors) {
                echo( result.ErrorsHtml );
                return;
            }

            String absCurrentDir = absRootDir;
            if (strUtil.HasText( paramDir )) absCurrentDir = strUtil.Join( absRootDir, paramDir.Replace( "/", "\\" ), "\\" );

            set( "ViewDir", getCurrentPath( absRootDir, absCurrentDir ) );

            if (Directory.Exists( absCurrentDir ) == false) {
                echo( lang( "NotFound404" ) );
                return;
            }

            StringBuilder sb = new StringBuilder();
            String[] dirs = Directory.GetDirectories( absCurrentDir );
            foreach (String rdir in dirs) {
                String dir = getItemName( rdir, absCurrentDir );
                sb.Append( "<div class=\"line dir\">" );
                sb.AppendFormat( "<a href=\"{1}\">{0}</a>", dir, to( Index ) + "?dir=" + getParamItem( rdir, absRootDir ) );
                sb.Append( "</div>" );
            }

            String[] files = Directory.GetFiles( absCurrentDir );
            showFiles( absRootDir, absCurrentDir, sb, files );

            set( "dirAndFiles", sb );
        }

        private void showFiles( String absRootDir, String absCurrentDir, StringBuilder sb, String[] files ) {
            foreach (String rf in files) {
                Boolean isPicFile = isPic( rf );
                String f = getItemName( rf, absCurrentDir );

                if (isPicFile)
                    sb.Append( "<div class=\"line pic\">" );
                else
                    sb.Append( "<div class=\"line file\">" );

                if (isPicFile || isText( rf )) {
                    sb.AppendFormat( "<a href=\"{1}\">{0}</a>", f, to( Edit ) + "?file=" + getParamItem( rf, absRootDir ) );
                }
                else {
                    sb.Append( f );
                }

                sb.Append( "</div>" );
            }
        }

        private String getCurrentPath( String absViewDir, String absCurrentDir ) {

            if (absCurrentDir.EndsWith( "\\" ) == false) absCurrentDir = absCurrentDir + "\\";

            String path = strUtil.TrimStart( absCurrentDir, absViewDir );

            String viewPathName = getViewPathName();
            String rootPathStr = string.Format( "<a href=\"{1}\">{0}</a>", viewPathName, to( Index ) );

            String[] arrPath = path.Split( '\\' );
            StringBuilder sb = new StringBuilder();
            sb.Append( rootPathStr );
            String tpath = "";
            for (int i = 0; i < arrPath.Length; i++) {
                if (strUtil.IsNullOrEmpty( arrPath[i] )) continue;
                tpath += arrPath[i] + "/";
                sb.Append( "/" );
                sb.AppendFormat( "<a href=\"{1}\">{0}</a>", arrPath[i], to( Index ) + "?dir=" + tpath );
            }

            return sb.ToString();
        }

        private String getViewPathName() {
            String[] arrPath = getRootPath().TrimEnd( '/' ).Split( '/' );
            return arrPath[arrPath.Length - 1];
        }

        private String getItemName( String rf, String absCurrentDir ) {
            return strUtil.TrimStart( rf, absCurrentDir ).Replace( "\\", "/" ).TrimStart( '/' );
        }

        protected String getParamItem( String rdir, String absViewDir ) {
            return strUtil.TrimStart( rdir, absViewDir ).Replace( "\\", "/" ).TrimStart( '/' );
        }

        public void Edit() {

            target( Update );

            String currentFile = ctx.Get( "file" );
            Result result = validateFile( currentFile );
            if (result.HasErrors) {
                echo( result.ErrorsHtml );
                return;
            }
            set( "fileName", currentFile );

            String absRootDir = PathHelper.Map( getRootPath() );
            String filePath = PathHelper.CombineAbs( new String[] { absRootDir, currentFile } );
            String absCurrentDir = Path.GetDirectoryName( filePath );

            String currentPathStr = getCurrentPath( absRootDir, absCurrentDir );
            currentPathStr = strUtil.Join( currentPathStr, Path.GetFileName( filePath ) );
            set( "ViewDir", currentPathStr );

            bindRestoreFileCmd( filePath, currentFile );

            if (isPic( filePath )) {
                ctx.SetItem( "ViewDir", currentPathStr );
                ctx.SetItem( "filePath", filePath );
                run( ShowPic );
                return;
            }
            else if (isText( filePath ) == false) {
                ctx.SetItem( "ViewDir", currentPathStr );
                ctx.SetItem( "filePath", filePath );
                run( ShowFile );
                return;
            }

            if (file.Exists( filePath ) == false) {
                echo( lang( "NotFound404" ) );
                return;
            }

            String templateContent = strUtil.EncodeTextarea( file.Read( filePath ) );
            templateContent = templateContent.Replace( "#{", "&#35;{" );
            set( "templateContent", templateContent );
            set( "editTip", this.getEditTip() );
        }



        private bool isPic( String filePath ) {

            String ext = Path.GetExtension( filePath ).ToLower();

            if (".jpg".Equals( ext )) return true;
            if (".jpeg".Equals( ext )) return true;
            if (".gif".Equals( ext )) return true;
            if (".png".Equals( ext )) return true;
            if (".bmp".Equals( ext )) return true;

            return false;
        }

        private bool isText( String filePath ) {

            String ext = Path.GetExtension( filePath ).ToLower();

            //String[] arrType = { ".txt", ".html", ".css", ".jpg" };
            //return new List<String>( arrType ).Contains( ext );

            if (".txt".Equals( ext )) return true;
            if (".html".Equals( ext )) return true;
            if (".htm".Equals( ext )) return true;
            if (".css".Equals( ext )) return true;
            if (".config".Equals( ext )) return true;
            if (".js".Equals( ext )) return true;
            if (".json".Equals( ext )) return true;
            if (".xml".Equals( ext )) return true;
            if (".asp".Equals( ext )) return true;
            if (".aspx".Equals( ext )) return true;
            if (".asax".Equals( ext )) return true;
            if (".cs".Equals( ext )) return true;

            return false;
        }

        public void ShowPic() {

            String filePath = ctx.GetItem( "filePath" ).ToString();
            String ViewDir = ctx.GetItem( "ViewDir" ).ToString();
            set( "ViewDir", ViewDir );

            String picUrl = strUtil.TrimStart( filePath, PathHelper.Map( "/" ) ).Replace( "\\", "/" );
            if (picUrl.StartsWith( "/" ) == false) picUrl = "/" + picUrl;

            set( "picUrl", picUrl );
        }

        public void ShowFile() {

            String filePath = ctx.GetItem( "filePath" ).ToString();
            String ViewDir = ctx.GetItem( "ViewDir" ).ToString();
            set( "ViewDir", ViewDir );
            set( "fileName", Path.GetFileName( filePath ) );

        }

        // 检查非法传入的参数
        private Result validateFile( String currentFile ) {
            Result result = new Result();
            if (strUtil.IsNullOrEmpty( currentFile )) {
                result.Add( lang( "NotFound404" ) );
            }
            if (currentFile.IndexOf( "\\" ) >= 0 || currentFile.IndexOf( ":" ) > 0) {
                result.Add( lang( "NotFound404" ) );
            }
            return result;
        }

        private Result validateDir( String dir ) {
            Result result = new Result();

            if (strUtil.IsNullOrEmpty( dir )) return result;

            if (dir.IndexOf( "\\" ) >= 0 || dir.IndexOf( ":" ) >= 0) {
                result.Add( lang( "NotFound404" ) );
            }

            return result;
        }

        [HttpPost]
        public void Update() {

            String currentFile = ctx.Post( "file" );
            Result result = validateFile( currentFile );
            if (result.HasErrors) {
                echo( result.ErrorsHtml );
                return;
            }

            String absViewDir = PathHelper.Map( getRootPath() );
            String filePath = PathHelper.CombineAbs( new String[] { absViewDir, currentFile } );

            if (file.Exists( filePath ) == false) {
                echo( lang( "NotFound404" ) );
                return;
            }

            String TemplateContent = ctx.PostHtmlAll( "TemplateContent" );
            if (file.Read( filePath.Trim() ).Equals( TemplateContent )) {
                echo( "没有修改内容" );
                return;
            }

            String backupPath = getBackupFilePath( filePath );
            if (file.Exists( backupPath ) == false) {
                file.Write( backupPath, file.Read( filePath ) );
            }

            // 备份前一个版本内容(如果是系统默认内容，也就是第一个版本会自动备份)
            else if (ctx.PostIsCheck( "IsBackupLast" )==1) {
                file.Write( getLastBackupFilePath( filePath ), file.Read( filePath ) );
            }

            file.Write( filePath, TemplateContent );

            afterUpdate();

            echoRedirect( lang( "opok" ) );
        }

        private string getBackupFilePath( string absFilePath ) {

            String ext = Path.GetExtension( absFilePath );
            return strUtil.TrimEnd( absFilePath, ext ) + "_backup" + ext;

        }

        private string getLastBackupFilePath( string absFilePath ) {

            String ext = Path.GetExtension( absFilePath );
            return strUtil.TrimEnd( absFilePath, ext ) + "_backupLast" + ext;

        }

        [HttpPut]
        public void Restore() {

            String currentFile = ctx.Get( "file" );
            Result result = validateFile( currentFile );
            if (result.HasErrors) {
                echo( result.ErrorsHtml );
                return;
            }

            String absViewDir = PathHelper.Map( getRootPath() );
            String filePath = PathHelper.CombineAbs( new String[] { absViewDir, currentFile } );

            if (file.Exists( filePath ) == false) {
                echo( lang( "NotFound404" ) );
                return;
            }

            String backupPath = getBackupFilePath( filePath );
            if (file.Exists( backupPath ) == false) {
                echo( lang( "exBackupExists" ) );
                return;
            }

            file.Write( filePath, file.Read( backupPath ) );
            echoRedirect( lang( "opok" ) );
        }

        private void bindRestoreFileCmd( string filePath, String fileName ) {

            if (isBakupFile( filePath )) {
                set( "disabled", "disabled=\"disabled\"" );
                set( "backupCmd", lang( "editBackupNo" ) );
                set( "restoreCmd", "" );
                return;
            }

            String backupPath = getBackupFilePath( filePath );
            if (file.Exists( backupPath ) == false) {
                set( "restoreCmd", "" );
                set( "disabled", "" );
                set( "backupCmd", lang( "editFirstBackup" ) );
                return;
            }

            String cmdBackup = "<div style=\"padding:5px 0px;\"><label><input name=\"IsBackupLast\" type=\"checkbox\" checked=\"checked\" /> " + lang( "editBackupLast" ) + "</label></div>";
            set( "backupCmd", cmdBackup );
            set( "disabled", "" );

            String lnkRestore = to( Restore ) + "?file=" + fileName;
            String imgBackup = strUtil.Join( sys.Path.Img, "back.gif" );
            set( "restoreCmd", string.Format( "<a href=\"{0}\" class=\"cmd putCmd\"><img src=\"{1}\" /> " + lang( "restoreDefault" ) + "</a>", lnkRestore, imgBackup ) );

        }

        private bool isBakupFile( string filePath ) {
            String fileName = Path.GetFileNameWithoutExtension( filePath );
            return fileName.EndsWith( "_backup" ) || fileName.EndsWith( "_backupLast" );
        }



    }
}

