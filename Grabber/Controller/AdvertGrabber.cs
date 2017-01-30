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
        /// <returns>Ожидаемый элемент.</returns>
        protected IWebElement waitElement(By by)
        {
            return waitElement(Driver, by);
        }

        /// <summary>
        /// Ждать элемент.
        /// </summary>
        /// <param name="cssSelector">CSS-селектор.</param>
        /// <returns>Ожидаемый элемент.</returns>
        protected IWebElement waitElement(String cssSelector)
        {
            return waitElement(Driver, cssSelector);
        }

    }
}
