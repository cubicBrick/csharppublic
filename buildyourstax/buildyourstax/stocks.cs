using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace buildyourstax
{
    public partial class MainForm : Form
    {
        private Label dateLabel;
        private Label moneyLabel;
        private List<GroupBox> stockData;
        private Dictionary<GroupBox, Label> stockLabels = new Dictionary<GroupBox, Label>();
        private Dictionary<GroupBox, Chart> stockCharts = new Dictionary<GroupBox, Chart>();
        private Dictionary<Stock, int> amountStock = new Dictionary<Stock, int>();

        private System.Windows.Forms.Timer fastTimer;

        

        private void DisplayStockBoxes()
        {
            int xPosition = 20;
            foreach (var stock in _stocks.getStocks())
            {
                GroupBox groupBox = CreateStockGroupBox(stock, xPosition);
                this.Controls.Add(groupBox);
                stockData.Add(groupBox);
                xPosition += 320;
            }
        }

        private GroupBox CreateStockGroupBox(Stock stock, int xPosition)
        {
            amountStock[stock] = 0;
            GroupBox groupBox = new GroupBox
            {
                Text = stock.CompanyName,
                Size = new Size(300, 350),
                Location = new Point(xPosition, 100),
                BackColor = applicationData.BACKGROUND_COLOR
            };

            Label priceLabel = new Label
            {
                Text = $"Price: {stock.prices[currentDate]:C}",
                Location = new Point(10, 30),
                AutoSize = true
            };
            Label quantityBox = new Label
            {
                Location = new Point(10, 280),
                Text = "You own " + amountStock[stock].ToString() + " shares.",
                Size = new Size(80, 30)
            };
            TextBox amountTextBox = new TextBox
            {
                Location = new Point(10, 300),
                Size = new Size(150, 30),
                Text = "10"
            };
            Label amountIn = new Label
            {
                Text = "Invested: $0",
                Location = new Point(100, 280),
                Size = new Size(180, 30)
            };
            Button buyButton = new Button
            {
                Text = "Buy",
                Location = new Point(10, 250),
                BackColor = applicationData.BUTTON_BUY
            };
            buyButton.Click += (sender, e) => Buy_Click(sender, e, stock, amountTextBox, quantityBox, amountIn);

            Button sellButton = new Button
            {
                Text = "Sell",
                Location = new Point(100, 250),
                BackColor = applicationData.BUTTON_SELL
            };
            sellButton.Click += (sender, e) => Sell_Click(sender, e, stock, amountTextBox, quantityBox, amountIn);
            Chart chart = stock.GetChart(currentDate);
            chart.Location = new Point(10, 60);
            chart.Size = new Size(280, 170);

            groupBox.Controls.Add(amountTextBox);
            groupBox.Controls.Add(buyButton);
            groupBox.Controls.Add(sellButton);
            groupBox.Controls.Add(chart);
            groupBox.Controls.Add(priceLabel);
            groupBox.Controls.Add(quantityBox);
            groupBox.Controls.Add(amountIn);
            stockLabels[groupBox] = priceLabel;
            stockCharts[groupBox] = chart;

            return groupBox;
        }

        private void Buy_Click(object? sender, EventArgs e, Stock stock, TextBox textbox, Label quantityBox, Label amountIn)
        {
            var canConvert = Int32.TryParse(textbox.Text, out int value);
            if (canConvert)
            {
                var cost = value * stock.prices[currentDate];
                if(cost > money)
                {
                    MessageBox.Show("You don't have enough money!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    amountStock[stock] += value;
                    quantityBox.Text = "You own " + amountStock[stock].ToString() + " shares.";
                    amountIn.Text = "Invested: $" + Math.Round((amountStock[stock] * stock.prices[currentDate]), 2).ToString();
                    money -= cost;
                }
            }
        }
        private void Sell_Click(object? sender, EventArgs e, Stock stock, TextBox textbox, Label quantityBox, Label amountIn)
        {
            var canConvert = Int32.TryParse(textbox.Text, out int value);
            if (canConvert)
            {
                var cost = value * stock.prices[currentDate];
                if (value > amountStock[stock])
                {
                    MessageBox.Show("You don't have enough of that stock!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    amountStock[stock] -= value;
                    quantityBox.Text = "You own " + amountStock[stock].ToString() + " shares.";
                    amountIn.Text = "Invested: $" + Math.Round((amountStock[stock] * stock.prices[currentDate]), 2).ToString();
                    money += cost;
                }
            }
        }

        private void DisableUI()
        {
            foreach (Control control in this.Controls)
            {
                control.Enabled = false;
            }
        }
    }
}
