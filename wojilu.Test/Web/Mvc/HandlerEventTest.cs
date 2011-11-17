using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace wojilu.Test.Web.Mvc {

    [TestFixture]
    public class HandlerEventTest {


        [Test]
        public void test() {

            Publisher pub = new Publisher();
            Subscriber sub1 = new Subscriber( "sub1", pub );
            Subscriber sub2 = new Subscriber( "sub2", pub );

            pub.DoSomething();



        }

    }


    public class woijiluEventArgs : EventArgs {
        public woijiluEventArgs( string s ) {
            msg = s;
        }
        private string msg;
        public string Message {
            get { return msg; }
            set { msg = value; }
        }
    }



    public class Publisher {

        public event EventHandler<woijiluEventArgs> RaiseCustomEvent;


        public void DoSomething() {
            OnRaiseCustomEvent( new woijiluEventArgs( "Did something" ) );
        }

        protected virtual void OnRaiseCustomEvent( woijiluEventArgs e ) {
            EventHandler<woijiluEventArgs> handler = RaiseCustomEvent;

            if (handler != null) {
                e.Message += String.Format( " at {0}", DateTime.Now.ToString() );
                handler( this, e );
            }
        }
    }

    class Subscriber {
        private string id;
        public Subscriber( string ID, Publisher pub ) {
            id = ID;
            pub.RaiseCustomEvent += HandleCustomEvent;
        }

        void HandleCustomEvent( object sender, woijiluEventArgs e ) {
            Console.WriteLine( id + " received this message: {0}", e.Message );
        }

    }



}
