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

namespace wojilu.Drawing {

    /// <summary>
    /// 验证码工具
    /// </summary>
    public class ValidationCode {

        /// <summary>
        /// 创建验证码，返回一个 Image 对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fontFamily"></param>
        /// <returns></returns>
        public Image CreateImage( String code, int width, int height, String fontFamily ) {

            Bitmap bm = new Bitmap( width, height );

            using (Graphics g = Graphics.FromImage( bm )) {

                g.SmoothingMode = SmoothingMode.AntiAlias;

                HatchBrush brush = new HatchBrush( HatchStyle.SmallConfetti, Color.LightGray, Color.White );
                Rectangle rect = new Rectangle( 0, 0, width, height );
                g.FillRectangle( brush, rect );

                int fontsize = rect.Height + 1;
                FontAndSize size = FontAndSize.GetValue( g, code, fontFamily, fontsize, bm.Width );

                GraphicsPath gpath = getGraphicsPath( code, size.font, rect );

                Color brushColor = ColorTranslator.FromHtml( "#000000" );
                brush = new HatchBrush( HatchStyle.Divot, brushColor, Color.DarkGray );
                g.FillPath( brush, gpath );

                addRandomNoise( g, rect );

            }
            return bm;
        }

        private void addRandomNoise( Graphics g, Rectangle rect ) {

            HatchBrush brush = new HatchBrush( HatchStyle.Weave, Color.LightGray, Color.White );

            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++) {
                int x = rd.Next( rect.Width );
                int y = rd.Next( rect.Height );
                g.FillEllipse( brush, x, y, 2, 3 );
            }
        }

        private Rectangle addRandomLine( Graphics g, Rectangle rect ) {
            int lineCount = rd.Next( 1, 8 );
            for (int i = 0; i < lineCount; i++) {

                Point pt1 = new Point();
                pt1.X = rd.Next( rect.Width );
                pt1.Y = rd.Next( rect.Height );

                Point pt2 = new Point();
                pt2.X = rd.Next( rect.Width );
                pt2.Y = rd.Next( rect.Height );

                float width = rd.Next( 2 );

                Pen p = new Pen( Color.Aqua, width );
                g.DrawLine( p, pt1, pt2 );
            }
            return rect;
        }

        private StringFormat getFormat() {
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            return format;
        }

        private GraphicsPath getGraphicsPath( String code, Font font, Rectangle rect ) {

            StringFormat format = getFormat();

            GraphicsPath path = new GraphicsPath();
            path.AddString( code, font.FontFamily, (int)font.Style, font.Size, rect, format );

            PointF[] points = getRandomPoint( rect );

            Matrix matrix = new Matrix();
            matrix.Translate( 0F, 0F );

            path.Warp( points, rect, matrix, WarpMode.Perspective, 0F );

            return path;
        }

        private PointF[] getRandomPoint( Rectangle rect ) {
            float v = 4F;
            PointF[] points =
			{
				new PointF(rd.Next(rect.Width) / v, rd.Next(rect.Height) / v),
				new PointF(rect.Width - rd.Next(rect.Width) / v, rd.Next(rect.Height) / v),
				new PointF(rd.Next(rect.Width) / v, rect.Height - rd.Next(rect.Height) / v),
				new PointF(rect.Width - rd.Next(rect.Width) / v, rect.Height - rd.Next(rect.Height) / v)
			};
            return points;
        }

        private Random rd = new Random();

    }

}
