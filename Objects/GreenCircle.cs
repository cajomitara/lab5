﻿using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace lab5.Objects
{
    class GreenCircle : BaseObject
    {
        public float size = 30;
        Random rnd = new();
        int areaWidth;
        int areaHeight;
        public GreenCircle(float x, float y, float angle, int areaWidth, int areaHeight) : base(x, y, angle)
        {
            this.areaWidth = areaWidth;
            this.areaHeight = areaHeight;
        }
        public override void Update()
        {
            size -= 0.25f;

            if (size <= 0)
            {
                X = rnd.Next(areaWidth);
                Y = rnd.Next(areaHeight);
                size = 40 + rnd.Next(60);
            }
        }
        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb(255, 144, 238, 144)), -(size / 2), -(size / 2), size, size);
        }
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-(size / 2), -(size / 2), size, size);
            return path;
        }
    }
}
