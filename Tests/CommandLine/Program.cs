using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdSitesGrabber.Controller;
using AdSitesGrabber.Model;

using CommandLine;
using CommandLine.Text;


namespace AdSitesGrabber
{
    class Program
    {

        class Options
        {
            
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

            Console.WriteLine("Для продолжения нажмите Enter.");
            Console.ReadLine();
        }
    }
}
