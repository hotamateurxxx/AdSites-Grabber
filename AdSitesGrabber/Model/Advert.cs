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

        #region Properties

        /// <summary>
        /// Адрес объявления.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        /// <remarks>По сути, наименование товара.</remarks>
        public string Title { get; set; }

        /// <summary>
        /// Текст.
        /// </summary>
        /// <remarks>Без HTML.</remarks>
        public string Text {get; set; }

        /// <summary>
        /// Текст с HTML.
        /// </summary>
        /// <remarks>Текст с HTML-форматированием.</remarks>
        public string HtmlText { get; set; }

        /// <summary>
        /// Время обновления строкой.
        /// </summary>
        public string UpdateTimeStr { get; set; }

        /// <summary>
        /// Обновлено.
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// Категории.
        /// </summary>
        public List<Category> Categories { get; set; }

        /// <summary>
        /// Место.
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        public Price Price { get; set; }

        /// <summary>
        /// Медиа-содержимое.
        /// </summary>
        public Media Media { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Advert()
        {
            Categories = new List<Category>();
            Location = new Location();
            Price = new Price();
            Media = new Media();
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
            string CategoriesStr = "";
            foreach (Category category in Categories)
            {
                CategoriesStr +=
                (
                    ((CategoriesStr == "") ? "" : "\n") 
                    + category.ToString()
                );
            }

            return
            (
                Url
                + "\n" + Location
                + ((CategoriesStr == "") ? "" : "\n") + CategoriesStr
                + "\n" + Title 
                + "\n" + "Цена: " + Price
                + "\n" + "Обновлено: " + UpdateTime.ToString()
            );
        }

        #endregion

    }

}
