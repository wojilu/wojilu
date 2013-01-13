/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.ORM;

namespace wojilu.Common.Feeds.Domain {

    [Serializable]
    public class ActionLink {
        public String Text { get; set; }
        public String Href { get; set; }
    }

    [Serializable]
    public class OneLineStoryTemplate {
        public String Title { get; set; }
    }

    [Serializable]
    public class ShortStoryTemplate {
        public String Title { get; set; }
        public String Body { get; set; }
    }


    [Serializable]
    public class TemplateBundle : ObjectBase<TemplateBundle> {

        public String OneLineStoryTemplatesStr { get; set; }

        [LongText]
        public String ShortStoryTemplatesStr { get; set; }

        [LongText]
        public String ActionLinksStr { get; set; }



        public  List<OneLineStoryTemplate> GetOneLineStoryTemplates(   ) {
            return Json.DeserializeList<OneLineStoryTemplate>( OneLineStoryTemplatesStr );
        }

        public  List<ShortStoryTemplate> GetShortStoryTemplates(  ) {
            return Json.DeserializeList<ShortStoryTemplate>( ShortStoryTemplatesStr );
        }

        public List<ActionLink> GetActionLinks(  ) {
            return Json.DeserializeList<ActionLink>( ActionLinksStr );
        }

        public static TemplateBundle GetBlogTemplateBundle() {
            return db.findById<TemplateBundle>( 1 );
        }

        //public static TemplateBundle GetBlogCommentTemplateBundle() {
        //    return db.findById<TemplateBundle>( 2 );
        //}

        public static TemplateBundle GetPhotoTemplateBundle() {
            return db.findById<TemplateBundle>( 3 );
        }

        //public static TemplateBundle GetPhotoCommentTemplateBundle() {
        //    return db.findById<TemplateBundle>( 4 );
        //}

        public static TemplateBundle GetForumTopicTemplateBundle() {
            return db.findById<TemplateBundle>( 5 );
        }

        public static TemplateBundle GetForumPostTemplateBundle() {
            return db.findById<TemplateBundle>( 6 );
        }

        public static TemplateBundle GetFriendsTemplateBundle() {
            return db.findById<TemplateBundle>( 9 );
        }

    }

}
