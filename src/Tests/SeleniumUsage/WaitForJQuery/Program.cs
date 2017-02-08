using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace WaitForJQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://www.avito.ru/moskva";
            string browserPath = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";
            FirefoxOptions options = new FirefoxOptions();
            options.BrowserExecutableLocation = browserPath;
            IWebDriver driver = new FirefoxDriver(options);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(url);

            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            //wait.Until(
            //    drv => ((Int64)(drv as IJavaScriptExecutor).ExecuteScript("return jQuery.active;")).Equals(0)
            //);
            //wait.Until(
            //    drv => ((String)(drv as IJavaScriptExecutor).ExecuteScript("return document.readyState;").ToString()).Equals("complete")
            //);

            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            try
            {
                String jsText = "$('body').prop('textContent')";
                jsText = "return " + jsText + ";";
                String textContent = ((String) js.ExecuteScript(jsText)).Trim();
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка:" + "\n" + e + "\n" + "Для продолжения нажмите Enter.");
                Console.ReadLine();
            }

            driver.Quit();
            Thread.Sleep(1000);
            driver.Dispose();

            Console.WriteLine();
            Console.WriteLine("Программа выполнена. Для выхода нажмите Enter.");
            Console.ReadLine();
            Console.WriteLine("Здесь должно все закончиться.");
        }
    }
}
