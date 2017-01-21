using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSitesGrabber.Controller
{
    /// <summary>
    /// Фабрика грабберов.
    /// </summary>
    class GrabberFactory
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
        public static Grabber CreateGrabber(string region, string url = null)
        {
            return CreateAvitoGrabber(region, url);
        }

        /// <summary>
        /// Создание граббера для Avito.
        /// </summary>
        /// <param name="region">Имя места, для которого выбираются объявления.</param>
        /// <param name="url">Отправной адрес, с которого начинается работа граббера.</param>
        public static AvitoGrabber CreateAvitoGrabber(string region, string url = null)
        {
            url = url ?? "http://avito.ru/";
            return new AvitoGrabber(region, url);
        }

    }
}
