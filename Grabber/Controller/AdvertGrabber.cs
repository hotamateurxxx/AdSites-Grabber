using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AdSitesGrabber.Controller
{
    class AdvertGrabber : PageGrabber
    {

        public IWebDriver Driver { get; set; }

        public AdvertGrabber(IWebDriver driver) : 
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
            return waitElement(Driver, by, context);
        }

        /// <summary>
        /// Ждать элемент.
        /// </summary>
        /// <param name="cssSelector">CSS-селектор.</param>
        /// <param name="context">Контекст поиска.</param>
        /// <returns>Ожидаемый элемент.</returns>
        protected IWebElement waitElement(String cssSelector, ISearchContext context = null)
        {
            return waitElement(Driver, cssSelector, context);
        }

    }
}
