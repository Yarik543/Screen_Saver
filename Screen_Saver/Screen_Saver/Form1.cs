using System.Windows.Forms;
namespace Screen_Saver
{
    public partial class Form1 : Form
    {
        class Snowflake
        {
            public float X;
            public float Y;
            public float Speed;
            public float Size;
            public Image Image;
        }

        private List<Snowflake> snowflakes = new List<Snowflake>();
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private Random rand = new Random();
        private Image background;
        private Image[] snowImages;

        public Form1()
        {
            InitializeComponent();
            // ��������� ����
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.DoubleBuffered = true;
            this.KeyDown += (s, e) => this.Close();
            this.MouseDown += (s, e) => this.Close();


            // ��������� �������� 
            snowImages = new Image[]
            {
                Image.FromFile(@"C:\C#\�������� 4 ����\Screen_Saver\Snow1.png"),
                Image.FromFile(@"C:\C#\�������� 4 ����\Screen_Saver\Snow2.png")
            };

            // ������ ��������
            for (int i = 0; i < 120; i++)
            {
                snowflakes.Add(CreateSnowflake());
            }

            // ������
            timer.Interval = 10;
            timer.Tick += timer1_Tick;
            timer.Start();
        }

        private Snowflake CreateSnowflake()
        {
            float size = rand.Next(10, 35);
            return new Snowflake
            {
                X = rand.Next(0, Screen.PrimaryScreen.Bounds.Width),
                Y = rand.Next(-Screen.PrimaryScreen.Bounds.Height, 0),
                Speed = (float)(rand.NextDouble() * 2 + size / 15f), // ������� ������ �������
                Size = size,
                Image = snowImages[rand.Next(snowImages.Length)]
            };
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var flake in snowflakes)
            {
                flake.Y += flake.Speed;
                if (flake.Y > Screen.PrimaryScreen.Bounds.Height)
                {
                    // ������������� ������
                    flake.Y = -flake.Size;
                    flake.X = rand.Next(0, Screen.PrimaryScreen.Bounds.Width);
                }
            }
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;


            // ������ ��������
            foreach (var flake in snowflakes)
            {
                g.DrawImage(flake.Image, flake.X, flake.Y, flake.Size, flake.Size);
            }
        }
    }
}
