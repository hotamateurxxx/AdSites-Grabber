using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSitesGrabber
{
    class Program
    {
        static void Main(string[] args)
        {
            Grabber grabber = new AvitoGrabber("Москва", "http://www.avito.ru/");
            grabber.Execute();

        }
    }
}
