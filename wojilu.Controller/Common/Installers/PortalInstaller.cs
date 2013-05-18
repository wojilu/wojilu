/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;

using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Web.Controller.Content.Section;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Enum;
using wojilu.Web.Mvc;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Common.Installers {

    public class PortalInstaller : CmsInstallerBase, IAppInstaller {

        private static readonly ILog logger = LogManager.GetLogger( typeof( PortalInstaller ) );

        public PortalInstaller() : base() { }

        protected override void createLayout() {
            this.app.Layout = "3/1/2/1/2/1/2";
            this.app.update();
        }

        protected override IMemberApp createPortalApp() {

            StringBuilder designCss = new StringBuilder();

            IMemberApp app = createApp();
            createLayout();

            ContentSection s = createSection( "焦点图片", typeof( SlideController ), "11" );
            createImgBig( s, "嫦娥二号整流罩坠落江西", "http://img2.cache.netease.com/cnews/2010/10/2/201010021526102b7fa.jpg" );
            createImgBig( s, "重庆特警开装甲车巡逻维稳", "http://img2.cache.netease.com/cnews/2010/10/2/201010020837190e40a.jpg" );
            createImgBig( s, "香港举行国庆烟花汇演", "http://img1.cache.netease.com/cnews/2010/10/2/20101002081623541f8.jpg" );
            designCss.AppendLine( "#portalContainer #section" + s.Id + " { border:0px; }" );
            designCss.AppendLine( "#sectionTitle" + s.Id + " { display:none; }" );
            designCss.AppendLine( "#sectionContent" + s.Id + " { padding-left:0px; padding-top:0px; padding-right:0px; padding-bottom:0px; }" );

            s = createSection( "娱乐专题", typeof( NormalController ), "11" );
            String desc = "<p>“我记录综合系统”是.net 领域领先的网站系统。集成了“SNS、门户、CMS、论坛、博客、相册、微博、群组、瀑布流、下载、WIKI”等APP，为您提供一站式解决方案。</p><p>功能强大、开放源代码、多语言支持，并带有丰富的二次开发教程和示例代码，扩展方便灵活。</p><p>我记录下载网址：<a href=\"http://www.wojilu.com\" title=\"我记录\">http://www.wojilu.com</a></p>";
            String summary = "“我记录综合系统”是.net 领域领先的网站系统。集成了“SNS、门户、CMS、论坛、博客、相册、微博、群组、瀑布流、下载、WIKI”等APP，为您提供一站式解决方案。";
            ContentPost post = createImgS( s, "我记录网站综合系统1.9发布", desc, "http://img3.cache.netease.com/ent/2010/10/18/2010101809310948fd9.png" );
            post.PickStatus = PickStatus.Focus;
            post.Summary = summary;
            post.update();
            designCss.AppendLine( "#sectionContent" + s.Id + " { font-size:12px; }" );

            createPost( s, "张柏芝借博客与谢霆锋传情" );
            createPost( s, "张家辉:拍激情戏不用向太太报批" );
            createPost( s, "金鹰节孙红雷摘三奖成最大赢家" );
            createPost( s, "钟楚红台湾出席剪彩活动" );
            createPost( s, "陈冠希被曝在香港秘密录唱" );
            createPost( s, "浙版西游记白骨精成为爱自尽美女" );

            s = createSectionMashup( "资讯头条", 38, 21, "12" );
            s.CssClass = "focusSection";
            s.ServiceParams = "param0=" + this.app.Id + ";param1=8";
            s.update();
            designCss.AppendLine( "#sectionContent" + s.Id + " { padding-left:0px; padding-top:0px; padding-right:0px; padding-bottom:0px; }" );

            s = createSection( "要闻列表", typeof( ListController ), "12" );
            s.ListCount = 9;
            s.update( "ListCount" );
            createPost( s, "西安市一在建民房倒塌造成8人死亡" );
            createPost( s, "国税总局反驳中国税负痛苦指数世界第二传闻" );
            createPost( s, "香港货轮与日本渔船昨日在日本海域发生碰撞" );
            createPost( s, "菅直人继续宣称钓鱼岛是日本领土 中方回应" );
            createPost( s, "温家宝离京访问欧亚四国 中国掀秋季外交高潮 " );
            createPost( s, "2010年诺贝尔奖公布日期全部确定" );
            createPost( s, "上海眼科假药事件部分涉案人员被刑拘" );
            createPost( s, "日本神户将就熊猫死亡赔偿中方50万美元" );
            createPost( s, "菲总统称下周在网上公开人质事件完整报告" );
            createPost( s, "美性病实验曾致700危地马拉人染病 奥巴马道歉" );

            // 创建自定义模板
            ContentCustomTemplate cs = new ContentCustomTemplate();
            cs.Content = @"<ul class=""dot2 ulLR unstyled"">
<!-- BEGIN list -->
<li style=""height:20px;""><div class=""left-title"" style=""width:310px;font-size:14px;"">
<a href=""#{post.Url}"" style=""#{post.TitleCss}"" title=""#{post.TitleFull}"" style=""#{post.TitleCss}"">#{post.Title}</a></div>
<div class=""right-info"">#{post.CreatedDay}</div>
<div class=""clear""></div>
</li>
<!-- END list -->
</ul>";
            cs.OwnerId = this.app.OwnerId;
            cs.OwnerType = this.app.OwnerType;
            cs.OwnerUrl = this.app.OwnerUrl;
            cs.Name = "" + DateTime.Now.ToShortDateString();
            cs.Creator = ctx.viewer.obj as User;
            tplService.Insert( cs );

            s.CustomTemplateId = cs.Id;
            s.update( "CustomTemplateId" );


            s = createSection( "信息发布", typeof( TextController ), "13" );
            createText( s, "<div style=\"margin:10px 0px 10px 10px;\">" +
                "<div><a href=\"" + Link.To( Site.Instance, new wojilu.Web.Controller.Content.Submit.PostController().Index, base.app.Id ) + "\" class=\"btn btn-danger right10\"><i class=\"icon-plus icon-white\"></i> 投递资讯</a>" +
                "<a href=\"" + Link.To( new LinkController().MyProfile ) + "\" class=\"btn btn-primary\"><i class=\"icon-plus icon-white\"></i> 我的资料</a></div>" +
                "<div><a href=\"#\" class=\"btn btn-info top10 right10\"><i class=\"icon-plus icon-white\"></i> 线下活动</a>" +
                "<a href=\"#\" class=\"btn btn-success top10\"><i class=\"icon-plus icon-white\"></i> 谈天说地</a></div>" +
                "</div><div class=\"clear\"></div>" );
            designCss.AppendLine( "#sectionContentText" + s.Id + " a{ color:#fff;}" );

            s = createSection( "公告", typeof( TextController ), "13" );
            createText( s, "<div style=\"font-size:12px;\"><div><a href=\"http://www.wojilu.com\">我记录网站综合系统1.9正式发布</a></div><div><a href=\"http://news.163.com/10/1018/15/6J9NSJI100014JB6.html\">时速1600公里超音速汽车英国问世</a></div><div><a href=\"http://news.sina.com.cn/c/2010-10-18/205321300806.shtml\">影著协回应网吧等放电影缴版权费质疑</a></div></div>" );

            s = createSection( "财经专栏", typeof( NormalController ), "13" );
            createImgS( s, "房产税成为开发商提价的借口", "浙江绍兴县家印染企业出资万余元，购买一条快艇送给镇政府监督辖区内企业的排污。这是为了动员", "http://img1.cache.netease.com/cnews/2010/10/1/20101001074938966c6.jpg" );
            createPost( s, "高明骏回应涉毒事件 向家人及公众致歉" );
            createPost( s, "商天娥办婚宴 刘嘉玲吴君如到贺(组图)" );
            createPost( s, "利智被曝曾遭周润发羞辱 悄悄躲后台哭泣" );
            createPost( s, "巴黎看秀丢行李 林志玲买内衣应急(图)" );
            createPost( s, "姜昆:“海派清口”是好表演 但不适合上春晚" );

            // 第二行
            s = createSection( "通栏广告", typeof( TextController ), "21" );
            createText( s, @"<img src=""/static/img/big/ad1.gif""/>" );

            designCss.AppendLine( "#portalContainer #section" + s.Id + " { border:0px; }" );
            designCss.AppendLine( "#sectionTitle" + s.Id + " { display:none; }" );
            designCss.AppendLine( "#sectionContent" + s.Id + " { padding-left:0px; padding-top:0px; padding-right:0px; padding-bottom:0px; margin-right:10px;}" );

            // 第三行
            s = createSection( "综合图文", typeof( PostImgController ), "31" );


            StringBuilder customCss = new StringBuilder();
            customCss.AppendLine( "#portalContainer #section" + s.Id + " {border:0px;}" );
            customCss.AppendLine( "#sectionTitle" + s.Id + " {background:url(/static/img/big/rowBg1.jpg) no-repeat;height:31px; border-bottom:0px;}" );
            customCss.AppendLine( "#sectionTitle" + s.Id + " .sectionTitleText a{color:#fff;}" );
            customCss.AppendLine( "#sectionTitle" + s.Id + " .sectionMore {margin-right:15px;}" );

            createImgPost( s, "丛林里的特雷莎修女", "谢欣燕填写问卷时，发现不对，计算机屏幕居然出现她和男友庄胜杰出游以及“嫁给我吧”的画面，被蒙在鼓里的谢女，一脸狐疑，此时，庄胜杰和几名友人，拿着“宝贝嫁给我吧”的海报，突然出现在她的眼前，让她又惊又喜。庄胜杰以颤抖音调说，“我们南北奔波那么久，可以不要再那么累，欣燕你可以嫁给我吗？”，说完送上捧花及定情戒指，旁边的友人起哄“嫁给他！嫁给他！”，谢女事先完全不知道有此场面，内心好感动，终于说出“我愿意”，立即一阵欢呼。", "http://img1.cache.netease.com/cnews/2010/10/11/201010110432204a6d3.jpg" );
            createPostC( s, "印度首富入住27层新居 造价66.5亿创全球最贵", "环球资讯" );
            createPostC( s, "美国汇率报告或推迟到11月G20峰会后发布", "股市金融" );
            createPostC( s, "北京周边县市出台政策限制外地人购房", "法律政策" );
            createPostC( s, "中国新车年销量将达1700万 逼近美国最高水平", "汽车销售" );
            createPostC( s, "河南教育厅发文组织数万学生到富士康实习", "教育政策" );
            createPostC( s, "印度首富入住27层新居 造价66.5亿创全球最贵", "环球资讯" );

            createImgHome( s, "湖南女特级教师驾车撞六学生", "女特级教师", 110, 80, "http://img4.cache.netease.com/club/2010/10/11/20101011082942d4f3e.jpg" );
            createImgHome( s, "明星与新欢旧爱间的情事纠葛", "明星与新欢", 110, 80, "http://img3.cache.netease.com/club/2010/10/11/20101011083326c2df5.jpg" );
            createImgHome( s, "陶寺遗址证 尧舜非传说", "陶寺遗址", 110, 80, "http://img1.cache.netease.com/cnews/2010/10/8/2010100812172846800.jpg" );
            createImgHome( s, "恋人搭高铁约会3年花36万", "恋人搭高铁", 110, 80, "http://img1.cache.netease.com/cnews/2010/10/3/20101003101238aa436.jpg" );

            s = createSection( "明星追踪", typeof( NormalController ), "32" );
            createImgS( s, "高明骏吸毒被抓满文军告密", "该片讲述了三个女人之间的情感故事，叶璇片中饰演一位寂寞的妻子", "http://img4.cache.netease.com/ent/2010/9/30/20100930191809ff7c0.png" );

            //createPost( s, "美国实验室创造4万亿摄氏度纪录" );
            createPost( s, "华航被曝无视油箱漏油冒险载客飞行" );
            createPost( s, "直击最疯狂的摩托车飙车秀无视" );
            createPost( s, "图说娱乐圈那些花瓶女星无视" );
            createPost( s, "大义灭亲可减刑是在亵渎法律无视" );
            createPost( s, "彭帅受困体能制约惨遭逆转无视" );

            // 第四行
            s = createSection( "通栏广告", typeof( TextController ), "41" );
            createText( s, @"<img src=""/static/img/big/ad2.gif""/>" );
            designCss.AppendLine( "#portalContainer #section" + s.Id + " { border:0px; }" );
            designCss.AppendLine( "#sectionTitle" + s.Id + " { display:none; }" );
            designCss.AppendLine( "#sectionContent" + s.Id + " { padding-left:0px; padding-top:0px; padding-right:0px; padding-bottom:0px; margin-right:10px;}" );

            // 第五行
            s = createSection( "娱乐资讯", typeof( PostImgController ), "51" );
            customCss.AppendLine( "#portalContainer #section" + s.Id + " {border:0px;}" );
            customCss.AppendLine( "#sectionTitle" + s.Id + " {background:url(/static/img/big/rowBg2.jpg) no-repeat;height:31px;border-bottom:0px;}" );
            customCss.AppendLine( "#sectionTitle" + s.Id + " .sectionTitleText a{color:#fff;}" );
            customCss.AppendLine( "#sectionTitle" + s.Id + " .sectionMore {margin-right:15px;}" );

            createImgPost( s, "林志颖生日曝儿子照片", "今天是林志颖36岁的生日，身在珠海赛车场拍戏的他发布微博曝光了小小志的照片，称儿子是今生最美好的事，还表示看著小小志用弯弯的眼睛对他笑感觉天使大概就是这样子，儿子是他的骄傲，儿子天使的笑容是最好的礼物", "http://img4.cache.netease.com/ent/2010/10/15/20101015132315ec733.png" );
            createPostC( s, "法国支持建立欧洲反导系统并称将保持核威慑", "环球军事" );
            createPostC( s, "深圳野生动物园承认失误 按工伤赔被虎咬死者", "社会新闻" );
            createPostC( s, "国家公务员考试昨日开始报名 网络一度堵塞", "天方夜谭" );
            createPostC( s, "住建部:违反限购房规定买卖住房有重大风险", "法律政策" );
            createPostC( s, "美国2010财年联邦财政赤字近１.3万亿美元", "财政要闻" );
            createPostC( s, "法国支持建立欧洲反导系统并称将保持核威慑", "环球军事" );

            createImgHome( s, "学历丢人成绩骄人的娱乐明星", "学历丢人明星", 110, 80, "http://img4.cache.netease.com/ent/2010/10/15/201010151038520fc9e.png?froment" );
            createImgHome( s, "摩登天空音乐节 世界城市音乐节", "摩登天空", 110, 80, "http://img3.cache.netease.com/ent/2010/9/13/201009131616576ab3c.jpg" );
            createImgHome( s, "第15届釜山电影节闭幕 中国两位新人导演获奖", "釜山电影节", 110, 80, "http://img4.cache.netease.com/ent/2010/10/11/201010111457590abb1.jpg" );
            createImgHome( s, "“东方风云榜”季选音乐营即将席卷热浪", "东方风云榜", 110, 80, "http://img4.cache.netease.com/ent/2010/10/11/201010111843492d214.jpg" );

            s = createSection( "上市公司", typeof( NormalController ), "52" );
            createImgS( s, "《时代》眼中的伊战", "该片讲述了三个女人之间的情感故事，叶璇片中饰演一位寂寞的妻子", "http://img2.cache.netease.com/cnews/2010/9/3/2010090308452932084.jpg" );
            createPost( s, "云南保山山体滑坡已致24人死亡24人失踪" );
            createPost( s, "广东政协委员调研称全省225万户仍住茅草房" );
            createPost( s, "徐州多家医院药品采购二次议价 不按中标价买" );
            createPost( s, "官员称供求关系变化系生姜价格上涨主因" );
            createPost( s, "舟曲上报重建规划 防洪标准升至50年一遇" );
            createPost( s, "发改委官员称国家将坚决抑制汽车产能过剩" );


            // 第六行
            s = createSection( "通栏广告", typeof( TextController ), "61" );
            createText( s, @"<img src=""/static/img/big/ad3.gif""/>" );
            designCss.AppendLine( "#portalContainer #section" + s.Id + " { border:0px; }" );
            designCss.AppendLine( "#sectionTitle" + s.Id + " { display:none; }" );
            designCss.AppendLine( "#sectionContent" + s.Id + " { padding-left:0px; padding-top:0px; padding-right:0px; padding-bottom:0px; margin-right:10px;}" );

            s = createSection( "综合图文", typeof( PostImgController ), "71" );
            customCss.AppendLine( "#portalContainer #section" + s.Id + "{border:0px;}" );
            customCss.AppendLine( "#sectionTitle" + s.Id + " {background:url(/static/img/big/rowBg3.jpg) no-repeat;height:31px;border-bottom:0px;}" );
            customCss.AppendLine( "#sectionTitle" + s.Id + " .sectionTitleText a{color:#fff;}" );
            customCss.AppendLine( "#sectionTitle" + s.Id + " .sectionMore {margin-right:15px;}" );

            createImgPost( s, "丛林里的特雷莎修女", "谢欣燕填写问卷时，发现不对，计算机屏幕居然出现她和男友庄胜杰出游以及“嫁给我吧”的画面，被蒙在鼓里的谢女，一脸狐疑，此时，庄胜杰和几名友人，拿着“宝贝嫁给我吧”的海报，突然出现在她的眼前，让她又惊又喜。庄胜杰以颤抖音调说，“我们南北奔波那么久，可以不要再那么累，欣燕你可以嫁给我吗？”，说完送上捧花及定情戒指，旁边的友人起哄“嫁给他！嫁给他！”，谢女事先完全不知道有此场面，内心好感动，终于说出“我愿意”，立即一阵欢呼。", "http://img1.cache.netease.com/cnews/2010/10/11/201010110432204a6d3.jpg" );
            createPostC( s, "印度首富入住27层新居 造价66.5亿创全球最贵", "环球资讯" );
            createPostC( s, "美国汇率报告或推迟到11月G20峰会后发布", "股市金融" );
            createPostC( s, "北京周边县市出台政策限制外地人购房", "法律政策" );
            createPostC( s, "中国新车年销量将达1700万 逼近美国最高水平", "汽车销售" );
            createPostC( s, "河南教育厅发文组织数万学生到富士康实习", "教育政策" );
            createPostC( s, "印度首富入住27层新居 造价66.5亿创全球最贵", "环球资讯" );

            createImgHome( s, "湖南女特级教师驾车撞六学生", "女特级教师", 110, 80, "http://img4.cache.netease.com/club/2010/10/11/20101011082942d4f3e.jpg" );
            createImgHome( s, "明星与新欢旧爱间的情事纠葛", "明星与新欢", 110, 80, "http://img3.cache.netease.com/club/2010/10/11/20101011083326c2df5.jpg" );
            createImgHome( s, "陶寺遗址证 尧舜非传说国内互联网业限时特卖", "陶寺遗址", 110, 80, "http://img1.cache.netease.com/cnews/2010/10/8/2010100812172846800.jpg" );
            createImgHome( s, "超强视觉冲击力恋人搭高铁约会3年花36万", "恋人搭高铁", 110, 80, "http://img1.cache.netease.com/cnews/2010/10/3/20101003101238aa436.jpg" );

            s = createSection( "明星追踪", typeof( NormalController ), "72" );
            createImgS( s, "任正非退休前打破神秘：为华为站出来中移动", "该片讲述了三个女人之间的情感故事，叶璇片中饰演一位寂寞的妻子", "http://img4.cache.netease.com/ent/2010/9/30/20100930191809ff7c0.png" );


            createPost( s, "91家网站华航被曝无视油箱漏油冒险载客飞行" );
            createPost( s, "微软回应直击最疯狂的摩托车飙车秀无视" );
            createPost( s, "图说娱乐圈那些花瓶女星无视图说娱乐圈" );
            createPost( s, "周末要闻回顾：苹果售后政策今秋起调整" );
            createPost( s, "美国政府叫停3D打印枪设计图下载美国政府" );

            setCss( designCss, customCss );

            return app;
        }

        private void setCss( StringBuilder designCss, StringBuilder customCss ) {
            this.app.Style = this.getThisCss() + Environment.NewLine
                + "/*****design css*****/" + Environment.NewLine + designCss.ToString();
            this.app.update();

            StringBuilder xCss = new StringBuilder();
            xCss.AppendLine( "#portalContainer  {color:#333333; margin-left:8px;}" );
            xCss.AppendLine( "#portalContainer a {color:}" );
            xCss.AppendLine( "#portalContainer .sectionPanel {border:1px #ddd solid;}" );
            xCss.AppendLine( "#portalContainer .sectionTitleText a {color:#c00;}" );
            xCss.AppendLine( "#portalContainer .sectionMore a {color:#666;}" );

            xCss.AppendLine( "#row1_column3 li{font-size:12px;}" );
            xCss.AppendLine( "#row3_column2 li{font-size:12px;}" );
            xCss.AppendLine( "#row5_column2 li{font-size:12px;}" );
            xCss.AppendLine( "#row7_column2 li{font-size:12px;}" );

            this.app.SkinStyle = xCss.ToString() + customCss.ToString();
            this.app.update();
        }

        private String getThisCss() {
            String style = @"
#row1_column1 {width:29%;margin-right:8px;margin-left:0px; }
#row1_column2 {width:43%; }
#row1_column3 {width:25%;margin-left:8px;margin-right:0px; }

#row2_column1 {width:100%; }
#row3_column1 {width:73%;margin-right:8px;margin-left:0px; }
#row3_column2 {width:25%; }

#row4_column1 {width:100%; }
#row5_column1 {width:73%;margin-right:8px;margin-left:0px; }
#row5_column2 {width:25%;}

#row6_column1 {width:100%; }
#row7_column1 {width:73%;margin-right:8px;margin-left:0px; }
#row7_column2 {width:25%; }
";
            return style;
        }
    }

}
