using lab5.Objects;
using System.Drawing.Imaging;

namespace lab5
{
    public partial class Form1 : Form
    {
        Random rnd = new();
        GreenCircle myCircle; // создадим поле под наш прямоугольник
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        public Form1()
        {
            InitializeComponent();

            myCircle = new GreenCircle(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);

            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            player.OnGCOverlap += (gc) =>
            {
                myCircle.X = rnd.Next() % pbMain.Width;
                myCircle.Y = rnd.Next() % pbMain.Height;
            };

            objects.Add(player);
            objects.Add(myCircle);
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            updatePlayer();

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
            }

            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }
        public void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                // по сути мы теперь используем вектор dx, dy
                // как вектор ускорения, точнее даже вектор притяжения
                // который притягивает игрока к маркеру
                // 0.5 просто коэффициент который подобрал на глаз
                // и который дает естественное ощущение движения
                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                // расчитываем угол поворота игрока 
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            // тормозящий момент,
            // нужен чтобы, когда игрок достигнет маркера произошло постепенное замедление
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            // пересчет позиция игрока с помощью вектора скорости
            player.X += player.vX;
            player.Y += player.vY;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // и главное не забыть пололжить в objects
            }

            marker.X = e.X;
            marker.Y = e.Y;
        }
    }
}
