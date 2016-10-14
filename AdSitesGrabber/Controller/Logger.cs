using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using log4net.Config;

namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Логгер приложения.
    /// </summary>
    class Logger
    {

        public static readonly ILog Events = LogManager.GetLogger("Events");
        public static readonly ILog Warns = LogManager.GetLogger("Warns");

        public static void InitLogger()
        {
            //BasicConfigurator.Configure();
            //XmlConfigurator.Configure();
        }

    }
}
