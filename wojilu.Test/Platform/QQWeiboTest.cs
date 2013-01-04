using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.weibo.Core.QQWeibo;
using Newtonsoft.Json.Linq;

namespace wojilu.weibo.test
{
    [TestFixture]
   public class QQWeiboTest
    {
       //请在这里输入QQ微博
       string AppKey = "801129314";
       string AppSerect = "445218f1e79125f73cfb26cff3a06fba";
       string AccessKey = "dab81a0d792b47829ba2fd3304ca1f00";
       string AccessSerect = "52c13e30221b9d60f1856868722a4739";

       OauthKey key;
       user user;
       T t;
       public QQWeiboTest()
       {
           key = new OauthKey(AppKey, AppSerect, AccessKey, AccessSerect);
            user = new user(key, "json");
            t = new T(key, "json");
       }

       [Test]
       public void TestUserInfo()
       {
           JToken j = user.info();
           Assert.AreEqual(Convert.ToInt32(user.info()["ret"].ToString()),0);
       }

       [Test]
       public void TestUserUpdateInfo()
       {
         //这里会更改你的微博信息，请注意
           JToken j = user.info();
           var data = j["data"];
           var name = data["nick"].ToString();
           var response = user.update("金辉", 0, 0, 0, 0, 0, 0, 0, "");
           var responseJson = JToken.Parse(response);
           Assert.AreEqual(responseJson["ret"].ToString(), "0");

       }


       [Test]
       public void TestSendWeibo()
       {
           string response = t.add("13种药用胶囊涉铬超标 河北查处时工厂起火 " + DateTime.Now.Ticks +" http://v.youku.com/v_playlist/f17349757o1p0.html", "127.0.0.1", "", "");
           JToken j = JToken.Parse(response);
           Assert.AreEqual(j["ret"].ToString(), "0");
       }

       [Test]
       public void TestSendWeiboWithPic()
       {
           //这里需要替换成你电脑上的图片
           string pic = @"C:\Users\Administrator\Desktop\刘德华.jpg";
           string response = t.add_pic("13种药用胶囊涉铬超标 河北查处时工厂起火 " + DateTime.Now.Ticks + " http://v.youku.com/v_playlist/f17349757o1p0.html", "127.0.0.1", "", "",pic);
           JToken j = JToken.Parse(response);
           Assert.AreEqual(j["ret"].ToString(), "0");
       }
    }
}
