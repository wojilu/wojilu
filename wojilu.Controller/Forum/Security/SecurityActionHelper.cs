/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace wojilu.Web.Controller.Forum.Utils {

    public class SecurityActionHelper {

        public static String getAdminActions() {

            Forum.Edits.TopicController ut = new Forum.Edits.TopicController();
            Forum.Edits.PostController up = new Forum.Edits.PostController();
            Forum.Edits.AttachmentController ac = new Forum.Edits.AttachmentController();
            Forum.Edits.TagController tc = new Forum.Edits.TagController();

            Forum.Moderators.TopicController t = new Forum.Moderators.TopicController();
            Forum.Moderators.TopicSaveController ts = new Forum.Moderators.TopicSaveController();
            Forum.Moderators.PostController p = new Forum.Moderators.PostController();
            Forum.Moderators.PostSaveController ps = new Forum.Moderators.PostSaveController();

            aActionWithId[] arrActions = new aActionWithId[] {


                // (admin) post
                p.AddCredit,
                ps.SaveCredit, ps.Ban, ps.UnBan, ps.Lock, ps.UnLock, ps.DeletePost, ps.DeleteTopic,

                // (edit) topic/post
                ut.Edit, ut.Update, up.Edit, up.Update,
                //up.SetReward, up.AddReward, up.SaveReward, 

                // (edit) attachment
                ac.Admin, ac.SetPermission, ac.SavePermission, ac.SaveSort, ac.Add, ac.SaveAdd, ac.Rename, ac.SaveRename, ac.Upload, ac.SaveUpload, ac.Delete,

                // (edit) tag
                tc.SaveTag
            };

            aAction[] otherActions = new aAction[] { 

                // (admin) topic 
                t.Sticky, t.Picked, t.Lock, t.Delete, t.Highlight, t.Category, t.SortSticky, t.GlobalSticky, t.Move,
                ts.Sticky, ts.StickyUndo, ts.Lock, ts.LockUndo, ts.GlobalSticky, ts.GlobalStickyUndo, ts.Pick, ts.PickedUndo, ts.Highlight, ts.HighlightUndo, ts.Delete,
                ts.Move, ts.SaveStickySort, ts.Category,
                t.GlobalSortSticky, ts.SaveGlobalStickySort
            };

            StringBuilder sb = new StringBuilder();
            addActions( sb, arrActions );
            addActions( sb, otherActions );

            return sb.ToString();
        }

        public static String getReply_Actions() {
            Forum.Users.PostController p = new Forum.Users.PostController();
            aActionWithId[] arrActions = new aActionWithId[] { p.ReplyPost, p.QuotePost, p.ReplyTopic, p.QuoteTopic };
            return getActionStr( arrActions );
        }

        public static String getTopicNew_Actions() {
            Forum.Users.TopicController t = new Forum.Users.TopicController();
            aAction[] arrActions = new aAction[] { t.NewTopic, t.Create };
            return getActionStr( arrActions );
        }

        public static String getQuestion_Actions() {
            Forum.Users.TopicController t = new Forum.Users.TopicController();
            aAction[] arrActions = new aAction[] { t.NewQ };
            return getActionStr( arrActions );
        }


        public static String getPoll_Actions() {
            Forum.Users.PollController t = new Forum.Users.PollController();
            aAction[] arrActions = new aAction[] { t.Add, t.Create };
            aActionWithId[] arrActions2 = new aActionWithId[] { t.Vote, t.Voter };
            StringBuilder sb = new StringBuilder();

            addActions( sb, arrActions );
            addActions( sb, arrActions2 );

            return sb.ToString();
        }

        //------------------------------------------------------------------------------

        private static String getActionStr( aActionWithId[] arrActions ) {
            StringBuilder sb = new StringBuilder();
            addActions( sb, arrActions );
            return sb.ToString();
        }

        private static String getActionStr( aAction[] arrActions ) {
            StringBuilder sb = new StringBuilder();
            addActions( sb, arrActions );
            return sb.ToString();
        }

        private static void addActions( StringBuilder sb, aActionWithId[] arrActions ) {
            foreach (aActionWithId a in arrActions) {
                sb.Append( getAction( a.Method ) );
            }
        }

        private static void addActions( StringBuilder sb, aAction[] arrActions ) {
            foreach (aAction a in arrActions) {
                sb.Append( getAction( a.Method ) );
            }
        }

        public static String getAction( aAction a ) {
            return getAction( a.Method );
        }
        public static String getAction( aActionWithId a ) {
            return getAction( a.Method );
        }

        public static String getAction( MethodInfo m ) {

            String adminType = m.ReflectedType.FullName;

            String path = strUtil.TrimStart( adminType, "wojilu.Web.Controller." ).Replace( ".", "/" );
            path = strUtil.TrimEnd( path, "Controller" );
            path = strUtil.Join( path, m.Name );

            return path + ";";

        }



    }

}
