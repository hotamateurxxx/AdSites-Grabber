using log4net;

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
