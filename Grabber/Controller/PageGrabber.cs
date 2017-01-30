using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace AdSitesGrabber.Controller
{

    /// <summary>
    /// Абстрактный граббер страницы.
    /// </summary>
    class PageGrabber : IDisposable
    {

        /// <summary>
        /// Освобождение.
        /// </summary>
        public virtual void Dispose()
        {
            // do nothing yet
        }

        /// <summary>
        /// Ждать элемент.
        /// </summary>
        /// <param name="driver">Веб-драйвер.</param>
        /// <param name="by">Критерий выбора элемента.</param>
        /// <returns>Ожидаемый элемент.</returns>
        protected static IWebElement waitElement(IWebDriver driver, By by)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            return wait.Until(drv => driver.FindElement(by));
        }

        /// <summary>
        /// Ждать элемент.
        /// </summary>
        /// <param name="driver">Веб-драйвер.</param>
        /// <param name="cssSelector">CSS-селектор.</param>
        /// <returns>Ожидаемый элемент.</returns>
        protected static IWebElement waitElement(IWebDriver driver, String cssSelector)
        {
            return waitElement(driver, By.CssSelector(cssSelector));
        }

    }
}
