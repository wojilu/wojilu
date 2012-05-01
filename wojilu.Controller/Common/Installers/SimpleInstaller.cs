/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Common.AppBase.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Content.Section;
using wojilu.Members.Users.Domain;


namespace wojilu.Web.Controller.Common.Installers {

    // TODO 列表页加上时间
    // 标题栏背景
    // sidebar加上图标
    // 登录、站内搜索 // 工具栏、人物推荐、投票、友情链接
    // 右侧再加上图片
    public class SimpleInstaller : CmsInstallerBase, IAppInstaller {

        public SimpleInstaller() : base() {
        }

        protected override void createLayout() {

            String style = @"
#row1_column1 {width:26%;margin-right:8px;margin-left:0px; }
#row1_column2 {width:36%; }
#row1_column3 {width:36%;margin-left:8px;margin-right:0px; }

#row2_column1 {width:26%;margin-right:8px;margin-left:0px; }
#row2_column2 {width:36%; }
#row2_column3 {width:36%;margin-left:8px;margin-right:0px; }

#row3_column1 {width:26%;margin-right:8px;margin-left:0px; }
#row3_column2 {width:36%;}
#row3_column3 {width:36%;margin-left:8px;margin-right:0px; }

#row4_column1 {width:26%;margin-right:8px;margin-left:0px; }
#row4_column2 {width:36%;}
#row4_column3 {width:36%;margin-left:8px;margin-right:0px; }

#row5_column1 {width:26%;margin-right:8px;margin-left:0px; }
#row5_column2 {width:36%;}
#row5_column3 {width:36%;margin-left:8px;margin-right:0px; }

#row6_column1 {width:100%; }

";
            this.app.Style = style;
            this.app.Layout = "3/3/3/3/3/1";
            this.app.update();
        }




        protected override IMemberApp createPortalApp() {

            StringBuilder css = new StringBuilder();

            IMemberApp app = createApp();
            createLayout();


            ContentCustomTemplate ct = base.createTemplate( base.getListViewWithTime() );
            ContentCustomTemplate ctn = base.createTemplate( base.getNormalViewWithTime() );

            ContentSection s = null;


            // 第一行
            s = createSectionMashup( "用户登录", 19, 18, "11" );


            s = createSection( "最新动态", typeof( ListController ), "11" );
            createPost( s, "西安市一在建民房倒塌造成8人死亡" );
            createPost( s, "国税总局反驳中国税负痛苦指数世界第二传闻" );
            createPost( s, "香港货轮与日本渔船昨日在日本海域发生碰撞" );
            createPost( s, "菅直人继续宣称钓鱼岛是日本领土 中方回应" );
            createPost( s, "温家宝离京访问欧亚四国 中国掀秋季外交高潮 " );
            createPost( s, "2010年诺贝尔奖公布日期全部确定" );
            createPost( s, "上海眼科假药事件部分涉案人员被刑拘" );

            s = createSection( "焦点图片", typeof( SlideController ), "12" );
            createImgBig( s, "嫦娥二号整流罩坠落江西", "http://img2.cache.netease.com/cnews/2010/10/2/201010021526102b7fa.jpg" );
            createImgBig( s, "重庆特警开装甲车巡逻维稳", "http://img2.cache.netease.com/cnews/2010/10/2/201010020837190e40a.jpg" );
            createImgBig( s, "香港举行国庆烟花汇演", "http://img1.cache.netease.com/cnews/2010/10/2/20101002081623541f8.jpg" );
            css.AppendLine( "#section" + s.Id + " { border-width:0px; }" );
            css.AppendLine( "#sectionTitle" + s.Id + " { display:none; }" );
            css.AppendLine( "#sectionContent" + s.Id + " { padding-left:0px; padding-top:0px; padding-right:0px; padding-bottom:0px; }" );

            s = createSection( "新闻头条", typeof( TextController ), "13" );
            createText( s, @"<div style=""font-size:12px;""><h2 style=""font-size:22px;font-weight:bold;""><a href=""http://www.wojilu.com/space/author/Blog1/Post/22"">门户+论坛+SNS整合系统源码</a></h2><div><a href=""http://www.wojilu.com/Content396/Post/415"">闪电击中自由女神像瞬间</a><span>&nbsp;</span>|<span>&nbsp;</span><a href=""http://www.wojilu.com/Content396/Post/424"">2010年胡润百富榜发布</a><span>&nbsp;</span><br></div>"+
@"<h3 style=""margin-top:10px;font-size:16px;font-weight:bold;""><a href=""http://www.wojilu.com/Content396/Post/399"">MIT技术评论：2010十大新兴技术</a></h3><div><a href=""http://www.wojilu.com/Content396/Post/379"">SNS的多栖“钱”景</a><span>&nbsp;</span>|<span>&nbsp;</span><a href=""http://www.wojilu.com/Content396/Post/394"">创业板估值体系面临考验</a><span>&nbsp;</span><br></div>"+
@"<h3 style=""margin-top:10px;font-size:16px;font-weight:bold;""><a href=""http://www.wojilu.com/help"">我记录网站综合系统使用指南</a></h3><div><a href=""http://www.wojilu.com/Content396/Post/280"">Google的十大核心技术</a><span>&nbsp;</span>|<span>&nbsp;</span><a href=""http://www.wojilu.com/Content396/Post/273"">最令人讨厌11种应用程序</a><span>&nbsp;</span></div>"+
@"<h3 style=""margin-top:10px;font-size:16px;font-weight:bold;""><a href=""http://www.wojilu.com/Content396/Post/399"">MIT技术评论：2010十大新兴技术</a></h3><div><a href=""http://www.wojilu.com/Content396/Post/379"">SNS的多栖“钱”景</a><span>&nbsp;</span>|<span>&nbsp;</span><a href=""http://www.wojilu.com/Content396/Post/394"">创业板估值体系面临考验</a><span>&nbsp;</span><br></div>" +
"</div>" );


            // 第二行

            // TODO 图片列表 或 用户聚合
            s = createSection( "精彩图片", typeof( ImgController ), "21" );
            createImgS( s, "a1", "http://img5.cache.netease.com/cnews/2011/12/21/20111221103323f115d.jpg" );
            createImgS( s, "a2", "http://img6.cache.netease.com/cnews/2011/12/16/20111216092426e71f2.jpg" );
            createImgS( s, "a3", "http://img6.cache.netease.com/ent/2011/12/16/2011121600352318942.jpg" );
            createImgS( s, "a4", "http://img2.cache.netease.com/cnews/2011/12/22/20111222111147f85fe.jpg" );

            createImgS( s, "a5", "http://img6.cache.netease.com/cnews/2011/12/19/2011121917334512da7.jpg" );
            createImgS( s, "a6", "http://img1.cache.netease.com/cnews/2011/12/20/20111220193736c2d59.jpg" );
            createImgS( s, "a7", "http://img6.cache.netease.com/cnews/2011/12/16/20111216092426e71f2.jpg" );
            createImgS( s, "a8", "http://img3.cache.netease.com/cnews/2011/12/11/20111211093436a1c2b.jpg" );
            createImgS( s, "a9", "http://img3.cache.netease.com/auto/2011/12/22/20111222105517febf7.jpg" );


            s = createSection( "娱乐专题", typeof( NormalController ), "22", ctn );
            createImgS( s, "敢死队致敬老派动作重现热血场面", "一代报业宗师张季鸾的墓地近年失修荒废，曾占地40亩的陵园仅剩一亩见方的土坡。如今在废旧的墓陵旁边还建起了臭气熏天的养猪场。据悉，张季鸾病逝时，国共两党最高层都在第一时间发来唁电，蒋介石、周恩来更是亲自前往吊", "http://img4.cache.netease.com/ent/2010/8/19/2010081908251234713.jpg" );
            createPost( s, "张柏芝借博客与谢霆锋传情张柏芝借博" );
            createPost( s, "张家辉:拍激情戏不用向太太报批张柏芝借博" );
            createPost( s, "金鹰节孙红雷摘三奖成最大赢家张柏芝借博" );
            createPost( s, "钟楚红台湾出席剪彩活动张柏芝借博出席剪彩活" );
            createPost( s, "陈冠希被曝在香港秘密录唱张柏芝借博" );
            createPost( s, "浙版西游记白骨精成为爱自尽美女张柏芝借博" );

            s = createSection( "财经访谈", typeof( NormalController ), "23", ctn );
            createImgS( s, "吴敬琏：中国模式祸福未定", "行政干预没有解决权力监督问题，行政干预和某些问题上有所加剧，行政干预没有解决权力监督问题。", "http://img1.cache.netease.com/cnews/2010/3/9/2010030914340556b2e.jpg" );
            createPost( s, "王杰批前女友方文琳骗钱骗婚将就熊友方文猫死亡" );
            createPost( s, "陈楚生成都开唱众多粉丝集合将友方文就熊猫死" );
            createPost( s, "凤凰传奇玲花母女同台(图集)将就友方文熊猫死亡" );
            createPost( s, "杨千嬅狂减14磅称老公不介意友方文将就熊猫死" );
            createPost( s, "菲总统称下周在网上公开人质事件完整报告" );
            createPost( s, "美性病实验曾致700危地马拉人染病 奥巴马道歉" );


            // 第三行

            s = createSection( "投票", typeof( PollController ), "31" );
            List<String> polls = new List<String>();
            polls.Add( "天气很好" );
            polls.Add( "寒潮来临" );
            polls.Add( "雨季要到" );
            polls.Add( "七月流火" );
            createPoll( s, "你对将来天气的预测", polls );

            s = createSection( "国际资讯", typeof( ListController ), "32", ct );
            createPost( s, "日本神户将就熊猫死亡赔偿中方50万美元三度爆发" );
            createPost( s, "菲总统称下周在网上公开人质事件完整报告" );
            createPost( s, "美性病实验曾致700危地马拉人染病 奥巴马道歉" );
            createPost( s, "俄罗斯主张与北约签署军事限制协议网上公" );
            createPost( s, "巴西今日大选有望产生首位女总统村民身家" );
            createPost( s, "法国月内三度爆发反对退休改革全国性示威" );
            createPost( s, "东京电影节公布竞赛片 秦海璐角逐金麒麟" );

            s = createSection( "社会要闻", typeof( ListController ), "33", ct );
            createPost( s, "辽宁抚顺境内失事飞机或为朝鲜飞机村民身家" );
            createPost( s, "中国廉价药缺口超300种 医生吃回扣成主因" );
            createPost( s, "杭州64位市民联名申请罢免下城区人大代表" );
            createPost( s, "广州天河拆迁村民身家均将过百万村民身家" );
            createPost( s, "最后一批美军作战部队开始从伊拉克撤离" );
            createPost( s, "四川资阳原交通局长自曝被调查时遭刑讯逼供" );
            createPost( s, "美性病实验曾致700危地马拉人染病 奥巴马道歉" );



            // 第四行

            s = createSection( "专家访谈", typeof( TextController ), "41" );
            createText( s, @"<div style=""text-align:center;""><div><img src=""http://img6.cache.netease.com/lady/2011/12/27/201112271052516c656.jpg"" /></div><div style=""margin:5px;font-size:12px;""><a href=""""中国税改应伴随财产保护</a></div></div>" );

            s = createSection( "娱乐地带", typeof( ListController ), "42", ct );
            createPost( s, "齐秦老狼助阵世界城市音乐节辉:拍激情戏不用向太" );
            createPost( s, "梁咏琪亮相“国庆文艺晚会”志封面 称“狄仁杰”是" );
            createPost( s, "张柏芝借博客与谢霆锋传情结众女星 汤唯苍井" );
            createPost( s, "张家辉:拍激情戏不用向太太报批结众女星 汤唯苍井" );
            createPost( s, "金鹰节孙红雷摘三奖成最大赢家志封面 称“狄仁杰”是" );
            createPost( s, "钟楚红台湾出席剪彩活动辉:拍激情戏不用向太" );
            createPost( s, "陈冠希被曝在香港秘密录唱婚宴 刘嘉玲吴君如到" );


            s = createSection( "电影空间", typeof( ListController ), "43", ct );
            createPost( s, "“金狮奖”得主索菲亚·科波拉：好导演烂演员" );
            createPost( s, "彼得·杰克逊工作室被烧《霍比特人》拍摄受阻" );
            createPost( s, "刘德华登时尚杂志封面 称“狄仁杰”是人不是神" );
            createPost( s, "周杰伦见过蔡依林新男友 称应给他们空间恋爱" );
            createPost( s, "张柏芝接 楚成新片 与汤唯《赛车》迁村民身" );
            createPost( s, "釜山电影节集结众女星 汤唯苍井优等将齐聚" );
            createPost( s, "彼得·杰克逊工作室被烧《霍比特人》拍摄受阻" );


            // 第五行

            s = createSection( "友情链接", typeof( TextController ), "51" );
            createText( s, "<div style=\"margin:5px 10px;\"><div><a href=\"\">中国文化研究网</a></div>"+
                "<div><a href=\"\">电子商品网络交易</a></div>"+
                "<div><a href=\"\">二手图书</a></div>"+
                "<div><a href=\"\">极客视频基地</a></div>"+
                "<div><a href=\"\">新浪微博</a></div>"+
                "<div><a href=\"\">软件开发指南</a></div>"+
                "<div><a href=\"\">驴友攻略</a></div></div>"
                );


            s = createSection( "音乐世界", typeof( ListController ), "52", ct );
            createPost( s, "高明骏回应涉毒事件 向家人及公众致歉" );
            createPost( s, "利智被曝曾遭周润发羞辱 悄悄躲后台哭泣" );
            createPost( s, "巴黎看秀丢行李 林志玲买内衣应急(图)" );
            createPost( s, "姜昆:“海派清口”是好表演 但不适合上春晚" );
            createPost( s, "应采儿透露确有淫媒 称朋友曾被唆使陪客”" );
            createPost( s, "利智被曝曾遭周润发羞辱 悄悄躲后台哭泣" );
            createPost( s, "维多利亚上节目打瞌睡被逮 韩国网友盛赞超可爱" );

            s = createSection( "企业资讯", typeof( ListController ), "53", ct );
            createPost( s, "辽宁抚顺境内失事飞机或为朝鲜飞机东京电影节" );
            createPost( s, "中国廉价药缺口超300种 医生吃回扣成主因" );
            createPost( s, "杭州64位市民联名申请罢免下城区人大代表" );
            createPost( s, "广州天河拆迁村民身家均将过百万曝曾遭周润发" );
            createPost( s, "最后一批美军作战部队开始从伊拉克撤离" );
            createPost( s, "四川资阳原交通局长自曝被调查时遭刑讯逼供" );
            createPost( s, "张柏芝借博客与谢霆锋传情遭周润发羞辱 悄悄躲" );

            //---------------------------------------------------            


            // 第六行
            s = createSection( "网站链接", typeof( TextController ), "61" );
            createText( s, @"<div>友情链接：<a href="""">网站abc</a> <a href="""">网站abc</a> <a href="""">网站abc</a> <a href="""">网站abc</a>  <a href="""">网站abc</a> <a href="""">网站abc</a> <a href="""">网站abc</a> <a href="""">网站abc</a>  <a href="""">网站abc</a>  <a href="""">网站abc</a>  <a href="""">网站abc</a>  <a href="""">网站abc</a>  <a href="""">网站abc</a>  <a href="""">新闻kwg</a> <a href="""">新闻kwg</a></div>" +
                @"<div>网站导航：<a href="""">新闻kwg</a> <a href="""">新闻kwg</a> <a href="""">新闻kwg</a> <a href="""">新闻kwg</a> <a href="""">新闻kwg</a> <a href="""">新闻kwg</a> <a href="""">新闻kwg</a> <a href="""">新闻kwg</a> <a href="""">新闻kwg</a> <a href="""">新闻kwg</a>  <a href="""">网站abc</a>  <a href="""">网站abc</a>  <a href="""">网站abc</a> </div>"
                );

            css.AppendLine( "#section" + s.Id + " { border-width:0px; height:}" );
            css.AppendLine( "#sectionTitle" + s.Id + " { display:none; }" );
            css.AppendLine( "#sectionContent" + s.Id + " { padding-left:0px; padding-top:0px; padding-right:0px; padding-bottom:0px; }" );



            this.app.Style = this.app.Style + Environment.NewLine + css.ToString();

            css = new StringBuilder();

            css.Append( @"
#pageWrap {background:url('/static/img/patterns/pbg.jpg');}
#pageWrapInner {background:#fff;padding:0px 10px;}

#portalContainer  {color:#333333;}
#portalContainer a {color:}
#portalContainer .sectionPanel {border:1px #e6e6e6 solid;}
.sectionTitle {}
#portalContainer .sectionTitleText a {color:#839300;}
#portalContainer .sectionMore a {color:#666;}

.column1 .sectionTitle {background:url('/static/img/admin/menu-bg.gif');}
.column1 {background:#f8f8f8;height:215px;}

#row6_column1 {height:90px;}
#row1_column1 {height:305px;}
#row2_column1 {height:300px;}
" );

            this.app.SkinStyle = css.ToString();
            this.app.update(  );

            return app;
        }




    }

}
