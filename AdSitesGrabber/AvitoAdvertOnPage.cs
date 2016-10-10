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
    /// Объявление на Avito (каким оно видно на странице объявления).
    /// </summary>
    class AvitoAdvertOnPage 
        : AvitoAdvertOnList, IAdvertOnPage
    {

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="element">Элемент.</param>
        public AvitoAdvertOnPage(IWebElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Апгрейд объявления со списка объявлением со страницы.
        /// </summary>
        /// <param name="advert">Объявление на странице списка.</param>
        public AvitoAdvertOnPage(AvitoAdvertOnList advert, IWebDriver driver = null)
            : base()
        {
            url = advert.Url;
            if (driver == null)
            {
                driver = new FirefoxDriver();
                ParsePage(driver);
                driver.Close();
            }
            else
            {
                ParsePage(driver);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Загрузка и разбор страницы с обявлением.
        /// </summary>
        public void ParsePage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(url);
            ParseTitle(driver);
            ParseCategories(driver);
            ParsePrice(driver);
            ParseLocation(driver);
            ParseText(driver);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Разбор заголовка.
        /// </summary>
        protected void ParseTitle(IWebDriver driver)
        {
            IWebElement elem = driver.FindElement(By.CssSelector(".clearfix > .h1[itemprop=name]"));
            title = elem.Text;
        }

        /// <summary>
        /// Разбор цены.
        /// </summary>
        protected void ParsePrice(IWebDriver driver)
        {
            try
            {
                IWebElement elem = driver.FindElement(By.CssSelector(".description_price > span[itemprop=price]"));
                priceStr = elem.Text;
            }
            catch (NoSuchElementException)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Разбор места.
        /// </summary>
        protected void ParseLocation(IWebDriver driver)
        {
            IWebElement elem = driver.FindElement(By.CssSelector("#map > span[itemprop=name]"));
            location = elem.Text;
        }

        /// <summary>
        /// Разбор текста.
        /// </summary>
        protected void ParseText(IWebDriver driver)
        {
            try
            {
                IWebElement elem = driver.FindElement(By.CssSelector("#desc_text > p"));
                text = elem.Text;
                htmlText = elem.ToString();
            }
            catch (NoSuchElementException)
            {
                IWebElement elem = driver.FindElement(By.CssSelector(".description.description-expanded"));
                text = elem.Text;
                htmlText = elem.ToString();
            }

        }

        /// <summary>
        /// Разбор категорий.
        /// </summary>
        protected void ParseCategories(IWebDriver driver)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> links = driver.FindElements(By.CssSelector(".b-catalog-breadcrumbs .breadcrumb-link"));
            categories = new List<List<string>>();
            foreach (IWebElement link in links)
            {
                // Создаем новую категорию
                List<string> newCategory = new List<string>();
                // Новая категория на Avito есть углубление предидущей категории
                if (categories.Count > 0)
                {
                    List<string> lastCategory = categories.Last();
                    foreach (string item in lastCategory)
                    {
                        newCategory.Add(item);
                    }
                }
                // Добавляем новый элемент категории
                string catItem = link.Text;
                newCategory.Add(catItem);
                // Добавляем категорию в список
                categories.Add(newCategory);
            }
        }

        #endregion

    }
}