using AdSitesGrabber.Controller;
using AdSitesGrabber.Controller.Avito;
using AdSitesGrabber.Extensions;
using AdSitesGrabber.Model;
using log4net.Config;
using OpenQA.Selenium;
using System;
using CommandLine;
using CommandLine.Text;
using CommandLine.Infrastructure;

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

            // Парсим входящие аргументы
            var result = Parser.Default.ParseArguments<CommandLineArguments>(args);
            
            //IWebDriverExtension.WaitTimeout = result.

            try
            {
                // Запускаем конфигурацию log4net напрямую, потому что непонятно что будет отрабатывать раньше:
                // мой вызов LogManager или какая-то из инструкций NHibernate.
                XmlConfigurator.Configure();

                using (WebManager webManager = WebManager.GetInstance())
                {

                    // Конфигурация веб-менеджера
                    webManager.FactoryDriver = AdSitesGrabber.Controller.WebManager.DriverType.Firefox;
                    webManager.BrowserPath = result.Value.BrowserPath;

                    IWebDriver driver = webManager.OccupyDriver(webManager);
                    var parser = new AvitoAdvertOnPageParser(driver);
                    Advert advert;
                    try
                    {
                        Console.WriteLine();
                        advert = parser.Parse(result.Value.Url);
                        Console.WriteLine("Со страницы прочитано следущее объявление:\n" + advert);
                        Console.ReadLine();
                    }
                    catch (Exception e)
                    {
                        Logger.Warns.Error("Ошибка разбора объявления.", e);
                        Console.WriteLine("Программа приостановлена. Для продолжения нажмите Enter.");
                        Console.ReadLine();
                        throw e;
                    }
                    webManager.ReleaseDriver(driver);

                }
            }
            catch (Exception e)
            {
                Logger.Warns.Error("Ошибка выполнения программы.", e);
                        Console.WriteLine("Программа приостановлена. Для продолжения нажмите Enter.");
                Console.ReadLine();
            }
            
            //Console.WriteLine();
            //Console.WriteLine("Программа выполнена. Для выхода нажмите Enter.");
            //Console.ReadLine();
            Console.WriteLine("Здесь должно все закончиться.");

        }

    }
}
