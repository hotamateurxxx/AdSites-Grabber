using AdSitesGrabber.Controller.Avito;
using AdSitesGrabber.Extensions;
using log4net.Config;
using System;

namespace AdSitesGrabber.Controller
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
            var options = CommandLine.Parser.Default.ParseArguments<CommandLineArguments>(args);
            IWebDriverExtension.WaitTimeout = options.Value.WaitTimeout;

            try
            {
                // Запускаем конфигурацию log4net напрямую, потому что непонятно что будет отрабатывать раньше:
                // мой вызов LogManager или какая-то из инструкций NHibernate.
                XmlConfigurator.Configure();

                using (WebManager webManager = WebManager.GetInstance())
                {

                    // Конфигурация веб-менеджера
                    webManager.FactoryDriver = options.Value.WebDriverType;
                    if (webManager.FactoryDriver == WebManager.DriverType.Firefox)
                    {
                        if (options.Value.BrowserPath != null)
                        {
                            webManager.BrowserPath = options.Value.BrowserPath;
                        }
                    }

                    using (DatabaseManager dbManager = DatabaseManager.GetInstance())
                    {
                        using (AvitoSiteGrabber grabber = new AvitoSiteGrabber(options.Value.Region, options.Value.Url))
                        {
                            SiteGrabber.ExecuteParams execParams = new SiteGrabber.ExecuteParams();
                            execParams.Count = options.Value.Count;
                            grabber.Execute(execParams);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Warns.Error("Ошибка выполнения программы.", e);
            }
            
            Console.WriteLine();
            Console.WriteLine("Программа выполнена. Для выхода нажмите Enter.");
            Console.ReadLine();
            Console.WriteLine("Здесь должно все закончиться.");

        }

    }
}
