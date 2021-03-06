﻿using FinanceWebScraper.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceWebScraper.Services
{
    public class Scraper
    {
        private string userid;
        private string password;

        public Scraper(string userid, string password)
        {
            this.userid = userid;
            this.password = password;
        }

        public List<Stock> Scrape()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArguments("--disable-gpu");
            options.AddArguments("disable-popup-blocking");//to disable pop-up blocking

            var chromeDriver = new ChromeDriver("C:\\Users\\anuradha\\source\\repos\\Scraper\\FinanceWebScraper\\FinanceWebScraper",options);

            chromeDriver.Navigate().GoToUrl("https://login.yahoo.com/");
            Console.WriteLine("In Yahoo home page");
            chromeDriver.Manage().Window.Maximize();

            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            chromeDriver.FindElementById("login-username").SendKeys(this.userid);
            chromeDriver.FindElementById("login-signin").Click();

            chromeDriver.FindElementById("login-passwd").SendKeys(this.password);
            chromeDriver.FindElementById("login-signin").Click();

            chromeDriver.Url = "https://finance.yahoo.com/portfolio/p_1/view/v1";
            Console.WriteLine("In yahoo finance page");

            //var closePopup = chromeDriver.FindElementByXPath("//*[@id=\"fin - tradeit\"]/div[2]/div");
            var closePopup = chromeDriver.FindElementByXPath("//dialog[@id = '__dialog']/section/button");
            closePopup.Click();
            //var items = chromeDriver.FindElementsByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[*]/td[*]/span/a").GetAttribute;
            IWebElement list = chromeDriver.FindElementByTagName("tbody");
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> items = list.FindElements(By.TagName("tr"));
            int count = items.Count();
            List<Stock> result = new List<Stock>();
            Console.WriteLine(count);
            //loop to get details of each stock symbol
            for (int i = 1; i <= count; i++)
            {
                Console.WriteLine(i);
                var symbol = chromeDriver.FindElementByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[1]/span/a").GetAttribute("innerText");
                var lastprice = chromeDriver.FindElementByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[2]/span").GetAttribute("innerText");
                var change = chromeDriver.FindElementByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[3]/span").GetAttribute("innerText");
                var percentChange = chromeDriver.FindElementByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[4]/span").GetAttribute("innerText");
                var currency = chromeDriver.FindElementByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[5]").GetAttribute("innerText");
                var avgVolume = chromeDriver.FindElementByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[9]").GetAttribute("innerText");
                var marketCap = chromeDriver.FindElementByXPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[13]/span").GetAttribute("innerText");

                Stock stock = new Stock();
                Console.WriteLine(symbol);
                stock.Symbol = symbol;
                Console.WriteLine(lastprice);
                stock.Price = Decimal.Parse(lastprice);
                Console.WriteLine(change);
                stock.Change = Decimal.Parse(change);
                Console.WriteLine(percentChange);
                stock.PercentChange = Decimal.Parse(percentChange.Trim('%'));
                Console.WriteLine(currency);
                stock.Currency = currency;
                Console.WriteLine(avgVolume);
                stock.AverageVolume = avgVolume;
                Console.WriteLine(marketCap);
                stock.MarketCap = marketCap;


                result.Add(stock);


            }
            return result;
        }
    }
}
