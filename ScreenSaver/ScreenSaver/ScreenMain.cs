using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace ScreenSaver
{
    public partial class ScreenMain : Form
    {

        private List<Snowflake> snowflakes = new List<Snowflake>();
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private Random rand = new Random();

        private Bitmap[] snowflakeImages;
        public ScreenMain()
        {
            InitializeComponent();
            // Настройка формы
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.DoubleBuffered = true;
            this.KeyDown += (s, e) => this.Close();
            this.MouseDown += (s, e) => this.Close();

            // Грузим картинки из ресурсов
            snowflakeImages = new Bitmap[]
            {
                Properties.Resources.q,
                Properties.Resources.w
            };

            for (int i = 0; i < 150; i++)
            {
                snowflakes.Add(CreateSnowflake());
            }

            timer.Interval = 10;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private Snowflake CreateSnowflake()
        {
            var size = rand.Next(15, 50); // чуть крупнее, т.к. изображение красивее
            return new Snowflake
            {
                X = rand.Next(0, Screen.PrimaryScreen.Bounds.Width),
                Y = rand.Next(-Screen.PrimaryScreen.Bounds.Height, 0),
                Speed = (float)(rand.NextDouble() * 3 + size / 15f),
                Size = size,
                Image = snowflakeImages[rand.Next(snowflakeImages.Length)]
            };
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (var flake in snowflakes)
            {
                flake.Y += flake.Speed;

                if (flake.Y > Screen.PrimaryScreen.Bounds.Height)
                {
                    // Перезапускаем сверхуы
                    flake.Y = -flake.Size;
                    flake.X = rand.Next(0, Screen.PrimaryScreen.Bounds.Width);
                }
            }
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (var flake in snowflakes)
            {
                g.DrawImage(flake.Image, flake.X, flake.Y, flake.Size, flake.Size);
            }
        }
    }
}
