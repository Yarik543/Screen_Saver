using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Screen_Saver
{
    public partial class ScreenMain : Form
    {
        class Snowflake
        {
            public float X;
            public float Y;
            public float Speed;
            public float Size;
            public bool IsStar; // звёздочка или шарик
        }

        private List<Snowflake> snowflakes = new List<Snowflake>();
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private Random rand = new Random();

        public ScreenMain()
        {
            InitializeComponent();

            // Настройки окна
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.DoubleBuffered = true;
            this.KeyDown += (s, e) => this.Close();
            this.MouseDown += (s, e) => this.Close();

            // Создаём снежинки
            for (int i = 0; i < 150; i++)
            {
                snowflakes.Add(CreateSnowflake());
            }

            // Таймер
            timer.Interval = 10;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private Snowflake CreateSnowflake()
        {
            float size = rand.Next(5, 25);
            return new Snowflake
            {
                X = rand.Next(0, Screen.PrimaryScreen.Bounds.Width),
                Y = rand.Next(-Screen.PrimaryScreen.Bounds.Height, 0),
                Speed = (float)(rand.NextDouble() * 5 + size / 6f),
                Size = size,
                IsStar = rand.Next(2) == 0 // половина кружки, половина звёздочки
            };
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (var flake in snowflakes)
            {
                flake.Y += flake.Speed;

                if (flake.Y > Screen.PrimaryScreen.Bounds.Height)
                {
                    // перезапускаем сверху
                    flake.Y = -flake.Size;
                    flake.X = rand.Next(0, Screen.PrimaryScreen.Bounds.Width);
                }
            }
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (SolidBrush brush = new SolidBrush(Color.White))
            using (Pen pen = new Pen(Color.White, 1))
            {
                foreach (var flake in snowflakes)
                {
                    if (flake.IsStar)
                    {
                        // рисуем простую "звёздочку" (крестик)
                        g.DrawLine(pen, flake.X - flake.Size / 2, flake.Y, flake.X + flake.Size / 2, flake.Y);
                        g.DrawLine(pen, flake.X, flake.Y - flake.Size / 2, flake.X, flake.Y + flake.Size / 2);
                    }
                    else
                    {
                        // рисуем круг
                        g.FillEllipse(brush, flake.X, flake.Y, flake.Size, flake.Size);
                    }
                }
            }
        }
    }
}