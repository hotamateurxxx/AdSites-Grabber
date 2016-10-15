using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace AdSitesGrabber.Model
{
    
    #region Interfaces

    /// <summary>
    /// Объявление на элементе веб-страницы.
    /// </summary>
    public interface IAdvertOnElement
    {

        /// <summary>
        /// Разбор элемента с объявлением.
        /// </summary>
        /// <param name="bodyElement">Элемент страницы, содержащий объявление.</param>
        void ParseElement(IWebElement element);

    }

    /// <summary>
    /// Объявление на странице объявления.
    /// </summary>
    public interface IAdvertOnPage : IAdvertOnElement
    {

        /// <summary>
        /// Разбор страницы с объявлением.
        /// </summary>
        /// <param name="driver">Драйвер с загруженной страницей, содержащей объявление.</param>
        void ParsePage(IWebDriver driver);

    }

    /// <summary>
    /// Объявление на странице списка объявлений.
    /// </summary>
    public interface IAdvertOnList : IAdvertOnElement
    {
    }

    #endregion

    /// <summary>
    /// Объявление.
    /// </summary>
    abstract class Advert : IAdvertOnElement
    {

        #region Declarations

        /// <summary>
        /// Адрес объявления.
        /// </summary>
        protected string url;

        /// <summary>
        /// Заголовок.
        /// </summary>
        /// <remarks>По сути, наименование товара.</remarks>
        protected string title;

        /// <summary>
        /// Цена строкой.
        /// </summary>
        /// <remarks>Пока как строка, потом будем заморачиваться с валютами.</remarks>
        protected string priceStr;

        /// <summary>
        /// Наименование валюты в цене.
        /// </summary>
        protected string priceUnit;

        /// <summary>
        /// Количество валюты в цене.
        /// </summary>
        protected decimal priceValue;

        /// <summary>
        /// Время обновления строкой.
        /// </summary>
        protected string updateTimeStr;

        /// <summary>
        /// Время обновления.
        /// </summary>
        protected DateTime updateTime;

        /// <summary>
        /// Заглавная фотография.
        /// </summary>
        protected string titleImgUrl;

        /// <summary>
        /// Количество фотографий.
        /// </summary>
        protected int photosCount;

        /// <summary>
        /// Место.
        /// </summary>
        protected string location;

        /// <summary>
        /// Текст.
        /// </summary>
        /// <remarks>Без HTML.</remarks>
        protected string text;

        /// <summary>
        /// Текст с HTML.
        /// </summary>
        /// <remarks>Текст с HTML-форматированием.</remarks>
        protected string htmlText;

        /// <summary>
        /// Категории.
        /// </summary>
        protected List<Category> categories;

        #endregion

        #region Properties

        /// <summary>
        /// Адрес объявления.
        /// </summary>
        public string Url
        {
            get
            {
                return url;
            }
        }

        /// <summary>
        /// Обновлено.
        /// </summary>
        public DateTime UpdateTime
        {
            get
            {
                return updateTime;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Advert()
        {
            categories = new List<Category>();
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="bodyElement">Элемент.</param>
        public Advert(IWebElement element)
            : this()
        {
            ParseElement(element);
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Разбор элемента с объявлением.
        /// </summary>
        /// <param name="bodyElement">Элемент страницы, содержащий объявление.</param>
        abstract public void ParseElement(IWebElement element);

        #endregion

        #region Public Methods

        /// <summary>
        /// Представление в строке.
        /// </summary>
        /// <returns>Представление в строке.</returns>
        public override string ToString()
        {
            string categoriesStr = "";
            foreach (Category category in categories)
            {
                categoriesStr +=
                (
                    ((categoriesStr == "") ? "" : "\n") 
                    + category.ToString()
                );
            }

            return
            (
                url
                + "\n" + location
                + ((categoriesStr == "") ? "" : "\n") + categoriesStr
                + "\n" + title 
                + "\n" + "Цена: " + ((priceUnit == null) ? priceStr : priceValue.ToString() + " " + priceUnit)
                + "\n" + "Обновлено: " + updateTime.ToString()
            );
        }

        #endregion

    }

}
