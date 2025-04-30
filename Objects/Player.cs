using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace lab5.Objects
{
    class Player : BaseObject
    {
        public Action<Marker> OnMarkerOverlap;
        public Action<GreenCircle> OnGCOverlap;
        public Action<RedCircle> OnRCOverlap;
        public Action<Enemy> OnEnemyOverlap;
        public float vX, vY;
        public Player(float x, float y, float angle) : base(x, y, angle)
        {

        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(
                new SolidBrush(Color.Chartreuse),
                -15, -15,
                30, 30
            );
            g.DrawEllipse(
                new Pen(Color.Black, 2),
                -15, -15,
                30, 30
                );
            g.DrawLine(new Pen(Color.Black, 2), 0, 0, 25, 0);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-15, -15, 30, 30);
            return path;
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);

            if(obj is Marker)
            {
                OnMarkerOverlap(obj as Marker);
            }
            else if (obj is GreenCircle)
            {
                OnGCOverlap(obj as GreenCircle);
            }
            else if (obj is RedCircle)
            {
                OnRCOverlap(obj as RedCircle);
            }
            else if (obj is Enemy)
            {
                OnEnemyOverlap(obj as Enemy);
            }
        }
    }
}
