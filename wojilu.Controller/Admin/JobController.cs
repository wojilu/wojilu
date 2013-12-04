using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Jobs;
using wojilu.Web.Mvc.Attr;
using System.Web;

namespace wojilu.Web.Controller.Admin {

    public class JobController : ControllerBase {

        public JobController() {
            base.LayoutControllerType = typeof( SiteConfigController );
        }

        public virtual void List() {

            set( "listUrl", to( List ) );

            List<WebJob> list = cdb.findAll<WebJob>();

            IBlock block = getBlock( "list" );
            foreach (WebJob job in list) {

                block.Set( "job.Name", job.Name );

                String interval = job.Interval + lang( "ms" ) + "/";
                interval += job.Interval / 1000 + lang( "second" ) + "/";
                interval += job.Interval / 1000 / 60 + lang( "minute" );

                block.Set( "job.Interval", interval );


                block.Set( "job.Type", job.Type );
                block.Set( "job.LinkEdit", to( Edit, job.Id ) );

                String lnkStop = getLinkStop( job );
                block.Set( "job.LinkStop", lnkStop );

                block.Next();
            }
        }


        private string getLinkStop( WebJob job ) {

            if (job.IsRunning)
                return string.Format( "<span class=\"running\">{1} <span href=\"{0}\" class=\"stopCmd cmd\">{2}</span></span>", to( Stop, job.Id ), lang( "running" ), lang( "jobStop" ) );
            else
                return string.Format( "<span class=\"stopped\">{1} <span href=\"{0}\" class=\"startCmd cmd\">{2}</span></span>", to( Start, job.Id ), lang( "stopped" ), lang( "jobStart" ) );

        }

        [HttpPost]
        public virtual void Stop( long id ) {


            WebJob job = cdb.findById<WebJob>( id );
            if (job == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            job.IsRunning = false;
            job.update();

            HttpRuntime.UnloadAppDomain();
            echoAjaxOk();

        }

        [HttpPost]
        public virtual void Start( long id ) {

            WebJob job = cdb.findById<WebJob>( id );
            if (job == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            job.IsRunning = true;
            job.update();

            HttpRuntime.UnloadAppDomain();
            echoAjaxOk();
        }

        public virtual void Edit( long id ) {
            target( Update, id );

            WebJob job = cdb.findById<WebJob>( id );
            if (job == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            set( "job.Interval", job.Interval );
        }

        [HttpPost]
        public virtual void Update( long id ) {

            int Interval = ctx.PostInt( "Interval" );
            WebJob job = cdb.findById<WebJob>( id );

            if (job == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            job.Interval = Interval;
            job.update();

            HttpRuntime.UnloadAppDomain();

            echoToParentPart( lang( "opok" ), to( List ), 999 );

        }

    }

}
