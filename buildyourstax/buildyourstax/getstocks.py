import yfinance as yf
from datetime import datetime

def get_monthly_close_data_detail(symbol, start_date, end_date):
    # Fetch monthly stock data (only the closing prices) for a given symbol and date range
    stock = yf.Ticker(symbol)
    data = stock.history(start=start_date, end=end_date, interval="1mo")
    return data[['Close']]  # Select only the Close column

def get_monthly_close_data(data, symbol):
    res = ""
    for index, row in data.iterrows():
        date_str = index.strftime('%Y-%m')  # Format as Year-Month
        close_price = row['Close']
        res += f"{symbol}|{date_str}|{close_price}\n";
    return res

def getAllStocks(symbols, names):
    res = ""
    for i, j in zip(symbols, names):
        res += get_monthly_close_data(get_monthly_close_data_detail(i, "1985-01-01", "2020-01-01"), j)
    return res

if __name__ == "__main__":
    symbols = [];
    names = [];
    with open("names.txt", "r") as f:
        stuff= f.read()
        symbols = [i.split("-")[0] for i in stuff.split("\n")]
        names = [i.split("-")[1] for i in stuff.split("\n")]

    with open("data.txt", "w") as f:
        f.write(getAllStocks(symbols, names))
