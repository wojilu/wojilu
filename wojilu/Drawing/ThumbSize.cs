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

using System.Drawing;


namespace wojilu.Drawing {

    /// <summary>
    /// Àı¬‘Õº≥ﬂ¥Á
    /// </summary>
    public class ThumbSize {

        public Size New { get; set; }
        public Size Src { get; set; }

        public Point Point { get; set; }

        public Rectangle getRect() {
            return new Rectangle( this.Point.X, this.Point.Y, this.Src.Width, this.Src.Height );
        }

        public Rectangle getNewRect() {
            return new Rectangle( 0, 0, this.New.Width, this.New.Height );
        }

    }

}
