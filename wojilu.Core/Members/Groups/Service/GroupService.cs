/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Context;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Domain;
using wojilu.Common.Jobs;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Groups.Interface;

namespace wojilu.Members.Groups.Service {


    public class GroupService : IGroupService {

        public virtual IMemberAppService appService { get; set; }
        public virtual IMenuService menuService { get; set; }
        public virtual IMemberGroupService mgrService { get; set; }

        public GroupService() {
            appService = new GroupAppService();
            menuService = new GroupMenuService();
            mgrService = new MemberGroupService();
        }

        public virtual Result Create( User creator, String name, String url, String description, int categoryId, int accessStats, MvcContext ctx ) {
            Group g = populateGroup( creator, name, url, description, categoryId, accessStats );
            Result result = insertGroup( g );
            if (result.IsValid) {
                mgrService.JoinCreateGroup( creator, g, ctx.Ip );
            }
            return result;
        }

        private Result insertGroup( Group g ) {
            Result result = this.isValid( g );
            if (result.IsValid) {
                g.TemplateId = GroupSetting.Instance.TemplateId;
                result = db.insert( g );
                result.Info = g;
            }
            return result;
        }

        public virtual Group GetByName( String name ) {
            return db.find<Group>( "Name=:name" ).set( "name", name ).first();
        }

        public virtual Group GetByUrl( String friendUrl ) {
            return db.find<Group>( "Url=:furl" ).set( "furl", friendUrl ).first();
        }

        public Boolean IsNameReservedOrExist( String inputName ) {

            if (strUtil.IsNullOrEmpty( inputName )) return true;

            if (config.Instance.Site.IsReservedKeyContains( inputName )) return true;

            // 是否存在
            Group g = GetByName( inputName );
            if (g != null) return true;

            return false;
        }

        public Boolean IsUrlReservedOrExist( String url ) {

            if (strUtil.IsNullOrEmpty( url )) return true;

            if (config.Instance.Site.IsReservedKeyContains( url )) return true;

            // 是否存在
            Group g = GetByUrl( url );
            if (g != null) return true;

            return false;
        }

        private Result isValid( Group g ) {
            Result result = new Result();
            if (this.GetByName( g.Name ) != null) result.Add( lang.get( "exNameFound" ) );
            if (this.GetByUrl( g.Url ) != null) result.Add( lang.get( "exUrlFound" ) );
            return result;
        }

        private static Group populateGroup( User creator, String name, String url, String description, int categoryId, int accessStats ) {
            Group group = new Group();
            group.Creator = creator;
            group.Name = name;
            group.Url = url;
            group.Category = new GroupCategory( categoryId );
            group.Description = description;
            group.AccessStatus = accessStats;

            if (group.AccessStatus == GroupAccessStatus.Secret)
                group.IsCloseJoinCmd = 1;

            return group;
        }


        public virtual void UpdateLogo( Group group ) {
            db.update( group, "Logo" );
        }

        public virtual Group GetById( int id ) {
            return db.findById<Group>( id );
        }


        public virtual List<Group> GetRecent( int count ) {

            if (count <= 0) count = 10;

            return db.find<Group>( getCondition() + " order by Id desc" )
                .list( count );
        }


        public virtual List<Group> AdminGetRecent( int count ) {

            if (count <= 0) count = 10;

            return db.find<Group>( " order by Id desc" ).list( count );
        }


        public virtual void AddHits( Group group ) {
            HitsJob.Add( group );
        }


        public virtual List<Group> Search( String term ) {

            if (strUtil.IsNullOrEmpty( term )) return new List<Group>();
            term = strUtil.SqlClean( term, 10 );
            return db.find<Group>( "Name like '%" + term + "%'" ).list();
        }

        public virtual DataPage<Group> SearchByCondition( String condition ) {

            String sconditiion;
            if (strUtil.HasText( condition ))
                sconditiion = condition + " and " + getCondition();
            else
                sconditiion = getCondition();

            return db.findPage<Group>( sconditiion );
        }

        public virtual DataPage<Group> AdminSearchByCondition( String condition ) {
            return db.findPage<Group>( condition );
        }


        public virtual void Lock( Group g ) {

            if (g.IsLock == 1)
                g.IsLock = 0;
            else
                g.IsLock = 1;

            db.update( g, "IsLock" );
        }

        public virtual void SystemHide( Group g ) {
            if (g.IsSystemHide == 1)
                g.IsSystemHide = 0;
            else
                g.IsSystemHide = 1;

            db.update( g, "IsSystemHide" );
        }

        public virtual void Delete( int id ) {
            Group g = GetById( id );
            if (g != null) db.delete( g );
        }


        public virtual List<Group> GetHots( int count ) {
            if (count <= 0) count = 10;
            return db.find<Group>( getCondition() + " order by MemberCount desc, Id desc" ).list( count );
        }


        public virtual DataPage<Group> GetByCategory( int categoryId ) {
            if (categoryId <= 0) return db.findPage<Group>( getCondition() );
            return db.findPage<Group>( "CategoryId=" + categoryId + " and " + getCondition() );
        }

        private String getCondition() {
            return "(AccessStatus<>" + GroupAccessStatus.Secret + " and IsSystemHide<>1 )";
        }




    }
}

