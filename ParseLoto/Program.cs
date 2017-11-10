using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.PhantomJS;

namespace WebsiteContentParser
{
  

    class Program
    {
        public static void Main(string[] args)

        {
            string st = GetNumberOrders("https://www.stoloto.ru/ruslotto/game?lastdraw");
            Console.WriteLine(st);
        }

       static public string GetNumberOrders(string URL)
        {
            /*
            WebB webControl1;
            webControl1.Source = new Uri(URL);
            while (webControl1.IsLoading)
            {
                Application.DoEvents();
            }
            return webControl1.ExecuteJavascriptWithResult("document.documentElement.outerHTML").ToString();

             */
            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
            service.IgnoreSslErrors = true;
            service.LoadImages = false;
            service.ProxyType = "none";

            IWebDriver driver = new PhantomJSDriver(service);
            driver.Navigate().GoToUrl(URL);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
    //        Console.WriteLine(driver.PageSource);
            doc.LoadHtml(driver.PageSource);
            string result = "";
          
            doc.OptionFixNestedTags = true;
           HtmlNode node = doc.DocumentNode.SelectSingleNode("//*[@class='bingo_ticket ruslotto']");
            result += node.InnerHtml;
            return result;
        }
    }



}