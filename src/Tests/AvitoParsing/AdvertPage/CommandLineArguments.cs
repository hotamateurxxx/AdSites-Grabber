using CommandLine;

namespace AdvertPage
{

    /// <summary>
    /// Обрабатываемые аргументы командной строки.
    /// </summary>
    class CommandLineArguments
    {

        /// <summary>
        /// Адрес страницы объявления на Авито.
        /// </summary>
        [Option("url", Required = true, HelpText = "Адрес страницы объявления на Авито.")]
        public string Url { get; set; }

        /// <summary>
        /// Путь к исполняемому файлу браузера.
        /// </summary>
        [Option("browserPath", Required = true, HelpText = "Путь к исполняемому файлу браузера.")]
        public string BrowserPath { get; set; }

        /// <summary>
        /// Таймаут ожидания веб-элемента после загрузки текста страницы в мс.
        /// </summary>
        [Option("waitTimeout", Default = 3000, HelpText = "Таймаут ожидания веб-элемента после загрузки текста страницы в мс.")]
        public int WaitTimeout { get; set; }

    }
}
