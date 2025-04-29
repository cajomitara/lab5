using lab5.Objects;
using System.Drawing.Imaging;

namespace lab5
{
    public partial class Form1 : Form
    {
        Random rnd = new();
        GreenCircle firstCircle;
        GreenCircle secondCircle;
        GreenCircle thirdCircle;
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        int score = 0;
        int time = 15;
        public Form1()
        {
            InitializeComponent();

            firstCircle = new GreenCircle(
                rnd.Next() % pbMain.Width,
                rnd.Next() % pbMain.Height,
                0,
                pbMain.Width,
                pbMain.Height);

            secondCircle = new GreenCircle(
                rnd.Next() % pbMain.Width,
                rnd.Next() % pbMain.Height,
                0,
                pbMain.Width,
                pbMain.Height);

            thirdCircle = new GreenCircle(
                rnd.Next() % pbMain.Width,
                rnd.Next() % pbMain.Height,
                0,
                pbMain.Width,
                pbMain.Height);

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);

            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] ����� ��������� � {obj}\n" + txtLog.Text;
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            player.OnGCOverlap += (gc) =>
            {
                gc.X = rnd.Next() % pbMain.Width;
                gc.Y = rnd.Next() % pbMain.Height;
                score += 1;
            };

            objects.Add(player);
            objects.Add(firstCircle);
            objects.Add(secondCircle);
            objects.Add(thirdCircle);
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

            txtScore.Text = String.Format("����: {0}", score);
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

                // �� ���� �� ������ ���������� ������ dx, dy
                // ��� ������ ���������, ������ ���� ������ ����������
                // ������� ����������� ������ � �������
                // 0.5 ������ ����������� ������� �������� �� ����
                // � ������� ���� ������������ �������� ��������
                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                // ����������� ���� �������� ������ 
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            // ���������� ������,
            // ����� �����, ����� ����� ��������� ������� ��������� ����������� ����������
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            // �������� ������� ������ � ������� ������� ��������
            player.X += player.vX;
            player.Y += player.vY;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var obj in objects.ToList())
            {
                obj.Update();
            }

            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // � ������� �� ������ ��������� � objects
            }

            marker.X = e.X;
            marker.Y = e.Y;
        }

        private void gameOverTimer_Tick(object sender, EventArgs e)
        {
            txtTime.Text = "�����: " + (time).ToString();
            time -= 1;



            if (time < 0)
            {
                gameOverTimer.Enabled = false;
                timer1.Enabled = false;
                DialogResult result = MessageBox.Show(
                    "��� ��������� � " + score.ToString() +
                    "\n���� ��������. ������ ������� ��� ���?",
                    "����� ����",
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
