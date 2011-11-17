/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace wojilu.Drawing {

    /// <summary>
    /// 水印工具
    /// </summary>
    public class Watermark {

        /// <summary>
        /// 将原始图片添加图片水印之后存储
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="savePath"></param>
        /// <param name="watermarkPath"></param>
        /// <param name="wp"></param>
        public static void MakeByPic( String srcPath, String savePath, String watermarkPath, WatermarkPosition wp ) {

            using (Image src = Image.FromFile( srcPath )) {
                using (Bitmap bm = new Bitmap( srcPath )) {
                    using (Graphics g = Graphics.FromImage( bm )) {
                        using (Image wm = Image.FromFile( watermarkPath )) {

                            ImageAttributes imgAttr = getImageAttributes();
                            Rectangle rect = getPicRectangle( src, wm, wp );

                            g.DrawImage( wm, rect, 0, 0, wm.Width, wm.Height, GraphicsUnit.Pixel, imgAttr );

                            try {
                                bm.Save( savePath );
                            }
                            catch (Exception e) {
                                throw e;
                            }

                        }
                    }
                }
            }

        }

        private static ImageAttributes getImageAttributes() {

            ImageAttributes imgAttr = new ImageAttributes();

            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb( 255, 0, 255, 0 );
            colorMap.NewColor = Color.FromArgb( 0, 0, 0, 0 );
            ColorMap[] remapTable = { colorMap };

            imgAttr.SetRemapTable( remapTable, ColorAdjustType.Bitmap );

            float[][] colorMatrixElements = { 
               new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
               new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
               new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
               new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},
               new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
            };

            ColorMatrix wmColorMatrix = new ColorMatrix( colorMatrixElements );
            imgAttr.SetColorMatrix( wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap );

            return imgAttr;
        }

        private static Rectangle getPicRectangle( Image src, Image wm, WatermarkPosition wp ) {

            int x = 10;
            int y = 10;

            if (wp == WatermarkPosition.TopLeft) {
            }
            else if (wp == WatermarkPosition.TopCenter) {
                x = src.Width / 2 - wm.Width / 2;
                y = 10;
            }
            else if (wp == WatermarkPosition.TopRight) {
                x = ((src.Width - wm.Width) - 10);
                y = 10;
            }
            else if (wp == WatermarkPosition.BottomLeft) {
                x = 10;
                y = src.Height - wm.Height - 10;
            }
            else if (wp == WatermarkPosition.BottomCenter) {
                x = src.Width / 2 - wm.Width / 2;
                y = src.Height - wm.Height - 10;
            }
            else if (wp == WatermarkPosition.BottomRight) {
                x = ((src.Width - wm.Width) - 10);
                y = src.Height - wm.Height - 10;
            }

            return new Rectangle( x, y, wm.Width, wm.Height );
        }

        /// <summary>
        /// 将原始图片添加文字水印之后存储
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="savePath"></param>
        /// <param name="words"></param>
        /// <param name="wp"></param>
        /// <param name="fontSize"></param>
        public static void MakeByText( String srcPath, String savePath, String words, WatermarkPosition wp, int fontSize ) {

            using (Image src = Image.FromFile( srcPath )) {
                using (Bitmap bm = new Bitmap( srcPath )) {
                    using (Graphics g = Graphics.FromImage( bm )) {

                        FontAndSize fs = FontAndSize.GetValue( g, words, "arial", fontSize, src.Width );
                        PointF p = getTextPoint( src, fs.size, wp );
                        StringFormat sf = getStringFormat();

                        drawText( g, words, fs.font, p, sf );

                        try {
                            bm.Save( savePath );
                        }
                        catch (Exception e) {
                            throw e;
                        }

                    }
                }
            }

        }

        private static void drawText( Graphics g, String wmText, Font font, PointF p, StringFormat format ) {

            SolidBrush brush = new SolidBrush( Color.FromArgb( 153, 255, 255, 255 ) );
            g.DrawString( wmText, font, brush, p, format );

            SolidBrush brush2 = new SolidBrush( Color.FromArgb( 153, 0, 0, 0 ) );
            g.DrawString( wmText, font, brush2, new PointF( p.X + 1, p.Y + 1 ), format );
        }

        private static StringFormat getStringFormat() {
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;
            return StrFormat;
        }

        private static PointF getTextPoint( Image src, SizeF textSize, WatermarkPosition wp ) {

            float x = textSize.Width / 2 + 5;
            float y = textSize.Height / 2 + 5;

            int margin = 10;

            if (wp == WatermarkPosition.TopLeft) {
                x = getLeftPosition( src, textSize, margin );
                y = getTopPosition( src, textSize, margin );
            }
            else if (wp == WatermarkPosition.TopCenter) {
                x = src.Width / 2;
                y = getTopPosition( src, textSize, margin );
            }
            else if (wp == WatermarkPosition.TopRight) {
                x = getRightPosition( src, textSize, margin );
                y = getTopPosition( src, textSize, margin );
            }
            else if (wp == WatermarkPosition.BottomLeft) {
                x = getLeftPosition( src, textSize, margin );
                y = getBottomPosition( src, textSize, margin );
            }
            else if (wp == WatermarkPosition.BottomCenter) {
                x = src.Width / 2;
                y = getBottomPosition( src, textSize, margin );
            }
            else if (wp == WatermarkPosition.BottomRight) {
                x = getRightPosition( src, textSize, margin );
                y = getBottomPosition( src, textSize, margin );
            }

            return new PointF( x, y );
        }

        private static float getLeftPosition( Image src, SizeF textSize, int margin ) {
            return textSize.Width / 2 + margin;
        }

        private static float getTopPosition( Image src, SizeF textSize, int margin ) {
            return textSize.Height / 2 + margin / 2;
        }

        private static float getRightPosition( Image src, SizeF textSize, int margin ) {
            float x = src.Width - margin * 2 - (textSize.Width / 2);
            return x;
        }

        private static float getBottomPosition( Image src, SizeF textSize, int margin ) {
            float y = src.Height - margin * 2 - (textSize.Height / 2);
            return y;
        }



    }

}
