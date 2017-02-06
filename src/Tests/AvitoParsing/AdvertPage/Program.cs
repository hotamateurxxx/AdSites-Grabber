using AdSitesGrabber.Extensions;
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

            // Парсим входящие аргументы
            var options = CommandLine.Parser.Default.ParseArguments<CommandLineArguments>(args);
            IWebDriverExtension.WaitTimeout = options.Value.WaitTimeout;



            Console.WriteLine();
            Console.WriteLine("Программа выполнена. Для выхода нажмите Enter.");
            Console.ReadLine();
            Console.WriteLine("Здесь должно все закончиться.");

        }
    }
}
