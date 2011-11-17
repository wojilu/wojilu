/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */


using System;
using System.Collections.Generic;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;

namespace wojilu.Apps.Content.Service {

    public class ContentSectionTemplateService : IContentSectionTemplateService {

        public virtual List<ContentSectionTemplate> GetAll() {
            return cdb.findAll<ContentSectionTemplate>();
        }

        public virtual List<ContentSectionTemplate> GetBy( String filterString ) {

            List<ContentSectionTemplate> list = new List<ContentSectionTemplate>();
            if (strUtil.IsNullOrEmpty( filterString )) return list;

            string[] arrFilter = filterString.Split( ';' );
            List<ContentSectionTemplate> all = this.GetAll();
            foreach (ContentSectionTemplate template in all) {
                if (this.isNeeded( template, arrFilter )) {
                    list.Add( template );
                }
            }

            return list;
        }

        public virtual ContentSectionTemplate GetById( int id ) {
            return cdb.findById<ContentSectionTemplate>( id ) ;
        }

        private Boolean isNeeded( ContentSectionTemplate template, string[] arrFilter ) {
            foreach (String ft in arrFilter) {
                if (strUtil.HasText( ft ) && template.TemplateName.Equals( ft.Trim() )) {
                    return true;
                }
            }
            return false;
        }
    }
}

