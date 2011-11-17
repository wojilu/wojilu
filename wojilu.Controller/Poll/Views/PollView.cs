/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Apps.Poll.Views {


    public class PollView {

        public String Question { get; set; }
        public String PollMsg { get; set; }
        public String Submit { get; set; }

        public List<PollViewItem> Items { get; set; }

        public Boolean hasVote { get; set; }
        public int isVisible { get; set; }

        public String GetBody( Boolean showQuestionDesciption ) {

            StringBuilder builder = new StringBuilder();
            if (showQuestionDesciption) {
                builder.AppendFormat( "<div>{0}</div>", this.Question );
            }

            builder.Append( "<div id=\"pollContainer\">" );

            builder.Append( "<table class=\"pollOptions\" cellpadding=\"0\" cellspacing=\"0\">\n" );
            for (int i = 0; i < this.Items.Count; i++) {
                PollViewItem item = this.Items[i];
                builder.Append( "<tr>" );


                builder.Append( "<td class=\"optionItemString\">" );
                if (!this.hasVote) {
                    builder.AppendFormat( "{0}", item.CheckBox );
                }

                builder.AppendFormat( "<label for=\"pollOption{1}\">{0}</label></td>", item.OptionString, item.OptionValue );
                String cssClass = "onePollResultNotShow";
                if (this.hasVote || (this.isVisible == 0)) cssClass = "onePollResult";
                builder.AppendFormat( "<td id=\"onePollResult{0}\" class=\"{1}\">{2}</td>", i, cssClass, item.ImgAndCountInfo );


                //builder.AppendFormat( "<label for=\"pollOption{1}\">{0}</label>", item.OptionString, item.OptionValue );
                //String cssClass = "onePollResultNotShow";
                //if (this.hasVote || (this.isVisible == 0)) cssClass = "onePollResult";
                //builder.AppendFormat( "{0}", item.ImgAndCountInfo );
                //builder.Append( "</td>" );


                builder.Append( "</tr>" );
            }


            builder.Append( "</table>" );

            builder.AppendFormat( "<div class=\"note\">{0}</div>", this.PollMsg );
            builder.AppendFormat( "<div class=\"pollSubmit\">{0}</div>", this.Submit );

            builder.Append( "</div>" );

            return builder.ToString();
        }



    }
}

