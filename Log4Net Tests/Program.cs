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

            ILog infoLogger = LogManager.GetLogger("InfoLogger");
            infoLogger.Info("Starting application.");

            ILog errorLogger = LogManager.GetLogger("ErrorLogger");
            errorLogger.Error("error occured", new Exception("some error"));

            Console.WriteLine("Done.");
            Console.ReadLine();

        }
    }
}
