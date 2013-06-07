/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Web.Context;
using wojilu.Members.Users.Domain;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.Drawing;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller {


    public class MicroblogInstaller : BaseInstaller {

        private static readonly ILog logger = LogManager.GetLogger( typeof( MicroblogInstaller ) );

        private MicroblogService microblogService = new MicroblogService();

        public void Init( MvcContext ctx, String appName, String fUrl ) {

            // 添加微博菜单，设置为default首页
            base.AddMenu( ctx, appName, alink.ToMicroblog(), fUrl );

            // 用当前帐号，试发10条图文微博
            addMicroblogs( ctx );
        }

        //----------------------------------------------------------------------------------------------

        private void addMicroblogs( MvcContext ctx ) {

            User creator = ctx.viewer.obj as User;

            List<String> list = getContentList();
            for (int i = 0; i < list.Count; i++) {
                Microblog blog = getBlog( creator, list[i], i );
                microblogService.Insert( blog );
            }
        }

        private Microblog getBlog( User creator, String content, int i ) {

            Microblog x = new Microblog();

            x.Creator = creator;
            x.Content = content;
            logger.Info( x.Content );

            if (i % 2 == 1 && i > 0) {
                // 发布一篇图片微博
                x.Pic = Img.CopyToUploadPath( "/__installer/pic/pic" + i + ".jpg" );
            }

            return x;
        }


        private List<String> getContentList() {
            List<String> list = new List<string>();
            list.Add( "乱云低薄暮，急雪舞回风" );
            list.Add( "百年歌自苦，未见有知音" );
            list.Add( "痛饮狂歌空度日，飞扬跋扈为谁雄" );
            list.Add( "谁怜一片影，相失万重云" );
            list.Add( "勋业频看镜，行藏独倚楼" );
            list.Add( "性豪业嗜酒，嫉恶怀刚肠" );
            list.Add( "暗水流花径，春星带草堂" );
            list.Add( "片云天共远，永夜月同孤" );
            list.Add( "文章千古事，得失寸心知" );
            list.Add( "四更山吐月，残夜水明楼" );
            list.Add( "丹青不知老将至，富贵于我如浮云" );
            list.Add( "自笑灯前舞，谁怜醉后歌" );
            list.Add( "饮酣视八极，俗物都茫茫" );

            return list;
        }


    }

}
