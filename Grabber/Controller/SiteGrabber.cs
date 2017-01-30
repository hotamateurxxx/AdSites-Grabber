using System.Collections.Generic;
using AdSitesGrabber.Model;


namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Абстрастный граббер сайта.
    /// </summary>
    abstract class SiteGrabber : PageGrabber
    {

        /// <summary>
        /// Параметры запуска граббера.
        /// </summary>
        public class ExecuteParams {

            /// <summary>
            /// Количество загружаемых объявлений.
            /// </summary>
            public int Count;

        }

        /// <summary>
        /// Отправной адрес, с которого начинается работа граббера.
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// Имя места, для которого выбираются объявления.
        /// </summary>
        public virtual string Region { get; set; }

        /// <summary>
        /// Объявления.
        /// </summary>
        protected List<Advert> adverts;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="region">Имя места, для которого выбираются объявления.</param>
        /// <param name="url">Отправной адрес, с которого начинается работа граббера.</param>
        public SiteGrabber(string region, string url)
        {
            adverts = new List<Advert>();
            Region = region;
            Url = url;
        }

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            string advertsStr = "";
            foreach (Advert advert in adverts)
            {
                advertsStr += "\n\n" + advert;
            }
            advertsStr = advertsStr.Replace("\n", "\n\t");

            return Url + "\n" + Region + "\n" + "Объявления:" + advertsStr;
        }

        /// <summary>
        /// Выполнение рабочей последовательности (загрузка стартовой страницы, выбор параметров и захват объявлений).
        /// </summary>
        /// <param name="execParams">Параметры выполнения граббера.</param>
        abstract public void Execute(ExecuteParams execParams);

    }

}
