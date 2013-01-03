using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using Microsoft.Win32;

namespace wojilu.weibo.Core.QQWeibo
{
    public static class HttpUtil
    {
        //根据文件名获取文件类型
        public static string GetContentType(string fileName)
        {
            string contentType = "application/octetstream";
            string ext = Path.GetExtension(fileName).ToLower();
            RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(ext);

            if (registryKey != null && registryKey.GetValue("Content Type") != null)
            {
                contentType = registryKey.GetValue("Content Type").ToString();
            }

            return contentType;
        }

        //根据query String获取parameter数据
        public static List<Parameter> GetQueryParameters(string queryString)
        {
            if (queryString.StartsWith("?"))
            {
                queryString = queryString.Remove(0, 1);
            }

            List<Parameter> result = new List<Parameter>();

            if (!string.IsNullOrEmpty(queryString))
            {
                string[] p = queryString.Split('&');
                foreach (string s in p)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        if (s.IndexOf('=') > -1)
                        {
                            string[] temp = s.Split('=');
                            result.Add(new Parameter(temp[0], temp[1]));
                        }
                    }
                }
            }

            return result;
        }
    }
}
