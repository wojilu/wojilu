/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */


using System;
using System.Collections.Generic;

using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;

namespace wojilu.Apps.Shop.Service {

    public class ShopSectionTemplateService : IShopSectionTemplateService {

        public virtual List<ShopSectionTemplate> GetAll() {
            return cdb.findAll<ShopSectionTemplate>();
        }

        public virtual List<ShopSectionTemplate> GetBy( String filterString ) {

            List<ShopSectionTemplate> list = new List<ShopSectionTemplate>();
            if (strUtil.IsNullOrEmpty( filterString )) return list;

            string[] arrFilter = filterString.Split( ';' );
            List<ShopSectionTemplate> all = this.GetAll();
            foreach (ShopSectionTemplate template in all) {
                if (this.isNeeded( template, arrFilter )) {
                    list.Add( template );
                }
            }

            return list;
        }

        public virtual ShopSectionTemplate GetById( int id ) {
            return cdb.findById<ShopSectionTemplate>( id ) ;
        }

        private Boolean isNeeded( ShopSectionTemplate template, string[] arrFilter ) {
            foreach (String ft in arrFilter) {
                if (strUtil.HasText( ft ) && template.TemplateName.Equals( ft.Trim() )) {
                    return true;
                }
            }
            return false;
        }
    }
}

