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
    class AvitoAdvertOnPage : AvitoAdvertOnList, IAdvertOnPage
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
        public AvitoAdvertOnPage(AvitoAdvertOnList advert, IWebDriver driver)
            : base()
        {
            url = advert.Url;
            updateTime = advert.UpdateTime;
            ParsePage(driver);
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
            ParseBody(driver);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Разбор заголовка.
        /// </summary>
        protected void ParseTitle(IWebDriver driver)
        {
            IWebElement titleElement = driver.FindElement(By.CssSelector("h1.h1[itemprop=name]"));
            title = titleElement.Text;
        }

        /// <summary>
        /// Разбор тела.
        /// </summary>
        protected void ParseBody(IWebDriver driver)
        {
            IWebElement bodyElement = driver.FindElement(By.CssSelector("div.g_92"));
            //IWebElement bodyElement = driver.FindElement(By.CssSelector("h1.h1[itemprop=name]::div"));
            //IWebElement bodyElement = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return $('h1.h1[itemprop=name]').next()");
            ParseCategories(bodyElement);
            ParsePrice(bodyElement);
            ParseLocation(bodyElement);
            ParseText(bodyElement);
            ParseId(bodyElement);
            //ParseUpdateTime(bodyElement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bodyElement"></param>
        protected override void ParseUpdateTime(IWebElement bodyElement)
        {
            throw new Exception("Метод пока не реализован.");
        }

        protected void ParseId(IWebElement bodyElement)
        {
            try
            {
                IWebElement elem = bodyElement.FindElement(By.CssSelector("#item_id"));
                id = Convert.ToUInt64(elem.Text);
            }
            catch (NoSuchElementException)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Разбор места.
        /// </summary>
        protected void ParseLocation(IWebElement bodyElement)
        {
            IWebElement elem = bodyElement.FindElement(By.CssSelector("#map > span[itemprop=name]"));
            location = elem.Text;
        }

        /// <summary>
        /// Разбор текста.
        /// </summary>
        protected void ParseText(IWebElement bodyElement)
        {
            try
            {
                IWebElement elem = bodyElement.FindElement(By.CssSelector("#desc_text > p"));
                text = elem.Text;
                htmlText = elem.ToString();
            }
            catch (NoSuchElementException)
            {
                IWebElement elem = bodyElement.FindElement(By.CssSelector(".description.description-expanded"));
                text = elem.Text;
                htmlText = elem.ToString();
            }

        }

        /// <summary>
        /// Разбор цены.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        protected void ParsePrice(IWebElement bodyElement)
        {
            try
            {
                IWebElement elem = bodyElement.FindElement(By.CssSelector(".description_price .p_i_price span[itemprop=price]"));
                priceStr = elem.Text;
            }
            catch (NoSuchElementException)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Разбор категорий.
        /// </summary>
        /// <param name="bodyElement">Элемент с телом объявления.</param>
        /// <remarks>Метод имеет косяк в том, что Avito сокращает список элементов категорий, заменяя текст ссылки на "...". Пока нормально, но нужно помнить.</remarks>
        protected override void ParseCategories(IWebElement bodyElement)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> links = bodyElement.FindElements(By.CssSelector(".b-catalog-breadcrumbs .breadcrumb-link"));
            // Создаем новую категорию
            AdvertCategory category = new AdvertCategory();
            foreach (IWebElement link in links)
            {
                // Добавляем новый элемент категории
                category.Add(link.Text);
            }
            // Добавляем категорию в список
            categories.Add(category);
        }

        #endregion

    }
}