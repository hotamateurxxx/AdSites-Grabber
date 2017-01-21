using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Engine;
using NHibernate.Mapping;

using log4net;
using log4net.Config;

using AdSitesGrabber.Controller;
using AdSitesGrabber.Model;

using CommandLine;
using CommandLine.Text;

namespace AdSitesGrabber
{

    class Program
    {

        class Options {

            [Option("webDriverType", DefaultValue = WebManager.DriverType.PhantomJS, HelpText = "Тип используемого веб-драйвера (Firefox или PhantomJS).")]
            public WebManager.DriverType WebDriverType { get; set; }

            [Option("browserPath", HelpText = "Путь к исполняемому файлу браузера.")]
            public string BrowserPath { get; set; }

            [Option("region", HelpText = "Регион для загрузки объявлений.")]
            public string Region { get; set; }

            [Option("url", HelpText = "Адрес сайта объявлений.")]
            public string Url { get; set; }

            [Option("count", DefaultValue = 10, HelpText = "Количество объявлений для загрузки.")]
            public int Count { get; set; }

        }

        static void Main(string[] args)
        {

            // Парсим входящие аргументы
            var options = CommandLine.Parser.Default.ParseArguments<Options>(args);
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
                        using (Grabber grabber = new AvitoGrabber(options.Value.Region, options.Value.Url))
                        {
                            Grabber.ExecuteParams execParams = new Grabber.ExecuteParams();
                            execParams.Count = options.Value.Count;
                            grabber.Execute(execParams);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Warns.Error("Ошибка выполнения программы.", e);
                Console.WriteLine("Для продолжения нажмите Enter.");
                Console.ReadLine();
            }

        }

    }
}
