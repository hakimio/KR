using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Dialog_Editor.utils
{
    public class Settings
    {
        public int idColumnWidth;
        public int linkColumnWidth;
        public Size windowSize;
        public Point windowLocation;

        public Settings() {}

        public Settings(int idColumnWidth, int linkColumnWidth, 
            Size windowSize, Point windowLocation)
        {
            this.idColumnWidth = idColumnWidth;
            this.linkColumnWidth = linkColumnWidth;
            this.windowSize = windowSize;
            this.windowLocation = windowLocation;
        }
    }
}
