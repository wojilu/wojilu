/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;

using wojilu.Drawing;

using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Resource;

namespace wojilu.Web.Controller.Users {

    public class UserVo {

        public UserVo( User user ) {

            this.Name = user.Name;
            this.Gender = AppResource.Gender.GetName( user.Gender );
            this.Birthday = user.BirthMonth + "-" + user.BirthDay + "";

            this.Region1 = AppResource.Province.GetName( user.ProvinceId1 ) + user.City1;
            this.Region2 = AppResource.Province.GetName( user.ProvinceId2 ) + user.City2;
            this.Relationship = AppResource.Relationship.GetName( user.Relationship );
            this.Blood = AppResource.Blood.GetName( user.Blood );
            this.Degree = AppResource.Degree.GetName( user.Degree );
            this.Zodiac = AppResource.Zodiac.GetName( user.Zodiac );

            this.Age = user.BirthYear <= 0 ? "" : (DateTime.Now.Year - user.BirthYear).ToString();
            this.Address = user.Profile.Address;

            this.Email = user.Email;
            this.QQ = user.QQ;
            this.MSN = user.MSN;
            this.Tel = user.Profile.Tel;
            this.WebSite = user.Profile.WebSite;

            this.Face = user.PicO;
            this.FaceMedium = user.PicM;
            this.FaceSmall = user.PicSmall;

            this.RoleSite = getRoleString( user );
            this.RankSite = getRankString( user );

            this.Description = user.Profile.Description;

            this.Music = user.Profile.Music;
            this.Sport = user.Profile.Sport;
            this.Movie = user.Profile.Movie;
            this.Eat = user.Profile.Eat;
            this.Book = user.Profile.Book;
        }

        public String Name {get;set;}
        public String Gender { get; set; }
        public String Birthday { get; set; }

        public String Region1 { get; set; }
        public String Region2 { get; set; }
        public String Relationship { get; set; }
        public String Blood { get; set; }
        public String Degree { get; set; }
        public String Zodiac { get; set; }

        public String Age { get; set; }

        public String Address { get; set; }
        public String Email { get; set; }
        public String QQ { get; set; }
        public String MSN { get; set; }
        public String Tel { get; set; }
        public String WebSite { get; set; }

        public String Face { get; set; }
        public String FaceMedium { get; set; }
        public String FaceSmall { get; set; }

        public String RoleSite { get; set; }
        public String RankSite { get; set; }

        public String Description { get; set; }

        public String Music { get; set; }
        public String Sport { get; set; }
        public String Movie { get; set; }
        public String Book { get; set; }
        public String Eat { get; set; }


        private String getRoleString( User user ) {
            SiteRole role = SiteRole.GetById( user.Id, user.RoleId );
            return role == null ? "" : role.Name;
        }

        private String getRankString( User user ) {
            SiteRank rank = SiteRank.GetById( user.RankId );
            return rank == null ? "" : rank.Name;
        }

    }

}
