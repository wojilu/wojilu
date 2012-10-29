/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Service;
using wojilu.Web.Context;
using wojilu.Apps.Photo.Interface;

namespace wojilu.Web.Controller.Photo.Vo {

    public class Post {

        public String Title { get; set; }
        public String Description { get; set; }
        public String ImgUrl { get; set; }
        public String ImgMediumUrl { get; set; }
        public String ImgThumbUrl { get; set; }

        public String Creator { get; set; }
        public String Created { get; set; }
        public int Hits { get; set; }

        public String Album { get; set; }
        public String AlbumLink { get; set; }
        public String AlbumAndLink { get; set; }

        public String Tags { get; set; }
        public String PrevNext { get; set; }
        public String NextLink { get; set; }

        public static Post Fill( PhotoPost post, MvcContext ctx, IPhotoPostService postService ) {

            Post p = new Post();

            p.Title = post.Title;
            p.Description = post.Description;
            p.ImgUrl = post.ImgUrl;
            p.ImgMediumUrl = post.ImgMediumUrl;
            p.ImgThumbUrl = post.ImgThumbUrl;

            p.Creator = post.Creator.Name;
            p.Created = post.Created.ToShortDateString();
            p.Hits = post.Hits;

            if (post.PhotoAlbum == null || post.PhotoAlbum.Id == 0) 
                p.Album = alang( ctx, "defaultAlbum" );
            else
                p.Album = post.PhotoAlbum.Name;


            p.AlbumLink = ctx.link.To( new PhotoController().Album, post.PhotoAlbum.Id );

            String lnk = string.Format( " | <a href=\"{0}\">{1}《{2}》</a> ", p.AlbumLink, alang( ctx, "returnAlbum" ), p.Album );

            p.AlbumAndLink = lnk;// strUtil.HasText( p.Album ) ? lnk : "";

            p.Tags = post.Tag.List.Count > 0 ? "tag:" + post.Tag.HtmlString : "";
            String prevnext = getPreNextHtml( ctx, post, postService );
            String nextLink = getNextLink( ctx, post, postService );

            p.PrevNext = prevnext;
            p.NextLink = nextLink;

            return p;
        }

        private static String alang( MvcContext ctx, String key ) {
            return ctx.controller.alang( key );
        }


        private static String getPreNextHtml( MvcContext ctx, PhotoPost post, IPhotoPostService postService ) {

            PhotoPost prev = postService.GetPre( post );
            PhotoPost next = postService.GetNext( post );

            String prenext;
            if (prev == null && next == null)
                prenext = "";
            else if (prev == null)
                prenext = "<a href=\"" + alink.ToAppData( next ) + "\">" + alang( ctx, "nextPhoto" ) + "</a> ";
            else if (next == null)
                prenext = "<a href=\"" + alink.ToAppData( prev ) + "\">" + alang( ctx, "prevPhoto" ) + "</a> ";
            else
                prenext = "<a href=\"" + alink.ToAppData( prev ) + "\">" + alang( ctx, "prevPhoto" ) + "</a> | <a href=\"" + alink.ToAppData( next ) + "\">" + alang( ctx, "nextPhoto" ) + "</a>";
            return prenext;
        }

        private static String getNextLink( MvcContext ctx, PhotoPost post, IPhotoPostService postService ) {
            PhotoPost next = postService.GetNext( post );
            if (next == null) {
                PhotoPost first = postService.GetFirst( post );
                return alink.ToAppData( first );
            }
            return alink.ToAppData( next );
        }

    }

}
