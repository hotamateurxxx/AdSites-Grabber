using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AdSitesGrabber
{
    class Program
    {
        
        static void Main(string[] args)
        {

            using (WebDriverManager manager = new WebDriverManager())
            {
                manager.FirefoxBinPath = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";

                Grabber grabber = new AvitoGrabber("Москва", "http://www.avito.ru/", manager);
                grabber.Execute();

                Console.WriteLine(grabber);
                Console.ReadLine();

                manager.Dispose();
            }

        }

    }
}
