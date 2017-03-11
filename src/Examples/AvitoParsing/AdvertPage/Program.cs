using AdSitesGrabber.Controller;
using AdSitesGrabber.Controller.Avito;
using AdSitesGrabber.Extensions;
using AdSitesGrabber.Model;
using CommandLine;
using OpenQA.Selenium;
using System;

namespace AdvertPage
{

    /// <summary>
    /// Класс программы.
    /// </summary>
    class Program
    {

        /// <summary>
        /// Основной метод программы.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        static void Main(string[] args)
        {

            try
            {

                Parsed<CommandLineArguments> arguments = (Parsed<CommandLineArguments>) Parser.Default.ParseArguments<CommandLineArguments>(args);
                IWebDriverExtension.WaitTimeout = arguments.Value.WaitTimeout;

                using (WebManager webManager = WebManager.GetInstance())
                {

                    webManager.FactoryDriver = AdSitesGrabber.Controller.WebManager.DriverType.Firefox;
                    webManager.BrowserPath = arguments.Value.BrowserPath;

                    IWebDriver driver = webManager.OccupyDriver(webManager);
                    var parser = new AvitoAdvertOnPageParser(driver);
                    Advert advert = parser.Parse(arguments.Value.Url);

                    Console.WriteLine();
                    Console.WriteLine("Со страницы прочитано следущее объявление:" + "\n" + advert);
                    Console.WriteLine("Программа приостановлена. Для продолжения нажмите Enter.");
                    Console.ReadLine();

                    webManager.ReleaseDriver(driver);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine("Ошибка выполнения программы:" + "\n" + e);
                Console.WriteLine("Программа приостановлена. Для продолжения нажмите Enter.");
                Console.ReadLine();

            }
            
            Console.WriteLine("Здесь должно все закончиться.");

        }

    }
}
