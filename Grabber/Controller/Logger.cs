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

        /// <summary>
        /// Отдельный лог информационный.
        /// </summary>
        public static readonly ILog Events = LogManager.GetLogger("InfoLogger");

        /// <summary>
        /// Отдельный лог для ошибок.
        /// </summary>
        public static readonly ILog Warns = LogManager.GetLogger("ErrorLogger");
    }

}
