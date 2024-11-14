namespace buildyourstax
{
    partial class MainForm : Form
    {
        private double bankYearlyInterest = 1.005;
        private double moneyinbank = 0;
        private GroupBox bank;
        private Label bankmoneylabel;
        private Label bankAPYLabel;
        private Button bankDepositButton;
        private Button bankWithdrawButton;
        private TextBox bankAmount;
        private Label dollarsign;

        private void InitilizeBank()
        {
            bank = new GroupBox()
            {
                Size = new Size(300, 200),
                Location = new Point(20, 460),
                Text = "Bank Account",
                BackColor = applicationData.BACKGROUND_COLOR
            };
            bankmoneylabel = new Label()
            {
                Text = "$" + Math.Round(moneyinbank,2).ToString(),
                Location = new Point(15, 20),
                Font = new Font("Ariel", 20),
                Size = new Size(160, 100)
            };
            bankAPYLabel = new Label()
            {
                Text = "APY: " + Math.Round(((bankYearlyInterest - 1) * 100),2).ToString() + "%",
                Location = new Point(20, 55),
                Size = new Size(160, 30)
            };
            bankDepositButton = new Button()
            {
                Text = "Deposit",
                Location = new Point(15, 100),
                Size = new Size(80, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = applicationData.BUTTON_BUY
            };
            bankDepositButton.Click += BankDepositButton_Click;
            bankWithdrawButton = new Button()
            {
                Text = "Withdraw",
                Location = new Point(105, 100),
                Size = new Size(80, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = applicationData.BUTTON_SELL
            };
            bankWithdrawButton.Click += BankWithdrawButton_Click;
            bankAmount = new TextBox()
            {
                Location = new Point(35, 130),
                Size = new Size(150, 30),
                TextAlign = HorizontalAlignment.Left
            };
            dollarsign = new Label()
            {
                Location = new Point(15, 129),
                Size = new Size(15, 30),
                Text = "$",
                Font = new Font("Ariel", 16)
            };

            bank.Controls.Add(dollarsign);
            bank.Controls.Add(bankDepositButton);
            bank.Controls.Add(bankWithdrawButton);
            bank.Controls.Add(bankAmount);
            bank.Controls.Add(bankAPYLabel);            
            bank.Controls.Add(bankmoneylabel);
            this.Controls.Add(bank);
        }

        private void BankWithdrawButton_Click(object? sender, EventArgs e)
        {
            var canConvert = Int32.TryParse(bankAmount.Text, out var amount);
            if (canConvert)
            {
                if (amount > moneyinbank)
                {
                    MessageBox.Show("You don't have enough money!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    money += amount;
                    moneyinbank -= amount;
                }
            }
        }

        private void BankDepositButton_Click(object? sender, EventArgs e)
        {
            var canConvert = Int32.TryParse(bankAmount.Text, out var amount);
            if (canConvert)
            {
                if(amount > money)
                {
                    MessageBox.Show("You don't have enough money!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    money -= amount;
                    moneyinbank += amount;
                }
            }
        }
        private void redrawBank()
        {
            bankmoneylabel.Text = "$" + Math.Round(moneyinbank, 2).ToString();
        }
        private void updateBank()
        {
            moneyinbank *= ((bankYearlyInterest-1)/12) + 1;
        }
    }
}