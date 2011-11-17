using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.AppBase;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Users.Admin {

    public class UserTopicTagController : ControllerBase {

        public TopicTagService tagService { get; set; }

        public UserTopicTagController() {
            tagService = new TopicTagService();
        }

        public void Index() {

            List<UserTopicTag> tags = tagService.GetByUser( ctx.owner.Id );
            set( "addLink", to( Add ) );
            set( "sortAction", to( SaveSort ) );

            bindList( "list", "data", tags, bindLink );
        }

        private void bindLink( IBlock block, int id ) {
            block.Set( "data.LinkEdit", to( Edit, id ) );
            block.Set( "data.LinkDelete", to( Delete, id ) );
        }


        [HttpPost]
        public virtual void SaveSort() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            UserTopicTag data = tagService.GetTagById( id );

            List<UserTopicTag> list = tagService.GetByUser( ctx.owner.Id );

            if (cmd == "up") {

                new SortUtil<UserTopicTag>( data, list ).MoveUp();
                echoRedirect( "ok" );
            }
            else if (cmd == "down") {

                new SortUtil<UserTopicTag>( data, list ).MoveDown();
                echoRedirect( "ok" );
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }

        }

        public void Add() {
            target( Create );
        }




        [HttpPost]
        public void Create() {
            string name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name )) {
                errors.Add( "请填写名称" );
                run( Add );
                return;
            }

            UserTopicTag data = new UserTopicTag();
            data.User = ctx.owner.obj as User;
            data.Name = name;
            data.insert();

            echoToParentPart( lang( "opok" ) );
             
        }

        public void Edit( int id ) {
            target( Update, id );

            UserTopicTag data = tagService.GetTagById( id );
            set( "Name", data.Name );
        }

        [HttpPost]
        public void Update( int id ) {

            string name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name )) {
                errors.Add( "请填写名称" );
                run( Edit, id );
                return;
            }

            UserTopicTag data = tagService.GetTagById( id );
            Result result = tagService.UpdateTagName( id, name );
            if (result.HasErrors) {
                errors.Join( result );
                run( Edit, id );
                return;
            }

            echoToParentPart( lang( "opok" ) );
        }

        [HttpDelete]
        public void Delete( int id ) {

            UserTopicTag f = tagService.GetTagById( id );
            if (f != null) {
                tagService.DeleteTag( id );
                echoRedirect( lang( "opok" ) );
            }

        }

    }

}
