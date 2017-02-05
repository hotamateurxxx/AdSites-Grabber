using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using AdSitesGrabber.Extensions;

namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Парсер объявления.
    /// </summary>
    public class AdvertParser : PageParser
    {

        /// <summary>
        /// Веб-драйвер.
        /// </summary>
        public IWebDriver Driver { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="driver">Вэб-драйвер.</param>
        public AdvertParser(IWebDriver driver) : 
            base()
        {
            Driver = driver;
        }

        /// <summary>
        /// Ждать элемент.
        /// </summary>
        /// <param name="by">Критерий выбора элемента.</param>
        /// <param name="context">Контекст поиска.</param>
        /// <returns>Ожидаемый элемент.</returns>
        protected IWebElement waitElement(By by, ISearchContext context = null)
        {
            return Driver.WaitElement(by, context);
        }

        /// <summary>
        /// Ждать элемент.
        /// </summary>
        /// <param name="cssSelector">CSS-селектор.</param>
        /// <param name="context">Контекст поиска.</param>
        /// <returns>Ожидаемый элемент.</returns>
        protected IWebElement waitElement(String cssSelector, ISearchContext context = null)
        {
            return Driver.WaitElement(cssSelector, context);
        }

        /// <summary>
        /// Ждать элемент и вернуть значение атрибута.
        /// </summary>
        /// <param name="cssSelector">CSS-селектор.</param>
        /// <param name="attrName">Имя атрибута.</param>
        /// <param name="context">Контекст поиска.</param>
        /// <returns>Значение атрибута.</returns>
        protected String waitAttrValue(String cssSelector, String attrName, ISearchContext context = null)
        {
            return Driver.WaitElement(cssSelector, context).GetAttribute(attrName);
        }

    }
}
