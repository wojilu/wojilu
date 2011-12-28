using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Security;
using System.Web.Profile;

namespace wojilu.AuthenticationModule
{
    //    <!--UserName  LoginUserName  FullName  UserGroup  CurJi  CurBh  CurBnbh -->
    //<add name="UserName" defaultValue="" allowAnonymous="false"/>
    //<add name="LoginUserName" defaultValue="" allowAnonymous="false"/>
    //<add name="FullName" defaultValue="" allowAnonymous="false"/>
    //<add name="Sex" defaultValue="" allowAnonymous="false"/>
    //<add name="UserGroup" defaultValue="" allowAnonymous="false"/>
    //<add name="CurJi" defaultValue="" allowAnonymous="false"/>
    //<add name="CurBh" defaultValue="" allowAnonymous="false"/>
    //<add name="CurBnbh" defaultValue="" allowAnonymous="false"/>

    public class UserProfile : ProfileBase
    {
        [System.Web.Profile.SettingsAllowAnonymous(false)]
        [System.Web.Profile.ProfileProvider("SqlProfileProvider")]
        [System.Configuration.SettingsSerializeAs(System.Configuration.SettingsSerializeAs.String)]  //使用xml序列化，可选。
        public string CurrentUserName
        {
            get { return base["CurrentUserName"].ToString(); }
            set { base["CurrentUserName"] = value; }
        }

        [System.Web.Profile.SettingsAllowAnonymous(false)]
        [System.Web.Profile.ProfileProvider("SqlProfileProvider")]
        [System.Configuration.SettingsSerializeAs(System.Configuration.SettingsSerializeAs.String)]  //使用xml序列化，可选。
        public string LoginUserName
        {
            get { return base["LoginUserName"].ToString(); }
            set { base["LoginUserName"] = value; }
        }

        [System.Web.Profile.SettingsAllowAnonymous(false)]
        [System.Web.Profile.ProfileProvider("SqlProfileProvider")]
        [System.Configuration.SettingsSerializeAs(System.Configuration.SettingsSerializeAs.String)]  //使用xml序列化，可选。
        public string FullName
        {
            get { return base["FullName"].ToString(); }
            set { base["FullName"] = value; }
        }

        [System.Web.Profile.SettingsAllowAnonymous(false)]
        [System.Web.Profile.ProfileProvider("SqlProfileProvider")]
        [System.Configuration.SettingsSerializeAs(System.Configuration.SettingsSerializeAs.String)]  //使用xml序列化，可选。
        public string Sex
        {
            get { return base["Sex"].ToString(); }
            set { base["Sex"] = value; }
        }

        [System.Web.Profile.SettingsAllowAnonymous(false)]
        [System.Web.Profile.ProfileProvider("SqlProfileProvider")]
        [System.Configuration.SettingsSerializeAs(System.Configuration.SettingsSerializeAs.String)]  //使用xml序列化，可选。
        public string UserGroup
        {
            get { return base["UserGroup"].ToString(); }
            set { base["UserGroup"] = value; }
        }

        [System.Web.Profile.SettingsAllowAnonymous(false)]
        [System.Web.Profile.ProfileProvider("SqlProfileProvider")]
        [System.Configuration.SettingsSerializeAs(System.Configuration.SettingsSerializeAs.String)]  //使用xml序列化，可选。
        public int? CurJi
        {
            get { return(int?)base["CurJi"]; }
            set { base["CurJi"] = value; }
        }

        [System.Web.Profile.SettingsAllowAnonymous(false)]
        [System.Web.Profile.ProfileProvider("SqlProfileProvider")]
        [System.Configuration.SettingsSerializeAs(System.Configuration.SettingsSerializeAs.String)]  //使用xml序列化，可选。
        public int? CurBh
        {
            get { return (int?)base["CurBh"]; }
            set { base["CurBh"] = value; }
        }

        [System.Web.Profile.SettingsAllowAnonymous(false)]
        [System.Web.Profile.ProfileProvider("SqlProfileProvider")]
        [System.Configuration.SettingsSerializeAs(System.Configuration.SettingsSerializeAs.String)]  //使用xml序列化，可选。
        public string CurBnbh
        {
            get { return base["CurBnbh"].ToString(); }
            set { base["CurBnbh"] = value; }
        }
    }
}
