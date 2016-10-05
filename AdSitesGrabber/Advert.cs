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
    /// Объявление.
    /// </summary>
    class Advert
    {

        /// <summary>
        /// Адрес объявления.
        /// </summary>
        protected string url;

        /// <summary>
        /// Драйвер.
        /// </summary>
        protected IWebDriver driver;

        /// <summary>
        /// Заголовок.
        /// </summary>
        /// <remarks>По сути, наименование товара.</remarks>
        protected string title;

        /// <summary>
        /// Цена.
        /// </summary>
        /// <remarks>Пока как строка, потому что заморачиваться с валютами накладно.</remarks>
        protected string price;

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
        /// <remarks>Двойной список используется для того, чтобы можно было использовать и композицию и пересечение элементов.</remarks>
        protected List<List<string>> categories;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Advert()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="url">Адрес объявления.</param>
        /// <param name="driver">Драйвер.</param>
        public Advert(string url, IWebDriver driver = null)
        {
            this.url = url;
            this.driver = driver ?? null;
        }

    }
}
