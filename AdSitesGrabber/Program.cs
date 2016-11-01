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

            [Option("WebDriverType", DefaultValue = WebManager.DriverType.PhantomJS, HelpText = "Тип используемого веб-драйвера (Firefox или PhantomJS).")]
            public WebManager.DriverType WebDriverType { get; set; }

            [Option("FirefoxBinPath", HelpText = "Путь к исполняемому файлу Mozilla Firefox.")]
            public string FirefoxBinPath { get; set; }

            [Option("Region", DefaultValue = "Москва", HelpText = "Регион для загрузки объявлений.")]
            public string Region { get; set; }

            [Option("Url", DefaultValue = "http://avito.ru", HelpText = "Адрес сайта объявлений.")]
            public string Url { get; set; }

            [Option("Count", DefaultValue = 10, HelpText = "Количество объявлений для загрузки.")]
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
                        if (options.Value.FirefoxBinPath != null)
                        {
                            webManager.FirefoxBinPath = options.Value.FirefoxBinPath;
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
            }

        }

    }
}
