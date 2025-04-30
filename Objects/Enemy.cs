using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace lab5.Objects
{
    class Enemy : BaseObject
    {
        public int size = 30;
        public float vX, vY;
        public Enemy(float x, float y, float angle) : base(x, y, angle)
        { }
        public void UpdateEnemy(float playerX, float playerY)
        {
            float dx = playerX - X;
            float dy = playerY - Y;
            float length = MathF.Sqrt(dx * dx + dy * dy);
            dx /= length;
            dy /= length;

            vX += dx * 0.2f;
            vY += dy * 0.2f;

            vX += -vX * 0.1f;
            vY += -vY * 0.1f;

            X += vX;
            Y += vY;

            Angle += 4f;

            Angle %= 360;
        }
        public override void Render(Graphics g)
        {
            using (var path = new GraphicsPath())
            {
                PointF[] points = {
                    new PointF(0, -size/2),
                    new PointF(size/2, size/2),
                    new PointF(-size/2, size/2)
                };

                path.AddPolygon(points);

                var transform = new Matrix();
                transform.Rotate(Angle);
                path.Transform(transform);

                g.FillPath(new SolidBrush(Color.Red), path);
                g.DrawPath(new Pen(Color.Black, 2), path);
            }
        }
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddPolygon(new[] {
                new PointF(0, -size/2),
                new PointF(size/2, size/2),
                new PointF(-size/2, size/2)
            });
            return path;
        }
    }
}
