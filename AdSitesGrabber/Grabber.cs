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

        #region Declarations

        /// <summary>
        /// Отправной адрес, с которого начинается работа граббера.
        /// </summary>
        protected string startUrl;

        /// <summary>
        /// Имя места, для которого выбираются объявления.
        /// </summary>
        protected string locationName;

        /// <summary>
        /// Драйвер.
        /// </summary>
        protected IWebDriver driver;

        /// <summary>
        /// Объявления.
        /// </summary>
        protected List<Advert> adverts;

        #endregion

        #region Properties

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

        #endregion

        #region Constructors

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Выполнение рабочей последовательности (загрузка стартовой страницы, выбор параметров и захват объявлений).
        /// </summary>
        abstract public void Execute();

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            string advertsStr = "";
            foreach (Advert advert in adverts)
            {
                advertsStr += "\n" + advert;
            }
            advertsStr.Replace("\n", "\t\n");

            return startUrl + "\n" + locationName + "\n" + "adverts:" + "\n" + advertsStr;
        }

        #endregion

    }

}
