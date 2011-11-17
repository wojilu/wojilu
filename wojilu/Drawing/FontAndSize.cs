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

namespace wojilu.Drawing {

    /// <summary>
    /// ×ÖÌå³ß´ç
    /// </summary>
    public class FontAndSize {

        public Font font { get; set; }
        public SizeF size { get; set; }

        public static FontAndSize GetValue( Graphics g, String text, String fontFamily, int fontSize, int srcWidth ) {

            FontAndSize wfont = new FontAndSize();

            Font font = null;
            SizeF size = new SizeF();

            for (int i = fontSize; i > 6; i = i - 2) {
                font = new Font( fontFamily, i, FontStyle.Bold );
                size = g.MeasureString( text, font );
                if (size.Width <= srcWidth) break;
            }

            wfont.font = font;
            wfont.size = size;

            return wfont;
        }
    }

}
