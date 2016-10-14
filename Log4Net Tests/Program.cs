using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using log4net.Config;

namespace Log4Net_Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicConfigurator.Configure();

            ILog log = LogManager.GetLogger("InfoLogger");
            log.Info("Starting application.");

            Console.WriteLine("Done.");
            Console.ReadLine();

        }
    }
}
