using lab5.Objects;
using System.Drawing.Imaging;

namespace lab5
{
    public partial class Form1 : Form
    {
        Random rnd = new();

        List<BaseObject> objects = new();
        List<Enemy> enemies = new();

        Player player;
        Marker marker;
        int score = 0;
        int time = 15;
        public Form1()
        {
            InitializeComponent();

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
                gc.size = 20 + rnd.Next(60);
                gc.X = rnd.Next() % pbMain.Width;
                gc.Y = rnd.Next() % pbMain.Height;

                score += 1;
            };

            player.OnRCOverlap += (rc) =>
            {
                objects.Remove(rc);
                rc = null;

                if (score > 0)
                {
                    score -= 1;
                }
            };

            player.OnEnemyOverlap += (e) =>
            {
                objects.Remove(e);
                enemies.Remove(e);
                e = null;

                if (score > 0)
                {
                    score -= 1;
                }
            };

            objects.Add(player);
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            updatePlayer();


            foreach (var enemy in enemies.ToList())
            {
                enemy.UpdateEnemy(player.X, player.Y);
            }

            foreach (var obj in objects.ToList())
            {
                obj.Update();
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

            txtScore.Text = String.Format("Очки: {0}", score);
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
                objects.Add(marker);
            }

            marker.X = e.X;
            marker.Y = e.Y;
        }

        private void gameOverTimer_Tick(object sender, EventArgs e)
        {
            txtTime.Text = "Время: " + (time).ToString();
            time -= 1;

            float newX = 0;
            float newY = 0;

            if (rnd.Next(0, 100) < 65)
            {
                var greenCircle = new GreenCircle(
                    rnd.Next() % pbMain.Width,
                    rnd.Next() % pbMain.Height,
                    0,
                    pbMain.Width,
                    pbMain.Height);

                objects.Add(greenCircle);
            }

            if (rnd.Next(0, 100) < 50)
            {
                var redCircle = new RedCircle(
                    rnd.Next() % pbMain.Width,
                    rnd.Next() % pbMain.Height,
                    0,
                    pbMain.Width,
                    pbMain.Height);

                objects.Add(redCircle);
            }

            if (rnd.Next(0, 100) < 35)
            {
                var side = new Random().Next(0, 4);
                switch (side)
                {
                    case 0:
                        newX = new Random().Next(-30, pbMain.Width + 30);
                        newY = -30 * 2;
                        break;
                    case 1:
                        newX = new Random().Next(-30, pbMain.Width + 30);
                        newY = pbMain.Height + 30 * 2;
                        break;
                    case 2:
                        newX = -30 * 2;
                        newY = new Random().Next(-30, pbMain.Height + 30);
                        break;
                    case 3:
                        newX = pbMain.Width + 30 * 2;
                        newY = new Random().Next(-30, pbMain.Height + 30);
                        break;
                }

                var enemy = new Enemy(
                    newX,
                    newY,
                    0);

                objects.Add(enemy);
                enemies.Add(enemy);
            }

            if (time < 0)
            {
                gameOverTimer.Enabled = false;
                timer1.Enabled = false;
                DialogResult result = MessageBox.Show(
                    "Ваш результат: " + score.ToString() +
                    "\nИгра окончена. Хотите сыграть ещё раз?",
                    "Конец игры",
                    MessageBoxButtons.YesNo
                );
                if (result == DialogResult.Yes)
                {
                    Application.Restart();
                    Environment.Exit(0);
                }
                else
                {
                    Application.Exit();
                }
            }
        }
    }
}
