#define DEBUG 

using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms.DataVisualization.Charting;

namespace buildyourstax
{
    public class ColorProgressBar : ProgressBar
    {
        public ColorProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // None... Helps control the flicker.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            const int inset = 2; // A single inset value to control teh sizing of the inner rect.

            using (Image offscreenImage = new Bitmap(this.Width, this.Height))
            {
                using (Graphics offscreen = Graphics.FromImage(offscreenImage))
                {
                    Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

                    if (ProgressBarRenderer.IsSupported)
                        ProgressBarRenderer.DrawHorizontalBar(offscreen, rect);

                    rect.Inflate(new Size(-inset, -inset)); // Deflate inner rect.
                    rect.Width = (int)(rect.Width * ((double)this.Value / this.Maximum));
                    if (rect.Width == 0) rect.Width = 1; // Can't draw rec with width of 0.

                    LinearGradientBrush brush = new LinearGradientBrush(rect, this.BackColor, this.ForeColor, LinearGradientMode.Vertical);
                    offscreen.FillRectangle(brush, inset, inset, rect.Width, rect.Height);

                    e.Graphics.DrawImage(offscreenImage, 0, 0);
                }
            }
        }
    }
    public class applicationData
    {
        public const string APPNAME = "Build Your $Tax Application";
        public static Color BACKGROUND_COLOR = Color.FromArgb(53, 156, 70);
        public static Color BUTTON_BUY = Color.FromArgb(46, 125, 209);
        public static Color BUTTON_SELL = Color.FromArgb(209, 92, 46);
    }
    public class Debugger
    {
        public static void DebugMessage(string message,
            [CallerLineNumber] int linenum = 0,
            [CallerFilePath] string fpath = "",
            [CallerMemberName] string mname = "")
        {
#if DEBUG
            MessageBox.Show("In method [" + mname + "], file [" + fpath + "], line [" + linenum.ToString() + "]:\n" + message, applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
        }
    }
    partial class MainForm : Form
    {
        private DateTime currentDate;
        private System.Windows.Forms.Timer gameTimer;
        private Stocks _stocks;
        private int startDate = 0;
        private Random random = new Random(DateTime.UtcNow.Millisecond * 100000 + DateTime.UtcNow.Second * 100 + DateTime.UtcNow.Hour);
        private ColorProgressBar progressBar;
        private PictureBox piggybank;
        private Label titleLabel;

        private double money = 4000;
        public MainForm()
        {
            this.ClientSize = new Size(1300, 1000);
            this.Icon = new Icon("..\\..\\..\\buildyourstax.ico");
            this.Name = applicationData.APPNAME;
            this.Text = applicationData.APPNAME;
            this.BackColor = applicationData.BACKGROUND_COLOR;

            stockData = new List<GroupBox>();
            startDate = random.Next(1985, 2001);
            currentDate = new DateTime(startDate, 1, 1);

            _stocks = new Stocks("..\\..\\..\\data.txt",
                                 "..\\..\\..\\inflation.txt", 4);
            Image img = Image.FromFile("..\\..\\..\\piggybank.png");
            piggybank = new PictureBox()
            {
                Location = new Point(1000, 800),
                Image = img,
                Size = new Size(300, 200),
                Visible = true
            };
            progressBar = new ColorProgressBar() 
            { 
                Visible = true,
                Minimum = 0,
                Maximum = 20*12,
                Step = 1,
                Location = new Point(250, 65),
                Size = new Size(200, 15),
                BackColor = applicationData.BUTTON_BUY
            };
            dateLabel = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(10, 60),
                Text = "Current Date: " + currentDate.ToString("yyyy-MM"),
                Name = "dateLabel",
                Font = new Font("Ariel", 16)
            };
            moneyLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Ariel", 20),
                Text = "Pocket Cash: " + money.ToString(),
                Name = "moneyLabel",
                Location = new Point(500, 60)
            };
            titleLabel = new Label
            {
                Font = new Font("Ariel", 30),
                Text = "Build Your $Tax!",
                Location = new Point(this.ClientSize.Width / 2 - 250, 5),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(500, 50)
            };
            DisplayStockBoxes();
            this.Controls.Add(titleLabel);
            this.Controls.Add(progressBar);
            this.Controls.Add(dateLabel);
            this.Controls.Add(moneyLabel);
            this.Controls.Add(piggybank);
            InitilizeBank();
            InitilizeCOD();

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 2500;
            gameTimer.Tick += OnGameTick;
            gameTimer.Start();

            fastTimer = new System.Windows.Forms.Timer();
            fastTimer.Interval = 5;
            fastTimer.Tick += FastGameTick;
            fastTimer.Start();
        }
        private void FastGameTick(object? sender, EventArgs e)
        {
            SuspendLayout();
            redrawBank();
            dateLabel.Text = $"Current Date: {currentDate:yyyy-MM}";
            moneyLabel.Text = "Pocket Cash: " + Math.Round(money,2).ToString();
            ResumeLayout(false);
        }

        private void OnGameTick(object? sender, EventArgs e)
        {
            SuspendLayout();

            currentDate = currentDate.AddMonths(1);
            progressBar.PerformStep();

            if (currentDate.Month == 12 || currentDate.Month == 6)
            {
                money += 4000;
            }
            updateBank();

            // Update stock prices for each group box
            foreach (var entry in stockLabels)
            {
                GroupBox groupBox = entry.Key;
                Label priceLabel = entry.Value;
                Stock stock = _stocks.getStocks().Find(s => s.CompanyName == groupBox.Text);

                // Update price label text based on the current date
                priceLabel.Text = $"Price: {stock.prices[currentDate]:C}";

            }
            foreach (var entry in stockCharts)
            {
                GroupBox groupBox = entry.Key;
                Chart chart = entry.Value;
                groupBox.Controls.Remove(chart);
                Stock stock = _stocks.getStocks().Find(s => s.CompanyName == groupBox.Text);

                chart.Series.Clear();
                chart = stock.GetChart(currentDate);
                chart.Location = new Point(10, 60);
                chart.Size = new Size(280, 170);
                groupBox.Controls.Add(chart);
                stockCharts[groupBox] = chart; // Update the reference in the dictionary

                chart.Invalidate(); // Force the chart to redraw itself with the new data
            }

            if (currentDate.Year >= startDate + 20)
            {
                gameTimer.Stop();
                MessageBox.Show("The game has ended after 20 years!");
                DisableUI();
            }

            ResumeLayout(false);
        }
    }
}