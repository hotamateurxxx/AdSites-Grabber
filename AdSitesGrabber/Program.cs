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

            [Option("FirefoxBinPath", HelpText = "Путь к исполняемому файлу Mozilla Firefox.")]
            public string FirefoxBinPath { get; set; }

            [Option("Region", HelpText = "Выбираемый регион для загрузки объявлений.")]
            public string Region { get; set; }

        }

        static void Main(string[] args)
        {

            // Парсим входящие аргументы
            var options = CommandLine.Parser.Default.ParseArguments<Options>(args);
            
            // Запускаем конфигурацию log4net напрямую, потому что непонятно что будет отрабатывать раньше:
            // мой вызов LogManager или какая-то из инструкций NHibernate.
            XmlConfigurator.Configure();

            using (WebManager webManager = WebManager.GetInstance())
            {
                //webManager.FirefoxBinPath = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";
                webManager.FactoryDriver = WebManager.DriverType.PhantomJS;
                using (DatabaseManager dbManager = DatabaseManager.GetInstance())
                {
                    using (Grabber grabber = new AvitoGrabber(options.Value.Region, "http://www.avito.ru/"))
                    {
                        Grabber.ExecuteParams execParams = new Grabber.ExecuteParams();
                        execParams.Count = 100;
                        grabber.Execute(execParams);
                    }
                }
            }

        }

    }
}
