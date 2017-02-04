using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;

namespace AdSitesGrabber.Extensions
{
    public static class IWebDriverExtension
    {

        /// <summary>
        /// Таймаут ожидания веб-элемента после загрузки текста страницы в мс.
        /// </summary>
        public static int WaitTimeout { get; set; }

        /// <summary>
        /// Ждать элемент.
        /// </summary>
        /// <param name="driver">Веб-драйвер.</param>
        /// <param name="by">Критерий выбора элемента.</param>
        /// <param name="context">Контекст поиска.</param>
        /// <returns>Ожидаемый элемент.</returns>
        public static IWebElement WaitElement(this IWebDriver driver, By by, ISearchContext context = null)
        {
            context = context ?? driver;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(WaitTimeout));
            return wait.Until(drv => context.FindElement(by));
        }

        /// <summary>
        /// Ждать элемент.
        /// </summary>
        /// <param name="driver">Веб-драйвер.</param>
        /// <param name="cssSelector">CSS-селектор.</param>
        /// <param name="context">Контекст поиска.</param>
        /// <returns>Ожидаемый элемент.</returns>
        public static IWebElement WaitElement(this IWebDriver driver, String cssSelector, ISearchContext context = null)
        {
            return driver.WaitElement(By.CssSelector(cssSelector), context);
        }

        /// <summary>
        /// Ждать окончания загрузки JavaScript и jQuery.
        /// </summary>
        /// <param name="driver">Веб-драйвер.</param>
        public static void WaitForJSandJQueryToLoad(this IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(
                drv => ((Int64) (drv as IJavaScriptExecutor).ExecuteScript("return jQuery.active;")).Equals(0)
            );
            wait.Until(
                drv => ((String) (drv as IJavaScriptExecutor).ExecuteScript("return document.readyState;").ToString()).Equals("complete")
            );
        }

    }
}
