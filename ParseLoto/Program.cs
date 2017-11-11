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

using System.Text.RegularExpressions;

using Ticketns;
namespace WebsiteContentParser
{


    class Program
    {
        public static void Main(string[] args)

        {
            int count = 100;
            for (int i = 0; i < count; i++)
            {


                Ticket[] ticketPack = GetNumberOrders("https://www.stoloto.ru/ruslotto/game?lastdraw");
                if (ticketPack != null)
                    foreach (Ticket tic in ticketPack)
                    {
                        tic.Write();
                    }
            }

        }

        static public Ticket[] GetNumberOrders(string URL)
        {     

            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
            service.IgnoreSslErrors = true;
            service.LoadImages = false;
            service.ProxyType = "none";

            IWebDriver driver = new PhantomJSDriver(service);
            driver.Navigate().GoToUrl(URL);
            
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(driver.PageSource);
            //driver.Close();

            HtmlNodeCollection ticketNodes = doc.DocumentNode.SelectNodes("//*[@class='bingo_ticket ruslotto']");
            HtmlNodeCollection numbersNodes = null;
            doc.OptionFixNestedTags = true;
            int m = 0;
            if (ticketNodes != null)
            {
                foreach (HtmlNode num in ticketNodes)
                    m++;
             }

            if (m > 2)
            {
                Ticket[] ticket = new Ticket[m];
                for (int i = 0; i < m; i++)
                {
                    ticket[i] = new Ticket();
                }

                if (ticketNodes != null)
                {
                    int k = 0;
                    foreach (HtmlNode num in ticketNodes)
                    {
                        ticket[k].amount = 0;
                        numbersNodes = num.SelectNodes(".//tr[@class='numbers']");
                        if (numbersNodes != null)
                            foreach (HtmlNode num2 in numbersNodes)
                            {
                                HtmlDocument temphtml = new HtmlDocument();
                                temphtml.LoadHtml(num2.InnerHtml);
                                foreach (HtmlNode temp in temphtml.DocumentNode.SelectNodes("//td"))
                                {
                                    string str = temp.InnerHtml;

                                    int[] matches = Regex.Matches(str, "\\d+")
                                        .Cast<Match>()
                                        .Select(x => int.Parse(x.Value))
                                        .ToArray();
                                    foreach (int match in matches)
                                    {
                                        ticket[k].number[match] = match;
                                        ticket[k].amount++;
                                    }
                                }

                            }
                        ticket[k].numberticket = Convert.ToInt32(num.SelectSingleNode(".//*[@class='ticket_id']").InnerHtml);
                        k++;
                    }

                    Console.WriteLine(k);
                }
                return ticket;
            }
            return null;
        }

    }
}