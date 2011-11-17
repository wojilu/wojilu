/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Web.GlobalApp {

    public class AppErrorJob {

        public static void Send ( String errorInfo ) {

            AppErrorItem item = new AppErrorItem();
            item.ErrorHtml = errorInfo.Replace( "\n", "<br/>" );
            item.Occured = DateTime.Now;
            item.Title = "site error (" + DateTime.Now + ")";

            item.insert();

        }


    }

}
