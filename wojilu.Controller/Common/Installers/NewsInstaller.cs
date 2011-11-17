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


namespace wojilu.Web.Controller.Common.Installers {

    public class NewsInstaller : CmsInstallerBase, IAppInstaller {

        public NewsInstaller () : base() {
        }

        protected override void createLayout() {

            String style = @"
#row1_column1 {width:31%;margin-right:8px;margin-left:0px; }
#row1_column2 {width:39%; }
#row1_column3 {width:28%;margin-left:8px;margin-right:0px; }

#row2_column1 {width:100%; }

#row3_column1 {width:31%;margin-right:8px;margin-left:0px; }
#row3_column2 {width:39%; }
#row3_column3 {width:28%;margin-left:8px;margin-right:0px; }

#row4_column1 {width:100%; }

#row5_column1 {width:31%;margin-right:8px;margin-left:0px; }
#row5_column2 {width:39%;}
#row5_column3 {width:28%;margin-left:8px;margin-right:0px; }
";
            this.app.Style = style;
            this.app.Layout = "3/1/3/1/3";
            this.app.update();
        }

        protected override IMemberApp createPortalApp() {

            StringBuilder css = new StringBuilder();

            IMemberApp app = createApp();
            createLayout();

            // 第一行
            ContentSection s = createSection( "▍视频", typeof( VideoShowController ), "11" );
            createVideo( s, "美国发现超级地球肯定有生物存在", "http://i3.ku6img.com/cms/news/201010/02/C211.jpg", "http://player.ku6.com/refer/ohj4iZjNDkjuwg8M/v.swf" );

            css.AppendLine( "#section" + s.Id + " { border-width:0px; }" );
            css.AppendLine( "#sectionTitle" + s.Id + " { display:none; }" );
            css.AppendLine( "#sectionContent" + s.Id + " { padding-left:0px; padding-top:0px; padding-right:0px; padding-bottom:0px; }" );


            s = createSection( "▍娱乐专题", typeof( NormalController ), "11" );
            createImgS( s, "敢死队致敬老派动作重现热血场面", "一代报业宗师张季鸾的墓地近年失修荒废，曾占地40亩的陵园仅剩一亩见方的土坡。如今在废旧的墓陵旁边还建起了臭气熏天的养猪场。据悉，张季鸾病逝时，国共两党最高层都在第一时间发来唁电，蒋介石、周恩来更是亲自前往吊", "http://img4.cache.netease.com/ent/2010/8/19/2010081908251234713.jpg" );
            createPost( s, "张柏芝借博客与谢霆锋传情" );
            createPost( s, "张家辉:拍激情戏不用向太太报批" );
            createPost( s, "金鹰节孙红雷摘三奖成最大赢家" );
            createPost( s, "钟楚红台湾出席剪彩活动" );
            createPost( s, "陈冠希被曝在香港秘密录唱" );
            createPost( s, "浙版西游记白骨精成为爱自尽美女" );

            //

            s = createSection( "〓 新闻头条", typeof( TextController ), "12" );
            createText( s, @"<div style=""font-size:12px;""><h2 style=""font-size:22px;font-weight:bold;""><a href=""http://www.wojilu.com/space/author/Blog1/Post/22"">门户+论坛+SNS整合系统源码</a></h2><div><a href=""http://www.wojilu.com/Content396/Post/415"">闪电击中自由女神像瞬间</a><span>&nbsp;</span>|<span>&nbsp;</span><a href=""http://www.wojilu.com/Content396/Post/424"">2010年胡润百富榜发布</a><span>&nbsp;</span><br></div><h3 style=""margin-top:10px;font-size:16px;font-weight:bold;""><a href=""http://www.wojilu.com/Content396/Post/399"">MIT技术评论：2010十大新兴技术</a></h3><div><a href=""http://www.wojilu.com/Content396/Post/379"">SNS的多栖“钱”景</a><span>&nbsp;</span>|<span>&nbsp;</span><a href=""http://www.wojilu.com/Content396/Post/394"">创业板估值体系面临考验</a><span>&nbsp;</span><br></div><h3 style=""margin-top:10px;font-size:16px;font-weight:bold;""><a href=""http://www.wojilu.com/help"">我记录网站综合系统使用指南</a></h3><div><a href=""http://www.wojilu.com/Content396/Post/280"">Google的十大核心技术</a><span>&nbsp;</span>|<span>&nbsp;</span><a href=""http://www.wojilu.com/Content396/Post/273"">最令人讨厌11种应用程序</a><span>&nbsp;</span></div></div>" );

            s = createSection( "〓 国内", typeof( ListController ), "12" );
            createPost( s, "西安市一在建民房倒塌造成8人死亡" );
            createPost( s, "国税总局反驳中国税负痛苦指数世界第二传闻" );
            createPost( s, "香港货轮与日本渔船昨日在日本海域发生碰撞" );
            createPost( s, "菅直人继续宣称钓鱼岛是日本领土 中方回应" );
            createPost( s, "温家宝离京访问欧亚四国 中国掀秋季外交高潮 " );
            createPost( s, "2010年诺贝尔奖公布日期全部确定" );
            createPost( s, "上海眼科假药事件部分涉案人员被刑拘" );

            s = createSection( "〓 国际", typeof( ListController ), "12" );
            createPost( s, "日本神户将就熊猫死亡赔偿中方50万美元" );
            createPost( s, "菲总统称下周在网上公开人质事件完整报告" );
            createPost( s, "美性病实验曾致700危地马拉人染病 奥巴马道歉" );
            createPost( s, "俄罗斯主张与北约签署军事限制协议" );
            createPost( s, "巴西今日大选有望产生首位女总统" );
            createPost( s, "法国月内三度爆发反对退休改革全国性示威" );
            createPost( s, "东京电影节公布竞赛片 秦海璐角逐金麒麟" );


            s = createSection( "▶ 焦点图片", typeof( SlideController ), "13" );
            createImgBig( s, "嫦娥二号整流罩坠落江西", "http://img2.cache.netease.com/cnews/2010/10/2/201010021526102b7fa.jpg" );
            createImgBig( s, "重庆特警开装甲车巡逻维稳", "http://img2.cache.netease.com/cnews/2010/10/2/201010020837190e40a.jpg" );
            createImgBig( s, "香港举行国庆烟花汇演", "http://img1.cache.netease.com/cnews/2010/10/2/20101002081623541f8.jpg" );
            css.AppendLine( "#section" + s.Id + " { border-width:0px; }" );
            css.AppendLine( "#sectionTitle" + s.Id + " { display:none; }" );
            css.AppendLine( "#sectionContent" + s.Id + " { padding-left:0px; padding-top:0px; padding-right:0px; padding-bottom:0px; }" );

            s = createSection( "▶ 近期要闻", typeof( ListController ), "13" );
            createPost( s, "辽宁抚顺境内失事飞机或为朝鲜飞机" );
            createPost( s, "中国廉价药缺口超300种 医生吃回扣成主因" );
            createPost( s, "杭州64位市民联名申请罢免下城区人大代表" );
            createPost( s, "广州天河拆迁村民身家均将过百万" );
            createPost( s, "最后一批美军作战部队开始从伊拉克撤离" );
            createPost( s, "四川资阳原交通局长自曝被调查时遭刑讯逼供" );

            createSection( "▶ 精彩图片", typeof( ImgController ), "13" );

            // 第二行
            s = createSection( "通栏广告", typeof( TextController ), "21" );
            createText( s, @"<img src=""http://img2.126.net/xoimages/game/20100901/tx/g/960x100.jpg""/>" );

            css.AppendLine( "#section" + s.Id + " { border-width:0px; }" );
            css.AppendLine( "#sectionTitle" + s.Id + " { display:none; }" );
            css.AppendLine( "#sectionContent" + s.Id + " { padding-left:0px; padding-top:0px; padding-right:0px; padding-bottom:0px; }" );


            s = createSection( "▍财经访谈", typeof( NormalController ), "31" );
            createImgS( s, "吴敬琏：中国模式祸福未定", "行政干预没有解决权力监督问题，行政干预和某些问题上有所加剧，行政干预没有解决权力监督问题。", "http://img1.cache.netease.com/cnews/2010/3/9/2010030914340556b2e.jpg" );
            createPost( s, "王杰批前女友方文琳骗钱骗婚" );
            createPost( s, "陈楚生成都开唱众多粉丝集合" );
            createPost( s, "凤凰传奇玲花母女同台(图集)" );
            createPost( s, "杨千嬅狂减14磅称老公不介意" );

            s = createSection( "▍娱乐", typeof( ListController ), "31" );
            createPost( s, "齐秦老狼助阵世界城市音乐节" );
            createPost( s, "梁咏琪亮相“国庆文艺晚会”" );
            createPost( s, "张柏芝借博客与谢霆锋传情" );
            createPost( s, "张家辉:拍激情戏不用向太太报批" );
            createPost( s, "金鹰节孙红雷摘三奖成最大赢家" );
            createPost( s, "钟楚红台湾出席剪彩活动" );
            createPost( s, "陈冠希被曝在香港秘密录唱" );
            createPost( s, "张柏芝借博客与谢霆锋传情" );


            s = createSection( "〓 电影", typeof( ListController ), "32" );
            createPost( s, "“金狮奖”得主索菲亚·科波拉：好导演烂演员" );
            createPost( s, "彼得·杰克逊工作室被烧《霍比特人》拍摄受阻" );
            createPost( s, "刘德华登时尚杂志封面 称“狄仁杰”是人不是神" );
            createPost( s, "周杰伦见过蔡依林新男友 称应给他们空间恋爱" );
            createPost( s, "张柏芝接 楚成新片 与汤唯《赛车》" );
            createPost( s, "釜山电影节集结众女星 汤唯苍井优等将齐聚" );
            createPost( s, "东京电影节公布竞赛片 秦海璐角逐金麒麟" );
            createPost( s, "彼得·杰克逊工作室被烧《霍比特人》拍摄受阻" );
            createPost( s, "利智被曝曾遭周润发羞辱 悄悄躲后台哭泣" );
            createPost( s, "布兰妮有望收回财政大权 被曝将投资某胸罩品牌" );

            s = createSection( "〓 音乐", typeof( ListController ), "32" );
            createPost( s, "高明骏回应涉毒事件 向家人及公众致歉" );
            createPost( s, "商天娥办婚宴 刘嘉玲吴君如到贺(组图)" );
            createPost( s, "利智被曝曾遭周润发羞辱 悄悄躲后台哭泣" );
            createPost( s, "巴黎看秀丢行李 林志玲买内衣应急(图)" );
            createPost( s, "姜昆:“海派清口”是好表演 但不适合上春晚" );
            createPost( s, "应采儿透露确有淫媒 称朋友曾被唆使陪客”" );
            createPost( s, "彼得·杰克逊工作室被烧《霍比特人》拍摄受阻" );
            createPost( s, "利智被曝曾遭周润发羞辱 悄悄躲后台哭泣" );
            createPost( s, "维多利亚上节目打瞌睡被逮 韩国网友盛赞超可爱" );

            s = createSection( "▶ 投票", typeof( PollController ), "33" );
            List<String> polls = new List<String>();
            polls.Add( "天气很好" );
            polls.Add( "寒潮来临" );
            polls.Add( "雨季要到" );
            polls.Add( "七月流火" );
            createPoll( s, "你对将来天气的预测", polls );

            s = createSection( "▶ 明星追踪", typeof( NormalController ), "33" );
            createImgS( s, "高明骏吸毒被抓满文军告密", "该片讲述了三个女人之间的情感故事，叶璇片中饰演一位寂寞的妻子", "http://img4.cache.netease.com/ent/2010/9/30/20100930191809ff7c0.png" );

            //createPost( s, "美国实验室创造4万亿摄氏度纪录" );
            createPost( s, "华航被曝无视油箱漏油冒险载客飞行" );
            createPost( s, "直击最疯狂的摩托车飙车秀无视" );
            createPost( s, "图说娱乐圈那些花瓶女星无视" );
            createPost( s, "大义灭亲可减刑是在亵渎法律无视" );
            createPost( s, "彭帅受困体能制约惨遭逆转无视" );

            //---------------------------------------------------            

            // 第四行
            s = createSection( "通栏广告", typeof( TextController ), "41" );
            createText( s, @"<img src=""http://img1.126.net/channel6/mail/0930/big960_100_1.gif""/>" );
            css.AppendLine( "#section" + s.Id + " { border-width:0px; }" );
            css.AppendLine( "#sectionTitle" + s.Id + " { display:none; }" );
            css.AppendLine( "#sectionContent" + s.Id + " { padding-left:0px; padding-top:0px; padding-right:0px; padding-bottom:0px; }" );

            // 第五行

            s = createSection( "▍财经专栏", typeof( NormalController ), "51" );
            createImgS( s, "房产税会不会成为开发商提价的借口", "浙江绍兴县家印染企业出资万余元，购买一条快艇送给镇政府监督辖区内企业的排污。这是为了动员全社会的力量来整治环境污染,府不会对捐赠企业手下留情", "http://img1.cache.netease.com/cnews/2010/10/1/20101001074938966c6.jpg" );
            createPost( s, "高明骏回应涉毒事件 向家人及公众致歉" );
            createPost( s, "商天娥办婚宴 刘嘉玲吴君如到贺(组图)" );
            createPost( s, "利智被曝曾遭周润发羞辱 悄悄躲后台哭泣" );
            createPost( s, "巴黎看秀丢行李 林志玲买内衣应急(图)" );
            createPost( s, "姜昆:“海派清口”是好表演 但不适合上春晚" );


            s = createSection( "▍企业", typeof( ListController ), "51" );
            createPost( s, "辽宁抚顺境内失事飞机或为朝鲜飞机" );
            createPost( s, "中国廉价药缺口超300种 医生吃回扣成主因" );
            createPost( s, "杭州64位市民联名申请罢免下城区人大代表" );
            createPost( s, "广州天河拆迁村民身家均将过百万" );
            createPost( s, "最后一批美军作战部队开始从伊拉克撤离" );
            createPost( s, "四川资阳原交通局长自曝被调查时遭刑讯逼供" );

            s = createSection( "〓 商业", typeof( ListController ), "52" );
            createPost( s, "应采儿透露确有淫媒 称朋友曾被唆使陪客”" );
            createPost( s, "北宋官员高薪历代罕有 为何仍未能养廉" );
            createPost( s, "唐代时尚女人交你围围巾（组图）" );
            createPost( s, "意籍传教士马国贤:康熙晚年偏爱江南女子" );
            createPost( s, "重庆谈判周恩来给毛泽东什么锦囊妙计" );
            createPost( s, "民国初八千麻袋大内档案流失经过" );
            createPost( s, "印度草拟铁矿石出口禁令引发内讧" );
            createPost( s, "北约在阿富汗连遭袭击致27人死" );
            createPost( s, "北京电影学首设相声班 俊男美女养眼各占一半" );

            s = createSection( "〓 环球资讯", typeof( ListController ), "52" );
            createPost( s, "美国拟修法确定互联网服务监管权最好" );
            createPost( s, "澳大利亚将对中医进行注册管理争议" );
            createPost( s, "传武汉加入富士康转移争夺战急招2.8万人" );
            createPost( s, "广东拟将调动干部纳入公租房保障遭代表质疑" );
            createPost( s, "李敖之子放弃台大读北大 称想读祖国的大学" );
            createPost( s, "镇江宋元粮仓遗址被毁 当事集团称并无过错" );
            createPost( s, "地方政府欲为731部队遗址申报文化遗产引" );
            createPost( s, "费玉清再踏红馆开个唱 徐小凤拉姑热捧偶像" );
            createPost( s, "沈傲君微博透露剖宫产顺利生下男婴(图)" );

            s = createSection( "▶ 上市公司", typeof( NormalController ), "53" );
            createImgS( s, "《时代》眼中的伊战", "该片讲述了三个女人之间的情感故事，叶璇片中饰演一位寂寞的妻子", "http://img2.cache.netease.com/cnews/2010/9/3/2010090308452932084.jpg" );
            createPost( s, "云南保山山体滑坡已致24人死亡24人失踪" );
            createPost( s, "广东政协委员调研称全省225万户仍住茅草房" );
            createPost( s, "徐州多家医院药品采购二次议价 不按中标价买" );
            createPost( s, "官员称供求关系变化系生姜价格上涨主因" );
            createPost( s, "舟曲上报重建规划 防洪标准升至50年一遇" );
            createPost( s, "发改委官员称国家将坚决抑制汽车产能过剩" );

            s = createSection( "▶ 教育前沿", typeof( ListController ), "53" );
            createPost( s, "农民陈凯旋给总理带路之后处理核废" );
            createPost( s, "未来3天长江淮河流域将迎强降雨" );
            createPost( s, "安徽千座水库超汛限水位处理核废" );
            createPost( s, "德国核废料处理或藏安全隐患近日" );
            createPost( s, "国际处理核废料的七大安全策略公布" );


            this.app.Style = this.app.Style + Environment.NewLine + css.ToString();

            css = new StringBuilder();
            css.AppendLine( "#portalContainer  {color:#333333;}" );
            css.AppendLine( "#portalContainer a {color:}" );
            css.AppendLine( "#portalContainer .sectionPanel {border:1px #e6e6e6 solid;}" );
            css.AppendLine( ".sectionTitle {background:url(/static/skin/apps/content/1/sectionTitleBg.jpg);}" );
            css.AppendLine( "#portalContainer .sectionTitleText a {color:#839300;}" );
            css.AppendLine( "#portalContainer .sectionMore a {color:#666;}" );

            this.app.SkinStyle = css.ToString();
            this.app.update(  );

            return app;
        }



    }

}
