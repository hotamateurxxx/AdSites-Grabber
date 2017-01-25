using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace WebDriver_Firefox
{
    class Program
    {
        static void Main(string[] args)
        {

            FirefoxOptions options = new FirefoxOptions();
            options.BrowserExecutableLocation = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";
            IWebDriver driver = new FirefoxDriver(options);
            driver.Navigate().GoToUrl("http://avito.ru/");
            driver.Close();
            driver.Dispose();

            /*
            Console.WriteLine("Для продолжения нажмите Enter.");
            Console.ReadLine();
            Console.WriteLine("Здесь должно все закончиться.");
             * */
        }
    }
}
