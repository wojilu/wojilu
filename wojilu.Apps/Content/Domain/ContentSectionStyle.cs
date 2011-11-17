///*
// * Copyright (c) 2010, www.wojilu.com. All rights reserved.
// */

//using System;
//using System.Text;

//namespace wojilu.Apps.Content.Domain {

//    public class ContentSectionStyle {

//        private String _Background_Color;
//        private String _Background_Image;
//        private String _Color;
//        private String _Font_Size;
//        private String _Header_Background_Color;
//        private String _Header_Background_Image;
//        private String _Header_Color;
//        private String _Header_Font_Size;
//        private String _Header_Font_Weight;
//        private String _OtherStyle;
//        private String _Width;

//        public String Background_Color {
//            get {
//                return _Background_Color;
//            }
//            set {
//                _Background_Color = value;
//            }
//        }

//        public String Background_Image {
//            get {
//                return _Background_Image;
//            }
//            set {
//                _Background_Image = value;
//            }
//        }

//        public String Color {
//            get {
//                return _Color;
//            }
//            set {
//                _Color = value;
//            }
//        }

//        public String Font_Size {
//            get {
//                return _Font_Size;
//            }
//            set {
//                _Font_Size = value;
//            }
//        }

//        public String Header_Background_Color {
//            get {
//                return _Header_Background_Color;
//            }
//            set {
//                _Header_Background_Color = value;
//            }
//        }

//        public String Header_Background_Image {
//            get {
//                return _Header_Background_Image;
//            }
//            set {
//                _Header_Background_Image = value;
//            }
//        }

//        public String Header_Color {
//            get {
//                return _Header_Color;
//            }
//            set {
//                _Header_Color = value;
//            }
//        }

//        public String Header_Font_Size {
//            get {
//                return _Header_Font_Size;
//            }
//            set {
//                _Header_Font_Size = value;
//            }
//        }

//        public String Header_Font_Weight {
//            get {
//                return _Header_Font_Weight;
//            }
//            set {
//                _Header_Font_Weight = value;
//            }
//        }

//        public String OtherStyle {
//            get {
//                return _OtherStyle;
//            }
//            set {
//                _OtherStyle = value;
//            }
//        }

//        public String Width {
//            get {
//                return _Width;
//            }
//            set {
//                _Width = value;
//            }
//        }

//        public ContentSectionStyle( String jsonString ) {
//            parseJSON( jsonString );
//        }

//        private void parseJSON( String jsonString ) {

//            string[] strArray = jsonString.TrimStart( '{' ).TrimEnd( '}' ).Split( ',' );
//            foreach (String str in strArray) {

//                string[] arrPair = str.Split( new char[] { ':' } );
//                String name = arrPair[0];
//                String val = arrPair[1];
//                if (name.Equals( "Width" )) {
//                    this.Width = val;
//                }
//                else if (name.Equals( "Color" )) {
//                    this.Color = val;
//                }
//                else if (name.Equals( "Font_Size" )) {
//                    this.Font_Size = val;
//                }
//                else if (name.Equals( "Background_Color" )) {
//                    this.Background_Color = val;
//                }
//                else if (name.Equals( "Background_Image" )) {
//                    this.Background_Image = val;
//                }
//                else if (name.Equals( "Header_Color" )) {
//                    this.Header_Color = val;
//                }
//                else if (name.Equals( "Header_Font_Size" )) {
//                    this.Header_Font_Size = val;
//                }
//                else if (name.Equals( "Header_Font_Weight" )) {
//                    this.Header_Font_Weight = val;
//                }
//                else if (name.Equals( "Header_Background_Color" )) {
//                    this.Header_Background_Color = val;
//                }
//                else if (name.Equals( "Header_Background_Image" )) {
//                    this.Header_Background_Image = val;
//                }
//            }
//        }

//        public String ToJSON() {
//            StringBuilder builder = new StringBuilder();
//            builder.Append( "{" );
//            builder.AppendFormat( "Width:{0},", this.Width );
//            builder.AppendFormat( "Color:{0},", this.Color );
//            builder.AppendFormat( "Font_Size:{0},", this.Font_Size );
//            builder.AppendFormat( "Background_Color:{0},", this.Background_Color );
//            builder.AppendFormat( "Background_Image:{0},", this.Background_Image );
//            builder.AppendFormat( "Header_Color:{0},", this.Header_Color );
//            builder.AppendFormat( "Header_Font_Size:{0},", this.Header_Font_Size );
//            builder.AppendFormat( "Header_Font_Weight:{0},", this.Header_Font_Weight );
//            builder.AppendFormat( "Header_Background_Color:{0},", this.Header_Background_Color );
//            builder.AppendFormat( "Header_Background_Image:{0},", this.Header_Background_Image );
//            builder.Append( "}" );
//            return builder.ToString();
//        }

//        public String ToStyleString() {
//            StringBuilder builder = new StringBuilder();
//            builder.Append( "<style>" );
//            builder.AppendFormat( ".pagesection {width:{0}px;}", this.Width );
//            builder.AppendFormat( ".pagesection a{color:{0};}", this.Color );
//            builder.AppendFormat( ".pagesection {font-size:{0}px;}", this.Font_Size );
//            builder.AppendFormat( ".pagesection {background-color:{0};}", this.Background_Color );
//            builder.AppendFormat( ".pagesection {background-image:{0};}", this.Background_Image );
//            builder.AppendFormat( ".pagesection .section_header a{color:{0};}", this.Header_Color );
//            builder.AppendFormat( ".pagesection .section_header {font-size:{0}px;}", this.Header_Font_Size );
//            builder.AppendFormat( ".pagesection .section_header {font-weight:{0};}", this.Header_Font_Weight );
//            builder.AppendFormat( ".pagesection .section_header {background-color:{0};}", this.Header_Background_Color );
//            builder.AppendFormat( ".pagesection .section_header {background-image:{0};", this.Header_Background_Image );
//            builder.Append( "</style>" );
//            return builder.ToString();
//        }

//    }
//}

