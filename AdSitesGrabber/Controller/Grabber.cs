using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

using AdSitesGrabber.Model;

namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Абстрастный граббер сайта.
    /// </summary>
    abstract class Grabber : IDisposable
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
        protected string startUrl;

        /// <summary>
        /// Имя места, для которого выбираются объявления.
        /// </summary>
        protected string locationName;

        /// <summary>
        /// Объявления.
        /// </summary>
        protected List<Advert> adverts;

        /// <summary>
        /// Значение по-умолчанию отправного адреса, с которого начинается работа граббера.
        /// </summary>
        protected virtual string defaultStartUrl
        {
            get
            {
                throw new Exception("Запрошено не определенное свойство.");
            }
        }

        /// <summary>
        /// Значение по-умолчанию имени места, для которого выбираются объявления.
        /// </summary>
        protected virtual string defaultLocationName 
        {
            get 
            {
                return "Санкт-Петербург";
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Grabber()
        {
            adverts = new List<Advert>();
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="locationName">Имя места, для которого выбираются объявления.</param>
        /// <param name="startUrl">Отправной адрес, с которого начинается работа граббера.</param>
        public Grabber(string locationName = null, string startUrl = null)
            : this()
        {
            this.locationName = locationName ?? defaultLocationName;
            this.startUrl = startUrl ?? defaultStartUrl;
        }

        /// <summary>
        /// Освобождение.
        /// </summary>
        public virtual void Dispose()
        {
            // do nothing yet
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

            return startUrl + "\n" + locationName + "\n" + "Объявления:" + advertsStr;
        }

        /// <summary>
        /// Выполнение рабочей последовательности (загрузка стартовой страницы, выбор параметров и захват объявлений).
        /// </summary>
        /// <param name="execParams">Параметры выполнения граббера.</param>
        abstract public void Execute(ExecuteParams execParams);

    }

}
