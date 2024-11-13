using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Shapes;
using System.Windows.Forms.DataVisualization.Charting;

namespace buildyourstax
{
    // Class to represent one stock
    public class Stock
    {
        private Random random = new Random(DateTime.UtcNow.Millisecond * 100000 + DateTime.UtcNow.Second * 100 + DateTime.UtcNow.Hour);
        public string CompanyName { get; private set; }
        public Dictionary<DateTime, double> prices { get; private set; }
        public Chart StockChart { get; private set; } 

        public Stock(string companyName, string fPath)
        {
            CompanyName = companyName;
            prices = new Dictionary<DateTime, double>();
            foreach(var data in File.ReadAllLines(fPath))
            {
                if (data.Split('|')[0] == companyName)
                {
                    prices[new DateTime(Int32.Parse(data.Split('|')[1].Split('-')[0]), Int32.Parse(data.Split('|')[1].Split('-')[1]), 1)] = Math.Max(double.Parse(data.Split("|")[2]) + (random.NextDouble() - 0.5) * 2, 0.2);
                }
            }
            if (prices.Count == 0)
            {
                MessageBox.Show("Error! Could not parse stock!", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

        }
        public List<KeyValuePair<DateTime, double>> GetLastSixMonths()
        {
            DateTime today = DateTime.Now;
            DateTime sixMonthsAgo = today.AddMonths(-6);

            return prices
                .Where(p => p.Key >= sixMonthsAgo)
                .OrderBy(p => p.Key)
                .ToList();
        }
        public Chart GetChart(DateTime givenDate)
        {
            // Calculate the date 6 months ago from the given date
            DateTime sixMonthsAgo = givenDate.AddMonths(-6);

            // Get stock data for the last 6 months from the given date
            var lastSixMonths = prices
                .Where(p => p.Key >= sixMonthsAgo && p.Key <= givenDate)
                .OrderBy(p => p.Key)
                .ToList();
            var chart = new Chart
            {
                Width = 600,
                Height = 400
            };
            var chartArea = new ChartArea
            {
                Name = "StockPriceArea"
            };
            chart.ChartAreas.Add(chartArea);

            chart.Titles.Add(new Title($"Stock Prices"));

            var series = new Series
            {
                Name = "Price",
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = System.Drawing.Color.Blue
            };
            foreach(var datapoint in lastSixMonths)
            {
                series.Points.AddXY(datapoint.Key, datapoint.Value);
            }
            chart.Series.Add(series);

            var legend = new Legend
            {
                Docking = Docking.Top
            };
            chart.Legends.Add(legend);

            return chart;
        }
    }

    public class Stocks
    {
        private List<string> _names;
        private List<Stock> _allStocks;
        private Dictionary<DateTime, double> _cpiData;
        private DateTime _startDate;
        private Random _random;

        private List<int> SelectedStocks;

        public Stocks(string stockFilePath, string cpiFilePath, int amountStock = 3)
        {
            _random = new Random(DateTime.UtcNow.Millisecond * 1000 + DateTime.UtcNow.Second);
            _startDate = new DateTime(_random.Next(1990, 2001), 1, 1);
            _names = new List<string>();
            _cpiData = new Dictionary<DateTime, double>();
            _allStocks = new List<Stock>();
            SelectedStocks = new List<int>();
            foreach (var data in File.ReadAllLines(stockFilePath))
            {
                if (_names.Count == 0 || data.Split('|')[0] != _names.Last())
                {
                    _names.Add(data.Split('|')[0]);
                }
            }
            foreach (var data in File.ReadAllLines(cpiFilePath))
            {
                _cpiData[new DateTime(Int32.Parse(data.Split(',')[0].Split('-')[0]), Int32.Parse(data.Split('|')[0].Split('-')[1]), 1)] = double.Parse(data.Split("|")[1]);
            }
            foreach(var name in _names)
            {
                _allStocks.Add(new Stock(name, stockFilePath));
            }
            if(_names.Count < amountStock)
            {
                MessageBox.Show("Error! Not enough stocks", applicationData.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            while(SelectedStocks.Count < amountStock)
            {
                var choice = _random.Next(_allStocks.Count);
                if (!SelectedStocks.Contains(choice))
                {
                    SelectedStocks.Add(choice);
                }
            }
        }

        public List<Stock> getStocks()
        {
            List<Stock> res = new List<Stock>();
            foreach (var i in SelectedStocks)
            {
                res.Add(_allStocks[i]);
            }
            return res;
        }
    }
}
