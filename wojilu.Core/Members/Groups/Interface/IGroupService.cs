/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Context;
using wojilu.Members.Groups.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Members.Groups.Interface;

namespace wojilu.Members.Groups.Interface {
    
    public interface IGroupService {

        IMemberAppService appService { get; set; }
        IMenuService menuService { get; set; }
        IMemberGroupService mgrService { get; set; }

        void AddHits( Group group );

        Result Create( User creator, String name, String url, String description, int categoryId, int accessStats, MvcContext ctx );


        Group GetById( int id );
        Group GetByName( String name );
        Group GetByUrl( String friendUrl );

        List<Group> GetHots( int count );
        List<Group> GetRecent( int count );
        DataPage<Group> GetByCategory( int categoryId );

        List<Group> AdminGetRecent( int count );
        DataPage<Group> AdminSearchByCondition( String condition );

        List<Group> Search( String term );
        DataPage<Group> SearchByCondition( String condition );

        void Lock( Group g );
        void SystemHide( Group g );
        void UpdateLogo( Group group );

        void Delete( int id );


        Boolean IsNameReservedOrExist( String name );
        Boolean IsUrlReservedOrExist( String url );
    }

}
