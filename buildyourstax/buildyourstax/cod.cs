namespace buildyourstax
{
    class CODdata
    {
        // Months
        public int length;
        public double amount;
        public GroupBox COD;
    }
    partial class MainForm: Form
    {
        private GroupBox CODgb;
        private List<CODdata> CODs = new();
        private List<ColorProgressBar> CODsProgressBar = new();
        private List<Button> CODsCollect = new();
        private const int CODMAX = 3;
        private TextBox CODinput;
        private Button COD3month;
        private Button COD1year;
        private Button COD5year;
        private double APY3MON = 1.0532;
        private double APY1YEAR = 1.0732;
        private double APY5YEAR = 1.0789;
        private Label COD3monAPY;
        private Label COD1yrAPY;
        private Label COD5yrAPY;
        private System.Windows.Forms.Timer CODTimer;
        private void InitilizeCOD()
        {
            CODgb = new GroupBox()
            {
                Location = new Point(20, 680),
                Size = new Size(300, 300),
                BackColor = applicationData.BACKGROUND_COLOR,
                Text = "Certificates Of Deposits"
            };
            CODinput = new TextBox()
            {
                Text = "1000",
                Location = new Point(10, 270),
                Size = new Size(280, 30)
            };
            COD3month = new Button()
            {
                Location = new Point(10, 210),
                Size = new Size(80, 30),
                Text = "3 Months",
                BackColor = applicationData.BUTTON_BUY
            };
            COD3month.Click += COD3month_Click;
            COD1year = new Button()
            {
                Location = new Point(100, 210),
                Size = new Size(80, 30),
                Text = "1 Year",
                BackColor = applicationData.BUTTON_BUY
            };
            COD1year.Click += COD1year_Click;
            COD5year = new Button()
            {
                Location = new Point(190, 210),
                Size = new Size(80, 30),
                Text = "5 Years",
                BackColor = applicationData.BUTTON_BUY
            };
            COD5year.Click += COD5year_Click;
            COD3monAPY = new Label()
            {
                Location = new Point(10, 245),
                Size = new Size(80, 20),
                Text = "APY: " + Math.Round(((APY3MON - 1) * 100),2).ToString() + "%"
            };
            COD1yrAPY = new Label()
            {
                Location = new Point(100, 245),
                Size = new Size(80, 20),
                Text = "APY: " + Math.Round(((APY1YEAR - 1) * 100),2).ToString() + "%"
            };
            COD5yrAPY = new Label()
            {
                Location = new Point(190, 245),
                Size = new Size(80, 20),
                Text = "APY: " + Math.Round(((APY5YEAR - 1) * 100), 2).ToString() + "%"
            };
            CODgb.Controls.Add(COD5yrAPY);
            CODgb.Controls.Add(COD3monAPY);
            CODgb.Controls.Add(COD1yrAPY);
            CODgb.Controls.Add(CODinput);
            CODgb.Controls.Add(COD3month);
            CODgb.Controls.Add(COD1year);
            CODgb.Controls.Add(COD5year);
            this.Controls.Add(CODgb);

            CODTimer = new System.Windows.Forms.Timer();
            CODTimer.Interval = 2500;
            CODTimer.Tick += CODTimer_Tick;
            CODTimer.Start();
        }

        private void CODTimer_Tick(object? sender, EventArgs e)
        {
            for (int i = 0; i < CODsProgressBar.Count; i++)
            {
                var currCOD = CODsProgressBar[i];
                currCOD.PerformStep();
            }
        }

        private void COD5year_Click(object? sender, EventArgs e)
        {
            if (CODs.Count > CODMAX - 1)
            {
                MessageBox.Show("You can't make any more Certificates of Deposits!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(!Int32.TryParse(CODinput.Text, out int amount))
            {
                return;
            }
            if(amount > money)
            {
                MessageBox.Show("You don't have enough money!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            money -= amount;
            GroupBox newCOD = new GroupBox()
            {
                Location = new Point(10, 20 + CODs.Count * 65),
                Size = new Size(280, 60),
                BackColor = applicationData.BACKGROUND_COLOR,
                Text = "Certificate of Deposit #" + (CODs.Count + 1).ToString()
            };
            ColorProgressBar time = new ColorProgressBar()
            {
                Location = new Point(5, 15),
                Size = new Size(270, 15),
                BackColor = applicationData.BUTTON_BUY,
                Minimum = 0,
                Maximum = 60,
                Step = 1,
                Name = "time"
            };
            Button collect = new Button()
            {
                Location = new Point(5, 35),
                Size = new Size(50, 20),
                Text = "Collect",
                BackColor = applicationData.BUTTON_SELL
            };
            newCOD.Controls.Add(collect);
            newCOD.Controls.Add(time);
            CODsProgressBar.Add(time);
            CODs.Add(new CODdata {length = 60, amount = amount, COD = newCOD });
            CODgb.Controls.Add(newCOD);
        }
        private void COD1year_Click(object? sender, EventArgs e)
        {
            if (CODs.Count > CODMAX - 1)
            {
                MessageBox.Show("You can't make any more Certificates of Deposits!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Int32.TryParse(CODinput.Text, out int amount))
            {
                return;
            }
            if (amount > money)
            {
                MessageBox.Show("You don't have enough money!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            GroupBox newCOD = new GroupBox()
            {
                Location = new Point(10, 20 + CODs.Count * 65),
                Size = new Size(280, 60),
                BackColor = applicationData.BACKGROUND_COLOR,
                Text = "Certificate of Deposit #" + (CODs.Count + 1).ToString()
            };
            ColorProgressBar time = new ColorProgressBar()
            {
                Location = new Point(5, 15),
                Size = new Size(270, 15),
                BackColor = applicationData.BUTTON_BUY,
                Minimum = 0,
                Maximum = 12,
                Step = 1,
                Name = "time"
            };
            Button collect = new Button()
            {
                Location = new Point(5, 35),
                Size = new Size(50, 20),
                Text = "Collect",
                BackColor = applicationData.BUTTON_SELL
            };
            newCOD.Controls.Add(collect);
            newCOD.Controls.Add(time);
            CODsProgressBar.Add(time);
            CODs.Add(new CODdata { length = 12, amount = amount, COD = newCOD });
            CODgb.Controls.Add(newCOD);
        }

        private void COD3month_Click(object? sender, EventArgs e)
        {
            if(CODs.Count > CODMAX - 1)
            {
                MessageBox.Show("You can't make any more Certificates of Deposits!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Int32.TryParse(CODinput.Text, out int amount))
            {
                return;
            }
            if (amount > money)
            {
                MessageBox.Show("You don't have enough money!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            GroupBox newCOD = new GroupBox()
            {
                Location = new Point(10, 20 + CODs.Count * 65),
                Size = new Size(280, 60),
                BackColor = applicationData.BACKGROUND_COLOR,
                Text = "Certificate of Deposit #" + (CODs.Count + 1).ToString()
            };
            ColorProgressBar time = new ColorProgressBar()
            {
                Location = new Point(5, 15),
                Size = new Size(270, 15),
                BackColor = applicationData.BUTTON_BUY,
                Minimum = 0,
                Maximum = 3,
                Step = 1,
                Name = "time"
            };
            Button collect = new Button()
            {
                Location = new Point(5, 35),
                Size = new Size(50, 20),
                Text = "Collect",
                BackColor = applicationData.BUTTON_SELL
            };
            newCOD.Controls.Add(collect);
            newCOD.Controls.Add(time);
            CODsProgressBar.Add(time);
            CODs.Add(new CODdata { length = 3, amount = amount, COD = newCOD });
            CODgb.Controls.Add(newCOD);
        }
    }
}