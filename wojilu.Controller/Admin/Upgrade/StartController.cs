using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using System.Data;

namespace wojilu.Web.Controller.Admin.Upgrade {

    public class StartController : ControllerBase {

        public void Index() {
        }


        public void UpdateTable() {

            if (db.getDatabaseType() == "access") {
                content( getAccessTableInfo() );
                return;
            }

            target( SaveTable );
        }

        private String getAccessTableInfo() {
            return "<div style=\"margin:30px;\">您当前使用的是access数据库，请手工添加、修改如下字段：——<br/>" +

@"[PhotoPost]增加字段SrcName(string文本)/SrcUrl(string文本)/SrcTool(string文本)/ParentId(int数字)/RootId(int数字)/Likes(int数字)/Pins(int数字)	
[Page]增加IsCollapse(int数字)和IsTextNode(int数字)
[PhotoPost]增加SizeInfo(string文本字段)
[SpiderTemplate]增加：DetailClearTag字段(ntext备注)
[ContentApp]、[PollApp]增加字段：CommentCount(int数字)
[Users] 增加 LoginType(int数字), IsBind(int数字), Pins(int数字), Likes(int数字)
[UserConnect] 增加Updated(datetime)
[OpenComment] 增加AppId(int数字)
[ContentPostSection] 增加 SaveStatus(int数字)
[Microblog] 增加字段 SaveStatus(int数字)
[BlogPost] 增加字段 AttachmentCount(int数字)
[Feed/GroupUser/FriendShip]增加字段Ip(string文本)
[BlogPost] 增加字段 SysCategoryId(int数字)
[OpenComment] 增加字段 RawUrl(string文本)
[ContentSection]增加 CssClass(string文本)，MetaKeywords(string文本)，MetaDescription(string文本)

[ContentApp] 的字段 Settings 改成 [ntext备注]
[ForumApp] 的字段 Settings 改成 [ntext备注]
".Replace( "\n", "<br/>" ) + "</div>";
        }

        public void SaveTable() {


            String sql = @"if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Page' AND COLUMN_NAME='IsCollapse') alter table Page add IsCollapse int;
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Page' AND COLUMN_NAME='IsTextNode') alter table Page add IsTextNode int;

if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='PhotoPost' AND COLUMN_NAME='SrcName') alter table PhotoPost add SrcName nvarchar(250);
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='PhotoPost' AND COLUMN_NAME='SrcUrl') alter table PhotoPost add SrcUrl nvarchar(250);
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='PhotoPost' AND COLUMN_NAME='SrcTool') alter table PhotoPost add SrcTool nvarchar(250);
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='PhotoPost' AND COLUMN_NAME='SizeInfo') alter table PhotoPost add SizeInfo nvarchar(250);
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='PhotoPost' AND COLUMN_NAME='Likes') alter table PhotoPost add Likes int;
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='PhotoPost' AND COLUMN_NAME='Pins') alter table PhotoPost add Pins int;
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='PhotoPost' AND COLUMN_NAME='RootId') alter table PhotoPost add RootId int;
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='PhotoPost' AND COLUMN_NAME='ParentId') alter table PhotoPost add ParentId int;

if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='SpiderTemplate' AND COLUMN_NAME='DetailClearTag') alter table SpiderTemplate add DetailClearTag ntext;

if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ContentApp' AND COLUMN_NAME='CommentCount') alter table ContentApp add CommentCount int;
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ContentPostSection' AND COLUMN_NAME='SaveStatus') alter table ContentPostSection add SaveStatus int;
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ContentSection' AND COLUMN_NAME='CssClass') alter table ContentSection add CssClass nvarchar(250);
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ContentSection' AND COLUMN_NAME='MetaKeywords') alter table ContentSection add MetaKeywords nvarchar(250);
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ContentSection' AND COLUMN_NAME='MetaDescription') alter table ContentSection add MetaDescription nvarchar(250);

if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='PollApp' AND COLUMN_NAME='CommentCount') alter table PollApp add CommentCount int;

if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Users' AND COLUMN_NAME='LoginType') alter table Users add LoginType int;
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Users' AND COLUMN_NAME='IsBind') alter table Users add IsBind int;
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Users' AND COLUMN_NAME='Likes') alter table Users add Likes int;
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Users' AND COLUMN_NAME='Pins') alter table Users add Pins int;


if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='UserConnect' AND COLUMN_NAME='Updated') alter table UserConnect add Updated DateTime;

if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='OpenComment' AND COLUMN_NAME='AppId') alter table OpenComment add AppId int;
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='OpenComment' AND COLUMN_NAME='RawUrl') alter table OpenComment add RawUrl nvarchar(250);

if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Microblog' AND COLUMN_NAME='SaveStatus') alter table Microblog add SaveStatus int;

if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='BlogPost' AND COLUMN_NAME='AttachmentCount') alter table BlogPost add AttachmentCount int;
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='BlogPost' AND COLUMN_NAME='SysCategoryId') alter table BlogPost add SysCategoryId int;

if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Feed' AND COLUMN_NAME='Ip') alter table Feed add Ip nvarchar(250);
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='GroupUser' AND COLUMN_NAME='Ip') alter table GroupUser add Ip nvarchar(250);
if not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='FriendShip' AND COLUMN_NAME='Ip') alter table FriendShip add Ip nvarchar(250);

alter table ContentApp alter column Settings ntext;
alter table ForumApp alter column Settings ntext;
";

            runsql( sql );


            echoAjaxOk();


        }

        private void runsql( string sql ) {
            IDbCommand cmd = db.getCommand( sql );
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }


    }
}
