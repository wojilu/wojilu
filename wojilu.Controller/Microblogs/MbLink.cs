using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Interface;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Microblogs {

    public class MbLink {

        /// <summary>
        /// 是否微博独立部署？
        /// </summary>
        private static int isMicroblogOnly = 0;

        /// <summary>
        /// 根据系统配置，决定指向feed的详细页，还是独立微博的详细页
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string ToShow(IMember owner, long id) {

            if (isMicroblogOnly == 0) {
                return ToShowFeed( owner, id );
            }
            else {
                return ToShowMicroblog( owner, id );
            }
        }

        /// <summary>
        /// 指向feed的详细页
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string ToShowFeed(IMember owner, long id) {
            return Link.To( owner, new Users.HomeController().Info, id );
        }

        /// <summary>
        /// 指向独立微博的详细页
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string ToShowMicroblog(IMember owner, long id) {
            return Link.To( owner, new Microblogs.MicroblogController().Show, id );
        }

        /// <summary>
        /// 根据系统配置，决定指向feed的at页，还是独立微博的at页
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static String ToAt( IMember owner ) {

            if (isMicroblogOnly == 0) {
                return ToAtFeed( owner );
            }
            else {
                return ToAtMicroblog( owner );
            }
        }


        /// <summary>
        /// 指向feed内部的at页
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static String ToAtFeed( IMember owner ) {
            return Link.To( owner, new Users.Admin.HomeController().Atme );
        }

        /// <summary>
        /// 指向独立微博的at页
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static String ToAtMicroblog( IMember owner ) {
            return Link.To( owner, new Microblogs.My.MicroblogController().Atme );
        }


    }
}
