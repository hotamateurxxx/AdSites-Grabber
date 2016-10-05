using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace AdSitesGrabber
{

    /// <summary>
    /// Абстрастный граббер сайта.
    /// </summary>
    abstract class Grabber
    {

        /// <summary>
        /// Отправной адрес, с которого начинается работа граббера.
        /// </summary>
        protected string startUrl;

        /// <summary>
        /// Имя места, для которого выбираются объявления.
        /// </summary>
        protected string locationName;

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
                return "Ижевск";
            }
        }

        /// <summary>
        /// Драйвер.
        /// </summary>
        protected IWebDriver driver;

        /// <summary>
        /// Объявления.
        /// </summary>
        protected List<Advert> adverts;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="locationName">Имя места, для которого выбираются объявления.</param>
        /// <param name="startUrl">Отправной адрес, с которого начинается работа граббера.</param>
        public Grabber(string locationName = null, string startUrl = null)
        {
            this.locationName = locationName ?? defaultLocationName;
            this.startUrl = startUrl ?? defaultStartUrl;
            adverts = new List<Advert>();
        }

        /// <summary>
        /// Выполнение рабочей последовательности (загрузка стартовой страницы, выбор параметров и захват объявлений).
        /// </summary>
        abstract public void Execute();

    }

}
