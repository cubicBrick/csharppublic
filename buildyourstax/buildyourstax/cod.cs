namespace buildyourstax
{
    class CODdata
    {
        // Months
        public int length = 3;
        public double APY = 1.00;
        public double amount = 0;
        public GroupBox COD = new();
        public bool inUse = false;
        public Label amountLabel = new Label();
        public ColorProgressBar time = new ColorProgressBar();
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
        private double APY3MON = 1.0148;
        private double APY1YEAR = 1.0232;
        private double APY5YEAR = 1.0319;
        private Label COD3monAPY;
        private Label COD1yrAPY;
        private Label COD5yrAPY;
        private System.Windows.Forms.Timer CODTimer;
        private void InitilizeCOD()
        {
            CODs = new List<CODdata>();
            for (int i = 0; i < CODMAX; i++)
            {
                CODs.Add(new CODdata());
            }
            CODgb = new GroupBox()
            {
                Location = new Point(20, 670),
                Size = new Size(300, 310),
                BackColor = applicationData.BACKGROUND_COLOR,
                Text = "Certificates Of Deposits"
            };
            CODinput = new TextBox()
            {
                Text = "1000",
                Location = new Point(10, 280),
                Size = new Size(280, 30)
            };
            COD3month = new Button()
            {
                Location = new Point(10, 220),
                Size = new Size(80, 30),
                Text = "3 Months",
                BackColor = applicationData.BUTTON_BUY
            };
            COD3month.Click += COD3month_Click;
            COD1year = new Button()
            {
                Location = new Point(100, 220),
                Size = new Size(80, 30),
                Text = "1 Year",
                BackColor = applicationData.BUTTON_BUY
            };
            COD1year.Click += COD1year_Click;
            COD5year = new Button()
            {
                Location = new Point(190, 220),
                Size = new Size(80, 30),
                Text = "5 Years",
                BackColor = applicationData.BUTTON_BUY
            };
            COD5year.Click += COD5year_Click; ;
            COD3monAPY = new Label()
            {
                Location = new Point(10, 255),
                Size = new Size(80, 20),
                Text = "APY: " + Math.Round(((APY3MON - 1) * 100),2).ToString() + "%"
            };
            COD1yrAPY = new Label()
            {
                Location = new Point(100, 255),
                Size = new Size(80, 20),
                Text = "APY: " + Math.Round(((APY1YEAR - 1) * 100),2).ToString() + "%"
            };
            COD5yrAPY = new Label()
            {
                Location = new Point(190, 255),
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

        private void COD5year_Click(object? sender, EventArgs e)
        {
            if (!Double.TryParse(CODinput.Text, out double amount))
            {
                return;
            }
            if (amount > money)
            {
                MessageBox.Show("You don't have enough money!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            for (int i = 0; i < CODMAX; i++)
            {
                if (CODs[i].inUse == false)
                {
                    // Use this one
                    GroupBox newCOD = new GroupBox()
                    {
                        Location = new Point(10, 20 + i * 65),
                        Size = new Size(280, 60),
                        BackColor = applicationData.BACKGROUND_COLOR,
                        Text = "Certificate of Deposit #" + (i + 1).ToString()
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
                    collect.Click += (s, e) => COD_collect(s, e, i);
                    Label amountLabel = new Label()
                    {
                        Location = new Point(60, 35),
                        Size = new Size(150, 20),
                        Text = "Amount: $" + Math.Round(amount, 2).ToString()
                    };
                    newCOD.Controls.Add(collect);
                    newCOD.Controls.Add(time);
                    newCOD.Controls.Add(amountLabel);
                    CODs[i].APY = APY5YEAR;
                    CODs[i].inUse = true;
                    CODs[i].COD = newCOD;
                    CODs[i].amount = amount;
                    CODs[i].length = 60;
                    CODs[i].time = time;
                    CODs[i].amountLabel = amountLabel;
                    CODgb.Controls.Add(newCOD);
                    return;
                }
            }
            MessageBox.Show("Error! All your CD slots are taken!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void COD1year_Click(object? sender, EventArgs e)
        {
            if (!Double.TryParse(CODinput.Text, out double amount))
            {
                return;
            }
            if (amount > money)
            {
                MessageBox.Show("You don't have enough money!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            for (int i = 0; i < CODMAX; i++)
            {
                if (CODs[i].inUse == false)
                {
                    // Use this one
                    GroupBox newCOD = new GroupBox()
                    {
                        Location = new Point(10, 20 + i * 65),
                        Size = new Size(280, 60),
                        BackColor = applicationData.BACKGROUND_COLOR,
                        Text = "Certificate of Deposit #" + (i + 1).ToString()
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
                    collect.Click += (s, e) => COD_collect(s, e, i);
                    Label amountLabel = new Label()
                    {
                        Location = new Point(60, 35),
                        Size = new Size(150, 20),
                        Text = "Amount: $" + Math.Round(amount, 2).ToString()
                    };
                    newCOD.Controls.Add(collect);
                    newCOD.Controls.Add(time);
                    newCOD.Controls.Add(amountLabel);
                    CODs[i].APY = APY1YEAR;
                    CODs[i].inUse = true;
                    CODs[i].COD = newCOD;
                    CODs[i].amount = amount;
                    CODs[i].length = 12;
                    CODs[i].time = time;
                    CODs[i].amountLabel = amountLabel;
                    CODgb.Controls.Add(newCOD);
                    return;
                }
            }
            MessageBox.Show("Error! All your CD slots are taken!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void COD3month_Click(object? sender, EventArgs e)
        {
            if (!Double.TryParse(CODinput.Text, out double amount))
            {
                return;
            }
            if (amount > money)
            {
                MessageBox.Show("You don't have enough money!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            for (int i = 0; i < CODMAX; i++)
            {
                if (CODs[i].inUse == false)
                {
                    // Use this one
                    GroupBox newCOD = new GroupBox()
                    {
                        Location = new Point(10, 20 + i * 65),
                        Size = new Size(280, 60),
                        BackColor = applicationData.BACKGROUND_COLOR,
                        Text = "Certificate of Deposit #" + (i + 1).ToString()
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
                    collect.Click += (s, e) => COD_collect(s, e, i);
                    Label amountLabel = new Label()
                    {
                        Location = new Point(60, 35),
                        Size = new Size(150, 20),
                        Text = "Amount: $" + Math.Round(amount, 2).ToString()
                    };
                    newCOD.Controls.Add(collect);
                    newCOD.Controls.Add(time);
                    newCOD.Controls.Add(amountLabel);
                    CODs[i].APY = APY5YEAR;
                    CODs[i].inUse = true;
                    CODs[i].COD = newCOD;
                    CODs[i].amount = amount;
                    CODs[i].length = 3;
                    CODs[i].time = time;
                    CODs[i].amountLabel = amountLabel;
                    CODgb.Controls.Add(newCOD);
                    return;
                }
            }
            MessageBox.Show("Error! All your CD slots are taken!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void COD_collect(object ?sender, EventArgs e, int pos)
        {
            if (CODs[pos].inUse == false)
            {
                // Oops! Something went wrong
                Debugger.DebugMessage("Collect was clicked with invalid COD");
                return;
            }
            double coff = 1.0;
            if (CODs[pos].time.Value < CODs[pos].time.Maximum)
            {
                if (MessageBox.Show("Careful!\nTaking out a CD early will inccur a 15% fee.\nContinue?", applicationData.APPNAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    coff = 0.85;
                }
                else
                {
                    return;
                }
            }
            CODs[pos].inUse = false;
            var curr = CODs[pos];
            money += curr.amount * coff;
            curr.COD.Dispose();
        }

        private void CODTimer_Tick(object? sender, EventArgs e)
        {
            for (int i = 0; i < CODMAX; i++)
            {
                if (CODs[i].inUse == false)
                {
                    continue;
                }
                CODs[i].amount *= (CODs[i].APY - 1) / 12 + 1;
                CODs[i].time.PerformStep();
                CODs[i].amountLabel.Text = "Amount: $" + Math.Round(CODs[i].amount, 2).ToString();
            }
        }
    }
}