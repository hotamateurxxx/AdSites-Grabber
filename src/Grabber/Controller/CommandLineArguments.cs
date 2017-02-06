using CommandLine;

using DriverType = AdSitesGrabber.Controller.WebManager.DriverType;

namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Обрабатываемые аргументы командной строки.
    /// </summary>
    class CommandLineArguments
    {

        /// <summary>
        /// Тип используемого веб-драйвера (Firefox или PhantomJS).
        /// </summary>
        [Option("webDriverType", DefaultValue = DriverType.PhantomJS, HelpText = "Тип используемого веб-драйвера (Firefox или PhantomJS).")]
        public DriverType WebDriverType { get; set; }

        /// <summary>
        /// Путь к исполняемому файлу браузера.
        /// </summary>
        [Option("browserPath", HelpText = "Путь к исполняемому файлу браузера.")]
        public string BrowserPath { get; set; }

        /// <summary>
        /// Регион для загрузки объявлений.
        /// </summary>
        [Option("region", HelpText = "Регион для загрузки объявлений.")]
        public string Region { get; set; }

        /// <summary>
        /// Адрес сайта объявлений.
        /// </summary>
        [Option("url", HelpText = "Адрес сайта объявлений.")]
        public string Url { get; set; }

        /// <summary>
        /// Количество объявлений для загрузки.
        /// </summary>
        [Option("count", DefaultValue = 10, HelpText = "Количество объявлений для загрузки.")]
        public int Count { get; set; }

        /// <summary>
        /// Таймаут ожидания веб-элемента после загрузки текста страницы в мс.
        /// </summary>
        [Option("waitTimeout", DefaultValue = 3000, HelpText = "Таймаут ожидания веб-элемента после загрузки текста страницы в мс.")]
        public int WaitTimeout { get; set; }

    }

}
