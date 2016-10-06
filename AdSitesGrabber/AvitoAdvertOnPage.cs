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
    /// Объявление на Avito.
    /// </summary>
    class AvitoAdvertOnPage : AvitoAdvertOnList, IAdvertOnPage
    {

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="element">Элемент.</param>
        public AvitoAdvertOnPage(IWebElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Выполнение рабочей последовательности.
        /// </summary>
        public void Execute(IWebDriver driver)
        {
            // Если драйвер не был передан
            if (driver == null)
            {
                driver = new FirefoxDriver();
                ParsePage(driver);
                driver.Close();
            }
            else
            {
                // Открытие во вкладках пока не работает - непонятно как переключиться на старую вкладку обратно
                throw new Exception("Работа во вкладке не реализована.");
                /*
                IWebElement body1 = driver.FindElement(By.TagName("body"));
                //string currentWindowHandle = driver.CurrentWindowHandle;
                body1.SendKeys(Keys.Control + 't');
                //string newWindowHandle = driver.WindowHandles[driver.WindowHandles.Count - 1].ToString();
                //driver.SwitchTo().Window(newWindowHandle);
                Parse();
                IWebElement body2 = driver.FindElement(By.TagName("body"));
                body2.SendKeys(Keys.Control + 'w');
                driver.SwitchTo().Frame(body1);
                */
            }
        }

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
            IWebElement elem = driver.FindElement(By.CssSelector(".description_price > span[itemprop=price]"));
            priceStr = elem.Text;
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
            IWebElement elem = driver.FindElement(By.CssSelector("#desc_text > p"));
            text = elem.Text;
            htmlText = elem.ToString();
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

    }
}