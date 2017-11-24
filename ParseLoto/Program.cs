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
using System.IO;
using System.Xml.Serialization;
using Ticketns;
namespace WebsiteContentParser
{


    class Program
    {
        public static void Main(string[] args)

        {
            string URL = "https://www.stoloto.ru/ruslotto/game?draw=1212";
            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
            service.IgnoreSslErrors = true;
            service.LoadImages = false;
            service.ProxyType = "none";

            IWebDriver driver = new PhantomJSDriver(service);
            driver.Navigate().GoToUrl(URL);
           CountTicket countTicket = new CountTicket();
            countTicket.NullAll();
            for (int i = 0; i < 10000; i++)
            {
                Ticket[] ticketPack = GetNumberOrders(driver);
                if (ticketPack != null)
                {  
                    foreach (Ticket tic in ticketPack)
                    {
                        if (countTicket.ExistsTicket(tic)==false)
                        countTicket.AddList(tic);
                    }
                }
                driver.FindElement(By.ClassName("refresh_btn")).Click();
               
            }

            countTicket.CountUp();
           


            XmlSerializer formatter = new XmlSerializer(typeof(CountTicket));

            using (FileStream fs = new FileStream("persons.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, countTicket);

                Console.WriteLine("Объект сериализован");
            }

        }

        static public Ticket[] GetNumberOrders(IWebDriver driver)
        {     

           
            
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(driver.PageSource);
           

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
                                        ticket[k].number[match] = 1;
                                        ticket[k].amount++;
                                    }
                                }

                            }
                        ticket[k].numberticket = Convert.ToInt32(num.SelectSingleNode(".//*[@class='ticket_id']").InnerHtml);
                        k++;
                    }
                }
                return ticket;
            }
            return null;
        }

    }
}