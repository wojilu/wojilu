using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.Microblogs.Interface;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;

namespace wojilu.Web.Controller.Admin {

    public class MicroblogController : ControllerBase {

        public IMicroblogService microblogService { get; set; }

        public MicroblogController() {
            microblogService = new MicroblogService();
        }

        public override void Layout() {

            set( "microblogLink", to( List ) );

            set( "microblogSettings", to( Settings ) );

        }

        public void Settings() {

            target( SaveSettings );

            set( "contentMax", config.Instance.Site.MicroblogContentMax );
            set( "pageSize", config.Instance.Site.MicroblogPageSize );
        }

        public void SaveSettings() {

            int contentMax = ctx.PostInt( "contentMax" );
            if (contentMax < 50) contentMax = 50;

            config.Instance.Site.MicroblogContentMax = contentMax;
            config.Instance.Site.Update( "MicroblogContentMax", contentMax );


            int pageSize = ctx.PostInt( "pageSize" );
            if (pageSize < 5) pageSize = 5;

            config.Instance.Site.MicroblogPageSize = pageSize;
            config.Instance.Site.Update( "MicroblogPageSize", pageSize );


            echoRedirect( lang( "opok" ) );

        }

        public void List() {

            set( "OperationUrl", to( Admin ) );

            DataPage<Microblog> list = microblogService.GetPageListAll( 30 );

            IBlock block = getBlock( "list" );
            foreach (Microblog blog in list.Results) {

                block.Set( "data.Id", blog.Id );
                block.Set( "data.Creator", blog.User.Name );

                block.Set( "data.CreatorLink", alink.ToUserMicroblog( blog.User ) );

                block.Set( "data.Content", blog.Content );
                block.Set( "data.Created", blog.Created );
                block.Set( "data.DeleteLink", to( Delete, blog.Id ) );
                block.Next();
            }

            set( "page", list.PageBar );
        }

        [HttpPost, DbTransaction]
        public void Admin() {

            String ids = ctx.PostIdList( "choice" );
            String cmd = ctx.Post( "action" );

            if ("delete".Equals( cmd )) {

                microblogService.DeleteBatch( ids );
                echoAjaxOk();

            }
            else {
                echoError( "errorCmd" );
            }

        }


        [HttpDelete, DbTransaction]
        public void Delete( int id ) {

            Microblog blog = microblogService.GetById( id );
            if (blog == null) {
                throw new NullReferenceException( lang( "exDataNotFound" ) );
            }

            microblogService.Delete( blog );


            echoAjaxOk();

        }

    }

}
