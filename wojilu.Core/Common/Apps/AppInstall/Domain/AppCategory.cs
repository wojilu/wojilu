/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Data;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Domain;
using wojilu.Members.Sites.Domain;

namespace wojilu.Common.AppInstall {

    public class AppCategory : CacheObject {

        public int CatId { get; set; }
        public String TypeFullName { get; set; }

        public static readonly int General = 0;

        public static int GetIdByOwnerType( String ownerType ) {

            List<AppCategory> cats = cdb.findBy<AppCategory>( "TypeFullName", ownerType );
            if (cats.Count == 0) throw new Exception( "AppCategory error, ownerType=" + ownerType );
            return cats[0].CatId;

        }

        public static String GetNameById( int catId ) {

            List<AppCategory> cats = cdb.findBy<AppCategory>( "CatId", catId );
            if (cats.Count == 0) throw new Exception( "AppCategory error, CatId=" + catId );

            return cats[0].Name;
        }

        public static String GetNameByType( String typeFullName ) {

            List<AppCategory> cats = cdb.findBy<AppCategory>( "TypeFullName", typeFullName );
            if (cats.Count == 0) throw new Exception( "AppCategory error, TypeFullName=" + typeFullName );

            return cats[0].Name;
        }


        public static List<AppCategory> GetAllWithoutGeneral() {

            List<AppCategory> results = new List<AppCategory>();
            List<AppCategory> cats = cdb.findAll<AppCategory>();
            foreach (AppCategory c in cats) {
                if (c.CatId == AppCategory.General) continue;
                results.Add( c );
            }

            return results;
        }

        public static String GetAllTypeNameWithoutGeneral() {

            List<AppCategory> cats = GetAllWithoutGeneral();

            String str = "";
            foreach (AppCategory c in cats) {
                str += c.TypeFullName + ",";
            }

            return str.TrimEnd( ',' );
        }

        public static String GetAllNameWithoutGeneral() {

            List<AppCategory> cats = GetAllWithoutGeneral();

            String str = "";
            foreach (AppCategory c in cats) {
                str += c.Name + ",";
            }

            return str.TrimEnd( ',' );
        }


        public static AppCategory GetByCatId( int catId ) {
            List<AppCategory> cats = cdb.findBy<AppCategory>( "CatId", catId );
            if (cats.Count == 0) throw new Exception( "AppCategory error, CatId=" + catId );

            return cats[0];
        }



    }

}

