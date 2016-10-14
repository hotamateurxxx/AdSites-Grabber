using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using log4net;
using log4net.Config;

using AdSitesGrabber.Controller;

namespace AdSitesGrabber
{
    class Program
    {

        static void Main(string[] args)
        {

            Logger.Events.Info("Program started.");

            Console.WriteLine("Done.");
            Console.ReadLine();

            /*
            using (WebDriverManager manager = new WebDriverManager())
            {
                manager.FirefoxBinPath = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";

                Grabber grabber = new AvitoGrabber("Москва", "http://www.avito.ru/", manager);
                grabber.Execute();

                //Console.WriteLine(grabber);
                Console.WriteLine("Done.");
                Console.ReadLine();
            }
             * */

        }

    }
}
