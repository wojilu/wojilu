using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace wojilu.weibo.Core.QQWeibo
{
    /// <summary>用户相关
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class user : QWeiboApiBase
    {
        /// <summary>
        /// 构造函数 <see cref="user"/> class.
        /// </summary>
        /// <param name="okey">The okey.</param>
        /// <param name="format">The format.</param>
        /// <remarks></remarks>
        public user(OauthKey okey, string format) : base(okey, format) { }

        /// <summary>
        /// 获取自己的详细资料
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public JToken info()
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format", format));

            return JToken.Parse(base.SyncRequest(TypeOption.TXWB_USER_INFO, paras, null));
        }


        /// <summary>更新用户信息
        /// 
        /// </summary>
        /// <param name="nick">The nick.</param>
        /// <param name="sex">The sex.</param>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="countrycode">The countrycode.</param>
        /// <param name="provincecode">The provincecode.</param>
        /// <param name="citycode">The citycode.</param>
        /// <param name="introduction">The introduction.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string update(string nick, int sex, int year, int month,
                    int day, int countrycode, int provincecode, int citycode,
                    string introduction)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format", format));
            if (!string.IsNullOrEmpty(nick))
                paras.Add(new Parameter("nick", nick));
            if (sex > 0)
                paras.Add(new Parameter("sex", sex.ToString()));
            if (year > 0)
                paras.Add(new Parameter("year", year.ToString()));
            if (month > 0)
                paras.Add(new Parameter("month", month.ToString()));
            if (day > 0)
                paras.Add(new Parameter("day", day.ToString()));
            if (countrycode > 0)
                paras.Add(new Parameter("countrycode", countrycode.ToString()));
            if (provincecode > 0)
                paras.Add(new Parameter("provincecode", provincecode.ToString()));
            if (citycode > 0)
                paras.Add(new Parameter("citycode", citycode.ToString()));
            if (!string.IsNullOrEmpty(introduction))
                paras.Add(new Parameter("introduction", introduction));

            return base.SyncRequest(TypeOption.TXWB_USER_UPDATE, paras, null);
        }

        /// <summary>更新用户头像信息
        /// 
        /// </summary>
        /// <param name="pic">The pic.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string update_head(string pic)
        {
            List<Parameter> paras = new List<Parameter>();
            List<Parameter> files = new List<Parameter>();

            paras.Add(new Parameter("format", format));
            files.Add(new Parameter("pic", pic));

            return base.SyncRequest(TypeOption.TXWB_USER_UPDATE_HEAD, paras, files);
        }

        /// <summary>更新用户教育信息
        /// 
        /// </summary>
        /// <param name="feildid">The feildid.</param>
        /// <param name="year">The year.</param>
        /// <param name="schoolid">The schoolid.</param>
        /// <param name="departmentid">The departmentid.</param>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string update_edu(int feildid, int year, int schoolid,
                    int departmentid, string level)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format", format));
            paras.Add(new Parameter("feildid", feildid.ToString()));
            paras.Add(new Parameter("year", year.ToString()));
            paras.Add(new Parameter("schoolid", schoolid.ToString()));
            paras.Add(new Parameter("departmentid", departmentid.ToString()));
            paras.Add(new Parameter("level", level));

            return base.SyncRequest(TypeOption.TXWB_USER_UPDATE_EDU, paras, null);
        }

        /// <summary>获取其他人资料
        /// 
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string other_info(string name)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format", format));
            paras.Add(new Parameter("name", name));

            return base.SyncRequest(TypeOption.TXWB_USER_OTHER_INFO, paras, null);
        }

        /// <summary>获取一批人的简单资料
        /// 
        /// </summary>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string infos(string names)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format", format));
            paras.Add(new Parameter("names", names));

            return base.SyncRequest(TypeOption.TXWB_USER_INFOS, paras, null);
        }
    }
}
