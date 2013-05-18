using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Context;

namespace wojilu.Web.Controller.Content.Utils {

    public class TemplateUtil {

        public static List<ContentSectionTemplate> addJson( List<ContentSectionTemplate> tpls ) {

            List<ContentSectionTemplate> list = new List<ContentSectionTemplate>();
            foreach (ContentSectionTemplate t in tpls) list.Add( t );
            list.Add( getJsonTemplate() );
            return list;

        }

        public static ContentSectionTemplate getJsonTemplate() {

            ContentSectionTemplate ct = new ContentSectionTemplate();
            ct.Name = "json自定义模板<br/><span class=\"note\">(需要自己写js来绑定)</span>";
            ct.TemplateName = "json";

            return ct;

        }

        public static String getServiceTemplates( String filterString, ContentSectionTemplate tpl, MvcContext ctx, IContentSectionTemplateService templateService ) {
            String tplName = tpl==null? "json": tpl.TemplateName;
            return getServiceTemplates( filterString, tplName, ctx, templateService );
        }

        public static String getServiceTemplates( String filterString, String tpl, MvcContext ctx, IContentSectionTemplateService templateService ) {

            String thumbPath = BinderUtils.GetBinderTemplateThumbPath();
            StringBuilder builder = new StringBuilder();
            builder.Append( "<table style=\"width: 100%\" cellpadding='5'><tr>" );

            List<ContentSectionTemplate> templates = templateService.GetBy( filterString );
            if (ctx.owner.obj is Site) {
                templates = TemplateUtil.addJson( templates );
            }

            int icount = 0;
            foreach (ContentSectionTemplate template in templates) {

                builder.AppendFormat( "<td style=\"vertical-align:top;\"><label for=\"template{0}\"><img src='{1}{2}.png' style='width:150px;height:100px;' /></label><br/>", template.Id, thumbPath, template.TemplateName );
                builder.AppendFormat( "<input name=\"templateId\" id=\"template{0}\" type=\"radio\" value=\"{0}\"", template.Id );
                if (tpl.Equals( template.TemplateName )) builder.Append( " checked " );
                builder.AppendFormat( "/><label for=\"template{0}\">{1}</label></td>", template.Id, template.Name );

                if ((icount % 4) == 3) builder.Append( "</tr><tr>" );

                icount++;
            }
            builder.Append( "</table>" );
            return builder.ToString();
        }


    }

}
