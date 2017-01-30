using AdSitesGrabber.Controller.Avito;

namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Фабрика грабберов.
    /// </summary>
    class SiteGrabberFactory
    {

        /// <summary>
        /// Типы грабберов.
        /// </summary>
        public enum GrabberType { Avito }

        /// <summary>
        /// Используемый граббер по-умолчанию.
        /// </summary>
        public static GrabberType DefaultGrabber { get; set; }

        /// <summary>
        /// Создание граббера для Avito.
        /// </summary>
        /// <param name="region">Имя места, для которого выбираются объявления.</param>
        /// <param name="url">Отправной адрес, с которого начинается работа граббера.</param>
        public static SiteGrabber CreateGrabber(string region, string url = null)
        {
            return CreateAvitoGrabber(region, url);
        }

        /// <summary>
        /// Создание граббера для Avito.
        /// </summary>
        /// <param name="region">Имя места, для которого выбираются объявления.</param>
        /// <param name="url">Отправной адрес, с которого начинается работа граббера.</param>
        public static AvitoSiteGrabber CreateAvitoGrabber(string region, string url = null)
        {
            url = url ?? "http://avito.ru/";
            return new AvitoSiteGrabber(region, url);
        }

    }
}
