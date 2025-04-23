using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5.Objects
{
    class GreenCircle : BaseObject
    {
        public GreenCircle(float x, float y, float angle) : base(x, y, angle)
        {
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.LawnGreen), -15, -15, 30, 30);
            g.DrawEllipse(new Pen(Color.Green, 2), -15, -15, 30, 30);
        }
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-15, -15, 30, 30);
            return path;
        }
    }
}
